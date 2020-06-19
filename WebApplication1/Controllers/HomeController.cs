using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            Framework.Log.ILog4.Debug("我是Debug");
            Framework.Log.ILog4.Info("我是Info");
            try { string a = null;
                aa();
            }
            catch (Exception e) {
                string err = e.InnerException.Message+","+ e.Message+","+ e.StackTrace;
                Framework.Log.ILog4.Error(err);
            }
            
            return View();
        }
        public void aa() {
            var a = new Framework.LogManager();
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
    }
}