using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using System.Data.Entity.Core.Objects;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Infrastructure.Data;

namespace WebFreight.Web.Monitoring
{
    public partial class BIReportsExecutionLogMonitoring : System.Web.UI.Page
    {
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
            bool isWaitingStatus = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    DateTime twoDaysBefore = DateTime.Now.AddDays(-2);
                    DateTime todayDateTime = DateTime.Now;
                    IInfrastructureContext commonDataContext = InfrastructureContext.GetContext(0);
                    return (from a in commonDataContext.BIReportsExecutionLogs
                            where a.CreateDate > twoDaysBefore
                            &&  ((a.StatusCode == "W"
                            && (EntityFunctions.DiffMinutes(a.CreateDate, todayDateTime) > 5) || a.StatusCode == "F"))
                            select a).Any();

                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "BIReportsExecutionLogs", "Bug in AnyWaitingStatus Method : isWaitingStatus = (from a in commonDataContext.ReportExecutionLogs ...", null);
                }
                scope.Complete();
            }
            return isWaitingStatus;
        }

    }
}