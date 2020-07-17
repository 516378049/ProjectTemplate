using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace WeChat.Common
{
    public class ScriptHelpers
    {
        public static void Alert(Page page, string msg)
        {

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", "alert('" + msg + "');", true);
        }

        public static void Alert(Control c, string msg,string url="")
        {
            string urlParam = string.IsNullOrWhiteSpace(url) ? "" : ",'" + url + "'";
            ScriptManager.RegisterClientScriptBlock(c, c.GetType(), "alert", "alert('" + msg + "'" + urlParam + ");", true);
        }

        public static void DivAlert(Control c, string msg, string url = "")
        {
            string urlParam = string.IsNullOrWhiteSpace(url) ? "" : ",'" + url + "'";
            ScriptManager.RegisterClientScriptBlock(c, c.GetType(), "alert", "alert('" + msg + "'" + urlParam + ");", true);
            //ScriptManager.RegisterClientScriptBlock(c, c.GetType(), "alert", "$('#ct-alert .am-modal-bd').html('" + msg + "');$('#ct-alert').modal();", true);
        }

        public static void Execute(Control c, string scriptText)
        {
            ScriptManager.RegisterClientScriptBlock(c, c.GetType(), "script", scriptText, true);
        }


        public static void ExecuteBottom(Control c, string scriptText)
        {
            ScriptManager.RegisterStartupScript(c, c.GetType(), "script", scriptText, true);
        }
    }
}
