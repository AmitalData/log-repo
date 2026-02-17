 
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

        public List<GLAccount> GetChildAccountsList(List<String> gLAccountIdList, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Tenant == tenant && gLAccountIdList.Any(b => a.ParentAccountId == b)
                    select a).ToList();
        }

        public GLAccount GetGLAccountByIdTenant(string GLAccountId, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Id == GLAccountId && a.Tenant == tenant
                    select a).FirstOrDefault();
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


        public IQueryable<GLAccount> GetQuaryAllControlAccount(int tenant)
       {
           return (from a in context.GLAccounts
                   where a.IsControlAccount == true && a.Tenant == tenant
                   select a);
        }
        public List<GLAccount> GetByGLAccountsIdList(List<String> GLAccountsIdList, int tenant)
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
             string Category1, string Category2, string Category3, string Category4, string Category5, string gLAccountType, string chartOfAccountsId)
        {
            return
            this
                .GetByAcountIdTypeCategories(tenant, AccountId, gLAccountType, chartOfAccountsId,
            Category1, Category2, Category3, Category4, Category5)
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
                        where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode
                        select a;
            return query.Where(r => !String.IsNullOrEmpty(r.VatNumber)
            && !String.IsNullOrEmpty(r.DeductionFileNumber)
            && !(r.Inactive.HasValue && r.Inactive.Value));
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
           , string jobControlAccountId
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
               .Where(a => a.AccountTypeCode == "1")// card    1	כרטיס	Card
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
                   .Where(a => a.AccountTypeCode == "2");//2	לקוח	Client
            
           }

           
           

           if (!string.IsNullOrWhiteSpace(vendorControlAccountId))
           {
               //exclude vendorControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != vendorControlAccountId);
               excludelist.Add(vendorControlAccountId);

               //include All AccountTypeCode  vendors
               qVendorTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "3");// 3	ספק	Vendor
            
           }

           

           if (!string.IsNullOrWhiteSpace(jobControlAccountId))
           {
               //exclude jobControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != jobControlAccountId);
               excludelist.Add(jobControlAccountId);

               //include All AccountTypeCode  Job
               qJobTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "4");//4	ג'וב	Job

           }


           if (!string.IsNullOrWhiteSpace(fileControlAccountId))
           {
               //exclude fileControlAccountId
               //qCardTenant=qCardTenant.Where(a => a.Id != fileControlAccountId);
               excludelist.Add(fileControlAccountId);

               //include All AccountTypeCode  file
               qFileTenant =
                   qAllInTenant
                   .Where(a => a.AccountTypeCode == "5");////5	תיק	File

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


 
    }

    public class GLAccountAndMoreDTO//: GLAccount
    {
        
        public string Id { get; set; }




        public string AccountTypeName { get; set; }
        public decimal? BalanceInLocalCurrency { get; set; }
        public decimal? LocalBalanceInDue { get;  set;  }
        public DateTime? NextDueDate { get;  set;  }
        public int Tenant { get;  set;  }
        public string InternalNumber { get;  set;  }
        public string AccountTypeCode { get;  set;  }
        public string DisplayNumber { get;  set;  }
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
}
   