using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.DistributedLock
{
    public abstract class BaseRedisDistributedLock : IDisposable
    {
        protected string key;
        protected LockResult lockresult = LockResult.LockSystemExceptionFailure;

        protected BaseRedisDistributedLock(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// 尝试获取分布式锁
        /// </summary>
        /// <returns></returns>
        public virtual LockResult TryGetDistributedLock(TimeSpan? getlockTimeOut, TimeSpan? taskrunTimeOut)
        {
            if (lockresult == LockResult.Success)
                throw new Exception("检测到当前锁已获取");
            lockresult = LockResult.LockSystemExceptionFailure;
            return lockresult;
        }

        public virtual void Dispose() { }
    }
}
