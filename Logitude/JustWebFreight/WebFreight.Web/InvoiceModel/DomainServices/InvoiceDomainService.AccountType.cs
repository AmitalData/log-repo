using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public void UpdateAccountTypeList(AccountTypeList currentEntity)
        {
        }

        public IQueryable<AccountType> GetAccountTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountTypeRepository = new AccountTypeRepository(tenant);
            return accountTypeRepository.GetAccountTypes();
        }

        public AccountTypePM GetSingleAccountType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountTypeQuery = new AccountTypeQuery(tenant);
            return accountTypeQuery.GetSingleAccountTypePM(code);
        }

        public AccountTypeList GetSingleAccountTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

           
            accountTypeQuery = new AccountTypeQuery(tenant);
            AccountTypeList entityList = null;
            AccountType entity = accountTypeRepository.GetSingleAccountType(code);

            if (entity != null)
            {
                List<AccountType> singleEntityList = new List<AccountType>();
                singleEntityList.Add(entity);

                IQueryable<AccountType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AccountTypeList> iQueryableEntityList = accountTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public IQueryable<AccountTypeList> GetAccountTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountTypeRepository = new AccountTypeRepository(tenant);
            accountTypeQuery = new AccountTypeQuery(accountTypeRepository);
            IQueryable<AccountType> iQueryable = accountTypeRepository.GetAccountTypes();
            IQueryable<AccountTypeList> query2 = accountTypeQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountTypeList> GetAccountTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountTypeRepository = new AccountTypeRepository(tenant);
            accountTypeQuery = new AccountTypeQuery(accountTypeRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountType> iQueryable = accountTypeRepository.GetAccountTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountType>(nonListQueryOperation, iQueryable);

            int skippedTypes = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<AccountTypeList> query2 = accountTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountTypeList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<AccountTypeList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<AccountTypeList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<AccountTypeList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<AccountTypeList, int>(queryOperations, query2);
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

            query2 = query2.Skip(skippedTypes);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountTypeRepository = new AccountTypeRepository(tenant);
            accountTypeQuery = new AccountTypeQuery(accountTypeRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountType> iQueryable = accountTypeRepository.GetAccountTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountType>(nonListQueryOperation, iQueryable);

            IQueryable<AccountTypeList> query2 = accountTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void MapAccountTypePMAccountType(AccountTypePM accountTypePM, Account accountType)
        {
            accountType.SearchFields = accountTypePM.Code + "," + accountTypePM.Name;
            accountType.Code = accountTypePM.Code;
            accountType.Name = accountTypePM.Name;
        }
    }
}