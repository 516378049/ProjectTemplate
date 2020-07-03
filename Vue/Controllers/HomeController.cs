using Bussiness.wx;
using LitJson;
using Runda.B2B.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Core.WebAPI;
//using WxPayAPI;
using wxRsult=WxPayAPI.wxRsult;
using wxPayApiMVC.lib;
using EF= Model.EF;
using Framework;

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
        public ActionResult wxAuthorize(string code)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                syscSeller();
                if (!string.IsNullOrEmpty("code"))
                {
                    //wxRsult.UserInfo user = WxUtility.GetObj.GetOpenidAndAccessTokenFromCode(code);
                    wxRsult.UserInfo user = new wxRsult.UserInfo();
                    user.openid = "11111111111";
                    user.accesstoken = "0123456789";
                    user.city = "湖南";
                    user.country = "中国";
                    user.headimgurl = "";
                    user.sex = 1;
                    //保存用户信息
                    EF.UserInfo EFinfo = Studio.UserInfo.Get(X => X.openid == user.openid).FirstOrDefault();
                    
                    if (EFinfo == null)
                    {
                        EFinfo = JsonHelper.ToObject<EF.UserInfo>(JsonHelper.ToJson(user));
                        EFinfo.access_token = user.accesstoken;
                        Studio.UserInfo.Insert(EFinfo);
                    }
                    else
                    {
                        EFinfo.access_token = user.accesstoken;
                        Studio.UserInfo.Update(EFinfo);
                    }

                    EF.Token token = new EF.Token();
                    token.access_token = user.accesstoken;
                    token.expires_in = "7200";//unint is second
                    token.userId = EFinfo.Id;
                    Studio.Token.Insert(token);
                    return CreateApiResult(user);
                }
                else
                {
                    return CreateApiResult("", "-1", "code不能为空");
                }
            }
            catch (Exception e)
            {
                return CreateApiResult("", "-1", e.Message);
            }
           
            
        }

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