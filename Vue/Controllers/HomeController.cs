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
using Model.EF.EF_ExAttr;
using WxPayAPI.wxRsult;

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
                        if (user.privilege != null && user.privilege.Length > 0)
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
                    token.access_token = EFinfo.access_token;
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
        /// 微信小程序授权登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult wxMiniAuthorize(string code, string userInfo)
        {
            wxRsult.UserInfo user = JsonHelper.ToObject<wxRsult.UserInfo>(userInfo);
            AuthCode2Session auth = WxUtility.GetObj.authCode2Session(code);

            EF.UserInfo EFinfo = Studio.UserInfo.Get(X => X.openid == auth.openid).FirstOrDefault();
            EFinfo= EFinfo == null ? new EF.UserInfo() : EFinfo;

            EFinfo.city = user.city;
            EFinfo.country = user.country;
            EFinfo.nickname = user.nickname;
            EFinfo.province = user.province;

            EFinfo.headimgurl = user.avatarUrl;
            EFinfo.sex = user.gender;
            EFinfo.openid = auth.openid;
            EFinfo.unionid = auth.unionid;
            EFinfo.access_token = auth.session_key;
            if (EFinfo.Id<=0)
            {
                Studio.UserInfo.Insert(EFinfo);
            }
            else
            {
                Studio.UserInfo.Update(EFinfo);
            }

            Log.ILog4_Debug.Debug("准备插入授权记录......");
            EF.Token token = new EF.Token();
            token.access_token = EFinfo.access_token;
            token.expires_in = "7200";//seconds
            token.userId = EFinfo.Id;
            Studio.Token.Insert(token);
            Log.ILog4_Debug.Debug("插入授权记录：" + JsonHelper.ToJson(token));

            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                if (!string.IsNullOrEmpty("code"))
                {
                    return CreateApiResult(EFinfo);
                }
                else
                {
                    return CreateApiResult("", "-1", "code不能为空");
                }
            }
            catch (Exception e)
            {
                Log.ILog4_Error.Error(e.Message, e);
                return CreateApiResult("", "-1", e.Message);
            }
        }

        /// <summary>
        /// sync database from data.json
        /// </summary>
        public void syscSeller()
        {
            if (Studio.Sellers.Get(X => X.DelFlag == 0).FirstOrDefault() != null)
            {
                return;
            }
        #region model entity addtional
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
                sellersEx ef_sellerEx = JsonHelper.ToObject<sellersEx>(seller);
                EF.sellers ef_seller = JsonHelper.ToObject<EF.sellers>(seller); ;
                //同步data.json数据到数据库
                int serllerId = Studio.Sellers.Insert(ef_seller).Id;
                ef_sellerEx.supports.ForEach(X => X.sellerId = serllerId);
                Studio.Supports.Insert(ef_sellerEx.supports);


                string _goods = JsonHelper.ConvertJsonResult(json, "goods");
                List<goodsEx> ef_goodsEx = JsonHelper.ToObject<List<goodsEx>>(_goods);
                ////同步data.json数据到数据库
                foreach (goodsEx ef_goodEx in ef_goodsEx)
                {
                    ef_goodEx.sellerId = serllerId;
                    EF.goods goods = JsonHelper.CopyEntity<EF.goods>(ef_goodEx);
                    int goodId = Studio.Goods.Insert(goods).Id;
                    foreach (foodsEx ef_foodEx in ef_goodEx.foods)
                    {
                        ef_foodEx.goodId = goodId;
                        EF.foods foods=JsonHelper.CopyEntity<EF.foods>(ef_foodEx);
                        int foodId = Studio.Foods.Insert(foods).Id;

                        foreach (EF.ratings rat in ef_foodEx.ratings)
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
            catch (Exception ex)
            {
                Studio.Rollback();
                return;
            }
            Studio.CommitTransaction();
            #endregion
        }
    }
}