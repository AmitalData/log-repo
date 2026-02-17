using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
        public void UpdateCustomerTenantAccessStatusTypeList(CustomerTenantAccessStatusTypeList currentEntity)
        {
        }

        //public IQueryable<CustomerTenantAccessStatusType> GetCustomerTenantAccessStatusTypes(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
        //    return customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes();
        //}

        public IQueryable<CustomerTenantAccessStatusType> CustomerTenantAccessStatusTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
            return customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes();
        }

        public CustomerTenantAccessStatusTypePM GetSingleCustomerTenantAccessStatusType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeQuery = new CustomerTenantAccessStatusTypeQuery(tenant);
            return customerTenantAccessStatusTypeQuery.GetSinglePM(code);
        }

        public CustomerTenantAccessStatusTypeList GetSingleCustomerTenantAccessStatusTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
            CustomerTenantAccessStatusTypeList CustomerTenantAccessStatusTypeList = null;
            CustomerTenantAccessStatusType CustomerTenantAccessStatusType = customerTenantAccessStatusTypeRepository.GetSingleCustomerTenantAccessStatusType(code);

            if (CustomerTenantAccessStatusType != null)
            {
                List<CustomerTenantAccessStatusType> singleEntityList = new List<CustomerTenantAccessStatusType>();
                singleEntityList.Add(CustomerTenantAccessStatusType);

                customerTenantAccessStatusTypeQuery = new CustomerTenantAccessStatusTypeQuery(customerTenantAccessStatusTypeRepository);
                IQueryable<CustomerTenantAccessStatusType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerTenantAccessStatusTypeList> iQueryableEntityList = customerTenantAccessStatusTypeQuery.GetIQueryableEntityList(iQueryable);
                CustomerTenantAccessStatusTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return CustomerTenantAccessStatusTypeList;
        }

        public IQueryable<CustomerTenantAccessStatusTypeList> GetCustomerTenantAccessStatusTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
            customerTenantAccessStatusTypeQuery = new CustomerTenantAccessStatusTypeQuery(customerTenantAccessStatusTypeRepository);

            IQueryable<CustomerTenantAccessStatusType> iQueryable = customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes();
            IQueryable<CustomerTenantAccessStatusTypeList> query2 = customerTenantAccessStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerTenantAccessStatusTypeList> GetCustomerTenantAccessStatusTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
            customerTenantAccessStatusTypeQuery = new CustomerTenantAccessStatusTypeQuery(customerTenantAccessStatusTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccessStatusType> iQueryable = customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccessStatusType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerTenantAccessStatusTypeList> query2 = customerTenantAccessStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessStatusTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerTenantAccessStatusTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerTenantAccessStatusType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessStatusTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessStatusTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessStatusTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessStatusTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessStatusTypeList, bool>(queryOperations, query2);
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

        public int GetCustomerTenantAccessStatusTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessStatusTypeRepository = new CustomerTenantAccessStatusTypeRepository(tenant);
            customerTenantAccessStatusTypeQuery = new CustomerTenantAccessStatusTypeQuery(customerTenantAccessStatusTypeRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccessStatusType> iQueryable = customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccessStatusType>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerTenantAccessStatusTypeList> query2 = customerTenantAccessStatusTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessStatusTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertCustomerTenantAccessStatusType(CustomerTenantAccessStatusType entity)
        {
            customerTenantAccessStatusTypeRepository.Add(entity);
        }

        public void UpdateCustomerTenantAccessStatusType(CustomerTenantAccessStatusType currentEntity)
        {
           customerTenantAccessStatusTypeRepository.Update(currentEntity);
        }

        public void DeleteCustomerTenantAccessStatusType(CustomerTenantAccessStatusType entity)
        {
            customerTenantAccessStatusTypeRepository.Remove(entity);
        }
    }
}