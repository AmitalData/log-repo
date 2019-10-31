

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

namespace WebFreight.Web.Monitoring
{
    public partial class ReportExecutionRunTimeMonitoring : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");

            if (AnyReportTakesMoreThanExpectedToRun())
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

        private bool AnyReportTakesMoreThanExpectedToRun()
        {
            bool isWaitingStatus = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    DateTime oneDaysBefore = DateTime.Now.AddDays(-1);
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
                    isWaitingStatus = (from d in commonDataContext.ReportExecutionLogs where d.StatusCode == "P" && d.CreateDate > oneDaysBefore && (EntityFunctions.DiffMinutes(d.CreateDate, DateTime.Now) > 60) select d).Any();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "ReportExecutionRunTimeMonitoring", "Bug in AnyReportTakesMoreThanExpectedToRun Method : isWaitingStatus = (from a in commonDataContext.ReportExecutionLogs ...", null);
                }
                scope.Complete();
            }
            return isWaitingStatus;
        }

    }
}