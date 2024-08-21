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
using Logitude.Accounting.BL.CoreBL.Batch;
namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class InterestReportUpdateServiceExt : IInterestReportUpdateServiceExt
    {
        public InterestReportUpdateServiceExt()
        {

        }

        public void UpdateConfirmCreateInvoice(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext,string ARInvoiceId=null,string InvoiceNumber=null, double? AmountInLocalCurrency = null, string InvoiceEntitiId = null)
        {
            InterestReportService interestReportService = new InterestReportService();
            interestReportService.PutConfirmCreateInvoice(interestReportPM, Tenant, MainContext,ARInvoiceId,InvoiceNumber, AmountInLocalCurrency, InvoiceEntitiId);
        }
        public void UpdateInterestReportStatus(string Statues, InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext, string ARInvoiceId = null, string InvoiceNumber = null, double? AmountInLocalCurrency = null, string InvoiceEntitiId = null,string CreatedByUserId = null)
        {
          
            InterestReportService interestReportService = new InterestReportService();
            interestReportService.UpdateInterestReportsStatues(Statues,interestReportPM, Tenant, MainContext, ARInvoiceId, InvoiceNumber, AmountInLocalCurrency, InvoiceEntitiId, CreatedByUserId);
        }
        public void CancelledInterestTransactionsByARPayment(string EntityId, int Tenant)
        {
            InterestReportService interestReportService = new InterestReportService();
            interestReportService.CancelInterestTransactionsByARPayment(EntityId, Tenant);
        }
    }
}
