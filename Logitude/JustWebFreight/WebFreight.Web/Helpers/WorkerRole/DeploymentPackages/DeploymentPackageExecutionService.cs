using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.Helpers.WorkerRole.Importer;

namespace WebFreight.Web.Helpers.WorkerRole.DeploymentPackages
{
    public class DeploymentPackageExecutionService
    {
        private DbQueueService queueService;
        private QueueResponse queueResponse;
        private int? tenant = null;
        private string deploymentPackageExecutionLogId = string.Empty;
        private DeploymentPackageExecutionLogRepository deploymentPackageExecutionLogRepository = null;
        private DeploymentPackageExecutionLog deploymentPackageExecutionLog = null;
        private DeploymentPackageService deploymentPackageService;
        private IWebFreightContext webFreightContext;
        private DateTime startDate = DateTime.Now;
        public DeploymentPackageExecutionService()
        {

        }

        public DeploymentPackageExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService != null && queueResponse != null)
            {
                deploymentPackageExecutionLogId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("DeploymentPackageExecutionLogId") ? queueResponse.MessageValues["DeploymentPackageExecutionLogId"].ToString() : "";
                tenant = GetTenantValueFromQueueResponse(queueResponse);
            }
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            int? tenant = null;
            if (queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("Tenant"))
            {
                string tenantString = queueResponse.MessageValues["Tenant"].ToString();
                if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
            }
            return tenant;
        }

        public void ExecuteDeploymentPackageExecutionQueue()
        {
            if (queueService == null || queueResponse == null) return;
            try
            {
                StartExecuteDeploymentPackageExecutionQueue();                
            }
            catch (Exception exception)
            {
                HandleExecutingDeploymentPackageExecutionQueueException(exception);
            }

        }

        private void StartExecuteDeploymentPackageExecutionQueue()
        {
            deploymentPackageExecutionLog = GetDocumentsExecutionLog();
            if (deploymentPackageExecutionLog != null && deploymentPackageExecutionLog.RetryNumber < 2 && (deploymentPackageExecutionLog.StatusCode == "W" || deploymentPackageExecutionLog.StatusCode == "P"))
            {
                UpdateDeploymentPackageExecutionLog(new DeploymentPackageExecutionLogArgs() { StartDate = startDate, StatusCode = "P" });
                ImportDeploymentPackage();
                return;
            }
            ExceptionHandler.HandleException(new Exception("Deploying the Imported Deployment Package failed after 3 retries or it reaches the time out.Please try again.If the issue is persistent then please kindly contact our Customer Support"), DateTime.Now, 0, null, "WorkerRole Monitor", null, System.Environment.MachineName);
            UpdateDeploymentPackageExecutionLog(new DeploymentPackageExecutionLogArgs() { Exception = new Exception("Deploying the Imported Deployment Package failed after 3 retries or it reaches the time out.Please try again.If the issue is persistent then please kindly contact our Customer Support"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
            queueService.Complete();
        }

        private void HandleExecutingDeploymentPackageExecutionQueueException(Exception exception)
        {
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Deployment Package execution queue worker role start", null, null);
            ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Deployment Package WorkerRole Monitor|" + "Catch ExecuteDeploymentPackageExecutionQueue", null, System.Environment.MachineName);
            try
            {
                UpdateDeploymentPackageExecutionLog(new DeploymentPackageExecutionLogArgs() { Exception = exception, DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
            }
            catch (Exception ex)
            {

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Deployment Package WorkerRole Monitor|" + "Catch ExecuteDocumentsExecutionQueue", " inside catch exception while running UpdateDeploymentPackageExecutionQueue", System.Environment.MachineName);
            }

            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 250));
        }

        private DeploymentPackageExecutionLog GetDocumentsExecutionLog()
        {
            DeploymentPackageExecutionLog deploymentPackageExecutionLog = null;
            if (string.IsNullOrEmpty(deploymentPackageExecutionLogId) || tenant == null) return deploymentPackageExecutionLog;
            deploymentPackageExecutionLogRepository = new DeploymentPackageExecutionLogRepository((int)tenant);
            deploymentPackageExecutionLog = deploymentPackageExecutionLogRepository.GetSingleDeploymentPackageExecutionLog(deploymentPackageExecutionLogId, (int)tenant);
            return deploymentPackageExecutionLog;
        }

        private void UpdateDeploymentPackageExecutionLog(DeploymentPackageExecutionLogArgs deploymentPackageExecutionLogArgs)
        {
            if (deploymentPackageExecutionLog == null) return;
            deploymentPackageExecutionLog.StatusCode = !string.IsNullOrEmpty(deploymentPackageExecutionLogArgs.StatusCode) ? deploymentPackageExecutionLogArgs.StatusCode : deploymentPackageExecutionLog.StatusCode;
            deploymentPackageExecutionLog.RetryNumber = queueResponse != null ? queueResponse.RetryNumber : deploymentPackageExecutionLog.RetryNumber;
            deploymentPackageExecutionLog.StartDate = deploymentPackageExecutionLogArgs.StartDate != null ? deploymentPackageExecutionLogArgs.StartDate : deploymentPackageExecutionLog.StartDate;
            deploymentPackageExecutionLog.ExceptionMessage = deploymentPackageExecutionLogArgs.Exception != null ? GetFullExceptionMessageFromException(deploymentPackageExecutionLogArgs.Exception) : deploymentPackageExecutionLog.ExceptionMessage;
            deploymentPackageExecutionLog.DoneDate = deploymentPackageExecutionLogArgs.DoneDate != null ? deploymentPackageExecutionLogArgs.DoneDate : deploymentPackageExecutionLog.DoneDate;
            deploymentPackageExecutionLog.ExecutedByServerName = !string.IsNullOrEmpty(System.Environment.MachineName) ? System.Environment.MachineName : deploymentPackageExecutionLog.ExecutedByServerName;
            if (deploymentPackageExecutionLog.RetryNumber >= 2 && deploymentPackageExecutionLog.StatusCode != "D" && deploymentPackageExecutionLogArgs.StatusCode != "P")
            {
                deploymentPackageExecutionLog.StatusCode = "F";
                deploymentPackageExecutionLog.DoneDate = DateTime.Now;
                queueService.CompleteAsFailed();
            }
            deploymentPackageExecutionLogRepository.Update(deploymentPackageExecutionLog);
            deploymentPackageExecutionLogRepository.SubmitChanges();
        }

        private string GetFullExceptionMessageFromException(Exception exception)
        {
            var exceptionMessage = string.Empty;
            if (exception == null) return exceptionMessage;
            exceptionMessage = exception.Message;
            if (exception.InnerException != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
            }
            if (exception.StackTrace != null)
            {
                exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
            }
            return exceptionMessage;
        }

        private void ImportDeploymentPackage()
        {
            DeploymentPackagePM deploymentPackagePM = !string.IsNullOrEmpty(deploymentPackageExecutionLog.RequestXML) ? LogitudeXmlSerializer.DeserializeObject<DeploymentPackagePM>(deploymentPackageExecutionLog.RequestXML) : null;
            if(deploymentPackagePM == null)
            {
                UpdateDeploymentPackageExecutionLog(new DeploymentPackageExecutionLogArgs() { Exception = new Exception("RequestXML is null"), DoneDate = DateTime.Now, StartDate = startDate, StatusCode = "F" });
                queueService.Complete();
                return;
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                StartImportingDeploymentPackage(deploymentPackagePM);
                scope.Complete();
            }

            UpdateDeploymentPackageExecutionLog(new DeploymentPackageExecutionLogArgs() { StatusCode = "D", DoneDate = DateTime.Now });
            queueService.Complete();
        }

        private void StartImportingDeploymentPackage(DeploymentPackagePM deploymentPackagePM)
        {
            webFreightContext = WebFreightContext.GetContext(deploymentPackagePM.Tenant);
            deploymentPackageService = new DeploymentPackageService(webFreightContext, deploymentPackagePM.Tenant);
            deploymentPackageService.Create(deploymentPackagePM, false);
            DeploymentPackageDetails deploymentPackageDetails = new DeploymentPackageExtractDetailsService().ExtractDeploymentPackageDetailsByDocumentId(deploymentPackagePM.DocumentId, deploymentPackagePM.Tenant);
            new DeploymentPackageImporter(deploymentPackageDetails, deploymentPackagePM.Tenant).Run();
        }
    }

    public class DeploymentPackageExecutionLogArgs
    {
        public string ExceptionMessage { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public Exception Exception { get; set; }

    }
}
