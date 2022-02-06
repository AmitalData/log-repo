using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.CustomFilters;
using Simplog.Data.CommonDataModel.Repositories;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public GLAccountPM GetSingleGLAccountPM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            gLAccountQuery = new GLAccountQueryService(accountingContext);
            GLAccountPM gLAccountPM = gLAccountQuery.GetSingle(code, true, false);
            return gLAccountPM;
        }

        [Invoke]
        public GLAccountPM GetGLAccountPM(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService qs = new GLAccountQueryService(accountingContext);
            GLAccountPM entityPM = qs.GetSingle(id, true, false);
            return entityPM;
        }

        [Invoke]
        public GLAccountList GetGLAccount(string id, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountListQueryService qs = new GLAccountListQueryService(accountingContext);
            GLAccountList entityList = qs.GetByAccountId(id, tenant);
            return entityList;
        }

        [Invoke]
        public bool CheckIfDisplayNumberExists(string displayNo, string internalNumber, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            gLAccountQuery = new GLAccountQueryService(accountingContext);
            return this.gLAccountQuery.CheckIfDisplayNumberExists(displayNo, internalNumber, tenant);
        }

        //[Invoke]
        //public bool CheckIfClientAndCurrencyExist(string clientId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(clientId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        accountingContext = AccountingContext.GetContext(tenant);
        //        gLAccountQuery = new GLAccountQueryService(accountingContext);
        //        return this.gLAccountQuery.CheckIfClientAndCurrencyExist(clientId, currencyId, internalNumber, tenant);
        //    }
        //}

        //[Invoke]
        //public bool CheckIfVendorAndCurrencyExist(string vendorId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(vendorId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        accountingContext = AccountingContext.GetContext(tenant);
        //        gLAccountQuery = new GLAccountQueryService(accountingContext);
        //        return this.gLAccountQuery.CheckIfVendorAndCurrencyExist(vendorId, currencyId, internalNumber, tenant);
        //    }
        //}

        public GLAccountList GetSingleGLAccountList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountListQueryService listService = new GLAccountListQueryService(accountingContext);
            GLAccountList gLAccountList = listService.GetSingle(code);
            if (!String.IsNullOrEmpty(gLAccountList.CustomerGLAccountId))
            {
                GLAccountList account = listService.GetSingle(gLAccountList.CustomerGLAccountId);
                if (account != null)
                {
                    gLAccountList.CustomerGLAccountName = account.EnglishName;
                    gLAccountList.CustomerGLAccountNumber = account.DisplayNumber;
                }

            }
            if (!String.IsNullOrEmpty(gLAccountList.ParentAccountId))
            {
                GLAccountList account = listService.GetSingle(gLAccountList.ParentAccountId);
                if (account != null)
                {
                    gLAccountList.ParentAccountName = account.EnglishName;
                    gLAccountList.ParentAccountNumber = account.DisplayNumber;
                }
            }

            return gLAccountList;
        }

        public List<GLAccountList> GetGLAccountLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountListQueryService listService = new GLAccountListQueryService(accountingContext);
          //  return listService.GetList(tenant);
            List<GLAccountList> list = listService.GetList(tenant);
            foreach (var gLAccountList in list)
            {

                if (!String.IsNullOrEmpty(gLAccountList.CustomerGLAccountId))
                {
                    GLAccountList account = listService.GetSingle(gLAccountList.CustomerGLAccountId);
                    if (account != null)
                    {
                        gLAccountList.CustomerGLAccountName = account.EnglishName;
                        gLAccountList.CustomerGLAccountNumber = account.DisplayNumber;
                    }
                }
                if (!String.IsNullOrEmpty(gLAccountList.ParentAccountId))
                {
                    GLAccountList account = listService.GetSingle(gLAccountList.ParentAccountId);
                    if (account != null)
                    {
                        gLAccountList.ParentAccountName = account.EnglishName;
                        gLAccountList.ParentAccountNumber = account.DisplayNumber;
                    }
                }
            }
            return list;

        }

        public List<GLAccountList> GetGLAccountFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountListQueryService listService = new GLAccountListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
          //  return listService.GetList(queryOperations, tenant);
            List<GLAccountList> list = listService.GetList(queryOperations, tenant);
            List<GLAccountList> ActiveGLAccountList = list.Where(a => a.Inactive == false).ToList();
            foreach (var gLAccountList in ActiveGLAccountList)
            {

                if (!String.IsNullOrEmpty(gLAccountList.CustomerGLAccountId))
                {
                    GLAccountList account = listService.GetSingle(gLAccountList.CustomerGLAccountId);
                    if (account != null)
                    {
                        gLAccountList.CustomerGLAccountName = account.EnglishName;
                        gLAccountList.CustomerGLAccountNumber = account.DisplayNumber;
                    }
                }
                if (!String.IsNullOrEmpty(gLAccountList.ParentAccountId))
                {
                    GLAccountList account = listService.GetSingle(gLAccountList.ParentAccountId);
                    if (account != null)
                    {
                        gLAccountList.ParentAccountName = account.EnglishName;
                        gLAccountList.ParentAccountNumber = account.DisplayNumber;
                    }
                }
            }
            return ActiveGLAccountList;

        }

        public int GetGLAccountFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            GLAccountListQueryService queryService = new GLAccountListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }


        public void InsertChartOfAccount(GLAccountPM entityPm)
        {
            SecurityUtility.CheckContactFeature("GLAccount", "NEW", entityPm.Tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            }


            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            GLAccountUpdateService service = new GLAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);

            service.Update(entityPm, true);
        }

        public void UpdateChartOfAccount(GLAccountPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("GLAccount", "UPDATE", currententityPm.Tenant);
            var sssss = this.ChangeSet.ChangeSetEntries;
            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(currententityPm.Tenant);
            }
            //currententityPm.MarkAsChanged = true;

            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            GLAccountUpdateService service = new GLAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), currententityPm.Tenant);

            service.Update(currententityPm, true);

        }

        [Query(HasSideEffects = true)]
        public List<GLAccountList> GetGLAccountCompactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }

            GLAccountRepository gLAccountRepository = new GLAccountRepository(accountingContext);
        //    GLAccountContactRepository = new GLAccountContactRepository(objectContext);
        //    PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();

            IQueryable<GLAccount> gLAccounts = gLAccountRepository.GetAll(tenant); //GetGLAccounts(tenant);
            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);

            object seachvalue = item != null ? item.FieldValue : null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            GLAccountCustomFilter customfilters = new GLAccountCustomFilter(tenant);

            gLAccounts = customfilters.GetFilteredQuery(queryOperations, gLAccounts, accountingContext);
            gLAccounts = filter.GetFilteredQuery<GLAccount>(nonListQueryOperation, gLAccounts);

            string multi = TranslateTextsClass.Translate("GLAccounts.Q.Multi", tenant);
            string active = TranslateTextsClass.Translate("GLAccounts.Q.Active", tenant);
            string inactive = TranslateTextsClass.Translate("GLAccounts.Q.Inactive", tenant);
            var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(tenant);
            
            IQueryable<GLAccountList> myList = (from gLAccount in gLAccounts
                                                join md in myGLAccountMoreDataRepo.GetAll(tenant) on gLAccount.Id equals md.AccountId
                                                where gLAccount.Tenant == tenant
                                                select new GLAccountList()
                                                {
                                                    Id = gLAccount.Id,
                                                    Tenant = gLAccount.Tenant,
                                                    InternalNumber = gLAccount.InternalNumber,
                                                    AccountTypeCode = gLAccount.AccountTypeCode,
                                                    DisplayNumber = gLAccount.DisplayNumber,
                                                    EnglishName = gLAccount.EnglishName,
                                                    LocalName = gLAccount.LocalName,
                                                    SearchFields = gLAccount.SearchFields,
                                                    IsMultiCurrency = gLAccount.IsMultiCurrency,
                                                    CurrencyId = gLAccount.CurrencyId,
                                                    RevenueExpenseType = gLAccount.RevenueExpenseType,
                                                    IsControlAccount = gLAccount.IsControlAccount,
                                                    ChartOfAccountsId = gLAccount.ChartOfAccountsId,
                                                    Inactive = gLAccount.Inactive,
                                                    ReconcileMethodCode = gLAccount.ReconcileMethodCode,
                                                    ChartOfAccountsTypeCode = gLAccount.ChartOfAccountsTypeCode,
                                                    AccountTypeName = gLAccount.GLAccountType != null ? gLAccount.GLAccountType.EnglishName : null,
                                                    RevenueExpenseName = gLAccount.RevenueExpense != null ? gLAccount.RevenueExpense.EnglishName : null,
                                                    ReconcileMethodName = gLAccount.ReconcileMethod != null ? gLAccount.ReconcileMethod.EnglishName : null,
                                                    CurrencyName = gLAccount.Currency != null ? gLAccount.Currency.EnglishName : null,
                                                    ChartOfAccountsTypeName = gLAccount.ChartOfAccountsType != null ? gLAccount.ChartOfAccountsType.EnglishName : null,
                                                    CurrencyCode = gLAccount.IsMultiCurrency == true ? multi : gLAccount.Currency != null ? gLAccount.Currency.Code : null,
                                                    ControlAccountName = gLAccount.ControlAccount != null ? gLAccount.ControlAccount.EnglishName : null,
                                                    ControlAccountId = gLAccount.ControlAccountId,
                                                    ControlAccountNumber = gLAccount.ControlAccount != null ? gLAccount.ControlAccount.DisplayNumber : null,
                                                    ChartOfAccountsName = gLAccount.ChartOfAccount != null ? gLAccount.ChartOfAccount.EnglishName : null,
                                                    ActiveStatusName = gLAccount.Inactive == false ? active : inactive,
                                                    AutomaticReconcileId = gLAccount.AutomaticReconcileId,
                                                    AutomaticReconcileName = gLAccount.AutomaticReconcile != null ?
                                                        !String.IsNullOrEmpty(gLAccount.AutomaticReconcile.AutomaticReconcile2) ?
                                                            !String.IsNullOrEmpty(gLAccount.AutomaticReconcile.AutomaticReconcile3) ?
                                                                gLAccount.AutomaticReconcile.AutomaticReconcile1 + "+" + gLAccount.AutomaticReconcile.AutomaticReconcile2 + "+" + gLAccount.AutomaticReconcile.AutomaticReconcile3
                                                                : gLAccount.AutomaticReconcile.AutomaticReconcile1 + "+" + gLAccount.AutomaticReconcile.AutomaticReconcile2
                                                            : gLAccount.AutomaticReconcile.AutomaticReconcile1
                                                        : null,
                                                    PreviousEnglishName = gLAccount.PreviousEnglishName,
                                                    PreviousEnglishNameChangeDate = gLAccount.PreviousEnglishNameChangeDate,
                                                    PreviousLocalName = gLAccount.PreviousLocalName,
                                                    PreviousLocalNameChangeDate = gLAccount.PreviousLocalNameChangeDate,
                                                    PreviousNumber = gLAccount.PreviousNumber,
                                                    PreviousNumberChangeDate = gLAccount.PreviousNumberChangeDate,
                                                    PreviousChartOfAccountsId = gLAccount.PreviousChartOfAccountsId,
                                                    PreviousChartOfAccountsChangeDate = gLAccount.PreviousChartOfAccountsChangeDate,
                                                    //ClientName = gLAccount.Client != null ? gLAccount.Client.Card.EnglishName : null,
                                                    //VendorName = gLAccount.Vendor != null ? gLAccount.Vendor.Card.EnglishName : null,
                                                    //ClientId = gLAccount.ClientId,
                                                    //VendorId = gLAccount.VendorId,
                                                    CustomerGLAccountId = gLAccount.CustomerGLAccountId,
                                                    BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                                                    RevaluationEnabled = gLAccount.RevaluationEnabled,
                                                    //ClientCode = gLAccount.Client != null ? gLAccount.Client.Card.Code : null,
                                                    //VendorCode = gLAccount.Vendor != null ? gLAccount.Vendor.Card.Code : null,
                                                    ParentAccountId = gLAccount.ParentAccountId,
                                                    Category1Id = gLAccount.Category1Id,
                                                    Category1Name = gLAccount.Category1 != null ? gLAccount.Category1.EnglishName : null,
                                                    Category2Id = gLAccount.Category2Id,
                                                    Category2Name = gLAccount.Category2 != null ? gLAccount.Category2.EnglishName : null,
                                                    Category3Id = gLAccount.Category3Id,
                                                    Category3Name = gLAccount.Category3 != null ? gLAccount.Category3.EnglishName : null,
                                                    Category4Id = gLAccount.Category4Id,
                                                    Category4Name = gLAccount.Category4 != null ? gLAccount.Category4.EnglishName : null,
                                                    Category5Id = gLAccount.Category5Id,
                                                    Category5Name = gLAccount.Category5 != null ? gLAccount.Category5.EnglishName : null, 
                                                });


            //CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            //myList = myFilter.RunFilter(myList);

            var query2 = myList;
            query2 = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2);
            List<GLAccountList> resultList;

            if (seachvalue != null)
            {
                listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);
                IQueryable<GLAccountList> nameQueryResult = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "EnglishName";
                queryOperations.SortDirectin = "Ascending";

                nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "GLAccount", tenant);
                resultList = nameQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("DisplayNumber", seachvalue, false, "StartsWith", null, false);

                    IQueryable<GLAccountList> displayNumberQueryResult = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2);

                    foreach (GLAccountList gLAccount in displayNumberQueryResult)
                    {
                        if (!resultList.Where(p => p.DisplayNumber == gLAccount.DisplayNumber).Any())
                        {
                            resultList.Add(gLAccount);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("DisplayNumber", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                        IQueryable<GLAccountList> searchFieldQueryResult = filter.GetFilteredQuery<GLAccountList>(listQueryOperation, query2);


                        foreach (GLAccountList gLAccount in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.DisplayNumber == gLAccount.DisplayNumber).Any())
                            {
                                resultList.Add(gLAccount);

                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }

                query2 = resultList.AsQueryable();
            }

            query2 = query2.Take(queryOperations.PageSize);
            List<GLAccountList> result = new List<GLAccountList>();
            foreach (GLAccountList list in query2)
            {
                //string partnerType = list.PartnerTypeId == "PO" ? "CS" : list.PartnerTypeId;
                //PartnerType type = partnersTypeRepository.GetSinglePartnerType(partnerType);
                //ObjectTablePM table = ObjectTabelQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                //if (table != null)
                //{
                //    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                //    {
                        result.Add(list);
                //    }
                //}
            }

            return result;
        }

        public void UpdateGLAccountList(GLAccountList entity)
        {

        }

        public GLAccountSummary GetGLAccountSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            GLAccountSummary result = new GLAccountSummary();

            if (SecurityUtility.CheckTableContactFeature("GLAccount", "READ", tenant))
            {

                GLAccountRepository glAccountRepository = new GLAccountRepository(tenant);
                var iQueryable_Data =
                    glAccountRepository.GetAllAsGLAccountAndMore(tenant);

                // Main GLAccounts
                result.ActiveGLAccountCount = iQueryable_Data.Where(d => d.AccountTypeCode == "1" && d.Inactive == false).Count();
                result.InactiveGLAccountCount = iQueryable_Data.Where(d => d.AccountTypeCode == "1" && d.Inactive == true).Count();
                result.AllGLAccountCount = iQueryable_Data.Where(d => d.AccountTypeCode == "1").Count();
                result.OpenFilesCount = iQueryable_Data.Where(d => d.AccountTypeCode == "5" && d.BalanceInLocalCurrency != 0).Count();
                result.OpenMastersCount = iQueryable_Data.Where(d => d.AccountTypeCode == "4" && d.BalanceInLocalCurrency != 0).Count();
                result.ClosedFilesGLAccountCount = iQueryable_Data.Where(d => d.AccountTypeCode == "5" && d.BalanceInLocalCurrency == 0).Count();
                result.AllFilesCount = iQueryable_Data.Where(d => d.AccountTypeCode == "5").Count();
                result.AllJobsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "4").Count();

                // Customers GLAccounts
                result.ActiveCustomersCount = iQueryable_Data.Where(d => d.AccountTypeCode == "2" && d.Inactive == false).Count();
                result.InactiveCustomersCount = iQueryable_Data.Where(d => d.AccountTypeCode == "2" && d.Inactive == true).Count();
                var allCustomers = iQueryable_Data.Where(d => d.AccountTypeCode == "2");
                result.CollectorsCount = GetGlaccountsThatConnectedCardCollectorAsLoggedUser(tenant, allCustomers);

                result.DebitorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "2" && d.LocalBalanceInDue > 0).Count();
                result.AllCustomersCount = iQueryable_Data.Where(d => d.AccountTypeCode == "2").Count();

                // Vendors GLAccounts
                result.ActiveVendorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "3" && d.Inactive == false).Count();
                result.InactiveVendorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "3" && d.Inactive == true).Count();
                //result.CollectorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "3").Count();
                //result.DebitorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "3" && d.BalanceInLocalCurrency > 0).Count();
                result.AllVendorsCount = iQueryable_Data.Where(d => d.AccountTypeCode == "3").Count();

            }

            return result;
        }

        private int GetGlaccountsThatConnectedCardCollectorAsLoggedUser(int tenant, IQueryable<GLAccountAndMoreDTO> allCustomers)
        {
            var loggedUserId = GetLoggedUser(tenant).Id;
            var glaccountIds = allCustomers.Include("CardsData").Where(e => e.CardsData.CollectorUserId == loggedUserId).Count();
            return glaccountIds;
        }

        private User GetLoggedUser(int tenant)
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            var loggedUser = userRepository.GetSingleUserByEmail(email, tenant, false);
            return loggedUser;
        }
        public JournalSummary GetJournalSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            JournalSummary result = new JournalSummary();

            if (SecurityUtility.CheckTableContactFeature("Journal", "READ", tenant))
            {
                JournalRepository journalRepository = new JournalRepository(tenant);
                IQueryable<Journal> iQueryable_Data = journalRepository.GetAll(tenant);

                result.AllJournalsCount = iQueryable_Data.Count();
                result.ApprovedJournalsCount = iQueryable_Data.Where(d => d.StatusCode == "2" && d.AccountingEntityCode == "1").Count();
                result.WaitingJournalsCount = iQueryable_Data.Where(d => d.StatusCode == "1" && d.AccountingEntityCode == "1").Count();
                result.VoidedJournalsCount = iQueryable_Data.Where(d => d.StatusCode == "3" && d.AccountingEntityCode == "1").Count();
                result.DraftJournalsCount = iQueryable_Data.Where(d => d.StatusCode == "0" && d.AccountingEntityCode == "1").Count();

            }

            return result;
        }

    }
}