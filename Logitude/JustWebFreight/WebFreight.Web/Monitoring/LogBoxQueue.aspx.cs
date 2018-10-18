using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class LogBoxQueue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (AnyFailedStatus())
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


        private bool AnyFailedStatus()
        {
            bool IsFaild = false;

            IWebFreightContext Context = WebFreightContext.GetContext(0);

            DateTime twoDaysBefore = DateTime.Now.AddMinutes(-5);

            try
            {
                IsFaild = (from a in Context.QueueMessages
                           where (a.QueueDefinitionCode.Contains("ImportersShipment") || a.QueueDefinitionCode.Contains("ForwardersShipment")) && (a.CreateDateTime > twoDaysBefore) && a.Status == 0
                           select a).Any();
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "LogBoxQueue", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.QueueMessages ...", null);
            }
            
            return IsFaild;
        }


    }
}