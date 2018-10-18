using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System.Transactions;
using WebFreight.Web.TopicQueues;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateCustomerFieldsUpdateSettingList(CustomerFieldsUpdateSettingList currentEntity)
        {
        }

        //public IQueryable<CustomerFieldsUpdateSetting> GetCustomerFieldsUpdateSettinges(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

        //    CustomerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);
        //    return CustomerFieldsUpdateSettingRepository.GetCustomerFieldsUpdateSettinges(0);
        //}

        public IQueryable<CustomerFieldsUpdateSettingPM> GetCustomerFieldsUpdateSettingesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

       
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            return customerFieldsUpdateSettingQuery.GetCustomerFieldsUpdateSettingPMsByTenant(tenant);
        }

        public CustomerFieldsUpdateSettingPM GetSingleCustomerFieldsUpdateSetting(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            //CustomerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);
            CustomerFieldsUpdateSettingQuery CustomerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            return CustomerFieldsUpdateSettingQuery.GetSinglePM(id, tenant);
        }

        public CustomerFieldsUpdateSettingPM GetCustomerFieldsUpdateSettingById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            //CustomerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);

            //CustomerFieldsUpdateSettingPM CustomerFieldsUpdateSetting = CustomerFieldsUpdateSettingRepository.GetSinglePM(id, tenant);
            CustomerFieldsUpdateSettingQuery CustomerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            return CustomerFieldsUpdateSettingQuery.GetSinglePM(id, tenant);
            //return CustomerFieldsUpdateSetting;
        }

        public CustomerFieldsUpdateSettingList GetSingleCustomerFieldsUpdateSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            CustomerFieldsUpdateSettingQuery CustomerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            return CustomerFieldsUpdateSettingQuery.GetSingleList(id, tenant);
       
            
        }

        public IQueryable<CustomerFieldsUpdateSettingList> GetCustomerFieldsUpdateSettingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            CustomerFieldsUpdateSettingRepository customerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(customerFieldsUpdateSettingRepository);
            IQueryable<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettinges = customerFieldsUpdateSettingRepository.GetCustomerFieldsUpdateSettings(tenant);
            IQueryable<CustomerFieldsUpdateSettingList> query2 = customerFieldsUpdateSettingQuery.GetIQueryableEntityList(CustomerFieldsUpdateSettinges);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerFieldsUpdateSettingList> GetCustomerFieldsUpdateSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            CustomerFieldsUpdateSettingRepository customerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(customerFieldsUpdateSettingRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettinges = customerFieldsUpdateSettingRepository.GetCustomerFieldsUpdateSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerFieldsUpdateSettinges = filter.GetFilteredQuery<CustomerFieldsUpdateSetting>(nonListQueryOperation, CustomerFieldsUpdateSettinges);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<CustomerFieldsUpdateSettingList> query2 = customerFieldsUpdateSettingQuery.GetIQueryableEntityList(CustomerFieldsUpdateSettinges);

            query2 = filter.GetFilteredQuery<CustomerFieldsUpdateSettingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerFieldsUpdateSettingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> customerFieldsUpdateObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerFieldsUpdateSetting", tenant).ToList();

                ObjectField objectField = (from a in customerFieldsUpdateObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerFieldsUpdateSettingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerFieldsUpdateSettingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerFieldsUpdateSettingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerFieldsUpdateSettingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerFieldsUpdateSettingList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.UpdateDirection);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.UpdateDirection);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetCustomerFieldsUpdateSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "READ", tenant);

            CustomerFieldsUpdateSettingRepository customerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(tenant);
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(customerFieldsUpdateSettingRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettinges = customerFieldsUpdateSettingRepository.GetCustomerFieldsUpdateSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CustomerFieldsUpdateSettinges = filter.GetFilteredQuery<CustomerFieldsUpdateSetting>(nonListQueryOperation, CustomerFieldsUpdateSettinges);

            IQueryable<CustomerFieldsUpdateSettingList> query2 = customerFieldsUpdateSettingQuery.GetIQueryableEntityList(CustomerFieldsUpdateSettinges);

            query2 = filter.GetFilteredQuery<CustomerFieldsUpdateSettingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //public void MapCustomerFieldsUpdateSettingCustomerFieldsUpdateSettingPM(CustomerFieldsUpdateSettingPM CustomerFieldsUpdateSettingPM, CustomerFieldsUpdateSetting CustomerFieldsUpdateSetting)
        //{
        //    CustomerFieldsUpdateSetting.EnglishName = CustomerFieldsUpdateSettingPM.EnglishName;
        //    CustomerFieldsUpdateSetting.Id = CustomerFieldsUpdateSettingPM.Id;
        //    CustomerFieldsUpdateSetting.InActive = CustomerFieldsUpdateSettingPM.InActive;
        //    CustomerFieldsUpdateSetting.LocalName = CustomerFieldsUpdateSettingPM.LocalName;
        //    CustomerFieldsUpdateSetting.Notes = CustomerFieldsUpdateSettingPM.Notes;
        //    CustomerFieldsUpdateSetting.Tenant = CustomerFieldsUpdateSettingPM.Tenant;
        //    CustomerFieldsUpdateSetting.SearchFields = CustomerFieldsUpdateSettingPM.EnglishName + "," + CustomerFieldsUpdateSettingPM.LocalName;
        //}

        public void InsertCustomerFieldsUpdateSetting(CustomerFieldsUpdateSettingPM CustomerFieldsUpdateSetting)
        {
            
                SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "NEW", CustomerFieldsUpdateSetting.Tenant);
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(CustomerFieldsUpdateSetting.Tenant);
                }
                CustomerFieldsUpdateSettingService service = new CustomerFieldsUpdateSettingService(objectContext, CustomerFieldsUpdateSetting.Tenant);
                service.Create(CustomerFieldsUpdateSetting);
                TableLastUpdateClass.UpdateTableHistory(CustomerFieldsUpdateSetting.Tenant, "CustomerFieldsUpdateSetting");
          
        }

        public void UpdateCustomerFieldsUpdateSetting(CustomerFieldsUpdateSettingPM currentCustomerFieldsUpdateSetting)
        {
            SecurityUtility.CheckContactFeature("CustomerFieldsUpdateSetting", "UPDATE", currentCustomerFieldsUpdateSetting.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentCustomerFieldsUpdateSetting.Tenant);
            }


            CustomerFieldsUpdateSettingRepository customerFieldsUpdateSettingRepository = new CustomerFieldsUpdateSettingRepository(objectContext);

            CustomerFieldsUpdateSettingService service = new CustomerFieldsUpdateSettingService(objectContext, currentCustomerFieldsUpdateSetting.Tenant);
            service.Update(currentCustomerFieldsUpdateSetting);
            TableLastUpdateClass.UpdateTableHistory(currentCustomerFieldsUpdateSetting.Tenant, "CustomerFieldsUpdateSetting");
  

        }

        public void DeleteCustomerFieldsUpdateSetting(CustomerFieldsUpdateSettingPM CustomerFieldsUpdateSetting)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(CustomerFieldsUpdateSetting.Tenant);
            }
            CustomerFieldsUpdateSettingRepository customerFieldsUpdateSettingRepository  = new CustomerFieldsUpdateSettingRepository(objectContext);
            CustomerFieldsUpdateSetting entity = customerFieldsUpdateSettingRepository.GetSingleCustomerFieldsUpdateSetting(CustomerFieldsUpdateSetting.Id, CustomerFieldsUpdateSetting.Tenant);
            customerFieldsUpdateSettingRepository.Remove(entity);
        }


        [Invoke]
        public bool CheckIfExistCustomerFieldsUpdateSetting(string objectFieldId, int tenant)
        {
            CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(tenant);
            bool result =customerFieldsUpdateSettingQuery.CheckIfExistCustomerFieldsUpdateSetting(objectFieldId, tenant);
            return result;

        }
    }
}