using Logitude.BL.CommonDataModel.Helpers;
using Logitude.Infrastructure.Data;
using Logitude.SystemLogs;

using System;
using System.Linq;
using System.Web;



namespace WebFreight.Web.Monitoring
{
    public partial class CargoBuild : System.Web.UI.Page
    {
        const string Fail = "F";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
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
            bool isFaild = false;
            IInfrastructureContext context = InfrastructureContext.GetContext(0);
            DateTime fiveMinutesBefore = DateTime.Now.AddMinutes(-5);
            try
            {
                isFaild = context.BatchTaskExecutions.Where(e =>e.Subject == BatchTaskNames.BuildCargoTrackingShipments && e.CreateDate >= fiveMinutesBefore && e.StatusCode == Fail).Any();
            }
            catch (Exception errorInfo)
            {
                ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "CargoBuild", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.BatchTaskExecutions ...", null);
            }
            return isFaild;
        }
    }
}