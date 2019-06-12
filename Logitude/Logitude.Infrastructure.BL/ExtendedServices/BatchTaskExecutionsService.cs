using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                this.ChangeStatus("D");
            }
            catch (Exception ex)
            {
                //log the exception.
                this.ChangeStatus("F",ex);
                //throw; Removed By Rabaia and Mohammad because it causes the WR to crash.
                    
            }
        }

        public virtual void RunCode() { }
        

        public virtual void ChangeStatus(string statusCode,Exception ex=null,string logStatus=null)
        {
            Logitude.Infrastructure.Data.IInfrastructureContext context = Logitude.Infrastructure.Data.InfrastructureContext.GetContext(this.BatchTaskExecution.Tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), this.BatchTaskExecution.Tenant);
            Debug.WriteLine(statusCode+",,inside chagne status");
            BatchTaskExecution.StatusCode = statusCode;
            switch (statusCode)
            {
                case "I":
                    {
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
            Debug.WriteLine(statusCode + ",,inside chagne status after update");
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
    }
}
