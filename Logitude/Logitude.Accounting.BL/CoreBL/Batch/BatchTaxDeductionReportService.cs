using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
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
            IContext MainContext = AccountingContext.GetContext(parameterArgs.Tenant);

            TaxDeductionReportUpdateService taxDeductionReportUpdateService = new TaxDeductionReportUpdateService(MainContext, new Dictionary<string, IContext>(), parameterArgs.Tenant);

            // Call the service

            TaxDeductionReportQueryService taxDeductionReportQueryService = new TaxDeductionReportQueryService(parameterArgs.Tenant);
            TaxDeductionReportPM taxDeductionReportPM = taxDeductionReportQueryService.GetSingle(parameterArgs.ReportId, false, false);
            try
            {
                DocumentsFilingPM docFilingPM = TaxDeductionReportService.Create856File(parameterArgs.ReportId, parameterArgs.Tenant);

                taxDeductionReportPM.StatusTypeCode = "3";
                taxDeductionReportPM.ChangeSetOp = ChangeSetOperation.Update;
                taxDeductionReportUpdateService.Update(taxDeductionReportPM, true);
            }

            catch (Exception ex)
            {


                taxDeductionReportPM.StatusTypeCode = "4";
                taxDeductionReportPM.ErrorMessage = ex.Message;
                taxDeductionReportPM.ChangeSetOp = ChangeSetOperation.Update;
                taxDeductionReportUpdateService.Update(taxDeductionReportPM, true);
                throw;

            }

        }
    }
}
