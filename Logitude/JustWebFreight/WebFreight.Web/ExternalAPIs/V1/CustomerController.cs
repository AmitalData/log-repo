using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Currency = Simplog.Data.CommonDataModel.EntityPOCOs.Currency;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class CustomerController : ApiController
    {
        public HttpResponseMessage GetSingleCustomer(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                CustomerQueryService Service = new CustomerQueryService(tenant);
                var Result = Service.GetCustomerById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleCustomerByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                CustomerQueryService customerQueryService = new CustomerQueryService(tenant);
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer customer = customerQueryService.GetCustomerByCode(code, tenant);
                customer.Contacts = customerQueryService.GetCustomerContacts(customer, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleCustomerByVatNumber(string vatNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                CustomerQueryService Service = new CustomerQueryService(tenant);
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer Result = Service.GetCustomerByCode(vatNumber, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleCustomerByExternalId(string externalId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                CustomerQueryService Service = new CustomerQueryService(tenant);
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer Result = Service.GetCustomerByCode(externalId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleCustomerByName(string name)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                CustomerQueryService Service = new CustomerQueryService(tenant);
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer Result = Service.GetCustomerByCode(name, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


        public HttpResponseMessage Post(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);                    
					SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                    SecurityUtility.AuthenticateAccessibleAPI("Customer", authToken.Tenant);

                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                    ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                    CardRepository cardRepository = new CardRepository(MyContext);

                    string computingPartnerCode = "";
                    if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                    {
                        computingPartnerCode = entity.ComputingPartnerCode;
                    }

                    if (!string.IsNullOrEmpty(entity.PartnerCode) && string.IsNullOrEmpty(computingPartnerCode))
                    {
                        throw new ApplicationException("Please provide the computing partner code");
                    }

                    this.ValidateAddress(entity.MainAddress, "Main");
                    this.ValidateAddress(entity.BillingAddress, "Billing");
                    this.ValidateAddress(entity.PickupDeliveryAddress, "Pickup/Delivery");
                    this.ValidateContacts(entity.Contacts, authToken.Tenant);

                    if (!string.IsNullOrEmpty(entity.Code))
                    {
                        bool exist = (from a in cardRepository.GetCards(authToken.Tenant)
                                      where a.PartnerTypeId == "CS" && a.Code == entity.Code && a.Tenant == authToken.Tenant
                                      select a).Any();

                        if (exist)
                        {
                            throw new Exception("Customer with code " + entity.Code + " already exists");
                        }
                    }
                                        
                    CustomerQueryService mappingService = new CustomerQueryService(authToken.Tenant);
                    CustomerPM entityPM = mappingService.CustomerCustomDataMappingAndValidating(entity, authToken.Tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        this.ValidateSalesman(entityPM.SalesmanUserId, authToken.Tenant);

                        if(entityPM.Addresses.Count == 0)
                        {
                            throw new ApplicationException("Missing Main Address");
                        }
                        else
                        {   
                            Tenant myTenant = MyContext.Tenants.Where(d => d.Id == authToken.Tenant).FirstOrDefault();
                            this.ValidateAddress_TenantSettings(myTenant, entity.MainAddress, "Main", entity.IsPotential);
                            this.ValidateAddress_TenantSettings(myTenant, entity.BillingAddress, "Billing", entity.IsPotential);
                            this.ValidateAddress_TenantSettings(myTenant, entity.PickupDeliveryAddress, "Pickup/Delivery", entity.IsPotential);                            
                        }

                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, authToken.Tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);
                        }

                        CustomerService service = new CustomerService(MyContext, entityPM);
                        service.Create();
                        service.Submit();

                        if (!string.IsNullOrEmpty(entity.PartnerCode) && !string.IsNullOrEmpty(computingPartnerCode))
                        {
                            this.CreateComputingPartnerTranslation(computingPartnerCode, entityPM.Code, entity.PartnerCode, authToken.Tenant);                            
                        }

                        #region GLAccount
                        if (entity.GLAccount != null)
                        {
                            FullAccountingHelper fullAccountingHelper = new FullAccountingHelper();                                                        
                            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(entityPM.Id, authToken.Tenant);
                            if (card != null)
                            {
                                GLAccountPM gLAccountEntity = new GLAccountPM();
                                gLAccountEntity.Tenant = authToken.Tenant;
                                gLAccountEntity.PassedFromAPI = true;
                                gLAccountEntity.ChartOfAccountsTypeCode = "3";
                                gLAccountEntity.RevenueExpenseType = "3";
                                gLAccountEntity.AccountTypeCode = "2";
                                gLAccountEntity.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                
                                //DisplayNumber
                                if (string.IsNullOrEmpty(entity.GLAccount.DisplayNumber))
                                {
                                    gLAccountEntity.DisplayNumber = card.Code;
                                }
                                else
                                {
                                    gLAccountEntity.DisplayNumber = entity.GLAccount.DisplayNumber;
                                }

                                //InternlNumber
                                if (string.IsNullOrEmpty(entity.GLAccount.InternalNumber))
                                {
                                    gLAccountEntity.InternalNumber = CodeCounter.GetNumber("GLAccount", authToken.Tenant).ToString();
                                }
                                else
                                {
                                    gLAccountEntity.InternalNumber = entity.GLAccount.InternalNumber;
                                    gLAccountEntity.DisplayNumber = entity.GLAccount.InternalNumber;
                                }
                                
                                //EnglishName
                                if (string.IsNullOrEmpty(entity.GLAccount.EnglishName))
                                {
                                    gLAccountEntity.EnglishName = card.EnglishName;
                                }
                                else
                                {
                                    gLAccountEntity.EnglishName = entity.GLAccount.EnglishName;
                                }

                                //LocalName
                                if (string.IsNullOrEmpty(entity.GLAccount.LocalName))
                                {
                                    gLAccountEntity.LocalName = card.LocalName;
                                }
                                else
                                {
                                    gLAccountEntity.LocalName = entity.GLAccount.LocalName;
                                }

                                //ChartOfAccount
                                if (entity.GLAccount.ChartOfAccount == null)
                                {
                                    throw new ApplicationException("GL Account Chart of Account is required");
                                }
                                else
                                {
                                    ChartOfAccountRepository chartOfAccountRepository = new ChartOfAccountRepository(authToken.Tenant);
                                    ChartOfAccount chartOfAccount = new ChartOfAccount();

                                    if (!string.IsNullOrEmpty(entity.GLAccount.ChartOfAccount.Id))
                                    {
                                        chartOfAccount = chartOfAccountRepository.GetSingle(entity.GLAccount.ChartOfAccount.Id, authToken.Tenant);
                                    }
                                    else
                                    {
                                        chartOfAccount = chartOfAccountRepository.GetSingleByCode(entity.GLAccount.ChartOfAccount.Code, authToken.Tenant);
                                    }                                    

                                    if (chartOfAccount == null)
                                    {
                                        throw new ApplicationException("ChartOfAccount doesn't exist");
                                    }

                                    else
                                    {
                                        gLAccountEntity.ChartOfAccountsId = chartOfAccount.Id;
                                    }
                                }

                                //Currency
                                if (entity.GLAccount.IsMultiCurrency == true)
                                {
                                    gLAccountEntity.IsMultiCurrency = true;
                                    gLAccountEntity.ReconcileMethodCode = "0";
                                }

                                else
                                {
                                    if (entity.GLAccount.Currency == null)
                                    {
                                        throw new ApplicationException("GL Account Currency is required");
                                    }
                                    else
                                    {
                                        CurrencyRepository currencyRepository = new CurrencyRepository(MyContext);
                                        Currency currency = currencyRepository.GetSingleCurrencyByCode(entity.GLAccount.Currency.Code, authToken.Tenant);
                                        if (currency == null)
                                        {
                                            throw new ApplicationException("Currency with Code " + entity.GLAccount.Currency.Code + " doesn't exist");
                                        }

                                        else
                                        {
                                            gLAccountEntity.CurrencyId = currency.Id;
                                        }
                                    }

                                    if (entity.GLAccount.ReconcileMethod == null)
                                    {
                                        throw new ApplicationException("GL Account Reconcile Method is required");
                                    }
                                    else
                                    {
                                        ReconcileMethodRepository reconcileMethodRepository = new ReconcileMethodRepository(authToken.Tenant);
                                        ReconcileMethod reconcileMethod = reconcileMethodRepository.GetSingle(entity.GLAccount.ReconcileMethod.Code);
                                        if (reconcileMethod == null)
                                        {
                                            throw new ApplicationException("reconcile Method with Code " + entity.GLAccount.ReconcileMethod.Code + " doesn't exist");
                                        }

                                        else
                                        {
                                            gLAccountEntity.ReconcileMethodCode = reconcileMethod.Code;
                                        }
                                    }
                                }                             
                                
                                string glAccountId = fullAccountingHelper.CreateGLAccount(gLAccountEntity);
                                if(!string.IsNullOrEmpty(glAccountId))
                                {
                                    card.GLAccountId = glAccountId;
                                    cardRepository.Update(card);
                                    cardRepository.SubmitChanges();
                                }
                            }
                        }
                        #endregion

                        scope.Complete();
                    }

                    var result = mappingService.GetCustomerById(entityPM.Id, authToken.Tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Customer", entityPM.Id, "Customer API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        private void ValidateAddress(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Address address, string addressType)
        {
            if (address == null) return;

            if (address.Country == null)
                throw new ApplicationException(addressType + " address country is required");

            if (address.City == null)
                throw new ApplicationException(addressType + " address city is required");
        }
        private void ValidateAddress_TenantSettings(Tenant myTenant, Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Address address, string addressType, bool isPotential)
        {
            if (address == null) return;

            if (isPotential)
            {
                if (myTenant.IsPotentialTelRequired && string.IsNullOrEmpty(address.PhoneNumber))
                    throw new ApplicationException(addressType + " address phone Number is required");

                if (myTenant.IsPotentialFaxRequired && string.IsNullOrEmpty(address.FaxNumber))
                    throw new ApplicationException(addressType + " address fax Number is required");
            }

            else
            {
                if (myTenant.IsCustomerTelRequired && string.IsNullOrEmpty(address.PhoneNumber))
                    throw new ApplicationException(addressType + " address phone Number is required");

                if (myTenant.IsCustomerFaxRequired && string.IsNullOrEmpty(address.FaxNumber))
                    throw new ApplicationException(addressType + " address fax Number is required");
            }
        }
        private void ValidateContacts(List<Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Contact> contacts, int tenant)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTI", tenant)) return;
            if (contacts == null || contacts.Count == 0) return;
            if(contacts.Count(d => d.IsPrimaryContact) > 1) throw new ApplicationException("Only one contact allowed to be set primary");

            foreach (var contact in contacts)
            {
                if(string.IsNullOrEmpty(contact.Email))
                    throw new ApplicationException("Contact email is required");

                if (string.IsNullOrEmpty(contact.EnglishName))
                    throw new ApplicationException("Contact name is required");
            }
        }
        private void ValidateSalesman(string salesmanUserId, int tenant)
        {
            if (string.IsNullOrEmpty(salesmanUserId)) return;

            UserRepository userRepository = new UserRepository(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRepository.GetSingleUser(salesmanUserId, tenant);
            if (user != null && !user.IsSalesman)
                throw new ApplicationException("Invalid salesman user");
        }

        public HttpResponseMessage Put(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer entity)
        {

            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }

        private void CreateComputingPartnerTranslation(string computingPartnerCode, string ourCode, string partnerCode, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            UserQuery userQuery = new UserQuery(tenant);
            UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + tenant + ".com", tenant, false);

            ComputingPartnerRepository computingPartnerRepository = new ComputingPartnerRepository(commonContext);
            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(computingPartnerRepository);
            ComputingPartnerPM myPartner = computingPartnerQuery.GetSinglePMByCode(computingPartnerCode, tenant);
            if (myPartner == null)
            {
                myPartner = computingPartnerQuery.GetSinglePMByCode(computingPartnerCode, 0);
            }

            if (myPartner == null)
            {
                throw new ApplicationException("Computing Partner with Name " + computingPartnerCode + " doesn't match any record");
            }

            ObjectTableQuery objectTableQuery = new ObjectTableQuery(0);
            ObjectTablePM myTable = objectTableQuery.GetObjectTableByName("Customer", 0);

            if (myPartner != null && myTable != null)
            {
                ComputingPartnerTableRepository computingPartnerTableRepository = new ComputingPartnerTableRepository(commonContext);
                ComputingPartnerTable myPartnerTable = computingPartnerTableRepository.GetSingleComputingPartnerTable(myPartner.Tenant, myTable.Id, myPartner.Id);
                if (myPartnerTable == null)
                {
                    myPartnerTable = new ComputingPartnerTable()
                    {
                        Tenant = myPartner.Tenant,
                        ObjectTableId = myTable.Id,
                        ComputingPartnerId = myPartner.Id,
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(myPartner.Tenant),
                        CreatedByUserId = MyUserPM.Id,
                        Name = "Customer",
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(myPartner.Tenant),
                        UpdatedByUserId = MyUserPM.Id,
                    };

                    commonContext.ComputingPartnerTables.Add(myPartnerTable);
                }
                
                ComputingPartnerTranslation myTranslation = new ComputingPartnerTranslation()
                {
                    Id = IdCounter.GetNumber("ComputingPartnerTranslation", myPartner.Tenant).ToString(),
                    Tenant = myPartner.Tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(myPartner.Tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(myPartner.Tenant),
                    CreatedByUserId = MyUserPM.Id,
                    UpdatedByUserId = MyUserPM.Id,
                    ComputingPartnerId = myPartner.Id,
                    ObjectTableId = myTable.Id,
                    OurCode = ourCode,
                    PartnerCode = partnerCode,
                    SearchFields = ourCode + "," + partnerCode,
                };

                commonContext.ComputingPartnerTranslations.Add(myTranslation);
                commonContext.SaveChanges();
            }
        }
    }
}