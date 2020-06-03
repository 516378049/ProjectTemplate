using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wxPayApiMVC.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        // GET: Common
        public ActionResult Error(string error="")
        {
            ViewBag.errMessage = error;
            return View();
        }
        
    }
}