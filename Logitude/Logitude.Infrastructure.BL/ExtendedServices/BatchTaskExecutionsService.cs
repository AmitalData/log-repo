using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Infrastructure.BL.ExtendedServices
{
    public abstract class BatchTaskExecutionsService
    {
        public BatchTaskExecutionPM BatchTaskExecution;
        public BatchTaskExecutionsService(BatchTaskExecutionPM batchTaskExecution)
        {
            this.BatchTaskExecution = batchTaskExecution;
        }

        public void Execute()
        {
            try
            {
                // status will change from Created to In Progress and update the start date time.
                this.ChangeStatus("I");

                // the code will run from the child class.
                this.RunCode();

                // status will change to Done and update the done date time.
                try
                {
                    this.ChangeStatus("D");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Tenant {BatchTaskExecution.Tenant} - Failed to change status to DONE in BatchTaskExecutionsService: {ex.Message}";
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(errorMessage);
                    this.ChangeStatus("D", null, errorMessage);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    this.ChangeStatus("F", ex);
                }
                catch (Exception otherEx)
                {
                    string errorMessage = $"Tenant {BatchTaskExecution.Tenant} - Failed to change status to FAILED in BatchTaskExecutionsService: {otherEx.Message}";
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(errorMessage);
                    this.ChangeStatus("F", null, errorMessage);
                }
            }
        }

        public abstract void RunCode();
        

        public virtual void ChangeStatus(string statusCode,Exception ex=null,string logStatus=null)
        {
            Logitude.Infrastructure.Data.IInfrastructureContext context = Logitude.Infrastructure.Data.InfrastructureContext.GetContext(this.BatchTaskExecution.Tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), this.BatchTaskExecution.Tenant);
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(statusCode+",,inside chagne status");
            BatchTaskExecution.StatusCode = statusCode;
            switch (statusCode)
            {
                case "I":
                    {
                        BatchTaskExecution.ProgressPercentage = 1;
                        BatchTaskExecution.StartDateTime = GetCurrentDateTime(BatchTaskExecution.Tenant);
                        break;
                    }
                case "D":
                    {
                        // not sure about the failed.
                        BatchTaskExecution.ProgressPercentage = 100;
                        BatchTaskExecution.DoneDateTime = GetCurrentDateTime(BatchTaskExecution.Tenant);
                        break;
                    }
                case "F":
                    {
                        // not sure about the failed.
                        BatchTaskExecution.DoneDateTime = GetCurrentDateTime(BatchTaskExecution.Tenant);
                        break;
                    }
            }

            if (ex != null)
            {
                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }

                BatchTaskExecution.ErrorLog = errorMessage;
                BatchTaskExecution.CallStack = ex.StackTrace;
            }
            else if (!string.IsNullOrEmpty(logStatus))
            {
                BatchTaskExecution.ErrorLog = logStatus;
            }
            BatchTaskExecution.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            batchTaskExecutionUpdateService.Update(BatchTaskExecution, true);
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(statusCode + ",,inside chagne status after update");
        }

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }

        public virtual void UpdateProcessPercentage()
        {

        }

        public virtual void UpdateProcessMessage()
        {

        }

        public  string CreateQBatchTaskExecution<TServiceArg>(TServiceArg args4PrametersXml, int tenant, string Subject ,bool delay2Min)
            where TServiceArg : class
        {




            var assemblyQualifiedName = this.GetType().AssemblyQualifiedName;
            assemblyQualifiedName = string.Join(",", assemblyQualifiedName.Split(',').Take(2).ToList());



            var stringwriter = new System.IO.StringWriter();
            var serializer1 = new XmlSerializer(typeof(TServiceArg));
            serializer1.Serialize(stringwriter, args4PrametersXml);
            string xmlParameters = stringwriter.ToString();

            BatchTaskExecutionPM taskExe = null;
            taskExe = new BatchTaskExecutionPM()
            {
                Subject = Subject,//$"CreateBatchFunctionalTestTask({args4PrametersXml.MyState.ToString()})",
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = assemblyQualifiedName,//"Logitude.Accounting.BL.CoreBL.Batch.BatchAccFunctionalTestTask,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",// wtf is "c" no alternative 

            };



            var MyContext = InfrastructureContext.GetContext(tenant);
            var bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            bteUpdateService.Update(taskExe, true);



            // 2- Send to queue
            var queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            TimeSpan? myTimeSpan = null;
            if (delay2Min) { myTimeSpan = TimeSpan.FromMinutes(2); }

            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                }, tenant, myTimeSpan);
            LogMessagingUtil.Instance.AppendLine("CreateQBatchTaskExecution:taskExe.Id:" + taskExe.Id);
            return taskExe.Id;
        }

        public T GetArgs<T>()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T args = (T)serializer.Deserialize(stringReader);
            return args;
        }
    }
}
