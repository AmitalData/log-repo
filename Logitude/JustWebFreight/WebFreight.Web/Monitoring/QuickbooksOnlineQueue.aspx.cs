using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
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
    public partial class QuickbooksOnlineQueue : System.Web.UI.Page
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
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            bool isFailed = false;


            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();

                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "QBO", "Bug in Quickbooksonline Method : globaldbRep.All()", null);
                }

                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {
                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = todayDateTime.AddDays(-2);

                try
                {
                    isFailed = (from a in Context.CommunicationLogs
                                where a.CommunicationStatusTypeCode == "f"
                                && (a.CreateDateUTC > twoDaysBefore) && (a.To == "QBO")
                                select a).Any();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "Quickbooksonline", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.CommunicationLogs [QBO] ...", null);
                }

                if (isFailed)
                {
                    break;
                }
            }

            return isFailed;
        }
    }
}