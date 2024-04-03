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
            bool incrementalDWUpdateFailed = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    incrementalDWUpdateFailed = CheckIfIncrementalDWUpdateFailed();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "MonitorIncrementalDWHService", "Bug in MonitorIncrementalDWHService Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return incrementalDWUpdateFailed;
        }

        private static bool CheckIfIncrementalDWUpdateFailed()
        {
            DateTime todayDateTime = DateTime.Now;
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            bool incrementalDWUpdateFailed = (from d in commonDataContext.DWHBuildStatus
                             where (d.LastIncrementalDWUpdateDate == null || (EntityFunctions.DiffMinutes(d.LastIncrementalDWUpdateDate, todayDateTime) >10)) && (!d.IsFullBuildDWRunning || (d.IsFullBuildDWRunning && EntityFunctions.DiffHours(d.LastIncrementalDWUpdateDate, todayDateTime) > 5))
                                              select d).Any();
            if (incrementalDWUpdateFailed)
            {
                if (CheckIfSystemIsUpgrading()) incrementalDWUpdateFailed = false;
            }
            return incrementalDWUpdateFailed;
        }

        private static bool CheckIfSystemIsUpgrading()
        {
            IGlobalContext globalContext = GlobalContext.GetContext();
            bool isUpgrading = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    isUpgrading = (from d in globalContext.GlobalDBs
                                   where d.IsUpgrading
                                   select d).Any();
                }
                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "MonitorIncrementalDWHService", "Bug in MonitorIncrementalDWHService Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }
            return isUpgrading;
        }
    }
}