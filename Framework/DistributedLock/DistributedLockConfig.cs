using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace Framework.DistributedLock
{
    public class DistributedLockConfig
    {
        /// <summary>
        /// 分布式锁redis服务器,分隔多台服务器地址
        /// </summary>
        public static string DistributedLockRedisServers = ConfigHelper.DistributedLockRedisServers;

        /// <summary>
        /// 分布式锁服务器最大客户端连接池
        /// </summary>
        public static int DistributedLockRedisServersMaxPoolClient = 5;

        /// <summary>
        /// 获取分布式锁redis服务器列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDistributedLockRedisServerList()
        {
            string[] servers = DistributedLockRedisServers.Trim(',').Split(',');
            return new List<string>(servers);
        }

        /// <summary>
        /// 获取redis连接池客户端
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetRedisPoolClient()
        {
            RedisClientManagerConfig config = new RedisClientManagerConfig();
            config.DefaultDb = ConfigHelper.DistributedLockDefaultDb;
            config.AutoStart = true;
            config.MaxReadPoolSize = DistributedLockConfig.DistributedLockRedisServersMaxPoolClient;
            config.MaxWritePoolSize = DistributedLockConfig.DistributedLockRedisServersMaxPoolClient;
            List<string> servers = GetDistributedLockRedisServerList();
            PooledRedisClientManager pool = new PooledRedisClientManager(servers, servers, config);
            return pool.GetClient();
        }

    }
}
