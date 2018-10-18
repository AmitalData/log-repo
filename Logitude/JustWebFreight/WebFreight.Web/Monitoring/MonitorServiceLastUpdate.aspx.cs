using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class MonitorServiceLastUpdate : System.Web.UI.Page
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
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(0);

                        IGlobalContext globalContext = GlobalContext.GetContext();
                        MonitorServiceLastUpdateRepository myRepository = new MonitorServiceLastUpdateRepository(globalContext);

                        string[] myTargetSplit = myTarget.Split(',');
                        string myCode = myTargetSplit[0];
                        string myGateway = myTargetSplit[1];

                        if (myCode != null)
                        {
                            myCode = myCode.ToUpper();
                        }

                        if (myGateway != null)
                        {
                            myGateway = myGateway.ToUpper();
                        }

                        string myFullTarget = myCode;

                        if (myGateway != null)
                        {
                            myFullTarget = myCode + "_" + myGateway.ToUpper();
                        }

                        switch (myCode)
                        {
                            case "CHAMP":
                                {
                                    try
                                    {
                                        isWaiting = (from a in myRepository.context.MonitorServiceLastUpdates
                                                     where a.Code == myFullTarget
                                                     && (DbFunctions.DiffMinutes(a.LastUpdate, todayDateTime) > 3)
                                                     select a).Any();
                                    }

                                    catch (Exception errorInfo)
                                    {
                                        ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "MonitorServiceLastUpdate", "Bug in AnyWaitingStatus Method : IsFaild = (from a in " + myFullTarget + " ...", null);
                                    }

                                    break;
                                }

                            case "GLSHK":
                                {
                                    try
                                    {
                                        isWaiting = (from a in myRepository.context.MonitorServiceLastUpdates
                                                     where a.Code == myFullTarget
                                                     && (DbFunctions.DiffMinutes(a.LastUpdate, todayDateTime) > 3)
                                                     select a).Any();
                                    }

                                    catch (Exception errorInfo)
                                    {
                                        ExceptionHandler.HandleException(errorInfo, todayDateTime, 0, "", "MonitorServiceLastUpdate", "Bug in AnyWaitingStatus Method : IsFaild = (from a in " + myFullTarget + " ...", null);
                                    }

                                    break;
                                }
                        }

                        scope.Complete();
                    }
                }
            }

            return isWaiting;
        }
    }
}