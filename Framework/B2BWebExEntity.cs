using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ThinkDev.Logging.Entity;

namespace Framework
{
    [Serializable]
    public class B2BWebExEntity : ExEntityBase
    {
        public string UrlInfo { get; set; }

        public string ClientIP { get; set; }

        public string HostIP { get; set; }

        public string UserAgent { get; set; }

        public B2BWebExEntity(Exception ex)
            : this()
        {
            this.SetException(ex);
        }

        public B2BWebExEntity()
        {
        }

        public new void SetException(Exception ex)
        {
            base.SetException(ex);
            this.UrlInfo = string.Format("{0} => {1}", HttpContext.Current.Request.UrlReferrer == (Uri)null ? (object)"" : (object)HttpContext.Current.Request.UrlReferrer.ToString(), HttpContext.Current.Request.Url == (Uri)null ? (object)"" : (object)HttpContext.Current.Request.Url.ToString());
            this.ClientIP = string.Format("{0} => {1}", (object)HttpContext.Current.Request.UserHostAddress, (object)HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]);
            this.HostIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            this.UserAgent = HttpContext.Current.Request.UserAgent;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("UrlInfo:[{0}]", (object)this.UrlInfo));
            stringBuilder.AppendLine(string.Format("UserAgent:[{0}]", (object)this.UserAgent));
            stringBuilder.AppendLine(string.Format("ClientIP:[{0}]", (object)this.ClientIP));
            stringBuilder.AppendLine(string.Format("HostIP:[{0}]", (object)this.HostIP));
            return stringBuilder.ToString() + base.ToString();
        }
    }
}
