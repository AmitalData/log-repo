





using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
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
    public partial class AnalyzeQueue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
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
            bool isWaiting = false;

            if (Request != null)
            {
                string myTarget = Request.QueryString["target"];

                if (!string.IsNullOrEmpty(myTarget))
                {
                    string fromParameter = null;

                    switch (myTarget)
                    {

                        case "MAILGUN":
                            {
                                fromParameter = "MailGun";
                                break;
                            }
                        case "SENDGRID":
                            {
                                fromParameter = "SendGrid";
                                break;
                            }

                        case "CHAMP":
                            {
                                fromParameter = "Champ";
                                break;
                            }

                        case "GLSHK":
                            {
                                fromParameter = "GLSHK";
                                break;
                            }

                        case "TFS":
                            {
                                fromParameter = "TFS";
                                break;
                            }


                        case "TMC":
                            {
                                fromParameter = "TMC";
                                break;
                            }

                        case "Artemus":
                            {
                                fromParameter = "Artemus";
                                break;
                            }

                        case "DocumentFilingEmail":
                            {
                                fromParameter = "DocumentFilingEmail";
                                break;
                            }
                    }

                    if (!string.IsNullOrEmpty(fromParameter))
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

               
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();
                            AnalyzeQueueRepository myRepository = new AnalyzeQueueRepository(globalContext);


                            try
                            {
                                isWaiting = (from a in myRepository.context.AnalyzeQueues
                                             where a.Status == "W" && a.From == fromParameter
                                             && (System.Data.Entity.DbFunctions.DiffMinutes(a.CreateDate, todayDateTime) > 5)
                                             select a).Any();
                                             
                            }

                            catch (Exception errorInfo)
                            {
                                ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "AnalyzeQueuesStatus", "Bug in AnyWaitingStatus Method : IsFaild = (from a in Context.AnalyzeQueues [" + fromParameter + "] ...", null);
                            }

                            scope.Complete();
                        }

                    }
                }
            }

            return isWaiting;
        }
    }
}