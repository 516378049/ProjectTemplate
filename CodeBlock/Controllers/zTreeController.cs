using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeBlock.Controllers
{
    public class zTreeController : Controller
    {
        // GET: zTree
        public ActionResult Index()
        {
            return View("ZTree");
        }
    }
}