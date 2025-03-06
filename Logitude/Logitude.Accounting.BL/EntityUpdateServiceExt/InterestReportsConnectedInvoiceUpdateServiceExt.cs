using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.InterestService;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.Enums;

namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class  InterestReportsConnectedInvoiceUpdateServiceExt : IInterestReportsConnectedInvoiceUpdateServiceExt
    {
        public InterestReportsConnectedInvoiceUpdateServiceExt()
        {

        }

        public void UpdateInterestLastBatchService(string ReportId, int Tenant, IAccountingContext MainContext,string ARInvoiceId)
        {
            if (MainContext == null)
            {
                MainContext = AccountingContext.GetContext(Tenant);
            }
            InterestReportsConnectInvoicePM InterestReportsConnectInvoicePM = new InterestReportsConnectInvoicePM();
            InterestReportsConnectInvoicePM.Tenant = Tenant;
            InterestReportsConnectInvoicePM.InvoiceId = ARInvoiceId;
            InterestReportsConnectInvoicePM.ReportId = ReportId;
            InterestReportsConnectInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
            InterestReportsConnectInvoiceUpdateService interestLastBatchServiceUpdateService = new InterestReportsConnectInvoiceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            interestLastBatchServiceUpdateService.Update(InterestReportsConnectInvoicePM,true);
        }
        public bool CheckInterestReportsConnected(string ReportId, int Tenant, IAccountingContext MainContext)
        {
            if (MainContext == null)
            {
                MainContext = AccountingContext.GetContext(Tenant);
            }
            var InterestReportsConnectInvoice = MainContext.InterestReportsConnectInvoices.FirstOrDefault(a => a.ReportId == ReportId && a.Tenant == Tenant);
            var InvoiceCreated = MainContext.InterestReports.FirstOrDefault(a => a.Id == ReportId && a.Tenant == Tenant && a.InterestReportStatusCode == InterestReportStatusCodes.Invoiced);
            return InterestReportsConnectInvoice != null || InvoiceCreated != null;
        }

    }
}
