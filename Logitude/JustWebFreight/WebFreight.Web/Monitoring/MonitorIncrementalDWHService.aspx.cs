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

namespace WebFreight.Web.Monitoring
{
    public partial class MonitorIncrementalDWHService : System.Web.UI.Page
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
                DateTime todayDateTime = DateTime.Now;

                IGlobalContext globalContext = GlobalContext.GetContext();
                MobileNotificationLogRepository myRepository = new MobileNotificationLogRepository(globalContext);


                try
                {

                    isFailed = (from d in myRepository.context.Settings 
                                                 where (d.LastIncrementalDWUpdateDate == null ||  (EntityFunctions.DiffMinutes(d.LastIncrementalDWUpdateDate, todayDateTime) > 5)) && !d.IsFullBuildDWRunning
                                                 select d).Any();
                    var isUpgrading = false;
                    if (isFailed)
                    {
                        isUpgrading = (from d in myRepository.context.GlobalDBs
                                    where d.IsUpgrading
                                    select d).Any();

                        if (isUpgrading) isFailed = false;
                    }

                }

                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "MonitorIncrementalDWHService", "Bug in MonitorIncrementalDWHService Method : globaldbRep.All()", null);
                }



                scope.Complete();
            }

            return isFailed;


        }





    }
}