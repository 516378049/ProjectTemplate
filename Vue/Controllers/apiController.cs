using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vue.Controllers
{
    public class apiController : Controller
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
            return Json(new { errno = 0, data=seller },JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult goods()
        {
            string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string _goods = JsonHelper.ConvertJsonResult(json, "goods");
            return Json(new { errno = 0, data= _goods }, JsonRequestBehavior.AllowGet);
        }
        // GET: api
        public JsonResult ratings()
        {
            string json = JsonHelper.ReadJsonFile(Server.MapPath("/WebVue/data.json"));
            string ratings =  JsonHelper.ConvertJsonResult(json, "ratings");
            return Json(new { errno = 0, data=ratings }, JsonRequestBehavior.AllowGet);
        }
    }
    public class foods
    {
        public string name { get; set; }
        public int price { get; set; }
        public string oldPrice { get; set; }
        public string description { get; set; }
        public int sellCount { get; set; }
        public int rating { get; set; }
        public string info { get; set; }
        public ratings ratings { get; set; }
        public string icon { get; set; }
        public string image { get; set; }

    }
    public class ratings {
        public string username { get; set; }
        public int rateTime { get; set; }
        public int rateType { get; set; }
        public string text { get; set; }
        public string avatar { get; set; }

    }
    public class goods
    {
        public string name { get; set; }
        public int type { get; set; }
        public foods foods { get; set; }
    }

}