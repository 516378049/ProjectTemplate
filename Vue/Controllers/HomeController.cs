using Bussiness.wx;
using LitJson;
using Runda.B2B.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Core.WebAPI;
using WxPayAPI;
using wxRsult=WxPayAPI.wxRsult;
using wxPayApiMVC.lib;
using EF=Vue.Models;

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
                    Studio.UserInfo.Insert(EFinfo);
                }
                Studio.Save();
                return CreateApiResult(user);
            }
            else
            {
                return CreateApiResult("", "-1", "code不能为空");
            }
            
        }
    }
}