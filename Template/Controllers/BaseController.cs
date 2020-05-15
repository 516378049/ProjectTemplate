using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using Models.Common;
using Framework;
using ThinkDev.FrameWork.Result;
using Newtonsoft.Json;
using System.Text;

namespace Template.Controllers
{
    public class BaseController : Controller
    {

        #region 验证是否是登录状态
        protected bool IsCheckLogin = true;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region 统一异常处理
        protected override void OnException(ExceptionContext filterContext)
        {
            string strContent = "Mai47发生异常!";
            Exception ex = filterContext.Exception;
            // 标记异常已处理
            filterContext.ExceptionHandled = true;
            // 跳转到指定错误页
            filterContext.Result = new RedirectResult(Url.Action("Error", "Common"));
            filterContext.Result = GetJson("-9999", "Mai47发生异常!", strContent);
        }
        #endregion

        #region 通用方法

        /// <summary>
        /// 日期转码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new JsonResult Json(object data)
        {
            return new MyJsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new MyJsonResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding };
        }

        /// <summary>
        /// 记录传入参数的json
        /// </summary>
        /// <param name="data"></param>
        /// <param name="jsonRequest"></param>
        /// <returns></returns>
        public new JsonResult Json(object data, JsonRequestBehavior jsonRequest)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            UserOptionLog.responseParams = JsonConvert.SerializeObject(data, setting);
            UserOptionLog.user = null;//"loginUser";
            return new MyJsonResult { Data = data, JsonRequestBehavior = jsonRequest };
        }
        /// <summary>
        /// 通用方法，得到对象的json字符串
        /// </summary>
        /// <param name="retCode"></param>
        /// <param name="retMsg"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public JsonResult GetJson(string retCode, string retMsg, object message = null)
        {
            var retJson = new
            {
                RetCode = retCode,
                RetMsg = retMsg,
                Message = message
            };
            return Json(retJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="moudleName">文件存放目录</param>
        /// <param name="encoding">默认为gb2312</param>
        /// <param name="rename">为0时不重命名已上传文件的名称保存，不为0时文件名重命名为filename（若filename为空则用guid命名）</param>
        /// <param name="waterRemarkType">水印图片类型</param>
        /// <param name="filename">保存文件名</param>
        /// <returns></returns>
        protected WebApiRet UploadBase(string moudleName, string encoding = "", string rename = "", int waterRemarkType = 0, string filename = "")
        {
            WebApiRet ret = new WebApiRet();
            ret.RetCode = "0";

            long imgSize = 0;

            if (Request.Files.Count <= 0)
            {
                ret.RetCode = "-1";
                ret.RetMsg = "请选择文件上传";
            }
            if (ret.RetCode == "0")
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];//获取上传的文件

                int hzIndex = file.FileName.LastIndexOf(".");
                string hz = file.FileName.Substring(hzIndex + 1);

                if (ret.RetCode == "0")
                {
                    if (hz.ToLower() != "zip")
                    {
                        imgSize = file.InputStream.Length;
                        if (imgSize > ConfigHelper.MaxExcelProcessNum * 1024 * 1024)
                        {
                            ret.RetCode = "-3";
                            ret.RetMsg = string.Format("文件不能超过{0}M,请选择其他图片上传", ConfigHelper.MaxExcelProcessNum);
                        }
                    }

                    if (ret.RetCode == "0")
                    {
                        int fileLength = (int)file.InputStream.Length;
                        byte[] bytesRead = new byte[(int)file.InputStream.Length];
                        Stream fileStream = file.InputStream;
                        fileStream.Read(bytesRead, 0, fileLength);
                        file.InputStream.Close();

                        if (!string.IsNullOrEmpty(filename) && !filename.EndsWith(hz))  //重命名文件名称无后缀时，添加后缀
                        {
                            filename = string.Concat(filename, ".", hz);
                        }
                        else
                        {
                            filename = file.FileName;
                        }

                        ApiRet uploadResult = NetHelper.UploadFile(ConfigHelper.UploadUrlNew, moudleName, filename, bytesRead, encoding, rename);

                        if (uploadResult.RetCode != "0")
                        {
                            ret.RetCode = "-4";
                            ret.RetMsg = "上传失败";
                            ret.Message = uploadResult;
                        }
                        else
                        {
                            //ret.Message = ConfigHelper.StaticUrl + uploadResult.Message;
                            ret.Message = uploadResult.Message;
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 上传Base64图片
        /// </summary>
        /// <param name="imgBase64String"></param>
        /// <returns></returns>
        public ResultInfo<string> UploadBase64Image(string imgBase64String)
        {
            if (!string.IsNullOrWhiteSpace(imgBase64String))
            {
                string[] base64Part = imgBase64String.Split(',');
                string imgBase64 = base64Part.Length == 1 ? base64Part[0] : base64Part[1];

                string filename = Guid.NewGuid() + ".jpg";
                byte[] fileBytes = Convert.FromBase64String(imgBase64);
                ApiRet uploadResult = NetHelper.UploadFile(ConfigHelper.UploadFileUrl, ConfigHelper.UploadMoudleName, filename, fileBytes);
                if (uploadResult.RetCode == "0")
                {
                    return new ResultInfo<string>(false, "上传成功", "0", uploadResult.Message.ToString());
                }
                else
                {
                    //上传失败
                    return new ResultInfo<string>(true, "上传失败", "-1", uploadResult.RetMsg);
                }
            }
            return null;
        }
        #endregion
    }

    /// <summary>
    /// 如期格式化，返回Json数据
    /// </summary>
    public class MyJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (ContentType!=null)
            {
                response.ContentType = base.ContentType;
            }
            
            if(ContentEncoding!= null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (this.Data != null)
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                // 设置日期序列化的格式
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                response.Write(JsonConvert.SerializeObject(Data, setting));
            }
        }
    }
}
