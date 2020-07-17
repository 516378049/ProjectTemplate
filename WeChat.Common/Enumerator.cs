using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Common
{
    public class Enumerator
    {
        /// <summary>
        /// 微型账号类型，企业号，公众号
        /// </summary>
        public enum WeChatAccountType
        {
            CropAccount = 0,
            PublicAccount = 1
        }

        /// <summary>
        /// 用于腾讯平台查询的用户状态
        /// </summary>
        public enum UserQueryStatus
        {
            AllUser = 0,
            Followed = 1,
            Freezed = 2,
            NotFollowed = 4
        }

        /// <summary>
        /// 微信管理平台的用户状态，
        /// </summary>
        public enum UserStatus
        {
            Deleted = 40,
            Init = 10,
            Invited = 30,
            Followed = 1,
            Freezed = 2,
            NotFollowed = 4
        }

        /// <summary>
        /// 用户腾讯平台的邀请类型
        /// </summary>
        public enum WeiXinUserInviteType
        {
            WeiXinInvite = 1,
            EmailInvite = 2
        }

        /// <summary>
        /// 微信文件类型
        /// </summary>
        public class WeiXinMaterialType
        {
            public string image = "image";
            public string voice = "voice";
            public string video = "video";
            public string file = "file";
        }


        /// <summary>
        /// 用户同步时所用状态
        /// </summary>
        public enum WeChatUserSyncStatus
        {
            MarkToDelete = 0,
            Normal = 1,
            MarkToUpdating = 2
        }

        /// <summary>
        /// 标签类型
        /// </summary>
        public enum TagType
        {
            StaticTag = 0,
            DynamicTag = 1
        }

        /// <summary>
        /// 平台应用安装状态
        /// </summary>
        public enum AppInstallStatus
        {
            ContainerInited = 0,
            WeXinAppCreated = 1,
            Installed = 2
        }

        /// <summary>
        /// 平台用户权限
        /// </summary>
        public enum PlatformUserScopeTpye
        {
            RoleCode = 0,
            DataScope_WeChatAccount = 1,
            DataScope_WeChatApp = 2
        }

        public class RoleCodes
        {
            public const string SuperAdmin = "SuperAdmin";
            public const string PlatformAdmin = "PlatformAdmin";
            public const string AccountAdmin = "AccountAdmin";
            public const string AppAdmin = "AppAdmin";
        }

        public enum UserProfileStatus
        {
            [Description("激活")]
            Active = 0,
            [Description("未激活")]
            Deactive = 1
        }

        public enum UserGroupStatus
        {
            [Description("待确认")]
            Pending = 0,
            [Description("已确认")]
            Confirm,
            [Description("已拒绝")]
            Reject,
            [Description("转让中")]
            Delegating,
            [Description("已转让")]
            Delegated,
            [Description("已删除")]
            Removed
        }

        public enum EventListStatus
        {
            [Description("准备中")]
            Preparation = 0,
            [Description("注册中")]
            Registration = 1,
            [Description("会议中")]
            Active = 2,
            [Description("已结束")]
            Close = 3
        }

        public class Language
        {
            public const string enUS = "en-US";
            public const string zhCN = "zh-CN";
            public const string en = "en";
        }

        public class Gender
        {
            public const string Female = "F";
            public const string Male = "M";
        }

        //Content的CategoryKey
        public enum ContentCategory
        {
            [Description("进门扫码")]
            FirstScanQrcodePerDay = 0
        }

        //Content-ChartType-Bar-Pie
        public class ChartType
        {
            public const string BarChart = "BarChart";
            public const string PieChart = "PieChart";
        }
    }
}
