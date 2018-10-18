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
namespace WebFreight.Web.Monitoring
{
    public partial class HybridTenantThreshold : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            string mytenant = Request.QueryString["Tenant"];
            int? tenant = null;
            if (!string.IsNullOrEmpty(mytenant))
            {
                 tenant = Int32.Parse(mytenant);
               
            }
            if (AnyFailedStatus(tenant))
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

        private bool AnyFailedStatus(int?tenant)
        {

            bool IsFailed = false;
           
             int? WaitingThresold  =null;
             int? FailedThresold=null;
             int? WaitingQueue=null;
             int? FailedQueue =null;
             DateTime? LastUpdate = null;
            List<GlobalDB> GlobalDatabases = new List<GlobalDB>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalDBRepository globaldbRep = new GlobalDBRepository();
                try
                {
                    GlobalDatabases = globaldbRep.All();

                }

                catch (Exception errorInfo)
                {

                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "HybridTenantThreshold", "Bug in AnyWaitingStatus Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {

                CommonDataContext Context = CommonDataContext.GetContextByDBId(db.Id);

                  try
                  {
                    var hybridTenantThreshold = (from a in Context.HybridTenantThresholds
                                            where a.Tenant == tenant
                                                 select new { a.WaitingThresold, a.FailedThresold }).FirstOrDefault();
                      if (hybridTenantThreshold != null)
                      {
                          WaitingThresold = hybridTenantThreshold.WaitingThresold;
                          FailedThresold = hybridTenantThreshold.FailedThresold;
                      }
                  }

                  catch (Exception errorInfo)
                  {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "HybridTenantThreshold", "Bug in AnyWaitingStatus Method : IsFaild = (from a in Context.HybridTenantThresholds ...", null);
                  }


                  try
                  {
                    var HybridTenantStates = (from a in Context.HybridTenantStates
                                          where a.Tenant == tenant
                                              select new { a.WaitingQueue, a.FailedQueue, a.LastUpdateDateTime }).FirstOrDefault();
                       if (HybridTenantStates != null)
                       {
                           WaitingQueue = HybridTenantStates.WaitingQueue;
                           FailedQueue = HybridTenantStates.FailedQueue;
                           LastUpdate = HybridTenantStates.LastUpdateDateTime;
                       }
                  
                   }


                  catch (Exception errorInfo)
                  {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "HybridTenantThreshold", "Bug in AnyWaitingStatus Method : IsFaild = (from a in Context.HybridTenantStates ...", null);
                  }


                  TimeSpan? date = DateTime.UtcNow - LastUpdate;
                if (LastUpdate != null && date.Value.Hours < 1)
                  {
                    if (WaitingQueue > WaitingThresold || FailedQueue > FailedThresold) IsFailed = true;
                      else IsFailed = false;
                  }
                  else IsFailed = true;
            }
            return IsFailed;

      
        }
    }
}