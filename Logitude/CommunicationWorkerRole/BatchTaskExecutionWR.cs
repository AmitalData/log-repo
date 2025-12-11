using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace CommunicationWorkerRole
{
    class BatchTaskExecutionWR : WorkerEntryPoint
    {
        DbQueueService batchTaskExecutionQueue;
        int tenant;

        public bool SupressStartThread { get; internal set; }
        private static DateTime freeTenantsDateTime = DateTime.Now;
        private string objectTable = "BatchTaskExecution";

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {

                    batchTaskExecutionQueue = new DbQueueService();
                    batchTaskExecutionQueue.InitializeQueue("batchtaskexecutionqueue", SettingUtil.GetTenantDBFromConfig());
                    if (DateTime.Now.Subtract(freeTenantsDateTime) >= TimeSpan.FromMinutes(10))
                    {
                        freeTenantsDateTime = DateTime.Now;
                        batchTaskExecutionQueue.FreeTenants(objectTable);
                    }
                    var response = batchTaskExecutionQueue.ReceiveDetailsByTenant(objectTable, new TimeSpan(0, 0, 0, 5));
                    if (response != null && response.MessageId != null)
                    {
                        ExecuteQueue(response);
                        batchTaskExecutionQueue.Complete();

                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }

        }

        public void ExecuteQueue(QueueResponse response)
        {
            LastActivity = DateTime.UtcNow;
            string batchTaskExecutionId = response.MessageValues["BatchTaskExecutionId"].ToString();
            int.TryParse(response.MessageValues["Tenant"], out tenant);
            BatchTaskExecutionPM batchTaskExecutionPM = null;
            try
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(batchTaskExecutionId, false, false);

                if (batchTaskExecutionPM != null)
                {
                    List<object> args = new List<object>();
                    args.Add(batchTaskExecutionPM);
                    object[] ArrArgs = args.ToArray();
                    string[] pathArr = batchTaskExecutionPM.ClassName.Split(',');
                    string assemblyName = pathArr[1];
                    string className = pathArr[0];



                    string contextClassName = Assembly.CreateQualifiedName(assemblyName, className);
                    Type executedClassType = Type.GetType(contextClassName);
                    var batchTaskService = System.Activator.CreateInstance(executedClassType, ArrArgs) as BatchTaskExecutionsService;
                    if (!this.SupressStartThread)
                    {
                        Thread thread = new Thread(batchTaskService.Execute);
                        thread.Start();
                    }
                    else
                    {
                        batchTaskService.Execute();
                    }

                }

            }
            catch (Exception ex)
            {
                #region HandleException
                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "BatchTaskExecutionWR", "", null);
                if (batchTaskExecutionPM != null)
                {
                    if (ex != null)
                    {
                        string errorMessage = ex.Message + Environment.NewLine;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                        }

                        batchTaskExecutionPM.ErrorLog = errorMessage;
                        batchTaskExecutionPM.CallStack = ex.StackTrace;
                    }
                    batchTaskExecutionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    Logitude.Infrastructure.Data.IInfrastructureContext context = Logitude.Infrastructure.Data.InfrastructureContext.GetContext(batchTaskExecutionPM.Tenant);
                    BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), batchTaskExecutionPM.Tenant);
                    batchTaskExecutionUpdateService.Update(batchTaskExecutionPM, true);
                }

                if (response.MessageValues.Keys.Contains("BatchTaskExecutionId"))
                {
                    if (response.RetryNumber <= 1)
                    {
                        batchTaskExecutionQueue.Delay(new TimeSpan(0, 0, 0, 5));
                    }

                    if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                    {
                        batchTaskExecutionQueue.Delay(new TimeSpan(0, 0, 0, 10));
                    }
                    if (response.RetryNumber >= 3)
                    {
                        batchTaskExecutionQueue.CompleteAsFailed();
                    }
                }
                else
                {
                    batchTaskExecutionQueue.CompleteAsFailed();
                }
                #endregion
            }
            finally
            {
                SetTenantIdle(response.Tenant);
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "BatchTaskExecution";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            try
            {
                batchTaskExecutionQueue = new DbQueueService();
                batchTaskExecutionQueue.InitializeQueue("batchtaskexecutionqueue", SettingUtil.GetTenantDBFromConfig());

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "batchtaskexecutionqueue Role", null, ip);


            }
            return base.OnStart();

        }

        private void SetTenantIdle(int tenant)
        {
            TenantIdleStatusRepository tenantRepository = new TenantIdleStatusRepository(tenant);
            TenantIdleStatus tenantObj = tenantRepository.GetAllByObjectTable(tenant, objectTable).FirstOrDefault();
            if (tenantObj == null) return;
            tenantObj.Idle = false;
            tenantObj.UpdateDate = DateTime.Now;
            tenantRepository.Update(tenantObj);
            tenantRepository.SubmitChanges();

        }
    }
}
