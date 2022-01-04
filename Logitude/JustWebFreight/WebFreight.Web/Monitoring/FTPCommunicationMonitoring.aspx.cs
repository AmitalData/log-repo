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
using System.Data.Entity;

namespace WebFreight.Web.Monitoring
{
    public partial class FTPCommunicationMonitoring : System.Web.UI.Page
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
            bool isFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isFailed = CheckIfAnyCommunicationLogsFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "FTPCommunicationMonitoring", "Bug in FTPCommunicationMonitoring Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyCommunicationLogsFailed()
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);

            return (from a in commonDataContext.CommunicationLogs
                    where a.CreateDateUTC > twoDaysBefore
                    && a.QueueName == "FTPCommunicationLogQueue" && ((a.CommunicationStatusTypeCode == "W"
                    && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5) || a.CommunicationStatusTypeCode == "F"))
                    select a).Any();

        }

    }
}