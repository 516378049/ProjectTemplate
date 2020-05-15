using Framework;
using JR.NewTenancy.DataAccess.Common;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template.Controllers
{
    public class HomeController : BaseController
    {
        [UserOptionLog]
        public ActionResult Index()
        {

            LogManager.DefaultLogger.Info("DefaultLogger");
            LogManager.StorageLogger.Info("StorageLogger");

            Utility util = new Utility();
            Dictionary<string, object> dic = new Dictionary<string, object>();


            //for below function , we suggest fllow's scenario
            //1、ExcuteProEntpriseLib ： excute the sp  with returnValue
            util.ExcuteProEntpriseLib("testExcuteProMSsql", dic);
            //2、ExcuteSqlEntpriseLib ： excute the sql  with returnValue
            util.ExcuteSqlEntpriseLib("select top 10 * from BatchId", dic);
            //3、getDataTableEntpriseLib ： excute the sp  with DataTable and  ReturnValue and outputs
            util.getDataTableEntpriseLib("testExcuteProMSsql", dic);
            //4、getListEntpriseLibsql ： excute the sql  with List<> 
            util.getListEntpriseLibsql("select top 10 * from BatchId", dic);
            //5、getListEntpriseLibsql ： excute the sp  with List<> 
            util.getList<SysUserInfo>("testExcuteProMSsql", dic);


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
    }
}