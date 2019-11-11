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
    public partial class DocumentsExecutionLogWaitingStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");

            if (AnyDocumentsExecutionLogsWaitingStatus())
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

        private bool AnyDocumentsExecutionLogsWaitingStatus()
        {
            bool isWaitingStatus = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    DateTime twoDaysBefore = DateTime.Now.AddDays(-2);
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
                    isWaitingStatus = (from d in commonDataContext.DocumentsExecutionLogs
                                       where d.CreateDate > twoDaysBefore && d.StatusCode == "W" && (EntityFunctions.DiffMinutes(d.CreateDate, DateTime.Now) > 1)
                                       select d).Any();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "DocumentsExecutionLogsWaitingStatus", "Bug in AnyWaitingStatus Method : isWaitingStatus = (from a in commonDataContext.DocumentsExecutionLogs ...", null);
                }
                scope.Complete();
            }
            return isWaitingStatus;
        }
    }
}