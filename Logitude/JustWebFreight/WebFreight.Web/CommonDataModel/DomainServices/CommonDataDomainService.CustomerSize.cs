using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCustomerSizeList(CustomerSizeList currentEntity)
        {
        }

        public IQueryable<CustomerSize> GetCustomerSizes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeRepository = new CustomerSizeRepository(tenant);
            return customerSizeRepository.GetCustomerSizes(0);
        }

        public IQueryable<CustomerSizePM> GetCustomerSizesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeQuery = new CustomerSizeQuery(tenant);
            return customerSizeQuery.GetCustomerSizePMsByTenant(tenant);
        }

        public CustomerSizePM GetSingleCustomerSize(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeQuery = new CustomerSizeQuery(tenant);
            return customerSizeQuery.GetSinglePM(id, tenant);
        }

        public CustomerSizePM GetCustomerSizeById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeQuery = new CustomerSizeQuery(tenant);
            return customerSizeQuery.GetSinglePM(id, tenant);
        }

        public CustomerSizeList GetSingleCustomerSizeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeRepository = new CustomerSizeRepository(tenant);
            customerSizeQuery = new CustomerSizeQuery(customerSizeRepository);
            CustomerSizeList CustomerSizeList = null;
            CustomerSize CustomerSize = customerSizeRepository.GetSingleCustomerSize(id, tenant);

            if (CustomerSize != null)
            {
                List<CustomerSize> singleEntityList = new List<CustomerSize>();
                singleEntityList.Add(CustomerSize);

                IQueryable<CustomerSize> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerSizeList> iQueryableEntityList = customerSizeQuery.GetIQueryableEntityList(iQueryable);
                CustomerSizeList = iQueryableEntityList.FirstOrDefault();
            }
            return CustomerSizeList;
        }

        public IQueryable<CustomerSizeList> GetCustomerSizeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeRepository = new CustomerSizeRepository(tenant);
            customerSizeQuery = new CustomerSizeQuery(customerSizeRepository);
            IQueryable<CustomerSize> CustomerSizes = customerSizeRepository.GetCustomerSizes(tenant);
            IQueryable<CustomerSizeList> query2 = customerSizeQuery.GetIQueryableEntityList(CustomerSizes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerSizeList> GetCustomerSizeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeRepository = new CustomerSizeRepository(tenant);
            customerSizeQuery = new CustomerSizeQuery(customerSizeRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerSize> CustomerSizes = customerSizeRepository.GetCustomerSizes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerSizes = filter.GetFilteredQuery<CustomerSize>(nonListQueryOperation, CustomerSizes);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<CustomerSizeList> query2 = customerSizeQuery.GetIQueryableEntityList(CustomerSizes);

            query2 = filter.GetFilteredQuery<CustomerSizeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerSizeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerSize", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerSizeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerSizeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerSizeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerSizeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerSizeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomerSizeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerSize", "READ", tenant);

            customerSizeRepository = new CustomerSizeRepository(tenant);
            customerSizeQuery = new CustomerSizeQuery(customerSizeRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerSize> CustomerSizes = customerSizeRepository.GetCustomerSizes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CustomerSizes = filter.GetFilteredQuery<CustomerSize>(nonListQueryOperation, CustomerSizes);

            IQueryable<CustomerSizeList> query2 = customerSizeQuery.GetIQueryableEntityList(CustomerSizes);

            query2 = filter.GetFilteredQuery<CustomerSizeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertCustomerSize(CustomerSizePM CustomerSize)
        {
            SecurityUtility.CheckContactFeature("CustomerSize", "NEW", CustomerSize.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(CustomerSize.Tenant);
            }

            bool exist = DoesCustomerSizeRankExist(CustomerSize.Order, CustomerSize.Tenant);

            if (!exist)
            {
                CustomerSizeService service = new CustomerSizeService(objectContext, CustomerSize.Tenant);
                service.Create(CustomerSize);
                TableLastUpdateClass.UpdateTableHistory(CustomerSize.Tenant, "CustomerSize");
            }
            else
            {
                string msg = "Customer Size with same Order already exists !";
                throw new Exception(msg);
            }            
        }

        public void UpdateCustomerSize(CustomerSizePM currentCustomerSize)
        {
            SecurityUtility.CheckContactFeature("CustomerSize", "UPDATE", currentCustomerSize.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentCustomerSize.Tenant);
            }

            customerSizeRepository = new CustomerSizeRepository(objectContext);

            string entityName = "CustomerSize" + currentCustomerSize.Id + currentCustomerSize.Tenant;
            string entityPmName = "CustomerSizePM" + currentCustomerSize.Id + currentCustomerSize.Tenant;

            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            bool exist = (from a in customerSizeRepository.GetCustomerSizes(currentCustomerSize.Tenant)
                          where a.OrderNumber == currentCustomerSize.Order && a.Id != currentCustomerSize.Id && a.Tenant == currentCustomerSize.Tenant
                          select a).Any();
            if (!exist)
            {
                CustomerSizeService service = new CustomerSizeService(objectContext, currentCustomerSize.Tenant);
                service.Update(currentCustomerSize);
                TableLastUpdateClass.UpdateTableHistory(currentCustomerSize.Tenant, "CustomerSize");
            }
            else
            {
                string msg = "Customer Size with same Order already exists !";
                throw new Exception(msg);
            }            
        }

        public void DeleteCustomerSize(CustomerSizePM CustomerSize)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(CustomerSize.Tenant);
            }
            customerSizeRepository = new CustomerSizeRepository(objectContext);
            CustomerSize entity = customerSizeRepository.GetSingleCustomerSize(CustomerSize.Id, CustomerSize.Tenant);
            customerSizeRepository.Remove(entity);
        }

        public bool DoesCustomerSizeRankExist(int rank, int tenant)
        {
            customerSizeRepository = new CustomerSizeRepository(tenant);
            return (customerSizeRepository.GetCustomerSizes(tenant).Where(d => d.OrderNumber == rank && d.Tenant == tenant)).Any();
        }
    }
}