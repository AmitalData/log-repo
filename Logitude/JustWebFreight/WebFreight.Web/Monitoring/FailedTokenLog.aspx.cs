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
    public partial class FailedTokenLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (FailedToken())
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

        private bool FailedToken()
        {
            bool IsFailedToken = false;

            int CountFailedToken = 0;

            ISystemLogContext systemLogContext = SystemLogContext.GetContext();

            try
            {
           

                CountFailedToken = (from a in systemLogContext.FailedTokenLogs
                                    where (DbFunctions.DiffMinutes(a.GMTDateTime, DateTime.Now) <= 2)

                                    select a).Count();


            }

            catch (Exception errorInfo)
            {

                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "FailedTokenLog", "Bug in FailedTokenLog Method : IsFailedToken= (from a in Context.FailedTokenLogs ...", "");
            }


            if (CountFailedToken > 10) IsFailedToken = true;
      

            return IsFailedToken;
        }
    }
}