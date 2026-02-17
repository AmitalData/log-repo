using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class FailedLoginLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (FailedLogin()) 
            {
                Response.Write("<status>Fail</status>");
            }
            else
            {
                Response.Write("<status>OK</status>");
            }
            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool FailedLogin()
        {
            bool IsFailedLogin = false;

            int CountFailedLogin = 0;
       
                ISystemLogContext systemLogContext = SystemLogContext.GetContext();
      
                try
                {
                    CountFailedLogin = (from a in systemLogContext.FailedLoginLogs
                                 where (DbFunctions.DiffMinutes(a.GMTDateTime, DateTime.Now) <= 2)

                                 select a).Count();
                
                }

                catch (Exception errorInfo)
                {

                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "FailedLoginLog", "Bug in FailedLoginLog Method : IsFailedLogin = (from a in Context.FailedLoginLogs ...", "");
                }


                if (CountFailedLogin > 10)
                {
                    IsFailedLogin = true;
                }

            return IsFailedLogin;
        }
    }
}