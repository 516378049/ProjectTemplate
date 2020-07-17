using Bussiness.wx;
using LitJson;
using Runda.B2B.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxRsult = WxPayAPI.wxRsult;
using wxPayApiMVC.lib;
using EF = Model.EF;
using Framework;
using System.Text;
using System.Security.Cryptography;
using WeChat.DataAccess.WeiXin.Public;
using WeChat.Common;

namespace Vue
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// 微信授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult wxAuthorize(string code)
        {
            Log.ILog4_Debug.Debug("用户访问地址：" +Request.RawUrl+ "\nIP："+ Request.UserHostAddress);
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                syscSeller();
                if (!string.IsNullOrEmpty("code"))
                {
                    wxRsult.UserInfo user = new wxRsult.UserInfo();//please ------------annotation (in production )
                    if (Request.UserHostAddress=="127.0.0.1")
                    {
                        user.openid = "oDLo50s4l5A8E0F3OYVqSub13Wdw"; // ------------------------annotation (in production )
                    }
                    else
                    {
                        user = WxUtility.GetObj.GetOpenidAndAccessTokenFromCode(code);//please ------------annotation (in development )
                    }
                    Log.ILog4_Debug.Debug("授权用户信息：" + JsonHelper.ToJson(user));
                    //保存用户信息
                    EF.UserInfo EFinfo = Studio.UserInfo.Get(X => X.openid == user.openid).FirstOrDefault();
                    
                    if (EFinfo == null)
                    {
                        Log.ILog4_Debug.Debug("准备新增授权用户信息......"); 
                        EFinfo = new EF.UserInfo();
                        EFinfo.city = user.city;
                        EFinfo.country = user.country;
                        EFinfo.headimgurl = user.headimgurl;
                        EFinfo.nickname = user.nickname;
                        if (user.privilege.Length > 0)
                        {
                            user.privilege.ToList().ForEach(X => { EFinfo.privilege += X; });
                        }
                        EFinfo.openid = user.openid;
                        EFinfo.province = user.province;
                        EFinfo.sex = user.sex;
                        EFinfo.unionid = user.unionid;
                        EFinfo.access_token = user.accesstoken;
                        Studio.UserInfo.Insert(EFinfo);
                        Log.ILog4_Debug.Debug("新增授权用户信息：" + JsonHelper.ToJson(user));
                    }
                    else
                    {
                        Log.ILog4_Debug.Debug("准备更新授权用户信息......");
                        if (Request.UserHostAddress != "127.0.0.1") { 
                            EFinfo.access_token = user.accesstoken;//please -----------------------------annotation (in development )
                            Studio.UserInfo.Update(EFinfo);//please ------------------------------------------annotation (in development )
                        }
                        Log.ILog4_Debug.Debug("更新授权用户信息：" + JsonHelper.ToJson(user));
                    }
                    Log.ILog4_Debug.Debug("准备插入授权记录......");
                    EF.Token token = new EF.Token();
                    token.access_token = user.accesstoken;
                    token.expires_in = "7200";//seconds
                    token.userId = EFinfo.Id;
                    Studio.Token.Insert(token);
                    Log.ILog4_Debug.Debug("插入授权记录：" + JsonHelper.ToJson(token));
                    return CreateApiResult(EFinfo);
                }
                else
                {
                    return CreateApiResult("", "-1", "code不能为空");
                }
            }
            catch (Exception e)
            {
                Log.ILog4_Error.Error(e.Message,e);
                return CreateApiResult("", "-1", e.Message);
            }


        }

        /// <summary>
        /// 微信支付结果回调
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult wxPayNotify()
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

                SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
                m_values = XmlHelper.FromXml(builder.ToString());

                

                //2015-06-29 错误是没有签名
                if (m_values["return_code"].ToString() != "SUCCESS")
                {
                    
                    return CreateApiResult("success");
                }
            }
            catch (Exception ex)
            {
                Log.ILog4_Error.Error("微信支付回调接口错误",ex);
                return CreateApiResult("Fail");
            }
            return CreateApiResult("success");
        }


        #region Sign签名算法,JSAPI
        [HttpGet]
        public ActionResult JSInit(string currentURL)
        {
            try
            {
                Log.ILog4_Debug.Debug("JSInit currentURL：" + currentURL);
                Log.ILog4_Debug.Debug("JSInit currentURL 转换后：" + currentURL.Split("#".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]); 

                string jsToken = string.Empty;
                string AccountID = string.Empty;
                AccountID = ConfigHelper.AppID;
                jsToken = RedisHelper.getRedisServer.StringGet(ConfigHelper.JsapiTicket);

                var nonceStr = GetRandomUInt().ToString();
                var timestamp = DateTime.Now.Ticks.ToString();
                var source = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsToken, nonceStr, timestamp, currentURL.Split("#".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]);
                JsInitResponse result = new JsInitResponse()
                {
                    appId = AccountID,
                    nonceStr = nonceStr,
                    signature = WXBizMsgCrypt.GenarateSignature(source),
                    timestamp = timestamp
                };
                Log.ILog4_Debug.Debug("获取JS签名参数："+JsonHelper.ToJson(result));
                return CreateApiResult(result);
            }
            catch (Exception e)
            {
                Log.ILog4_Error.Error("JSInit出错啦",e);
                throw;
            }

        }
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <returns></returns>
        public uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }
        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            var csp = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }
        #endregion

        public void syscSeller()
        {
            if (Studio.Sellers.Get(X => X.DelFlag == 0).FirstOrDefault() != null)
            {
                return;
            }
            #region
        //public List<supports> supports { get; set; }
        //public List<foods> foods { get; set; }
        //public List<ratings> ratings { get; set; }
        #endregion
        #region empty database
        //delete foods;
        //delete goods;
        //delete ratings;
        //delete RatingsSellers;
        //delete sellers;
        //delete supports;
        //delete Token;
        //delete UserInfo;
        #endregion
        #region abandon, will use new fundation to instead it 

        //string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
        //string seller = JsonHelper.ConvertJsonResult(json, "seller");
        //seller seller_ = JsonHelper.ToObject<seller>(seller);

        ////同步data.json数据到数据库
        //EF.sellers ef_seller = EntityHelper.EntityCopy<EF.sellers, seller>(seller_);
        //int serllerId = Studio.Sellers.Insert(ef_seller).Id;
        //foreach (supports sps in seller_.supports)
        //{
        //    EF.supports ef_supports = EntityHelper.EntityCopy<EF.supports, supports>(sps);
        //    ef_supports.sellerId = serllerId;
        //    Studio.Supports.Insert(ef_supports);
        //}


        //string _goods = JsonHelper.ConvertJsonResult(json, "goods");
        //List<goods> goods = JsonHelper.ToObject<List<goods>>(_goods);
        //////同步data.json数据到数据库
        //foreach (goods gds in goods)
        //{
        //    EF.goods ef_goods = EntityHelper.EntityCopy<EF.goods, goods>(gds);
        //    ef_goods.sellerId = serllerId;
        //    int goodId = Studio.Goods.Insert(ef_goods).Id;

        //    foreach (foods fds in gds.foods)
        //    {
        //        EF.foods ef_foods = EntityHelper.EntityCopy<EF.foods, foods>(fds);
        //        ef_foods.goodId = goodId;
        //        int foodId = Studio.Foods.Insert(ef_foods).Id;

        //        foreach (ratings rat in fds.ratings)
        //        {
        //            EF.ratings ef_ratings = EntityHelper.EntityCopy<EF.ratings, ratings>(rat);
        //            ef_ratings.foodId = foodId;
        //            Studio.Ratings.Insert(ef_ratings);
        //        }
        //    }
        //}

        ////sysnc ratingsSeller
        //string ratings = JsonHelper.ConvertJsonResult(json, "ratings");
        //List<EF.RatingsSeller> ratings_ = JsonHelper.ToObject<List<EF.RatingsSeller>>(ratings);
        //Studio.RatingsSeller.Insert(ratings_);

        #endregion

        #region sync database from data.json

        Studio.BeginTransaction();
            try
            {
                string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
                string seller = JsonHelper.ConvertJsonResult(json, "seller");
                EF.sellers ef_seller = JsonHelper.ToObject<EF.sellers>(seller);

                //同步data.json数据到数据库
                int serllerId = Studio.Sellers.Insert(ef_seller).Id;
                ef_seller.supports.ForEach(X => X.sellerId = serllerId);
                Studio.Supports.Insert(ef_seller.supports);


                string _goods = JsonHelper.ConvertJsonResult(json, "goods");
                List<EF.goods> ef_goods = JsonHelper.ToObject<List<EF.goods>>(_goods);
                ////同步data.json数据到数据库
                foreach (EF.goods ef_good in ef_goods)
                {
                    ef_good.sellerId = serllerId;
                    int goodId = Studio.Goods.Insert(ef_good).Id;
                    foreach (EF.foods ef_food in ef_good.foods)
                    {
                        ef_food.goodId = goodId;
                        int foodId = Studio.Foods.Insert(ef_food).Id;

                        foreach (EF.ratings rat in ef_food.ratings)
                        {
                            rat.foodId = foodId;
                            Studio.Ratings.Insert(rat);
                        }
                    }
                }

                //sysnc ratingsSeller
                string ratings = JsonHelper.ConvertJsonResult(json, "ratings");
                List<EF.RatingsSellers> ratings_ = JsonHelper.ToObject<List<EF.RatingsSellers>>(ratings);
                ratings_.ForEach(X => X.sellerId = serllerId);
                Studio.RatingsSeller.Insert(ratings_);
            }
            catch (Exception)
            {
                Studio.Rollback();
                return;
            }
            Studio.CommitTransaction();
            #endregion
        }
    }
}