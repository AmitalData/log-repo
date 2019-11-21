using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
     public  class BatchRevaluationService: BatchTaskExecutionsService
    {
        public BatchRevaluationService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(RevaluationArgs));
            RevaluationArgs parameterArgs = serializer.Deserialize(stringReader) as RevaluationArgs;
            IContext MainContext = AccountingContext.GetContext(parameterArgs.Tenant);

            RevaluationUpdateService revaluationUpdateService = new RevaluationUpdateService(MainContext, new Dictionary<string, IContext>(), parameterArgs.Tenant);

            // Call the service

            RevaluationQueryService revaluationQueryService = new RevaluationQueryService(parameterArgs.Tenant);
            RevaluationPM revaluationPM = revaluationQueryService.GetSingle(parameterArgs.RevalyuationId, false, false);
                try
            {
                RevaluationBatch revaluationBatch = new RevaluationBatch();
                revaluationBatch.RunAllOpenRevaluations(parameterArgs.Tenant);
                //DocumentOutPM documentOutPM = TaxDeductionReportService.CreateDocumentOut(documentType.Id, parameterArgs.ReportId, null, null, table.Id, parameterArgs.Tenant);
                revaluationPM.Status = "2";
                revaluationPM.ChangeSetOp = ChangeSetOperation.Update;
                revaluationUpdateService.Update(revaluationPM, true);
            }

            catch (Exception ex)
            {


                revaluationPM.Status = "1";

                revaluationPM.ChangeSetOp = ChangeSetOperation.Update;
                revaluationUpdateService.Update(revaluationPM, true);
                throw;

            }

        }



    }
}
