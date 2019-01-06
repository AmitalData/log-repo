using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class ForwarderShipmentDocumentQueue : System.Web.UI.Page
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
            bool IsFaildWaiting = false;
            bool IsFaild = false;

            IWebFreightContext Context = WebFreightContext.GetContext(0);

            DateTime twoDaysBefore = DateTime.Now.AddMinutes(-5);

            try
            {
                IsFaildWaiting = (from a in Context.QueueMessages
                                  where a.QueueDefinitionCode.Contains("ForwardersShipmentDocumentsQueue") && a.Status == 0
                                  select a).Count() > 30;

                IsFaild = (from a in Context.QueueMessages
                           where a.QueueDefinitionCode.Contains("ForwardersShipmentDocumentsQueue") && a.Status == -1
                           && a.CreateDateTime >= DateTime.Now.Date
                           select a).Count() > 50;
            }

            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "ForwardersShipmentDocumentsQueue", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.QueueMessages ...", null);
            }

            return (IsFaild || IsFaildWaiting);
        }
    }
}