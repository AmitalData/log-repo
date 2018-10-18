using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
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
        public void UpdateCustomerTenantAccessList(CustomerTenantAccessList currentEntity)
        {
        }

        //public IQueryable<CustomerTenantAccess> GetCustomerTenantAccesses(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
        //    return customerTenantAccessRepository.GetCustomerTenantAccessesByTenant(tenant);
        //}

        public IQueryable<CustomerTenantAccess> CustomerTenantAccessesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
            return customerTenantAccessRepository.GetCustomerTenantAccessesByTenant(tenant);
        }

        public CustomerTenantAccessPM GetSingleCustomerTenantAccess(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
            return customerTenantAccessQuery.GetSinglePM(id,tenant);
        }

        public CustomerTenantAccessList GetSingleCustomerTenantAccessList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", tenant);

            customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
            CustomerTenantAccessList customerTenantAccessList = null;
            CustomerTenantAccess customerTenantAccess = customerTenantAccessRepository.GetSingleCustomerTenantAccess(id,tenant);

            if (customerTenantAccess != null)
            {
                List<CustomerTenantAccess> singleEntityList = new List<CustomerTenantAccess>();
                singleEntityList.Add(customerTenantAccess);

                customerTenantAccessQuery = new CustomerTenantAccessQuery(customerTenantAccessRepository);
                IQueryable<CustomerTenantAccess> iQueryable = singleEntityList.AsQueryable();
                IQueryable<CustomerTenantAccessList> iQueryableEntityList = customerTenantAccessQuery.GetIQueryableEntityList(iQueryable);
                customerTenantAccessList = iQueryableEntityList.FirstOrDefault();
            }
            return customerTenantAccessList;
        }


        public IQueryable<CustomerTenantAccessList> GetCustomerTenantAccessLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
            customerTenantAccessQuery = new CustomerTenantAccessQuery(customerTenantAccessRepository);

            IQueryable<CustomerTenantAccess> iQueryable = customerTenantAccessRepository.GetCustomerTenantAccessesByTenant(tenant);
            IQueryable<CustomerTenantAccessList> query2 = customerTenantAccessQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<CustomerTenantAccessList> GetCustomerTenantAccessFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
            customerTenantAccessQuery = new CustomerTenantAccessQuery(customerTenantAccessRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccess> iQueryable = customerTenantAccessRepository.GetCustomerTenantAccessesByTenant(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccess>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerTenantAccessList> query2 = customerTenantAccessQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerTenantAccessList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("CustomerTenantAccess", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CustomerTenantAccessList, bool>(queryOperations, query2);
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

        public int GetCustomerTenantAccessFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customerTenantAccessRepository = new CustomerTenantAccessRepository(tenant);
            customerTenantAccessQuery = new CustomerTenantAccessQuery(customerTenantAccessRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomerTenantAccess> iQueryable = customerTenantAccessRepository.GetCustomerTenantAccessesByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<CustomerTenantAccess>(nonListQueryOperation, iQueryable);

            IQueryable<CustomerTenantAccessList> query2 = customerTenantAccessQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<CustomerTenantAccessList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertCustomerTenantAccessPM(CustomerTenantAccessPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccess", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CustomerTenantAccessService service = new CustomerTenantAccessService(objectContext, entityPM.Tenant, entityPM, ServiceContext.User.Identity.Name);
            service.Create();

            //ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomerTenantAccess", 0, true);
            //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", entityPM.UpdatedByUserId);

          

            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(entityPM.Tenant);
            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomerTenantAccess", webfreightcontext);
        }

        public void UpdateCustomerTenantAccess(CustomerTenantAccessPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccess", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            List<CustomerTenantAccessCardPM> customerTenantAccessCardsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerTenantAccessCards).Cast<CustomerTenantAccessCardPM>().ToList();
            foreach (CustomerTenantAccessCardPM itemPM in customerTenantAccessCardsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }


            CustomerTenantAccessService service = new CustomerTenantAccessService(objectContext, entityPM.Tenant, entityPM, ServiceContext.User.Identity.Name);
            service.SetChangeSet(customerTenantAccessCardsChangeSet);
            service.Update();

            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(entityPM.Tenant);

            //ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CustomerTenantAccess",0, true);
            //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);
            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomerTenantAccess", webfreightcontext);
        }

        public CustomerTenantAccessRequestStatusCount GetCustomerTenantAccessRequestStatusCount(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomerTenantAccessRequestStatusCount dataClass = new CustomerTenantAccessRequestStatusCount() { Id = "0001" };

            customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
            IQueryable<CustomerTenantAccessList> CustomerTenantAccess = customerTenantAccessQuery.GetCustomerTenantAccessListByTenant(tenant);



            if (CustomerTenantAccess != null && CustomerTenantAccess.Count() > 0)
            {
                dataClass.WaitingCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "W").Count();
                dataClass.InProgressCount= CustomerTenantAccess.Where(d => d.Status.ToUpper() == "IP").Count();
                dataClass.AcceptedCount = CustomerTenantAccess.Where(d => d.Status.ToUpper() == "A").Count();
                
            }

           

            return dataClass;
        }

        public List<CustomerTenantAccessList> GetLastCustomerRequest(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", tenant);

            customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant); 
            List<CustomerTenantAccessList> list = customerTenantAccessQuery.GetLastCustomerRequests(tenant);
              
            return list;

        }

        public List<CustomerTenantAccessCardPM> GetCustomerTenantAccessCardLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("CustomerTenantAccessCard", "READ", tenant);


            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            var customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            var temp = customerTenantAccessCardQuery.GetCustomerTenantAccessCardsByTenant(tenant);
            return temp;

        }

        public CustomerTenantAccessCardPM GetIfCustomerTenantAccessCardsSelected(string CustomerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("CustomerTenantAccessCard", "READ", tenant);


            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            var customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            var temp = customerTenantAccessCardQuery.GetIfCustomerTenantAccessCardsSelected(CustomerId,tenant);
            return temp;

        }

        public CustomerTenantAccessCardPM GetCustomerTenantAccessCard(string CustomerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("CustomerTenantAccessCard", "READ", tenant);


            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            var customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(tenant);
            var temp = customerTenantAccessCardQuery.GetSingleCustomerTenantAccessCardPMById(CustomerId, tenant);
            return temp;

        }

        public CustomerTenantAccessPM GetCustomerTenantAccessPMsByTenantCustomerTenant(int tenant, int CustomerTenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
            return customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenantCustomerTenant(tenant, CustomerTenant);
           
        }
       
    }
}