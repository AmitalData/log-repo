using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.BL.DataContracts;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateCustomerList(CustomerList currentEntity)
        {
        }

        public IQueryable<CustomerPM> GetCustomersByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            return this.customerQuery.GetCustomerPMsByTenant(tenant);
        }

        public CustomerPM GetCustomerById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            CustomerPM entityPM = customerQuery.GetSinglePM(id, tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            if (myFilter.IsCustomerAllowed(entityPM))
            {
                return entityPM;
            }

            else
            {
                throw new ApplicationException("Can't view this Customer due to Business unit access level");
            }
        }

        public List<EntityPartner> GetEntityPartners(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);
            List<EntityPartner> list = new List<EntityPartner>();
            ContactQuery query = new ContactQuery(tenant);
            List<ContactPM> customerContacts = query.GetContactsbyCardId(entityId, tenant).ToList();

            int idCounter = 0;
                
            //foreach (ContactPM contact in customerContacts)
            //{
                list.Add(new EntityPartner()
                        {
                            Id = idCounter++,
                            PartnerId = entityId,//contact.Id,
                            PartnerType = "Customer Contacts",
                           // PartnerContactId = contact.Id,
                          //  IsUser = true,
                           // PartnerContactName = contact.EnglishName,
                           // PartnerContactMail = contact.Email,
                        });


           // }


            return list;
        }

        public CustomerList GetSingleCustomerList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            return customerQuery.GetSingleCustomerListById(id, tenant);
            }

        public IQueryable<CustomerList> GetCustomerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);
            IQueryable<CustomerList> query2 = customerQuery.GetIQueryableEntityList(customers);

            CustomerBusinessUnitFilter filter = new CustomerBusinessUnitFilter(tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Customer", tenant, query2.Cast<object>().ToList());

            return query2;
        }

        [Query(HasSideEffects = true)]
        public List<CustomerList> GetCustomerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(CustomerRepository);
            return customerQuery.GetCustomerFilters(xmlFilters, tenant);
        }

        [Query(HasSideEffects = true)]
        public List<CustomerList> GetCustomerCompactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);

            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerCustomFilter customfilters = new CustomerCustomFilter(tenant);
            customers = customfilters.GetFilteredQuery(queryOperations, customers);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            customers = myFilter.RunFilter(customers);

            customers = filter.GetFilteredQuery<CustomersDataView>(nonListQueryOperation, customers);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerList> query2 = customerQuery.GetIQueryableEntityList(customers);
            query2 = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);

            List<CustomerList> resultList;

            queryOperations.SortByColumnName = "EnglishName";
            queryOperations.SortDirectin = "Ascending";
            query2 = QuerySortClass.GetSortedQuery(queryOperations, query2, "Card", tenant);

            if (seachvalue != null)
            {
                listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);
                IQueryable<CustomerList> nameQueryResult = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2).Take(queryOperations.PageSize);

               
                resultList = nameQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("Code", seachvalue, false, "StartsWith", null, false);

                    IQueryable<CustomerList> coedQueryResult = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);

                    foreach (CustomerList card in coedQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == card.Code).Any())
                        {
                            resultList.Add(card);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                        IQueryable<CustomerList> searchFieldQueryResult = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);


                        foreach (CustomerList card in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == card.Code).Any())
                            {
                                resultList.Add(card);

                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }

                query2 = resultList.AsQueryable();
            }




            IQueryable<CustomerList> bigQuery = query2;

            bigQuery = bigQuery.Skip(skippedPorts);
            bigQuery = bigQuery.Take(queryOperations.PageSize);


            List<CustomerList> listQuery = bigQuery.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Customer", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public int GetCustomerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            customers = myFilter.RunFilter(customers);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerCustomFilter customfilters = new CustomerCustomFilter(tenant);
            customers = customfilters.GetFilteredQuery(queryOperations, customers);

            customers = filter.GetFilteredQuery<CustomersDataView>(nonListQueryOperation, customers);
            IQueryable<CustomerList> query2 = customerQuery.GetIQueryableEntityList(customers);

            query2 = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public bool DoesCustomerVatNumberExist(string number, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            //CustomerRepository = new CustomerRepository(tenant);
            //return (CustomerRepository.GetCustomers(tenant).Where(d => d.Card.VatNumber == number && d.Tenant == tenant)).Any();
            return CustomerService.DoesCustomerVatNumberExist(number, tenant); //mirit 17/02/16 Task 20233
        }


        public bool DoesCustomerCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomerRepository = new CustomerRepository(tenant);
            return (CustomerRepository.GetCustomers(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<CustomerPM> GetCustomersSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            IQueryable<CustomerPM> q = customerQuery.GetCustomersByNameOrCode(code, name, tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            q = myFilter.RunFilter(q);

            return q;
        }

        public List<CustomerProductPM> GetCustomerProducts(string customerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerProductQuery query = new CustomerProductQuery(tenant);
            return query.GetCustomerProductPMsByCustomerId(customerId, tenant);
        }

        public List<CustomerProductActualDataPM> GetCustomerActualData(string customerId, int year, int month, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerProductActualDataQuery query = new CustomerProductActualDataQuery(tenant);
            return query.GetCustomerActualData(customerId, year, month, tenant);
        }

        public List<CustomerProductActualDataPM> GetCustomerProductHistoryActualData(string customerId, string productTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerProductActualDataQuery query = new CustomerProductActualDataQuery(tenant);
            return query.GetCustomerActualData(customerId, productTypeCode, tenant);
        }

        public List<CustomerProductLocationPM> GetCustomerProductLocations(string customerId, string productTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerProductLocationQuery query = new CustomerProductLocationQuery(tenant);
            return query.GetLocationPMsByProduct(customerId, productTypeCode, tenant).ToList();            
        }

        public List<CustomerProductLocationActualDataPM> GetCustomerProductLocationsActualData(string customerId, string productTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerProductLocationActualDataQuery query = new CustomerProductLocationActualDataQuery(tenant);
            return query.GetCustomerProductLocationPMs(customerId, productTypeCode, tenant).ToList();
        }

        [Invoke]
        public void UpdateCustomerActualData(string customerId,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "UPDATE", tenant);
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                CommonModelProcedureClass.ExecuteSingleCustomerActualData(customerId, tenant);
                scope.Complete();
            }
        }

        public void InsertCustomer(CustomerPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Customer", "NEW", entityPM.Tenant);

            CustomerService.CreateNew(objectContext, entityPM, null);//itzik + mirit !!!
        }

        

        public void UpdateCustomer(CustomerPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);

            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            CustomerRepository = new CustomerRepository(entityPM.Tenant);

            Card card_Poco = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);
            Customer entity_Poco = CustomerRepository.GetSingleCustomer(entityPM.Id, entityPM.Tenant, false);

            this.CheckFeatureAccessLevel(entityPM.Tenant, "UPDATE", card_Poco.CreatedByUserId, entity_Poco.SalesmanUserId);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            List<CustomerSalesNotePM> salesNotesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.SalesNotes).Cast<CustomerSalesNotePM>().ToList();
            foreach (CustomerSalesNotePM itemPM in salesNotesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                }
            }

            List<CustomerProductPM> productsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerProducts).Cast<CustomerProductPM>().ToList();
            foreach (CustomerProductPM itemPM in productsChangeSet)
            {                
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.LocationsChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ProductLocations).Cast<CustomerProductLocationPM>().ToList();

                            foreach (CustomerProductLocationPM locationPM in itemPM.LocationsChangeSet)
                            {
                                switch (ChangeSet.GetChangeOperation(locationPM))
                                {
                                    case ChangeOperation.Insert: { locationPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { locationPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { locationPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { locationPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerCompetitorPM> competitorsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerCompetitors).Cast<CustomerCompetitorPM>().ToList();
            foreach (CustomerCompetitorPM itemPM in competitorsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;

                            itemPM.CustomerCompetitorProducts = ChangeSet.GetAssociatedChanges(itemPM, d => d.CustomerCompetitorProducts).Cast<CustomerCompetitorProductPM>().ToList();

                            foreach (CustomerCompetitorProductPM productPM in itemPM.CustomerCompetitorProducts)
                            {
                                switch (ChangeSet.GetChangeOperation(productPM))
                                {
                                    case ChangeOperation.Insert: { productPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                                    case ChangeOperation.Update: { productPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                                    case ChangeOperation.Delete: { productPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                                    default: { productPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerAdditionalServicePM> servicesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerAdditionalServices).Cast<CustomerAdditionalServicePM>().ToList();
            foreach (CustomerAdditionalServicePM itemPM in servicesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerSalesmanByProductPM> customerSalesmanByProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerSalesmanByProducts).Cast<CustomerSalesmanByProductPM>().ToList();
            foreach (CustomerSalesmanByProductPM itemPM in customerSalesmanByProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerAccountManagerByProductPM> customerAccountManagerByProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerAccountManagerByProducts).Cast<CustomerAccountManagerByProductPM>().ToList();
            foreach (CustomerAccountManagerByProductPM itemPM in customerAccountManagerByProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerCustomsAgentByProductPM> customerCustomsAgentByProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerCustomsAgentByProducts).Cast<CustomerCustomsAgentByProductPM>().ToList();
            foreach (CustomerCustomsAgentByProductPM itemPM in customerCustomsAgentByProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerForwarderByProductPM> customerForwarderByProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerForwarderByProducts).Cast<CustomerForwarderByProductPM>().ToList();
            foreach (CustomerForwarderByProductPM itemPM in customerForwarderByProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CustomerMediatorByProductPM> customerMediatorByProductsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerMediatorByProducts).Cast<CustomerMediatorByProductPM>().ToList();
            foreach (CustomerMediatorByProductPM itemPM in customerMediatorByProductsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
            foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrenciesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            List<ProductItemPM> productItemsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomerProductItems).Cast<ProductItemPM>().ToList();
            foreach (ProductItemPM itemPM in productItemsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }

            CustomerService service = new CustomerService(objectContext, entityPM);
            service.SetChangeSet(salesNotesChangeSet, productsChangeSet, competitorsChangeSet, servicesChangeSet, customerSalesmanByProductsChangeSet, customerAccountManagerByProductsChangeSet, customerCustomsAgentByProductsChangeSet, customerForwarderByProductsChangeSet, customerMediatorByProductsChangeSet, cardExternalCodeByCurrenciesChangeSet,productItemsChangeSet);
            service.Update();

            if (this.ChangeSet != null)
            {
                this.ChangeSet.Associate(entityPM, service.entityPOCO, MapBackCustomer);
            }
        }


        private void MapBackCustomer(CustomerPM entityPM, Customer entityPoco)
        {
            if (!string.IsNullOrEmpty(entityPoco.LeadSourceId))
            {
                LeadSourceRepository leadSourceRepository = new LeadSourceRepository(entityPoco.Tenant);
                LeadSource leadSource = leadSourceRepository.GetSingleLeadSource(entityPoco.LeadSourceId, entityPoco.Tenant);
                if (leadSource != null)
                {
                    entityPM.LeadSourceName = leadSource.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPoco.Card.PrimaryContactId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPoco.Tenant);
                Contact primaryContact = contactRepository.GetSingleContact(entityPoco.Card.PrimaryContactId, entityPoco.Tenant);

                if (primaryContact != null)
                {
                    entityPM.PrimaryContactName = primaryContact.EnglishName;
                    entityPM.PrimaryContactEmail = primaryContact.Email;
                    entityPM.PrimaryContactPhone = primaryContact.BusinessPhone;
                }
            }
        }

        public void DeleteCustomer(CustomerPM customer)
        {

        }

        private void CheckFeatureAccessLevel(int tenant, string featureCode, string createdByUserId, string salesmanUserId)
        {
            string myUserId = createdByUserId;
            string myBusinessUnitId = tenant.ToString();

            if (string.IsNullOrEmpty(myUserId))
            {
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                if (loggedContact != null)
                {
                    myUserId = loggedContact.Id;
                }
            }

            if (!string.IsNullOrEmpty(salesmanUserId))
            {
                myUserId = salesmanUserId;

                UserRepository userRepository = new UserRepository(tenant);
                User user = userRepository.GetSingleUser(myUserId, tenant, false);
                if (user != null)
                {
                    myBusinessUnitId = user.BusinessUnitId;
                }
            }

            SecurityUtility.CheckFeatureAccessLevelPermission("Customer", featureCode, myUserId, myBusinessUnitId, tenant);
        }

        public CRMSummary GetCustomersSummary(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerRepository = new CustomerRepository(tenant);

            IQueryable<CustomersDataView> dataSource = CustomerRepository.GetCustomersDataViews(tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            dataSource = myFilter.RunFilter(dataSource);

            string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
            ContactRepository myContactRepository = new ContactRepository(tenant);
            Contact loggedContact = myContactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);

            CRMSummary summaryClass = new CRMSummary();
            
            summaryClass.MyOpenDataCount = dataSource.Where(d => d.SalesmanUserId == loggedContact.Id && d.IsCustomer == true && d.InActive == false).Count();
            summaryClass.MyOpenAsAccountManagerDataCount = dataSource.Where(d => d.AccountManagerUserId == loggedContact.Id && d.IsCustomer == true && d.InActive == false).Count();

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.SalesmanBusinessUnitId == businessUnitId);
            }

            summaryClass.Customers_Waiting = dataSource.Where(d => d.CustomerStatusCode == "WAC" && d.IsCustomer).Count();
            summaryClass.Customers_Potential = dataSource.Where(d => d.CustomerStatusCode == "POT" && d.IsCustomer).Count();
            summaryClass.Customers_Active = dataSource.Where(d => d.CustomerStatusCode == "ACT" && d.IsCustomer).Count();
            summaryClass.Customers_Inactive = dataSource.Where(d => d.CustomerStatusCode == "INA" && d.IsCustomer).Count();
            return summaryClass;
        }

        public IQueryable<CompareDataClass> GetCustomersDecreasedShipments(string dataTypeCode, DateTime startDate, string timeRange, string ownerId, string businessUnitId, int tenant, string RecordsTypeCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            IQueryable<CompareDataClass> myResult = null;
            CustomerProductActualDataRepository myRepository = new CustomerProductActualDataRepository(tenant);
            IQueryable<CustomerProductActualData> dataSource = myRepository.GetCustomerProductActualDatas(tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            dataSource = myFilter.RunFilter(dataSource);

            if (dataSource.Count() > 0)
            {
                if (RecordsTypeCode == "C")
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.Customer.Card.CreatedByUserId == ownerId);
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.Customer.SalesmanUserId == ownerId);
                    }

                    if (!string.IsNullOrEmpty(businessUnitId))
                    {
                        dataSource = dataSource.Where(d => d.Customer.SalesmanUser.BusinessUnitId == businessUnitId);
                    }
                }

                if (dataSource.Count() > 0)
                {
                    int n = 1;

                    if (timeRange == "AV.03")
                    {
                        n = 3;
                    }

                    else if (timeRange == "AV.12")
                    {
                        n = 12;
                    }

                    IQueryable<CustomerProductActualData> iQueryable_New = (from a in dataSource where a.Year == startDate.Year && a.Month == startDate.Month select a);
                    IQueryable<CustomerProductActualData> iQueryable_Old = null;

                    for (int i = 1; i <= n; i++)
                    {
                        DateTime myDateTime = startDate.AddMonths(-1 * i);

                        if (iQueryable_Old == null)
                        {
                            iQueryable_Old = (from a in dataSource where a.Year == myDateTime.Year && a.Month == myDateTime.Month select a);
                        }

                        else
                        {
                            IQueryable<CustomerProductActualData> myData = (from a in dataSource where a.Year == myDateTime.Year && a.Month == myDateTime.Month select a);
                            iQueryable_Old = iQueryable_Old.Concat(myData);
                        }
                    }

                    myResult = (

                        (from a in iQueryable_New
                         group a by new { a.CustomerId, a.Customer.Card.EnglishName, a.Customer.Rank } into g1
                         select new CompareDataClass()
                         {
                             Id = g1.Key.CustomerId,
                             EntityName = g1.Key.EnglishName,
                             RankCode = g1.Key.Rank.Code,
                             RankName = g1.Key.Rank.Name,

                             TEU_New = g1.Sum(s => s.TEU),
                             Revenue_New = g1.Sum(s => s.Revenue),
                             ChargeableWeight_New = g1.Sum(s => s.ChargeableWeight),
                             NumberOfShipments_New = g1.Sum(s => s.NumberOfShipments),

                             TEU_Old = 0,
                             Revenue_Old = 0,
                             ChargeableWeight_Old = 0,
                             NumberOfShipments_Old = 0,

                             TEU = 0,
                             Revenue = 0,
                             ChargeableWeight = 0,
                             NumberOfShipments = 0,
                         })

                            .Union

                            (from b in iQueryable_Old
                             group b by new { b.CustomerId, b.Customer.Card.EnglishName, b.Customer.Rank } into g2
                             select new CompareDataClass()
                             {
                                 Id = g2.Key.CustomerId,
                                 EntityName = g2.Key.EnglishName,
                                 RankCode = g2.Key.Rank.Code,
                                 RankName = g2.Key.Rank.Name,

                                 TEU_New = 0,
                                 Revenue_New = 0,
                                 ChargeableWeight_New = 0,
                                 NumberOfShipments_New = 0,

                                 TEU_Old = g2.Sum(s => s.TEU) / n,
                                 Revenue_Old = g2.Sum(s => s.Revenue) / n,
                                 ChargeableWeight_Old = g2.Sum(s => s.ChargeableWeight) / n,
                                 NumberOfShipments_Old = g2.Sum(s => s.NumberOfShipments) / n,

                                 TEU = 0,
                                 Revenue = 0,
                                 ChargeableWeight = 0,
                                 NumberOfShipments = 0,
                             })
                            )

                            .GroupBy(d => new { d.Id, d.EntityName, d.RankCode, d.RankName })

                            .Select(s => new CompareDataClass()
                            {
                                Id = s.Key.Id,
                                EntityName = s.Key.EntityName,
                                RankCode = s.Key.RankCode,
                                RankName = s.Key.RankName,

                                TEU_New = s.Sum(k => k.TEU_New),
                                Revenue_New = s.Sum(k => k.Revenue_New),
                                ChargeableWeight_New = s.Sum(k => k.ChargeableWeight_New),
                                NumberOfShipments_New = s.Sum(k => k.NumberOfShipments_New),

                                TEU_Old = s.Sum(k => k.TEU_Old),
                                Revenue_Old = s.Sum(k => k.Revenue_Old),
                                ChargeableWeight_Old = s.Sum(k => k.ChargeableWeight_Old),
                                NumberOfShipments_Old = s.Sum(k => k.NumberOfShipments_Old),

                                TEU = s.Sum(k => k.TEU_New - k.TEU_Old),
                                Revenue = s.Sum(k => k.Revenue_New - k.Revenue_Old),
                                ChargeableWeight = s.Sum(k => k.ChargeableWeight_New - k.ChargeableWeight_Old),
                                NumberOfShipments = s.Sum(k => k.NumberOfShipments_New - k.NumberOfShipments_Old),
                            });

                    int takeNumber = 20;

                    switch (dataTypeCode)
                    {
                        case "S":
                            {
                                myResult = myResult.Where(d => d.NumberOfShipments < 0).OrderBy(o => o.NumberOfShipments).Take(takeNumber);
                                break;
                            }

                        case "T":
                            {
                                myResult = myResult.Where(d => d.TEU < 0).OrderBy(o => o.TEU).Take(takeNumber);
                                break;
                            }

                        case "R":
                            {
                                myResult = myResult.Where(d => d.Revenue < 0).OrderBy(o => o.Revenue).Take(takeNumber);
                                break;
                            }

                        case "C":
                            {
                                myResult = myResult.Where(d => d.ChargeableWeight < 0).OrderBy(o => o.ChargeableWeight).Take(takeNumber);
                                break;
                            }
                    }
                }
            }

            return myResult;
        }

        public List<CustomerSalesNotePM> GetCustomerSalesNotes(string entityId, int tenant)
        {
            List<CustomerSalesNotePM> myResult = new List<CustomerSalesNotePM>();

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(tenant);
            CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
            myResult = salesNoteQuery.GetSalesNotesByCustomerId(entityId, tenant).ToList();

            return myResult;
        }

        public List<CustomerList> GetRecentCustomers(string ownerId, string businessUnitId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            string mail = SecurityUtility.GetAuthenticatedUser();
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM contact = contactQuery.GetContactByEmailOnly(mail, tenant);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Customer", 0, true);

            customerQuery = new CustomerQuery(tenant);
            IQueryable<CustomerList> first = customerQuery.GetRecentEntityLists(tenant, contact.Id, objectTable.Id).AsQueryable();
            IQueryable<CustomerList> myResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myResult = myFilter.RunFilter(myResult);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Customer", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        public List<CustomerCurrencyCode> GetCustomersCurrencies(string customerId, int tenant)
        {
            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            List<CustomerCurrencyCode> customerCurrencyCodes = customerQuery.GetCurrencyCodeForCustomer(customerId, tenant);

            return customerCurrencyCodes;

        }

        public CardExternalCodeByCurrencyPM GetSingleCardExternalCodeByCurrencyPMByCardIDandCurrencyID(string cardId, string currencyId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);


            return customerQuery.GetSingleCardExternalCodeByCurrencyPM(cardId, currencyId, tenant);
        }

        public void UpdateChartingDataClass(ChartingDataClass entity)
        {

        }


        public List<ChartingDataClass> GetCustomersGroupBySalesmanCustom(DateTime? FromDate,DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            List<ChartingDataClass> myResult = customerQuery.GetCustomersGroupBySalesmanCustom(FromDate,ToDate, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            foreach (ChartingDataClass item in myResult)
            {
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    item.ShortLabelProperty = nameString[0];
                }
            }

            return myResult;
        }

        public List<ChartingDataClass> GetCustomersGroupBySalesman(int days, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            List<ChartingDataClass> myResult = customerQuery.GetCustomersGroupBySalesman(days, ownerId, businessUnitId, fieldCode, tenant, isTopTen);

            foreach (ChartingDataClass item in myResult)
            {
                if (item.StringProperty != null)
                {
                    string[] nameString = item.StringProperty.Split(' ');
                    item.ShortLabelProperty = nameString[0];
                    item.Code = days.ToString();
                }
            }

            return myResult;
        }

        [Invoke]
        public bool IsCustomerConnectedToEntities(string customerId, int tenant)
        {
            bool myResult = false;

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            myResult = shipmentRepository.IsCustomerConnectedToShipments(customerId, tenant);

            return myResult;
        }

        public List<CustomerList> GetCustomersQuickSearch(int tenant, string mySearchText)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            List<CustomerList> myResult = new List<CustomerList>();

            CustomerRepository = new CustomerRepository(tenant);
            customerQuery = new CustomerQuery(CustomerRepository);

            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);
            customers = customers.Where(d => d.IsCustomer == true);
            customers = customers.Where(d => d.CustomerStatusCode != "INA");

            bool setBlockedFlag = true;
            CustomerBusinessUnitFilter myBusinessUnitFilter = new CustomerBusinessUnitFilter(tenant);

            if (!SecurityUtility.CheckTableContactFeature("Customer", "QUICKSEARCHALL", tenant))
            {              
                setBlockedFlag = false;
                customers = myBusinessUnitFilter.RunFilter(customers);
            }

            if (!string.IsNullOrEmpty(mySearchText) && FeatureToggleHelper.HasFeatureToggle("CQS", tenant))
            {
                myResult = GetCustomerListsByApplyCardSearchMechanizm(tenant, mySearchText, customers);
            }
            else
            {
                if (!string.IsNullOrEmpty(mySearchText))
                {
                    customers = customers.Where(d => d.SearchFields.ToLower().Contains(mySearchText.ToLower()));
                }

                if (customers.Count() > 0)
                {
                    customers = customers.OrderByDescending(d => d.EnglishName);
                    customers = customers.Take(11);

                    IQueryable<CustomerList> myListQuery = customerQuery.GetIQueryableEntityList(customers);
                    myResult = myListQuery.ToList();
                }
            }

            if (setBlockedFlag)
            {
                myBusinessUnitFilter.SetBlockedQuickSearch(myResult);
            }

            return myResult;
        }

        private List<CustomerList> GetCustomerListsByApplyCardSearchMechanizm(int tenant, string mySearchText, IQueryable<CustomersDataView> customers)
        {
            List<CustomerList> myResult;

            IQueryable<CustomerList> myListQuery = customerQuery.GetIQueryableEntityList(customers);
            CustomerSearchArgs customerSearchArgs = new CustomerSearchArgs()
            {
                Tenant = tenant,
                SearchText = mySearchText,
                PageSize = 11,
                EntityLists = myListQuery,
                FilterItems = GetQueryFilterItems(),
            };
            CustomerDataSearchService customerDataSearchService = new CustomerDataSearchService();
            myResult = customerDataSearchService.Run(customerSearchArgs);

            return myResult;
        }

        private List<QueryFilterItem>  GetQueryFilterItems()
        {
            var queryFilterItems = new List<QueryFilterItem>();
            queryFilterItems.Add(new QueryFilterItem() { FieldName = "IsCustomer", FieldValue = true });
            return queryFilterItems;
        }

        [Invoke]
        public bool CheckIfVatUnique(string vatNumber, string customerId, string entityCountryId, int tenant)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant myTenant = tenantRepository.GetSingleTenant(tenant);
            bool isNewEntity = string.IsNullOrEmpty(customerId) ? true : false;
            bool isAlreadyExists = false;

            IQueryable<Card> allMatchedCards
                = (from a in addressRepository.context.Cards
                   where a.Tenant == tenant
                   && (a.PartnerTypeId == "CS" || a.PartnerTypeId == "PO")
                   && a.VatNumber == vatNumber
                   select a);

            if (allMatchedCards != null)
            {
                if (myTenant.VatUniqueTypeCode == "UFA")
                {
                    if (isNewEntity)
                    {
                        if (allMatchedCards.Count() > 0)
                        {
                            isAlreadyExists = true;
                        }
                    }

                    else
                    {
                        if (allMatchedCards.Where(d => d.Id != customerId).Any())
                        {
                            isAlreadyExists = true;
                        }
                    }
                }

                else if (myTenant.VatUniqueTypeCode == "USC")
                {
                    if (!isNewEntity)
                    {
                        allMatchedCards = allMatchedCards.Where(d => d.Id != customerId);
                    }

                    if (allMatchedCards != null)
                    {
                        if (!string.IsNullOrEmpty(entityCountryId))
                        {
                            if (entityCountryId == myTenant.VatUniqueCountryId)
                            {
                                foreach (Card item in allMatchedCards)
                                {
                                    Address address = addressRepository.GetMainAddressByCardId(item.Id, tenant);
                                    if (address != null)
                                    {
                                        if (address.CountryId == entityCountryId)
                                        {
                                            isAlreadyExists = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } 
            }

            return isAlreadyExists;
        }

        [Invoke]
        public DateTime InsertCustomerTraceEvent(CustomerPM entityPM, DateTime? eventDate, string note, string eventTypeId, string objectTableId, string userId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            string entityId = entityPM.Id;

            //EventTypeRepository eventTypeRep = new EventTypeRepository(tenant);
            //EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
            //WebFreightDomainService webFreightService = new WebFreightDomainService();

            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = note;
            newTraceEvent.ObjectTableId = objectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = userId;
            newTraceEvent.EventTypeId = eventTypeId;
            newTraceEvent.EventDateTime = eventDate != null ? eventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = entityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = true;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            traceEventRep.Add(newTraceEvent);
            traceEventRep.SubmitChanges();

            return newTraceEvent.LogDateTime;
        }

        [Invoke]
        public void DeleteCustomerTraceEvent(CustomerPM entityPM, string traceEventId, int tenant, bool external)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);            
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
        }

        public CustomerPM GetSingleCustomerPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

            customerQuery = new CustomerQuery(tenant);
            CustomerPM entityPM = customerQuery.GetSinglePM(id, tenant);

          //  CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
           // entityPM.IsCustomerAllowed = myFilter.IsCustomerAllowed(entityPM);

            return entityPM;

        }
    }
}