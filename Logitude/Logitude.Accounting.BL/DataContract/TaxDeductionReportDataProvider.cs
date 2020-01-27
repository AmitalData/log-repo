using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class TaxDeductionReportDataProvider
    {
        public int Tenant;
        public DateTime startDate;
        public DateTime endDate;
        private IInvoiceContext invoiceContext;
        private Simplog.Data.CommonDataModel.ICommonDataContext commoncontext;
        private IAccountingContext accountingContext;
        public List<LedgerTransaction> transactions;
        public List<APPayment> payments;
        public List<Card> vendors;
        public List<GLAccount> gLAccounts;
        public List<APPayment> cancelledPayments;
        public TaxDeductionReportDataProvider(int tenant, int? reportYear)
        {
            Tenant = tenant;
            startDate = new DateTime((int)reportYear, 1, 1);
            endDate = new DateTime((int)reportYear, 12, 31);
            invoiceContext = InvoiceContext.GetContext(tenant);
            commoncontext = CommonDataContext.GetContext(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
        }
        public TaxDeductionReportData GetTaxDeductionReportData(int? reportYear, int tenant)
        {
            //step1
         //   FillTaxDeductionReportUsingLedgerTransactions();

            //step 2
            payments = GetAPPayments();
     //       vendors = GetPaymentVendors(payments);
            gLAccounts = GetVendorGLAccounts();
            //step3
            cancelledPayments = GetCancelledPayments();


        }

        private FullAccountingSettingPM GetFullAccountingPMForTenant()
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(Tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(Tenant);
        }

        public List<LedgerTransaction> GetTransactions()
        {
            FullAccountingSettingPM setting = GetFullAccountingPMForTenant();
            return (from a in accountingContext.LedgerTransactions
                    join j in accountingContext.Journals on a.JournalId equals j.Id
                    where (a.DocumentDate >= startDate && a.DocumentDate <= endDate)
                    && a.Tenant == Tenant
                    && setting.TaxWithholdingGLAccountId == a.AccountId && j.ExternalSystem != null
                    select a).ToList();
        }

        public List<APPayment> GetAPPayments() {
            List<APPayment> payments= (from a in invoiceContext.APPayments
                    where a.Tenant == Tenant
                     && (a.RegisterDate >= startDate && a.RegisterDate < endDate)
                     && (a.StatusCode == "AD" || a.StatusCode == "CL" || a.StatusCode == "PR")
                    select a).ToList();
          payments=  getAPPaymentsWithGLAccountsAndVendor(payments);
            return payments;
        }

        public List<Card> GetPaymentVendors(List<APPayment> payments)
        {
            List<string> vendorIds = payments.Select(d => d.VendorId).ToList();
            return (from a in commoncontext.Cards
                    where vendorIds.Contains(a.Id) && a.CountryCode == "IL"
                    select a).ToList();
        }
        public List<GLAccount> GetVendorGLAccounts()
        {
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant
                    && a.ExcludeFromDeductionReport == false && a.AccountTypeCode == "3"
                    select a).ToList();

        }
        //public TaxDeductionReportData FillTaxDeductionReportUsingLedgerTransactions()
        //{
        //    transactions = GetTransactions();
        //    foreach (LedgerTransaction transaction in transactions)
        //    {



        //    }
        //}

        private List<APPayment> getAPPaymentsWithGLAccountsAndVendor(List<APPayment> payments)
        {
            vendors = GetPaymentVendors(payments);
            payments = (from a in payments
                                 join v in vendors on a.VendorId equals v.Id
                                 join g in gLAccounts on v.GLAccountId equals g.Id
                                 select a).ToList();
            return payments;
        }
        private List<APPayment> GetCancelledPayments()
        {

            List<APPayment> cancelledPayments = (from a in invoiceContext.APPayments
                                                 where a.AccountingCancelationDate >= startDate && a.AccountingCancelationDate < endDate
                                                 && (a.StatusCode == "VD" && a.AccountingCancelationDate.Value.Year != a.RegisterDate.Value.Year && a.DontIncludeInDeductionReport == false)
                                                 select a).ToList();
            cancelledPayments = getAPPaymentsWithGLAccountsAndVendor(cancelledPayments);
            return cancelledPayments;

        }
    }
}
