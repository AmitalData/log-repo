using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public AccountPM GetSingleAccountPM(string id, int tenant)
        {
             SecurityUtility.AuthenticationOnTenant(tenant);

             accountQuery = new AccountQuery(tenant);
             return accountQuery.GetSinglePM(id, tenant);
        }

        public AccountList GetSingleAccountListByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountRepository = new AccountRepository(tenant);
            AccountList accountList = null;
            Account account = accountRepository.GetSingleAccountByCode(code, tenant);
            accountQuery = new AccountQuery(accountRepository);
            if (account != null)
            {
                List<Account> singleEntityList = new List<Account>();
                singleEntityList.Add(account);

                IQueryable<Account> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AccountList> iQueryableEntityList = accountQuery.GetIQueryableEntityList(iQueryable);
                accountList = iQueryableEntityList.FirstOrDefault();
            }
            
             return accountList;
        }

        public AccountList GetSingleAccountList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountRepository = new AccountRepository(tenant);
            accountQuery = new AccountQuery(accountRepository);
            AccountList accountList = null;
            Account account = accountRepository.GetSingleAccount(id, tenant);
             accountQuery = new AccountQuery(accountRepository);
            if (account != null)
            {
                List<Account> singleEntityList = new List<Account>();
                singleEntityList.Add(account);

                IQueryable<Account> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AccountList> iQueryableEntityList = accountQuery.GetIQueryableEntityList(iQueryable);
                accountList = iQueryableEntityList.FirstOrDefault();
            }
            return accountList;
        }

        public IQueryable<AccountList> GetAccountLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountRepository = new AccountRepository(tenant);
            accountQuery = new AccountQuery(accountRepository);
            IQueryable<Account> accounts = accountRepository.GetAccountsByTenant(tenant);

            IQueryable<AccountList> query2 = accountQuery.GetIQueryableEntityList(accounts);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountList> GetAccountFilters(byte[] xmlFilters, int tenant)
        {
             SecurityUtility.AuthenticationOnTenant(tenant);

            accountRepository = new AccountRepository(tenant);
            accountQuery = new AccountQuery(accountRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Account> accounts = accountRepository.GetAccountsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            accounts = filter.GetFilteredQuery<Account>(nonListQueryOperation, accounts);

            int skippedAccounts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

           IQueryable<AccountList> query2 = accountQuery.GetIQueryableEntityList(accounts);

            query2 = filter.GetFilteredQuery<AccountList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountList).GetProperty(queryOperations.SortByColumnName);

                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<AccountList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<AccountList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<AccountList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<AccountList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Code);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedAccounts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Account> accounts = accountRepository.GetAccountsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            accounts = filter.GetFilteredQuery<Account>(nonListQueryOperation, accounts);
            accountQuery = new AccountQuery(accountRepository);
           IQueryable<AccountList> query2 = accountQuery.GetIQueryableEntityList(accounts);

            query2 = filter.GetFilteredQuery<AccountList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<AccountPM> GetAccountsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
 
            accountQuery = new AccountQuery(tenant);
            return accountQuery.GetAccountByCodeOrName(code, name, tenant);
        }

        public bool DoesAccountCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountRepository = new AccountRepository(tenant);
            return (accountRepository.GetAccountsByTenant(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public void MapAccountAccountPM(AccountPM accountPM, Account account)
        {
            AccountType type = null;
            if (accountPM.AccountTypeCode != null)
            {
                type = accountTypeRepository.GetSingleAccountType(accountPM.AccountTypeCode);
            }

            account.Id = accountPM.Id;
            account.Tenant = accountPM.Tenant;
            account.Code = accountPM.Code;
            account.Name = accountPM.Name;
            account.AccountTypeCode = accountPM.AccountTypeCode;
            account.ExternalAccountingCard = accountPM.ExternalAccountingCard;
            account.InActive = accountPM.InActive;
            account.AddedManually = accountPM.AddedManually;
            account.SearchFields = accountPM.Code + "," + accountPM.Name + "," + account.ExternalAccountingCard + "," + (type != null ? type.Code : null);
        }

        public void InsertAccount(AccountPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entityPM.Tenant);
            }
            accountRepository = new AccountRepository(objectContext);
            bool exist = (from a in accountRepository.GetAccountsByTenant(entityPM.Tenant)
                          where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                          select a).Any();

            if (!exist)
            {
                if (entityPM.AccountTypeCode != "AR" && entityPM.AccountTypeCode != "AP")
                {
                    Account newEntity = new Account();
                    newEntity.Id = IdCounter.GetNumber("Account", entityPM.Tenant).ToString();
                    entityPM.Id = newEntity.Id;
                    MapAccountAccountPM(entityPM, newEntity);

                    accountRepository.Add(newEntity);

                    ContactRepository contactsRepository = new ContactRepository(entityPM.Tenant);
                    ContactQuery contactQuery = new ContactQuery(contactsRepository);
                    ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant, true);
                    if (contact != null)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = entityPM.Tenant,
                            EventTypeCode = "CRAC",
                            UserId = contact.Id,
                            EntityId = entityPM.Id,
                            ObjectTableName = "Account",
                        });
                    }

                    TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Account");
                }
                else
                {
                    throw new Exception("Sorry but you can't add an account of type account receivable or account payable!");
                }
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Account");
                throw new Exception(msg);
            }
        }

        public void UpdateAccount(AccountPM currentEntityPM)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(currentEntityPM.Tenant);
            }
            accountRepository = new AccountRepository(objectContext);
            //this.ChangeConnectionString(currentEntity.Tenant);
            string entityName = "Account" + currentEntityPM.Id + currentEntityPM.Tenant;
            string entityPMName = "AccountPM" + currentEntityPM.Id + currentEntityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPMName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPMName);
            }
            //  this.ChangeConnectionString(currentCountry.Tenant);
            bool exist = (from a in accountRepository.GetAccounts(currentEntityPM.Tenant)
                          where a.Code == currentEntityPM.Code && a.Id != currentEntityPM.Id && a.Tenant == currentEntityPM.Tenant
                          select a).Any();

            if (!exist)
            {
                Account updatedEntity = accountRepository.GetSingleAccount(currentEntityPM.Id, currentEntityPM.Tenant);
                MapAccountAccountPM(currentEntityPM, updatedEntity);
                accountRepository.Update(updatedEntity);

                ContactRepository contactsRepository = new ContactRepository(currentEntityPM.Tenant);
                ContactQuery contactQuery = new ContactQuery(contactsRepository);
                ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentEntityPM.Tenant, true);
                if (contact != null)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = currentEntityPM.Tenant,
                        EventTypeCode = "UPAC",
                        UserId = contact.Id,
                        EntityId = currentEntityPM.Id,
                        ObjectTableName = "Account",
                    });
                }

                TableLastUpdateClass.UpdateTableHistory(currentEntityPM.Tenant, "Account");
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentEntityPM.Tenant);
                msg = msg.Replace("%Entity", "Account");
                throw new Exception(msg);
            }
        }

        public void DeleteAccount(AccountPM entity)
        {
            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entity.Tenant);
            }
            accountRepository = new AccountRepository(objectContext);
            Account account = accountRepository.GetSingleAccount(entity.Id, entity.Tenant);
            accountRepository.Remove(account);
        }
    }
}
