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
        public void UpdateCustomerProductList(CustomerProductList currentEntity)
        {
        }

        public IQueryable<CustomerProduct> GetCustomerProductes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductRepository = new CustomerProductRepository(tenant);
            return customerProductRepository.GetCustomerProducts(0);
        }

        public CustomerProductPM GetSingleCustomerProductPM(string customerId, string typeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductQuery = new CustomerProductQuery(tenant);
            return customerProductQuery.GetSinglePM(customerId, typeCode, tenant);
        }

        public CustomerProductList GetSingleCustomerProductList(string customerId, string typeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductRepository = new CustomerProductRepository(tenant);
            customerProductQuery = new CustomerProductQuery(customerProductRepository);
            CustomerProductList CustomerProductList = null;
            CustomerProduct CustomerProduct = customerProductRepository.GetSingleCustomerProduct(customerId, typeCode, tenant);

            if (CustomerProduct != null)
            {
                List<CustomerProduct> singleEntityList = new List<CustomerProduct>();
                singleEntityList.Add(CustomerProduct);

                IQueryable<CustomerProduct> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerProductList> iQueryableEntityList = customerProductQuery.GetIQueryableEntityList(iQueryable);
                CustomerProductList = iQueryableEntityList.FirstOrDefault();
            }
            return CustomerProductList;
        }

        public IQueryable<CustomerProductList> GetCustomerProductLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductRepository = new CustomerProductRepository(tenant);
            customerProductQuery = new CustomerProductQuery(customerProductRepository);
            IQueryable<CustomerProduct> CustomerProductes = customerProductRepository.GetCustomerProducts(tenant);
            IQueryable<CustomerProductList> query2 = customerProductQuery.GetIQueryableEntityList(CustomerProductes);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerProductList> GetCustomerProductFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductRepository = new CustomerProductRepository(tenant);
            customerProductQuery = new CustomerProductQuery(customerProductRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerProduct> CustomerProductes = customerProductRepository.GetCustomerProducts(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerProductes = filter.GetFilteredQuery<CustomerProduct>(nonListQueryOperation, CustomerProductes);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<CustomerProductList> query2 = customerProductQuery.GetIQueryableEntityList(CustomerProductes);

            query2 = filter.GetFilteredQuery<CustomerProductList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerProductList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerProduct", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerProductList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerProductList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerProductList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerProductList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerProductList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CustomerId);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CustomerId);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomerProductFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerProduct", "READ", tenant);

            customerProductRepository = new CustomerProductRepository(tenant);
            customerProductQuery = new CustomerProductQuery(customerProductRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerProduct> CustomerProductes = customerProductRepository.GetCustomerProducts(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CustomerProductes = filter.GetFilteredQuery<CustomerProduct>(nonListQueryOperation, CustomerProductes);

            IQueryable<CustomerProductList> query2 = customerProductQuery.GetIQueryableEntityList(CustomerProductes);

            query2 = filter.GetFilteredQuery<CustomerProductList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void InsertCustomerProduct(CustomerProductPM CustomerProduct)
        //{
        //    SecurityUtility.CheckContactFeature("CustomerProduct", "NEW", CustomerProduct.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(CustomerProduct.Tenant);
        //    }
        //    CustomerProductService service = new CustomerProductService(objectContext, CustomerProduct.Tenant);
        //    service.Create(CustomerProduct);

        //    TableLastUpdateClass.UpdateTableHistory(CustomerProduct.Tenant, "CustomerProduct");
        //}

        //public void UpdateCustomerProduct(CustomerProductPM currentCustomerProduct)
        //{
        //    SecurityUtility.CheckContactFeature("CustomerProduct", "UPDATE", currentCustomerProduct.Tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(currentCustomerProduct.Tenant);
        //    }

        //    customerProductRepository = new CustomerProductRepository(objectContext);

        //    string entityName = "CustomerProduct" + currentCustomerProduct.CustomerId + currentCustomerProduct.Tenant;
        //    string entityPmName = "CustomerProductPM" + currentCustomerProduct.CustomerId + currentCustomerProduct.Tenant;

        //    if (CacheManager.CacheWrapper.Get(entityName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityName);
        //    }
        //    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
        //    {
        //        CacheManager.CacheWrapper.Invalidate(entityPmName);
        //    }
        //    CustomerProductService service = new CustomerProductService(objectContext, currentCustomerProduct.Tenant);
        //    service.Update(currentCustomerProduct);
        //    TableLastUpdateClass.UpdateTableHistory(currentCustomerProduct.Tenant, "CustomerProduct");
        //}

        //public void DeleteCustomerProduct(CustomerProductPM CustomerProduct)
        //{
        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(CustomerProduct.Tenant);
        //    }
        //    customerProductRepository = new CustomerProductRepository(objectContext);
        //    CustomerProduct entity = customerProductRepository.GetSingleCustomerProduct(CustomerProduct.CustomerId, CustomerProduct.ProductTypeCode, CustomerProduct.Tenant);
        //    customerProductRepository.Remove(entity);
        //}
	}
}