using System;
using System.Collections.Generic;


using System.Threading;
using Logitude.SystemLogs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.QueueService;
using System.Web;
using System.Reflection;
using Logitude.Infrastructure.BL.EntityUpdateServices;

namespace CommunicationWorkerRole
{
    class BatchTaskExecutionWR : WorkerEntryPoint
    {
        IQueueService batchTaskExecutionQueue;
        int tenant;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    batchTaskExecutionQueue = new DbQueueService();
                    batchTaskExecutionQueue.InitializeQueue("batchtaskexecutionqueue", 0);
                    var response = batchTaskExecutionQueue.Receive();
                    if (response != null && response.MessageId != null)
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
                                //batchTaskExecutionPM.ClassName this is the path of the class i want to execute which inhirits from BatchTaskExecutionService plus the assembly name.
                                List<object> args = new List<object>();
                                args.Add(batchTaskExecutionPM);
                                object[] ArrArgs = args.ToArray();
                                string[] pathArr = batchTaskExecutionPM.ClassName.Split(',');
                                string assemblyName = pathArr[1];
                                string className = pathArr[0];



                                string contextClassName = Assembly.CreateQualifiedName(assemblyName, className);
                                Type executedClassType = Type.GetType(contextClassName);
                                var batchTaskService = System.Activator.CreateInstance(executedClassType, ArrArgs) as BatchTaskExecutionsService;

                                // open a new thread and call the class runcode.
                                Thread thread = new Thread(batchTaskService.Execute);
                                thread.Start();
                                batchTaskExecutionQueue.Complete();

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
                                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService=new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), batchTaskExecutionPM.Tenant);
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

                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
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
                batchTaskExecutionQueue.InitializeQueue("batchtaskexecutionqueue", 0);

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

    }
}
