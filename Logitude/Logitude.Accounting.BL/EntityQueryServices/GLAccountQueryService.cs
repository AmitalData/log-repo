using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityDataMappings;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel;
using Logitude.Accounting.BL.DataContract;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.CloseTables;
using Logitude.BL.Resolvers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Accounting.Data.DataContract;
using Simplog.Data.Helpers;
using Logitude.Accounting.BL.EntityUpdateServices;
using System.Runtime.Remoting.Contexts;
using Logitude.BL.Security;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountQueryService : EntityQueryService<GLAccount, GLAccountKeys, GLAccountPM, object, GLAccountKeys>, IGLAccountQueryService
    {

        public override void GetComposition(EntityKeyFields entityKeys, GLAccountPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            GLAccountKeys gLAccountKeys = entityKeys as GLAccountKeys;

            GLAccountWithholdingTaxQueryService gLAccountWithholdingTaxQueryService = new GLAccountWithholdingTaxQueryService(context);
            entityPM.GLAccountWithholdingTaxes = gLAccountWithholdingTaxQueryService.GetMulti(gLAccountKeys, true);

            GLAccountInterestPeriodQueryService gLAccountInterestPeriodQueryService = new GLAccountInterestPeriodQueryService(context);
            entityPM.GLAccountInterestPeriods = gLAccountInterestPeriodQueryService.GetMulti(gLAccountKeys, true);

            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(context);
            entityPM.GLAccountCurrencies = gLAccountCurrencyQueryService.GetMulti(gLAccountKeys, true);

            entityPM.GLAccountChildren = this.GetChildAccounts(entityPM.Id, entityPM.Tenant);

            if (entityPM.GLAccountWithholdingTaxes.Count > 0)
            {
                entityPM.TaxWithholdingLastLine = gLAccountWithholdingTaxQueryService.GetMaxLineNumber(entityPM.Id, entityPM.Tenant);
            }

        }
        public IQueryable<string> GetQGLAccIdByCollectorId(int tenant, string CollectorId, string AccountTypeCode)
        {
            var q = (
                from a in this.repository.GetQAllByAccountTypeCode(tenant, AccountTypeCode)
                join card in (this.context as AccountingContext).Cards.Where(r => r.Tenant == tenant)
                on a.Id equals card.GLAccountId
                join cust in (this.context as AccountingContext).Customers
                .Where(r => r.CollectorId == CollectorId && r.Tenant == tenant)
                on card.Id equals cust.Id
                select a.Id);
            return q;
        }

        public IQueryable<string> GetQGLAccIdByCollectorId_ForAgingReport(int tenant, string CollectorId, string AccountTypeCode)
        {
            var q = (
                from a in this.repository.GetQAllByAccountTypeCode(tenant, AccountTypeCode)
                join cardsdata in (this.context as AccountingContext).GLAccountCardsDatas
                    .Where(r => r.CollectorUserId == CollectorId && r.Tenant == tenant)
                 on a.CardsDataId equals cardsdata.Id
                select a.Id);
            return q;
        }

        internal IQueryable<string> GetQByChartOfAccountsTypeCode(int tenant, string chartOfAccountsTypeCode)
        {
            var q = (
                  from a in this.repository.GetbychartOfAccountsTypeCode(tenant, chartOfAccountsTypeCode)
                  select a.Id);
            return q;
        }
        public IQueryable<string> GetQGLAccIdByChartOfAccountsId(int tenant, string ChartOfAccountsId)
        {
            var q = (
                from a in this.repository.GetbyChartOfAccountsId(tenant, ChartOfAccountsId)
                select a.Id);
            return q;
        }

        public GLAccountPM GetGlaAccountByJouranlIdAndJournalLineNumber(int tenant, string JournalId, int JournalLineNumber)
        {
            var glAccountPM = (
                from a in this.repository.GetGlaAccountByJouranlIdAndJournalLineNumber(tenant, JournalId, JournalLineNumber)
                select new GLAccountPM()
                {
                    Id = a.Id,
                    CurrencyId = a.CurrencyId,
                    DisplayNumber = a.DisplayNumber,
                    Inactive = a.Inactive,
                    CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                    ChartOfAccountsId = a.ChartOfAccountsId,
                    ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                    LocalName = a.LocalName,
                    ReconcileMethodCode = a.ReconcileMethodCode,
                    RevenueExpenseType = a.RevenueExpenseType,
                    Tenant = tenant,
                    AccountTypeCode = a.AccountTypeCode,
                    AutomaticReconcileId = a.AutomaticReconcileId,
                    ControlAccountId = a.ControlAccountId,
                    CustomerGLAccountId = a.CustomerGLAccountId,
                    InternalNumber = a.InternalNumber

                }).FirstOrDefault();
            return glAccountPM;
        }
        public IQueryable<string> GetQGLAccIdBySalesmanId(int tenant, string SalesmanId, string AccountTypeCode)
        {
            IQueryable<string> q = (
               from a in this.repository.GetQAllByAccountTypeCode(tenant, AccountTypeCode)
               join card in (this.context as AccountingContext).Cards.Where(r => r.Tenant == tenant)
               on a.Id equals card.GLAccountId

               join cust in (this.context as AccountingContext).Customers
               .Where(r => r.SalesmanUserId == SalesmanId && r.Tenant == tenant)
               on card.Id equals cust.Id

               select a.Id);

            IQueryable<string> qChild = (from b in q
                                         join glaccountChild in this.context.GLAccountCurrencies.Where(r => r.Tenant == tenant)
                                         on b equals glaccountChild.MainGLAccountId
                                         select glaccountChild.GLAccountId);

            IQueryable<string> combinedQuery = q.Concat(qChild);




            return combinedQuery;
        }
        public bool CheckIfDisplayNumberExists(string displayNo, string internalNumber, int tenant)
        {
            return this.repository.CheckIfDisplayNumberExists(displayNo, internalNumber, tenant);
        }


        public List<string> GetGLAccountsWithoutLedgerTransactions(List<string> glAccountIds, OpenFormatReportPM openFormatReport, int tenant)
        {
            List<string> accountIds = (from l in context.LedgerTransactions

                                       where glAccountIds.Contains(l.AccountId) && l.Tenant == tenant && l.AccountingDate >= openFormatReport.FromDate && l.AccountingDate <= openFormatReport.ToDate
                                       select l.AccountId).ToList();

            return glAccountIds.Where(d => !accountIds.Contains(d)).ToList();


        }

        public IQueryable<GLAccountAndMoreDTO> GetQAllRevenueExpenseCardsByIsControlAccount(int tenant, bool isControlAccount)
        {
            var pocoGLAccountAndMores = this.repository.GetQAllRevenueExpenseCardsByIsControlAccount(tenant, isControlAccount);//.ToList();
            //(this.mapping as GLAccountDataMapping).SuppressGetCardByGLAccountId = true;
            //var pms = pocos.Select(r => this.GetEntityPM(r)).ToList();
            //return pms;
            return pocoGLAccountAndMores;
        }
        public List<GLAccountAndMoreDTO> GetAllRevenueExpenseCards(int tenant)
        {
            var pocoGLAccountAndMores = this.repository.GetQAllRevenueExpenseCards(tenant).ToList();
            return pocoGLAccountAndMores;
            //var pms = pocos.Select(r => this.GetEntityPM(r)).ToList();
            //return pms;
        }

        public List<GLAccount> GetAllGLAccountIdsByChartsofAccountId(int tenant, string chartsofAccountId)
        {
            List<GLAccount> GLAccounts = this.repository.GetAll(tenant).Where(s => s.Inactive == false && s.ChartOfAccountsId == chartsofAccountId).ToList();
            return GLAccounts;
        }


        public List<GLAccount> GetControlAccountForChartOfAccount(int tenant, string chartsofAccountId)
        {
            List<GLAccount> GLAccounts = this.repository.GetAll(tenant)
                .Where(s => s.Inactive == false
                            && s.ChartOfAccountsId == chartsofAccountId
                            && s.IsControlAccount == true).ToList();
            return GLAccounts;
        }

        public IQueryable<CardGLAccountDataView> GetQAllVendorGLAccountCardsHavingDeduction(int tenant)
        {
            var pocoGLAccountCard = this.repository.GetQAllVendorGLAccountCardsHavingDeduction(tenant);
            return pocoGLAccountCard;
        }

        public IQueryable<GLAccount> GetQueryAllSmallCashbookAccount(int tenant)
        {
            var pocoGLAccount = this.repository.GetQueryAllSmallCashbookAccount(tenant);
            return pocoGLAccount;
        }

        public List<int> GetTenantByNextDueDate(DateTime today, List<string> accountTypeCodeList)
        {
            return this.repository.GetTenantByNextDueDate(today, accountTypeCodeList);
        }

        public IQueryable<GLAccountAndMoreDTO> GetQByAccountTypeCodeList(int tenant, List<string> accountTypeCodeList)
        {
            return this.repository.GetQByAccountTypeCodeList(tenant, accountTypeCodeList);
        }
        public IQueryable<string> GetListByNextDueDate(int tenant, DateTime today, List<string> accountTypeCodeList)
        {
            return this.repository.GetListByNextDueDate(tenant, today, accountTypeCodeList);
        }

        public List<GLAccountPM> GetByAcountTypeCategories(int tenant, string AccountType,
            string Category1, string Category2, string Category3, string Category4, string Category5)
        {
            var listPoco = this.repository.GetByAcountTypeCategories(tenant, AccountType,
            Category1, Category2, Category3, Category4, Category5).ToList();
            var pmList = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return pmList;
        }



        public bool CheckIfInternalNumberExists(string internalNumber, string id, int tenant)
        {
            return this.repository.CheckIfInternalNumberExists(internalNumber, id, tenant);
        }
        public HashSet<string> GetAllIdAccounts(int tenant, string GLAccountId,
            bool IncludeRelatedCurrenciesAccount, bool IncludeChildAccounts)
        {
            var allIdAccounts = new List<string>() { GLAccountId };
            if (IncludeRelatedCurrenciesAccount)
            {
                var myGLAccountCurrencyRepository = new GLAccountCurrencyRepository(this.context);
                var relatedCurrenciesAccountByCustomerGLAccount = myGLAccountCurrencyRepository.GetRelatedCurrenciesAccountByCustomerGLAccountActive(tenant, GLAccountId)
                    .Select(ca => ca.GLAccountId).ToList();
                allIdAccounts.AddRange(relatedCurrenciesAccountByCustomerGLAccount);
            }
            if (IncludeChildAccounts)
            {
                //var myGLAccountRepository = new GLAccountRepository(_AccountingContext);
                var ChildAccounts = repository.GetChildAccounts(GLAccountId, tenant)
                .Select(ca => ca.Id).ToList();
                allIdAccounts.AddRange(ChildAccounts);
            }

            return new HashSet<string>(allIdAccounts);
        }
        public IQueryable<string> GetQAllIdAccounts(int tenant, string GLAccountId,
           bool IncludeRelatedCurrenciesAccount, bool IncludeChildAccounts)
        {
            //var allIdAccounts = new List<string>() { GLAccountId };
            IQueryable<string> allIdAccounts = repository.GetQId(new List<string>() { GLAccountId }, tenant);
            if (IncludeRelatedCurrenciesAccount)
            {
                var myGLAccountCurrencyRepository = new GLAccountCurrencyRepository(this.context);
                var relatedCurrenciesAccountByCustomerGLAccount = myGLAccountCurrencyRepository.
                    GetRelatedCurrenciesAccountByCustomerGLAccountAll(tenant, GLAccountId)
                    .Select(ca => ca.GLAccountId).AsQueryable<string>();// ToList();
                ////i decided to add this due unittest :Run_IncludeRelatedCurrenciesAccount_AllCurrencies
                allIdAccounts =
                ////i decided to add this due unittest :Run_IncludeRelatedCurrenciesAccount_AllCurrencies
                    allIdAccounts.Union(relatedCurrenciesAccountByCustomerGLAccount);
            }
            if (IncludeChildAccounts)
            {
                //var myGLAccountRepository = new GLAccountRepository(_AccountingContext);
                var ChildAccounts = repository.GetChildAccounts(GLAccountId, tenant)
                .Select(ca => ca.Id).AsQueryable<string>();// ToList();

                //allIdAccounts.AddRange(ChildAccounts);

                allIdAccounts = allIdAccounts.Union(ChildAccounts);

            }

            return allIdAccounts;
        }

        public List<string> GetChildAccountsCurrencies(int tenant, string GLAccountId)
        {
            return repository.GetChildAccounts(GLAccountId, tenant)
                .Select(ca => ca.CurrencyId).ToList();
        }

        public HashSet<string> GetAllIdAccountsCat(int tenant, string GLAccountId, string cat1, string cat2, string cat3, string cat4, string cat5,
    bool IncludeChildAccounts)
        {
            var allIdAccounts = new List<string>() { GLAccountId };
            if (!String.IsNullOrWhiteSpace(cat1) || !String.IsNullOrWhiteSpace(cat2) || !String.IsNullOrWhiteSpace(cat3) || !String.IsNullOrWhiteSpace(cat4) || !String.IsNullOrWhiteSpace(cat5))
            {
                var catAccounts = repository.GetQAccIdByAcountIdCategories(tenant, GLAccountId, cat1, cat2, cat3, cat4, cat5)
                    .ToList();
            }
            if (IncludeChildAccounts)
            {
                var ChildAccounts = repository.GetChildAccounts(GLAccountId, tenant)
                .Select(ca => ca.Id).ToList();
                allIdAccounts.AddRange(ChildAccounts);
            }

            return new HashSet<string>(allIdAccounts);
        }

        public IQueryable<string> GetAllIdAccountsTypeCat(int tenant, string GLAccountId, string cat1, string cat2, string cat3, string cat4, string cat5, string gLAccountType, string chartOfAccountsId,
    bool IncludeChildAccounts, string ChartOfAccountsTypeCode, string salesmanId, bool includeControlAccount, bool useSecurityLevel, string collectorId)
        {
            int? securityLevel = GetSecurityLevel(useSecurityLevel, tenant);
            // List<String> allIdAccounts = new List<string>() { GLAccountId };
            IQueryable<string> allIdAccounts = repository.GetQId(new List<string>() { GLAccountId }, tenant);
            if (!String.IsNullOrWhiteSpace(cat1) || !String.IsNullOrWhiteSpace(cat2) || !String.IsNullOrWhiteSpace(cat3) || !String.IsNullOrWhiteSpace(cat4)
                || !String.IsNullOrWhiteSpace(cat5) || !String.IsNullOrWhiteSpace(gLAccountType) || !String.IsNullOrWhiteSpace(chartOfAccountsId)
                || !String.IsNullOrWhiteSpace(ChartOfAccountsTypeCode)
                || !String.IsNullOrWhiteSpace(salesmanId) || !String.IsNullOrWhiteSpace(collectorId)
                )
            {

             
                    IAccountingContext context = AccountingContext.GetContext(tenant);
                    GLAccountRepository repository = new GLAccountRepository(context);
                                      
                   allIdAccounts = repository.GetQAccIdByAcountIdTypeCategories(tenant, GLAccountId, cat1, cat2, cat3, cat4, cat5, gLAccountType, chartOfAccountsId, ChartOfAccountsTypeCode, salesmanId, includeControlAccount, collectorId, securityLevel);
                    if (IncludeChildAccounts && allIdAccounts != null && allIdAccounts.ToList().Count > 0)
                    {
                        IQueryable<String> ChildAccounts = repository.GetChildAccountsQ(allIdAccounts, tenant, securityLevel)
                        .Select(ca => ca.Id).AsQueryable<string>();//ToList();
                        allIdAccounts.Union(ChildAccounts);
               
                    return allIdAccounts;// new HashSet<string>(allIdAccounts);
                }
            }

            return allIdAccounts;
        }
         
        //public List<string> GetGLAccountIdByTypeControl(int tenant, string accountTypeCode, bool? isControlAccount)
        //{
        //    List<string> allIdAccounts = repository.GetGLAccountIdByTypeControl(tenant, accountTypeCode, isControlAccount);

        //    return allIdAccounts;
        //}


        public List<string> GetNextGLAccountIdByTypeControl(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            List<string> allIdAccounts = repository.GetNextGLAccountIdByTypeControl(tenant, accountTypeCode, isControlAccount, lastMadeGLAccountId, maxGLAccountsPerQuery);

            return allIdAccounts;
        }

        public List<string> GetNextGLAccountIdByTypeControlNoParent(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            List<string> allIdAccounts = repository.GetNextGLAccountIdByTypeControlNoParent(tenant, accountTypeCode, isControlAccount, lastMadeGLAccountId, maxGLAccountsPerQuery);

            return allIdAccounts;
        }

        public List<string> GetNextGLAccountIdByTypeControlDescendant(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            List<string> allIdAccounts = repository.GetNextGLAccountIdByTypeControlDescendant(tenant, accountTypeCode, isControlAccount, lastMadeGLAccountId, maxGLAccountsPerQuery);

            return allIdAccounts;
        }



        private int? GetSecurityLevel(bool useSecurityLevel, int tenant)
        {
            UserPM loggedUser = GetLoggedUser(tenant);
            if (loggedUser != null && (loggedUser.IsCustomerCare || !useSecurityLevel))
                return loggedUser.SecurityLevel = null;
            else
                return null;
        }

        public List<GLAccountAndMoreDTO> GetCurrentBalanceByType(int tenant)
        {
            var fullPM = FullAccountingSettingQueryService.Get(tenant);
            //fullPM.

            var qAllCards = this.repository.GetQAllCards(tenant);

            var qGruop = (from acc in qAllCards
                          where acc.IsControlAccount == false
                          group acc by 1 into g
                          select new
                          {
                              TypeName = "All Rest",
                              BalanceInLocalCurrency = g.Sum(r => r.BalanceInLocalCurrency)
                          }
                ).Union(

                (from acc in qAllCards
                 where acc.Id == fullPM.AirExportJobControlAccountId
                 select new
                 {
                     TypeName = "Air Export Job",
                     BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                 })

                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.AirImportJobControlAccountId
                  select new
                  {
                      TypeName = "Air Import Job",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })

                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.CustomerControlAccountId
                  select new
                  {
                      TypeName = "Customer",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })

                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.FileControlAccountId
                  select new
                  {
                      TypeName = "File",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })

                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.OceanExportJobControlAccountId
                  select new
                  {
                      TypeName = "Ocean Export Job",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })


                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.OceanImportJobControlAccountId
                  select new
                  {
                      TypeName = "Ocean Import Job",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })

                 ).Union(
                 (from acc in qAllCards
                  where acc.Id == fullPM.VendorControlAccountName
                  select new
                  {
                      TypeName = "Vendor",
                      BalanceInLocalCurrency = acc.BalanceInLocalCurrency
                  })

                 )
                 ;
            var list = qGruop.ToList();
            var dic = new List<GLAccountAndMoreDTO>();
            list.ForEach(r =>
              {
                  dic.Add(new GLAccountAndMoreDTO()
                  {
                      AccountTypeName = r.TypeName,
                      BalanceInLocalCurrency = r.BalanceInLocalCurrency.GetValueOrDefault()
                  });
              }
            );
            return dic;
        }


        internal void SetSuppressFetchOpenReconcilation(bool suppressFetchOpenReconcilation)
        {
            (this.mapping as GLAccountDataMapping).SuppressFetchOpenReconcilation = suppressFetchOpenReconcilation;
        }

        //public bool CheckIfClientAndCurrencyExist(string clientId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(clientId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return this.repository.CheckIfClientAndCurrencyExist(clientId, currencyId, internalNumber, tenant);
        //    }
        //}

        //public bool CheckIfVendorAndCurrencyExist(string vendorId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(vendorId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return this.repository.CheckIfVendorAndCurrencyExist(vendorId, currencyId, internalNumber, tenant);
        //    }
        //}
        public GLAccountPM GetSinglePM(string gLAccountId, int tenant)
        {
            GLAccount gLAccountPOCO = repository.GetGLAccountByIdTenant(gLAccountId, tenant);
            GLAccountPM gLAccountPM = GetEntityPM(gLAccountPOCO);

            return gLAccountPM;
        }


        public FullAccountingSettingPM GetFullAccountingSettings(int tenant)
        {
            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            return fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
        }
        private UserPM GetLoggedUser(int tenant)
        {
            UserPM loggedUser;
            UserQuery userQuery = new UserQuery(tenant);
            if (AuthenticationUtil.AuthenticatedUserEmail != null)
            { // user set and passed from from WR
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, tenant);
            }
            else
            {
                ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);
                loggedUser = userQuery.GetSinglePM(loggedContact.Id, tenant);
            }

            if (loggedUser == null)
            {
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, 0);
            }

            if (loggedUser == null)
            {
                loggedUser = userQuery.GetSinglePMByEmail(AuthenticationUtil.AuthenticatedUserEmail, 0);
            }

            return loggedUser;
        }

        public GLAccount GetSingleByAccountId(string gLAccountId, int tenant)
        {
            GLAccount gLAccountPOCO = null;
            gLAccountPOCO = repository.GetGLAccountByIdTenant(gLAccountId, tenant);

            return gLAccountPOCO;
        }

        public string GetGLAccountDisplayNoAndLocalName(string gLAccountId, int tenant)
        {
            GLAccount gLAccountPOCO = null;
            gLAccountPOCO = repository.GetGLAccountByIdTenant(gLAccountId, tenant);
            string result = gLAccountPOCO.DisplayNumber + ',' + gLAccountPOCO.LocalName;
            return result;
        }
        public string GetDisplayNumberByGLAccountId(string gLAccountId, int tenant)
        {
            string result = repository.GetDisplayNumberByGLAccountId(gLAccountId, tenant);
            return result;
        }

        public GLAccountPM GetSinglePMByInternalNumber(string internalNumber, int tenant)
        {
            GLAccount gLAccountPOCO = null;
            gLAccountPOCO = repository.GetGLAccountByInternalNoAndTenant(internalNumber, tenant);
            GLAccountPM pm = this.GetEntityPM(gLAccountPOCO);
            return pm;
        }
        public GLAccountPM GetSinglePMByDisplayNumber(string displayNo, int tenant)
        {
            GLAccount gLAccountPOCO = null;
            gLAccountPOCO = repository.GetGLAccountByDisplayNoAndTenant(displayNo, tenant);
            GLAccountPM pm = this.GetEntityPM(gLAccountPOCO);
            return pm;
        }

        public List<GLAccountPM> GetByRevaluationEnabled_OtherParams(bool? revaluationEnabled, string chartOfAccountsTypeCode, string chartOfAccountsId, string accountTypeCode, string gLAccountId, string accountingCurrencyId, int tenant)
        {
            List<GLAccount> gLAccountPOCOs = null;
            gLAccountPOCOs = repository.GetByRevaluationEnabled_OtherParams(revaluationEnabled, chartOfAccountsTypeCode, chartOfAccountsId, accountTypeCode, gLAccountId, accountingCurrencyId, tenant);
            List<GLAccountPM> pms = gLAccountPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public List<GLAccountPM> GetChildAccounts(string GLAccountId, int tenant)
        {
            var pms = this.repository.GetChildAccounts(GLAccountId, tenant);
            return pms.Select(rec => this.GetEntityPM(rec)).ToList();

        }
        internal List<GLAccountPM> GetChildAccountsByChartOfAccountIdList(List<string> chartOfAccountIdList, int tenant)
        {
            var pms = this.repository.GetChildAccountsByChartOfAccountIdList(chartOfAccountIdList, tenant);
            return pms.Select(rec => this.GetEntityPM(rec)).ToList();
        }
        public List<GLAccountPM> GetByGLAccountsIdList(List<String> GLAccountsIdList, int tenant)
        {
            var pms = this.repository.GetByGLAccountsIdList(GLAccountsIdList, tenant);
            return pms.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public GLAccountPM GetControlGLAccountByChart(String chartOfAccountsId, int tenant)
        {
            var rec = this.repository.GetControlGLAccountByChart(chartOfAccountsId, tenant);
            return this.GetEntityPM(rec);
        }

        internal IQueryable<GLAccount> GetQAllControlAccount(int tenant)
        {
            return this.repository.GetQAllControlAccount(tenant);

        }
        public List<GLAccountCurrencyBalance> GetCurrencyBalances(GLAccountPM gLAccountPM, DateTime revaluationDate, int tenant)
        {
            DateTime revDate = revaluationDate.Date;
            List<GLAccountCurrencyBalance> rvList = new List<GLAccountCurrencyBalance>();
            if (gLAccountPM != null)
            {
                DateTime monthLastDate = GetDate(revDate);

                GLAccountTotalByMonthQueryService gLAccountTotalByMonthsQueryServices = new GLAccountTotalByMonthQueryService(tenant);
                DateTime monthUpTo = gLAccountTotalByMonthsQueryServices.GLAccountMonthTotalsUpToDate(gLAccountPM.Id, monthLastDate, tenant);
                DateTime noMonthsComputed = DateTime.MinValue; // no months computed 
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);

                List<CurrencySum> allSum;
                if (monthUpTo != noMonthsComputed)
                {
                    List<CurrencySum> monthsSum = gLAccountTotalByMonthsQueryServices.GetSumByMonth(gLAccountPM.Id, monthUpTo.Year, monthUpTo.Month, tenant);
                    if (monthLastDate == revDate && (monthUpTo.Year == revDate.Year && monthUpTo.Month == revDate.Month)) // rev date is last day of month AND we have all the sums computed already 
                    {
                        allSum = monthsSum;
                    }
                    else // here we need to compute the remaining period
                    //      from the 1th of the next month after monthUpTo    to the revaluationDate
                    // then add it to the monthsSum
                    {
                        DateTime fromD = new DateTime(monthUpTo.Year, monthUpTo.Month, 1);
                        fromD = fromD.AddMonths(1);
                        List<CurrencySum> remainingSum = ledgerTransactionQueryService.GetLedgerTransactionTotalLocalAmountFromTo(gLAccountPM.Id, fromD, revDate, tenant);
                        allSum = MergeLists(monthsSum, remainingSum);
                    }
                }
                else // here we need to compute all the period up to the rev date
                {
                    allSum = ledgerTransactionQueryService.GetLedgerTransactionTotalLocalAmountFromTo(gLAccountPM.Id, DateTime.MinValue, revDate, tenant);
                }

                foreach (CurrencySum item in allSum)
                {
                    GLAccountCurrencyBalance balance = new GLAccountCurrencyBalance
                    {
                        AccountId = item.AccountId,
                        CurrencyId = item.CurrencyId,
                        ForeignAmount = (decimal)item.ForeignAmountDebit - (decimal)item.ForeignAmountCredit,
                        LocalAmount = (decimal)item.LocalAmountDebit - (decimal)item.LocalAmountCredit,
                    };
                    rvList.Add(balance);
                }
            }
            return rvList;
        }

        private static DateTime GetDate(DateTime revaluationDate)
        {
            DateTime monthLastDate;
            DateTime nextDay = revaluationDate.AddDays(1d);
            if (nextDay.Month == revaluationDate.Month) // revaluationDate is not the last day of its month
            {
                DateTime fromDate = new DateTime(revaluationDate.Year, revaluationDate.Month, 1); // 1st of the same month
                monthLastDate = fromDate.AddDays(-1d); // the last day of the previous month
            }
            else
            {
                monthLastDate = revaluationDate; // the same
            }
            return monthLastDate;
        }


        private static List<CurrencySum> MergeLists(List<CurrencySum> left, List<CurrencySum> right)
        {
            List<CurrencySum> rvList;
            if (left == null || left.Count == 0)
            {
                rvList = right;
            }
            else if (right == null || right.Count == 0)
            {
                rvList = left;
            }

            else
            {
                rvList = new List<CurrencySum>();
                foreach (CurrencySum item in left)
                {
                    CurrencySum rightItem = right.Where(d => d.AccountId == item.AccountId && d.CurrencyId == item.CurrencyId).FirstOrDefault();
                    if (!(rightItem == null || String.IsNullOrEmpty(rightItem.AccountId)))
                    {
                        item.ForeignAmountCredit += rightItem.ForeignAmountCredit;
                        item.ForeignAmountDebit += rightItem.ForeignAmountDebit;
                        item.LocalAmountCredit += rightItem.LocalAmountCredit;
                        item.LocalAmountDebit += rightItem.LocalAmountDebit;
                        right.Remove(rightItem);
                    }
                    rvList.Add(item);
                }
                foreach (CurrencySum item in right) // all that remains in the right (and has not been also in the left)
                {
                    rvList.Add(item);
                }
            }

            return rvList;
        }


        public GLAccountPM GetByInternalNumber(string internalNumber, int tenant)
        {

            var Ids = this.repository.GetIdsByInternalNumber(internalNumber, tenant);
            if (Ids.Count == 0)
            {
                return null;
            }

            GLAccountPM account = this.GetSingle(Ids.FirstOrDefault(), false, true);
            SetGLAccountMoreDataFields(account);
            return account;
            //List<GLAccount> pocos = this.repository.GetByInternalNumber(internalNumber, tenant);
            //return pocos.Select(rec => this.GetEntityPM(rec)).ToList();
        }
        private void SetGLAccountMoreDataFields(GLAccountPM gLAccount)
        {
            GLAccountMoreDataPM gLAccountMoreData = GetGLAccountMoreData(gLAccount);
            if (gLAccountMoreData != null)
            {
                gLAccount.TotalOpenChequesInLocalCur = gLAccountMoreData.TotalOpenChequesInLocalCur;
                gLAccount.TotFutureOpenChequesInLocalCur = gLAccountMoreData.TotFutureOpenChequesInLocalCur;
            }
        }
        private GLAccountMoreDataPM GetGLAccountMoreData(GLAccountPM gLAccount)
        {
            GLAccountMoreDataQueryService gLAccountMoreDataQueryService = new GLAccountMoreDataQueryService(gLAccount.Tenant);
            return gLAccountMoreDataQueryService.GetSinglePMByAccountId(gLAccount.Id, gLAccount.Tenant);

        }
        public List<GLAccountPM> GetByDisplayNumber(string displayNumber, int tenant)
        {
            List<GLAccount> pocos = this.repository.GetByDisplayNumber(displayNumber, tenant);
            return pocos.Select(rec => this.GetEntityPM(rec)).ToList();
        }
        public List<GLAccountPM> GetByDisplayNumberEnding(string displayNumberEnding, int tenant)
        {
            List<GLAccount> pocos = this.repository.GetByDisplayNumberEnding(displayNumberEnding, tenant);
            return pocos.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public List<GLAccount> GetByDisplayNumberAndAccType(string displayNumber, string accTypeCode, int tenant)
        {
            List<GLAccount> pocos = this.repository.GetByDisplayNumberAndAccType(displayNumber, accTypeCode, tenant);
            return pocos;
        }

        public IQueryable<GLAccountPM> GetSplittedByCurrencyGLAccounts(string accountId, int tenant)
        {

            IQueryable<GLAccountPM> Accounts = from a in context.GLAccounts
                                               join
                  c in context.GLAccountCurrencies on a.Id equals c.GLAccountId
                                               join MoreDatas in context.GLAccountMoreDatas on a.Id equals MoreDatas.AccountId
                                               where c.MainGLAccountId == accountId && a.Tenant == tenant
                                               select new GLAccountPM()
                                               {
                                                   Id = a.Id,
                                                   CreateDate = a.CreateDate,
                                                   CurrencyId = c.CurrencyId,
                                                   DisplayNumber = c.GLAccount != null ? c.GLAccount.DisplayNumber : null,
                                                   Inactive = a.Inactive,
                                                   CurrencyCode = c.Currency != null ? c.Currency.Code : null,
                                                   ChartOfAccountsId = c.GLAccount != null ? c.GLAccount.ChartOfAccountsId : null,
                                                   ChartOfAccountsTypeCode = c.GLAccount != null ? c.GLAccount.ChartOfAccountsTypeCode : null,
                                                   LocalName = c.GLAccount != null ? c.GLAccount.LocalName : null,
                                                   ReconcileMethodCode = c.GLAccount != null ? c.GLAccount.ReconcileMethodCode : null,
                                                   RevenueExpenseType = c.GLAccount != null ? c.GLAccount.RevenueExpenseType : null,
                                                   Tenant = tenant,
                                                   BalanceInLocalCurrency = MoreDatas.BalanceInLocalCurrency,

#if GLAccMoreData
                                                   //BalanceInLocalCurrency = c.GLAccount != null? c.GLAccount.BalanceInLocalCurrency:null,
                                                   LocalBalanceInDue = c.GLAccount != null ? c.GLAccount.LocalBalanceInDue : null,
                                                   NextDueDate = c.GLAccount != null ? c.GLAccount.NextDueDate : null,
#endif
                                                   AccountTypeCode = c.GLAccount.AccountTypeCode,
                                                   AutomaticReconcileId = c.GLAccount.AutomaticReconcileId,
                                                   ControlAccountId = c.GLAccount.ControlAccountId,
                                                   CustomerGLAccountId = c.GLAccount.CustomerGLAccountId,
                                                   InternalNumber = c.GLAccount.InternalNumber

                                               };
            return Accounts;
        }
        public List<string> GetSplittedByCurrencyGLAccountIds(string accountId, int tenant)
        {
            IQueryable<GLAccount> glaccounts = repository.GetAll(tenant);
            return (from a in glaccounts
                    join
                   c in context.GLAccountCurrencies on a.Id equals c.GLAccountId
                    where c.MainGLAccountId == accountId && a.Tenant == tenant && a.ActiveForInterest == true
                    select a.Id).ToList();

        }
        public List<string> GetChildrenByCurrencyGLAccountIds(string accountId, int tenant)
        {
            IQueryable<GLAccount> glaccounts = repository.GetAll(tenant);
            return (from a in glaccounts
                    join
                   c in context.GLAccountCurrencies on a.Id equals c.GLAccountId
                    where c.MainGLAccountId == accountId && a.Tenant == tenant
                    select a.Id).ToList();

        }

        public IQueryable<GLAccountPM> GetChildrenGLAccounts(string accountId, int tenant)
        {

            IQueryable<GLAccountPM> Accounts = from a in context.GLAccounts
                                               where a.ParentAccountId == accountId && a.Tenant == tenant
                                               select new GLAccountPM()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   InternalNumber = a.InternalNumber,
                                                   AccountTypeCode = a.AccountTypeCode,
                                                   DisplayNumber = a.DisplayNumber,
                                                   EnglishName = a.EnglishName,
                                                   LocalName = a.LocalName,
                                                   SearchFields = a.SearchFields,
                                                   IsMultiCurrency = a.IsMultiCurrency,
                                                   CurrencyId = a.CurrencyId,
                                                   RevenueExpenseType = a.RevenueExpenseType,
                                                   IsControlAccount = a.IsControlAccount,
                                                   ChartOfAccountsId = a.ChartOfAccountsId,
                                                   Inactive = a.Inactive,
                                                   ReconcileMethodCode = a.ReconcileMethodCode,
                                                   ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                                                   AccountTypeName = a.GLAccountType != null ? a.GLAccountType.EnglishName : null,
                                                   RevenueExpenseName = a.RevenueExpense != null ? a.RevenueExpense.EnglishName : null,
                                                   ReconcileMethodName = a.ReconcileMethod != null ? a.ReconcileMethod.EnglishName : null,
                                                   CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                   ChartOfAccountsTypeName = a.ChartOfAccountsType != null ? a.ChartOfAccountsType.EnglishName : null,

                                                   CurrencySign = a.IsMultiCurrency == true ? "" : a.Currency != null ? a.Currency.Sign : null,
                                                   ControlAccountName = a.ControlAccount != null ? a.ControlAccount.EnglishName : null,
                                                   ControlAccountId = a.ControlAccountId,
                                                   ControlAccountNumber = a.ControlAccount != null ? a.ControlAccount.DisplayNumber : null,
                                                   ChartOfAccountsName = a.ChartOfAccount != null ? a.ChartOfAccount.LocalName : null,

                                                   AutomaticReconcileId = a.AutomaticReconcileId,

                                                   PreviousEnglishName = a.PreviousEnglishName,
                                                   PreviousEnglishNameChangeDate = a.PreviousEnglishNameChangeDate,
                                                   PreviousLocalName = a.PreviousLocalName,
                                                   PreviousLocalNameChangeDate = a.PreviousLocalNameChangeDate,
                                                   PreviousNumber = a.PreviousNumber,
                                                   PreviousNumberChangeDate = a.PreviousNumberChangeDate,
                                                   PreviousChartOfAccountsId = a.PreviousChartOfAccountsId,
                                                   PreviousChartOfAccountsChangeDate = a.PreviousChartOfAccountsChangeDate,

                                                   CustomerGLAccountId = a.CustomerGLAccountId,

                                                   RevaluationEnabled = a.RevaluationEnabled,

                                                   ParentAccountId = a.ParentAccountId,
                                                   IsVATExempt = a.IsVATExempt,


                                               };
            return Accounts;
        }


        public static TaxDeductionReportData taxDeduction;



        public TaxDeductionReportData GetTaxDeductionReportData(int? reportYear, int tenant)
        {

            Simplog.Data.CommonDataModel.ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            IInvoiceContext invoicecontext = InvoiceContext.GetContext(tenant);
            taxDeduction = new TaxDeductionReportData();
            //    taxDeduction.TaxYear = reportYear;
            // 1- get ap payments by year and status ad
            // 2- group appayments by vendorId and percentage
            // 3- Get Cards by list of vendorids from step 2
            // 4- Get GlAccounts by list of glaccountids from step 3
            // 5- GetAddresses by card ids from step 3 with filter of main address.
            taxDeduction.ByVendorList = new List<ByVendorList>();
            List<APPayment> payments = (from a in invoicecontext.APPayments
                                        where a.RegisterDate.Value.Year == reportYear && a.Tenant == tenant && a.StatusCode == "AD"
                                        select a).ToList();




            List<string> vendorIds = payments.Select(d => d.VendorId).ToList();
            List<CardList> vendors = (from a in commoncontext.Cards
                                      where
         vendorIds.Contains(a.Id) && a.CountryCode == "IL"
                                      select new CardList()
                                      {
                                          Id = a.Id,
                                          CityName = a.CityName,

                                          GLAccountId = a.GLAccountId,
                                          IsAutonomy = a.IsAutonomy,
                                          IsInternationalPartner = a.IsInternationalPartner,
                                          EnglishName = a.EnglishName,
                                          VatNumber = a.VatNumber,
                                          LocalName = a.LocalName
                                      }
                                      ).ToList();

            List<GLAccountList> glaccounts = (from a in context.GLAccounts.Include("AccountingCompanyType").Include("TaxWithholdingAssessOffice").Include("WithholdingTaxDeductionType")
                                              where a.AccountTypeCode == "3" && a.ExcludeFromDeductionReport == false && a.Tenant == tenant

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



            List<DBVendorsList> DBVendorsList = (from a in payments
                                                 join v in vendors on a.VendorId equals v.Id
                                                 join g in glaccounts on v.GLAccountId equals g.Id
                                                 select new DBVendorsList()
                                                 {
                                                     GlAccountId = g.Id,
                                                     VendorId = a.VendorId,
                                                     AmountInLocalCurrency = a.AmountInLocalCurrency,
                                                     TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                                                     DeductionFileTypeCode = g.DeductionFileTypeCode,
                                                     RigesterDate = a.RegisterDate,

                                                 }).ToList();
            DateTime fromdate = new DateTime((int)reportYear, 1, 1);
            DateTime todate = new DateTime((int)reportYear, 12, 31);


            List<APPaymentList> groupedpayments = (from a in payments
                                                   join v in vendors on a.VendorId equals v.Id
                                                   join g in glaccounts on v.GLAccountId equals g.Id
                                                   group a by
                                new { a.VendorId, a.TaxDeductionPercentage } into g
                                                   select new APPaymentList
                                                   {
                                                       VendorId = g.Key.VendorId,
                                                       TaxDeductionPercentage = g.Key.TaxDeductionPercentage,
                                                       AmountInLocalCurrency = g.Sum(s => s.AmountInLocalCurrency),
                                                       TaxDeductionLocalAmount = g.Sum(s => s.TaxDeductionLocalAmount),
                                                       RegisterDate = g.Select(s => s.RegisterDate).FirstOrDefault(),
                                                   } into s
                                                   select s).ToList();




            var groupedByMonthpayments = (from a in payments
                                          group a by
                                           new { a.RegisterDate.Value.Month } into g
                                          select new
                                          {

                                              g.Key
                                          }).ToList();



            glaccounts = (from a in glaccounts
                          join v in vendors on a.Id equals v.GLAccountId
                          select a).ToList();



            foreach (APPaymentList item in groupedpayments)
            {
                ByVendorList byVendorList = new ByVendorList()
                {
                    Month = item.RegisterDate.Value.Month,
                    TaxDeductionPercentage = item.TaxDeductionPercentage,
                    VendorId = item.VendorId,


                };
                byVendorList.EndYearBalance = 0;
                CardList selectedVendor = vendors.Where(d => d.Id == item.VendorId).FirstOrDefault();
                if (selectedVendor != null)
                {
                    GLAccountList gLAccount = glaccounts.Where(d => d.Id == selectedVendor.GLAccountId).FirstOrDefault();

                    if (gLAccount != null)
                    {
                        byVendorList.DisplayNumber = gLAccount.DisplayNumber;

                        byVendorList.Occupation = gLAccount.Occupation;
                        byVendorList.GLAccountLocalName = gLAccount.LocalName;
                        byVendorList.AssessingOfficerCode = gLAccount.AssessingOfficeCode;
                        byVendorList.AssessingOfficerName = gLAccount.AssessingOfficeName;
                        byVendorList.DeductionFileTypeCode = gLAccount.DeductionFileTypeCode;
                        byVendorList.DeductionFileNumber = gLAccount.DeductionFileNumber;
                        byVendorList.DeductionType = gLAccount.DeductionTypeId;
                        byVendorList.EnglishName = gLAccount.EnglishName;
                        //IGrouping<string, TrailReportM> trailReportM = null;
                        //if (result.ContainsKey(gLAccount.Id))
                        //{
                        //    trailReportM = result[gLAccount.Id];
                        //}
                        //if (trailReportM != null)
                        //{
                        //    byVendorList.EndYearBalance = trailReportM.Select(d => d.LocalOpenBalance).Sum();
                        //}
                        LedgerTransactionBalanceFilter LTBFilter = new LedgerTransactionBalanceFilter();


                        LTBFilter.PageSize = 30;
                        LTBFilter.PageStartAtRecordIndex = 0;
                        LTBFilter.Tenant = tenant;



                        LTBFilter.GLAccountId = gLAccount.Id;
                        DateTime today = DateTime.Today;
                        LTBFilter.From = today.AddMonths(-1);

                        LTBFilter.To = today;
                        LTBFilter.IncludeRelatedCurrenciesAccount = false;
                        LTBFilter.IncludeChildAccounts = false;
                        LTBFilter.DateTypeCode = "1";
                        var ledgerTransactionBalanceService = new LedgerTransactionBalanceService(context, LTBFilter);
                        ledgerTransactionBalanceService.Run();
                        LTBFilter.CallBack = new LedgerTransactionBalanceFilterCallBack()
                        {
                            EndBalanceLocal = ledgerTransactionBalanceService.Response.EndBalanceLocal,

                        };
                        byVendorList.EndYearBalance = Math.Round((ledgerTransactionBalanceService.Response.EndBalanceLocal != null ? ledgerTransactionBalanceService.Response.EndBalanceLocal : 0).Value, 0);


                        if (byVendorList.EndYearBalance >= 0)
                        {
                            byVendorList.EndYearBalance = 0;
                        }

                    }


                    if (byVendorList.EndYearBalance == null)
                    {
                        byVendorList.EndYearBalance = 0;
                    }

                    byVendorList.VATNumber = selectedVendor.VatNumber;
                    byVendorList.VendorName = selectedVendor.EnglishName;
                    byVendorList.VendorAddress = selectedVendor.MainAddressId;
                    byVendorList.VendorCity = selectedVendor.CityName;
                    byVendorList.IsAutonomy = selectedVendor.IsAutonomy;
                    byVendorList.IsInternationlPartner = selectedVendor.IsInternationalPartner;
                    byVendorList.VendorLocalName = selectedVendor.LocalName;
                }
                byVendorList.SumOfAmountInLocalCurrency = Math.Round(item.AmountInLocalCurrency.Value, 0);
                byVendorList.SumOfTaxDeductionLocalAmount = Math.Round(item.TaxDeductionLocalAmount.Value, 0);



                taxDeduction.ByVendorList.Add(byVendorList);

            }

            taxDeduction.ByMonthList = new List<ByMonthList>();
            List<int> months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            foreach (var item in months)

            {
                var month = item;
                //  var year = item.Key.RegisterDate.Value.Year.ToString().Substring(2, 2);
                ByMonthList byMonthList = new ByMonthList()
                {
                    Month = month,
                    TotalVendors = DBVendorsList.Where(d => d.RigesterDate.Value.Month == month).GroupBy(d => d.VendorId).Count(),
                    TotalPaymentsWithoutDivided = Math.Round(DBVendorsList.Where(d => d.RigesterDate.Value.Month == month && d.DeductionFileTypeCode != "18").Sum(d => d.AmountInLocalCurrency).Value, 0),
                    TotalDeductionsWithoutDivided = Math.Round(DBVendorsList.Where(d => d.RigesterDate.Value.Month == month && d.DeductionFileTypeCode != "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                    TotalDivided = Math.Round(DBVendorsList.Where(d => d.RigesterDate.Value.Month == month && d.DeductionFileTypeCode == "18").Sum(d => d.AmountInLocalCurrency).Value, 0),
                    TotalDeductionsFromDivided = Math.Round(DBVendorsList.Where(d => d.RigesterDate.Value.Month == month && d.DeductionFileTypeCode == "18").Sum(d => d.TaxDeductionLocalAmount).Value, 0),
                    ReportMonth = month + "." + reportYear,
                };



                taxDeduction.ByMonthList.Add(byMonthList);
            }


            FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant);
            FullAccountingSettingPM setting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            taxDeduction.VendorsCount = DBVendorsList.GroupBy(d => d.VendorId).Count();
            taxDeduction.TotalAmountInLocalCurrency = Math.Round(DBVendorsList.Sum(d => d.AmountInLocalCurrency).Value, 0);
            taxDeduction.TotalDeductionInLocalCurrency = Math.Round(DBVendorsList.Sum(d => d.TaxDeductionLocalAmount).Value, 0);
            taxDeduction.TotalAmountInLocalCurrency08 = Math.Round(DBVendorsList.Where(d => d.DeductionFileTypeCode == "08").Sum(d => d.AmountInLocalCurrency).Value, 0);
            taxDeduction.TotalTaxDeductionInLocalCurrency08 = Math.Round(DBVendorsList.Where(d => d.DeductionFileTypeCode == "08").Sum(d => d.TaxDeductionLocalAmount).Value, 0);
            if (taxDeduction.ByVendorList != null)
            {
                taxDeduction.TotalEndBalance = Math.Round(taxDeduction.ByVendorList.Sum(d => d.EndYearBalance).Value, 0);//  DBVendorsList.Sum(d => d.EndYearBalance).Value,0);
            }

            //if (result.ContainsKey(item.GLAccountId))
            //{
            //    trailReportM = result[item.GLAccountId];
            //}
            //if (trailReportM != null)
            //{
            //    item.OpeningBalance = trailReportM.Select(d => d.LocalOpenBalance).Sum();
            return taxDeduction;

            //}
        }

        public List<B110Data> GetB110sForGLAccounts(int tenant)
        {

            List<B110Data> b110s = new List<B110Data>();
            //List<GLAccountPM> glaccounts = (from a in context.GLAccounts
            //                                join c in context.ChartOfAccounts on a.ChartOfAccountsId equals c.Id

            //                                where a.Inactive == false && a.Tenant == tenant
            //                                select new GLAccountPM()
            //                                {
            //                                    Id = a.Id,
            //                                    Tenant = a.Tenant,
            //                                    ChartOfAccountsCode = c.Code,
            //                                    DisplayNumber = a.DisplayNumber,
            //                                    LocalName = a.LocalName,
            //                                    EnglishName = a.EnglishName,
            //                                    ChartOfAccountsName = c.LocalName,
            //                                    AccountTypeCode = a.AccountTypeCode,
            //                                    IsMultiCurrency = a.IsMultiCurrency,
            //                                    CurrencyId = a.CurrencyId,
            //                                    CurrencyCode = a.Currency.Code
            //                                }).ToList();


            //ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            //List<AddressData> addresses = (from a in commonDataContext.Cards
            //                             join d in commonDataContext.Addresses on a.Id equals d.CardId
            //                             join dt in commonDataContext.AddressTypes on d.AddressTypeId equals dt.Id
            //                             where a.Tenant == tenant
            //                             select new AddressData()
            //                             {
            //                                 GLAccountId = a.GLAccountId,
            //                                CardId= a.Id,
            //                                AddressType = dt.Id,
            //                                VatNumber = a.VatNumber,
            //                                CountryCode = a.CountryCode,
            //                                City = a.CityName,
            //                                CountryName = a.CountryName,
            //                                Address1 = d.Address1,
            //                                ZipCode = a.ZipCode,
            //                             }).ToList();


            b110s = (from a in context.GLAccounts
                     join c in context.ChartOfAccounts on a.ChartOfAccountsId equals c.Id

                     where a.Tenant == tenant

                     select new B110Data()
                     {
                         ChartOfAccountsCode = c.Code,
                         DisplayNumber = a.DisplayNumber,
                         LocalName = a.LocalName,
                         EnglishName = a.EnglishName,
                         ChartOfAccountsName = c.LocalName,
                         AccountTypeCode = a.AccountTypeCode,
                         GLAccountId = a.Id,
                         IsMultiCurrency = a.IsMultiCurrency,
                         CurrecnyId = a.CurrencyId,
                         CustomerGLAccountId = a.CustomerGLAccountId,
                         CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                     }).ToList();


            return b110s;

        }

        public List<GLAccountPM> GetGLAccountsByIds(List<string> Ids, int tenant)
        {
            List<GLAccountPM> Accounts = (from a in context.GLAccounts
                                          where Ids.Contains(a.Id) && a.Tenant == tenant
                                          select new GLAccountPM()
                                          {
                                              Id = a.Id,
                                              Tenant = a.Tenant,
                                              InternalNumber = a.InternalNumber,
                                              AccountTypeCode = a.AccountTypeCode,
                                              DisplayNumber = a.DisplayNumber,

                                          }).ToList();
            return Accounts;
        }

        public GLAccountPM GetSplittedByCurrencyGLAccount(string accountId, int tenant, string currency)
        {

            return (from a in context.GLAccounts
                    where a.CustomerGLAccountId == accountId && a.CurrencyId == currency && a.Tenant == tenant
                    && a.Inactive == false//Task 61118: Service for retrieving the splitted GLAccounts- change logic if GLAccountCurrencies is block
                    select new GLAccountPM()
                    {
                        Id = a.Id,
                        CurrencyId = a.CurrencyId,
                        DisplayNumber = a.DisplayNumber,
                        Inactive = a.Inactive,
                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                        ChartOfAccountsId = a.ChartOfAccountsId,
                        ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                        LocalName = a.LocalName,
                        ReconcileMethodCode = a.ReconcileMethodCode,
                        RevenueExpenseType = a.RevenueExpenseType,
                        Tenant = tenant,
                        AccountTypeCode = a.AccountTypeCode,
                        AutomaticReconcileId = a.AutomaticReconcileId,
                        ControlAccountId = a.ControlAccountId,
                        CustomerGLAccountId = a.CustomerGLAccountId,
                        InternalNumber = a.InternalNumber

                    }).FirstOrDefault();
        }

        public GLAccountPM GetSplittedGLAccount(string accountId, int tenant, string currency)
        {
            IQueryable<GLAccount> glaccounts = repository.GetAll(tenant);
            return (from a in glaccounts
                    join
                   c in context.GLAccountCurrencies on a.Id equals c.GLAccountId
                    where c.MainGLAccountId == accountId && c.CurrencyId == currency && a.Tenant == tenant && a.Inactive == false
                    select new GLAccountPM()
                    {
                        Id = a.Id,
                        CurrencyId = a.CurrencyId,
                        DisplayNumber = a.DisplayNumber,
                        Inactive = a.Inactive,
                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                        ChartOfAccountsId = a.ChartOfAccountsId,
                        ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                        LocalName = a.LocalName,
                        ReconcileMethodCode = a.ReconcileMethodCode,
                        RevenueExpenseType = a.RevenueExpenseType,
                        Tenant = tenant,
                        AccountTypeCode = a.AccountTypeCode,
                        AutomaticReconcileId = a.AutomaticReconcileId,
                        ControlAccountId = a.ControlAccountId,
                        CustomerGLAccountId = a.CustomerGLAccountId,
                        InternalNumber = a.InternalNumber

                    }).FirstOrDefault();
        }

        public GLAccountPM GetGLAccountByCardId(string cardId, int tenant)
        {
            CardPM card = GetCardById(cardId, tenant);
            if (card.GLAccountId != null)
            {
                return GetSinglePM(card.GLAccountId, tenant);
            }
            return null;
        }
        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }

        public static int Update_ConnectCardToGLAccount(string cardId, int tenant, string gLAccountId, string gLAccountDisplayNumber)
        {
            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE Cards SET GLAccountId= @V_gLAccountId , GLAccountDisplayNumber= @V_gLAccountDisplayNumber " +
                        "WHERE Id =@V_cardId and tenant= @V_tenant";

                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@V_tenant", SqlDbType.Int);
                    command.Parameters["@V_tenant"].Value = tenant;

                    command.Parameters.Add("@V_cardId", SqlDbType.VarChar);
                    command.Parameters["@V_cardId"].Value = cardId;

                    command.Parameters.Add("@V_gLAccountId", SqlDbType.VarChar);
                    command.Parameters["@V_gLAccountId"].Value = gLAccountId;

                    command.Parameters.Add("@V_gLAccountDisplayNumber", SqlDbType.VarChar);
                    command.Parameters["@V_gLAccountDisplayNumber"].Value = gLAccountDisplayNumber;


                    int rows = command.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
            }
        }

        public void ConnectCardToGLAccount(CardGLAccountConnectionArgs args)
        {
            //  CardPM cardPM = GetCardById(args.CardId, args.Tenant);
            IAccountingContext context = MainContext as AccountingContext;
            GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(context, new Dictionary<string, IContext>(), args.Tenant);

            if (!args.SkipConnectedCardsValidation)
                CheckConnectCards(args.AccountId, args.CardId, args.Tenant);

            //   cardPM.GLAccountId = args.AccountId;
            //   cardPM.GLAccountDisplayNumber =  GetDisplayNumberByGLAccountId(args.AccountId, args.Tenant);
            string displayNumber = GetDisplayNumberByGLAccountId(args.AccountId, args.Tenant);

            if (IsFullAccountingActivated(args.Tenant))
            {
                CreateTraceEvent(args.AccountId, args.AccountId, args.Tenant, "GLAccount", "DSCS");
                CreateTraceEvent(args.CardId, args.AccountId, args.Tenant, "Customer", "CSCS");
            }

            //SubmitCard(cardPM);
            if (!String.IsNullOrEmpty(args.CardId) && !String.IsNullOrEmpty(args.AccountId) && args.Tenant > 0)
            {
                Update_ConnectCardToGLAccount(args.CardId, args.Tenant, args.AccountId, displayNumber);
                gLAccountUpdateService.UpdateCardDisplayNumber(args.Tenant, args.CardId, displayNumber);
            }

            //ICommonDataContext context = CommonDataContext.GetContext(args.Tenant);
            //CardRepository cardRepository = new CardRepository(context);
            //Card card = null;
            //CardMapping.MapEntity(cardPM, card, false);
            //cardRepository.Update(card);
            //cardRepository.SubmitChanges();
            //if(cardPM.PartnerTypeId == PartnerTypeValues.Vendor)
            //{
            //    cardPM.GLAccountId = accountId;
            //    SubmitCard(cardPM);
            //}
            //else
            //{
            //    if(!skipConnectedCardsValidation)
            //        CheckConnectCards(accountId, tenant);

            //    cardPM.GLAccountId = accountId;
            //    SubmitCard(cardPM);
            //}

        }

        private void CreateTraceEvent(string CardId, string AccountId, int Tenant, string objectTableName, string eventTypeCode)
        {
            // ContactPM loggedContact = new ContactQuery(Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), Tenant);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = CardId,
                Tenant = Tenant,
                //    UserId = loggedContact.Id,//contact.Id,
                ObjectTableName = objectTableName,
                IsAddedManually = false,
                EventTypeCode = eventTypeCode,
                Notes = SetNotesForConnectGLAccountEvent(AccountId, Tenant),
            });
        }
        private string SetNotesForConnectGLAccountEvent(string id, int tenant)
        {
            //   GLAccountPM glaccount = this.GetSingle(id, false, false);
            GLAccountPMLite accLite = this.GetLiteById(id, tenant);

            return string.Concat("Internal number: ", accLite.InternalNumber, "\nLocal name: ", accLite.LocalName);
        }

        private GLAccountPMLite GetLiteById(string id, int tenant)
        {
            GLAccountPMLite rv = new GLAccountPMLite();
            if (!String.IsNullOrEmpty(id))
            {
                List<GLAccount> GLAccounts = this.repository.GetAll(tenant).Where(s => s.Id == id).ToList();
                if (GLAccounts != null && GLAccounts.Count == 1)
                {
                    GLAccount gLAccount = GLAccounts[0];
                    rv.Id = gLAccount.Id;
                    rv.InternalNumber = gLAccount.InternalNumber;
                    rv.DisplayNumber = gLAccount.DisplayNumber;
                    rv.EnglishName = gLAccount.EnglishName;
                    rv.LocalName = gLAccount.LocalName;
                }
            }
            return rv;
        }

        private void CheckConnectCards(string accountId, string cardId, int tenant)
        {
            List<CardList> connectedCards = GetConnectedCards(accountId, tenant);
            if (connectedCards.Count > 0)
            {
                string msg = "";
                CardPM cardPM = GetCardById(cardId, tenant);


                //if (cardPM.PartnerTypeId == PartnerTypeValues.CustomClearance || cardPM.PartnerTypeId == PartnerTypeValues.CustomAgent || cardPM.PartnerTypeId == PartnerTypeValues.CustomsShipper || cardPM.PartnerTypeId == PartnerTypeValues.Coloader)
                //    msg = GetCustomersErrorMessage(tenant, connectedCards);
                //else
                msg = GetVendorsWarningMessage(tenant, connectedCards);
                throw new ApplicationException(msg);
            }
        }

        private string GetCustomersErrorMessage(int tenant, List<CardList> connectedCards)
        {
            string connectedCardsCodes = string.Join(", ", connectedCards.Select(d => d.Code));

            string msg;
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            string textCode = TextCodesTranslator.TranslateText("GLAccount.O.ThisGLAccountConnected", tenant, showLocal);
            msg = string.IsNullOrWhiteSpace(textCode) ? "This GL Account is already connected to card (#cards). GL Account cannot be linked to two clients card." : textCode;
            msg = msg.Replace("#cards", connectedCardsCodes);
            return msg;
        }

        private string GetVendorsWarningMessage(int tenant, List<CardList> connectedCards)
        {
            string connectedCardsCodes = string.Join(", ", connectedCards.Select(d => d.Code));

            string msg;
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            string textCode = TextCodesTranslator.TranslateText("GLAccount.O.ThisGLAccountConnectedContinue", tenant, showLocal);
            msg = string.IsNullOrWhiteSpace(textCode) ? "This GL Account is already connected to cards (#cards), Do you want to continue?" : textCode;

            msg = msg.Replace("#cards", connectedCardsCodes);
            return msg;
        }

        public List<CardList> GetConnectedCards(string accountId, int tenant)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            List<CardList> connectCards = cardQuery.GetCardPMsByGLAccountId(accountId, tenant);
            return connectCards;
        }

        private void SubmitCard(CardPM cardPM)
        {
            CardService service = new CardService(CommonDataContext.GetContext(cardPM.Tenant), cardPM.Tenant);
            service.Update(cardPM);
        }

        private CardPM GetCardById(string cardId, int tenant)
        {
            CardQuery query = new CardQuery(tenant);
            CardPM cardPM = query.GetSinglePM(cardId, tenant);
            return cardPM;
        }

        public DateTime? GetInterestCalculationStartDate(string glaccountId, int tenant)
        {
            GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);
            DateTime? interestCalculationStartDate = gLAccountRepository.GetInterestCalculationStartDate(glaccountId, tenant);
            return interestCalculationStartDate;
        }



        public List<InterestReportCustomerPM> GetEligibleCustomersForInterestReports(int tenant)
        {
            GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);
            IQueryable<InterestReportCustomerData> interestReportCustomerDatas = gLAccountRepository.GetEligibleCustomersForInterestReports(tenant);
            List<InterestReportCustomerPM> gLAccountPMs = (from a in interestReportCustomerDatas
                                                           select new InterestReportCustomerPM()
                                                           {
                                                               GLAccountId = a.GLAccountId,
                                                               ActiveForInterest = a.ActiveForInterest,
                                                               InterestCalculationStartDate = a.InterestCalculationStartDate,
                                                               InterestCreditLimit = a.InterestCreditLimit,
                                                               MinimumInterestInvoiceBilling = a.MinimumInterestInvoiceBilling,
                                                               EnglishName = a.EnglishName,
                                                               LocalName = a.LocalName,
                                                               Tenant = a.Tenant,
                                                               CustomerId = a.CustomerId,

                                                           }).ToList();
            return gLAccountPMs;
        }


        public bool CheckInactiveGLAccounts(List<string> glaccountIds, int tenant)
        {
            List<GLAccount> glaccounts = GetGLAccountsByIdsList(glaccountIds, tenant);
            var hasInactiveAccounts = glaccounts.Any(a => a.Inactive == true);
            return hasInactiveAccounts;
        }

        private List<GLAccount> GetGLAccountsByIdsList(List<string> glaccountIds, int tenant)
        {
            GLAccountRepository gLAccountRepository = new GLAccountRepository(tenant);
            return gLAccountRepository.GetByGLAccountsIdList(glaccountIds, tenant);
        }

        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {
            IAccountingContext context = MainContext as AccountingContext;
            GLAccountUpdateService service = new GLAccountUpdateService(context, new Dictionary<string, IContext>(), tenatToCopy);
            GLAccountMoreDataUpdateService glAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(context, new Dictionary<string, IContext>(), tenatToCopy);


            ChartOfAccountRepository ChartOfAccountRepository = new ChartOfAccountRepository(context);
            List<ChartOfAccount> chartOfAccount = ChartOfAccountRepository.GetAll(tenatToCopy).Where(t => t.Inactive == false).ToList();
            //List<GLAccount> pocosIsControlAccount = repository.GetAll(tenant).Where(t => t.Inactive == false && t.IsControlAccount==true).ToList();
            List<GLAccount> pocosNotIsControlAccount = repository.GetAll(tenant).Where(t => t.Inactive == false && t.IsControlAccount == false).ToList();




            foreach (var item in pocosNotIsControlAccount)
            {
                GLAccountPM glAccountPM = new GLAccountPM()
                {
                    Tenant = tenatToCopy,
                    AccountTypeCode = item.AccountTypeCode,
                    DisplayNumber = item.DisplayNumber,
                    LocalName = item.LocalName,
                    EnglishName = item.EnglishName,
                    SearchFields = item.SearchFields,
                    IsMultiCurrency = item.IsMultiCurrency,
                    CurrencyId = item.CurrencyId,
                    RevenueExpenseType = item.RevenueExpenseType,
                    IsControlAccount = item.IsControlAccount,
                    ChartOfAccountsId = chartOfAccount?.Where(c => c.TypeCode == item.ChartOfAccountsTypeCode).FirstOrDefault().Id,
                    Inactive = item.Inactive,
                    ChartOfAccountsTypeCode = item.ChartOfAccountsTypeCode,
                    ReconcileMethodCode = item.ReconcileMethodCode,
                    ControlAccountId = item.ChartOfAccountsTypeCode == "3" ? pocosNotIsControlAccount?.Where(t => t?.ChartOfAccountsTypeCode == "3").FirstOrDefault().Id : item.ChartOfAccountsTypeCode == "4" ? pocosNotIsControlAccount?.Where(t => t?.ChartOfAccountsTypeCode == "4").FirstOrDefault().Id : null,
                    AutomaticReconcileId = item.AutomaticReconcileId,
                    PreviousEnglishName = item.PreviousEnglishName,
                    PreviousLocalName = item.PreviousLocalName,
                    PreviousNumber = item.PreviousNumber,
                    PreviousChartOfAccountsId = item.PreviousChartOfAccountsId,
                    CustomerGLAccountId = item.CustomerGLAccountId,
                    RevaluationEnabled = item.RevaluationEnabled,
                    ParentAccountId = item.ParentAccountId,
                    Category1Id = item.Category1Id,
                    Category2Id = item.Category2Id,
                    Category3Id = item.Category3Id,
                    Category4Id = item.Category4Id,
                    Category5Id = item.Category5Id,
                    IsVATExempt = item.IsVATExempt,
                    DeductionFileTypeId = item.DeductionFileTypeId,
                    DeductionFileNumber = item.DeductionFileNumber,
                    AssessingOfficeCode = item.AssessingOfficeCode,
                    Occupation = item.Occupation,
                    DeductionTypeId = item.DeductionTypeId,
                    ConsolidationVat = item.ConsolidationVat,
                    IsEquipmentVendor = item.IsEquipmentVendor,
                    ExcludeFromDeductionReport = item.ExcludeFromDeductionReport,

                    AllowEditChequePayToName = item.AllowEditChequePayToName,
                    ActiveForInterest = item.ActiveForInterest,
                    InterestCalculationStartDate = item.InterestCalculationStartDate,
                    ActiveForInterestCreditInvoice = item.ActiveForInterestCreditInvoice,
                    InterestCreditLimit = item.InterestCreditLimit,
                    NameForPrintingCheques = item.NameForPrintingCheques,
                    Smallcashbook = item.Smallcashbook,
                    MinimumInterestInvoiceBilling = item.MinimumInterestInvoiceBilling,
                    ReportingAsAnotherDocument = item.ReportingAsAnotherDocument,
                    CreditAllotmentPercentage = item.CreditAllotmentPercentage,
                    CardsDataId = item.CardsDataId,
                    PostponedChequesCommission = item.PostponedChequesCommission,
                    InterestOpenBalance = item.InterestOpenBalance




                };





                glAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                service.Update(glAccountPM, true);

            }

            GLAccountMoreDataRepository glAccountMoreDataRepository = new GLAccountMoreDataRepository(context);
            List<GLAccountMoreData> GLAccountMoreData0 = glAccountMoreDataRepository.GetAll(tenant).ToList();
            List<GLAccount> GLAccountTenant0 = repository.GetAll(tenant).Where(t => t.Inactive == false && t.IsControlAccount == false).ToList();
            List<GLAccount> GLAccount = repository.GetAll(tenatToCopy).Where(t => t.Inactive == false && t.IsControlAccount == false).ToList();
            GLAccountMoreData0 = GLAccountMoreData0.Where(data => GLAccountTenant0.Any(account => account.Id == data.AccountId)).ToList();

            foreach (var item in GLAccountMoreData0)
            {
                var interNumber = GLAccountTenant0.Find(c => c.Id == item.AccountId)?.InternalNumber;
                var AccountId = GLAccount.Find(c => c.InternalNumber == interNumber)?.Id;
                GLAccountMoreDataPM glAccountMoreDataPM = new GLAccountMoreDataPM()
                {
                    AccountId = AccountId,
                    Tenant = tenatToCopy,
                    BalanceInLocalCurrency = item.BalanceInLocalCurrency,
                    LocalBalanceInDue = item.LocalBalanceInDue,
                    NextDueDate = item.NextDueDate,
                    TotalOpenChequesInLocalCur = item.TotalOpenChequesInLocalCur,
                    TotFutureOpenChequesInLocalCur = item.TotFutureOpenChequesInLocalCur,
                    BalanceInForeignCurrency = item.BalanceInForeignCurrency,
                    ForeignBalanceInDue = item.ForeignBalanceInDue
                };
                glAccountMoreDataPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                glAccountMoreDataUpdateService.Update(glAccountMoreDataPM, true);
                context.SaveChanges();


            }



            context.SaveChanges();

        }
    }

    internal class GLAccountPMLite
    {
        public string Id { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string DisplayNumber { get; set; }
        public string InternalNumber { get; set; }

    }

    public class GLAccountCurrencyBalance
    {
        public decimal? ForeignAmount { get; set; }
        public decimal? LocalAmount { get; set; }
        public string CurrencyId { get; set; }
        public string AccountId { get; set; }
        //public string LocalName { get; set; }
    }

    public class CardGLAccountConnectionArgs
    {
        public string AccountId { get; set; }
        public string CardId { get; set; }
        public int Tenant { get; set; }
        public bool SkipConnectedCardsValidation { get; set; }
    }

    public interface IGLAccountQueryService
    {
        List<GLAccountPM> GetByGLAccountsIdList(List<String> GLAccountsIdList, int tenant);
    }






}



