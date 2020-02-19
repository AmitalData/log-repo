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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

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
        int? ReportYear;
        List<CardList> transactionsVendors;
        List<Address> addresses;
        List<GLAccountList> transactionsGLAccounts;
        public TaxDeductionReportDataProvider(int? reportYear,int tenant)
        {
            Tenant = tenant;
            ReportYear = reportYear;
            startDate = new DateTime((int)reportYear, 1, 1);
            endDate = new DateTime((int)reportYear, 12, 31);
            invoiceContext = InvoiceContext.GetContext(tenant);
            commoncontext = CommonDataContext.GetContext(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            vendors = new List<CardList>();
            gLAccounts = new List<GLAccountList>();
            addresses = new List<Address>();
            transactionsGLAccounts = new List<GLAccountList>();
        }
        public TaxDeductionReportData GetTaxDeductionReportData()
        {            
            TaxDeductionReportData taxDeductionReport = new TaxDeductionReportData();
            taxDeductionReport.deductionLines = GetTaxReportDeductionLines();
           
            taxDeductionReport.ByVendorList = FillGroupByVendorList(taxDeductionReport.deductionLines);
            taxDeductionReport.ByMonthList = FillGroupedByMonthList(taxDeductionReport.deductionLines);
            taxDeductionReport = FillTotalForCompany(taxDeductionReport.deductionLines, taxDeductionReport);
            return taxDeductionReport;
        }
        private List<TaxDeductionReportLine> GetTaxReportDeductionLines()
        {
            List<TaxDeductionReportLine> transactionDeductionLines = FillTaxDeductionReportUsingLedgerTransactions();
            List<TaxDeductionReportLine> paymentsDeductionLines = CreateTaxDeductionLinesByApprovedAPPayments();
            List<TaxDeductionReportLine> cancelledPaymentsDeductionLines = CreateTaxDeductionLinesByCancelledAPPayments();
            deductionLines = transactionDeductionLines.Concat(paymentsDeductionLines).Concat(cancelledPaymentsDeductionLines).ToList();
            return deductionLines;
        }
        private List<TaxDeductionReportLine> CreateTaxDeductionLinesByAPPayments(List<APPayment> payments,bool cancelled) {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            foreach (APPayment payment in payments) {
                TaxDeductionReportLine taxDeductionReportline = new TaxDeductionReportLine();
                taxDeductionReportline.VendorId = payment.VendorId;
                taxDeductionReportline.MonthOfRegisterDate = cancelled? payment.AccountingCancelationDate.Value.Month : payment.RegisterDate.Value.Month;
                taxDeductionReportline.AmountInLocalCurrency = cancelled ?  payment.AmountInLocalCurrency*-1 : payment.AmountInLocalCurrency;
                taxDeductionReportline.TaxDeductionLocalAmount =cancelled ? payment.TaxDeductionLocalAmount*-1 : payment.TaxDeductionLocalAmount;
                taxDeductionReportline.TaxDeductionPercentage = payment.TaxDeductionPercentage;
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

        public List<LedgerTransaction> GetTransactions()
        {
            FullAccountingSettingPM setting = GetFullAccountingPMForTenant();
            List<LedgerTransaction> transactions = (from a in accountingContext.LedgerTransactions
                    join j in accountingContext.Journals on a.JournalId equals j.Id
                    where (a.DocumentDate >= startDate && a.DocumentDate <= endDate)
                    && a.Tenant == Tenant
                    && setting.TaxWithholdingGLAccountId == a.AccountId && j.ExternalSystem != null
                    select a).ToList();
            List<string> accountsIds = transactions.Select(d => d.OppositeAccountId ).ToList(); 
            transactionsVendors = GetVendorsByAccountsIds(accountsIds);          
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
            List<APPayment> payments= (from a in invoiceContext.APPayments.Include("VendorCard")
                    where a.Tenant == Tenant
                     && (a.RegisterDate >= startDate && a.RegisterDate < endDate)
                     && (a.StatusCode == "AD" || a.StatusCode == "CL" || a.StatusCode == "PR")
                    select a).ToList();
          payments=  getAPPaymentsWithGLAccountsAndVendor(payments);
            return payments;
        }

        public List<CardList> GetVendorsByIds(List<APPayment> payments)
        {
            List<string> vendorIds = payments.Select(d => d.VendorId).ToList();
            addresses = addresses.Concat(GetVendorsAddresses(vendorIds)).ToList();
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
                        LocalName = a.LocalName

                    }).ToList();
            gLAccounts = gLAccounts.Concat(GetVendorsGLAccounts(vendors)).ToList();
            return vendors;

        }

        public List<CardList> GetVendorsByAccountsIds(List<string> accountIds)
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
                                          LocalName = a.LocalName

                                      }).ToList();
            List<string> vendorIds = vendors.Select(d => d.Id).ToList();
            addresses =addresses.Concat(GetVendorsAddresses(vendorIds)).ToList();
            return vendors;

        }
        List<Address> GetVendorsAddresses(List<string> vendorsIds)
        {
            return (from a in commoncontext.Addresses
                    where a.Tenant == Tenant && vendorsIds.Contains(a.CardId)
                    && a.AddressTypeId == "M" select a).ToList();


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

        public List<GLAccountList> GetTransactionsGLAccounts(List<LedgerTransaction> transactions)
        {
            List<string> glAccountIds = transactions.Select(d => d.OppositeAccountId).ToList();
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant
                    && glAccountIds.Contains(a.Id)                 
                    select new GLAccountList() {
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




                    }).ToList();

        }
        public List<TaxDeductionReportLine> FillTaxDeductionReportUsingLedgerTransactions()
        {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            transactions = GetTransactions();
            transactionsGLAccounts = GetTransactionsGLAccounts(transactions);
            foreach (LedgerTransaction transaction in transactions)
            {
                TaxDeductionReportLine taxDeductionReportLine = new TaxDeductionReportLine();
                CardList vendor = transactionsVendors.Where(d => d.GLAccountId == transaction.OppositeAccountId).FirstOrDefault();
                taxDeductionReportLine.VendorId = vendor!= null? vendor.Id :null;
                taxDeductionReportLine.MonthOfRegisterDate = transaction.DocumentDate.Month;
                LedgerTransaction oppositeTransaction = oppositeAccountTransactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).FirstOrDefault();
                taxDeductionReportLine.AmountInLocalCurrency = oppositeTransaction != null ? (double?)oppositeTransaction.LocalAmountCredit: 0;
                taxDeductionReportLine.TaxDeductionLocalAmount = transaction.LocalAmountCredit;
                taxDeductionReportLine.TaxDeductionPercentage =(int?)( transaction.LocalAmountCredit == 0 ? 0 : (transaction.LocalAmountCredit / (transaction.LocalAmountCredit * 2)));
                GLAccountList account = transactionsGLAccounts.Where(d => d.Id == transaction.OppositeAccountId).FirstOrDefault();
                taxDeductionReportLine.DeductionType = account != null ? account.DeductionFileTypeCode : null;
                lines.Add(taxDeductionReportLine);
              }
            return lines;
        }

        private List<APPayment> getAPPaymentsWithGLAccountsAndVendor(List<APPayment> payments)
        {
            vendors = vendors.Concat(GetVendorsByIds(payments)).ToList();
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
                                                 && a.Tenant == Tenant
                                                 && (a.StatusCode == "VD" && a.AccountingCancelationDate.Value.Year != a.RegisterDate.Value.Year && a.DontIncludeInDeductionReport == false)
                                                 select a).ToList();
            cancelledPayments = getAPPaymentsWithGLAccountsAndVendor(cancelledPayments);
            return cancelledPayments;

        }
        private List<ByVendorList> FillGroupByVendorList(List<TaxDeductionReportLine> deductionLines)
        {
            List<ByVendorList> byVendorList = new List<ByVendorList>();
            List<TaxDeductionReportLine> groupeddeductionLines = GroupDeductionLinesByVendorAndPercentage(deductionLines);
            vendors = vendors.Concat(transactionsVendors).ToList();
            gLAccounts = gLAccounts.Concat(transactionsGLAccounts).ToList();
            foreach (TaxDeductionReportLine item in groupeddeductionLines)
            {
                ByVendorList groupedbyVendor = new ByVendorList()
                {
                    Month = item.MonthOfRegisterDate,
                    TaxDeductionPercentage = item.TaxDeductionPercentage,
                    VendorId = item.VendorId,
                };
                groupedbyVendor.EndYearBalance = 0;
                CardList selectedVendor = vendors.Where(d => d.Id == item.VendorId).FirstOrDefault();
                if (selectedVendor != null)
                {
                    GLAccountList gLAccount = gLAccounts.Where(d => d.Id == selectedVendor.GLAccountId).FirstOrDefault();

                    if (gLAccount != null)
                    {
                        groupedbyVendor = SetGLAccountFields(gLAccount, groupedbyVendor);
                    }
                    groupedbyVendor.VATNumber = selectedVendor.VatNumber;
                    groupedbyVendor.VendorName = selectedVendor.EnglishName;
                    Address address = addresses.Where(d => d.CardId == selectedVendor.Id).FirstOrDefault();

                    groupedbyVendor.VendorAddress = address != null ? address.Name : null;
                    groupedbyVendor.VendorCity = address != null ? address.City : null;// selectedVendor.CityName;
                    groupedbyVendor.IsAutonomy = selectedVendor.IsAutonomy;
                    groupedbyVendor.IsInternationlPartner = selectedVendor.IsInternationalPartner;
                    groupedbyVendor.VendorLocalName = selectedVendor.LocalName;
                    groupedbyVendor.SumOfAmountInLocalCurrency = Math.Round(item.AmountInLocalCurrency.Value, 0);
                    groupedbyVendor.SumOfTaxDeductionLocalAmount = Math.Round(item.TaxDeductionLocalAmount.Value, 0);
                  
                }
                byVendorList.Add(groupedbyVendor);
            }
            return byVendorList;
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
                LTBFilter.From = new DateTime(ReportYear.Value, 1, 1);
                LTBFilter.To = new DateTime(ReportYear.Value,12,31);
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
            return endYearBalance;
        }
       private ByVendorList SetGLAccountFields(GLAccountList gLAccount , ByVendorList groupedbyVendor)
        {
            groupedbyVendor.DisplayNumber = gLAccount.DisplayNumber;

            groupedbyVendor.Occupation = gLAccount.Occupation;
            groupedbyVendor.GLAccountLocalName = gLAccount.DeductionFileTypeCode =="08" ?  gLAccount.EnglishName.ToUpper(): gLAccount.LocalName;
            groupedbyVendor.AssessingOfficerCode = gLAccount.AssessingOfficeCode;
            groupedbyVendor.AssessingOfficerName = gLAccount.AssessingOfficeName;
            groupedbyVendor.DeductionFileTypeCode = gLAccount.DeductionFileTypeCode;
            groupedbyVendor.DeductionFileNumber = gLAccount.DeductionFileNumber;
            groupedbyVendor.DeductionType = gLAccount.DeductionTypeId;
            groupedbyVendor.EnglishName = gLAccount.EnglishName;
            groupedbyVendor.EndYearBalance = GetEndYearBalance(gLAccount.Id);

            return groupedbyVendor;
        }

        private List<TaxDeductionReportLine> GroupDeductionLinesByVendorAndPercentage(List<TaxDeductionReportLine> deductionLines)
        {

            return (from a in deductionLines
                    group a by
                        new { a.VendorId, a.TaxDeductionPercentage } into g
                    select new TaxDeductionReportLine
                    {
                        VendorId = g.Key.VendorId,
                        TaxDeductionPercentage = g.Key.TaxDeductionPercentage,
                        AmountInLocalCurrency = g.Sum(s => s.AmountInLocalCurrency),
                        TaxDeductionLocalAmount = g.Sum(s => s.TaxDeductionLocalAmount),

                    } into s
                    select s).ToList();
        }

        private List<ByMonthList> FillGroupedByMonthList(List<TaxDeductionReportLine> deductionLines)
        {
            List<ByMonthList> groupedByMonthLines = new List<ByMonthList>();
            for (int i=1; i< 13 ;i++)
            {
                var month = i;
                ByMonthList byMonthList = new ByMonthList()
                {
                    Month = month,
                    TotalVendors = deductionLines.Where(d => d.MonthOfRegisterDate ==month).GroupBy(d => d.VendorId).Count(),
                    TotalPaymentsWithoutDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.AmountInLocalCurrency).Value, 0),
                    TotalDeductionsWithoutDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                    TotalDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.AmountInLocalCurrency).Value, 0),
                    TotalDeductionsFromDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                    ReportMonth = month + "." + ReportYear,
                };
                groupedByMonthLines.Add(byMonthList);
            }
            return groupedByMonthLines;
        }
        private TaxDeductionReportData FillTotalForCompany(List<TaxDeductionReportLine> deductionLines, TaxDeductionReportData taxDeduction)
        {          
            taxDeduction.VendorsCount = taxDeduction.ByVendorList.GroupBy(d => d.VendorId).Count();
            taxDeduction.TotalAmountInLocalCurrency = Math.Round(deductionLines.Sum(d => d.AmountInLocalCurrency).Value, 0);
            taxDeduction.TotalDeductionInLocalCurrency = Math.Round(deductionLines.Sum(d => d.TaxDeductionLocalAmount).Value, 0);
            taxDeduction.TotalAmountInLocalCurrency08 = Math.Round(deductionLines.Where(d => d.DeductionType == "08").Sum(d => d.AmountInLocalCurrency).Value, 0);
            taxDeduction.TotalTaxDeductionInLocalCurrency08 = Math.Round(deductionLines.Where(d => d.DeductionType == "08").Sum(d => d.TaxDeductionLocalAmount).Value, 0);
            if (taxDeduction.ByVendorList != null)
            {
                taxDeduction.TotalEndBalance = Math.Round(taxDeduction.ByVendorList.Sum(d => d.EndYearBalance).Value, 0);//  DBVendorsList.Sum(d => d.EndYearBalance).Value,0);
            }
            taxDeduction.TotalForCompany = FillCompanyTotalForPDFReport(taxDeduction);
            return taxDeduction;

        }
        private List<TotalForCompany> FillCompanyTotalForPDFReport(TaxDeductionReportData taxDeduction)
        {
            FullAccountingSettingPM setting = GetFullAccountingPMForTenant();
            TenantPM tenantPM = GetTenantPM();
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
            TenantPM tenantPM = tenantQuery.GetSinglePM(Tenant);
            return tenantPM;
        }
    }
}
