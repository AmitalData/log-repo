using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
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

namespace Logitude.Accounting.BL.CoreBL
{
    public class RevaluationService
    {

        public static BatchTaskExecutionPM CreateRevaluationInBatch(string revaluationId, int tenant)
        {
            BatchTaskExecutionPM taskExe;
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                RevaluationArgs args = new RevaluationArgs() { RevalyuationId = revaluationId, Tenant = tenant };
                string xmlParameters=  GetXMLParameters(args);
                taskExe = CreateBatchTaskExecutionPM(xmlParameters, tenant);
                SendToQueue(taskExe);
                scope.Complete();
            }

            return taskExe;
        }
        private static string GetXMLParameters(RevaluationArgs args) {

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(RevaluationArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();
            return xmlParameters;
        }

        private static BatchTaskExecutionPM CreateBatchTaskExecutionPM(string xmlParameters, int tenant)
        {
            BatchTaskExecutionPM taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Update Revaluation Status",
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchRevaluationService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };
            InsertBatchTaskExcutionPM(taskExe);
            return taskExe;

        }

        private static void InsertBatchTaskExcutionPM(BatchTaskExecutionPM taskExecution)
        {
            IInfrastructureContext MyContext = InfrastructureContext.GetContext(taskExecution.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), taskExecution.Tenant);
            bteUpdateService.Update(taskExecution, true);


        }

        private static void SendToQueue(BatchTaskExecutionPM taskExecution)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExecution.Id },
                    { "Tenant", taskExecution.Tenant.ToString() }
                });

        }



    }

    public class RevaluationArgs
    {
        public int Tenant { get; set; }
        public string RevalyuationId { get; set; }
    }





    
}
