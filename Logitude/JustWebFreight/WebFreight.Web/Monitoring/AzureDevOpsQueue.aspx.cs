using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel;
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

namespace WebFreight.Web.Monitoring
{
    public partial class AzureDevOpsQueue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (AnyWaitingStatus())
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

        private bool AnyWaitingStatus()
        {
            bool IsWaiting = false;
            bool IsFailed = false;

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AzureDevOpsQueue", "Bug in AnyWaitingStatus Method : globaldbRep.All()", null);
                }

                scope.Complete();
            }

            foreach (GlobalDB db in GlobalDatabases)
            {
                GlobalContext Context = GlobalContext.GetContextByDBId(db.Id);

                DateTime fiveMinutesBefore = DateTime.Now.AddMinutes(-5);
                DateTime TwoDaysAgo = DateTime.Now.AddDays(-2);
                try
                {
                    IsWaiting = (from a in Context.AnalyzeQueues.Include("AnalyzeQueueStatus")
                                 where
                                 a.Status == "W"
                                 && a.From == "TFSAggregator"
                                 && a.CreateDate < fiveMinutesBefore
                                 && (System.Data.Entity.DbFunctions.DiffMinutes(a.CreateDate, DateTime.Now) > 5)
                                 select a).Any();
                    if (!IsWaiting)
                    {
                        IsFailed = (from a in Context.AnalyzeQueues.Include("AnalyzeQueueStatus")
                                    where
                                    a.Status == "F"
                                    && a.From == "TFSAggregator"
                                    && a.CreateDate > TwoDaysAgo
                                    select a).Any();
                    }

                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AzureDevOpsQueue", "Bug in AnyWaitingStatus Method : IsFaild = (from a in Context.AnalyzeQueues ...", null);
                }

                if (IsWaiting || IsFailed)
                {
                    break;
                }
            }

            return (IsWaiting | IsFailed);
        }

    }
}