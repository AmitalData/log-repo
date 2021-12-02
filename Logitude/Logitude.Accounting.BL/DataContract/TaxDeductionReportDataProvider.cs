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
using Logitude.Server.Tools.Helpers;

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
        List<GLAccountList> transactionsOppositGLAccounts;
        DateTime? reportMonth;
        TaxDeductionReportPM taxDeductionReport;
        List<GLAccountList> mainGLAccounts;
        List<GLAccountCurrency> accountCurrencies;
        public TaxDeductionReportDataProvider(TaxDeductionReportPM  report,int tenant)
        {
            Tenant = tenant;

            ReportYear = report.TaxYear;
           
            startDate = report.ByMonth? new DateTime((int)report.TaxYear, (int) report.Month.Value.Month, 1,0,0,0) :   new DateTime((int)report.TaxYear, 1, 1,0,0,0);
            endDate = report.ByMonth ? new DateTime((int)report.TaxYear, (int)report.Month.Value.Month, DateTime.DaysInMonth((int)report.TaxYear, report.Month.Value.Month),23,59,59) : new DateTime((int)report.TaxYear , 12, 31,23,59,59);
            this.taxDeductionReport = report;
            reportMonth = report.Month;
            taxDeductionReport.ErrorMessage = null;
            invoiceContext = InvoiceContext.GetContext(tenant);
            commoncontext = CommonDataContext.GetContext(tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            vendors = new List<CardList>();
            gLAccounts = new List<GLAccountList>();
            addresses = new List<Address>();
            transactionsOppositGLAccounts = new List<GLAccountList>();
            mainGLAccounts = new List<GLAccountList>();
            accountCurrencies = new List<GLAccountCurrency>();
        }
        TaxDeductionReportData taxDeductionReportData;
        public TaxDeductionReportData GetTaxDeductionReportData()
        {
            taxDeductionReportData = new TaxDeductionReportData();
            taxDeductionReportData.TaxYear = ReportYear.ToString();
            setting = GetFullAccountingPMForTenant();
            taxDeductionReportData.SettingDeductionFileNumber = setting.DeductionFileNumber;
            taxDeductionReportData.deductionLines = GetTaxReportDeductionLines();

            taxDeductionReportData.ByVendorList = FillGroupByVendorList(taxDeductionReportData.deductionLines);
            taxDeductionReportData.ByMonthList = FillGroupedByMonthList(taxDeductionReportData.deductionLines, taxDeductionReportData);
            taxDeductionReportData = FillTotalForCompany(taxDeductionReportData.deductionLines, taxDeductionReportData);
            return taxDeductionReportData;
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
            string id = null;
            foreach (APPayment payment in payments) {

                if (id == payment.Id) continue;
                id = payment.Id;
                TaxDeductionReportLine taxDeductionReportline = new TaxDeductionReportLine();
                taxDeductionReportline.VendorId = payment.VendorCard!= null? payment.VendorCard.GLAccountId: null;
                
                taxDeductionReportline.MonthOfRegisterDate = cancelled? payment.AccountingCancelationDate.Value.Month : payment.RegisterDate.Value.Month;
                taxDeductionReportline.AmountInLocalCurrency = cancelled ?  payment.AmountInLocalCurrency*-1 : payment.AmountInLocalCurrency;
                taxDeductionReportline.TaxDeductionLocalAmount =cancelled ? payment.TaxDeductionLocalAmount*-1 : payment.TaxDeductionLocalAmount;
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
                    where (a.AccountingDate >= startDate && a.AccountingDate <= endDate)
                    && a.Tenant == Tenant &&  a.AccountId == setting.TaxWithholdingGLAccountId
                    && j.ExternalSystem != null && a.LocalAmountDebit == 0
                   select a).ToList();
            List<string> journalIds = transactions.Select(d => d.JournalId).ToList();
            journalLines = GetJournalLinesByJournalds(journalIds);
            List<string> accountsIds = transactions.Where(d=> d.OppositeAccountId != null).Select(d => d.OppositeAccountId ).ToList(); 
            transactionsVendors = GetVendorsByAccountsIds(accountsIds);          
            oppositeAccountTransactions = GetOppositeTransactions(transactions);

            return transactions;
        }
        private List<JournalLine> GetJournalLinesByJournalds(List<string> journalIds )
        {
            List<JournalLine> journalLines = (from a in accountingContext.JournalLines                                       
                                                    where 
                                                     a.Tenant == Tenant && (a.DocumentDate >= startDate && a.DocumentDate <= endDate)
                                                    && a.ActionCode == "2" && journalIds.Contains(a.JournalId)
                                                    select a).ToList();
            return journalLines;
        }

        private List<LedgerTransaction> GetOppositeTransactions(List<LedgerTransaction> transactions)
        {
            List<string> oopositeAccountIds = transactions.Where(d => d.OppositeAccountId != null).Select(d => d.OppositeAccountId).ToList();
            return (from a in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on a.JournalId equals journal.Id
                    where oopositeAccountIds.Contains(a.AccountId)
                    && a.Tenant == Tenant && (a.AccountingDate >= startDate && a.AccountingDate <= endDate) && journal.ExternalSystem != null
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
            vendors = vendors.Concat(GetAccountsVendors(gLAccounts.Select(d => d.Id).ToList())).ToList();
            addresses = addresses.Concat(GetVendorsAddresses(vendors.Select(d=> d.Id).ToList())).ToList();
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
                                          LocalName = a.LocalName,
                                          Code = a.Code,
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

        public List<GLAccountList> GetTransactionsOppositeGLAccounts(List<LedgerTransaction> transactions)
        {
            List<string> glAccountIds = transactions.Select(d => d.OppositeAccountId).ToList();
            return GetGLAccountsByIds(glAccountIds);
           

        }
        public List<GLAccountList> GetTransactionsGLAccounts(List<LedgerTransaction> transactions) {

            List<string> glAccountIds = transactions.Select(d => d.AccountId).ToList();
            return GetGLAccountsByIds(glAccountIds);


        }
        public List<GLAccountList> GetGLAccountsByIds(List<string> accountIds)
        {
            return (from a in accountingContext.GLAccounts
                    where a.Tenant == Tenant && a.ExcludeFromDeductionReport ==false
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
        List<LedgerTransaction> createdLines;
        public List<TaxDeductionReportLine> FillTaxDeductionReportUsingLedgerTransactions()
        {
            List<TaxDeductionReportLine> lines = new List<TaxDeductionReportLine>();
            transactions = GetTransactions();
            transactionsOppositGLAccounts = GetTransactionsOppositeGLAccounts(transactions);
            accountCurrencies = GetGLAccountCurrenciesForOppositeAccounts(transactionsOppositGLAccounts);
            mainGLAccounts = GetGLAccountsByIds(accountCurrencies.Select(d => d.MainGLAccountId).ToList());
            transactionsGLAccounts = GetTransactionsGLAccounts(transactions);

            //    transactions = transactions.Where(d => d.AccountId == setting.TaxWithholdingGLAccountId).ToList();// == a.AccountId
            createdLines = new List<LedgerTransaction>();
            foreach (LedgerTransaction transaction in transactions)
            {
                bool LineCreated = CheckIfLineCreated(transaction);
                if (!LineCreated)
                {
                    UpdateCreatedLinesList(transaction);
                    TaxDeductionReportLine taxDeductionReportLine = new TaxDeductionReportLine();
                    string vendorId = GetVendorId(transaction);
                    taxDeductionReportLine.VendorId = vendorId;
                    taxDeductionReportLine.MonthOfRegisterDate = transaction.AccountingDate.Month;
                    List<LedgerTransaction> oppositeTransactions = oppositeAccountTransactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).ToList();
                    if (oppositeTransactions != null && oppositeTransactions.Count > 0)
                    {
                        taxDeductionReportLine.AmountInLocalCurrency = (double?)oppositeTransactions.Sum(d => d.LocalAmountDebit);
                    }
                    List<LedgerTransaction> duplicatedCreditLines = transactions.Where(d => d.JournalId == transaction.JournalId && d.AccountId == transaction.AccountId && d.OppositeAccountId == transaction.OppositeAccountId && d.Reference1 == transaction.Reference1).ToList();
                    if(duplicatedCreditLines != null && duplicatedCreditLines.Count > 0)
                    {
                        transaction.LocalAmountCredit = duplicatedCreditLines.Sum(d => d.LocalAmountCredit);
                    }
                    taxDeductionReportLine.TaxDeductionLocalAmount = transaction.LocalAmountCredit;
                    taxDeductionReportLine.TaxDeductionPercentage = (int?)(transaction.LocalAmountCredit == 0 ? 0 : Math.Round(((transaction.LocalAmountCredit / (decimal)taxDeductionReportLine.AmountInLocalCurrency)) * 100, 2));
                    GLAccountList account = transactionsOppositGLAccounts.Where(d => d.Id == transaction.OppositeAccountId).FirstOrDefault();
                    taxDeductionReportLine.DeductionType = account != null ? account.DeductionFileTypeCode : null;
                    if (taxDeductionReportLine.VendorId != null) lines.Add(taxDeductionReportLine);
                }
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
        private void UpdateCreatedLinesList(LedgerTransaction transaction)
        {
            createdLines.Add(transaction);
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
                    List<JournalLine> selectedJournalLines = journalLines.Where(d => d.JournalId == transaction.JournalId && d.Reference1 == transaction.Reference1).ToList();
                    List<string> debitAccountIds = selectedJournalLines.Select(d => d.DebitAccountId).ToList();
                    GLAccountList gLAccount = transactionsGLAccounts.Where(d => d.ChartOfAccountsTypeCode != "5" && debitAccountIds.Contains(d.Id) && d.Id != setting.TaxWithholdingGLAccountId && !d.ExcludeFromDeductionReport).FirstOrDefault();
                    if (gLAccount != null)
                    {
                        vendorId = gLAccount.Id;// vendors.Where(d => d.GLAccountId == gLAccount.Id).FirstOrDefault();
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
        private string GetVendorIdByMainAccount(string accountId)
        {
            GLAccountList account = transactionsOppositGLAccounts.Where(d => d.Id == accountId).FirstOrDefault(); 
            if (account == null || account.IsMultiCurrency == true) return null;         
            return GetVendorIdForSingleCurrencyAccount(account);


        }
        private string GetVendorIdForSingleCurrencyAccount(GLAccountList account)
        {
            GLAccountCurrency accountCurrency = accountCurrencies.Where(d => d.GLAccountId == account.Id).FirstOrDefault();
            if (accountCurrency == null)
            {
                return null;
            }
            return accountCurrency.MainGLAccountId;          
        }
      
        private List<GLAccountCurrency> GetGLAccountCurrenciesForOppositeAccounts(List<GLAccountList> accounts)
        {
            List<string> accountsIds = accounts.Where(d => d.IsMultiCurrency == false).Select(d => d.Id).ToList();
            return (from accountCurrency in accountingContext.GLAccountCurrencies
                    where accountsIds.Contains(accountCurrency.GLAccountId)
                    select accountCurrency).ToList();          
        }
                     
        private List<APPayment> getAPPaymentsWithGLAccountsAndVendor(List<APPayment> payments)
        {
            vendors = vendors.Concat(GetVendorsByIds(payments)).ToList();
            payments = (from a in payments
                                 join v in vendors on a.VendorId equals v.Id
                                 join g in gLAccounts on v.GLAccountId equals g.Id  into g 
                                 select a).ToList();
            
            return payments;
        }
        private List<APPayment> GetCancelledPayments()
        {

            List<APPayment> cancelledPayments = (from a in invoiceContext.APPayments.Include("VendorCard")
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
            List<CardList> mainCards = GetMainAccountsCards();
            vendors = vendors.Concat(transactionsVendors).Concat(mainCards).ToList();
            gLAccounts = gLAccounts.Concat(transactionsOppositGLAccounts).Concat(transactionsGLAccounts).Concat(mainGLAccounts).ToList();

            foreach (TaxDeductionReportLine item in groupeddeductionLines)
            {
               
              //  var deductionPercentage = item.TaxDeductionLocalAmount ==0 ? 0 : item.TaxDeductionLocalAmount / (item.TaxDeductionLocalAmount + (decimal) item.AmountInLocalCurrency);
                ByVendorList groupedbyVendor = new ByVendorList()
                {
                    Month = item.MonthOfRegisterDate,
                    TaxDeductionPercentage =(int?)Math.Round((double)item.TaxDeductionPercentage, MidpointRounding.AwayFromZero),
                    VendorId = item.VendorId,
                };
                groupedbyVendor.EndYearBalance = 0;
                GLAccountList gLAccount = gLAccounts.Where(d => d.Id == item.VendorId).FirstOrDefault();

                if (gLAccount != null)
                {
                    List<CardList> selectedVendors = FillSelectedVendosList(vendors, item.VendorId);
                //    List<CardList> selectedVendors = vendors.Where(d => d.GLAccountId == item.VendorId).Distinct().ToList();
                    ValidateGLAccountVendors(selectedVendors, gLAccount);
                    groupedbyVendor = SetGLAccountFields(gLAccount, groupedbyVendor);
                    groupedbyVendor = SetVendorVatNumberAndAddress(selectedVendors, groupedbyVendor);
                    if (selectedVendors.Count > 0)
                    {
                        groupedbyVendor.VendorName = selectedVendors[0].EnglishName;
                        groupedbyVendor.IsAutonomy = selectedVendors[0].IsAutonomy;
                        groupedbyVendor.IsInternationlPartner = selectedVendors[0].IsInternationalPartner;
                        groupedbyVendor.VendorLocalName = selectedVendors[0].LocalName;
                    }
                        groupedbyVendor.SumOfAmountInLocalCurrency = Math.Round(item.AmountInLocalCurrency.Value, 0);
                        groupedbyVendor.SumOfTaxDeductionLocalAmount = Math.Round(item.TaxDeductionLocalAmount.Value, 0);
                    if (groupedbyVendor.SumOfAmountInLocalCurrency > 0)
                    {
                        byVendorList.Add(groupedbyVendor);
                    }
                  //  else { DeleteVendorFromTaxDeductionReportLines(item.VendorId); }
                      
                }
                else
                {
                    DeleteVendorFromTaxDeductionReportLines(item.VendorId);
                }

            }
            if(taxDeductionReport.ErrorMessage != null)
            {
                throw new Exception(taxDeductionReport.ErrorMessage);
            }
            return byVendorList;
        }
        private List<CardList> GetMainAccountsCards()
        {
            List<string> mainAccountsIds = mainGLAccounts.Select(d => d.Id).ToList();
            return GetVendorsByAccountsIds(mainAccountsIds);
        }
        private ByVendorList SetVendorVatNumberAndAddress(List<CardList> selectedVendors, ByVendorList groupedbyVendor) {
            string cardsCodes = "";
            if (selectedVendors.Count > 1)
            {

                foreach (CardList item in selectedVendors.OrderBy(d => d.Code))
                {
                    cardsCodes = cardsCodes + item.Code + ",";
                    Address address = addresses.Where(d => d.CardId == item.Id).FirstOrDefault();
                    if (groupedbyVendor.VATNumber == null && item.VatNumber != null)
                    {
                        groupedbyVendor.VATNumber = item.VatNumber;
                    }
                    if (groupedbyVendor.VendorAddress == null || groupedbyVendor.VendorCity ==null)
                    {
                        groupedbyVendor.VendorAddress = address!= null? address.Name: null;
                        groupedbyVendor.VendorCity =address!= null? address.City: null;
                    }
                    if (groupedbyVendor.VATNumber != null && address != null)
                    {
                        break;
                    }
                }
               
            }
            else if(selectedVendors.Count == 1)
            {
                cardsCodes = selectedVendors[0].Code;
                groupedbyVendor.VATNumber = selectedVendors[0].VatNumber;
                Address address = addresses.Where(d => d.CardId == selectedVendors[0].Id).FirstOrDefault();
                groupedbyVendor.VendorAddress = address != null ? address.Name : null;
                groupedbyVendor.VendorCity = address != null ? address.City : null;
            }
            if (groupedbyVendor.VATNumber == null)
            {
                taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + (selectedVendors.Count>1? " Vendor GLAccount " + groupedbyVendor.DisplayNumber + " is connected to more than one Operational Vendor Card and none of them contain a VAT number" + cardsCodes : TextCodesTranslator.TranslateText("TaxDeductionReport.O.CardWithoutVatNumber", Tenant) + ", " + TextCodesTranslator.TranslateText("Card.F.Code", Tenant) + ":" + selectedVendors[0].Code) ;
            }
            if (groupedbyVendor.VendorAddress == null && groupedbyVendor.VendorCity == null)
            {
                taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + (selectedVendors.Count>1? " Vendor GLAccount " + groupedbyVendor.DisplayNumber + " is connected to more than one Operational Vendor Card and none of them contain an address" + cardsCodes : TextCodesTranslator.TranslateText("TaxDeductionReport.O.CardWithoutAddress", Tenant) + ", " + TextCodesTranslator.TranslateText("Card.F.Code", Tenant) + ":" + selectedVendors[0].Code);
            }
            return groupedbyVendor;
        }

        private void DeleteVendorFromTaxDeductionReportLines(string vendorId)
        {
            List<TaxDeductionReportLine> excludeditems = deductionLines.Where(d => d.VendorId == vendorId).ToList();
            foreach (TaxDeductionReportLine line in excludeditems)
            {
                deductionLines.Remove(line);
            }

        }

        private List<CardList> FillSelectedVendosList(List<CardList> vendors, string accountId)
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
            if (selectedVendors.Count() > 1)
            {
                 
                //string error = TextCodesTranslator.TranslateText("TaxDeductionReport.O.VendorGLAccount", Tenant) + " " + gLAccount.DisplayNumber + " " + TextCodesTranslator.TranslateText("TaxDeductionReport.O.Connected2ManyCards", Tenant) + " ";
                //foreach (CardList vendor in selectedVendors)
                //{
                    
                //    error = error + "," + vendor.Code;
                //    taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + (error); 
                //}
              

            }
            else if (selectedVendors.Count() == 0)
            {
                string error= TextCodesTranslator.TranslateText("TaxDeductionReport.O.VendorGLAccount", Tenant) + " " + gLAccount.DisplayNumber + " " + TextCodesTranslator.TranslateText("TaxDeductionReport.O.AccountWithoutVendor", Tenant);

                taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + (error);
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
                LTBFilter.From = startDate;// new DateTime(ReportYear.Value, 1, 1);
                LTBFilter.To = endDate;// new DateTime(ReportYear.Value,12,31);
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
       private ByVendorList SetGLAccountFields(GLAccountList gLAccount , ByVendorList groupedbyVendor)
        {
            ValidateGLAccountDeductionTypeFields(gLAccount);
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
            taxDeductionReport.ErrorMessage = taxDeductionReport.ErrorMessage + Environment.NewLine + "Glaccount without " + TextCodesTranslator.TranslateText("GLaccount.F." + Fieldname, Tenant) + " , " + TextCodesTranslator.TranslateText("GLAccount.F.DisplayNumber", Tenant) + ": " + gLAccount.DisplayNumber;
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
        private List<ByMonthList> FillGroupByMonthData(int month, List<ByMonthList> groupedByMonthLines)
        {
            ByMonthList byMonthList = new ByMonthList()
            {

                Month = month,
                TotalVendors = deductionLines.Where(d => d.MonthOfRegisterDate == month).GroupBy(d => d.VendorId).Count(),
                TotalPaymentsWithoutDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.AmountInLocalCurrency).Value, 0),
                TotalDeductionsWithoutDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType != "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                TotalDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.AmountInLocalCurrency + (double?)d.TaxDeductionLocalAmount).Value, 0),
                TotalDeductionsFromDivided = Math.Round(deductionLines.Where(d => d.MonthOfRegisterDate == month && d.DeductionType == "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                ReportMonth = month + "." + ReportYear,
            };
            groupedByMonthLines.Add(byMonthList);
            return groupedByMonthLines;

        }
        private TaxDeductionReportData FillTotalForCompany(List<TaxDeductionReportLine> deductionLines, TaxDeductionReportData taxDeduction)
        {          
            taxDeduction.VendorsCount = deductionLines.GroupBy(d => d.VendorId).Count();
            taxDeduction.TotalAmountInLocalCurrency = Math.Round(deductionLines.Sum(d => d.AmountInLocalCurrency ).Value, 0);
            taxDeduction.TotalDeductionInLocalCurrency = Math.Round(taxDeductionReportData.ByMonthList.Sum(d => d.TotalDeductionsWithoutDivided + d.TotalDeductionsFromDivided).Value, 0);
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
