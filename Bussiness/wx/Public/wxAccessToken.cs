using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChat.DataAccess.WeiXin;
namespace Bussiness.wx.Public
{
    public class wxAccessToken
    {
        public static WeiXinPublicAccount PublicAccount = new WeiXinPublicAccount(ConfigHelper.AppID,ConfigHelper.AppSecret);

        public static void Init()
        {
            PublicAccount.OnTokenRenew += publicAccount_OnTokenRenew;
        }

        static void publicAccount_OnTokenRenew(string accountId, string accessToken, string jsToken)
        {
            try
            {
                RedisHelper.getRedisServer.StringSet(ConfigHelper.GeneralAccessToken, accessToken);
                RedisHelper.getRedisServer.StringSet(ConfigHelper.JsapiTicket, jsToken);
                Log.ILog4_Info.Info("auto refresh GeneralAccessToken and JsapiTicket:");
                Log.ILog4_Info.Info(ConfigHelper.GeneralAccessToken + "：" + accessToken);
                Log.ILog4_Info.Info(ConfigHelper.JsapiTicket + "：" + jsToken);
            }
            catch (Exception e) {
                Log.ILog4_Error.Error("保存JsapiTicket和accessToken失败", e);
            }
        }
    }
}
