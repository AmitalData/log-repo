using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCustomerTenantAccessRequestList(CustomerTenantAccessRequestList currentEntity)
        {
        }

        public IQueryable<CustomerTenantAccessRequest> CustomerTenantAccessRequestesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRequestRepository = new CustomerTenantAccessRequestRepository(tenant);
            return customerTenantAccessRequestRepository.GetCustomerTenantAccessRequestsByTenant(tenant);
        }

        public CustomerTenantAccessRequestPM GetSingleCustomerTenantAccessRequest(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(tenant);
            return customerTenantAccessRequestQuery.GetSinglePM(id, tenant);
        }

        public CustomerTenantAccessRequestList GetSingleCustomerTenantAccessRequestList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "READ", tenant);

            customerTenantAccessRequestRepository = new CustomerTenantAccessRequestRepository(tenant);
            CustomerTenantAccessRequestList CustomerTenantAccessRequestList = null;
            CustomerTenantAccessRequest CustomerTenantAccessRequest = customerTenantAccessRequestRepository.GetSingleCustomerTenantAccessRequest(id, tenant);

            if (CustomerTenantAccessRequest != null)
            {
                List<CustomerTenantAccessRequest> singleEntityList = new List<CustomerTenantAccessRequest>();
                singleEntityList.Add(CustomerTenantAccessRequest);

                customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessRequestRepository);
                IQueryable<CustomerTenantAccessRequest> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerTenantAccessRequestList> iQueryableEntityList = customerTenantAccessRequestQuery.GetIQueryableEntityList(iQueryable);
                CustomerTenantAccessRequestList = iQueryableEntityList.FirstOrDefault();
            }
            return CustomerTenantAccessRequestList;
        }


        public IQueryable<CustomerTenantAccessRequestList> GetCustomerTenantAccessRequestLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRequestRepository = new CustomerTenantAccessRequestRepository(tenant);
            customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessRequestRepository);

            IQueryable<CustomerTenantAccessRequest> iQueryable = customerTenantAccessRequestRepository.GetCustomerTenantAccessRequestsByTenant(tenant);
            IQueryable<CustomerTenantAccessRequestList> query2 = customerTenantAccessRequestQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerTenantAccessRequestList> GetCustomerTenantAccessRequestFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRequestRepository = new CustomerTenantAccessRequestRepository(tenant);
            customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessRequestRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccessRequest> iQueryable = customerTenantAccessRequestRepository.GetCustomerTenantAccessRequestsByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccessRequest>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerTenantAccessRequestList> query2 = customerTenantAccessRequestQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessRequestList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerTenantAccessRequestList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerTenantAccessRequest", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessRequestList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessRequestList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessRequestList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessRequestList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessRequestList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.RequestDateTime);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomerTenantAccessRequestFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRequestRepository = new CustomerTenantAccessRequestRepository(tenant);
            customerTenantAccessRequestQuery = new CustomerTenantAccessRequestQuery(customerTenantAccessRequestRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccessRequest> iQueryable = customerTenantAccessRequestRepository.GetCustomerTenantAccessRequestsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccessRequest>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerTenantAccessRequestList> query2 = customerTenantAccessRequestQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessRequestList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertCustomerTenantAccessRequestPM(CustomerTenantAccessRequestPM entity)
        {
            SecurityUtility.AuthenticationOnTenant(entity.Tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(objectContext, entity.Tenant);
            service.Create(entity);
        }

        public void UpdateCustomerTenantAccessRequest(CustomerTenantAccessRequestPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccessRequest", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            CustomerTenantAccessRequestService service = new CustomerTenantAccessRequestService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
        }

        public void DeleteCustomerTenantAccessRequest(CustomerTenantAccessRequestPM entity)
        {

        }
    }
}