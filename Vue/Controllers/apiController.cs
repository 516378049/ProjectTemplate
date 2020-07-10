using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF= Model.EF;
namespace Vue.Controllers
{
    public class apiController : BaseController
    {
        // GET: api
        public ActionResult Index()
        {
            return View();
        }
        // GET: api
        public JsonResult seller(int id)
        {
            syscSeller(id);
            //string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            //string seller = JsonHelper.ConvertJsonResult(json, "seller");
            //seller seller_ = JsonHelper.ToObject<seller>(seller);
            EF.sellers ef_seller = Studio.Sellers.Get(X => X.Id == id && X.DelFlag == 0).FirstOrDefault();
            List<EF.supports> ef_supports = Studio.Supports.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            ef_seller.supports = ef_supports;
            return Json(new { errno = 0, data= ef_seller },JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult goods(int id)
        {
            //string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            //string _goods = JsonHelper.ConvertJsonResult(json, "goods");
            //List<goods> goods = JsonHelper.ToObject<List<goods>>(_goods);
            List<EF.goods> ef_goods = Studio.Goods.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            foreach (EF.goods good in ef_goods)
            {
                List<EF.foods> ef_foods = Studio.Foods.Get(X => X.goodId == good.Id && X.DelFlag == 0).ToList();
                good.foods = ef_foods;
                foreach (EF.foods food in ef_foods)
                {
                    List<EF.ratings> ef_ratings = Studio.Ratings.Get(X => X.foodId == food.Id && X.DelFlag == 0).ToList();
                    food.ratings = ef_ratings;
                }
            }
            return Json(new { errno = 0, data= ef_goods }, JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult ratings(int id)
        {
            List<EF.RatingsSellers> ratings_ = Studio.RatingsSeller.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            return Json(new { errno = 0, data= ratings_ }, JsonRequestBehavior.AllowGet);
        }

        public void syscSeller(int id)
        {
            if (Studio.Sellers.Get(X => X.Id == id && X.DelFlag == 0).FirstOrDefault() != null)
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
        //delete RatingsSeller;
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

            #endregion
        }

    }
    public class Root
    {
        public seller seller { get; set; }

        public List<goods> goods { get; set; }

        public List<ratings> ratings { get; set; }
    }

    public class goods
    {
        public string name { get; set; }

        public int type { get; set; }

        public List<foods> foods { get; set; }
    }

    public class foods
    {
        public string name { get; set; }

        public decimal price { get; set; }

        public string oldPrice { get; set; }

        public string description { get; set; }

        public int sellCount { get; set; }

        public string rating { get; set; }

        public string info { get; set; }

        public List<ratings> ratings { get; set; }

        public string icon { get; set; }

        public string image { get; set; }
    }

    public class ratings
    {
        public string username { get; set; }

        public string rateTime { get; set; }

        public int rateType { get; set; }

        public string text { get; set; }

        public string avatar { get; set; }
    }

    public class seller
    {
        public string name { get; set; }

        public string description { get; set; }

        public int deliveryTime { get; set; }

        public decimal score { get; set; }

        public decimal serviceScore { get; set; }

        public decimal foodScore { get; set; }

        public decimal rankRate { get; set; }

        public decimal minPrice { get; set; }

        public decimal deliveryPrice { get; set; }

        public int ratingCount { get; set; }

        public int sellCount { get; set; }

        public string bulletin { get; set; }

        public List<supports> supports { get; set; }

        public string avatar { get; set; }

        public List<string> pics { get; set; }

        //public List<string> infos { get; set; }
    }

    public class supports
    {
        public int type { get; set; }

        public string description { get; set; }
    }



    public class RatingsSeller
    {
        public string username { get; set; }

        public string rateTime { get; set; }

        public string deliveryTime { get; set; }

        public int score { get; set; }

        public int rateType { get; set; }

        public string text { get; set; }

        public string avatar { get; set; }

        //public List<string> recommend { get; set; }
    }









































    //public class foods
    //{
    //    public string name { get; set; }
    //    public int price { get; set; }
    //    public string oldPrice { get; set; }
    //    public string description { get; set; }
    //    public int sellCount { get; set; }
    //    public int rating { get; set; }
    //    public string info { get; set; }
    //    public ratings ratings { get; set; }
    //    public string icon { get; set; }
    //    public string image { get; set; }

    //}
    //public class ratings {
    //    public string username { get; set; }
    //    public int rateTime { get; set; }
    //    public int rateType { get; set; }
    //    public string text { get; set; }
    //    public string avatar { get; set; }

    //}
    //public class goods
    //{
    //    public string name { get; set; }
    //    public int type { get; set; }
    //    public foods foods { get; set; }
    //}

}