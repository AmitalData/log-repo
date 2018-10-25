using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
   public class BatchTaxDeductionReportService: BatchTaskExecutionsService
    {

        public BatchTaxDeductionReportService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PNCFileArgs));
            PNCFileArgs parameterArgs = serializer.Deserialize(stringReader) as PNCFileArgs;

            // Call the service
           DocumentsFilingPM docFilingPM = TaxDeductionReportService.Create856File(parameterArgs.ReportId, parameterArgs.Tenant);

        }
    }
}
