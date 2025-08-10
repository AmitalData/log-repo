using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using System.Data.Entity.Core.Objects;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Web.Util;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.Data.Enums;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.CloseTables;

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
        public List<CardList> vendors;
        public List<GLAccountList> gLAccounts;
        public List<APPayment> cancelledPayments;
        private List<LedgerTransaction> oppositeAccountTransactions;
        public List<TaxDeductionReportLine> deductionLines;
        private TenantPM tenantPM;
        private const string completedStatusCode = "3";
        private HashSet<string> VendorsWithoutVatNumberCache = new HashSet<string>();
        private HashSet<string> VendorsWithoutGLAccountCache = new HashSet<string>();
        int? ReportYear;
        List<CardList> transactionsVendors;
        HashSet<Address> addresses;
        List<GLAccountList> transactionsOppositGLAccounts;
        DateTime? reportMonth;
        TaxDeductionReportPM taxDeductionReport;
        List<GLAccountList> mainGLAccounts;
        List<GLAccountCurrency> accountCurrencies;
        TaxDeductionPerVendorReportParameters taxDeductionPerVendorReportParameters;
        public TaxDeductionReportDataProvider(TaxDeductionReportPM report, int tenant, TaxDeductionPerVendorReportParameters TaxDeductionPerVendorReportParameters)
        {
            Tenant = tenant;
            taxDeductionPerVendorReportParameters = TaxDeductionPerVendorReportParameters;
            ReportYear = report != null ? report.TaxYear : null;

            this.taxDeductionReport = report;
            SetDates();
            reportMonth = report != null ? report.Month : null;
            InitErrorMessage();

            invoiceContext = InvoiceContext.GetContext(tenant);
            commoncontext = CommonDataContext.GetContext(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            vendors = new List<CardList>();
            gLAccounts = new List<GLAccountList>();
            addresses = new HashSet<Address>();
            transactionsOppositGLAccounts = new List<GLAccountList>();
            mainGLAccounts = new List<GLAccountList>();
            accountCurrencies = new List<GLAccountCurrency>();
        }
        private void InitErrorMessage()
        {
            if (taxDeductionReport != null)
            {
                taxDeductionReport.ErrorMessage = null;
            }
        }
        private void SetDates()
        {
            if (taxDeductionPerVendorReportParameters != null)
            {
                startDate = taxDeductionPerVendorReportParameters.FromDate;
                endDate = taxDeductionPerVendorReportParameters.ToDate;
            }
            else
            {
                startDate = taxDeductionReport.ByMonth ? new DateTime((int)taxDeductionReport.TaxYear, (int)taxDeductionReport.Month.Value.Month, 1, 0, 0, 0) : new DateTime((int)taxDeductionReport.TaxYear, 1, 1, 0, 0, 0);
                endDate = taxDeductionReport.ByMonth ? new DateTime((int)taxDeductionReport.TaxYear, (int)taxDeductionReport.Month.Value.Month, DateTime.DaysInMonth((int)taxDeductionReport.TaxYear, taxDeductionReport.Month.Value.Month), 23, 59, 59) : new DateTime((int)taxDeductionReport.TaxYear, 12, 31, 23, 59, 59);

            }
        }
        TaxDeductionReportData taxDeductionReportData;
        public TaxDeductionReportData GetTaxDeductionReportData()
        {
            taxDeductionReportData = new TaxDeductionReportData();
            taxDeductionReportData.TaxYear = ReportYear.ToString();
            tenantPM = GetTenantPM();
            setting = GetFullAccountingPMForTenant();
            taxDeductionReportData.SettingDeductionFileNumber = setting.DeductionFileNumber;
            taxDeductionReportData.deductionLines = GetTaxReportDeductionLines();


            taxDeductionReportData.ByVendorList = FillGroupByVendorList(taxDeductionReportData.deductionLines);
            if (taxDeductionPerVendorReportParameters == null)
            {
                taxDeductionReportData.ByMonthList = FillGroupedByMonthList(taxDeductionReportData.deductionLines, taxDeductionReportData);
                taxDeductionReportData = FillTotalForCompany(taxDeductionReportData.deductionLines, taxDeductionReportData);
            }

            if (taxDeductionPerVendorReportParameters == null && taxDeductionReport.ErrorMessage != null && taxDeductionReport.StatusTypeCode != completedStatusCode)
            {
                throw new ApplicationException(taxDeductionReport.ErrorMessage);
            }

            taxDeductionReportData.FromDate = startDate;
            taxDeductionReportData.ToDate = endDate;
            taxDeductionReportData.TenantAddress1 = tenantPM.InvoiceSection1;
            taxDeductionReportData.TenantAddress2 = tenantPM.InvoiceSection2;
            return taxDeductionReportData;
        }
        private List<TaxDeductionReportLine> GetTaxReportDeductionLines()
        {
            List<TaxDeductionReportLine> transactionDeductionLines;

            if (FeatureToggleHelper.HasFeatureToggle("TXD", Tenant))  
            { 
                transactionDeductionLines = FillTaxDeductionReportUsingCreditLedgerTransactions(); 
            }
            else
            { 
                transactionDeductionLines = FillTaxDeductionReportUsingLedgerTransactions(); 
            }
            List<TaxDeductionReportLine> paymentsDeductionLines = CreateTaxDeductionLinesByApprovedAPPayments();
            List<TaxDeductionReportLine> cancelledPaymentsDeductionLines = CreateTaxDeductionLinesByCancelledAPPayments();
            deductionLines = transactionDeductionLines.Concat(paymentsDeductionLines).Concat(cancelledPaymentsDeductionLines).ToList();
            return deductionLines;
        }


        private string BuildVendorErrorMessage(IEnumerable<APPayment> payments, string paymentOrderText, string vendorErrorText)
        {
            var sb = new StringBuilder();
            foreach (var p in payments)
            {
                sb.AppendLine()
                .Append(paymentOrderText)
                .Append(p.PaymentNo).AppendLine()
                .Append(p.VendorCard.Code)
                .Append(vendorErrorText)
                .AppendLine().AppendLine();
            }
            return sb.ToString();
        }


        private List<TaxDeductionReportLine> CreateTaxDeductionLinesByAPPayments(List<APPayment> payments,bool cancelled) {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            string id = null;
            string paymentOrderText = TextCodesTranslator.TranslateText("TaxDeductionReport.O.PaymentOrder", Tenant);
            var vendorsWithoutGLAccount = payments.Where(d => d.VendorCard != null && d.VendorCard.GLAccountId == null
                                                            && !VendorsWithoutGLAccountCache.Contains(d.VendorCard.Code)).ToHashSet();
            if (vendorsWithoutGLAccount.Any() && taxDeductionReport != null)
            {
                taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "") 
                    + BuildVendorErrorMessage(vendorsWithoutGLAccount, paymentOrderText, TextCodesTranslator.TranslateText("TaxDeductionReport.O.VendorWithoutAccount", Tenant));
                VendorsWithoutGLAccountCache.UnionWith(vendorsWithoutGLAccount.Select(d => d.VendorCard.Code).Distinct());
            }

            var vendorsWithoutVatNumber = payments.Where(d => d.VendorCard != null && d.VendorCard.VatNumber == null 
                                                            && !VendorsWithoutVatNumberCache.Contains(d.VendorCard.Code)).ToHashSet();
            if (vendorsWithoutVatNumber.Any() && taxDeductionReport != null)
            {
                taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "")
                    + BuildVendorErrorMessage(vendorsWithoutVatNumber, paymentOrderText, TextCodesTranslator.TranslateText("TaxDeductionReport.O.CardWithoutVatNumber", Tenant));
                VendorsWithoutVatNumberCache.UnionWith(vendorsWithoutVatNumber.Select(d => d.VendorCard.Code).Distinct());
            }
            foreach (APPayment payment in payments) {

                if (id == payment.Id) continue;
                id = payment.Id;
                TaxDeductionReportLine taxDeductionReportline = new TaxDeductionReportLine();
                taxDeductionReportline.VendorId = payment.VendorCard!= null? payment.VendorCard.GLAccountId: null;
                if (taxDeductionReportline.VendorId == null)
                {
                    if(taxDeductionReport != null) 
                        taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + "הוראת תשלום :" + payment.PaymentNo + "\n" + "הכרטיס התפעולי לא מחובר לכרטיס ההנח\"ש";

                }
                taxDeductionReportline.MonthOfRegisterDate = cancelled? payment.AccountingCancelationDate.Value.Month : payment.RegisterDate.Value.Month;
                taxDeductionReportline.AmountInLocalCurrency = cancelled ?  payment.AmountInLocalCurrency*-1 : payment.AmountInLocalCurrency;
                taxDeductionReportline.AmountInLocalCurrency = Math.Round((double)taxDeductionReportline.AmountInLocalCurrency, 0);
                taxDeductionReportline.TaxDeductionLocalAmount =cancelled ? payment.TaxDeductionLocalAmount*-1 : payment.TaxDeductionLocalAmount;
                taxDeductionReportline.TaxDeductionLocalAmount = Math.Round((decimal)taxDeductionReportline.TaxDeductionLocalAmount, 0);
                taxDeductionReportline.TaxDeductionPercentage = payment.TaxDeductionLocalAmount == 0 ? 0 : (int?) Math.Round((double)(( payment.TaxDeductionLocalAmount /  (decimal)payment.AmountInLocalCurrency))*100,MidpointRounding.AwayFromZero);
                if (payment.VendorCard != null)
                {
                    GLAccountList glaccount = gLAccounts.Where(d => d.Id == payment.VendorCard.GLAccountId).FirstOrDefault();
                    taxDeductionReportline.DeductionType = glaccount != null ? glaccount.DeductionFileTypeCode : null;
                }
                lines.Add(taxDeductionReportline);
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
            payments = GetCancelledPayments();
            return CreateTaxDeductionLinesByAPPayments(payments, true);
        }
        private FullAccountingSettingPM GetFullAccountingPMForTenant()
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(Tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(Tenant);
        }
        List<JournalLine> journalLines;
        public List<LedgerTransaction> GetTransactions()
        {
          
            List<LedgerTransaction> transactions = (from a in accountingContext.LedgerTransactions
                    join j in accountingContext.Journals on a.JournalId equals j.Id
                    join g in accountingContext.GLAccounts on a.OppositeAccountId equals g.Id
                                                    where (EntityFunctions.TruncateTime(a.AccountingDate) >= startDate.Date && EntityFunctions.TruncateTime(a.AccountingDate) <= endDate.Date)
                    && a.Tenant == Tenant &&  a.AccountId == setting.TaxWithholdingGLAccountId
                    && j.ExternalSystem != null && a.LocalAmountDebit == 0
                    &&  g.AccountTypeCode == "3"
                                                    select a).ToList();
            List<string> journalIds = transactions.Select(d => d.JournalId).ToList();
            journalLines = GetJournalLinesByJournalds(journalIds);
            List<string> accountsIds_1 = transactions.Where(d=> d.OppositeAccountId != null).Select(d => d.OppositeAccountId ).ToList();
            var accountsIds = FilterOffClients(accountsIds_1).ToHashSet<string>();
            transactionsVendors = GetVendorsByAccountsIds(accountsIds);          
            oppositeAccountTransactions = GetOppositeTransactions(transactions);

            return transactions;
        }

        public List<LedgerTransaction> GetCreditTransactions()
        {
            string wh = setting.TaxWithholdingGLAccountId;
            if (String.IsNullOrEmpty(wh))
                throw new ApplicationException("Withholding Account not defined in Full Accounting Settings");

            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(accountingContext);

            List<LedgerTransaction> transactions = ledgerTransactionQueryService.GetTransactionsDeduction(wh, startDate, endDate, Tenant);


            var tempAccountsIds = (
                from d in transactions
                join glacurr in accountingContext.GLAccountCurrencies
                    on d.OppositeAccountId equals glacurr.GLAccountId into glacurrGroup
                from glacurrItem in glacurrGroup.DefaultIfEmpty()
                where d.OppositeAccountId != null
                select new { d.OppositeAccountId, glacurrItem?.MainGLAccountId }
            ).ToHashSet();

            var accountsIds = new HashSet<string>(tempAccountsIds.Select(x => x.OppositeAccountId));

            accountsIds.UnionWith(tempAccountsIds.Where(x => x.MainGLAccountId != null).Select(x => x.MainGLAccountId));



            transactionsVendors = GetVendorsByAccountsIds(accountsIds);
            return transactions;
        }


        private List<string> FilterOffClients(List<string> accountIds)
        {
            if (accountIds == null || accountIds.Count == 0)
            {
                return accountIds;
            }

            return accountingContext.GLAccounts
                .Where(a => accountIds.Contains(a.Id)
                            && a.Tenant == Tenant
                            && a.AccountTypeCode != GLAccountTypes.Client) 
                .Select(a => a.Id)
                .ToList();
        }

        private List<JournalLine> GetJournalLinesByJournalds(List<string> journalIds )
        {
            List<JournalLine> journalLines = (from a in accountingContext.JournalLines                                       
                                                    where 
                                                     a.Tenant == Tenant && (EntityFunctions.TruncateTime(a.DocumentDate) >= startDate.Date && EntityFunctions.TruncateTime(a.DocumentDate) <= endDate.Date)
                                                    && a.ActionCode == JournalActionTypes.Debit && journalIds.Contains(a.JournalId)
                                                    select a).ToList();
            return journalLines;
        }

        private List<LedgerTransaction> GetOppositeTransactions(List<LedgerTransaction> transactions)
        {
            List<string> oppositeAccountIds = transactions.Where(d => d.OppositeAccountId != null).Select(d => d.OppositeAccountId).ToList();
            return (from a in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on a.JournalId equals journal.Id
                    where oppositeAccountIds.Contains(a.AccountId)
                    && a.Tenant == Tenant && (EntityFunctions.TruncateTime(a.AccountingDate) >= startDate.Date && EntityFunctions.TruncateTime(a.AccountingDate) <= endDate.Date) && journal.ExternalSystem != null
                    select a).ToList();

        }

        public List<APPayment> GetAPPayments()
        {
            List<APPayment> payments = (from a in invoiceContext.APPayments.Include("VendorCard")
                                        where a.Tenant == Tenant
                                         && (a.RegisterDate >= startDate && a.RegisterDate < endDate)
                                         && !(a.AccountingCancelationDate != null  && a.DontIncludeInDeductionReport == false
                                                && (a.AccountingCancelationDate >= startDate && a.AccountingCancelationDate < endDate
                                                        || a.AccountingCancelationDate.Value.Year == a.RegisterDate.Value.Year && a.AccountingCancelationDate.Value.Month == a.RegisterDate.Value.Month
                                                   )
                                             )
                                         && (a.StatusCode == APPaymentStatusValues.Void 
                                          || a.StatusCode == APPaymentStatusValues.Approved 
                                          || a.StatusCode == APPaymentStatusValues.Closed 
                                          || a.StatusCode == APPaymentStatusValues.Printed)
                                        select a).ToList();
            payments = getAPPaymentsWithGLAccountsAndVendor(payments);
            return payments;
        }

        public List<CardList> GetVendorsByIds(List<APPayment> payments)
        {
            List<string> vendorIds = payments.Select(d => d.VendorId).ToList();
          
            List<CardList> vendors= (from a in commoncontext.Cards 
                                     where vendorIds.Contains(a.Id) && a.CountryCode == "IL"
                    && a.Tenant == Tenant
                    select new CardList()
                    {
                        Id = a.Id,
                        CityName = a.CityName,
                        MainAddressId = a.Address1,
                        GLAccountId = a.GLAccountId,
                        IsAutonomy = a.IsAutonomy,
                        IsInternationalPartner = a.IsInternationalPartner,
                        EnglishName = a.EnglishName,
                        VatNumber = a.VatNumber,
                        LocalName = a.LocalName,
                        Code = a.Code
                    }).ToList();
           
            gLAccounts = gLAccounts.Concat(GetVendorsGLAccounts(vendors)).ToList();
            vendors = GetAccountsVendors(gLAccounts.Select(d => d.Id).ToList()).ToList();
            addresses = addresses.Concat(GetVendorsAddresses(vendors.Select(d => d.Id).ToHashSet())).ToHashSet();
            return vendors;

        }

        private List<CardList> GetAccountsVendors(List<string> accountIds)
        {
            List<CardList> vendors = (from a in commoncontext.Cards
                                      where accountIds.Contains(a.GLAccountId)
                                      && a.Tenant == Tenant && a.CountryCode == "IL"
                                      select new CardList()
                                      {
                                          Id = a.Id,
                                          CityName = a.CityName,
                                          MainAddressId = a.Address1,
                                          GLAccountId = a.GLAccountId,
                                          IsAutonomy = a.IsAutonomy,
                                          IsInternationalPartner = a.IsInternationalPartner,
                                          EnglishName = a.EnglishName,
                                          VatNumber = a.VatNumber,
                                          LocalName = a.LocalName,
                                          Code = a.Code
                                      }).ToList();
            return vendors;
        }

        public List<CardList> GetVendorsByAccountsIds(HashSet<string> accountIds)
        {
            List<CardList> vendors = (from a in commoncontext.Cards
                                      where accountIds.Contains(a.GLAccountId)
                                      && a.Tenant == Tenant 
                                      select new CardList()
                                      {
                                          Id = a.Id,
                                          CityName = a.CityName,
                                          MainAddressId = a.Address1,
                                          GLAccountId = a.GLAccountId,
                                          IsAutonomy = a.IsAutonomy,
                                          IsInternationalPartner = a.IsInternationalPartner,
                                          EnglishName = a.EnglishName,
                                          VatNumber = a.VatNumber,
                                          LocalName = a.LocalName,
                                          Code = a.Code,
                                      }).ToList();
            HashSet<string> vendorIds = vendors.Select(d => d.Id).Distinct().ToHashSet();
            addresses = addresses.Concat(GetVendorsAddresses(vendorIds)).ToHashSet();
            return vendors;

        }
        HashSet<Address> GetVendorsAddresses(HashSet<string> vendorsIds)
        {
            return (from a in commoncontext.Addresses
                    where a.Tenant == Tenant && vendorsIds.Contains(a.CardId)
                    && a.AddressTypeId == "M" select a).ToHashSet();


        }
        public List<GLAccountList> GetVendorsGLAccounts(List<CardList> vendors)
        {
            List<string> glAccountIds = vendors.Select(d => d.GLAccountId).ToList();
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant
                    && glAccountIds.Contains(a.Id)
                   && a.ExcludeFromDeductionReport == false && a.AccountTypeCode == "3"
                    select new GLAccountList()
                    {
                        Id = a.Id,
                        DisplayNumber = a.DisplayNumber,
                        Occupation = a.Occupation,
                        LocalName = a.LocalName,
                        DeductionTypeId = a.AccountingCompanyType != null ? a.AccountingCompanyType.Code : null,
                        DeductionFileTypeCode = a.WithholdingTaxDeductionType != null ? a.WithholdingTaxDeductionType.Code : null,
                        DeductionFileTypeName = a.WithholdingTaxDeductionType != null ? a.WithholdingTaxDeductionType.LocalName : null,
                        AssessingOfficeCode = a.TaxWithholdingAssessOffice != null ? a.TaxWithholdingAssessOffice.Code : null,
                        AssessingOfficeName = a.TaxWithholdingAssessOffice != null ? a.TaxWithholdingAssessOffice.LocalName : null,
                        EnglishName = a.EnglishName,
                        DeductionTypeEnglishName = a.AccountingCompanyType != null ? a.AccountingCompanyType.EnglishName : null,
                        DeductionFileNumber = a.DeductionFileNumber
                    }
                        ).ToList();
        
        }

        public List<GLAccountList> GetTransactionsOppositeGLAccounts(List<LedgerTransaction> transactions)
        {
            List<string> glAccountIds = transactions.Select(d => d.OppositeAccountId).ToList();
            return GetGLAccountsByIds(glAccountIds);
           

        }
        public List<GLAccountList> GetTransactionsGLAccounts(List<LedgerTransaction> transactions) {

            List<string> glAccountIds = transactions.Select(d => d.AccountId).ToList();
            return GetGLAccountsByIds(glAccountIds);


        }
        public List<GLAccountList> GetGLAccountsByIds(List<string> accountIds, bool dontExcludeFromdeductionReport = false)
        {
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant && (a.ExcludeFromDeductionReport == false || dontExcludeFromdeductionReport)
                    && accountIds.Contains(a.Id)
                    select new GLAccountList()
                    {
                        Id = a.Id,
                        DisplayNumber = a.DisplayNumber,
                        Occupation = a.Occupation,
                        LocalName = a.LocalName,
                        DeductionTypeId = a.AccountingCompanyType != null ? a.AccountingCompanyType.Code : null,
                        IsMultiCurrency =a.IsMultiCurrency,
                        DeductionFileTypeCode = a.WithholdingTaxDeductionType != null ? a.WithholdingTaxDeductionType.Code : null,
                        DeductionFileTypeName = a.WithholdingTaxDeductionType != null ? a.WithholdingTaxDeductionType.LocalName : null,
                        AssessingOfficeCode = a.TaxWithholdingAssessOffice != null ? a.TaxWithholdingAssessOffice.Code : null,
                        AssessingOfficeName = a.TaxWithholdingAssessOffice != null ? a.TaxWithholdingAssessOffice.LocalName : null,
                        EnglishName = a.EnglishName,
                        DeductionTypeEnglishName = a.AccountingCompanyType != null ? a.AccountingCompanyType.EnglishName : null,
                        DeductionFileNumber = a.DeductionFileNumber

                    }).ToList();

        }
        List<GLAccountList> transactionsGLAccounts;

        FullAccountingSettingPM setting;
        HashSet<LedgerTransaction> createdLines;
        public List<TaxDeductionReportLine> FillTaxDeductionReportUsingLedgerTransactions()
        {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            transactions = GetTransactions();
            transactionsOppositGLAccounts = GetTransactionsOppositeGLAccounts(transactions);
            accountCurrencies = GetGLAccountCurrenciesForOppositeAccounts(transactionsOppositGLAccounts);
            mainGLAccounts = GetGLAccountsByIds(accountCurrencies.Select(d => d.MainGLAccountId).ToList());
            transactionsGLAccounts = GetTransactionsGLAccounts(transactions);

            
            createdLines = new HashSet<LedgerTransaction>();
            foreach (LedgerTransaction transaction in transactions)
            {
                bool LineCreated = CheckIfLineCreated(transaction);
                if (!LineCreated)
                {
                    createdLines.Add(transaction);
                    TaxDeductionReportLine taxDeductionReportLine = new TaxDeductionReportLine();
                    string vendorId = GetVendorId(transaction);
                    taxDeductionReportLine.VendorId = vendorId;
                    taxDeductionReportLine.MonthOfRegisterDate = transaction.AccountingDate.Month;
                    List<LedgerTransaction> oppositeTransactions = oppositeAccountTransactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).ToList();
                    if (oppositeTransactions != null && oppositeTransactions.Count > 0)
                    {
                        taxDeductionReportLine.AmountInLocalCurrency = Math.Round((double)oppositeTransactions.Sum(d => d.LocalAmountDebit), 0);
                    }
                    List<LedgerTransaction> duplicatedCreditLines = transactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.AccountId && d.OppositeAccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).ToList();
                    if(duplicatedCreditLines != null && duplicatedCreditLines.Count > 0)
                    {
                        transaction.LocalAmountCredit = duplicatedCreditLines.Sum(d => d.LocalAmountCredit);
                    }
                    taxDeductionReportLine.TaxDeductionLocalAmount = Math.Round(transaction.LocalAmountCredit, 0);
                    taxDeductionReportLine.TaxDeductionPercentage = (int?)(transaction.LocalAmountCredit == 0 || taxDeductionReportLine.AmountInLocalCurrency == 0 || taxDeductionReportLine.AmountInLocalCurrency == null ? 0 : Math.Round(((transaction.LocalAmountCredit / (decimal)taxDeductionReportLine.AmountInLocalCurrency)) * 100, 2));
                    GLAccountList account = transactionsOppositGLAccounts.Where(d => d.Id == transaction.OppositeAccountId).FirstOrDefault();
                    taxDeductionReportLine.DeductionType = account != null ? account.DeductionFileTypeCode : null;
                    if (taxDeductionReportLine.VendorId != null) lines.Add(taxDeductionReportLine);
                }
            }
            return lines;
        }

        public List<TaxDeductionReportLine> FillTaxDeductionReportUsingCreditLedgerTransactions()
        {
            var lines = new List<TaxDeductionReportLine>();
            var creditTransactions = GetCreditTransactions();  // by debit bank or tax withholding, opposite acc=vendor
            transactionsGLAccounts = GetTransactionsOppositeGLAccounts(creditTransactions);  // vendor accounts
            accountCurrencies = GetGLAccountCurrenciesForOppositeAccounts(transactionsGLAccounts);
            mainGLAccounts = GetGLAccountsByIds(accountCurrencies.Select(d => d.MainGLAccountId).ToList());

            var groupedCreditTransactions = creditTransactions
                .GroupBy(t => new { t.OppositeAccountId, t.JournalId, t.Reference1 })
                .Select(g => new
                {
                    g.Key.OppositeAccountId,
                    g.Key.JournalId,
                    g.Key.Reference1,
                    Transactions = g.ToList()
                })
                .ToList();

            var processedKeys = new HashSet<string>();
            

            foreach (var grp in groupedCreditTransactions)
            {
                var groupKey = $"{grp.OppositeAccountId}~{grp.JournalId}~{grp.Reference1}";
                if (processedKeys.Contains(groupKey)) 
                    continue;

                processedKeys.Add(groupKey);


                var firstTransaction = grp.Transactions.First();

                var taxDeductionReportLine = new TaxDeductionReportLine
                {
                    VendorId = GetVendorIdFromCreditLine(firstTransaction),
                    MonthOfRegisterDate = firstTransaction.AccountingDate.Month
                };

                var transactionsTaxWhLookup = grp.Transactions.ToLookup(tr => tr.AccountId == setting.TaxWithholdingGLAccountId);

                var whTransactions = transactionsTaxWhLookup[true]; 

                var bankTransactions = transactionsTaxWhLookup[false]; 

                if (whTransactions.Any())
                {
                    taxDeductionReportLine.TaxDeductionLocalAmount = Math.Round(whTransactions.Sum(tr => tr.LocalAmountCredit), 0);

                    if (bankTransactions.Any())
                    {
                        taxDeductionReportLine.AmountInLocalCurrency = ((double)(taxDeductionReportLine.TaxDeductionLocalAmount??0)) + Math.Round((double)bankTransactions.Sum(tr => tr.LocalAmountCredit), 0);
                        taxDeductionReportLine.TaxDeductionPercentage = taxDeductionReportLine.TaxDeductionLocalAmount == 0 || taxDeductionReportLine.AmountInLocalCurrency == 0
                            ? 0
                            : (int?)Math.Round(((taxDeductionReportLine.TaxDeductionLocalAmount ?? 0) / (decimal)taxDeductionReportLine.AmountInLocalCurrency) * 100, 2);
                    }
                    else
                    {
                        taxDeductionReportLine.AmountInLocalCurrency = (double)(taxDeductionReportLine.TaxDeductionLocalAmount ?? 0);
                        taxDeductionReportLine.TaxDeductionPercentage = taxDeductionReportLine.TaxDeductionLocalAmount == 0 || taxDeductionReportLine.AmountInLocalCurrency == 0
                            ? 0
                            : 100;
                    }
                }
                else
                {
                    taxDeductionReportLine.AmountInLocalCurrency = Math.Round((double)grp.Transactions.Sum(tr => tr.LocalAmountCredit), 0);
                    taxDeductionReportLine.TaxDeductionLocalAmount = 0;
                    taxDeductionReportLine.TaxDeductionPercentage = 0;
                }

                var vendorAccount = transactionsGLAccounts.FirstOrDefault(d => d.Id == firstTransaction.OppositeAccountId);
                taxDeductionReportLine.DeductionType = vendorAccount?.DeductionFileTypeCode;

                if (taxDeductionReportLine.VendorId != null)
                    lines.Add(taxDeductionReportLine);
            }

            return lines;
        }

        private bool CheckIfLineCreated(LedgerTransaction transaction)
        {
            LedgerTransaction ledgerTransaction = createdLines.Where(d => d.JournalId == transaction.JournalId && d.OppositeAccountId ==transaction.OppositeAccountId&& d.Reference1 == transaction.Reference1).FirstOrDefault();
            if (ledgerTransaction == null)
            {
                return false;
            }
            else return true;           
        }



        private string GetVendorId(LedgerTransaction transaction)
        {
            CardList vendor = null;
            string vendorId = null;
            if (transaction.OppositeAccountId != null)
            {

                vendor = transactionsVendors.Where(d => d.GLAccountId == transaction.OppositeAccountId).FirstOrDefault();

                if (vendor == null)
                {
                    var debitAccountIds = journalLines.Where(d => d.JournalId == transaction.JournalId && d.Reference1 == transaction.Reference1)
                        .Select(d => d.DebitAccountId).ToHashSet();
                    GLAccountList gLAccount = transactionsGLAccounts.Where(d => d.ChartOfAccountsTypeCode != "5" && debitAccountIds.Contains(d.Id) && d.Id != setting.TaxWithholdingGLAccountId && !d.ExcludeFromDeductionReport).FirstOrDefault();
                    if (gLAccount != null)
                    {
                        vendorId = gLAccount.Id;
                    }
                    else
                    {
                        vendorId = GetVendorIdByMainAccount(transaction.OppositeAccountId);
                    }
                }
                else vendorId = vendor.GLAccountId;
            }
            return vendorId;
        }


        private string GetVendorIdFromCreditLine(LedgerTransaction transaction)
        {
            if (transaction.OppositeAccountId != null)
            {

                // Make a distinct copy of transactionsVendors so only the first item remains per GLAccountId
                var distinctTransactionsVendors = transactionsVendors
                    .GroupBy(v => v.GLAccountId)
                    .Select(g => g.First())
                    .ToList();

                // Then create the dictionary from this filtered list
                var vendorDict = distinctTransactionsVendors.ToDictionary(v => v.GLAccountId);

                vendorDict.TryGetValue(transaction.OppositeAccountId, out var vendor);
                return vendor?.GLAccountId ?? transaction.OppositeAccountId;
            }
            else
                return null;
        }


        private string GetVendorIdByMainAccount(string accountId)
        {
            GLAccountList account = transactionsOppositGLAccounts.Where(d => d.Id == accountId).FirstOrDefault(); 
            if (account == null || account.IsMultiCurrency == true) return null;
            return GetVendorIdForSingleCurrencyAccount(account.Id);

        }
        private string GetVendorIdForSingleCurrencyAccount(string accountId)
        {
            GLAccountCurrency accountCurrency = accountCurrencies.Where(d => d.GLAccountId == accountId).FirstOrDefault();
            if (accountCurrency == null)
            {
                return null;
            }
            return accountCurrency.MainGLAccountId;          
        }
      
        private List<GLAccountCurrency> GetGLAccountCurrenciesForOppositeAccounts(List<GLAccountList> accounts)
        {
            var accountsIds = accounts.Where(d => d.IsMultiCurrency == false).Select(d => d.Id).ToHashSet();
            return (from accountCurrency in accountingContext.GLAccountCurrencies
                    where accountsIds.Contains(accountCurrency.GLAccountId)
                    select accountCurrency).ToList();          
        }

        private List<GLAccountCurrency> GetGLAccountCurrenciesForAccounts(List<GLAccountList> accounts)
        {
            return GetGLAccountCurrenciesForOppositeAccounts(accounts);
        }

        private List<APPayment> getAPPaymentsWithGLAccountsAndVendor(List<APPayment> payments)
        {
            vendors = vendors.Concat(GetVendorsByIds(payments)).ToList();
            payments = (from a in payments
                                 join v in vendors on a.VendorId equals v.Id
                                 join g in gLAccounts.Where(acc => acc.AccountTypeCode != "2") on v.GLAccountId equals g.Id into g
                                 select a).ToList();
            
            return payments;
        }
        private List<APPayment> GetCancelledPayments()
        {

            List<APPayment> cancelledPayments = (from a in invoiceContext.APPayments.Include("VendorCard")
                                                 where a.AccountingCancelationDate >= startDate && a.AccountingCancelationDate < endDate &&
                                                 !(a.RegisterDate >= startDate && a.RegisterDate < endDate)
                                                 && a.Tenant == Tenant
                                                 && !(a.AccountingCancelationDate.Value.Year == a.RegisterDate.Value.Year 
                                                        && a.AccountingCancelationDate.Value.Month == a.RegisterDate.Value.Month
                                                     )
                                                 && a.StatusCode == APPaymentStatusValues.Void && a.DontIncludeInDeductionReport == false
                                                 select a).ToList();
            cancelledPayments = getAPPaymentsWithGLAccountsAndVendor(cancelledPayments);
            return cancelledPayments;

        }
        private List<ByVendorList> FillGroupByVendorList(List<TaxDeductionReportLine> deductionLines) // 60
        {
            List<ByVendorList> byVendorList = new List<ByVendorList>();
            List<TaxDeductionReportLine> groupeddeductionLines = GroupDeductionLinesByVendorAndPercentage(deductionLines);
            List<CardList> mainCards = GetMainAccountsCards();
            vendors = vendors.Concat(transactionsVendors).Concat(mainCards).ToList();
            if (taxDeductionPerVendorReportParameters != null &&!string.IsNullOrWhiteSpace(taxDeductionPerVendorReportParameters.CardId)) {
                vendors = vendors.Where(x => x.Id == taxDeductionPerVendorReportParameters.CardId).ToList();
            }
            gLAccounts = (gLAccounts ?? new List<GLAccountList>())
                            .Concat(transactionsOppositGLAccounts ?? new List<GLAccountList>())
                            .Concat(transactionsGLAccounts ?? new List<GLAccountList>())
                            .Concat(mainGLAccounts ?? new List<GLAccountList>())
                            .Distinct()
                            .ToList();
            if (taxDeductionPerVendorReportParameters != null && taxDeductionPerVendorReportParameters.VendorId != null)
            {
                groupeddeductionLines = groupeddeductionLines.Where(d => d.VendorId == taxDeductionPerVendorReportParameters.VendorId).ToList();
            }



            foreach (TaxDeductionReportLine item in groupeddeductionLines)
            {
                var deductionPercentage = Math.Round((double)((item.TaxDeductionLocalAmount == 0 ? 0 : item.TaxDeductionLocalAmount / (decimal)item.AmountInLocalCurrency) * 100), MidpointRounding.AwayFromZero);
                ByVendorList groupedbyVendor = new ByVendorList()
                {
                    Month = item.MonthOfRegisterDate,
                    TaxDeductionPercentage = (int?)deductionPercentage,
                    VendorId = item.VendorId,
                };
                groupedbyVendor.EndYearBalance = 0;
                GLAccountList gLAccount = gLAccounts.Where(d => d.Id == item.VendorId).FirstOrDefault();

                if (gLAccount != null)
                {
                    string mainGLAccountId = String.Empty;
                    GLAccountList childGLAccount = null;
                    List<CardList> selectedVendors = GetSelectedVendorsList(vendors, item.VendorId, ref mainGLAccountId);
                    if (!String.IsNullOrEmpty(mainGLAccountId))
                    {
                        childGLAccount = gLAccount;
                        gLAccount = gLAccounts.Where(d => d.Id == mainGLAccountId).FirstOrDefault();

                        if (gLAccount == null)
                        {
                            gLAccount = GetOneGLAccountByIds(mainGLAccountId);
                        }
                    }
                    if (taxDeductionPerVendorReportParameters == null) ValidateGLAccountVendors(selectedVendors, gLAccount);
                    groupedbyVendor = SetGLAccountFields(gLAccount, groupedbyVendor, childGLAccount);

                    groupedbyVendor = SetVendorVatNumberAndAddress(selectedVendors, groupedbyVendor); 
                    
                    if (selectedVendors.Count > 0)
                    {
                        groupedbyVendor.VendorName = selectedVendors[0].EnglishName;
                        groupedbyVendor.IsAutonomy = selectedVendors[0].IsAutonomy;
                        groupedbyVendor.IsInternationlPartner = selectedVendors[0].IsInternationalPartner;
                        groupedbyVendor.VendorLocalName = selectedVendors[0].LocalName;
                    }
                        groupedbyVendor.SumOfAmountInLocalCurrency = item.AmountInLocalCurrency.Value;
                        groupedbyVendor.SumOfTaxDeductionLocalAmount = item.TaxDeductionLocalAmount.Value;
                       groupedbyVendor.TotalAmount =(decimal?)groupedbyVendor.SumOfAmountInLocalCurrency;
                    if (groupedbyVendor.SumOfAmountInLocalCurrency > 0 || groupedbyVendor.SumOfAmountInLocalCurrency < 0)
                    {
                        byVendorList.Add(groupedbyVendor);
                    }
                      
                }
                else
                {
                    DeleteVendorFromTaxDeductionReportLines(item.VendorId);
                }

            }

            if (byVendorList.Count() < 1)
            {
                ByVendorList  emptyVendor = new ByVendorList();
                byVendorList.Add(emptyVendor);
            }

 
            var byVendorsGroups = byVendorList
                .GroupBy(x => new { x.DeductionFileNumber, x.VATNumber, x.TaxDeductionPercentage });

            var byVendors = byVendorsGroups
                .Select(g => CombineByVendorItems(g))
                .ToList();

            return byVendors;
        }

        private GLAccountList GetOneGLAccountByIds(string id)
        {
            List<string> gLAccountIds = new List<string>() { id };
            return GetGLAccountsByIds(gLAccountIds, true).FirstOrDefault();
        }

        private ByVendorList CombineByVendorItems(IEnumerable<ByVendorList> gr)
        {
             var grp = gr.ToList();  // Materialization
             
             var combined = grp.First();


            if (grp.Count() > 1)
            {
                // Those variables are there to eliminate update 'in the place' while summing
                double sumOfAmountInLocalCurrency = grp.Sum(x => x.SumOfAmountInLocalCurrency ?? 0);
                combined.SumOfAmountInLocalCurrency = sumOfAmountInLocalCurrency;

                decimal sumOfTaxDeductionLocalAmount = grp.Sum(x => x.SumOfTaxDeductionLocalAmount ?? 0);
                combined.SumOfTaxDeductionLocalAmount = sumOfTaxDeductionLocalAmount;

                decimal totalAmount = grp.Sum(x => x.TotalAmount ?? 0);
                combined.TotalAmount = totalAmount;

                if (grp.Select(x => x.VendorLocalName).Distinct().Count() > 1 &&
                     !string.IsNullOrEmpty(combined.GLAccountLocalName))
                {
                    combined.VendorLocalName = combined.GLAccountLocalName;
                }
            }
            return combined;
         }

        private List<CardList> GetMainAccountsCards()
        {
            var mainAccountsIds = mainGLAccounts.Select(d => d.Id).ToHashSet();
            return GetVendorsByAccountsIds(mainAccountsIds);
        }
        private ByVendorList SetVendorVatNumberAndAddress(List<CardList> selectedVendors, ByVendorList groupedbyVendor)
        {
            string cardsCodes = "";
            if (selectedVendors.Count > 1)
            {

                foreach (CardList item in selectedVendors.OrderBy(d => d.Code))
                {
                    cardsCodes = cardsCodes + item.Code + ",";
                    Address address = addresses.Where(d => d.CardId == item.Id).FirstOrDefault();
                    groupedbyVendor.CardAddress1 = address?.Address1;
                    if (groupedbyVendor.VATNumber == null && item.VatNumber != null)
                    {
                        groupedbyVendor.VATNumber = item.VatNumber;
                    }
                    if (groupedbyVendor.VendorAddress == null || groupedbyVendor.VendorCity == null)
                    {
                        groupedbyVendor.VendorAddress = address != null ? address.Name : null;
                        groupedbyVendor.VendorCity = address != null ? address.City : null;
                    }
                    if (groupedbyVendor.VATNumber != null && address != null)
                    {
                        break;
                    }
                }

            }
            else if (selectedVendors.Count == 1)
            {
                cardsCodes = selectedVendors[0].Code;
                groupedbyVendor.VATNumber = selectedVendors[0].VatNumber;
                Address address = addresses.Where(d => d.CardId == selectedVendors[0].Id).FirstOrDefault();
                groupedbyVendor.VendorAddress = address != null ? address.Name : null;
                groupedbyVendor.VendorCity = address != null ? address.City : null;
                groupedbyVendor.CardAddress1 = address?.Address1;
            }
            if (groupedbyVendor.VATNumber == null)
            {
                groupedbyVendor.VATNumber = GetVendorVatNumberFromMainAccount(groupedbyVendor);
            }

            SetErrorMessage(groupedbyVendor,selectedVendors, cardsCodes);

            return groupedbyVendor;
        }

        private void SetErrorMessage(ByVendorList groupedbyVendor, List<CardList> selectedVendors, string cardsCodes)
        {
            if (groupedbyVendor.VATNumber == null)
            {
                var vendorsWithoutVatNumberCache = selectedVendors.Where(d => d.VatNumber == null
                                                            && !VendorsWithoutVatNumberCache.Contains(d.Code)).ToList();
                if (vendorsWithoutVatNumberCache.Any())
                {
                    taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "") + Environment.NewLine + (vendorsWithoutVatNumberCache.Count > 1
                        ? " Vendor GLAccount " + groupedbyVendor.DisplayNumber + " is connected to more than one Operational Vendor Card and none of them contain a VAT number" + cardsCodes
                        : TextCodesTranslator.TranslateText("TaxDeductionReport.O.CardWithoutVatNumber", Tenant) + ", " + TextCodesTranslator.TranslateText("Card.F.Code", Tenant) + ":" + vendorsWithoutVatNumberCache[0].Code);
                    VendorsWithoutVatNumberCache.UnionWith(vendorsWithoutVatNumberCache.Select(d => d.Code).Distinct());

                }
            }
            if (groupedbyVendor.VendorAddress == null && groupedbyVendor.VendorCity == null)
            {
                taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "") + Environment.NewLine + (selectedVendors.Count > 1 
                    ? " Vendor GLAccount " + groupedbyVendor.DisplayNumber + " is connected to more than one Operational Vendor Card and none of them contain an address" + cardsCodes 
                    : TextCodesTranslator.TranslateText("TaxDeductionReport.O.CardWithoutAddress", Tenant) + ", " + TextCodesTranslator.TranslateText("Card.F.Code", Tenant) + ":" + selectedVendors[0].Code);
            }
        }

        private void DeleteVendorFromTaxDeductionReportLines(string vendorId)
        {
            deductionLines.RemoveAll(line => line.VendorId == vendorId);
        }

        private List<CardList> GetSelectedVendorsList(List<CardList> vendors, string accountId, ref string mainAccountId)
        {
            List<CardList> selectedVendors = new List<CardList>();
            bool hasConnectedVendors = vendors.Where(d => d.GLAccountId == accountId).Any();
            if (hasConnectedVendors)
            {
                selectedVendors = FillAccountVendorsList(vendors, accountId);
            }
            else
            {
                mainAccountId = GetVendorIdForSingleCurrencyAccount(accountId);
                selectedVendors = FillAccountVendorsList(vendors, mainAccountId);
            }
            return selectedVendors;
        }
        private string GetVendorVatNumberFromMainAccount(ByVendorList groupedbyVendor)
        {
            string mainAccountId = GetVendorIdForSingleCurrencyAccount(groupedbyVendor.VendorId);
            var vendorWithVatNumber = vendors.Where(d => d.GLAccountId == mainAccountId && d.VatNumber != null).FirstOrDefault();
            return vendorWithVatNumber != null ? vendorWithVatNumber.VatNumber : null;

        }
        private List<CardList> FillAccountVendorsList(List<CardList> vendors, string accountId)
        {
            List<CardList> selectedVendors = new List<CardList>();
            foreach (CardList vendor in vendors)
            {
                var exist = selectedVendors.Where(d => d.Id == vendor.Id).Any();
                if (!exist)
                {
                    if (vendor.GLAccountId == accountId )
                    {
                        selectedVendors.Add(vendor);
                    }
                }
            }
            return selectedVendors;
        }

        private void ValidateGLAccountVendors(List<CardList> selectedVendors, GLAccountList gLAccount)
        {
            if (selectedVendors.Count() == 0)
            {
                string error= TextCodesTranslator.TranslateText("TaxDeductionReport.O.VendorGLAccount", Tenant) + " " + gLAccount.DisplayNumber + " " + TextCodesTranslator.TranslateText("TaxDeductionReport.O.AccountWithoutVendor", Tenant);

                taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "") + Environment.NewLine + (error);
            }
        }
        public decimal? GetEndYearBalance(string glaccountId)
        {
                decimal? endYearBalance = 0;
                LedgerTransactionBalanceFilter LTBFilter = new LedgerTransactionBalanceFilter();
                LTBFilter.PageSize = 30;
                LTBFilter.PageStartAtRecordIndex = 0;
                LTBFilter.Tenant = Tenant;
                LTBFilter.GLAccountId = glaccountId;
                DateTime today = DateTime.Today;
                LTBFilter.From = startDate;
                LTBFilter.To = endDate;
                LTBFilter.IncludeRelatedCurrenciesAccount = false;
                LTBFilter.IncludeChildAccounts = false;
                LTBFilter.DateTypeCode = "1";
                var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(accountingContext, LTBFilter);
                ledgerTransactionBalanceService.Run();
                LTBFilter.CallBack = new LedgerTransactionBalanceFilterCallBack()
                {
                    EndBalanceLocal = ledgerTransactionBalanceService.Response.EndBalanceLocal,
                };
                endYearBalance = Math.Round((ledgerTransactionBalanceService.Response.EndBalanceLocal != null ? ledgerTransactionBalanceService.Response.EndBalanceLocal : 0).Value, 0);           
            if (endYearBalance >= 0 || endYearBalance == null)
            {
                endYearBalance = 0;
            }
            return endYearBalance*-1;
        }
       private ByVendorList SetGLAccountFields(GLAccountList gLAccount , ByVendorList groupedbyVendor, GLAccountList childGLAccount)
        {
            if (taxDeductionPerVendorReportParameters == null) ValidateGLAccountDeductionTypeFields(gLAccount);
            groupedbyVendor.DisplayNumber = gLAccount.DisplayNumber;

            groupedbyVendor.Occupation = gLAccount.Occupation;
            groupedbyVendor.GLAccountLocalName = gLAccount.DeductionFileTypeCode =="08" ?  gLAccount.EnglishName.ToUpper(): gLAccount.LocalName;
            groupedbyVendor.AssessingOfficerCode = gLAccount.AssessingOfficeCode;
            groupedbyVendor.AssessingOfficerName = gLAccount.AssessingOfficeName;
            groupedbyVendor.DeductionFileTypeCode = gLAccount.DeductionFileTypeCode;
            groupedbyVendor.DeductionFileTypeName = gLAccount.DeductionFileTypeName;
            groupedbyVendor.DeductionFileNumber = gLAccount.DeductionFileNumber;
            groupedbyVendor.DeductionType = gLAccount.DeductionTypeId;
            groupedbyVendor.EnglishName = childGLAccount != null ? childGLAccount.EnglishName : gLAccount.EnglishName;
            if (taxDeductionPerVendorReportParameters == null)
                groupedbyVendor.EndYearBalance = GetEndYearBalance(gLAccount.Id);

            return groupedbyVendor;
        }
        private void ValidateGLAccountDeductionTypeFields(GLAccountList gLAccount)
        {
            if (gLAccount.DeductionFileTypeCode == null)
            {
                GenerateGLAccountRequiredFieldsError("DeductionFileTypeCode", gLAccount);
            }
            if (gLAccount.DeductionTypeId == null)
            {
                GenerateGLAccountRequiredFieldsError("DeductionTypeId", gLAccount);
            }
            if (gLAccount.DeductionFileNumber == null)
            {
                GenerateGLAccountRequiredFieldsError("DeductionFileNumber", gLAccount);
            }
        }
        private void GenerateGLAccountRequiredFieldsError(string Fieldname, GLAccountList gLAccount)
        {
            string missingField = TextCodesTranslator.TranslateText("GLAccounts.O.MissingFieldInAccount", Tenant, true)
                .Replace("{field}", TextCodesTranslator.TranslateText("GLaccount.F." + Fieldname, Tenant, true))
                .Replace("{account}", gLAccount.DisplayNumber);

            taxDeductionReport.ErrorMessage = (taxDeductionReport.ErrorMessage ?? "") + Environment.NewLine + missingField;
        }
        private List<TaxDeductionReportLine> GroupDeductionLinesByVendorAndPercentage(List<TaxDeductionReportLine> deductionLines)
        {

            return (from a in deductionLines
                    group a by
                        new { a.VendorId } into g
                    select new TaxDeductionReportLine
                    {
                        VendorId = g.Key.VendorId,
                        AmountInLocalCurrency = g.Sum(s => s.AmountInLocalCurrency),
                        TaxDeductionLocalAmount = g.Sum(s => s.TaxDeductionLocalAmount)

                    } into s
                    select s).ToList();
        }

        private List<ByMonthList> FillGroupedByMonthList(List<TaxDeductionReportLine> deductionLines, TaxDeductionReportData taxDeduction)
        {
            List<ByMonthList> groupedByMonthLines = new List<ByMonthList>();
            if (taxDeductionReport.ByMonth)
            {
                groupedByMonthLines= FillGroupByMonthData(taxDeductionReport.Month.Value.Month, groupedByMonthLines);
            }
            else
            {
                for (int i = 1; i < 13; i++)
                {
                    var month = i;
                    groupedByMonthLines= FillGroupByMonthData(month, groupedByMonthLines);

                }
            }
            return groupedByMonthLines;
        }
        private List<ByMonthList> FillGroupByMonthData(int month, List<ByMonthList> groupedByMonthLines) // 80
        {
            ByMonthList byMonthList = new ByMonthList()
            {

                Month = month,
                TotalVendors = deductionLines.Where(d => d.MonthOfRegisterDate == month).GroupBy(d => d.VendorId).Count(),
                TotalPaymentsWithoutDivided = deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.AmountInLocalCurrency).Value,
                TotalDeductionsWithoutDivided = deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.TaxDeductionLocalAmount).Value,
                TotalDivided = deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.AmountInLocalCurrency ).Value,
                TotalDeductionsFromDivided = deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.TaxDeductionLocalAmount).Value,
                ReportMonth = month + "." + ReportYear,
            };
            groupedByMonthLines.Add(byMonthList);
            return groupedByMonthLines;

        }
        private TaxDeductionReportData FillTotalForCompany(List<TaxDeductionReportLine> deductionLines, TaxDeductionReportData taxDeduction)
        {          
            taxDeduction.VendorsCount = deductionLines.GroupBy(d => d.VendorId).Count();
            taxDeduction.TotalAmountInLocalCurrency = deductionLines.Sum(d => d.AmountInLocalCurrency ).Value;
            taxDeduction.TotalDeductionInLocalCurrency = taxDeductionReportData.ByMonthList.Sum(d => d.TotalDeductionsWithoutDivided + d.TotalDeductionsFromDivided).Value;
            taxDeduction.TotalAmountInLocalCurrency08 = deductionLines.Where(d => d.DeductionType == "08").Sum(d => d.AmountInLocalCurrency).Value;
            taxDeduction.TotalTaxDeductionInLocalCurrency08 = deductionLines.Where(d => d.DeductionType == "08").Sum(d => d.TaxDeductionLocalAmount).Value;
            if (taxDeduction.ByVendorList != null)
            {
                taxDeduction.TotalEndBalance = Math.Round(taxDeduction.ByVendorList.Sum(d => d.EndYearBalance).Value, 0);
            }
            taxDeduction.TotalForCompany = FillCompanyTotalForPDFReport(taxDeduction);
            return taxDeduction;

        }
        private List<TotalForCompany> FillCompanyTotalForPDFReport(TaxDeductionReportData taxDeduction)
        {
            FullAccountingSettingPM setting = GetFullAccountingPMForTenant();
            tenantPM = GetTenantPM();
            List<TotalForCompany> totals = new List<TotalForCompany>();
            TotalForCompany totalForCompany = new TotalForCompany()
            {
                DeductionFileNumber = setting.DeductionFileNumber,
                CompanyName = tenantPM.Company,
                TotalPayments = taxDeduction.TotalAmountInLocalCurrency,
                TotalDeductions= taxDeduction.TotalDeductionInLocalCurrency,

            };
            totals.Add(totalForCompany);
            return totals;
        }
        private TenantPM GetTenantPM()
        {
            TenantQuery tenantQuery = new TenantQuery(Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(Tenant, false);
            return tenantPM;
        }
    }
}
