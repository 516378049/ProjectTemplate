using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Common;
using ServiceStack.Text;

namespace Framework.DistributedLock
{
    public class RedisDistributedLockFromServiceStack : BaseRedisDistributedLock
    {
        public RedisDistributedLockFromServiceStack(string key)
            : base(key)
        { }

        public override LockResult TryGetDistributedLock(TimeSpan? getlockTimeOut, TimeSpan? taskrunTimeOut)
        {
            if (lockresult == LockResult.Success)
                throw new Exception("检测到当前锁已获取");
            try
            {
                using (var redisClient = DistributedLockConfig.GetRedisPoolClient())
                {
                    //内部的函数返回true，这个方法才会结束
                    ExecExtensions.RetryUntilTrue(
                             () =>
                             {
                                 //SETNX 语法规则详见： http://redis.io/commands/setnx

                                 //计算lock的 unix time 过期时间 
                                 //TimeSpan realSpan = taskrunTimeOut ?? TimeSpan.FromMilliseconds(DistributedLockConfig.MaxLockTaskRunTime);
                                 TimeSpan realSpan = taskrunTimeOut.Value;
                                 DateTime expireTime = DateTime.UtcNow.Add(realSpan);
                                 string lockString = (expireTime.ToUnixTimeMs() + 1).ToString();

                                 //SetEntryIfNotExists：尝试 set 锁，如果锁不存在则 set 到 redis 里，并返回true
                                 var nx = redisClient.SetEntryIfNotExists(key, lockString);
                                 if (nx)
                                 {
                                     lockresult = LockResult.Success;
                                     return true;
                                 }

                                 //如果锁已经存在，有可能是正确的获取，也可能是进程异常，没有正确释放锁，
                                 //因此我们要检测 lock 的值（unix time），判断 lock 何时过期，否则会引起死锁

                                 //监视 lock 的 key，如果在事务执行之前这个(或这些) key 被其他命令所改动，那么事务将被打断。
                                 redisClient.Watch(key);
                                 string lockExpireString = redisClient.Get<string>(key);
                                 long lockExpireTime;
                                 //如果 lock 的 value 不能正确转换为过期时间，则 lock 不正确，
                                 //有可能是因为 lock 被其他进程改动了
                                 if (!long.TryParse(lockExpireString, out lockExpireTime))
                                 {
                                     redisClient.UnWatch();
                                     lockresult = LockResult.GetLockTimeOutFailure;
                                     return false;
                                 }

                                 //如果过期时间大于当前时间，则锁还未过期，当前进程不能继续执行
                                 if (lockExpireTime > DateTime.UtcNow.ToUnixTimeMs())
                                 {
                                     redisClient.UnWatch();
                                     lockresult = LockResult.GetLockTimeOutFailure;
                                     return false;
                                 }

                                 //如果过期时间小于当前时间，则锁没有被正常释放，之前的线程可能出现了异常
                                 //在这之前用watch监视了lock的key，如果lock的值没有被改动，则当前事务将正常执行
                                 using (var trans = redisClient.CreateTransaction())
                                 {
                                     trans.QueueCommand(r => r.Set(key, lockString));//lockString在循环的开始会重新计算
                                     var t = trans.Commit();
                                     lockresult = t == false ? LockResult.GetLockTimeOutFailure : LockResult.Success;
                                     return t;
                                 }
                             },
                             getlockTimeOut
                         );

                }
            }
            catch (Exception exp)
            {
                LogManager.DefaultLogger.Error(exp, "redis分布式尝试锁系统级别严重异常");
                lockresult = LockResult.LockSystemExceptionFailure;
            }
            return lockresult;
        }

        public override void Dispose()
        {
            try
            {
                using (var redisClient = DistributedLockConfig.GetRedisPoolClient())
                {
                    redisClient.Remove(key);
                }
            }
            catch (Exception exp)
            {
                LogManager.DefaultLogger.Error(exp, "redis分布式尝试锁释放严重异常");
            }
        }
    }
}
