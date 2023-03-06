using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.Helpers.WorkerRole.DeploymentPackages;

namespace CommunicationWorkerRole
{
    public class DeploymentPackageWorkerRole : WorkerEntryPoint
    {

        private DbQueueService queueService;

        private static DateTime startExecuteDate;
        int tenant = 0;
        string deploymentPackageExecutionProgressStatus = "P";
        string deploymentPackageExecutionFailedStatus = "F";

        public DeploymentPackageWorkerRole()
        {
        }
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DeploymentPackageWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();

            new Thread(new ThreadStart(CleanUp)).Start();
            ExceptionHandler.HandleException(new Exception("Deployment Package Worker role started"), DateTime.Now, 0, null, ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);

            return base.OnStart();

        }

        private void ConnectClient()
        {
            try
            {
                queueService = new DbQueueService();
                queueService.InitializeQueue("DeploymentPackageQueue", 0);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Deployment Package execution worker role start", null, null);
            }
        }

        private void CleanUp()
        {
            bool isUpdatedRequired = false;
            DeploymentPackageExecutionLogRepository deploymentPackageExecutionLogRepository = new DeploymentPackageExecutionLogRepository(tenant);
            var deploymentPackageExecutionLogs = deploymentPackageExecutionLogRepository.GetAllDeploymentPackageExecutionLogs().Where(d => d.ExecutedByServerName == System.Environment.MachineName && d.StatusCode == deploymentPackageExecutionProgressStatus && d.StartDate < DateTime.Now).ToList();
            foreach (DeploymentPackageExecutionLog deploymentPackageExecutionLog in deploymentPackageExecutionLogs)
            {
                MarkDeploymentPackageExecutionLogFailed(deploymentPackageExecutionLog, deploymentPackageExecutionLogRepository);
                isUpdatedRequired = true;
            }

            if (!isUpdatedRequired) return;
            deploymentPackageExecutionLogRepository.SubmitChanges();
            ExceptionHandler.HandleException(new Exception("Deployment Package Worker cleaned up all stuck queue messages(" + deploymentPackageExecutionLogs.Count() + ") and convert them to Fail"), DateTime.Now, 0, null, ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
        }

        private void MarkDeploymentPackageExecutionLogFailed(DeploymentPackageExecutionLog deploymentPackageExecutionLog, DeploymentPackageExecutionLogRepository deploymentPackageExecutionLogRepository)
        {
            deploymentPackageExecutionLog.StatusCode = deploymentPackageExecutionFailedStatus;
            deploymentPackageExecutionLog.ExceptionMessage = "The deployment package failed to deploy.Please try again. Server Machine was down";
            deploymentPackageExecutionLogRepository.Update(deploymentPackageExecutionLog);
        }

        public override void Run()
        {
            startExecuteDate = DateTime.Now;
            ExceptionHandler.HandleException(new Exception("Deployment Package Worker role thread start running"), DateTime.Now, 0, null, ThreadedRoleEntryPoint.getWorkerRoleName(), null, System.Environment.MachineName);
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        ExecuteQueue();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Deployment Package execution queue worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }
                else Thread.Sleep(60000);
            }
        }
        private void ExecuteQueue()
        {

            queueService = new DbQueueService("DeploymentPackageQueue", 0);
            var queueResponse = queueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                new DeploymentPackageExecutionService(queueService, queueResponse).ExecuteDeploymentPackageExecutionQueue();
                queueService.Complete();
            }
            else
            {
                Thread.Sleep(10000);
            }
        }


    }
}
