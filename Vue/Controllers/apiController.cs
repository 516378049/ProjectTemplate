using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Core.WebAPI;
using EF=Vue.Models;
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
        public JsonResult seller()
        {
            string json= JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string seller =  JsonHelper.ConvertJsonResult(json, "seller");
            seller seller_=  JsonHelper.ToObject<seller>(seller);
           
           


            return Json(new { errno = 0, data= seller_ },JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult goods()
        {
            string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string _goods = JsonHelper.ConvertJsonResult(json, "goods");
            List<goods> goods = JsonHelper.ToObject<List<goods>>(_goods);
            return Json(new { errno = 0, data= goods }, JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult ratings()
        {
            string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string ratings =  JsonHelper.ConvertJsonResult(json, "ratings");
            return Json(new { errno = 0, data=ratings }, JsonRequestBehavior.AllowGet);
        }


        public void syscSeller()
        {
            string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string seller = JsonHelper.ConvertJsonResult(json, "seller");
            seller seller_ = JsonHelper.ToObject<seller>(seller);

            //同步data.json数据到数据库
            EF.sellers ef_seller = EntityHelper.EntityCopy<EF.sellers, seller>(seller_);
            int serllerId = Studio.Sellers.Insert(ef_seller).Id;
            foreach (supports sps in seller_.supports)
            {
                EF.supports ef_supports = EntityHelper.EntityCopy<EF.supports, supports>(sps);
                ef_supports.sellerId = serllerId;
                Studio.Supports.Insert(ef_supports);
            }


            string _goods = JsonHelper.ConvertJsonResult(json, "goods");
            List<goods> goods = JsonHelper.ToObject<List<goods>>(_goods);
            ////同步data.json数据到数据库
            foreach (goods gds in goods)
            {
                EF.goods ef_goods = EntityHelper.EntityCopy<EF.goods, goods>(gds);
                ef_goods.sellerId = serllerId;
                int goodId = Studio.Goods.Insert(ef_goods).Id;

                foreach (foods fds in gds.foods)
                {
                    EF.foods ef_foods = EntityHelper.EntityCopy<EF.foods, foods>(fds);
                    ef_foods.goodId = goodId;
                    int foodId = Studio.Foods.Insert(ef_foods).Id;

                    foreach (ratings rat in fds.ratings)
                    {
                        EF.ratings ef_ratings = EntityHelper.EntityCopy<EF.ratings, ratings>(rat);
                        ef_ratings.foodId = foodId;
                        Studio.Ratings.Insert(ef_ratings);
                    }
                }

            }
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



    //public class ratings
    //{
    //    public string username { get; set; }

    //    public int rateTime { get; set; }

    //    public int deliveryTime { get; set; }

    //    public int score { get; set; }

    //    public int rateType { get; set; }

    //    public string text { get; set; }

    //    public string avatar { get; set; }

    //    public List<string> recommend { get; set; }
    //}









































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