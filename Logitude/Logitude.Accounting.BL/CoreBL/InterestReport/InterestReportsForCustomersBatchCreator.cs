using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportsForCustomersBatchCreator
    {
        private InterestLastBatchServicePM interestLastBatchService;
        public string CreateBatchTaskExecution(InterestReportsCreationForCustomersBatchArgs args)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                bool exists = CheckIfAnotherBatchIsStillInProgress(args.Tenant);
                string batchTaskId = null;
                if (!exists)
                {
                    batchTaskId = Create(args);
                    UpdateOrCreateInterestLastBatch(args.Tenant, batchTaskId);
                }
                else
                {
                    ContactPM contact = GetLoggedContact(args.Tenant);
                    bool showLocals = !contact.DontShowLocal;
                    throw new ApplicationException(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchReportStillInProgress", args.Tenant, showLocals));
                }

                scope.Complete();
                return batchTaskId;
            }
              
        }
        private bool CheckIfAnotherBatchIsStillInProgress(int tenant)
        {
            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
            interestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);
            
            bool exists = false;
            if (interestLastBatchService != null && !string.IsNullOrEmpty(interestLastBatchService.CreateReportsBatchId)) 
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(interestLastBatchService.CreateReportsBatchId, false, false);
                if (batchTaskExecutionPM != null && batchTaskExecutionPM.StatusCode != "D" && batchTaskExecutionPM.StatusCode != "F")
                {
                    exists = true;
                }
            }
            
            return exists;
        }

        private string Create(InterestReportsCreationForCustomersBatchArgs args) 
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportsCreationForCustomersBatchArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Interest Reports Creation In Batch",
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportsForEligibleCustomersCreationService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);



            return taskExe.Id;
        } 
        private void UpdateOrCreateInterestLastBatch(int tenant,string batchTaskId)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            InterestLastBatchServiceUpdateService interestLastBatchServiceUpdateService = new InterestLastBatchServiceUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            if (interestLastBatchService != null)
            {
                interestLastBatchService.CreateReportsBatchId = batchTaskId;
                interestLastBatchService.ChangeSetOp = ChangeSetOperation.Update;
            }
            else
            {
                interestLastBatchService = new InterestLastBatchServicePM();
                interestLastBatchService.Tenant = tenant;
                interestLastBatchService.CreateReportsBatchId = batchTaskId;
                interestLastBatchService.ChangeSetOp = ChangeSetOperation.Insert;
            }
            interestLastBatchServiceUpdateService.Update(interestLastBatchService, true);
        }
        private ContactPM GetLoggedContact(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
    }
}
