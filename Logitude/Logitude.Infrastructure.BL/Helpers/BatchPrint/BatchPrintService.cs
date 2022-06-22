using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Infrastructure.BL.Helpers.BatchPrint
{
    public class BatchPrintService
    {
        public void Print(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            string xmlParameters = ParsParameters(batchPrintManagerArgs);
            BatchTaskExecutionPM batchPrintTask = CreateBatchTask(batchPrintManagerArgs, xmlParameters);
            IInfrastructureContext MyContext = InfrastructureContext.GetContext(batchPrintManagerArgs.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), batchPrintManagerArgs.Tenant);
            bteUpdateService.Update(batchPrintTask, true);

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", batchPrintTask.Id },
                    { "Tenant", batchPrintManagerArgs.Tenant.ToString() }
                }, batchPrintManagerArgs.Tenant);
        }

        private static string ParsParameters(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(BatchPrintManagerArgs));
            serializer.Serialize(stringwriter, batchPrintManagerArgs);
            string xmlParameters = stringwriter.ToString();
            return xmlParameters;
        }

        private static BatchTaskExecutionPM CreateBatchTask(BatchPrintManagerArgs batchPrintManagerArgs, string xmlParameters)
        {
            return new BatchTaskExecutionPM()
            {
                Subject = $"Multi Print",
                Tenant = batchPrintManagerArgs.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Infrastructure.BL.Helpers.BatchPrint.BatchPrintManager,Logitude.Infrastructure.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C"
            };
        }
    }
}
