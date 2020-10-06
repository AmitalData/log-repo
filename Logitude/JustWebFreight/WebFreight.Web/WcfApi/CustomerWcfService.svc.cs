using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Validators;
using System.Transactions;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using WebFreight.Web.WebServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.SystemLogs;
using System.Web;
using System.Threading;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomerHypredService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomerHypredService.svc or CustomerHypredService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerWcfService : ICustomerWcfService
    {
        public Response Upsert(CustomerPM entityPM, bool batch)
        {

            //return new Response() { HasError = true, ErrorMessage = "not WOrk at 17r02 " } ;
            //throw new Exception("not WOrk at 17r02 ");
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Customer", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("Customer", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                    CustomerRepository customerRepository = new CustomerRepository(objectContext);
                    UserRepository userReporistory = new UserRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(objectContext);
                    CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(objectContext);
                    CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(objectContext);
                    CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(objectContext);
                    CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(objectContext);
                    TenantRepository tenantRepository = new TenantRepository(objectContext);
                    CountryRepository countryRepository = new CountryRepository(objectContext);
                    ContactRepository contactRepository = new ContactRepository(objectContext);
                    RankRepository rankRepository = new RankRepository(objectContext);
                    Tenant tenantEntity = tenantRepository.GetSingleTenantOnly(entityPM.Tenant);

                    CustomerService service = new CustomerService(objectContext, entityPM);

                    if (entityPM.CollectorId != null)
                    {
                        User user = userReporistory.GetSingleUserByCode(entityPM.CollectorId, entityPM.Tenant, true);
                        if (user != null)
                        {
                            entityPM.CollectorId = user.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CollectorId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    if (entityPM.ClassifierId != null)
                    {
                        User user = userReporistory.GetSingleUserByCode(entityPM.ClassifierId, entityPM.Tenant, true);
                        if (user != null)
                        {
                            entityPM.ClassifierId = user.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ClassifierId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.FreelancerId != null)
                    {
                        User user = userReporistory.GetSingleUserByCode(entityPM.FreelancerId, entityPM.Tenant, true);
                        if (user != null)
                        {
                            entityPM.FreelancerId = user.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "FreelancerId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.SalesmanUserId != null)
                    {
                        User user = userReporistory.GetSingleUserByCode(entityPM.SalesmanUserId, entityPM.Tenant, true);
                        if (user != null)
                        {
                            entityPM.SalesmanUserId = user.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SalesmanUserId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.AccountManagerUserId != null)
                    {
                        User user = userReporistory.GetSingleUserByCode(entityPM.AccountManagerUserId, entityPM.Tenant, true);
                        if (user != null)
                        {
                            entityPM.AccountManagerUserId = user.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "AccountManagerUserId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }



                    if (entityPM.ForwarderId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCodeForHybrid(entityPM.ForwarderId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.ForwarderId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ForwarderId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.CustomsAgentId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCodeForHybrid(entityPM.CustomsAgentId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.CustomsAgentId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CustomsAgentId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.MediatorId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCodeForHybrid(entityPM.MediatorId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.MediatorId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MediatorId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.PrimaryContactId != null)
                    {

                        Contact contact = contactRepository.GetSingleContactByExternalId(entityPM.PrimaryContactId, entityPM.Tenant);
                        if (contact != null && !string.IsNullOrEmpty(contact.ExternalId))
                        {
                            entityPM.PrimaryContactId = contact.Id;
                        }
                        else
                        {
                           // response.HasError = true;
                            //response.ErrorMessage = "PrimaryContactId field doesn't exist in the database,Upsert this entity before using it.";
                            //return response;
                            //response.HasError = true;
                            //response.ErrorMessage = "PrimaryContactId field doesn't exist in the database,Upsert this entity before using it.";
                            //return response;
                        }
                    }


                    if (entityPM.RankCode != null)
                    {

                        Rank rank = rankRepository.GetSingleRankByCode(entityPM.RankCode, entityPM.Tenant);
                        if (rank != null)
                        {
                            entityPM.RankId = rank.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "RankCode field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }



                    #region Customer salesman by product


                    foreach (CustomerSalesmanByProductPM salesman in entityPM.CustomerSalesmanByProducts)
                    {
                        salesman.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (salesman.SalesmanUserId != null)
                        {
                            User user = userReporistory.GetSingleUserByCode(salesman.SalesmanUserId, salesman.Tenant, true);
                            if (user != null)
                            {
                                salesman.SalesmanUserId = user.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "SalesmanUserId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "SalesmanUserId field is required";
                            return response;
                        }



                    }


                    bool clearCustomerSalesmanByProducts = false;
                    if (entityPM.CustomerSalesmanByProducts.Count() > 1)
                    {
                        if (entityPM.SalesmanUserId == null)
                        {

                            CustomerSalesmanByProductPM salesmanUser = entityPM.CustomerSalesmanByProducts.Where(d => d.SalesmanUserId != null).FirstOrDefault();
                            string salesManId = salesmanUser.SalesmanUserId;
                            bool sameUser = true;
                            foreach (CustomerSalesmanByProductPM salesman in entityPM.CustomerSalesmanByProducts)
                            {
                                if (salesman.SalesmanUserId != salesManId)
                                {
                                    sameUser = false;
                                }

                            }
                            if (sameUser)
                            {
                                entityPM.SalesmanUserId = salesManId;//user.Id;
                                clearCustomerSalesmanByProducts = true;

                            }
                        }

                        if (clearCustomerSalesmanByProducts)
                        {
                            entityPM.CustomerSalesmanByProducts.Clear();
                        }

                    }


                    #endregion

                    #region Customer account manager by product
                    foreach (CustomerAccountManagerByProductPM accountmanager in entityPM.CustomerAccountManagerByProducts)
                    {
                        accountmanager.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (accountmanager.AccountManagerId != null)
                        {
                            User user = userReporistory.GetSingleUserByCode(accountmanager.AccountManagerId, accountmanager.Tenant, true);
                            if (user != null)
                            {
                                accountmanager.AccountManagerId = user.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "AccountManagerId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "AccountManagerId field is required";
                            return response;
                        }



                    }
                    #endregion

                    #region Customer customs agent by product
                    foreach (CustomerCustomsAgentByProductPM customsagent in entityPM.CustomerCustomsAgentByProducts)
                    {
                        customsagent.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (customsagent.CustomsAgentId != null)
                        {
                            Card card = cardRepository.GetSingleCardByCodeForHybrid(customsagent.CustomsAgentId, customsagent.Tenant, false);
                            if (card != null)
                            {
                                customsagent.CustomsAgentId = card.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "CustomsAgentId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CustomsAgentId field is required";
                            return response;
                        }



                    }
                    #endregion

                    #region Customer forwarder by product
                    foreach (CustomerForwarderByProductPM forwarder in entityPM.CustomerForwarderByProducts)
                    {
                        forwarder.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (forwarder.ForwarderId != null)
                        {
                            Card card = cardRepository.GetSingleCardByCodeForHybrid(forwarder.ForwarderId, forwarder.Tenant, false);
                            if (card != null)
                            {
                                forwarder.ForwarderId = card.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "ForwarderId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ForwarderId field is required";
                            return response;
                        }



                    }
                    #endregion

                    #region Customer madiator by product
                    foreach (CustomerMediatorByProductPM mediator in entityPM.CustomerMediatorByProducts)
                    {
                        mediator.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (mediator.MediatorId != null)
                        {
                            Card card = cardRepository.GetSingleCardByCodeForHybrid(mediator.MediatorId, mediator.Tenant, false);
                            if (card != null)
                            {
                                mediator.MediatorId = card.Id;
                            }
                            else
                            {
                                response.HasError = true;
                                response.ErrorMessage = "MediatorId field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "MediatorId field is required";
                            return response;
                        }



                    }
                    #endregion


                    entityPM.IsHybrid = true;
                    Customer entity = customerRepository.GetSingleCustomerByCodeForHybrid(entityPM.Code, entityPM.Tenant, false);

                    if (entity == null && !string.IsNullOrEmpty(entityPM.VatNumber))
                    {
                        entity = GetCustomerByVatNumber(entityPM, customerRepository, countryRepository, tenantEntity);
                    }

                    if (entity == null)
                    {
                        service.SetChangeSet(new List<CustomerSalesNotePM>(), new List<CustomerProductPM>(), new List<CustomerCompetitorPM>(), new List<CustomerAdditionalServicePM>(), entityPM.CustomerSalesmanByProducts, entityPM.CustomerAccountManagerByProducts, entityPM.CustomerCustomsAgentByProducts, entityPM.CustomerForwarderByProducts, entityPM.CustomerMediatorByProducts, entityPM.CardExternalCodeByCurrencies);
                        service.Create();
                    }
                    else
                    {
                        entityPM.Id = entity.Id;
                        bool exist = SecurityUtility.CheckFeature("Customer", "EDITCREDITAMOUNT", entity.Tenant);
                        if (exist)
                        {
                            entityPM.CreditLimitAmount = entity.CreditLimitAmount;  
                        }


                        if (LogitudeSettings.WorkEnvironment == "cloud" && (entity.LogBoxActivated != entityPM.LogBoxActivated))
                        {
                            response.HasError = true;
                            response.ErrorMessage = "Sorry you can't update LogBoxActivated field";
                            return response;
                        }


                        CustomerFieldsUpdateSettingQuery customerFieldsUpdateSettingQuery = new CustomerFieldsUpdateSettingQuery(entityPM.Tenant);
                        List<CustomerFieldsUpdateSettingPM> settings = customerFieldsUpdateSettingQuery.GetCustomerFieldsUpdateSettingPMsByTenant(entityPM.Tenant).ToList();
                        var salesManSettings = settings.FirstOrDefault(s => s.ObjectFieldName == "SalesmanUserId");
                        if (salesManSettings != null && salesManSettings.UpdateDirection == "NOUP")
                        {
                            entityPM.SalesmanUserId = entity.SalesmanUserId;
                            entityPM.CustomerSalesmanByProducts = new List<CustomerSalesmanByProductPM>();
                        }
                        else
                        {
                            CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                            List<CustomerSalesmanByProductPM> CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);
                            foreach (CustomerSalesmanByProductPM salesman in CustomerSalesmanByProducts)
                            {
                                salesman.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                entityPM.CustomerSalesmanByProducts.Add(salesman);


                            }
                        }

                        
                       

                        CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                        List<CustomerAccountManagerByProductPM> CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                        foreach (CustomerAccountManagerByProductPM accountmanager in CustomerAccountManagerByProducts)
                        {
                            accountmanager.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.CustomerAccountManagerByProducts.Add(accountmanager);
                        }

                        CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                        List<CustomerCustomsAgentByProductPM> CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                        foreach (CustomerCustomsAgentByProductPM customsagent in CustomerCustomsAgentByProducts)
                        {
                            customsagent.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.CustomerCustomsAgentByProducts.Add(customsagent);

                        }

                        CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                        List<CustomerForwarderByProductPM> CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                        foreach (CustomerForwarderByProductPM forwarder in CustomerForwarderByProducts)
                        {
                            forwarder.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.CustomerForwarderByProducts.Add(forwarder);

                        }

                        CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);
                        List<CustomerMediatorByProductPM> customerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);
                        foreach (CustomerMediatorByProductPM madiator in customerMediatorByProducts)
                        {
                            madiator.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.CustomerMediatorByProducts.Add(madiator);

                        }

                        service.SetChangeSet(new List<CustomerSalesNotePM>(), new List<CustomerProductPM>(), new List<CustomerCompetitorPM>(), new List<CustomerAdditionalServicePM>(), entityPM.CustomerSalesmanByProducts, entityPM.CustomerAccountManagerByProducts, entityPM.CustomerCustomsAgentByProducts, entityPM.CustomerForwarderByProducts, entityPM.CustomerMediatorByProducts, entityPM.CardExternalCodeByCurrencies);
                        service.Update();
                    }



                    response.Result = entityPM.Id;
                    scope.Complete();
                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (ApplicationException ex)
            {
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

            

        }

        private static Customer GetCustomerByVatNumber(CustomerPM entityPM, CustomerRepository customerRepository, CountryRepository countryRepository, Tenant tenantEntity)
        {
            Customer entity = null;
            if (tenantEntity.VatUniqueTypeCode == "UFA")
            {
                entity = GetCustomerByVatUniquePartnerType(entityPM, customerRepository, tenantEntity);
            }
            if (tenantEntity.VatUniqueTypeCode == "USC")
            {
                Country customerCountry = countryRepository.GetSingleCountryByCode(entityPM.CountryCode, entityPM.Tenant);
                if (customerCountry != null && tenantEntity.VatUniqueCountryId == customerCountry.Id)
                {
                    entity = GetCustomerByVatUniquePartnerType(entityPM, customerRepository, tenantEntity);
                    //entity = customerRepository.GetSingleCustomerByVatForHybrid(entityPM.VatNumber, entityPM.Tenant, false);
                    if (entity != null && (entity.Card.Code != entityPM.Code))
                    {
                        throw new ApplicationException("A customer with the same vat and different code already exists.");
                        //response.HasError = true;
                        //response.ErrorMessage = "A customer with the same vat and different code already exists.";
                        //return response;
                    }
                }
            }

            return entity;
        }

        private static Customer GetCustomerByVatUniquePartnerType(CustomerPM entityPM, CustomerRepository customerRepository, Tenant tenantEntity)
        {
            Customer entity = null;
            bool isPotentialCustomer = (entityPM.CustomerStatusCode == "POT" || entityPM.CustomerStatusCode == "WAC" || entityPM.SetReady);
            if (tenantEntity.VatUniquePartnerTypeCode == "ALL" || string.IsNullOrEmpty(tenantEntity.VatUniquePartnerTypeCode))
            {
                if (isPotentialCustomer)
                {
                    entity = customerRepository.GetSingleCustomerByVatForHybrid(entityPM.VatNumber, entityPM.Tenant, false);
                }
                else
                {
                    entity = customerRepository.GetSingleCustomerByVatAndStatusForHybrid(entityPM.VatNumber, "ACT", entityPM.Tenant, false);
                }
            }
            else if (tenantEntity.VatUniquePartnerTypeCode == "CS" && (entityPM.CustomerStatusCode == "ACT" || entityPM.SetActivated))
            {
                entity = customerRepository.GetSingleCustomerByVatAndStatusForHybrid(entityPM.VatNumber, "ACT", entityPM.Tenant, false);
            }
            else if (tenantEntity.VatUniquePartnerTypeCode == "POT" && isPotentialCustomer)
            {
                entity = customerRepository.GetSingleCustomerByVatForHybrid(entityPM.VatNumber, entityPM.Tenant, false);

            }

            return entity;
        }

        public List<CustomerList> GetCustomerList(string searchText, string email, bool myCustomer, int tenant, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ

                List<CustomerList> result = new List<CustomerList>();
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commoncontext);
                CardContactRepository cardContactRepository = new CardContactRepository(commoncontext);
                CustomerRepository customerRepository = new CustomerRepository(commoncontext);
                IQueryable<CustomersDataView> customers = customerRepository.GetCustomersDataViews(tenant);

                if (!string.IsNullOrEmpty(searchText))
                {
                    customers = customers.Where(c => c.SearchFields.Contains(searchText));
                }
                if (myCustomer)
                {
                    Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
                    if (contact != null)
                    {
                        customers = customers.Where(c => c.SalesmanUserId == contact.Id || c.AccountManagerUserId == contact.Id);
                    }
                }

                customers = customers.Where(c => c.IsCustomer).OrderBy(c => c.EnglishName).Skip(skip).Take(take);
                CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                result = customerQuery.GetIQueryableEntityList(customers).ToList();

                return result;

            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }


        }


        public List<CustomerList> GetCustomerListByEmail(string email, int tenant, ref Response response)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                List<CustomerList> result = new List<CustomerList>();
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commoncontext);
                CardContactRepository cardContactRepository = new CardContactRepository(commoncontext);

                if (!string.IsNullOrEmpty(email))
                {
                    Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
                    if (contact != null)
                    {
                        List<Card> customercards = cardContactRepository.GeContactCustomersByContact(contact.Id, null, tenant);
                        foreach (Card card in customercards)
                        {
                            if (!result.Where(c => c.Id == card.Id && c.Tenant == card.Tenant).Any())
                            {
                                CustomerList customerList = (from customer in commoncontext.Customers.Include("Rank")//.Include("AccountManagerUser.Contact").Include("SalesmanUser.Contact").Include("Card.SharedLogisticsInvitationStatus")
															 where customer.Tenant == tenant && customer.Id == card.Id
                                                             select new CustomerList()
                                                             {
                                                                 Code = customer.Card.Code,
                                                                 EnglishName = customer.Card.EnglishName,
                                                                 LocalName = customer.Card.LocalName,
                                                                 ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                                                 PayablesAccountingCard = card.PayablesAccountingCard,
                                                                 InActive = customer.Card.InActive,
                                                                 Notes = customer.Card.Notes,
                                                                 BillToId = customer.BillToId,
                                                                 Website = customer.Card.Website,
                                                                 SalesmanUserId = customer.SalesmanUserId,
                                                                 Id = customer.Id,
                                                                 PaymentTermId = customer.Card.PaymentTermId,
                                                                 CreateDate = customer.Card.CreateDate,
                                                                 Tenant = customer.Tenant,
                                                                 VatNumber = customer.Card.VatNumber,
                                                                 SearchFields = customer.Card.SearchFields,
                                                                 PaymentTermEnglishName = customer.Card.PaymentTerm != null ? customer.Card.PaymentTerm.EnglishName : null,
                                                                 InvoiceCurrencyId = customer.Card.InvoiceCurrencyId,
                                                                 LastShipmentDate = customer.LastShipmentDate,
                                                                 StartWorkingDate = customer.StartWorkingDate,
                                                                 StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                                                 //AccountManagerUserEnglishName = customer.AccountManagerUser == null ? null : (customer.AccountManagerUser.Contact == null ? null : customer.AccountManagerUser.Contact.EnglishName),
                                                                 //SalesmanUserEnglishName = customer.SalesmanUser == null ? null : (customer.SalesmanUser.Contact == null ? null : customer.SalesmanUser.Contact.EnglishName),
                                                                 CityName = customer.Card.CityName,
                                                                 VatTypeId = customer.Card.VatTypeId,
                                                                 BillToName = customer.BillToCard.EnglishName,
                                                                 Field1 = customer.Field1,
                                                                 Field2 = customer.Field2,
                                                                 Field3 = customer.Field3,
                                                                 Field4 = customer.Field4,
                                                                 Field5 = customer.Field5,
                                                                 Field6 = customer.Field6,
                                                                 Field7 = customer.Field7,
                                                                 Field8 = customer.Field8,
                                                                 Field9 = customer.Field9,
                                                                 Field10 = customer.Field10,
                                                                 RankCode = customer.Rank.Code,
                                                                 RankName = customer.Rank.Name,
                                                                 //SharedLogisticsInvitationStatusName = customer.Card.SharedLogisticsInvitationStatus != null ? customer.Card.SharedLogisticsInvitationStatus.Name : null,
                                                                 LastLoginDate = customer.Card.LastLoginDate,
                                                                 InvitationDate = customer.Card.InvitationDate,
                                                             }).FirstOrDefault();


                                result.Add(customerList);
                            }
                        }
                    }
                }



                return result;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        public CustomerList GetCustomerListById(string id, int tenant, ref Response response)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                CustomerQuery query = new CustomerQuery(tenant);
                CustomerList list = query.GetSingleCustomerList(id, tenant);

                return list;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        public CustomerPM GetCustomerPM(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {
           // return null;
            //throw new Exception("not WOrk at 17r02 ");
            try
            {
                CustomerPM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                UserRepository userReporistory = new UserRepository(objectContext);
                CardRepository cardRepository = new CardRepository(objectContext);
                ContactRepository contactRepository = new ContactRepository(objectContext);
                CountryRepository countryRepository = new CountryRepository(objectContext);
                RankRepository rankRepository = new RankRepository(objectContext);
                CustomerQuery query = new CustomerQuery(tenant);
                if (filters.ById)
                {
                    entityPM = query.GetSinglePMForHybrid(filters.SearchCode, tenant);
                }
                if (filters.ByCode)
                {
                    entityPM = query.GetSinglePMByCodeForHybrid(filters.SearchCode, tenant, false);
                }
                if (filters.ByVatNumber)
                {
                    entityPM = query.GetSinglePMByVatNumberForHybrid(filters.SearchCode, tenant, false);
                }

                if (entityPM != null)
                {
                    if (entityPM.CollectorId != null)
                    {
                        User user = userReporistory.GetSingleUser(entityPM.CollectorId, entityPM.Tenant, false);
                        if (user != null)
                        {
                            entityPM.CollectorId = user.Code;
                            entityPM.CollectorName = user.Contact != null ? user.Contact.EnglishName : null;
                        }
                    }


                    if (entityPM.ClassifierId != null)
                    {
                        User user = userReporistory.GetSingleUser(entityPM.ClassifierId, entityPM.Tenant, false);
                        if (user != null)
                        {
                            entityPM.ClassifierId = user.Code;
                            entityPM.ClassifierName = (user.Contact != null ? user.Contact.EnglishName : null);
                        }
                    }

                    if (entityPM.FreelancerId != null)
                    {
                        User user = userReporistory.GetSingleUser(entityPM.FreelancerId, entityPM.Tenant, false);
                        if (user != null)
                        {
                            entityPM.FreelancerId = user.Code;
                            entityPM.FreelancerName = (user.Contact != null ? user.Contact.EnglishName : null);
                        }

                    }

                    if (entityPM.SalesmanUserId != null)
                    {
                        User user = userReporistory.GetSingleUser(entityPM.SalesmanUserId, entityPM.Tenant, false);
                        if (user != null)
                        {
                            entityPM.SalesmanUserId = user.Code;
                            entityPM.SalesmanUserEnglishName = (user.Contact != null ? user.Contact.EnglishName : null);
                        }

                    }

                    if (entityPM.AccountManagerUserId != null)
                    {
                        User user = userReporistory.GetSingleUser(entityPM.AccountManagerUserId, entityPM.Tenant, false);
                        if (user != null)
                        {
                            entityPM.AccountManagerUserId = user.Code;
                            entityPM.AccountManagerUserEnglishName = (user.Contact != null ? user.Contact.EnglishName : null);
                        }

                    }

                    if (entityPM.ForwarderId != null)
                    {
                        Card card = cardRepository.GetSingleCard(entityPM.ForwarderId, entityPM.Tenant);
                        if (card != null)
                        {
                            entityPM.ForwarderId = card.Code;
                            entityPM.ForwarderName = card.EnglishName;
                        }

                    }

                    if (entityPM.CustomsAgentId != null)
                    {
                        Card card = cardRepository.GetSingleCard(entityPM.CustomsAgentId, entityPM.Tenant);
                        if (card != null)
                        {
                            entityPM.CustomsAgentId = card.Code;
                            entityPM.CustomsAgentName = card.EnglishName;
                        }
                    }

                    if (entityPM.MediatorId != null)
                    {
                        Card card = cardRepository.GetSingleCard(entityPM.MediatorId, entityPM.Tenant);
                        if (card != null)
                        {
                            entityPM.MediatorId = card.Code;
                            entityPM.MediatorName = card.EnglishName;
                        }
                    }

                    if (entityPM.PrimaryContactId != null)
                    {
                        Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                        if (contact != null && !string.IsNullOrEmpty(contact.ExternalId))
                        {
                            entityPM.PrimaryContactId = contact.ExternalId;
                            entityPM.PrimaryContactEmail = contact.Email;
                            entityPM.PrimaryContactName = contact.EnglishName;
                            entityPM.PrimaryContactPhone = contact.BusinessPhone;

                        }
                    }

                    if (entityPM.RankId != null)
                    {
                        Rank rank = rankRepository.GetSingleRank(entityPM.RankId, entityPM.Tenant);
                        if (rank != null)
                        {
                            entityPM.RankCode = rank.Code;
                            entityPM.RankName = rank.Name;
                        }
                    }

                    foreach (CustomerSalesmanByProductPM customerSalesManByProduct in entityPM.CustomerSalesmanByProducts)
                    {

                        if (customerSalesManByProduct.SalesmanUserId != null)
                        {
                            User user = userReporistory.GetSingleUser(customerSalesManByProduct.SalesmanUserId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                customerSalesManByProduct.SalesmanUserId = user.Code;
                                customerSalesManByProduct.SalesmanUserName = (user.Contact != null ? user.Contact.EnglishName : null);
                            }

                        }
                    }

                    foreach (CustomerAccountManagerByProductPM accountmanager in entityPM.CustomerAccountManagerByProducts)
                    {
                        if (accountmanager.AccountManagerId != null)
                        {
                            User user = userReporistory.GetSingleUser(accountmanager.AccountManagerId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                accountmanager.AccountManagerId = user.Code;
                                accountmanager.AccountManagerName = (user.Contact != null ? user.Contact.EnglishName : null);
                            }

                        }
                    }

                    foreach (CustomerCustomsAgentByProductPM customsagent in entityPM.CustomerCustomsAgentByProducts)
                    {
                        if (customsagent.CustomsAgentId != null)
                        {
                            Card card = cardRepository.GetSingleCard(customsagent.CustomsAgentId, entityPM.Tenant);
                            if (card != null)
                            {
                                customsagent.CustomsAgentId = card.Code;
                                customsagent.CustomsAgentName = card.EnglishName;
                            }
                        }
                    }

                    foreach (CustomerForwarderByProductPM forwarder in entityPM.CustomerForwarderByProducts)
                    {
                        if (forwarder.ForwarderId != null)
                        {
                            Card card = cardRepository.GetSingleCard(forwarder.ForwarderId, entityPM.Tenant);
                            if (card != null)
                            {
                                forwarder.ForwarderId = card.Code;
                                forwarder.ForwarderName = card.EnglishName;
                            }
                        }
                    }
                    foreach (CustomerMediatorByProductPM mediator in entityPM.CustomerMediatorByProducts)
                    {
                        if (mediator.MediatorId != null)
                        {
                            Card card = cardRepository.GetSingleCard(mediator.MediatorId, entityPM.Tenant);
                            if (card != null)
                            {
                                mediator.MediatorId = card.Code;
                                mediator.MediatorName = card.EnglishName;
                            }
                        }
                    }

                    TenantRepository tenantRepository = new TenantRepository(objectContext);
                    ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                    Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);
                    if (!string.IsNullOrEmpty(currentTenant.DefaultQuestionnaireId))
                    {
                        ObjectTable table = objecttableRepository.GetObjectTableByName("Customer", 0, false);
                        entityPM.QuestionnaireAnsewrsHtmlString = GetActivationQuestionnaireAnswers(currentTenant.DefaultQuestionnaireId, tenant, table.Id, entityPM.Id, entityPM.EnglishName);
                    }

                }

                return entityPM;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        public List<AddressPM> GetCustomerAddresses(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {

            //return new List<AddressPM>();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                List<AddressPM> addresses = new List<AddressPM>();

                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                AddressQuery addressQuery = new AddressQuery(new AddressRepository(commoncontext));
                CustomerPM pm = null;
                CustomerQuery query = new CustomerQuery(tenant);
                if (filters.ById)
                {
                    pm = query.GetSinglePMForHybrid(filters.SearchCode, tenant);
                }
                if (filters.ByCode)
                {
                    pm = query.GetSinglePMByCodeForHybrid(filters.SearchCode, tenant, false);
                }
                if (filters.ByVatNumber)
                {
                    pm = query.GetSinglePMByVatNumberForHybrid(filters.SearchCode, tenant, false);
                }

                if (pm != null)
                {
                    addresses = addressQuery.GetAddressesByCardId(pm.Id, tenant);
                }

                return addresses;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        public List<ContactPM> GetCustomerContacts(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                List<ContactPM> contacts = new List<ContactPM>();

                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                ContactQuery contactQuery = new ContactQuery(new ContactRepository(commoncontext));
                CustomerPM pm = null;
                CustomerQuery query = new CustomerQuery(tenant);
                if (filters.ById)
                {
                    pm = query.GetSinglePM(filters.SearchCode, tenant);
                }
                if (filters.ByCode)
                {
                    pm = query.GetSinglePMByCodeForHybrid(filters.SearchCode, tenant, false);
                }
                if (filters.ByVatNumber)
                {
                    pm = query.GetSinglePMByVatNumberForHybrid(filters.SearchCode, tenant, false);
                }

                if (pm != null)
                {
                    contacts = contactQuery.GetContactsbyCardId(pm.Id, tenant).ToList();
                }

                return contacts;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        //public byte[] GetQuestionnaireAnswers(string QuestionnaireId, int tenant, string tableId, string entityId, ref Response response)
        //{
        //   // QuestionnaireId = "1-104";
        //   // tenant = 9;
        //    StringBuilder HtmlTemplate = new StringBuilder();
        //    ICRMContext context = CRMContext.GetContext(tenant);
        //    QuestionnaireQueryService questionnaireQuery = new QuestionnaireQueryService(context);
        //    QuestionnairePM entityPM = questionnaireQuery.GetSingle(QuestionnaireId, true, false);

        //    QuestionnaireAnswerQueryService questionnaireAnswerQuery = new QuestionnaireAnswerQueryService(context);
        //    QuestionnaireAnswerPM QuestionnaireAnswerPM = questionnaireAnswerQuery.GetQuestionnaireAnswerPMByquestioneerIdAndversion(entityPM.Id , tenant, tableId, entityId);

        //    if (QuestionnaireAnswerPM.QuestionnaireAnswerLines != null)
        //    {



        //        foreach (QuestionnaireQuestionPM question in entityPM.QuestionnaireQuestions)
        //        {

        //            if (entityPM.RightToLeft)
        //            {
        //                HtmlTemplate.Append("<p  dir='rtl' style='font-weight:bold;height:24px; font-size:16px;'>" + question.Question + "</p>");
        //                string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == question.QuestionNumber).Max(d => d.AnswerValue);
        //                HtmlTemplate.Append("<p dir='rtl' style='font-weight:normal;height:20px; font-size:15px;margin-top:-17px'>" + anser + "</p>");
        //            }
        //            else
        //            {

        //            HtmlTemplate.Append("<p  style='font-weight:bold;height:24px; font-size:16px;'>" + question.Question + "</p>");
        //            string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == question.QuestionNumber).Max(d => d.AnswerValue);
        //            HtmlTemplate.Append("<p style='font-weight:normal;height:20px; font-size:15px;margin-top:-17px'>" + anser + "</p>");
        //            }

        //        }



        //    }
        //    return Encoding.UTF8.GetBytes(HtmlTemplate.ToString());
        //}


        public CustomerPM GetReadyForActivationCustomer(int tenant, ref Response response)
        {
            try
            {
                CustomerPM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);

                QueueClient client = ServiceBusQueueHelper.CreateCustomerQueue(tenant);

                var message = client.Receive(new TimeSpan(0, 0, 20));
                if (message != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    
                    AzureLog.SaveLogsInStorage("Message retreived from activation queue (Tenant:" + tenant + ")", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);

                    if (message.Properties["CustomerId"] != null)
                    {
                        string customerIdParam = message.Properties["CustomerId"].ToString();
                        int tenantParam = int.Parse(message.Properties["Tenant"].ToString());


                        AzureLog.SaveLogsInStorage("Customer retreived from activation queue (Id:" + customerIdParam + " ,Tenant:" + tenant + ",Token:" + message.LockToken + ",DeliveryCount:" + message.DeliveryCount + ")", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);


                        ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                        CustomerQuery customerQuery = new CustomerQuery(new CustomerRepository(commoncontext));
                        TenantRepository tenantRepository = new TenantRepository(commoncontext);
                        ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);

                        ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                        UserRepository userReporistory = new UserRepository(objectContext);
                        CardRepository cardRepository = new CardRepository(objectContext);

                        CountryRepository countryRepository = new CountryRepository(objectContext);

                        entityPM = customerQuery.GetSinglePM(customerIdParam, tenantParam);

                        if (entityPM.CustomerStatusCode == "ACT")
                        {
                            AzureLog.SaveLogsInStorage("Message removed from activation queue (the customer is active)", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                            message.Complete();
                            return null;
                        }
                        else if (entityPM.CustomerStatusCode != "WAC")
                        {
                            if (message.DeliveryCount <= 5)
                            {
                                int count = 0;
                                while (count < 3)
                                {
                                    Thread.Sleep(50);
                                    entityPM = customerQuery.GetSinglePM(customerIdParam, tenantParam);
                                    if (entityPM.CustomerStatusCode == "WAC")
                                    {
                                        break;
                                    }

                                    count++;
                                }

                                if (entityPM.CustomerStatusCode != "WAC")
                                {
                                    AzureLog.SaveLogsInStorage("Message abandoned from activation queue (entityPM.CustomerStatusCode != WAC)", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                                    message.Abandon();
                                    return null;
                                }
                            }
                            else
                            {
                                AzureLog.SaveLogsInStorage("Message removed from activation queue (entityPM.CustomerStatusCode != WAC)", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                                message.Complete();
                                return null;
                            }
                        }

                        entityPM.QueueMessageLockToken = message.LockToken;
                        Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);
                        if (!string.IsNullOrEmpty(currentTenant.DefaultQuestionnaireId))
                        {
                            ObjectTable table = objecttableRepository.GetObjectTableByName("Customer", 0, false);
                            entityPM.QuestionnaireAnsewrsHtmlString = GetActivationQuestionnaireAnswers(currentTenant.DefaultQuestionnaireId, tenant, table.Id, entityPM.Id, entityPM.EnglishName);
                        }

                        if (entityPM.UpdatedByUserId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.UpdatedByUserId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.UpdatedByUserCode = user.Code;
                            }
                        }


                        if (entityPM.CollectorId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.CollectorId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.CollectorId = user.Code;
                            }
                        }


                        if (entityPM.ClassifierId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.ClassifierId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.ClassifierId = user.Code;
                            }
                        }

                        if (entityPM.FreelancerId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.FreelancerId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.FreelancerId = user.Code;
                            }

                        }

                        if (entityPM.SalesmanUserId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.SalesmanUserId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.SalesmanUserId = user.Code;
                            }

                        }

                        if (entityPM.AccountManagerUserId != null)
                        {
                            User user = userReporistory.GetSingleUser(entityPM.AccountManagerUserId, entityPM.Tenant, false);
                            if (user != null)
                            {
                                entityPM.AccountManagerUserId = user.Code;
                            }

                        }

                        if (entityPM.ForwarderId != null)
                        {
                            Card card = cardRepository.GetSingleCard(entityPM.ForwarderId, entityPM.Tenant);
                            if (card != null)
                            {
                                entityPM.ForwarderId = card.Code;
                            }

                        }

                        if (entityPM.CustomsAgentId != null)
                        {
                            Card card = cardRepository.GetSingleCard(entityPM.CustomsAgentId, entityPM.Tenant);
                            if (card != null)
                            {
                                entityPM.CustomsAgentId = card.Code;
                            }
                        }

                        if (entityPM.MediatorId != null)
                        {
                            Card card = cardRepository.GetSingleCard(entityPM.MediatorId, entityPM.Tenant);
                            if (card != null)
                            {
                                entityPM.MediatorId = card.Code;
                            }
                        }

                        if (entityPM.PrimaryContactId != null)
                        {
                            Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                            if (contact != null && !string.IsNullOrEmpty(contact.ExternalId))
                            {
                                entityPM.PrimaryContactId = contact.ExternalId;
                            }
                        }

                        foreach (CustomerSalesmanByProductPM customerSalesManByProduct in entityPM.CustomerSalesmanByProducts)
                        {

                            if (customerSalesManByProduct.SalesmanUserId != null)
                            {
                                User user = userReporistory.GetSingleUser(customerSalesManByProduct.SalesmanUserId, entityPM.Tenant, false);
                                if (user != null)
                                {
                                    customerSalesManByProduct.SalesmanUserId = user.Code;
                                }

                            }
                        }

                        foreach (CustomerAccountManagerByProductPM accountmanager in entityPM.CustomerAccountManagerByProducts)
                        {
                            if (accountmanager.AccountManagerId != null)
                            {
                                User user = userReporistory.GetSingleUser(accountmanager.AccountManagerId, entityPM.Tenant, false);
                                if (user != null)
                                {
                                    accountmanager.AccountManagerId = user.Code;
                                }

                            }
                        }

                        foreach (CustomerCustomsAgentByProductPM customsagent in entityPM.CustomerCustomsAgentByProducts)
                        {
                            if (customsagent.CustomsAgentId != null)
                            {
                                Card card = cardRepository.GetSingleCard(customsagent.CustomsAgentId, entityPM.Tenant);
                                if (card != null)
                                {
                                    customsagent.CustomsAgentId = card.Code;
                                }
                            }
                        }

                        foreach (CustomerForwarderByProductPM forwarder in entityPM.CustomerForwarderByProducts)
                        {
                            if (forwarder.ForwarderId != null)
                            {
                                Card card = cardRepository.GetSingleCard(forwarder.ForwarderId, entityPM.Tenant);
                                if (card != null)
                                {
                                    forwarder.ForwarderId = card.Code;
                                }
                            }
                        }
                        foreach (CustomerMediatorByProductPM mediator in entityPM.CustomerMediatorByProducts)
                        {
                            if (mediator.MediatorId != null)
                            {
                                Card card = cardRepository.GetSingleCard(mediator.MediatorId, entityPM.Tenant);
                                if (card != null)
                                {
                                    mediator.MediatorId = card.Code;
                                }
                            }
                        }


                    }
                    else
                    {
                        AzureLog.SaveLogsInStorage("Message removed from activation queue CustomerId is null", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);

                        message.Complete();
                    }


                }


                return entityPM;

            }
            catch (Exception ex)
            {
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                }

                AzureLog.SaveLogsInStorage("Error while getting customer from activation queue (Tenant:" + tenant + ")", "E", DateTime.Now, ex.Message, ex.StackTrace, 0, loggedContact.Id, loggedContact.EnglishName,currentIP );

                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }


        public Response RemoveFromCustomersQueue(Guid queueMessageLockToken, int tenant)
        {
            Response response = new Response();
            try
            {

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                QueueClient client = ServiceBusQueueHelper.CreateCustomerQueue(tenant);


                client.Complete(queueMessageLockToken);

                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                }

                AzureLog.SaveLogsInStorage("Message removed from activation queue (Token:" + queueMessageLockToken + " ,Tenant:" + tenant + ")", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }

            return response;
        }



        public string GetActivationQuestionnaireAnswers(string QuestionnaireId, int tenant, string tableId, string entityId,string customername)
        {
            StringBuilder HtmlTemplate = new StringBuilder();
            ICRMContext context = CRMContext.GetContext(tenant);
            QuestionnaireQueryService questionnaireQuery = new QuestionnaireQueryService(context);

            QuestionnairePM entityPM = questionnaireQuery.GetSingle(QuestionnaireId, false, false);

            QuestionnaireAnswerQueryService questionnaireAnswerQuery = new QuestionnaireAnswerQueryService(context);
            QuestionnaireAnswerPM QuestionnaireAnswerPM = questionnaireAnswerQuery.GetQuestionnaireAnswerPMByquestioneerIdAndversion(QuestionnaireId, tenant, tableId, entityId);


            if (QuestionnaireAnswerPM != null)
            {
                QuestionnaireQuestionQueryService questionsQuery = new QuestionnaireQuestionQueryService(context);
                List<QuestionnaireQuestionPM> QuestionnaireQuestions = questionsQuery.GetQuestionnaireQuestionByQuestioneerVersion(QuestionnaireId, QuestionnaireAnswerPM.VersionNumber, tenant);


                if (QuestionnaireAnswerPM.QuestionnaireAnswerLines != null)
                {
                    //lang="ar"



                    HtmlTemplate.Append("<!DOCTYPE html>");
                    HtmlTemplate.Append("<html  lang='ar'>");
                    HtmlTemplate.Append("<head>");
                    HtmlTemplate.Append("<title></title>");
                    HtmlTemplate.Append("<meta charset='utf-8'>");
                    HtmlTemplate.Append("</head>");
                    HtmlTemplate.Append("<body>");


                    if (!QuestionnaireAnswerPM.HasTwoColumn)
                    {


                        if (entityPM.RightToLeft)  HtmlTemplate.Append("<div  dir='rtl'  style ='margin-left:2%; margin-right:2%;'>");

                        else HtmlTemplate.Append("<div style ='margin-left:2%; margin-right:2%;'>");
                    
                        HtmlTemplate.Append("<div style ='width:100%'>");

                        HtmlTemplate.Append("<Div  style='font-weight:bold;margin-top:10px;margin-bottom:20px;height:auto;display:block;font-size:20px;'" + " width='auto%' " + ">"); HtmlTemplate.Append("Customer : " + customername + "</Div>");

                        foreach (QuestionnaireQuestionPM question in QuestionnaireQuestions)
                        {
                            if (question.QuestionTypeCode == "HL")
                            {
                                HtmlTemplate.Append("<p  style='font-weight:bold;height:auto;display:block;background-color:lightslategray;font-size:20px;'>" + question.Question + "</p>");
                            }
                            else
                            {
                                HtmlTemplate.Append("<p  style='font-weight:bold;height:24px; font-size:18px;'>" + question.Question + "</p>");
                            }

                            string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == question.QuestionNumber).Max(d => d.AnswerValue);


                            if (question.QuestionTypeCode != "HL")
                            {
                                HtmlTemplate.Append("<p style='font-weight:normal;height:20px; font-size:16px;margin-top:-10px;'>" + anser + "</p>");
                            }

                        }
                        HtmlTemplate.Append("</div>");
                    }

                    else
                    {
                        //HasTwoColumn

                        if (entityPM.RightToLeft)  HtmlTemplate.Append("<div  dir='rtl'  style ='margin-left:2%; margin-right:2%;'>");
                
                        else  HtmlTemplate.Append("<div style ='margin-left:2%; margin-right:2%;'>");
                   
                        HtmlTemplate.Append("<Div  style='font-weight:bold;margin-top:10px;margin-bottom:20px;height:auto;display:block;font-size:20px;'" + " width='auto%' " + ">"); HtmlTemplate.Append("Customer : " + customername + "</Div>");


                        HtmlTemplate.Append("<table  width='100%'>");
                        List<QuestionnaireQuestionPM> QuestionnaireQuestionsList1 = new List<QuestionnaireQuestionPM>(QuestionnaireQuestions);
                        List<QuestionnaireQuestionPM> QuestionnaireQuestionsList2 = new List<QuestionnaireQuestionPM>();
                        foreach (QuestionnaireQuestionPM question in QuestionnaireQuestions)
                        {

                            QuestionnaireQuestionPM questionPM = QuestionnaireQuestionsList2.Where(d => d.QuestionNumber == question.QuestionNumber).Max();

                            if (questionPM == null)
                            {
                                if (question.QuestionTypeCode == "HL")
                                {
                                    HtmlTemplate.Append("<tr  style='font-weight:bold;height:auto;display:block;background-color:lightslategray; font-size:20px;margin-bottom:15px;margin-top:5px;'>" + "<td>" + "<div>" + question.Question + "</div>" + "</td>" + "</tr>");
                                    QuestionnaireQuestionsList1.Remove(question);
                                    QuestionnaireQuestionsList2.Remove(question);
                                }

                                else
                                {
                                    HtmlTemplate.Append("<tr style ='display:block;width:auto;'>");
                                    for (int i = 0; QuestionnaireQuestionsList1.Count > i && QuestionnaireQuestionsList1[i].QuestionTypeCode != "HL" && i != 2; i++)
                                    {
                                        HtmlTemplate.Append("<td style ='display:inline-block;width:auto;'>");

                                        HtmlTemplate.Append("<div  style='font-weight:bold;height:auto;display:block;max-width:900px; font-size:18px;word-wrap: break-word'>" + QuestionnaireQuestionsList1[i].Question + "</div>");
                                        string anser = QuestionnaireAnswerPM.QuestionnaireAnswerLines.Where(d => d.QuestionNumber == QuestionnaireQuestionsList1[i].QuestionNumber).Max(d => d.AnswerValue);
                                        HtmlTemplate.Append("<div style='font-weight:normal;height:auto;display:block;max-width:900px;word-wrap: break-word; font-size:16px;margin-top:-1px;'>" + anser + "</div>");
                                        HtmlTemplate.Append("<div style='font-weight:normal;height:5px;'>" + "</div>");
                                        QuestionnaireQuestionsList2.Add(QuestionnaireQuestionsList1[i]);
                                        HtmlTemplate.Append("</td>");
                                        HtmlTemplate.Append("<td style ='display:inline-block;width:100px;'>");
                                        HtmlTemplate.Append("</td>");
                                    }

                                    foreach (QuestionnaireQuestionPM item in QuestionnaireQuestionsList2) QuestionnaireQuestionsList1.Remove(item);
                                 
                                    HtmlTemplate.Append("</tr>");
                                }


                            }
                            else
                            {
                                QuestionnaireQuestionsList2.Remove(questionPM);
                            }

                        }
                        HtmlTemplate.Append("</table>");
                    }

                    HtmlTemplate.Append("</div>");
                    HtmlTemplate.Append("</body>");
                    HtmlTemplate.Append("</html>");

                }





            }

            return HtmlTemplate.ToString();
        }


    }
}
    //width:283px;
    //word-wrap: break-word;
    //font-family:Adventure Subtitles;
    //font-size: 15px;
    //color:red;