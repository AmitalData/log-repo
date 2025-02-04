using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
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
   public class BatchCreateTaxReportService: BatchTaskExecutionsService
    {

        public BatchCreateTaxReportService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(PNCFileArgs));
            PNCFileArgs parameterArgs = serializer.Deserialize(stringReader) as PNCFileArgs;
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(parameterArgs.Tenant);
            TaxReportPM taxReportPM = taxReportQueryService.GetSingle(parameterArgs.ReportId, false, false);
            IAccountingContext accountingContext = AccountingContext.GetContext(taxReportPM.Tenant);
            TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(accountingContext, new Dictionary<string, IContext>(), taxReportPM.Tenant);
            List<TaxReportLinePM> lines = TaxReportService.CreateTaxReportLines(taxReportPM, parameterArgs.Tenant,parameterArgs.RecalculateData);
            TaxReportService.CalculateReportTotals(taxReportPM, lines);
            taxReportPM.IsEdited = false;
            taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
            taxReportUpdateService.Update(taxReportPM, true);
        }




    }
}
