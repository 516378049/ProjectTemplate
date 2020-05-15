using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace Framework.DistributedLock
{
    public class RedisDistributedLock : BaseRedisDistributedLock
    {
        private RedisLock _lock;
        private IRedisClient _client;
        public RedisDistributedLock(string key)
            : base(key)
        { }

        public override LockResult TryGetDistributedLock(TimeSpan? getlockTimeOut, TimeSpan? taskrunTimeOut)
        {
            if (lockresult == LockResult.Success)
                throw new Exception("检测到当前锁已获取");
            _client = DistributedLockConfig.GetRedisPoolClient();
            //当其获取锁后,redis连接资源会一直占用,直到获取锁的资源释放后,连接才会跳出，可能会导致连接池资源的浪费。
            try
            {
                this._lock = new RedisLock(_client as RedisClient, key, getlockTimeOut);
                lockresult = LockResult.Success;
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
                if (_lock != null)
                    _lock.Dispose();

                if (_client != null)
                    _client.Dispose();
            }
            catch (Exception exp)
            {
                LogManager.DefaultLogger.Error(exp, "redis分布式尝试锁释放严重异常");
            }
        }
    }
}
