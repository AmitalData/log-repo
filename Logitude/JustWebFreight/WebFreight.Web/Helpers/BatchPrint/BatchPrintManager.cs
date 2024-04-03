using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrintManager : BatchTaskExecutionsService
    {
        BatchPrintManagerArgs batchPrintManagerArgs;
        BatchTaskExecutionPM batchTaskExecution;
        public BatchPrintManager(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            this.batchTaskExecution = batchTaskExecution;
        }

        public override void RunCode()
        {
            batchPrintManagerArgs = DeserilaizeParameters();
            var batchPrinterArgs = new BatchPrinterArgs(batchPrintManagerArgs, batchTaskExecution);
            var batchPrinter = BatchPrinterFactory.GetBatchPrinter(batchPrinterArgs);
            PrintingResult result = batchPrinter.PrintDocuments();
            batchTaskExecution.PrametersXml = ParsParameters(result);

        }

        private static string ParsParameters(PrintingResult data)
        {
            return JsonConvert.SerializeObject(data);
        }
        private BatchPrintManagerArgs DeserilaizeParameters()
        {
            return JsonConvert.DeserializeObject<BatchPrintManagerArgs>(BatchTaskExecution.PrametersXml);
        }
    }
}
