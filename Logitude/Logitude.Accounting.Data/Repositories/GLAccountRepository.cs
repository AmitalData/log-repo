 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Diagnostics;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.DataContract;
using System.Data.Entity.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class GLAccountRepository:IRepository<GLAccount>
   {
        
		public List<GLAccount> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<GLAccount> GetChildAccounts(string GLAccountId, int tenant)
        {
            return (from a in context.GLAccounts
             where a.ParentAccountId == GLAccountId && a.Tenant == tenant
             select a).ToList();
        }

        public List<KeyValuePair<string,string>> GetDisplayNumberList(HashSet<string> GLAccountIdSet, int tenant)
        {
            if (GLAccountIdSet?.Count<1)
            {
                return new List<KeyValuePair<string, string>>();
            }
            var l = (from a in context.GLAccounts
                     where GLAccountIdSet.Contains(a.Id) && a.Tenant == tenant
                     select new { a.Id, a.DisplayNumber })
                     .ToList();
            return l.Select(r => new KeyValuePair<string, string>(r.Id, r.DisplayNumber))
                .ToList();
                    
        }

        public List<GLAccount> GetChildAccountsList(List<String> gLAccountIdList, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant && gLAccountIdList.Any(b => a.ParentAccountId == b)
                    select a).ToList();
        }

        public List<GLAccount> GetChildAccountsQ(IQueryable<String> gLAccountIdQ, int tenant, int? userSecurityLevel)
        {
            if(userSecurityLevel != null)
            {
                return (from a in context.GLAccounts
                        join chartOfAccount in context.ChartOfAccounts on a.ChartOfAccountsId equals chartOfAccount.Id
                        where a.Tenant == tenant && gLAccountIdQ.Any(b => a.ParentAccountId == b) 
                        && (chartOfAccount.ChartOfAccountSecurityLevel ?? 0) >= (userSecurityLevel ?? 0)
                        select a).ToList();
            }
            else
            {
                return (from a in context.GLAccounts                    
                        where a.Tenant == tenant && gLAccountIdQ.Any(b => a.ParentAccountId == b) 
                        select a).ToList();
            }
          
        }

        public GLAccount GetGLAccountByIdTenant(string GLAccountId, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Id == GLAccountId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public string GetDisplayNumberByGLAccountId(string GLAccountId, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Id == GLAccountId && a.Tenant == tenant
                    select a.DisplayNumber).FirstOrDefault();
        }

        public GLAccount GetGLAccountByDisplayNoAndTenant(string displayNo, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.DisplayNumber == displayNo && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public GLAccount GetGLAccountByInternalNoAndTenant(string internalNo, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.InternalNumber == internalNo && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public GLAccount GetControlGLAccountByChart(string chartOfAccountsId, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.ChartOfAccountsId == chartOfAccountsId && a.Tenant == tenant
                            && a.IsControlAccount.HasValue && a.IsControlAccount.Value == true
                            && (!a.Inactive.HasValue || a.Inactive.Value == false)
                    select a).FirstOrDefault();
        }


        public IQueryable<GLAccount> GetQuaryAllControlAccount(int tenant)
       {
           return (from a in context.GLAccounts
                   where a.IsControlAccount == true && a.Tenant == tenant
                   select a);
        }

        public IQueryable<GLAccount> GetQueryAllSmallCashbookAccount(int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Smallcashbook == true && a.Tenant == tenant
                    select a);
        }


        public List<GLAccount> GetByGLAccountsIdList(List<string> GLAccountsIdList, int tenant)
        {
            return (from a in context.GLAccounts
                    where GLAccountsIdList.Contains(a.Id) && a.Tenant == tenant
                    select a).ToList();
        }

        public IQueryable<GLAccount> GetQAllControlAccount(int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant
                    where a.IsControlAccount == true
                    select a);
        }

        public IQueryable<GLAccount> GetbychartOfAccountsTypeCode(int tenant, string chartOfAccountsTypeCode)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant
                    where a.ChartOfAccountsTypeCode == chartOfAccountsTypeCode
                    select a);

        }

        public IQueryable<GLAccount> GetbyChartOfAccountsId(int tenant, string chartOfAccountsId)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant
                    where a.ChartOfAccountsId == chartOfAccountsId
                    select a);

        }

        public IQueryable<GLAccount> GetGlaAccountByJouranlIdAndJournalLineNumber(int tenant, string JournalId, int JournalLineNumber)
        {
            var query = from g in context.GLAccounts
                        join l in context.LedgerTransactions on g.Id equals l.AccountId
                        join j in context.JournalLines on new { JournalId = l.JournalId, LineNumber = l.JournalLineNumber } equals new { JournalId = j.JournalId, LineNumber = j.Line }
                        where l.Tenant == tenant && l.JournalId == JournalId && l.JournalLineNumber == JournalLineNumber
                        select g;

            return query.Distinct();
        }

        public List<GLAccount> GetChildAccountsByChartOfAccountIdList(List<String> chartOfAccountIdList, int tenant)
        {
            return GetQChildAccountsByChartOfAccountIdList(chartOfAccountIdList, tenant).ToList();
        }

        public IQueryable<GLAccount> GetQChildAccountsByChartOfAccountIdList(List<string> chartOfAccountIdList, int tenant)
        {
            return (from a in context.GLAccounts
                    where chartOfAccountIdList.Contains(a.ChartOfAccountsId) && a.Tenant == tenant
                    select a);
        }
        public IQueryable<GLAccountAndMoreDTO> GetAllAsGLAccountAndMore(int tenant)
        {
            return (
                from a in GetAll(tenant)
                join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                select new GLAccountAndMoreDTO()
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
                    CardsDataId = a.CardsDataId,
                    CardsData = a.GLAccountCardsData,
                    ControlAccountId = a.ControlAccountId,

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
                    BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                    RevaluationEnabled = a.RevaluationEnabled,

                    ParentAccountId = a.ParentAccountId,
                    IsVATExempt = a.IsVATExempt,
                    LocalBalanceInDue = md.LocalBalanceInDue,
                    ForeignBalanceInDue = md.ForeignBalanceInDue,
                    NextDueDate = md.NextDueDate,
                    AutomaticReconcile = a.AutomaticReconcile,

                    Category1Id = a.Category1Id,
                    Category2Id = a.Category2Id,
                    Category3Id = a.Category3Id,
                    Category4Id = a.Category4Id,

                    Category5Id = a.Category5Id,
                    DeductionFileNumber = a.DeductionFileNumber,
                }
                );
        }
        public IQueryable<GLAccount> GetQAccountsByChartOfAccountsTypeCodeList(List<string> ChartOfAccountsTypeCodeList, int tenant)
        {
            return (from a in context.GLAccounts
                    where ChartOfAccountsTypeCodeList.Contains(a.ChartOfAccountsTypeCode) && a.Tenant == tenant
                    select a);
        }
        public List<KeyValuePair<string, decimal>> 

            GetGLAccountsLocalBalanceGByChartOfAccountsTypeCode(int tenant)
        {
            var q = (
                from aa in
                    (from a in 
                         this.GetAllAsGLAccountAndMore(tenant)
                     //    context.GLAccounts
                     //join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                     //where a.Tenant == tenant
                     where a.AccountTypeCode == "1" //only Card
                     select a //GLAccountAndMoreDTO.GetGLAccountAndMore(a, md)
                     )
                group aa by aa.ChartOfAccountsTypeCode into g

                select new  //GLAccount() { EnglishName
                            //Tuple<string, decimal>()
                {
                    Item1 = g.Key,
                    Item2 = g.Sum(a => a.BalanceInLocalCurrency)
                }
                    );

            return q.ToList()
                .Select(r => new KeyValuePair<string, decimal>(r.Item1, r.Item2.GetValueOrDefault()))
                .ToList();
        }
        public IQueryable<string> GetQAccIdByAcountTypeCategories(int tenant, string AccountTypeCode,
             string Category1, string Category2, string Category3, string Category4, string Category5)
        {
            return 
            this
                .GetByAcountTypeCategories(tenant, AccountTypeCode,
            Category1, Category2, Category3, Category4, Category5)
            .Select(a => a.Id);

        }


        public IQueryable<string> GetQAccIdByAcountIdCategories(int tenant, string AccountId,
             string Category1, string Category2, string Category3, string Category4, string Category5)
        {
            return
            this
                .GetByAcountIdCategories(tenant, AccountId,
            Category1, Category2, Category3, Category4, Category5)
            .Select(a => a.Id);

        }


        public IQueryable<string> GetQAccIdByAcountIdTypeCategories(int tenant, string AccountId,
             string Category1, string Category2, string Category3, string Category4, string Category5, string gLAccountType, string chartOfAccountsId,
             string ChartOfAccountsTypeCode, string salesmanId, string collectorId, bool includeControlAccount, int? securityLevel)
        {
            return
            this
                .GetByAcountIdTypeCategories(tenant, AccountId, gLAccountType, chartOfAccountsId,
            Category1, Category2, Category3, Category4, Category5, ChartOfAccountsTypeCode, salesmanId, collectorId, includeControlAccount, securityLevel)
            .Select(a => a.Id);

        }

        public List<int> GetTenantByNextDueDate(DateTime today, List<string> accountTypeCodeList)
        {
            var q = (from a in context.GLAccounts
                     join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                     //where a.Tenant == tenant
                     where md.NextDueDate <= today
                     where accountTypeCodeList.Contains(a.AccountTypeCode)
                     select a.Tenant
                     );
            return q.Distinct().ToList();
        }
        public IQueryable<GLAccountAndMoreDTO> GetQByAccountTypeCodeList(int tenant, List<string> accountTypeCodeList)
        {
            var q = (from a in //context.GLAccounts
                         this.GetAllAsGLAccountAndMore(tenant)
                     where a.Tenant == tenant
                     where accountTypeCodeList.Contains(a.AccountTypeCode)
                     select a
                  );
            return q;
        }
        public IQueryable<string> GetListByNextDueDate(int tenant, DateTime today, List<string> accountTypeCodeList)
        {
            var q =(from a in GetQByAccountTypeCodeList(tenant, accountTypeCodeList)
                    //join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                    where a.NextDueDate <= today
                     //I think No Need  where a.NextDueDate != null 
                     
                     select a.Id
                     );
            return q;
                    

            
        }
        public IQueryable<GLAccount> GetByAcountTypeCategories(int tenant, string AccountTypeCode,
            string Category1, string Category2, string Category3, string Category4, string Category5)
       {
            var q= (from a in context.GLAccounts
                   where a.Tenant == tenant
                    where a.AccountTypeCode == AccountTypeCode
                   select a);
            if (!string.IsNullOrWhiteSpace( Category1))
            {
                q = q.Where(a => a.Category1Id == Category1);
            }
            if (!string.IsNullOrWhiteSpace(Category2))
            {
                q = q.Where(a => a.Category2Id == Category2);
            }
            if (!string.IsNullOrWhiteSpace(Category3))
            {
                q = q.Where(a => a.Category3Id == Category3);
            }
            if (!string.IsNullOrWhiteSpace(Category4))
            {
                q = q.Where(a => a.Category4Id == Category4);
            }
            if (!string.IsNullOrWhiteSpace(Category5))
            {
                q = q.Where(a => a.Category5Id == Category5);
            }
            return q;
       }


        public Card GetByGlAccountIdJoinCurrencies(int tenant , string glaccountId)
        {
            Card myCard = (from gc in (this.context as AccountingContext).GLAccounts

                        join c in (this.context as AccountingContext).GLAccountCurrencies on gc.Id equals c.GLAccountId into gj

                 from subc in gj.DefaultIfEmpty()

                 join gg in (this.context as AccountingContext).Cards on subc.MainGLAccountId equals gg.GLAccountId into cardJoin

                 from card in cardJoin.DefaultIfEmpty()

                 where gc.Id == glaccountId && gc.Tenant==tenant
                           select card
                          ).Concat(
                           from gc in (this.context as AccountingContext).GLAccounts

                           join c in (this.context as AccountingContext).GLAccountCurrencies on gc.Id equals c.GLAccountId into gj

                           from subc in gj.DefaultIfEmpty()

                           join gg in (this.context as AccountingContext).Cards on gc.Id equals gg.GLAccountId into cardJoin

                           from card in cardJoin.DefaultIfEmpty()

                          where gc.Id == glaccountId && gc.Tenant==tenant
                           select card
                    ).FirstOrDefault();

            return myCard;

        }
        public IQueryable<GLAccount> GetByAcountIdCategories(int tenant, string AccountId,
            string Category1, string Category2, string Category3, string Category4, string Category5)
        {
            IQueryable<GLAccount> q;
            if (!string.IsNullOrWhiteSpace(AccountId))
            {
                q = (from a in context.GLAccounts
                     where a.Tenant == tenant
                     where a.Id == AccountId
                     select a);
            }
            else
            {
                q = (from a in context.GLAccounts
                     where a.Tenant == tenant
                     select a);
            }
            if (!string.IsNullOrWhiteSpace(Category1))
            {
                q = q.Where(a => a.Category1Id == Category1);
            }
            if (!string.IsNullOrWhiteSpace(Category2))
            {
                q = q.Where(a => a.Category2Id == Category2);
            }
            if (!string.IsNullOrWhiteSpace(Category3))
            {
                q = q.Where(a => a.Category3Id == Category3);
            }
            if (!string.IsNullOrWhiteSpace(Category4))
            {
                q = q.Where(a => a.Category4Id == Category4);
            }
            if (!string.IsNullOrWhiteSpace(Category5))
            {
                q = q.Where(a => a.Category5Id == Category5);
            }
            return q;
        }


        public IQueryable<GLAccount> GetByAcountIdTypeCategories(int tenant, string AccountId, string gLAccountType, string chartOfAccountsId,
            string Category1, string Category2, string Category3, string Category4, string Category5,
            string ChartOfAccountsTypeCode, string salesmanId, string collectorId, bool includeControlAccount, int? securityLevel)
        {
            IQueryable<GLAccount> q;
            if (!string.IsNullOrWhiteSpace(AccountId))
            {
                q = (from a in context.GLAccounts
                     where a.Tenant == tenant
                     where a.Id == AccountId
                     select a);
            }
            else
            {
                q = (from a in context.GLAccounts
                     where a.Tenant == tenant
                     select a);
            }
            if (!includeControlAccount)
            {
                q = q.Where(a => a.IsControlAccount == false);
            }
            if (!string.IsNullOrWhiteSpace(gLAccountType))
            {
                q = q.Where(a => a.AccountTypeCode == gLAccountType);
            }
            if (!string.IsNullOrWhiteSpace(chartOfAccountsId))
            {
                q = q.Where(a => a.ChartOfAccountsId == chartOfAccountsId);
            }
            if (!string.IsNullOrWhiteSpace(Category1))
            {
                q = q.Where(a => a.Category1Id == Category1);
            }
            if (!string.IsNullOrWhiteSpace(Category2))
            {
                q = q.Where(a => a.Category2Id == Category2);
            }
            if (!string.IsNullOrWhiteSpace(Category3))
            {
                q = q.Where(a => a.Category3Id == Category3);
            }
            if (!string.IsNullOrWhiteSpace(Category4))
            {
                q = q.Where(a => a.Category4Id == Category4);
            }
            if (!string.IsNullOrWhiteSpace(Category5))
            {
                q = q.Where(a => a.Category5Id == Category5);
            }
            if (!string.IsNullOrWhiteSpace(ChartOfAccountsTypeCode))
            {
                q = q.Where(r => r.ChartOfAccountsTypeCode == ChartOfAccountsTypeCode);
            }
            if (!string.IsNullOrWhiteSpace(salesmanId))
            {
                q = (

                  from glacc in q

                  join card in (this.context as AccountingContext).Cards.Where(r => r.Tenant == tenant)
                  on glacc.Id equals card.GLAccountId

                  join cust in (this.context as AccountingContext).Customers
                     .Where(r => r.SalesmanUserId == salesmanId && r.Tenant == tenant)
                  on card.Id equals cust.Id

                  select glacc
                   );

            }
            if (!string.IsNullOrWhiteSpace(collectorId))
            {
                q = (from gc in q

                     join c in (this.context as AccountingContext).GLAccountCurrencies on gc.Id equals c.GLAccountId into gj

                     from subc in gj.DefaultIfEmpty()

                     join gg in (this.context as AccountingContext).Cards on subc.MainGLAccountId equals gg.GLAccountId into cardJoin

                     from card in cardJoin.DefaultIfEmpty()

                     where card.CollectorId == collectorId
                     select gc
                           ).Concat(
                            from gc in q

                            join c in (this.context as AccountingContext).GLAccountCurrencies on gc.Id equals c.GLAccountId into gj

                            from subc in gj.DefaultIfEmpty()

                            join gg in (this.context as AccountingContext).Cards on gc.Id equals gg.GLAccountId into cardJoin

                            from card in cardJoin.DefaultIfEmpty()

                            where card.CollectorId == collectorId
                            select gc
                     ).Distinct();





                //q = (
                //from gc in q
                //join c in (this.context as AccountingContext).GLAccountCurrencies
                //on gc.Id equals c.GLAccountId into gac
                //from subC in gac.DefaultIfEmpty()
                //join t3_1 in (this.context as AccountingContext).Cards
                //on subC.MainGLAccountId equals t3_1.GLAccountId
                //into gac2_1
                //from subC2_1 in gac2_1.DefaultIfEmpty()

                //    // Second join on subC.GLAccount.Id
                //join t3_2 in (this.context as AccountingContext).Cards
                //on subC.GLAccount.Id.ToString() equals t3_2.GLAccountId.ToString()
                //into gac2_2
                //from subC2_2 in gac2_2.DefaultIfEmpty()
                //where (subC2_1 != null || subC2_2 != null) && subC2_1.CollectorId == collectorId
                //select gc
                // );

            }

            if (securityLevel != null)
            {
                q = (
                    from glaccount in q

                    join chartOfAccount in (this.context as AccountingContext).ChartOfAccounts.Where(r => r.Tenant == tenant && (r.ChartOfAccountSecurityLevel <= securityLevel || r.ChartOfAccountSecurityLevel==null))
                    on glaccount.ChartOfAccountsId equals chartOfAccount.Id

                    select glaccount
                     );

            }
            return q;
        }




        public IQueryable<GLAccountAndMoreDTO> GetQAllByAccountTypeCode(int tenant, string AccountTypeCode)
        {
            var q=(from a in
                    //context.GLAccounts
                    //join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                    //where a.Tenant == tenant
                    this.GetAllAsGLAccountAndMore(tenant)
             
             select a);
            if (!string.IsNullOrWhiteSpace(AccountTypeCode))
            {
                q = q.Where(r => r.AccountTypeCode == AccountTypeCode);
            }
            return q;

            return (from a in
                    //context.GLAccounts
                    //join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                    //where a.Tenant == tenant
                    this.GetAllAsGLAccountAndMore(tenant)
                    where a.AccountTypeCode == AccountTypeCode
                    select a
                    );
        }
        public IQueryable<GLAccountAndMoreDTO> GetQAllRevenueExpenseCardsByIsControlAccount(int tenant,bool isControlAccount)
        {

            return
            GetQAllRevenueExpenseCards(tenant)
            .Where(r => r.IsControlAccount == isControlAccount);
        }
        public IQueryable<GLAccountAndMoreDTO> GetQAllRevenueExpenseCards(int tenant)
        {
            string AccountTypeCode = "1";//only Card
            return
                GetQAllByAccountTypeCode(tenant, AccountTypeCode)
                .Where( a=> a.RevenueExpenseType =="1" || a.RevenueExpenseType =="2");
        }

 
        public IQueryable<CardGLAccountDataView> GetQAllVendorGLAccountCardsHavingDeduction(int tenant)
        {
            string accountTypeCode = "3";//only Vendor
            ICardGLAccountDataViewContext cardGLAccountDataViewContext = CardGLAccountDataViewContext.GetContext(tenant);

            var query = from a in cardGLAccountDataViewContext.CardGLAccountDataViews
                        where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode && a.CountryCode == "IL"
                        select a;
            return query.Where(r => !String.IsNullOrEmpty(r.VatNumber)
            && !(r.Inactive.HasValue && r.Inactive.Value)
            && !r.ExcludeFromDeductionReport);
        }




        public IQueryable<GLAccountAndMoreDTO> GetQAllCards(int tenant)
       {
           string AccountTypeCode= "1" ;//only Card
           return 
               //(from a in context.GLAccounts
               //    where a.Tenant == tenant
               //    where a.AccountTypeCode == "1" //only Card
               //    select a);
               GetQAllByAccountTypeCode(tenant, AccountTypeCode);
       }
        public bool CheckIfDisplayNumberExists(string displayNo,string internalNumber,int tenant)
        {
            bool exists;
            if (String.IsNullOrEmpty(internalNumber))
            {
                exists = (from a in context.GLAccounts
                         where a.DisplayNumber == displayNo && a.Tenant == tenant
                         select a).Any();
            }
            else
            {
                exists = (from a in context.GLAccounts
                          where a.DisplayNumber == displayNo && a.Tenant == tenant && a.InternalNumber != internalNumber
                          select a).Any();
            
            }
            return exists;

        }

        public bool CheckIfInternalNumberExists(string internalNumber, string id, int tenant)
        {
            bool exists;
            if (String.IsNullOrWhiteSpace(id))
            {
                exists = (from a in context.GLAccounts
                          where a.InternalNumber == internalNumber && a.Tenant == tenant
                          select a).Any();
            }
            else
            {
                exists = (from a in context.GLAccounts
                          where a.InternalNumber == internalNumber && a.Tenant == tenant && a.Id != id
                          select a).Any();
            }
            return exists;

        }

        //public bool CheckIfClientAndCurrencyExist(string clientId, string currencyId, string internalNumber, int tenant)
        //{
        //    bool exist;
        //    if (String.IsNullOrEmpty(clientId))
        //    {
        //        exist = false;
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(internalNumber))
        //        {
        //            if (String.IsNullOrEmpty(currencyId))
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.ClientId == clientId && a.Tenant == tenant && (a.CurrencyId == null || a.CurrencyId == "")
        //                          select a).Any();
        //            }
        //            else
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.ClientId == clientId && a.Tenant == tenant && a.CurrencyId == currencyId
        //                          select a).Any();
        //            }
        //        }
        //        else
        //        {
        //            if (String.IsNullOrEmpty(currencyId))
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.ClientId == clientId && a.Tenant == tenant && a.InternalNumber != internalNumber && (a.CurrencyId == null || a.CurrencyId == "")
        //                          select a).Any();
        //            }
        //            else
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.ClientId == clientId && a.Tenant == tenant && a.InternalNumber != internalNumber && a.CurrencyId == currencyId
        //                          select a).Any();
        //            }
        //        }
        //    }
        //    return exist;
        //}


        //public bool CheckIfVendorAndCurrencyExist(string vendorId, string currencyId, string internalNumber, int tenant)
        //{
        //    bool exist;
        //    if (String.IsNullOrEmpty(vendorId))
        //    {
        //        exist = false;
        //    }
        //    else
        //    {
        //        if (String.IsNullOrEmpty(internalNumber))
        //        {
        //            if (String.IsNullOrEmpty(currencyId))
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.VendorId == vendorId && a.Tenant == tenant && (a.CurrencyId == null || a.CurrencyId == "")
        //                          select a).Any();
        //            }
        //            else
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.VendorId == vendorId && a.Tenant == tenant && a.CurrencyId == currencyId
        //                          select a).Any();
        //            }
        //        }
        //        else
        //        {
        //            if (String.IsNullOrEmpty(currencyId))
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.VendorId == vendorId && a.Tenant == tenant && a.InternalNumber != internalNumber && (a.CurrencyId == null || a.CurrencyId == "")
        //                          select a).Any();
        //            }
        //            else
        //            {
        //                exist = (from a in context.GLAccounts
        //                          where a.VendorId == vendorId && a.Tenant == tenant && a.InternalNumber != internalNumber && a.CurrencyId == currencyId
        //                          select a).Any();
        //            }
        //        }
        //    }
        //    return exist;
        //}

        public List<string> GetGLAccountId(int tenant, string AccountTypeCode, string RevenueExpenseType, int top)
        {

            var q =context.GLAccounts.Where(record => record.Tenant == tenant);
            if (!String.IsNullOrWhiteSpace(AccountTypeCode))
            {
                q.Where(record => record.AccountTypeCode == AccountTypeCode);
            }
            if (!String.IsNullOrWhiteSpace(RevenueExpenseType))
            {
                q.Where(record => record.RevenueExpenseType == RevenueExpenseType);
            }


            return q.Take(top).Select(record => record.Id).ToList();
        }


        //public List<string> GetGLAccountIdByTypeControl(int tenant, string accountTypeCode, bool? isControlAccount)
        //{
        //    var q = context.GLAccounts.Where(record => record.Tenant == tenant && 
        //        (!isControlAccount.HasValue || (record.IsControlAccount.HasValue && record.IsControlAccount.Value == isControlAccount.Value)));
        //    if (!String.IsNullOrWhiteSpace(accountTypeCode))
        //    {
        //        q.Where(record => record.AccountTypeCode == accountTypeCode);
        //    }

        //    return q.Select(record => record.Id).ToList();
        //}


        public List<string> GetNextGLAccountIdByTypeControl(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            var q = context.GLAccounts.OrderBy(rec => rec.Id).Where(record => record.Tenant == tenant &&
                (lastMadeGLAccountId == null || lastMadeGLAccountId == "" || String.Compare(record.Id, lastMadeGLAccountId) > 0) &&
                (!isControlAccount.HasValue || (record.IsControlAccount.HasValue && record.IsControlAccount.Value == isControlAccount.Value)) &&
                (accountTypeCode == null || accountTypeCode == "" || record.AccountTypeCode == accountTypeCode) &&
                record.ActiveForInterest).Take(maxGLAccountsPerQuery);

            return q.Select(record => record.Id).ToList();
        }


        public List<string> GetNextGLAccountIdByTypeControlNoParent(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            var q = context.GLAccounts.OrderBy(rec => rec.Id).Where(record => record.Tenant == tenant &&
                (lastMadeGLAccountId == null || lastMadeGLAccountId == "" || String.Compare(record.Id, lastMadeGLAccountId) > 0) &&
                (!isControlAccount.HasValue || (record.IsControlAccount.HasValue && record.IsControlAccount.Value == isControlAccount.Value)) &&
                (accountTypeCode == null || accountTypeCode == "" || record.AccountTypeCode == accountTypeCode) &&
                (record.ParentAccountId == null || record.ParentAccountId == "") &&
                record.ActiveForInterest).Take(maxGLAccountsPerQuery);

            return q.Select(record => record.Id).ToList();
        }

        public List<string> GetNextGLAccountIdByTypeControlDescendant(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        {
            var q = context.GLAccounts.OrderBy(rec => rec.Id).Where(record => record.Tenant == tenant &&
                (lastMadeGLAccountId == null || lastMadeGLAccountId == "" || String.Compare(record.Id, lastMadeGLAccountId) > 0) &&
                (!isControlAccount.HasValue || (record.IsControlAccount.HasValue && record.IsControlAccount.Value == isControlAccount.Value)) &&
                (accountTypeCode == null || accountTypeCode == "" || record.AccountTypeCode == accountTypeCode) &&
                (record.ParentAccountId != null && record.ParentAccountId != null) &&
                record.ActiveForInterest).Take(maxGLAccountsPerQuery);

            return q.Select(record => record.Id).ToList();
        }



        //public List<string> GetNextGLAccountIdByTypeControlInterest(int tenant, string accountTypeCode, bool? isControlAccount, string lastMadeGLAccountId, int maxGLAccountsPerQuery)
        //{
        //    var q = (from acc in context.GLAccounts.OrderBy(rec => rec.Id)
        //             join interestReport in context.InterestReports on acc.Id equals interestReport.GLAccountId
        //             where acc.Tenant == tenant &&
        //             (lastMadeGLAccountId == null || lastMadeGLAccountId == "" || String.Compare(acc.Id, lastMadeGLAccountId) > 0) &&
        //             (!isControlAccount.HasValue || (acc.IsControlAccount.HasValue && acc.IsControlAccount.Value == isControlAccount.Value)) &&
        //             acc.ParentAccountId == null &&
        //             (accountTypeCode == null || accountTypeCode == "" || acc.AccountTypeCode == accountTypeCode) &&
        //              acc.ActiveForInterest
        //             select acc).Take(maxGLAccountsPerQuery);

        //    return q.Select(acc => acc.Id).ToList();
        //}

        //public List<string> GetNextGLAccountIdByIdControlInterest(int tenant, string id, bool? isControlAccount)
        //{
        //    var q = (from acc in context.GLAccounts.Where(rec => rec.Id == id)
        //             join interestReport in context.InterestReports on acc.Id equals interestReport.GLAccountId
        //             where acc.Tenant == tenant &&
        //             (!isControlAccount.HasValue || (acc.IsControlAccount.HasValue && acc.IsControlAccount.Value == isControlAccount.Value)) &&
        //             acc.ParentAccountId == null &&
        //             acc.ActiveForInterest
        //             select acc);

        //    return q.Select(acc => acc.Id).ToList();
        //}



        public List<GLAccount> GetByRevaluationEnabled_OtherParams(bool? revaluationEnabled, string chartOfAccountsTypeCode, string chartOfAccountsId, string accountTypeCode, string gLAccountId, string accountingCurrencyId, int tenant)
        {
            if (revaluationEnabled.HasValue && revaluationEnabled.Value)
            {
                List<GLAccount> rv1;
                IQueryable<GLAccount> rec1 =
                from record in context.GLAccounts
                        where record.Tenant == tenant && record.Inactive != true && record.RevaluationEnabled.HasValue && record.RevaluationEnabled.Value 
                        && (!record.IsControlAccount.HasValue || record.IsControlAccount == false)
                 select record;
                if (rec1 != null)
                {
                    rv1 = rec1.ToList();
                    return (rv1);
                }
                else
                {
                    return null;
                }
            }
            else if (!String.IsNullOrEmpty(chartOfAccountsTypeCode) || !String.IsNullOrEmpty(chartOfAccountsId) || !String.IsNullOrEmpty(accountTypeCode) || !String.IsNullOrEmpty(gLAccountId))
            {
                List<GLAccount> rv2;
                IQueryable<GLAccount> rec2 = 
                 from record in context.GLAccounts
                        where record.Tenant == tenant && (record.Inactive == null || record.Inactive == false)
                                    && (record.ChartOfAccountsTypeCode == chartOfAccountsTypeCode || String.IsNullOrEmpty(chartOfAccountsTypeCode))
                                    && (record.ChartOfAccountsId == chartOfAccountsId || String.IsNullOrEmpty(chartOfAccountsId))
                                    && (record.AccountTypeCode == accountTypeCode || String.IsNullOrEmpty(accountTypeCode))
                                    && (record.Id == gLAccountId || String.IsNullOrEmpty(gLAccountId)
                                    && (record.CurrencyId != accountingCurrencyId || (record.IsMultiCurrency.HasValue && record.IsMultiCurrency.Value) || String.IsNullOrEmpty(accountingCurrencyId))
                                    && (!record.IsControlAccount.HasValue || record.IsControlAccount == false)
               )
                 select record;
                if (rec2 != null)
                {
                    rv2 = rec2.ToList();
                    return (rv2);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        public List<string> GetIdsByInternalNumber(String internalNumber, int tenant)
        {
            IQueryable<GLAccount> query = from a in context.GLAccounts
                                          where a.InternalNumber == internalNumber && a.Tenant == tenant
                                          select a;
            return query.Select(r => r.Id).ToList();

        }
        public List<GLAccount> GetByInternalNumber(String internalNumber, int tenant)
        {
            if (String.IsNullOrEmpty(internalNumber))
            {
                List<GLAccount> rv = new List<GLAccount>();
                return rv;
            }
            else
            {
               IQueryable<GLAccount> query = from a in context.GLAccounts
                        where a.InternalNumber == internalNumber && a.Tenant == tenant
                        select a;
               if (query.Any())
               {
                   return (query).ToList();
               }
               else
               {
                   List<GLAccount> rv = new List<GLAccount>();
                   return rv;
               }
            }
        }

       public IQueryable<GLAccount> GetQAllCardsAndDetailsAccType(int Tenant
           , string clientControlAccountId
           , string vendorControlAccountId
           , List<string> jobControlAccountId_list
           , string fileControlAccountId
           )
       {
           var qAllInTenant =this.GetAll(Tenant);
           IQueryable<GLAccount> qQTrail = null;
           IQueryable<GLAccount> qClientTenant = null;
           IQueryable<GLAccount> qVendorTenant = null;
           IQueryable<GLAccount> qJobTenant = null;
           IQueryable<GLAccount> qFileTenant = null;
           


           var qCardTenant =
           qAllInTenant 
               .Where(a => a.AccountTypeCode == "1")// card    1	�����	Card
               ;
           var excludelist = new List<string>();

           if (!string.IsNullOrWhiteSpace(clientControlAccountId))
           {
               //exclude clientControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != clientControlAccountId);
               excludelist.Add(clientControlAccountId);
               //include All AccountTypeCode  client
               qClientTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "2");//2	����	Client
            
           }

           
           

           if (!string.IsNullOrWhiteSpace(vendorControlAccountId))
           {
               //exclude vendorControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != vendorControlAccountId);
               excludelist.Add(vendorControlAccountId);

               //include All AccountTypeCode  vendors
               qVendorTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "3");// 3	���	Vendor
            
           }



            //if (!string.IsNullOrWhiteSpace(jobControlAccountId_list))
            if (jobControlAccountId_list!=null && jobControlAccountId_list.Count>0)
            {
               //exclude jobControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != jobControlAccountId);
               excludelist.AddRange(jobControlAccountId_list);

               //include All AccountTypeCode  Job
               qJobTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "4");//4	�'��	Job

           }


           if (!string.IsNullOrWhiteSpace(fileControlAccountId))
           {
               //exclude fileControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != fileControlAccountId);
               excludelist.Add(fileControlAccountId);

               //include All AccountTypeCode  file
               qFileTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "5");////5	���	File

           }

           if (excludelist.Count() > 0)
           {
               qCardTenant = qCardTenant.Where(a => excludelist.Contains(a.Id) == false);
           }
           qQTrail = qCardTenant;
           
           
           if (qClientTenant != null)
           {
               qQTrail = qQTrail.Union(qClientTenant);
           }
           if (qVendorTenant != null)
           {
               qQTrail = qQTrail.Union(qVendorTenant);
           }

           if (qJobTenant!= null)
           {
               qQTrail = qQTrail.Union(qJobTenant);
           }
           if (qFileTenant!= null)
           {
               qQTrail = qQTrail.Union(qFileTenant);
           }
           
           
           return qQTrail;
       }

       partial void onUpdate()//Partial Methods Definition in Generated
       {
           InsureUsingOnlyByUpdateService();
       }
       partial void onAdd()//Partial Methods
       {
           InsureUsingOnlyByUpdateService();
       }
       private void InsureUsingOnlyByUpdateService()
       {
            var myName = this.NameOf();
            if (myName != "GLAccountRepositoryPriv")
            {
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);


                throw new Exception("InsureUsingOnlyByUpdateService");

            }
            return;//mohammad temp fix until itzik is back
            int iFrame = 3;
           var mth = new StackTrace().GetFrame(iFrame).GetMethod();

           var cls = mth.ReflectedType.Name;
           if (cls == "GLAccountUpdateService") //never happen 
           {
               return;
           }
           if (cls == "JournalApproveService")
           {
               return;
           }

           if (cls == "EntityUpdateService`3" && mth.Name == "PerformUpdate")
           {
               return;
           }
           if ((new StackTrace().GetFrame(4).GetMethod()).ReflectedType.FullName == "Logitude.Accounting.BL.CoreBL.BuildTenant.FullAccountingProvider")
            {
                return;
            }
           AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
            var checkInsureUsingOnlyByUpdateService=  System.Configuration.ConfigurationManager.AppSettings.Get("InsureUsingOnlyByUpdateService");
            if (!string.IsNullOrWhiteSpace(checkInsureUsingOnlyByUpdateService))
            {
                throw new Exception("InsureUsingOnlyByUpdateService");
            }

       }

        public List<GLAccount> GetByDisplayNumber(String displayNumber, int tenant)
        {
            if (String.IsNullOrEmpty(displayNumber))
            {
                List<GLAccount> rv = new List<GLAccount>();
                return rv;
            }
            else
            {
                IQueryable<GLAccount> query = from a in context.GLAccounts
                                              where a.DisplayNumber == displayNumber && a.Tenant == tenant
                                              select a;
                if (query.Any())
                {
                    return (query).ToList();
                }
                else
                {
                    List<GLAccount> rv = new List<GLAccount>();
                    return rv;
                }
            }
        }

        public List<GLAccount> GetByDisplayNumberEnding(String displayNumberEnding, int tenant)
        {
            if (String.IsNullOrEmpty(displayNumberEnding))
            {
                List<GLAccount> rv = new List<GLAccount>();
                return rv;
            }
            else
            {
                IQueryable<GLAccount> query = from a in context.GLAccounts
                                              where a.DisplayNumber.Replace(" ", "").EndsWith(displayNumberEnding) && a.Tenant == tenant
                                              select a;
                if (query.Any())
                {
                    return (query).ToList();
                }
                else
                {
                    List<GLAccount> rv = new List<GLAccount>();
                    return rv;
                }
            }
        }

        public List<CardDTO> GetVendorCardsWithoutGLAccountMatchDisplayNumber(int tenant)
        {
            var cards = from crm in context.Cards
                                         where 
                                         crm.Tenant == tenant &&
                                        ( crm.PayablesAccountingCard !=null || !crm.PayablesAccountingCard.Equals("")) &&
                                        (crm.GLAccountId == null || crm.GLAccountId == "")
                                            && (crm.PartnerTypeId == "VD" || crm.PartnerTypeId == "DR" || crm.PartnerTypeId == "LL" || crm.PartnerTypeId == "WA" || crm.PartnerTypeId == "AG")
 

                        join a in context.GLAccounts
                                              .Where(r => (r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
                                              on crm.PayablesAccountingCard equals a.DisplayNumber

                        select new CardDTO()
                        {
                            Id = crm.Id,
                            PayablesAccountingCard = crm.PayablesAccountingCard,
                            GLAccountId = a.Id,
                            AccountNumber = a.DisplayNumber
                        };

            return cards.ToList();
        }
        public List<CardDTO> GetAllOtherCardsWithoutGLAccountMatchDisplayNumberPayable(int tenant)
        {

            var cards = from crm in context.Cards
                        where crm.Tenant == tenant &&
                        (crm.PayablesAccountingCard!= null || !crm.PayablesAccountingCard.Equals("")) &&
                        (crm.GLAccountId == null || crm.GLAccountId == "")

                                            && (crm.PartnerTypeId != "CS" && crm.PartnerTypeId != "PO" && crm.PartnerTypeId != "AG")
                                            && (crm.PartnerTypeId != "VD" && crm.PartnerTypeId != "DR" && crm.PartnerTypeId != "LL" && crm.PartnerTypeId != "WA")


                        join a in context.GLAccounts
       .Where(r => (r.AccountTypeCode == "1" || r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
       on crm.PayablesAccountingCard equals a.DisplayNumber

                        select new CardDTO()
                        {
                            Id = crm.Id,
                            PayablesAccountingCard = crm.PayablesAccountingCard,
                            GLAccountId = a.Id,
                            AccountNumber = a.DisplayNumber
                        };
            return cards.ToList();
        }
        public List<CardDTO> GetAllOtherCardsWithoutGLAccountMatchDisplayNumberReceivable(int tenant)
        {
                       
            var cards = from crm in context.Cards
                        where crm.Tenant == tenant &&
                        (crm.ReceivablesAccountingCard != null || !crm.ReceivablesAccountingCard.Equals("")) &&
                        (crm.GLAccountId == null || crm.GLAccountId == "")

                                            && (crm.PartnerTypeId != "CS" && crm.PartnerTypeId != "PO" && crm.PartnerTypeId != "AG")
                                            && (crm.PartnerTypeId != "VD" && crm.PartnerTypeId != "DR" && crm.PartnerTypeId != "LL" && crm.PartnerTypeId != "WA")

                                                            
                                                            join a in context.GLAccounts
                                           .Where(r => (r.AccountTypeCode == "1" || r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
                                           on crm.ReceivablesAccountingCard equals a.DisplayNumber

                                                            select new CardDTO()
                                                            {
                                                                Id = crm.Id,
                                                                ReceivablesAccountingCard = crm.ReceivablesAccountingCard,
                                                                GLAccountId = a.Id,
                                                                AccountNumber = a.DisplayNumber
                                                            };
            return cards.ToList();
        }
        public List<CardDTO> GetCustomerCardsWithoutGLAccountMatchDisplayNumber(int tenant)
        {
            var cards = from crm in context.Cards
                        where crm.Tenant == tenant &&
                        (crm.ReceivablesAccountingCard != null || !crm.ReceivablesAccountingCard.Equals("")) &&
                        (crm.GLAccountId == null || crm.GLAccountId == "")
                        
                           && (crm.PartnerTypeId == "CS" || crm.PartnerTypeId == "PO")
                        join a in context.GLAccounts
                        .Where(r => (r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
                        on crm.ReceivablesAccountingCard equals a.DisplayNumber

                        select new CardDTO()
                        {
                            Id = crm.Id,
                            ReceivablesAccountingCard = crm.ReceivablesAccountingCard,
                            GLAccountId = a.Id,
                            AccountNumber = a.DisplayNumber
                        };

            return cards.ToList();
        }
		public Card GetSingleCardWithoutGLAccountById(int tenant, string cardId)
        {
			return (from crm in context.Cards
						where crm.Tenant == tenant && crm.Id == cardId &&
					   string.IsNullOrEmpty(crm.GLAccountId) &&
					   (!string.IsNullOrEmpty(crm.ReceivablesAccountingCard) ||
					   !string.IsNullOrEmpty(crm.PayablesAccountingCard))
						select crm).FirstOrDefault();
		}

		public CardDTO GetSingleCardsReceivablesMatchDisplayNumber(int tenant, string cardId)
		{
			var cardDTO = from crm in context.Cards
					  where crm.Tenant == tenant && crm.Id == cardId

					  join a in context.GLAccounts
					  .Where(r => (r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
					  on crm.ReceivablesAccountingCard equals a.DisplayNumber

					  select new CardDTO()
					  {
						  Id = crm.Id,
						  ReceivablesAccountingCard = crm.ReceivablesAccountingCard,
						  GLAccountId = a.Id,
						  AccountNumber = a.DisplayNumber
					  };
            return cardDTO.FirstOrDefault();
		}
		public CardDTO GetSingleCardsPayablesMatchDisplayNumber(int tenant, string cardId)
		{
			var cardDTO = from crm in context.Cards
					  where
					  crm.Tenant == tenant && crm.Id == cardId

					  join a in context.GLAccounts
					  .Where(r => (r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
					  on crm.PayablesAccountingCard equals a.DisplayNumber

					  select new CardDTO()
					  {
						  Id = crm.Id,
						  PayablesAccountingCard = crm.PayablesAccountingCard,
						  GLAccountId = a.Id,
						  AccountNumber = a.DisplayNumber
					  };
			return cardDTO.FirstOrDefault();
		}
		public CardDTO GetSingleCardsPayablesAllMatchDisplayNumber(int tenant, string cardId)
		{
			var cardDTO = from crm in context.Cards
						  where crm.Tenant == tenant && crm.Id == cardId
						  join a in context.GLAccounts
						 .Where(r => (r.AccountTypeCode == "1" || r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
						 on crm.PayablesAccountingCard equals a.DisplayNumber

						  select new CardDTO()
						  {
							  Id = crm.Id,
							  PayablesAccountingCard = crm.PayablesAccountingCard,
							  GLAccountId = a.Id,
							  AccountNumber = a.DisplayNumber
						  };
			return cardDTO.FirstOrDefault();
		}
		public CardDTO GetSingleCardsReceivablesAllMatchDisplayNumber(int tenant, string cardId)
		{
			var cardDTO = from crm in context.Cards
						  where crm.Tenant == tenant && crm.Id == cardId
						  join a in context.GLAccounts
							 .Where(r => (r.AccountTypeCode == "1" || r.AccountTypeCode == "2" || r.AccountTypeCode == "3") && r.Tenant == tenant)
							 on crm.ReceivablesAccountingCard equals a.DisplayNumber

						  select new CardDTO()
						  {
							  Id = crm.Id,
							  ReceivablesAccountingCard = crm.ReceivablesAccountingCard,
							  GLAccountId = a.Id,
							  AccountNumber = a.DisplayNumber
						  };
			return cardDTO.FirstOrDefault();
		}
		
		public List<GLAccount> GetByDisplayNumberAndAccType(String displayNumber, String accTypeCode, int tenant)
        {
            if (String.IsNullOrEmpty(displayNumber) || String.IsNullOrEmpty(accTypeCode))
            {
                List<GLAccount> rv = new List<GLAccount>();
                return rv;
            }
            else
            {
                IQueryable<GLAccount> query = from a in context.GLAccounts
                                              where a.DisplayNumber == displayNumber && a.AccountTypeCode == accTypeCode && a.Tenant == tenant
                                              select a;
                if (query.Any())
                {
                    return (query).ToList();
                }
                else
                {
                    List<GLAccount> rv = new List<GLAccount>();
                    return rv;
                }
            }
        }


        public IQueryable<string> GetQId(List<string> AllIdAccounts,int tenant)
        {
            return
            this.GetAll(tenant).Where(r => AllIdAccounts.Contains(r.Id)).Select(r => r.Id).AsQueryable<string>();
        }
       // to use it in GetRecentGLAccount
       public IQueryable<GLAccount> GetAllByAccountType(string accountTypeCode, int tenant)
       {
           return from a in context.GLAccounts
                  where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode
                  select a;
       }

       public IQueryable<GLAccountAndMoreDTO> GetByCOATypeCodeCOATypeId(int tenant, string chartOfAccountsTypeCode, string chartOfAccountsId = null)
       {
            var q = (from a in
                     //    context.GLAccounts
                     //join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                     //where a.Tenant == tenant
                     this.GetAllAsGLAccountAndMore(tenant)
                     where a.ChartOfAccountsTypeCode == chartOfAccountsTypeCode
                     //select GLAccountAndMoreDTO.GetGLAccountAndMore(a,md))
                     select a
                     );
                  
           if (!string.IsNullOrWhiteSpace(chartOfAccountsId))
           {
               q = q.Where(a => a.ChartOfAccountsId == chartOfAccountsId);
           }

           return q;
       }

        

        public IQueryable<CardGLAccountDataView> GetCardGLAccountDataViews(string accountTypeCode, int tenant)
       {
           ICardGLAccountDataViewContext cardGLAccountDataViewContext = CardGLAccountDataViewContext.GetContext(tenant);
           return (from a in cardGLAccountDataViewContext.CardGLAccountDataViews
                   where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode
                   select a);
       }

        public IQueryable<GLAccount> GetSplittedByCurrencyGLAccounts(string accountId, int tenant)
        {
            return from a in context.GLAccounts
                   join c in context.GLAccountCurrencies on a.Id equals c.MainGLAccountId
                   where a.Id == accountId && a.Tenant == tenant
                   select new GLAccount()
                   {
                       Id =a.Id,
                       CurrencyId = c.CurrencyId,
                       DisplayNumber = c.GLAccount != null? c.GLAccount.DisplayNumber: null,

                   }; 
        }

        public DateTime? GetInterestCalculationStartDate(string glaccountId, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant && a.Id == glaccountId
                    select a.InterestCalculationStartDate).FirstOrDefault();
        }

        public IQueryable<InterestReportCustomerData> GetEligibleCustomersForInterestReports(int tenant)
        {

            IQueryable<InterestReportCustomerData> gLAccounts = (from GLAccount in context.GLAccounts
                                                                 where GLAccount.Tenant == tenant && GLAccount.ActiveForInterest == true
                                                                 && (GLAccount.Inactive != null && !GLAccount.Inactive.Value)
                                                                 join Card in context.Cards on GLAccount.Id equals Card.GLAccountId
                                                                 where Card.PartnerTypeId == "CS" && !Card.InActive
                                                                 select new { GLAccount = GLAccount, Card = Card }).GroupBy(x => x.GLAccount.Id)
                                                                           .Select(x => new InterestReportCustomerData()
                                                                           {
                                                                               GLAccountId = x.FirstOrDefault().GLAccount.Id,
                                                                               ActiveForInterest = x.FirstOrDefault().GLAccount.ActiveForInterest,
                                                                               EnglishName = x.FirstOrDefault().GLAccount.EnglishName,
                                                                               InterestCalculationStartDate = x.FirstOrDefault().GLAccount.InterestCalculationStartDate,
                                                                               InterestCreditLimit = x.FirstOrDefault().GLAccount.InterestCreditLimit,
                                                                               LocalName = x.FirstOrDefault().GLAccount.LocalName,
                                                                               MinimumInterestInvoiceBilling = x.FirstOrDefault().GLAccount.MinimumInterestInvoiceBilling,
                                                                               Tenant = x.FirstOrDefault().GLAccount.Tenant,
                                                                               CustomerId = x.FirstOrDefault().Card.Id,

                                                                           });
            return gLAccounts;


        }

        public List<GLAccount> GetAllActivityAccountsByTenant(int tenant)
        {
            List<GLAccount> accounts = ((from a in context.GLAccounts
                                 where a.ChartOfAccountsTypeCode == "3" && a.Inactive == false && (tenant == 0 || a.Tenant == tenant)
                                 select a).ToList());

            return accounts;
        }

        public List<GLAccount> GetAllInActivityAccountsByTenant(int tenant)
        {
            List<GLAccount> accounts = ((from a in context.GLAccounts
                                         where a.Inactive == true && a.Tenant == tenant
                                         select a).ToList());

            return accounts;
        }
    }

    public class GLAccountAndMoreDTO//: GLAccount
    {
        
        public string Id { get; set; }




        public string AccountTypeName { get; set; }
        public decimal? BalanceInLocalCurrency { get; set; }
        public decimal? LocalBalanceInDue { get;  set;  }
        public decimal? ForeignBalanceInDue  { get; set; } 
        
        public DateTime? NextDueDate { get;  set;  }
        public int Tenant { get;  set;  }
        public string InternalNumber { get;  set;  }
        public string AccountTypeCode { get;  set;  }
        public string DisplayNumber { get;  set;  }
        public string CardsDataId { get; set; }
        public GLAccountCardsData CardsData { get; set; }

        public string EnglishName { get;  set;  }
        public string LocalName { get;  set;  }
        public string SearchFields { get;  set;  }
        public bool? IsMultiCurrency { get;  set;  }
        public string CurrencyId { get;  set;  }
        public string RevenueExpenseType { get; set; }
        public bool? IsControlAccount { get;  set;  }
        public string ChartOfAccountsId { get;  set;  }
        public bool? Inactive { get;  set;  }
        public string ReconcileMethodCode { get;  set;  }
        public string ChartOfAccountsTypeCode { get;  set;  }
        public string ControlAccountId { get;  set;  }
        public string AutomaticReconcileId { get;  set;  }
        public string PreviousEnglishName { get;  set;  }
        public DateTime? PreviousEnglishNameChangeDate { get;  set;  }
        public string PreviousLocalName { get;  set;  }
        public DateTime? PreviousLocalNameChangeDate { get;  set;  }
        public string PreviousNumber { get;  set;  }
        public DateTime? PreviousNumberChangeDate { get;  set;  }
        public string PreviousChartOfAccountsId { get;  set;  }
        public DateTime? PreviousChartOfAccountsChangeDate { get;  set;  }
        public string CustomerGLAccountId { get;  set;  }
        public bool? RevaluationEnabled { get;  set;  }
        public string ParentAccountId { get;  set;  }
        public bool? IsVATExempt { get;  set;  }
        public AutomaticReconcileMethod AutomaticReconcile { get;  set;  }
        public string Category1Id { get;  set;  }
        public string Category2Id { get;  set;  }
        public string Category3Id { get;  set;  }
        public string Category4Id { get;  set;  }
        public string Category5Id { get;  set;  }
        public string DeductionFileNumber { get; set; }
#if false
        public static GLAccountAndMoreDTO GetGLAccountAndMore(GLAccount a, GLAccountMoreData md)
        {
            return new GLAccountAndMoreDTO()
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

                ControlAccountId = a.ControlAccountId,

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
                BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                RevaluationEnabled = a.RevaluationEnabled,

                ParentAccountId = a.ParentAccountId,
                IsVATExempt = a.IsVATExempt,
                LocalBalanceInDue = md.LocalBalanceInDue,
                NextDueDate = md.NextDueDate,
                AutomaticReconcile = a.AutomaticReconcile,

                Category1Id = a.Category1Id,
                Category2Id = a.Category2Id,
                Category3Id = a.Category3Id,
                Category4Id = a.Category4Id,

                Category5Id = a.Category5Id,
            };
        }
        
#endif
    }

    public class CardDTO
    {
        public string Id { get;  set; }
        public string ReceivablesAccountingCard { get;  set; }
        public string GLAccountId { get;  set; }
        public string AccountNumber { get;  set; }
        public string PayablesAccountingCard { get;  set; }
    }
}
   