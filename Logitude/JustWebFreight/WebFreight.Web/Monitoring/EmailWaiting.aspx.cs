using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.GlobalModelDB;
using System.Transactions;
using WebFreight.Web.CommonDataModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.GlobalModel;
using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace WebFreight.Web.Monitoring
{
    public partial class EmailWaiting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (AnyWaitingEmail()) // waiting more than 5 minutes
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

        private bool AnyWaitingEmail()
        {
            bool IsWaiting = false;
            List<GlobalDB> GlobalDatabases=new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();
                try
                {
                    GlobalDatabases = globaldbRep.All();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "EmailFailure", "Bug in AnyWaitingEmail Method : globaldbRep.All()", "");
                }
                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {

                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                DateTime twoDaysBefore = DateTime.Now.AddDays(-2);

                try
                {
                    IsWaiting = (from a in Context.CommunicationLogs
                                 where a.CreateDateUTC > twoDaysBefore && a.CommunicationStatusTypeCode == "W" && a.CommunicationLogTypeCode == "E" &&  (DbFunctions.DiffMinutes(a.CreateDateUTC, DateTime.Now) > 5) 
                                 //where a.CreateDateUTC > twoDaysBefore && a.CommunicationStatusTypeCode == "W" && (a.To != "Champ" || a.From != "Champ") && (DbFunctions.DiffMinutes(a.CreateDateUTC, DateTime.Now) > 5) 
                                 //where a.CommunicationStatusTypeCode == "W" && EntityFunctions.DiffMinutes(a.CreateDateUTC, a.NextTryDateTimeUTC.Value) > 5 && (a.To != "Champ" || a.From != "Champ")
                               select a).Any();
                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "EmailWaiting", "Bug in AnyWaitingEmail Method : IsWaiting = (from a in Context.CommunicationLogs ...", "");
                }


                if (IsWaiting)
                {
                    break;
                }
            }

            return IsWaiting;
        }
    }
}