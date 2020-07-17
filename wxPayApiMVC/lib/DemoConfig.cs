using System;
using System.Configuration;

namespace WxPayAPI.lib
{
    public class DemoConfig:IConfig
    {
        public DemoConfig()
        {
        }

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        */

        public string GetAppID(){
            return ConfigurationManager.AppSettings["AppID"];
        }
        public string GetAppSecret()
        {
            return ConfigurationManager.AppSettings["AppSecret"];
        }

        public string GetMchID(){
            return ConfigurationManager.AppSettings["MchID"];
        }
        public string GetKey(){
            return ConfigurationManager.AppSettings["MchKey"];
        }
        

        //=======【小程序基本信息设置】=====================================
        /* 小程序信息配置
        * APPID：绑定支付的APPID（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        */
        public string GetAppIDmini()
        {
            return ConfigurationManager.AppSettings["AppID_mini"];
        }
        public string GetAppSecretmini()
        {
            return ConfigurationManager.AppSettings["AppSecret_mini"];
        }
        public string GetAuthorizationCodemini()
        {
            return ConfigurationManager.AppSettings["AuthorizationCode_mini"];
        }
        


        //授权配置
        public string GetScope()
        {
            return ConfigurationManager.AppSettings["Scope"];
        }
        public string GetLang()
        {
            return ConfigurationManager.AppSettings["lang"];
        }
        




        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
         * 1.证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载；
         * 2.建议将证书文件名改为复杂且不容易猜测的文件
         * 3.商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
        */

        public string GetSSlCertPath(){
            return ConfigurationManager.AppSettings["SSlCertPath"];
        }
        public string GetSSlCertPassword(){
            return ConfigurationManager.AppSettings["SSlCertPassword"];
        }



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public string GetNotifyUrl(){
            return ConfigurationManager.AppSettings["NotifyUrl"];
        }

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public string GetNativeNotifyUrl()
        {
            return ConfigurationManager.AppSettings["NativeNotifyUrl"];
        }

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调后同步转发url，用于同步商城接收支付结果
        */
        public string GetMallNotifyUrl()
        {
            return ConfigurationManager.AppSettings["MallNotifyUrl"];
        }
        //=======【支付完成后返回url】===================================== 
        public string GetReturnUrl()
        {
            return ConfigurationManager.AppSettings["ReturnUrl"];
        }
        

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public string GetIp(){
            return ConfigurationManager.AppSettings["MchIp"];
        }


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public string GetProxyUrl(){
            return "";
        }


        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public int GetReportLevel(){
            return int.Parse(ConfigurationManager.AppSettings["ReportLevel"]); 
        }


        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public int GetLogLevel(){
            return int.Parse(ConfigurationManager.AppSettings["LogLevel"]); ;
        }
    }

    public static class wxApiUrl
    {
        #region
        /// <summary>
        /// 1、appid 小程序 appId
        /// 2、secret  小程序 appSecret
        /// 3、js_code wx.login 接口获得临时登录凭证 code
        /// 4、grant_type 授权类型，此处只需填写 authorization_code返回值
        /// </summary>
        /// <returns></returns>
        public static string auth_code2Session()
        {
            return "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type={3}";
        }

        /// <summary>
        /// 1、grant_type	string		是	填写 client_credential
        /// 2、appid string 是   小程序唯一凭证，即 AppID，可在「微信公众平台 - 设置 - 开发设置」页中获得。（需要已经成为开发者，且帐号没有异常状态）
        /// 3、secret string 是   小程序唯一凭证密钥，即 AppSecret，获取方式同 appid
        /// </summary>
        /// <returns></returns>
        public static string token()
        {
            return "https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}";
        }
        

            #endregion

        /// <summary>
        /// 1appid  公众号的唯一标识   
        /// 2、grant_type	填写为refresh_token
        /// 3、refresh_token 填写通过access_token获取到的refresh_token参数
        /// </summary>
        /// <returns></returns>
        public static string refresh_token() {
            return "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type={1}&refresh_token={2}";
        }

        /// <summary>
        /// 1、access_token  公众号的唯一标识   
        /// 2、appid	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// 3、lang 返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语
        /// </summary>
        /// <returns></returns>
        public static string userinfo()
        {
            return "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}";
        }

        /// <summary>
        /// 1、access_token  公众号的唯一标识   
        /// 2、appid	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        /// <returns></returns>
        public static string check_access_token()
        {
            return "https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}";
        }
    }
}
