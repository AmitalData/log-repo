using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.Accounting.Data.CustomFilters;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class GLAccountListQueryService
    {
	    private IQueryable<GLAccountList> GetIqueryableList(IQueryable<GLAccount> iQueryable)
        {
            string multi = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
            string active = TranslateTextsClass.Translate("GLAccounts.Q.Active", 0);
            string inactive = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", 0);
            GLAccountRepository repository = new GLAccountRepository(context);
            IQueryable<GLAccountList> query = (from a in iQueryable
                                               join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                                               select new GLAccountList()
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
                                                        ReconcileMethodLocalName = a.ReconcileMethod != null ? a.ReconcileMethod.LocalName : null,
                                                        CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                        ChartOfAccountsTypeName = a.ChartOfAccountsType != null ? a.ChartOfAccountsType.EnglishName : null,
                                                        CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                                                        CurrencySign = a.IsMultiCurrency == true ? "" : a.Currency != null ? a.Currency.Sign : null,
                                                        ControlAccountName = a.ControlAccount != null ? a.ControlAccount.EnglishName : null,
                                                        ControlAccountId = a.ControlAccountId,
                                                        ControlAccountNumber = a.ControlAccount != null ? a.ControlAccount.DisplayNumber : null,
                                                        ChartOfAccountsName = a.ChartOfAccount != null ? a.ChartOfAccount.LocalName : null,
                                                        ActiveStatusName = a.Inactive == false ? active : inactive,
                                                        AutomaticReconcileId = a.AutomaticReconcileId,
                                                        AutomaticReconcileName = a.AutomaticReconcile != null ?
                                                            !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile2) ?
                                                                !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile3) ?
                                                                    a.AutomaticReconcile.AutomaticReconcileField1.EnglishName 
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField2.EnglishName
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField3.EnglishName
                                                                    : a.AutomaticReconcile.AutomaticReconcileField1.EnglishName
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField2.EnglishName
                                                                : a.AutomaticReconcile.AutomaticReconcileField1.EnglishName 
                                                            : null,
                                                   AutomaticReconcileLocalName = a.AutomaticReconcile != null ?
                                                            !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile2) ?
                                                                !String.IsNullOrEmpty(a.AutomaticReconcile.AutomaticReconcile3) ?
                                                                    a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField2.LocalName
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField3.LocalName
                                                                    : a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                                                    + "+" + a.AutomaticReconcile.AutomaticReconcileField2.LocalName
                                                                : a.AutomaticReconcile.AutomaticReconcileField1.LocalName
                                                            : null,
                                                   PreviousEnglishName = a.PreviousEnglishName,
                                                        PreviousEnglishNameChangeDate = a.PreviousEnglishNameChangeDate,
                                                        PreviousLocalName = a.PreviousLocalName,
                                                        PreviousLocalNameChangeDate = a.PreviousLocalNameChangeDate,
                                                        PreviousNumber = a.PreviousNumber,
                                                        PreviousNumberChangeDate = a.PreviousNumberChangeDate,
                                                        PreviousChartOfAccountsId = a.PreviousChartOfAccountsId,
                                                        PreviousChartOfAccountsChangeDate = a.PreviousChartOfAccountsChangeDate,
                                                        //ClientName = a.Client != null ? a.Client.Card.EnglishName : null,
                                                        //VendorName = a.Vendor != null ? a.Vendor.Card.EnglishName : null,
                                                        //ClientId = a.ClientId,
                                                        //VendorId = a.VendorId,
                                                        CustomerGLAccountId = a.CustomerGLAccountId,
                                                        BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                                                        RevaluationEnabled = a.RevaluationEnabled,
                                                        //ClientCode = a.Client != null ? a.Client.Card.Code : null,
                                                        //VendorCode = a.Vendor != null ? a.Vendor.Card.Code : null,
                                                        ParentAccountId = a.ParentAccountId,
                                                        IsVATExempt = a.IsVATExempt,
                                                        LocalBalanceInDue = md.LocalBalanceInDue,
                                                        NextDueDate = md.NextDueDate,
                                                        TotalOpenChequesInLocalCur = md.TotalOpenChequesInLocalCur,
                                                        TotFutureOpenChequesInLocalCur = md.TotFutureOpenChequesInLocalCur,
                                                        DeductionFileNumber = a.DeductionFileNumber,

                                                        //categories
                                                        Category1Name = a.Category1.EnglishName,
                                                        Category2Name = a.Category2.EnglishName,
                                                        Category3Name = a.Category3.EnglishName,
                                                        Category4Name = a.Category4.EnglishName,
                                                        Category5Name = a.Category5.EnglishName,
                                               });
            return query;
        }

		private IQueryable<GLAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccount> iQueryable,int tenant)
        {
            GLAccountCustomFilter filters = new GLAccountCustomFilter(tenant);

            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            return iQueryable;
		}

		private IQueryable<GLAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccount> iQueryable,int tenant)
        {
			return iQueryable;
		}


        public GLAccountList GetByAccountId(string accountId, int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                              where a.Tenant == tenant && a.Id == accountId
                                                              select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List <GLAccountList> accountList = accountListQuery.ToList();
            GLAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public List<GLAccountList> GetRevenueExpenseGLAccountList(int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && (a.AccountTypeCode == "1" || a.AccountTypeCode=="2" )
                                                  select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<GLAccountList> accountList = accountListQuery.ToList();

            return accountList;
        }

        public List<CardGLAccountListDataView> GetLastActivityGLAccounts(int tenant, string userId, string objectTableId, string accountTypeCode)
        {
            List<CardGLAccountListDataView> entityList = new List<CardGLAccountListDataView>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            GLAccountRepository repository = new GLAccountRepository(tenant);
            IQueryable<CardGLAccountDataView> entities = entities = repository.GetCardGLAccountDataViews(accountTypeCode, tenant);

            string multi = TranslateTextsClass.Translate("GLAccounts.Q.Multi", 0);
            string active = TranslateTextsClass.Translate("GLAccounts.Q.Active", 0);
            string inactive = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", 0);
            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                CardGLAccountDataView a = 
                            (from d in entities
                           where d.Id == lastActivity.EntityId
                           select d).FirstOrDefault();



                if (a != null)
                {
                    //get glamore data
                    GLAccountMoreData glAccountMoreData = GetAccountMoreData(a.Id, a.Tenant);

                    CardGLAccountListDataView list = new CardGLAccountListDataView()
                    {
                        //glaccount
                        Id = a.Id,
                        Tenant = a.Tenant,
                        InternalNumber = a.InternalNumber,
                        AccountTypeCode = a.AccountTypeCode,
                        DisplayNumber = a.DisplayNumber,
                        GLAccountEnglishName = a.GLAccountEnglishName,
                        GLAccountLocalName = a.GLAccountLocalName,
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
                        ActiveStatusName = a.Inactive == false ? active : inactive,
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
                  //      BalanceInLocalCurrency = a.BalanceInLocalCurrency,
                        RevaluationEnabled = a.RevaluationEnabled,
                        ParentAccountId = a.ParentAccountId,
                        IsVATExempt = a.IsVATExempt,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.LocalName,
                        ChartOfAccountsName = a.ChartOfAccountsEnglishName != null ? a.ChartOfAccountsEnglishName : null, //ChartOfAccountsLocalName

                        DeductionFileNumber = a.DeductionFileNumber,

                        // Card
                        SalesmanUserId = a.SalesmanUserId    ,
                        CollectorId = a.CollectorId          ,
                        CardEnglishName = a.CardEnglishName  ,
                        CardLocalName = a.CardLocalName      ,
                        CardGLAccountId = a.CardGLAccountId  ,
                        VatTypeId = a.VatTypeId              ,
                        CountryId = a.CountryId              ,
                        CountryCode = a.CountryCode          ,
                        CityName = a.CityName                ,
                        CountryName = a.CountryName          ,
                        PaymentTermId = a.PaymentTermId      ,
                        VatNumber = a.VatNumber              ,

                        // Contacts (SalesMans)
                        SalesManEnglishName = a.SalesManEnglishName,
                        SalesManLocalName = a.SalesManLocalName,

                        // Contacts (Collectors)
                        CollectorEnglishName = a.CollectorEnglishName,
                        CollectorLocalName = a.CollectorLocalName,

                        // GLACCOUNT MORE DATA
                        BalanceInLocalCurrency = glAccountMoreData.BalanceInLocalCurrency,
                        LocalBalanceInDue = glAccountMoreData.LocalBalanceInDue,
                        NextDueDate = glAccountMoreData.NextDueDate,


                    };


                    entityList.Add(list);
                }
            }




            return entityList;
        }
        public GLAccountMoreData GetAccountMoreData(string accountId, int tenant)
        {
            GLAccountMoreData _md = (from a in context.GLAccountMoreDatas
                                                   where a.Tenant == tenant && a.AccountId == accountId
                                                   select a).FirstOrDefault();

            return _md;
        }

        public List<GLAccountList> GetByAccountType(string accountTypeCode, string searchFields, int tenant)
        {
            IQueryable<GLAccount> accountsQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && a.AccountTypeCode == accountTypeCode && searchFields.Contains(searchFields)
                                                  select a);

            IQueryable<GLAccountList> accountsListQuery = this.GetIqueryableList(accountsQuery);
            List<GLAccountList> accountsList = accountsListQuery.ToList();
            return accountsList;
        }

        public List<GLAccountList> GetTopDeptors(string filterString, string accountTypeCode, int tenant)
        {
            IQueryable<GLAccount> accountsQuery;
            if (filterString == "Balance Due")
            {
                accountsQuery = (from a in context.GLAccounts
                                 join md in context.GLAccountMoreDatas on a.Id equals md.AccountId
                                 where a.AccountTypeCode == accountTypeCode && md.LocalBalanceInDue > 0 && a.Tenant == tenant
                                 orderby md.LocalBalanceInDue descending
                                 select a).Take(10);
            }
            else
            {
                accountsQuery = (from a in context.GLAccounts
                                 join md in context.GLAccountMoreDatas
                                 on a.Id equals md.AccountId

                                 where a.AccountTypeCode == accountTypeCode && md.BalanceInLocalCurrency > 0 && a.Tenant == tenant
                                 orderby md.BalanceInLocalCurrency descending
                                select a).Take(10);
            }
            

            IQueryable<GLAccountList> accountsListQuery = this.GetIqueryableList(accountsQuery);
            List<GLAccountList> accountsList = accountsListQuery.ToList();
            return accountsList;
        }

        public List<GLAccountList> GetChildrenGLAccounts(string GLAccountId, int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                  where a.Tenant == tenant && a.ParentAccountId == GLAccountId 
                                                  select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<GLAccountList> accountList = accountListQuery.ToList();

            return accountList;
        }
        
        public IQueryable<GLAccountList> GetByIds(List<string> ids, int tenant)
        {
            IQueryable<GLAccount> accountQuery = (from a in context.GLAccounts
                                                              where a.Tenant == tenant && ids.Contains(a.Id)
                                                              select a);

            IQueryable<GLAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            return accountListQuery;
        }

    }


}
	