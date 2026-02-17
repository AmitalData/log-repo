using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
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
    public partial class CommunicationFailedStatus : System.Web.UI.Page
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

            if (Request != null)
            {
                string myTarget = Request.QueryString["target"];

                if (!string.IsNullOrEmpty(myTarget))
                {
                    string toParameter = null;

                    switch (myTarget)
                    {
                        case "CHAMP":
                            {
                                toParameter = "Champ";
                                break;
                            }

                        case "GLSHK":
                            {
                                toParameter = "GLSHK";
                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(toParameter))
                    {
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
                                ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "CommunicationFailedStatus", "Bug in AnyFailedStatus Method : globaldbRep.All()", null);
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
                                            where a.CommunicationStatusTypeCode == "f" &&  a.CommunicationLogTypeCode != "A"
                                            && (a.CreateDateUTC > twoDaysBefore) && a.To == toParameter  
                                            select a).Any();
                            }

                            catch (Exception errorInfo)
                            {
                                ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "CommunicationFailedStatus", "Bug in AnyFailedStatus Method : IsFaild = (from a in Context.CommunicationLogs [" + toParameter + "] ...", null);
                            }

                            if (isFailed)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return isFailed;
        }
    }
}