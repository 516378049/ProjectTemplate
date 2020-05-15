using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.DistributedLock
{
    public class DistributedLockHelper
    {
        /// <summary>
        /// 分布式尝试锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="action"></param>
        /// <param name="getLockTimeOut">单位：毫秒</param>
        /// <param name="taskRunTimeOut">单位：毫秒</param>
        /// <returns></returns>
        public static LockResult TryLock(string lockKey, Action action, long? getLockTimeOut = null, long? taskRunTimeOut = null)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(lockKey))
                {
                    if (string.IsNullOrEmpty(DistributedLockConfig.DistributedLockRedisServers))
                    {
                        throw new Exception("请在web.config或app.config中配置DistributedLockRedisServers");
                    }

                    lockKey = CacheManager.GetKey("LockKey." + lockKey);

                    using (var @lock = new RedisDistributedLockFromServiceStack(lockKey))
                    {
                        RedisDistributedLockInfo info = new RedisDistributedLockInfo();
                        TimeSpan? _getLockTimeOut = getLockTimeOut != null
                            ? TimeSpan.FromMilliseconds(getLockTimeOut.Value)
                            : info.GetLockTimeOut;
                        TimeSpan? _taskRunTimeOut = taskRunTimeOut != null
                            ? TimeSpan.FromMilliseconds(taskRunTimeOut.Value)
                            : info.TaskRunTimeOut;
                        var islock = @lock.TryGetDistributedLock(_getLockTimeOut, _taskRunTimeOut);
                        if (islock == LockResult.Success)
                            action();
                        return islock;
                    }
                }
                throw new Exception("lockKey 不能为空");
            }
            catch (Exception exp)
            {
                LogManager.DefaultLogger.Error(exp,
                    string.Format("分布式尝试锁错误,key:{0}", string.IsNullOrWhiteSpace(lockKey) ? "" : lockKey));
                throw;
            }
        }

        /// <summary>
        /// 分布式锁基本信息
        /// </summary>
        public abstract class BaseDistributedLockInfo
        {
            /// <summary>
            /// 不断尝试获取锁的超时的时间;null 表示 仅尝试"一次"获取锁,若失败就放弃尝试。
            /// </summary>
            public TimeSpan? GetLockTimeOut { get; set; }

            /// <summary>
            /// 定义任务执行最大超时时间（即任务锁最多保留的时间）,默认情况下任务锁被获取，任务执行完毕立即释放。但是若任务执行期间异常等情况，任务锁未被释放并被一直保留在分布式内存中。
            /// null表示系统最大执行时间(默认为30s);超过这个执行时间后,任务锁将被系统自动释放;一旦被自动回收，即便任务依旧在运行中,其他竞争者依旧可以获取锁并执行任务。
            /// 另外任务锁保留期间,任何对锁的获取都会超时失败。
            /// 一般情况下建议GetLockTimeOut>=TaskRunTimeOut,这样就能保证锁一定能被获取到，无非等待时间较长，具体看业务情况而定。
            /// </summary>
            public TimeSpan? TaskRunTimeOut { get; set; }
        }

        /// <summary>
        /// redis分布式锁基本信息
        /// </summary>
        public class RedisDistributedLockInfo : BaseDistributedLockInfo
        {
            public RedisDistributedLockInfo()
            {
                //获取锁的超时时间，超时时间内未获取锁,则返回 GetLockTimeOutFailure
                GetLockTimeOut = TimeSpan.FromSeconds(ConfigHelper.GetLockTimeOut);  //ConfigHelper.GetLockTimeOut 默认30s
                //定义任务最大的执行时间，超过这个时间,任务产生的锁将被系统自动回收，一旦被自动回收，即便任务依旧在运行中,其他竞争者依旧可以获取锁。
                TaskRunTimeOut = TimeSpan.FromSeconds(ConfigHelper.TaskRunTimeOut);  //ConfigHelper.TaskRunTimeOut 默认30s
            }
        }
    }
}
