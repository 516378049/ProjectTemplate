using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThoughtWorks.QRCode.Codec;
using WxPayAPI;
using WxPayAPI.lib;
using WxPayAPI.wxRsult;
using wxPayApiMVC.lib;
using wxPayApiMVC.Models;
//(AuthorizeInfo)Session["authorizeInfo"];
//(UserInfo)Session["userinfo"];
//Session["orderId"].ToString();
//Session["totalFee"].ToString();
namespace wxPayApiMVC.Controllers
{
    public class HomeController : BaseController
    {
        #region  annotation
       
        public ActionResult Index()
        {
            //string testTemp = "20200415916224|1|#wechat_redirect".Split('|')[0];

            ////判断取不存在cacheKey情况
            ////重复insert cache
            ////过期时间
            //object test=CacheHelper.GetCache("testcache");
            //CacheHelper.SetCache("testcache", "1", TimeSpan.FromSeconds(10));
            //test=CacheHelper.GetCache("testcache");
            //CacheHelper.SetCache("testcache", "11", TimeSpan.FromSeconds(60));
            //test = CacheHelper.GetCache("testcache");
            //int a = Session.Timeout;


            //test = CacheHelper.GetCache("testcache");

            //string result =
            //            "{\"openid\":\"oDLo50s4l5A8E0F3OYVqSub13Wdw\",\"nickname\":\"长春\",\"sex\":1,\"language\":\"zh_CN\",\"city\":\"永州\",\"province\":\"湖南\",\"country\":\"中国\",\"headimgurl\":\"http:\\/thirdwx.qlogo.cn\\/mmopen\\/vi_32\\/Q0j4TwGTfTJAGathZp2spKBKKadN9pTM87TzIkR7cKeX3EC9W3znYoqkxoZsrAzwXxiaoF83zbYtT9NUca5nhZw\\/132\",\"privilege\":[]}";


            ////throw new Exception("Session[\"totalFee\"]==null 服务器session失效，请联系管理员重启服务");
            //JsonData temp1 = JsonMapper.ToObject(result);
            //UserInfo userinfo = JsonMapper.ToObject<UserInfo>(result);
            //string test = Newtonsoft.Json.JsonConvert.SerializeObject(userinfo);

            //Log.UserAuthorizeInfo("GetUserInfo", "oauthrize info : " + JsonMapper.ToJson(userinfo));//记录用户授权后的用户信息



            //string t = Request.QueryString["t"];
            //string dataPara = @"
            //        <xml><return_code><![CDATA[SUCCESS]]></return_code>
            //        <return_msg><![CDATA[OK]]></return_msg>
            //        <appid><![CDATA[wxba3211abca2a188c]]></appid>
            //        <mch_id><![CDATA[1544571151]]></mch_id>
            //        <nonce_str><![CDATA[d5P57VWpTmwlfwUl]]></nonce_str>
            //        <sign><![CDATA[44D195C21ACE587C00ECAC78A490EF31B07C0CD1F194FAA3F3402194314A9C0A]]></sign>
            //        <result_code><![CDATA[SUCCESS]]></result_code>
            //        <prepay_id><![CDATA[wx02212824175011b91e9d4e171890324800]]></prepay_id>
            //        <trade_type><![CDATA[JSAPI]]></trade_type>
            //        </xml>
            //        ";
            //WxPayAPI.lib.DemoConfig dConfig = new WxPayAPI.lib.DemoConfig();
            //HttpService.Post(dataPara.ToString(), dConfig.GetMallNotifyUrl(), false, 20);
            ////Response.Redirect("/Home/About?"+ dataPara, false);
            //string openid = "1";

            //string access_token = "11";
            //object openidandaccess_token = Json(new { openid, access_token }).Data;


            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string xxx = jss.Serialize(openidandaccess_token);
            //JsonData jd = JsonMapper.ToObject(xxx);
            //string openid1 = (string)jd["openid"];
            //string access_token1 = (string)jd["access_token"];

            //string aaaa = "{ \"getBrandWCPayRequest\":{ \"appId\":\"wxba3211abca2a188c\",\"nonceStr\":\"1927539821\",\"package\":\"prepay_id=wx02033541113552f4b8a35ff81917270600\",\"paySign\":\"c0a52b6d0f423405adb6e2b154fb975abd7e79eafc7317e4a25aa4f7812f8635\",\"signType\":\"MD5\",\"timeStamp\":\"1585769741\"}}";
            //jd = JsonMapper.ToObject(aaaa);
            //object aacc = jd["getBrandWCPayRequest"];
            //openid1 = JsonMapper.ToJson(aacc);
            //jd = (JsonData)aacc;
            

            //openid1 = (string)jd["appId"];
            return View();
            
        }

        //public ActionResult About()
        //{
        //    //string t = Request.QueryString["t"];
        //    //ViewBag.Message = "Your application description page.";
        //    //System.IO.Stream s = Request.InputStream;
        //    //int count = 0;
        //    //byte[] buffer = new byte[1024];
        //    //StringBuilder builder = new StringBuilder();
        //    //while ((count = s.Read(buffer, 0, 1024)) > 0)
        //    //{
        //    //    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
        //    //}
        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        #endregion

        #region 支付接口统一入口
        /// <summary>
        /// 商城支付接口统一入口
        /// </summary>
        /// <returns></returns>
        [VaildateApiParamas]
        //[HttpGet]
        public ActionResult wxPay()
        {
            //获取商城传入的交易商户单号和金额，并保存到session
            string totalFee = Request.QueryString["totalFee"];
            string orderId = Request.QueryString["orderId"];
            Log.Info(this.GetType().ToString(), "request params:" + "orderId:" + orderId + "totalFee:" + totalFee);

            #region 记录用户访问信息,此记录可能没有授权相关信息，只有支付信息
            //UserInfo info = new UserInfo();
            //if (Session["userinfo"] != null)
            //{
            //    info = JsonConvert.DeserializeObject<UserInfo>(Session["userinfo"].ToString());
            //    Log.UserVisitInfo("wxPay", "VisitInfo[Session]: " + Newtonsoft.Json.JsonConvert.SerializeObject(info)+ " "+"orderId：" + orderId + " totalFee：" + totalFee);//记录用户访问信息
            //}
            //else
            //{
            //    Log.UserVisitInfo("wxPay", "VisitInfo[]: " + "orderId：" + orderId + " totalFee：" + totalFee);//记录用户访问信息
            //}
            #endregion

            #region 判断单号和金额是否正常  已对参数进行过滤处理，此处可注释
            //int _totalFee = 0;
            //StringBuilder error = new StringBuilder("");
            //if (string.IsNullOrEmpty(totalFee) || string.IsNullOrEmpty(orderId))
            //{
            //    error.Append("订单号和费用不能为空");
            //}
            //else if (orderId.Contains("|") && int.TryParse(totalFee,out _totalFee))
            //{
            //    error.Append("订单号不能包含“|”，费用值必须为>=1的整数");
            //}
            //else
            //{
            //    WxPayData data = new WxPayData();
            //    data.SetValue("out_trade_no", orderId);
            //    data = WxPayApi.OrderQuery(data);//查询订单

            //    if (data.GetValue("return_code").ToString() == "FAIL")
            //    {
            //        error.Append(data.GetValue("return_msg").ToString());
            //        //return Redirect(String.Concat("http://", "www.changchunamy.com", "/Common/Error?error=" + error));
            //    }
            //    else if (data.GetValue("result_code").ToString() == "SUCCESS")
            //    {
            //        tradeState.TradeState = data.GetValue("trade_state").ToString();
            //        error.Append("商户订单号:" + orderId);
            //        error.Append(",查询订单[" + orderId + "]已存在[" + tradeState.TradeState + "],请返回商城" +
            //            "重新下单或联系客服处理此订单");
            //    }
            //}
            //if(!string.IsNullOrEmpty(error.ToString()))
            //{
            //    Log.Debug("订单已存在","will redirect to http://www.changchunamy.com/Common/Error");
            //    return Redirect(String.Concat("http://", "www.changchunamy.com", "/Common/Error?error=" + error));
            //}
            #endregion

            #region 根据客户端来源，发起不同支付场景
            
            //获取用户客户端信息
            string UserAgent = Request.UserAgent;
            Log.Info(this.GetType().ToString(), "broswer type:" + UserAgent);
            if (UserAgent.ToUpper().Contains("MOBILE"))
            {
                if (UserAgent.ToUpper().Contains("MICROMESSENGER"))
                {
                    return wxJsPayApi(orderId, totalFee);//JSAPI支付
                }
                else
                {
                    //H5支付
                    PayParams payParams = new PayParams();
                    payParams.OrderId = orderId;
                    payParams.TotalFee = int.Parse(totalFee);
                    payParams.TradeType = ConstDefin.TRADE_TYPE_H5;
                    payParams.SpbillCreateIp = Request.UserHostAddress;
                    return H5Pay(payParams);
                }
                
            }  //调起微信JS支付
            else
            {
                return NativePay(orderId,totalFee);
            }   //网页扫码支付
            #endregion
        }
        #endregion
        #region native二维码支付
        public ActionResult NativePay(object orderId,string totalFee)
        {
            Log.Info(this.GetType().ToString(), "page load");
            NativePay nativePay = new NativePay();

            //生成扫码支付模式一（用户手动输入金额信息进行支付）
            //string url1 = nativePay.GetPrePayUrl("123456789");
            //ViewBag.Image1 =  HttpUtility.UrlEncode(url1);

            //生成扫码支付模式二 (指定产品和金额信息)
            int total_fee = int.Parse(totalFee);
            string productId = "123456789";

            //生成二维码图片的url
            string url2 = nativePay.GetPayUrl(productId, orderId, total_fee);
            ViewBag.Image2 = HttpUtility.UrlEncode(url2);
            ViewBag.orderId = orderId;
            ViewBag.totalFee =totalFee;

            return View("NativePay");
        }
        /// <summary>
        /// 生成native支付二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult getNativePayCode(string url) {
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;
            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(url, Encoding.Default);
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            //输出二维码图片
            //Response.BinaryWrite(ms.GetBuffer());
            //Response.End();
            return File(ms.ToArray(), @"image/jpeg");
        }
        #endregion

        #region JsPayApi 支付
        public ActionResult wxJsPayApi(string orderId="",string totalFee="", string code="", string state="")
        {
           
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                //获取code码，并通过code获取openid和access_token
                Log.Debug(this.GetType().ToString(), "Get code : " + code);
                Log.Debug(this.GetType().ToString(), "Get state : " + state);
                //解码state获取订单和费用
                orderId = state.Split('|')[0];
                totalFee = state.Split('|')[1];
                int _totle_fee = int.Parse(totalFee);
                //调用获取accessToken 和 openId
                JsonResult openidandaccess_token = GetOpenidAndAccessTokenFromCode(code);
                //反序列化获取微信接口返回的json对象中的accessToken 和 openid
                string openid = ConvertJsonResult(openidandaccess_token, "openid");
                string access_token = ConvertJsonResult(openidandaccess_token, "access_token");

                PayParams payParams = new PayParams();
                payParams.OrderId = orderId;
                payParams.TotalFee = _totle_fee;
                payParams.OpenId = openid;
                payParams.TradeType = ConstDefin.TRADE_TYPE_JSAPI;
                //发起微信统一下单，获取jsapi下单参数
                JsonResult UnifiedOrderResult = GetUnifiedOrderResult(payParams);
                //解析微信同一下单后的参数到前台
                ViewBag.getBrandWCPayRequest = ConvertJsonResult(UnifiedOrderResult);
                ViewBag.totalFee = totalFee;
                ViewBag.OrderId = orderId;
            }
            else
            {
                #region 构造网页授权获取code微信重定向回调URL
                string host = ConfigurationManager.AppSettings["webHost"]; //Request.Url.Host;
                string path = "/Home/wxJsPayApi";// Request.Path;
                string redirect_uri = HttpUtility.UrlEncode(host + path);

                //微信授权接口
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.GetConfig().GetAppID());
                data.SetValue("redirect_uri", redirect_uri);
                data.SetValue("response_type", "code");
                data.SetValue("scope", WxPayConfig.GetConfig().GetScope());
                //data.SetValue("state", "STATE" + "#wechat_redirect");
                data.SetValue("state", orderId + "|" + totalFee + "|#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
                Log.Debug(this.GetType().ToString(), "Will Redirect to URL : " + url);
                try
                {
                    //触发微信返回code码         
                    Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                }
                #endregion
            }
            return View("wxJsPayApi");
        }
        /**
        * 
        * 通过code换取网页授权access_token和openid的返回数据，正确时返回的JSON数据包如下：
        * {
        *  "access_token":"ACCESS_TOKEN",
        *  "expires_in":7200,
        *  "refresh_token":"REFRESH_TOKEN",
        *  "openid":"OPENID",
        *  "scope":"SCOPE",
        *  "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
        * }
        * 其中access_token可用于获取共享收货地址
        * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
        */
        public JsonResult GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                #region 构造获取openid及access_token的url
                WxPayData data = new WxPayData();
                data.SetValue("appid", WxPayConfig.GetConfig().GetAppID());
                data.SetValue("secret", WxPayConfig.GetConfig().GetAppSecret());
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();
                //请求url以获取数据
                string result = HttpService.Get(url);
                Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);
                //保存access_token，用于收货地址获取
                JsonData jd = JsonMapper.ToObject(result);
                #endregion

                string access_token = (string)jd["access_token"];
                string openid = (string)jd["openid"];

                //保存用户授权信息 用来获取地址和刷新Token
                AuthorizeInfo authorizeInfo = JsonConvert.DeserializeObject<AuthorizeInfo>(result);
                authorizeInfo.authorizeTime = System.DateTime.Now;
                CacheHelper.SetCache(openid, authorizeInfo, TimeSpan.FromMinutes(20));

                #region 记录用户授权授，如果模式为snsapi_userinfo，则保存用户基本信息，否则保存授权信息
                if (WxPayConfig.GetConfig().GetScope()== "snsapi_userinfo")
                {
                    UserInfo info= wxPageApi.GetUserInfo(access_token, openid, WxPayConfig.GetConfig().GetLang());
                    Log.UserAuthorizeInfo("GetUserInfo", "UserInfo : " + Newtonsoft.Json.JsonConvert.SerializeObject(info));//记录用户授权后的用户信息
                    Log.UserAuthorizeInfo("GetUserInfo", "oauthrizeInfo : " + result);//记录用户授权后的用户信息
                }
                else
                {
                    Log.UserAuthorizeInfo("GetUserInfo", "oauthrizeInfo : " + result);//记录用户授权后的用户信息
                }
                Log.UserAuthorizeInfo("GetUserInfo", "end~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ : " + result);//记录用户授权后的用户信息
                #endregion

                return Json(new { openid, access_token },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }
        /**
        * 调用统一下单，获得下单结果
        * @return 统一下单结果
        * @失败时抛异常WxPayException
        */
        public JsonResult GetUnifiedOrderResult(PayParams payParams)
        {
            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", "ShangHai just do and belive - wxMall");//上海行信信息技术-微商城支付
            data.SetValue("attach", "null");
            data.SetValue("out_trade_no", payParams.OrderId); //WxPayApi.GenerateOutTradeNo());//;//orderId
            data.SetValue("total_fee", payParams.TotalFee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", payParams.TradeType);
            data.SetValue("spbill_create_ip", payParams.SpbillCreateIp);

            if (payParams.TradeType!=ConstDefin.TRADE_TYPE_H5)
            {
                data.SetValue("openid", payParams.OpenId);
            }

            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            if(payParams.TradeType==ConstDefin.TRADE_TYPE_H5)
            {
                Log.Debug("GetUnifiedOrderResult TRADE_TYPE: ", ConstDefin.TRADE_TYPE_H5);
                string mweb_url = result.GetValue("mweb_url").ToString();
                string ReturnUrl =
                    ConfigurationManager.AppSettings["H5PayConfirm"]+ "?OrderId=" + payParams.OrderId+"&TotalFee="+ payParams.TotalFee.ToString();
                
                mweb_url = mweb_url + "&redirect_url=" + Url.Encode(ReturnUrl);
                
                //保存支付参数到缓存
                CacheHelper.SetCache(payParams.OrderId+".mweb_url", mweb_url, TimeSpan.FromMinutes(5));
                return Json(new { mweb_url }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Log.Debug("GetUnifiedOrderResult TRADE_TYPE: ", ConstDefin.TRADE_TYPE_JSAPI);
                JsApiPay jsapi = new JsApiPay();
                SortedDictionary<string, object> m_values = jsapi.GetJsApiParameters(result);
                return Json(new { BrandWCPayRequestParams = m_values }, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion

        #region H5Pay
        /// <summary>
        /// H5支付
        /// </summary>
        /// <param name="payParams"></param>
        /// <returns></returns>
        public ActionResult H5Pay(PayParams payParams)
        {
            JsonResult JR = GetUnifiedOrderResult(payParams);
            ViewBag.wxPayUrlTransfel = ConvertJsonResult(JR, "mweb_url");
            ViewBag.TotalFee = payParams.TotalFee.ToString(); 
            Log.Debug("[H5Pay] will redirect to WX custome: ", ViewBag.wxPayUrlTransfel);
            return View("H5Pay");
        }
        /// <summary>
        /// 确认支付
        /// </summary>
        /// <returns></returns>
        public ActionResult H5PayConfirm()
        {
            if(Request.QueryString["OrderId"]!=null)
            {
                string OrderId = Request.QueryString["OrderId"].ToString();
                string mweb_url = CacheHelper.GetCache(OrderId + ".mweb_url").ToString();

                ViewBag.wxPayUrlTransfel = mweb_url;
                ViewBag.OrderId = OrderId;
                ViewBag.TotalFee = Request.QueryString["TotalFee"].ToString();
                Log.Debug("H5PayConfirm ", mweb_url);
            }
            return View();
        }
        #endregion

        #region  wxPayHepler
        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public void GetwxResultNotifyData()
        {
            ResultNotify resultNotify = new ResultNotify();
            resultNotify.ProcessNotify();
        }

        /// <summary>
        /// 接收从微信扫码支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public void GetwxNativeNotifyData()
        {
            NativeNotify resultNotify = new NativeNotify();
            resultNotify.ProcessNotify();
        }

        
        /// <summary>
        /// 查询订单，Native支付成功验证
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult wxOrderQuery(string orderId)
        {
            string data= OrderQuery.QueryOrder("", orderId);//调用订单查询业务逻辑
            return Json(new { data}, JsonRequestBehavior.AllowGet); 
        }

        #region test
        /// <summary>
        /// 返回给商城后台
        /// </summary>
        public void testRestult()
        {
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            Log.Info(this.GetType().ToString(), "商城接收微信支付结果通知："+ builder.ToString());
        }
        #endregion

        #endregion

        #region miniPro
        /**
        * 
        * 通过小程序端code 换取 openid 和 session_key 等信息 
        * {
        *  openid	string	用户唯一标识
        *  session_key	string	会话密钥
        *  unionid	string	用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明。
        *  errcode	number	错误码
        *  errmsg	string	错误信息 
        * }
        * openid是微信支付jsapi支付接口统一下单时必须的参数
        * 更详细的说明请参考网页授权获取用户基本信息：http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
        * @失败时抛异常WxPayException
        */
        //[HttpGet]
        public JsonResult AuthLoginByWeixin(string code)
        {
            try
            {
                //接收从微信后台POST过来的数据
                System.IO.Stream s = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                Log.Info(this.GetType().ToString(), "Receive data from MiniPro : " + builder.ToString());

                

                string Appid = WxPayConfig.GetConfig().GetAppIDmini();
                string AppSecret = WxPayConfig.GetConfig().GetAppSecretmini();
                string authorizationCode = WxPayConfig.GetConfig().GetAuthorizationCodemini();

                AuthCode2Session userInfo = new AuthCode2Session();
                userInfo = wxPageApi.authCode2Session(Appid, AppSecret, code, authorizationCode);

                //Token token = new Token();
                //token = wxPageApi.token("client_credential", Appid, AppSecret);
                return Json(new { userInfo.openid, userInfo.session_key }, JsonRequestBehavior.AllowGet);
                //return Json(new { userInfo, token }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayException(ex.ToString());
            }
        }

        /**
        * 调用统一下单，获得下单结果(小程序appId和公众号appId不一样，只需改变appId的值)
        * @return 统一下单结果
        * @失败时抛异常WxPayException
        */
        //[VaildateApiParamas("openid")]
        //[HttpGet]
        public JsonResult GetMiniUnifiedOrderResult(string openid="", string orderId="", int totalFee=0)
        {
            Log.Debug("GetMiniUnifiedOrderResult openid", openid);
            Log.Debug("GetMiniUnifiedOrderResult orderId", orderId);
            Log.Debug("GetMiniUnifiedOrderResult total_fee", totalFee.ToString());
            //统一下单
            if(string.IsNullOrEmpty(openid))
            {
                openid = "otSOX5HA_GnStxwRNUtHZ6wRDKwU";
            }
            if(totalFee==0)
            {
                totalFee = 1;
            }
            if(orderId=="")
            {
                orderId = System.DateTime.Now.Millisecond.ToString();
            }
            WxPayData data = new WxPayData();
            data.SetValue("body", "ShangHai just do and belive - miniMall");//上海行信信息技术-微商城支付
            data.SetValue("attach", "null");
            data.SetValue("out_trade_no", orderId); //WxPayApi.GenerateOutTradeNo());//;//orderId
            data.SetValue("total_fee", totalFee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            data.SetValue("appid", WxPayConfig.GetConfig().GetAppIDmini());//小程序账号ID

            
            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }
            JsApiPay jsapi = new JsApiPay();
            SortedDictionary<string, object> m_values = jsapi.GetJsApiParameters(result);
            Log.Debug("GetMiniUnifiedOrderResult retutn to miniPro: ", JsonConvert.SerializeObject(m_values));
            return Json(new { BrandWCPayRequestParams = m_values }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}