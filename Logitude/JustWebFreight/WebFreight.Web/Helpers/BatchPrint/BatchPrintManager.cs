using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
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
            this.batchTaskExecution = batchTaskExecution
        }

        public override void RunCode()
        {
            batchPrintManagerArgs = DeserilaizeParameters();
            var batchPrinter = BatchPrinterFactory.GetBatchPrinter(batchPrintManagerArgs);
            var result = batchPrinter.PrintDocuments();
            batchTaskExecution.PrametersXml = ParsParameters(result);

        }




        private static string ParsParameters(object data)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(BatchPrintManagerArgs));
            serializer.Serialize(stringwriter, data);
            string xmlParameters = stringwriter.ToString();
            return xmlParameters;
        }
        private BatchPrintManagerArgs DeserilaizeParameters()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(BatchPrintManagerArgs));
            return serializer.Deserialize(stringReader) as BatchPrintManagerArgs;
        }
    }
}
