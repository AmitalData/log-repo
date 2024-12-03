using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<AccountingSystem> GetAccountingSystems(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            accountingSystemRepository = new AccountingSystemRepository(tenant);
            return accountingSystemRepository.GetAccountingSystems();
        }

        public AccountingSystemPM GetSingleAccountingSystem(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("AccountingSystem", "READ", tenant);

            accountingSystemQuery = new AccountingSystemQuery(tenant);
            return accountingSystemQuery.GetSingleAccountingSystemPM(code);
        }

        public AccountingSystemList GetSingleAccountingSystemList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("AccountingSystem", "READ", tenant);

            AccountingSystemList entityList = null;
            accountingSystemRepository = new AccountingSystemRepository(tenant);
            AccountingSystem entity = accountingSystemRepository.GetSingleAccountingSystem(code);

            if (entity != null)
            {
                List<AccountingSystem> SingleEntityList = new List<AccountingSystem>();
                SingleEntityList.Add(entity);

                accountingSystemQuery = new AccountingSystemQuery(accountingSystemRepository);
                IQueryable<AccountingSystem> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<AccountingSystemList> iQueryableEntityList = accountingSystemQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<AccountingSystemList> GetAccountingSystemsLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("AccountingSystem", "READ", tenant);

            accountingSystemRepository = new AccountingSystemRepository(tenant);
            accountingSystemQuery = new AccountingSystemQuery(accountingSystemRepository);

            IQueryable<AccountingSystem> iQueryable = accountingSystemRepository.GetAccountingSystems();
            IQueryable<AccountingSystemList> query2 = accountingSystemQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingSystemList> GetAccountingSystemFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("AccountingSystem", "READ", tenant);

            accountingSystemRepository = new AccountingSystemRepository(tenant);
            accountingSystemQuery = new AccountingSystemQuery(accountingSystemRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystem> iQueryable = accountingSystemRepository.GetAccountingSystems();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingSystem>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AccountingSystemList> query2 = accountingSystemQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountingSystemList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountingSystemList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AccountingSystem", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingSystemList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingSystemList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingSystemList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingSystemList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<AccountingSystemList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetAccountingSystemFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("AccountingSystem", "READ", tenant);

            accountingSystemRepository = new AccountingSystemRepository(tenant);
            accountingSystemQuery = new AccountingSystemQuery(accountingSystemRepository);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingSystem> iQueryable = accountingSystemRepository.GetAccountingSystems();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingSystem>(nonListQueryOperation, iQueryable);

            IQueryable<AccountingSystemList> query2 = accountingSystemQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountingSystemList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void UpdateAccountingSystem(AccountingSystemPM entityPM, int tenant)
        {
            SecurityUtility.CheckContactFeature("AccountingSystem", "UPDATE", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            AccountingSystemService service = new AccountingSystemService(objectContext, tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(tenant, "AccountingSystem");
        }

        public void UpdateAccountingSystemList(AccountingSystemList currentEntity)
        {

        }
    }
}