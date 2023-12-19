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
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Infrastructure.Data;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Stimulsoft.Base.Json.Linq;
using Newtonsoft.Json;
using static Dropbox.Api.Sharing.SharePathError;

namespace WebFreight.Web.Monitoring
{
    public partial class CloudThreshold : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Clear();
            Response.ContentType = "text/xml";
            Response.Write("<pingdom_http_custom_check>");
            string mytenant = Request.QueryString["Tenant"];
            int tenant = 0;
            if (!string.IsNullOrEmpty(mytenant))
            {
                tenant = Int32.Parse(mytenant);

            }
            string message = string.Empty;
            if (AnyFailedStatus(tenant))
            {
                Response.Write("<status>Fail</status>");
                //Response.Write("<list>${message}</list>");
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
        class MyClass
        {
            public string BatchTaskExecutionId;
        }


        private bool AnyFailedStatus(int tenant)
        {

            bool IsFaildWaiting = true;

            //int? WaitingThresold = null;
            //int? FailedThresold = null;
            //int? WaitingQueue = null;
            //int? FailedQueue = null;
            //DateTime? LastUpdate = null;
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

                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "CloudThreshold", "Bug in AnyWaitingStatus Method : globaldbRep.All()", null);
                }
                scope.Complete();
            }


            foreach (GlobalDB db in GlobalDatabases)
            {

                CommonDataContext CommonContext = CommonDataContext.GetContextByDBId(db.Id);
                IWebFreightContext Context = WebFreightContext.GetContext(tenant);

                CargoTrackingWatermarkQueryService cargoTrackingWatermarkQueryService = new CargoTrackingWatermarkQueryService(tenant);
                List<CargoTrackingWatermark> cargoTrackingWatermarks = cargoTrackingWatermarkQueryService.GetAllWaterMarks();
                var IncrementalLastRun = cargoTrackingWatermarks == null ? null : cargoTrackingWatermarks.OrderByDescending(s => s.LastRun).FirstOrDefault().LastRun;
                //    DateTime TenMinutesBefore = DateTime.Now.AddMinutes(-10);
                try
                {
                    var hybridTenantThreshold = (from a in CommonContext.HybridTenantThresholds
                                                 where a.Tenant == 16
                                                 select new
                                                 {
                                                     a.Tenant,
                                                     a.WaitingThresold,
                                                     a.FailedThresold,
                                                 }).FirstOrDefault();
                    if (hybridTenantThreshold != null)
                    {
                        //  List<String> taskList = (from task in infrastructureContext.BatchTaskExecutions
                        //                           where task.Subject == BatchTaskNames.BuildCargoTrackingShipments
                        //                           select task.Id).ToList();

                        //List<QueueMessage> queueList = (from queue in webFreightContext.QueueMessages where queue.Status == 0 && queue.QueueDefinitionCode == "batchtaskexecutionqueue" && taskList.Contains(queue.MessageBody["BatchTaskExecutionId"].ToString()) select queue).ToList();
                        //  if (queueList?.Count() > hybridTenantThreshold?.FailedThresold && IncrementalLastRun > TenMinutesBefore)
                        //      IsFailed = true;
                        IsFaildWaiting = (from a in Context.QueueMessages
                                          where (a.QueueDefinitionCode.Contains("ImportersShipmentQueue") || a.QueueDefinitionCode.Contains("ImportersShipmentsBatchQueue")) && a.Status == 0
                                          select a).Count() > hybridTenantThreshold.WaitingThresold && IncrementalLastRun < DateTime.Now.AddMinutes(-10);

                    }
                }


                catch (Exception errorInfo)
                {
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, 0, "", "CloudThreshold", "Bug in AnyWaitingStatus Method : IsFaildWaiting = (from a in Context.HybridTenantThresholds ...", null);
                }





            }
            return IsFaildWaiting;
        }
    }
}