using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {


        public CustomerTenantAccessCardsBatchPM GetSingleCustomerTenantAccessCardsBatch(string BatchNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(tenant);
            return customerTenantAccessCardBatchQuery.GetSinglePM(BatchNumber, tenant);
        }

        public IQueryable<CustomerTenantAccessCardsBatchList> GetCustomerTenantAccessCardsBatchLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessCardsBatchRepository = new CustomerTenantAccessCardsBatchRepository(tenant);
            customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(customerTenantAccessCardsBatchRepository);

            IQueryable<CustomerTenantAccessCardsBatch> iQueryable = customerTenantAccessCardsBatchRepository.GetCustomerTenantAccessCradsBatchByTenant(tenant);
            IQueryable<CustomerTenantAccessCardsBatchList> query2 = customerTenantAccessCardBatchQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        public IQueryable<CustomerTenantAccessCardsBatchPM> GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(string CustomerId, string CustomerTenantAccessId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(tenant);
            IQueryable<CustomerTenantAccessCardsBatchPM> q = customerTenantAccessCardBatchQuery.GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(CustomerId, CustomerTenantAccessId, tenant);
           
            return q;
        }

        
        public int GetCustomerTenantAccessCardsBatchFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessCardsBatchRepository = new CustomerTenantAccessCardsBatchRepository(tenant);
            customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(customerTenantAccessCardsBatchRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccessCardsBatch> iQueryable = customerTenantAccessCardsBatchRepository.GetCustomerTenantAccessCradsBatchByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccessCardsBatch>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerTenantAccessCardsBatchList> query2 = customerTenantAccessCardBatchQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessCardsBatchList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<CustomerTenantAccessCardsBatchList> GetCustomerTenantAccessCardsBatchFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("CustomerTenantAccessCardsBatch", "READ", tenant);

            customerTenantAccessCardsBatchRepository = new CustomerTenantAccessCardsBatchRepository(tenant);
            customerTenantAccessCardBatchQuery = new CustomerTenantAccessCardBatchQuery(customerTenantAccessCardsBatchRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerTenantAccessCardsBatch> countries = customerTenantAccessCardsBatchRepository.GetCustomerTenantAccessCradsBatchByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            countries = filter.GetFilteredQuery<CustomerTenantAccessCardsBatch>(nonListQueryOperation, countries);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerTenantAccessCardsBatchList> query2 = customerTenantAccessCardBatchQuery.GetIQueryableEntityList(countries);

            query2 = filter.GetFilteredQuery<CustomerTenantAccessCardsBatchList>(listQueryOperation, query2);
            //-------------------------------------------------------------------------------
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerTenantAccessCardsBatchList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> CustomerTenantAccessCardsBatchObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerTenantAccessCardsBatch", tenant).ToList();

                ObjectField objectField = (from a in CustomerTenantAccessCardsBatchObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessCardsBatchList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessCardsBatchList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessCardsBatchList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessCardsBatchList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessCardsBatchList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDateTime);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDateTime);
            }
            //--------------------------------------------------------------------------------------------------
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public void InsertCustomerTenantAccessCardsBatch(CustomerTenantAccessCardsBatchPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            //SecurityUtility.CheckContactFeature("CustomerTenantAccessCardsBatch", "New", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CustomerTenantAccessCardsBatchService service = new CustomerTenantAccessCardsBatchService(objectContext, entityPM.Tenant, entityPM);
            service.Create();
        }

        public void UpdateCustomerTenantAccessCardsBatch(CustomerTenantAccessCardsBatchPM entityPM)
        {
             
        }

       

    }
}