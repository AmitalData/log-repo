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
using Simplog.Data.InfrastructureModel;

namespace WebFreight.Web.Monitoring
{
    public partial class AgentSharedDocumentMonitoring : System.Web.UI.Page
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
                    isFailed = CheckIfAnyQueueMessagesFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "AgentsSharedDocumentQueue", "Bug in AgentsSharedDocumentQueue Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isFailed;
        }

        private static bool CheckIfAnyQueueMessagesFailed()
        {
            DateTime todayDateTime = DateTime.Now;
            DateTime twoDaysBefore = todayDateTime.AddDays(-2);
            IWebFreightContext context = WebFreightContext.GetContext(0);
            return (from a in context.QueueMessages
                    where a.CreateDateTime > twoDaysBefore
                    && a.QueueDefinitionCode == "AgentsSharedDocumentQueue" && ((a.Status == 0
                    && (EntityFunctions.DiffMinutes(a.CreateDateTime, todayDateTime) > 5)) || a.Status == -1)
                    select a).Any();

        }


    }
}