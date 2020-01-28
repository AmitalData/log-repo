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
        private List<LedgerTransaction> oppositeAccountTransactions;
        public List<TaxDeductionReportLine> deductionLines;
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
            TaxDeductionReportData taxDeductionReport = new TaxDeductionReportData();
            //step1
            List<TaxDeductionReportLine> transactionDeductionLines =  FillTaxDeductionReportUsingLedgerTransactions();
            //step 2
            List<TaxDeductionReportLine> paymentsDeductionLines = CreateTaxDeductionLinesByApprovedAPPayments();          
            //step3
            List<TaxDeductionReportLine> cancelledPaymentsDeductionLines = CreateTaxDeductionLinesByCancelledAPPayments();
            deductionLines = transactionDeductionLines.Concat(paymentsDeductionLines).Concat(cancelledPaymentsDeductionLines).ToList();
            taxDeductionReport.deductionLines = deductionLines;
            return taxDeductionReport;
        }

        private List<TaxDeductionReportLine> CreateTaxDeductionLinesByAPPayments(List<APPayment> payments,bool cancelled) {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            foreach (APPayment payment in payments) {
                TaxDeductionReportLine taxDeductionReportParams = new TaxDeductionReportLine();
                taxDeductionReportParams.VendorId = payment.VendorId;
                taxDeductionReportParams.MonthOfRegisterDate = payment.RegisterDate.Value.Month;
                taxDeductionReportParams.AmountInLocalCurrency = cancelled ?  payment.AmountInLocalCurrency*-1 : payment.AmountInLocalCurrency;//.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).FirstOrDefault().LocalAmountCredit;
                taxDeductionReportParams.TaxDeductionLocalAmount =cancelled ? payment.TaxDeductionLocalAmount*-1 : payment.TaxDeductionLocalAmount;
                taxDeductionReportParams.TaxDeductionPercentage = payment.TaxDeductionPercentage;//.LocalAmountCredit == 0 ? 0 : (transaction.LocalAmountCredit / (transaction.LocalAmountCredit * 2));
                taxDeductionReportParams.DeductionType = gLAccounts.Where(d => d.Id == payment.VendorCard.GLAccountId).FirstOrDefault().DeductionTypeId;
            }
            return lines;
        }
        private List<TaxDeductionReportLine> CreateTaxDeductionLinesByApprovedAPPayments()
        {
            payments = GetAPPayments();
            return CreateTaxDeductionLinesByAPPayments(payments, false);
        }
        private List<TaxDeductionReportLine> CreateTaxDeductionLinesByCancelledAPPayments()
        {
            payments = GetAPPayments();
            return CreateTaxDeductionLinesByAPPayments(payments, true);
        }
        private FullAccountingSettingPM GetFullAccountingPMForTenant()
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(Tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(Tenant);
        }

        public List<LedgerTransaction> GetTransactions()
        {
            FullAccountingSettingPM setting = GetFullAccountingPMForTenant();
            List<LedgerTransaction> transactions = (from a in accountingContext.LedgerTransactions
                    join j in accountingContext.Journals on a.JournalId equals j.Id
                    where (a.DocumentDate >= startDate && a.DocumentDate <= endDate)
                    && a.Tenant == Tenant
                    && setting.TaxWithholdingGLAccountId == a.AccountId && j.ExternalSystem != null
                    select a).ToList();
            oppositeAccountTransactions = GetOppositeTransactions(transactions);
            return transactions;
        }
        private List<LedgerTransaction> GetOppositeTransactions(List<LedgerTransaction> transactions)
        {
            List<string> accountIds = transactions.Select(d => d.AccountId).ToList();
            return (from a in accountingContext.LedgerTransactions                  
                    where accountIds.Contains(a.OppositeAccountId)
                    && a.Tenant == Tenant                  
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
            List<Card> vendors= (from a in commoncontext.Cards
                    where vendorIds.Contains(a.Id) && a.CountryCode == "IL"
                    select a).ToList();
            gLAccounts = GetVendorsGLAccounts(vendors);
            return vendors;

        }
        public List<GLAccount> GetVendorsGLAccounts(List<Card> vendors)
        {
            List<string> glAccountIds = vendors.Select(d => d.GLAccountId).ToList();
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant
                    && glAccountIds.Contains(a.Id)
                    && a.ExcludeFromDeductionReport == false && a.AccountTypeCode == "3"
                    select a).ToList();

        }
        public List<TaxDeductionReportLine> FillTaxDeductionReportUsingLedgerTransactions()
        {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            transactions = GetTransactions();
            foreach (LedgerTransaction transaction in transactions)
            {
                TaxDeductionReportLine taxDeductionReportParams = new TaxDeductionReportLine();
                taxDeductionReportParams.VendorId = transaction.OppositeAccountId;
                taxDeductionReportParams.MonthOfRegisterDate = transaction.DocumentDate.Month;
                taxDeductionReportParams.AmountInLocalCurrency =(double?) oppositeAccountTransactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).FirstOrDefault().LocalAmountCredit;
                taxDeductionReportParams.TaxDeductionLocalAmount = transaction.LocalAmountCredit;
                taxDeductionReportParams.TaxDeductionPercentage =(int?)( transaction.LocalAmountCredit == 0 ? 0 : (transaction.LocalAmountCredit / (transaction.LocalAmountCredit * 2)));
                taxDeductionReportParams.DeductionType = gLAccounts.Where(d => d.Id == transaction.AccountId).FirstOrDefault().DeductionTypeId;
            }
            return lines;
        }

        private List<APPayment> getAPPaymentsWithGLAccountsAndVendor(List<APPayment> payments)
        {
            vendors = GetPaymentVendors(payments);
            gLAccounts = GetVendorsGLAccounts(vendors);
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
