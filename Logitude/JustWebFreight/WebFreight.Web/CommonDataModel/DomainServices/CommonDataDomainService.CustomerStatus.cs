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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCustomerStatusList(CustomerStatusList currentEntity)
        {
        }

        public IQueryable<CustomerStatus> GetCustomerStatus(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusRepository = new CustomerStatusRepository(tenant);
            return customerStatusRepository.GetCustomerStatus();
        }

        public IQueryable<CustomerStatusPM> GetCustomerStatusByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusQuery = new CustomerStatusQuery(tenant);
            return customerStatusQuery.GetCustomerStatusPMs();
        }

        public CustomerStatusPM GetSingleCustomerStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusQuery = new CustomerStatusQuery(tenant);
            return customerStatusQuery.GetSinglePM(code);
        }

        public CustomerStatusList GetSingleCustomerStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusRepository = new CustomerStatusRepository(tenant);
            CustomerStatusList entityList = null;
            CustomerStatus entity = customerStatusRepository.GetSingleCustomerStatus(code);

            if (entity != null)
            {
                List<CustomerStatus> singleEntityList = new List<CustomerStatus>();
                singleEntityList.Add(entity);

                customerStatusQuery = new CustomerStatusQuery(customerStatusRepository);
                IQueryable<CustomerStatus> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerStatusList> iQueryableEntityList = customerStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }
            return entityList;
        }

        public IQueryable<CustomerStatusList> GetCustomerStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusRepository = new CustomerStatusRepository(tenant);
            customerStatusQuery = new CustomerStatusQuery(customerStatusRepository);

            IQueryable<CustomerStatus> iQueryable = customerStatusRepository.GetCustomerStatus();
            IQueryable<CustomerStatusList> query2 = customerStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerStatusList> GetCustomerStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusRepository = new CustomerStatusRepository(tenant);
            customerStatusQuery = new CustomerStatusQuery(customerStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerStatus> iQueryable = customerStatusRepository.GetCustomerStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerStatusList> query2 = customerStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomerStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerStatusList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomerStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerStatusRepository = new CustomerStatusRepository(tenant);
            customerStatusQuery = new CustomerStatusQuery(customerStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerStatus> iQueryable = customerStatusRepository.GetCustomerStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerStatus>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerStatusList> query2 = customerStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<CustomerStatusList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void MapCustomerStatusPMCustomerStatus(CustomerStatusPM CustomerStatusPM, CustomerStatus CustomerStatus)
        {
            CustomerStatus.Code = CustomerStatusPM.Code;
            CustomerStatus.Name = CustomerStatusPM.Name;
            CustomerStatus.SearchFields = CustomerStatusPM.Code + "," + CustomerStatusPM.Name;
        }

        public void InsertCustomerStatus(CustomerStatusPM entity)
        {
            CustomerStatus newEntity = new CustomerStatus();
            newEntity.Code = entity.Code;
            MapCustomerStatusPMCustomerStatus(entity, newEntity);
            customerStatusRepository.Add(newEntity);
        }

        public void UpdateCustomerStatus(CustomerStatusPM currentEntity)
        {
            CustomerStatus entity = customerStatusRepository.GetSingleCustomerStatus(currentEntity.Code);
            MapCustomerStatusPMCustomerStatus(currentEntity, entity);
            customerStatusRepository.Update(entity);
        }

        public void DeleteCustomerStatus(CustomerStatusPM entityPm)
        {
            CustomerStatus entity = customerStatusRepository.GetSingleCustomerStatus(entityPm.Code);
            customerStatusRepository.Remove(entity);
        }
    }
}