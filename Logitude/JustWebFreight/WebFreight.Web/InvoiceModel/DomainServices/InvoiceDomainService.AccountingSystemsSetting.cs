using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public AccountingSystemsSettingPM GetSingleAccountingSystemsSettingPM(string id, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("AccountingSystemsSetting", "READ", tenant);

            accountingSystemsSettingQuery = new AccountingSystemsSettingQuery(tenant);
            return accountingSystemsSettingQuery.GetSinglePM(id, tenant);
        }

        public void UpdateAccountingSystemsSettingList(AccountingSystemsSettingList currentEntity)
        {

        }

        public AccountingSystemsSettingList GetSingleAccountingSystemsSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("AccountingSystemsSetting", "READ", tenant);

            AccountingSystemsSettingList entityList = null;
            accountingSystemsSettingRepository = new AccountingSystemsSettingRepository(tenant);
            accountingSystemsSettingQuery = new AccountingSystemsSettingQuery(accountingSystemsSettingRepository);
            AccountingSystemsSetting entity = accountingSystemsSettingRepository.GetSingleAccountingSystemsSetting(id, tenant);

            if (entity != null)
            {
                List<AccountingSystemsSetting> SingleEntityList = new List<AccountingSystemsSetting>();
                SingleEntityList.Add(entity);

                IQueryable<AccountingSystemsSetting> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<AccountingSystemsSettingList> iQueryableEntityList = accountingSystemsSettingQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public AccountingSystemsSettingPM GetAccountingSystemsSettingForTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSystemsSettingQuery = new AccountingSystemsSettingQuery(tenant);
            return accountingSystemsSettingQuery.GetAccountingSystemsSettingPMByTenant(tenant);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingSystemsSettingList> GetAccountingSystemsSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSystemsSettingRepository = new AccountingSystemsSettingRepository(tenant);
            accountingSystemsSettingQuery = new AccountingSystemsSettingQuery(accountingSystemsSettingRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystemsSetting> AccountingSystemsSettings = accountingSystemsSettingRepository.GetAccountingSystemsSettingsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AccountingSystemsSettings = filter.GetFilteredQuery<AccountingSystemsSetting>(nonListQueryOperation, AccountingSystemsSettings);

            int skippedAccountingSystemsSettings = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<AccountingSystemsSettingList> query2 = accountingSystemsSettingQuery.GetIQueryableEntityList(AccountingSystemsSettings);

            query2 = filter.GetFilteredQuery<AccountingSystemsSettingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountingSystemsSettingList).GetProperty(queryOperations.SortByColumnName);

                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSettingList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSettingList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSettingList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingSystemsSettingList, int>(queryOperations, query2);
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

            query2 = query2.Skip(skippedAccountingSystemsSettings);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountingSystemsSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystemsSetting> AccountingSystemsSettings = accountingSystemsSettingRepository.GetAccountingSystemsSettingsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            AccountingSystemsSettings = filter.GetFilteredQuery<AccountingSystemsSetting>(nonListQueryOperation, AccountingSystemsSettings);
            accountingSystemsSettingQuery = new AccountingSystemsSettingQuery(accountingSystemsSettingRepository);
            IQueryable<AccountingSystemsSettingList> query2 = accountingSystemsSettingQuery.GetIQueryableEntityList(AccountingSystemsSettings);

            query2 = filter.GetFilteredQuery<AccountingSystemsSettingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAccountingSystemsSetting(AccountingSystemsSettingPM AccountingSystemsSetting)
        {
            //  SecurityUtility.CheckContactFeature("AccountingSystemsSetting", "NEW", AccountingSystemsSetting.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(AccountingSystemsSetting.Tenant);
            }
            AccountingSystemsSettingService service = new AccountingSystemsSettingService(objectContext, AccountingSystemsSetting.Tenant);
            service.Create(AccountingSystemsSetting);


            TableLastUpdateClass.UpdateTableHistory(AccountingSystemsSetting.Tenant, "AccountingSystemsSetting");
        }

        public void UpdateAccountingSystemsSetting(AccountingSystemsSettingPM currentAccountingSystemsSetting)
        {
            //SecurityUtility.CheckContactFeature("AccountingSystemsSetting", "UPDATE", currentAccountingSystemsSetting.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(currentAccountingSystemsSetting.Tenant);
            }

            AccountingSystemsSettingService service = new AccountingSystemsSettingService(objectContext, currentAccountingSystemsSetting.Tenant);
            service.Update(currentAccountingSystemsSetting);
            TableLastUpdateClass.UpdateTableHistory(currentAccountingSystemsSetting.Tenant, "AccountingSystemsSetting");


        }
    }
}