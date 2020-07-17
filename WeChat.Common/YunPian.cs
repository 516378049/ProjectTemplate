using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WeChat.Common
{
    public class YunPian
    {
        public static bool SendSms(string mobile, int tplId, string code)
        {
            var values = new Dictionary<string, string>();
            values.Add("code", code);
            return SendSms(mobile, tplId, values);
        }

        public static bool SendSms(string mobile, int tplId, string key, string value)
        {
            var values = new Dictionary<string, string>();
            values.Add(key, value);
            return SendSms(mobile, tplId, values);
        }

        public static bool SendSms(string mobile, int tplId, Dictionary<string, string> tplValues)
        {
            var apikey = CloudConfigHelper.GetSetting("YunPianApiKey");
            var url = CloudConfigHelper.GetSetting("YunPianSendUrl");

            string tpl_value = "";

            foreach (var key in tplValues.Keys)
            {
                string value = tplValues[key];
                tpl_value += HttpUtility.UrlEncode("#" + key + "#", Encoding.UTF8) + "=" +
                    HttpUtility.UrlEncode(value, Encoding.UTF8) + "&";
            }
            tpl_value.TrimEnd('&');

            if (!string.IsNullOrWhiteSpace(apikey) && !string.IsNullOrWhiteSpace(url))
            {
                var client = new HttpClient();
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("apikey",apikey),
                    new KeyValuePair<string,string>("mobile",mobile),
                    new KeyValuePair<string,string>("tpl_id",tplId.ToString()),
                    new KeyValuePair<string,string>("tpl_value",tpl_value)
                });
                var res = client.PostAsync(url, content).Result;
                if (res.IsSuccessStatusCode) return true;
                else return false;
            }

            return false;
        }
    }
}
