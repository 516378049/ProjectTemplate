using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeBlock.Controllers
{
    public class UEditorController : Controller
    {
        // GET: UEditor
        public ActionResult Index()
        {
            return View("UEditor");
        }


        /// <summary>
        /// EDitor Hander
        /// </summary>
        public void ProcessRequest()
        {
            HttpContext context = System.Web.HttpContext.Current;
            Handler action = null;
            switch (Request["action"])
            {
                case "config":
                    action = new ConfigHandler(context);
                    break;
                case "uploadimage":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("imageAllowFiles"),
                        PathFormat = Config.GetString("imagePathFormat"),
                        SizeLimit = Config.GetInt("imageMaxSize"),
                        UploadFieldName = Config.GetString("imageFieldName")
                    });
                    break;
                case "uploadscrawl":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = Config.GetString("scrawlPathFormat"),
                        SizeLimit = Config.GetInt("scrawlMaxSize"),
                        UploadFieldName = Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                    break;
                case "uploadvideo":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("videoAllowFiles"),
                        PathFormat = Config.GetString("videoPathFormat"),
                        SizeLimit = Config.GetInt("videoMaxSize"),
                        UploadFieldName = Config.GetString("videoFieldName")
                    });
                    break;
                case "uploadfile":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("fileAllowFiles"),
                        PathFormat = Config.GetString("filePathFormat"),
                        SizeLimit = Config.GetInt("fileMaxSize"),
                        UploadFieldName = Config.GetString("fileFieldName")
                    });
                    break;
                case "listimage":
                    action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                    break;
                case "listfile":
                    action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage":
                    action = new CrawlerHandler(context);
                    break;
                default:
                    action = new NotSupportedHandler(context);
                    break;
            }
            action.Process();
        }

        # region imageUp
        [HttpPost]
        public void uploadimage()
        {
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            //上传配置
            string pathbase = "/Content/UEditor/upload/image/";                                                          //保存路径
            int size = 10;                     //文件大小限制,单位mb                                                                                   //文件大小限制，单位KB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                    //文件允许格式

            string callback = Request["callback"];
            string editorId = Request["editorid"];

            //上传图片
            Hashtable info;
            Uploader up = new Uploader();

            info = up.upFile(pathbase, filetype, size); //获取上传状态
            string json = BuildJson(info);

            HttpContext.Response.ContentType = "text/html";
            if (callback != null)
            {
                HttpContext.Response.Write(String.Format("<script>{0}(\"{1}\");</script>", callback, json));
            }
            else
            {
                HttpContext.Response.Write(json);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string BuildJson(Hashtable info)
        {
            List<string> fields = new List<string>();
            string[] keys = new string[] { "imageUrl", "imagePath", "imageFieldName", "imageMaxSize", "imageAllowFiles" };
            for (int i = 0; i < keys.Length; i++)
            {
                fields.Add(String.Format("\"{0}\": \"{1}\"", keys[i], info[keys[i]]));
            }
            return "{" + String.Join(",", fields) + "}";
        }
        #endregion
    }
}