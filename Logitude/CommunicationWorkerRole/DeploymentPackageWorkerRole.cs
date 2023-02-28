using CommunicationWorkerRole.Services.DeploymentPackages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CommunicationWorkerRole
{
    public class DeploymentPackageWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        string URI = "";
        DeploymentPackageWRService deploymentPackageService;
        string deploymentPackageId;
        public DeploymentPackageWorkerRole()
        {
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DeploymentPackageWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("DeploymentPackageQueue", 0);

            }

            catch (Exception ex)
            {

                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DeploymentPackageQueue Role", null, ip);


            }
            return base.OnStart();


        }

        string Token;
        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("DeploymentPackageQueue", 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        int tenant = 0;

                        if (response != null && response.MessageId != null)
                        {
                            try
                            {
                                deploymentPackageId = response.MessageValues["DeploymentPackageId"].ToString();
                                int.TryParse(response.MessageValues["Tenant"], out tenant);


                                if (string.IsNullOrEmpty(deploymentPackageId))
                                {
                                    queueservice.Complete();
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(deploymentPackageId))
                                {
                                    deploymentPackageService = new DeploymentPackageWRService(deploymentPackageId);
                                }

                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                #region HandleException
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                if (response.MessageValues.Keys.Contains("Id"))
                                {
                                    string errorMessage = ex.Message + Environment.NewLine;

                                    if (ex.InnerException != null)
                                    {
                                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                                    }

                                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                                    var msg = ex.Message + DateTime.Now;
                                    if (!string.IsNullOrEmpty(deploymentPackageId))
                                    {

                                        if (response.RetryNumber <= 1)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                        }
                                        if (response.RetryNumber >= 3)
                                        {
                                            queueservice.CompleteAsFailed();
                                        }

                                    }
                                    else
                                    {
                                        queueservice.CompleteAsFailed();
                                    }
                                }
                                else
                                {
                                    queueservice.CompleteAsFailed();
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            Thread.Sleep(10000);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Deployment Package Queue worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }



    }
}
