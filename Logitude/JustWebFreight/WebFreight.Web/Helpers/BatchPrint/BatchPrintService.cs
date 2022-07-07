using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrintService
    {
        public string Print(BatchPrintManagerArgs batchPrintManagerArgs)
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
            return batchPrintTask.Id;
        }

        private static string ParsParameters(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            return JsonConvert.SerializeObject(batchPrintManagerArgs);
        }

        private static BatchTaskExecutionPM CreateBatchTask(BatchPrintManagerArgs batchPrintManagerArgs, string xmlParameters)
        {
            return new BatchTaskExecutionPM()
            {
                Subject = $"Multi Print",
                Tenant = batchPrintManagerArgs.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "WebFreight.Web.Helpers.BatchPrint.BatchPrintManager,WebFreight.Web",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",
                ProgressPercentage = 0
            };
        }
    }
}
