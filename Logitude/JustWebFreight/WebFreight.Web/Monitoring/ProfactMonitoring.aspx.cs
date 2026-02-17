using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class ProfactMonitoring : System.Web.UI.Page
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
            bool isWaiting = false;


            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

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
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "ProfactMonitoring", "Bug in ProfactMonitoring", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = todayDateTime.AddDays(-2);


                try
                {
                    isWaiting = (from a in Context.CommunicationLogs
                                 where a.CreateDateUTC > twoDaysBefore
                                 && a.QueueName == "SATInterface" && ((a.CommunicationStatusTypeCode == "W"
                                 && (EntityFunctions.DiffMinutes(a.CreateDateUTC, todayDateTime) > 5)|| a.CommunicationStatusTypeCode == "F"))
                                 select a).Any();


                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "Profact", "Bug in Profact Method : IsFaild", null);
                }

                if (isWaiting)
                {
                    break;
                }
            }
            return isWaiting;
        }
    }
}