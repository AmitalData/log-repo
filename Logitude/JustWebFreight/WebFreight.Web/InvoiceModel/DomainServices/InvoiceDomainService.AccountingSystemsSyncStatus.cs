using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public AccountingSystemsSyncStatusPM GetSingleAccountingSystemsSyncStatusPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("AccountingSystemsSyncStatus", "READ", tenant);

            accountingSystemsSyncStatusQuery = new AccountingSystemsSyncStatusQuery(tenant);
            return accountingSystemsSyncStatusQuery.GetSinglePM(id, tenant);
        }

        public void UpdateAccountingSystemsSyncStatusList(AccountingSystemsSyncStatusList currentEntity)
        {

        }

        public AccountingSystemsSyncStatusList GetSingleAccountingSystemsSyncStatusList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("AccountingSystemsSyncStatus", "READ", tenant);

            AccountingSystemsSyncStatusList entityList = null;
            accountingSystemsSyncStatusRepository = new AccountingSystemsSyncStatusRepository(tenant);
            accountingSystemsSyncStatusQuery = new AccountingSystemsSyncStatusQuery(accountingSystemsSyncStatusRepository);
            AccountingSystemsSyncStatus entity = accountingSystemsSyncStatusRepository.GetSingleAccountingSystemsSyncStatus(id, tenant);

            if (entity != null)
            {
                List<AccountingSystemsSyncStatus> SingleEntityList = new List<AccountingSystemsSyncStatus>();
                SingleEntityList.Add(entity);

                IQueryable<AccountingSystemsSyncStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<AccountingSystemsSyncStatusList> iQueryableEntityList = accountingSystemsSyncStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }


        public AccountingSystemsSyncStatusPM GetAccountingSystemsSyncStatusForTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSystemsSyncStatusQuery = new AccountingSystemsSyncStatusQuery(tenant);
            return accountingSystemsSyncStatusQuery.GetSingleAccountingSystemsSyncStatusPMForTenant(tenant);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingSystemsSyncStatusList> GetAccountingSystemsSyncStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSystemsSyncStatusRepository = new AccountingSystemsSyncStatusRepository(tenant);
            accountingSystemsSyncStatusQuery = new AccountingSystemsSyncStatusQuery(accountingSystemsSyncStatusRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuss = accountingSystemsSyncStatusRepository.GetAccountingSystemsSyncStatusesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AccountingSystemsSyncStatuss = filter.GetFilteredQuery<AccountingSystemsSyncStatus>(nonListQueryOperation, AccountingSystemsSyncStatuss);

            int skippedAccountingSystemsSyncStatuss = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<AccountingSystemsSyncStatusList> query2 = accountingSystemsSyncStatusQuery.GetIQueryableEntityList(AccountingSystemsSyncStatuss);

            query2 = filter.GetFilteredQuery<AccountingSystemsSyncStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountingSystemsSyncStatusList).GetProperty(queryOperations.SortByColumnName);

                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSyncStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSyncStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSyncStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSyncStatusList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Id);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedAccountingSystemsSyncStatuss);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountingSystemsSyncStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuss = accountingSystemsSyncStatusRepository.GetAccountingSystemsSyncStatusesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AccountingSystemsSyncStatuss = filter.GetFilteredQuery<AccountingSystemsSyncStatus>(nonListQueryOperation, AccountingSystemsSyncStatuss);
            accountingSystemsSyncStatusQuery = new AccountingSystemsSyncStatusQuery(accountingSystemsSyncStatusRepository);
            IQueryable<AccountingSystemsSyncStatusList> query2 = accountingSystemsSyncStatusQuery.GetIQueryableEntityList(AccountingSystemsSyncStatuss);

            query2 = filter.GetFilteredQuery<AccountingSystemsSyncStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


    }
}