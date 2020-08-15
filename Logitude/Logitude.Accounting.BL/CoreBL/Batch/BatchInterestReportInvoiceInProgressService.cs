using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchInterestReportInvoiceInProgressService : BatchTaskExecutionsService
    {
         
        public BatchInterestReportInvoiceInProgressService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            
        }

        public override void RunCode()
        {
            InterestReportArguments interestReportArgs = GetInterestReportArgs();
            if (interestReportArgs.AllSelected)
            {
                UpdateStatusForALLNotInvoicedInterestReports(interestReportArgs, interestReportArgs.Tenant);
            }
            else
            {
                UpdateStatusForSelectedInterestReport(interestReportArgs, interestReportArgs.Tenant);
            }
        }

        private InterestReportArguments GetInterestReportArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportArguments));
            InterestReportArguments interestReportArgs = serializer.Deserialize(stringReader) as InterestReportArguments;
            return interestReportArgs;
        }

        private void UpdateStatusForSelectedInterestReport(InterestReportArguments interestReportArgs, int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            List<InterestReportPM> interestReports = interestReportQueryService.GetInterestReportsByIds(interestReportArgs.SelectedIds, tenant);
            UpdateInterestReports(interestReports, tenant);

        }
        private void UpdateInterestReports(List<InterestReportPM> interestReports, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            foreach (InterestReportPM report in interestReports)
            {
                report.InterestReportStatusCode = "8";
                report.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                report.IsUpdatedFromBatch = true;
                service.Update(report, true);
            }
        }

        private void UpdateStatusForALLNotInvoicedInterestReports(InterestReportArguments interestReportArgs, int tenant)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);

            List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArgs.FromDate, interestReportArgs.ToDate, tenant);

            if (interestReportArgs.ExcludedIds != null)
            {
                interestReports = (from a in interestReports
                                   where !interestReportArgs.ExcludedIds.Contains(a.Id)
                                   select a).ToList();
            }
            UpdateInterestReports(interestReports, tenant);
        }
    }
}
