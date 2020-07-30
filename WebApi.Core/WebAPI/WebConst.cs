using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Core.WebAPI
{
    public class WebConst
    {
        public const string Header_BeginTime = "WebAPI.BeginTime";
        public const string Header_AppID = "WebAPI.AppID";
        public const string Header_ApiID = "WebAPI.ApiID";
        public const string Header_ApiName = "WebAPI.ApiName";
        public const string Header_ApiModule = "WebAPI.ApiModule";
        public const string Header_PostString = "WebAPI.PostString";
        public const string Header_RetCode = "WebAPI.RetCode";
        public const string Header_AutoWriteLogFlag = "WebAPI.AutoWriteLogFlag";

        //OrderMeal
        public const string Header_UserId = "UserId"; 

        public const string HttpParam_AppID = "appid";
        public const string HttpParam_Encrypt = "encrypt";
        public const string HttpParam_UserIP = "userip";

        public const string HttpPostData_Key = "WEBAPI_POSTDATA";
        public const string HttpPostString_Key = "RUNDA_WEBAPI_POSTSTRING";


        public const string RetCode_Error = "-999999";
        public const string RetCode_SysError = "-999999";

    }
}