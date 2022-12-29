using Intuit.Ipp.Core.Configuration;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class PartnersDomainController : ApiController
    {
        public HttpResponseMessage GetAllowedAirlineId()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);
                AirlineRepository myRepository = new AirlineRepository(tenant);
                string myAirlineId = myRepository.GetAllowedAirlineId(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myAirlineId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineMessagingRuleLists()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

                AirlineMessagingRuleRepository entityRepository = new AirlineMessagingRuleRepository(tenant);
                AirlineMessagingRuleQuery entityQuery = new AirlineMessagingRuleQuery(entityRepository);
                IQueryable<AirlineMessagingRule> iQueryable = entityRepository.GetAirlineMessagingRules(0);
                IQueryable<AirlineMessagingRuleList> myResult = entityQuery.GetIQueryableEntityList(iQueryable);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCarrierUpdate(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CardQuery cardQuery = new CardQuery(authToken.Tenant);
                CardList myResult = cardQuery.GetCarrierUpdate(entityId, authToken.Tenant, null, null, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMessagingRulesForAirline(string myAirlineCode, string myMessageCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("AirlineMessagingRule", "READ", tenant);

                AirlineMessagingRuleRepository entityRepository = new AirlineMessagingRuleRepository(tenant);
                AirlineMessagingRuleQuery entityQuery = new AirlineMessagingRuleQuery(entityRepository);
                IQueryable<AirlineMessagingRule> iQueryable = entityRepository.GetAirlineMessagingRulesForAirline(myAirlineCode, myMessageCode, 0);
                IQueryable<AirlineMessagingRuleList> myResult = entityQuery.GetIQueryableEntityList(iQueryable);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerActualData(string customerId,int year,int month)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;              
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService domainService = new PartnersDomainService();
                List<CustomerProductActualDataPM> myResult= domainService.GetCustomerActualData(customerId, year, month, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage GetCustomerSalesNotes(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService domainService = new PartnersDomainService();
                List<CustomerSalesNotePM> myResult = domainService.GetCustomerSalesNotes(entityId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);                
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage GetAllAddressesPMsbyCardId(string myCardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                AddressQuery entityQuery = new AddressQuery(tenant);
                IQueryable<AddressPM> myResult = entityQuery.GetAddressePMsByTenant(tenant).Where(a => a.CardId == myCardId);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllContactsPMsbyCardId(string myCardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

                ContactQuery entityQuery = new ContactQuery(tenant);
                IQueryable<ContactPM> myResult = entityQuery.GetContactsbyCardId(myCardId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }       
        public HttpResponseMessage GetTarrifHeadersByCardIdAndTypeCode(string cardId, string typeCode, bool getAll)
        {
            try
            {
                if (cardId == "null")
                {
                    cardId = null;
                }

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", authToken.Tenant);

                TarrifHeaderQuery entityQuery = new TarrifHeaderQuery(tenant);
                List<TarrifHeaderPM> myResult = entityQuery.GetTarrifHeadersByCardIdAndTypeCode(cardId, typeCode, getAll, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCardsForContact(string contactId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                CardContactQuery cardContactQuery = new CardContactQuery(tenant);
                List<CardPM> myResult = cardContactQuery.GetCardsForContact(contactId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetContactsByEmail(string email)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);

                ContactQuery entityQuery = new ContactQuery(tenant);
                List<ContactPM> myResult = entityQuery.GetContactsByEmail(email, tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCardContactsByContact(string contactId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                CardContactQuery entityQuery = new CardContactQuery(tenant);
                List<CardContactPM> myResult = entityQuery.GetCardContactPMsByTenant(tenant).Where(cc => cc.ContactId == contactId && cc.Tenant == tenant).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCarrierCopyToCurrentTenant(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CardQuery cardQuery = new CardQuery(authToken.Tenant);
                CardList myResult = cardQuery.GetCarrierCopyToCurrentTenant(entityId, authToken.Tenant, null, null, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetIsCustomerConnectedToEntities(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                PartnersDomainService partnersDomain = new PartnersDomainService();
                bool myResult = partnersDomain.IsCustomerConnectedToEntities(entityId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetInUseCarrier(string type, string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService partnersDomain = new PartnersDomainService();

                bool myResult = false;

                CardQuery query = new CardQuery(tenant);
                CardRepository rep = new CardRepository(tenant);

                List<Card> cards = rep.GetCarrierCards(tenant).ToList();

                if (!string.IsNullOrEmpty(code))
                {
                    if (cards.Where(p => p.Code == code).FirstOrDefault() != null)
                    {
                        myResult =  true;
                    }
                    else
                    {
                        myResult =  false;
                    }
                }
                else
                {
                    myResult = false;
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineByPrefix(string Prefix)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                AirlineList myResult = null;
                AirlineRepository airlineRepository = new AirlineRepository(tenant);               
                Airline airline = airlineRepository.GetSingleAirlineByPrefix(Prefix, tenant);

                if (airline != null)
                {
                    List<Airline> singleEntityList = new List<Airline>();
                    singleEntityList.Add(airline);

                    AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);
                    IQueryable<Airline> iQueryable = singleEntityList.AsQueryable();
                    IQueryable<AirlineList> iQueryableEntityList = airlineQuery.GetIQueryableEntityList(iQueryable);
                    myResult = iQueryableEntityList.FirstOrDefault();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }       
        public HttpResponseMessage GetAirlineByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);

                AirlineQuery entityQuery = new AirlineQuery(authToken.Tenant);
                AirlinePM myResult = entityQuery.GetSinglePMByCode(code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineByICAO(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);

                AirlineQuery entityQuery = new AirlineQuery(tenant);
                AirlinePM myResult = entityQuery.GetSinglePMByICAO(code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetShippingLineByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("ShippingLine", "READ", authToken.Tenant);

                ShippingLineQuery entityQuery = new ShippingLineQuery(tenant);
                ShippingLinePM myResult = entityQuery.GetSinglePMByCode(code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTruckerByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Trucker", "READ", authToken.Tenant);

                TruckerQuery entityQuery = new TruckerQuery(tenant);
                TruckerPM myResult = entityQuery.GetSinglePMByCode(code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetWarehouseByCode(string code, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Warehouse", "READ", authToken.Tenant);

                WarehouseQuery entityQuery = new WarehouseQuery(tenant);
                WarehousePM myResult = entityQuery.GetSinglePMByCode(code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBillingAddressListByCardId(string cardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressList myResult = addressQuery.GetAddressListByTypeAndCard(cardId, "B", tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetMainAddressListByCardId(string cardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressList myResult = addressQuery.GetAddressListByTypeAndCard(cardId, "M", tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetBillingOrMainAddressListByCardId(string cardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressList myResult = addressQuery.GetAddressListByTypeAndCard(cardId, "B", tenant);
                if (myResult == null)
                {
                    myResult = addressQuery.GetAddressListByTypeAndCard(cardId, "M", tenant);
                }
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }        
        public HttpResponseMessage GetAddressByCardAndType(string cardId, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressList myResult = addressQuery.GetAddressListByTypeAndCard(cardId, type, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerById(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

                CustomerQuery customerQuery = new CustomerQuery(tenant);
                CustomerPM entityPM = customerQuery.GetSinglePM(id, tenant);

                CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                if (myFilter.IsCustomerAllowed(entityPM))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }

                else
                {
                    throw new ApplicationException("Can't view this Customer due to Business unit access level");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetIsVATUniqueForCustomer(string vatNumber, string customerId, string countryId, string partnerTypeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
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
                       && !a.InActive
                       && a.Customer.CustomerStatusCode != "INA"
                       select a);

                if (allMatchedCards != null)
                {
                    if (myTenant.VatUniqueTypeCode != "UNT")
                    {
                        bool doValidation = false;

                        if ((myTenant.VatUniquePartnerTypeCode == "POT" && partnerTypeId == "PO") || myTenant.VatUniquePartnerTypeCode == "ALL")
                        {
                            doValidation = true;
                        }

                        else if (myTenant.VatUniquePartnerTypeCode == "CUS" && partnerTypeId == "CS")
                        {
                            allMatchedCards = allMatchedCards.Where(d => d.PartnerTypeId == "CS" && d.Customer.CustomerStatusCode == "ACT");
                            doValidation = true;
                        }

                        if (doValidation)
                        {
                            isAlreadyExists = this.ValidateVAT_UniqueCountry(customerId, countryId, allMatchedCards, myTenant, isNewEntity, addressRepository);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, isAlreadyExists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private bool ValidateVAT_UniqueCountry(string customerId, string countryId, IQueryable<Card> allMatchedCards, Tenant myTenant, bool isNewEntity, AddressRepository addressRepository)
        {
            bool isAlreadyExists = false;

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
                    if (!string.IsNullOrEmpty(countryId))
                    {
                        if (countryId == myTenant.VatUniqueCountryId)
                        {
                            foreach (Card item in allMatchedCards)
                            {
                                Address address = addressRepository.GetMainAddressByCardId(item.Id, myTenant.Id);
                                if (address != null)
                                {
                                    if (address.CountryId == countryId)
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

            return isAlreadyExists;
        }

        public HttpResponseMessage GetCardExternalAccountsByProducts(string myCardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                CardExternalAccountsByProductQuery entityQuery = new CardExternalAccountsByProductQuery(tenant);
                IQueryable<CardExternalAccountsByProductPM> myResult = entityQuery.GetCardExternalAccountsByProductsByCardId(myCardId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerCardListByTenantVatNumber(string vatNumber)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService entityQuery = new PartnersDomainService();
                List<CardList> myResult = entityQuery.GetCustomerCardListByTenantVatNumber(tenant,vatNumber).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        // Partners, Address, Contact
        public HttpResponseMessage PostPartnerAddress(PartnerServicePM args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    switch (args.PartnerTypeId)
                    {
                        case "AG":
                            {
                                if (args.Agent != null)
                                {
                                    if (args.Agent.Id == null)
                                    {
                                        this.CreateAgent(args);
                                    }

                                    else
                                    {
                                        this.UpdateAgent(args);
                                    }
                                }

                                break;
                            }

                        case "CS":
                        case "PO":
                            {
                                if (args.Customer != null)
                                {
                                    if (args.Customer.Id == null)
                                    {
                                        this.CreateCustomer(args);
                                    }

                                    else
                                    {
                                        this.UpdateCustomer(args);
                                    }
                                }

                                break;
                            }

                        case "CG":
                            {
                                if (args.CustomAgent != null)
                                {
                                    if (args.CustomAgent.Id == null)
                                    {
                                        this.CreateCustomAgent(args);
                                    }

                                    else
                                    {
                                        this.UpdateCustomAgent(args);
                                    }
                                }

                                break;
                            }

                        case "SG":
                            {
                                if (args.ShippingAgent != null)
                                {
                                    if (args.ShippingAgent.Id == null)
                                    {
                                        this.CreateShippingAgent(args);
                                    }

                                    else
                                    {
                                        this.UpdateShippingAgent(args);
                                    }
                                }

                                break;
                            }

                        case "VD":
                            {
                                if (args.Vendor != null)
                                {
                                    if (args.Vendor.Id == null)
                                    {
                                        this.CreateVendor(args);
                                    }

                                    else
                                    {
                                        this.UpdateVendor(args);
                                    }
                                }

                                break;
                            }

                        case "WH":
                            {
                                if (args.Warehouse != null)
                                {
                                    if (args.Warehouse.Id == null)
                                    {
                                        this.CreateWarehouse(args);
                                    }

                                    else
                                    {
                                        this.UpdateWarehouse(args);
                                    }
                                }

                                break;
                            }

                        case "AL":
                            {
                                if (args.Airline != null)
                                {
                                    if (args.Airline.Id == null)
                                    {
                                        this.CreateAirline(args);
                                    }

                                    else
                                    {
                                        this.UpdateAirline(args);
                                    }
                                }

                                break;
                            }

                        case "SL":
                            {
                                if (args.ShippingLine != null)
                                {
                                    if (args.ShippingLine.Id == null)
                                    {
                                        this.CreateShippingLine(args);
                                    }

                                    else
                                    {
                                        this.UpdateShippingLine(args);
                                    }
                                }

                                break;
                            }

                        case "TR":
                            {
                                if (args.Trucker != null)
                                {
                                    if (args.Trucker.Id == null)
                                    {
                                        this.CreateTrucker(args);
                                    }

                                    else
                                    {
                                        this.UpdateTrucker(args);
                                    }
                                }

                                break;
                            }


                        case "CO":
                            {
                                if (args.Contact != null)
                                {
                                    HandleInactiveContact(args);
                                    if (args.Contact.Id == null)
                                    {
                                        this.CreateContact(args);
                                    }

                                    else
                                    {
                                        this.UpdateContact(args);
                                    }
                                }

                                break;
                            }
                        case "AC"://Accounting Partner
                            {
                                if (args.AccountingPartner != null)
                                {
                                    if (args.AccountingPartner.Id == null)
                                    {
                                        this.CreateAccountingPartner(args);
                                    }

                                    else
                                    {
                                        this.UpdateAccountingPartner(args);
                                    }
                                }

                                break;
                            }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void HandleInactiveContact(PartnerServicePM args)
        {
            if (args.InactiveContactId != null)
            {
                ContactPM tempContactPM = args.Contact;
                ContactQuery contactQuery = new ContactQuery(args.Tenant);
                ContactPM contactPM = contactQuery.GetSingleContactPM(args.InactiveContactId);
                args.Contact = contactPM;
                args.Contact.InActive = false;
                ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
                this.UpdatePartnerContact(args, objectContext);
                args.Contact = tempContactPM;
                args.Contact.InActive = !args.IsReactivatingContact;
            }
        }

        public HttpResponseMessage GetRecentCustomers(string ownerId, string businessUnitId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                if (ownerId == "all" || ownerId == "null" || ownerId == "undefined")
                {
                    ownerId = null;
                }

                if (businessUnitId == "all" || businessUnitId == "null" || businessUnitId == "undefined")
                {
                    businessUnitId = null;
                }

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Customer", 0, true);

                CustomerQuery customerQuery = new CustomerQuery(tenant);
                IQueryable<CustomerList> myResult = customerQuery.GetRecentEntityLists(tenant, contact.Id, objectTable.Id).AsQueryable();

                CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                myResult = myFilter.RunFilter(myResult);

                CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
                customFieldResolver.SetCustomFieldsValues("Customer", tenant, myResult.Cast<object>().ToList());

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomersCounts(string ownerId, string businessUnitId, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Customer", "READ", tenant);

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                CustomerRepository customerRepository = new CustomerRepository(tenant);
                IQueryable<CustomersDataView> dataSource = customerRepository.GetCustomersDataViews(tenant);

                CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                dataSource = myFilter.RunFilter(dataSource);

                CRMSummary myResult = new CRMSummary();

                if(RecordsTypeCode == "C")
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.CreatedByUserId == ownerId);
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(ownerId))
                    {
                        dataSource = dataSource.Where(d => d.SalesmanUserId == ownerId);
                    }

                    if (!string.IsNullOrEmpty(businessUnitId))
                    {
                        dataSource = dataSource.Where(d => d.SalesmanBusinessUnitId == businessUnitId);
                    }
                }

                dataSource = dataSource.Where(d => d.IsCustomer == true);

                string loggedUserId = this.GetLoggedUserId(loggedUserEmail, tenant);

                myResult.MyOpenDataCount = dataSource.Where(d => d.SalesmanUserId == loggedUserId && d.InActive == false).Count();
                myResult.MyOpenAsAccountManagerDataCount = dataSource.Where(d => d.AccountManagerUserId == loggedUserId && d.InActive == false).Count();
                myResult.Customers_Waiting = dataSource.Where(d => d.CustomerStatusCode == "WAC").Count();
                myResult.Customers_Potential = dataSource.Where(d => d.CustomerStatusCode == "POT").Count();
                myResult.Customers_Active = dataSource.Where(d => d.CustomerStatusCode == "ACT").Count();
                myResult.Customers_Inactive = dataSource.Where(d => d.CustomerStatusCode == "INA").Count();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomersDecreasedShipments(string dataTypeCode, string startDate, string timeRange, string ownerId, string businessUnitId, string RecordsTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                DateTime? date = DateHelper.GetDate(startDate);

                ownerId = this.FixFilter(ownerId);
                businessUnitId = this.FixFilter(businessUnitId);
                RecordsTypeCode = this.FixFilter(RecordsTypeCode);

                PartnersDomainService service = new PartnersDomainService();
                IQueryable<CompareDataClass> myResult = service.GetCustomersDecreasedShipments(dataTypeCode, date.Value, timeRange, ownerId, businessUnitId, tenant, RecordsTypeCode);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAirlinesBySearchTextAndTenant(string AWBMessagesCCSTypeCode, string searchText, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                searchText = this.FixFilter(searchText);

                AirlineRepository rep = new AirlineRepository(tenant);
                AirlineQuery query = new AirlineQuery(rep);

                IQueryable<Airline> pocos = rep.GetAirlines(0);
                IQueryable<AirlineList> myResult = query.GetIQueryableEntityList(pocos);

                if (!string.IsNullOrEmpty(AWBMessagesCCSTypeCode))
                {
                    if (AWBMessagesCCSTypeCode.ToUpper() == "GLSHK")
                    {
                        myResult = myResult.Where(d => d.GLSHKPIMA != null);
                    }

                    else
                    {
                        myResult = myResult.Where(d => d.TTY != null);
                    }
                }
                
                if(!string.IsNullOrEmpty(searchText))
                {
                    myResult = myResult.Where(d => d.SearchFields.Contains(searchText));
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult.Take(500));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllowAirline(bool isAllowed, string code, int myTenantId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService service = new PartnersDomainService();
                service.AllowAirline(isAllowed, code, tenant, myTenantId);                

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetIsDirect(int forwarderTenantId, int airlineTenantId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                bool myResult = false;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ParticipantRepository participantRepository = new ParticipantRepository(airlineTenantId);
                Participant participant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(forwarderTenantId, airlineTenantId);

                if (participant == null)
                {
                    myResult = false;
                }

                else
                {
                    myResult = participant.IsDirect;
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRegistrationRequested(bool isRequested, string tenantAirlineId, string zeroAirlineId, int tenantManagmentId, string AWBMessagesCCSTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService service = new PartnersDomainService();
                service.RegistrationRequested(isRequested, tenantAirlineId, zeroAirlineId, tenant, tenantManagmentId, AWBMessagesCCSTypeCode);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRegisteringAirline(bool isRegistered, string tenantAirlineId, string zeroAirlineId, int tenantManagmentId, string AWBMessagesCCSTypeCode, string loggedContactName)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService service = new PartnersDomainService();
                service.RegisteringAirline(isRegistered, tenantAirlineId, zeroAirlineId, tenant, tenantManagmentId, AWBMessagesCCSTypeCode, loggedContactName);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSetIsDirect(bool isDirect, string tenantAirlineId, string zeroAirlineId, int tenantManagmentId, string AWBMessagesCCSTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService service = new PartnersDomainService();
                service.SetIsDirect(isDirect, tenantAirlineId, zeroAirlineId, tenant, tenantManagmentId, AWBMessagesCCSTypeCode);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSetIsDeclined(bool isDeclined, string declineNotes, string tenantAirlineId, string zeroAirlineId, int tenantManagmentId, string AWBMessagesCCSTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                declineNotes = this.FixFilter(declineNotes);

                PartnersDomainService service = new PartnersDomainService();
                service.SetIsDeclined(isDeclined, declineNotes, tenantAirlineId, zeroAirlineId, tenant, tenantManagmentId, AWBMessagesCCSTypeCode);

                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        private void CreateContact(PartnerServicePM args)
        { 
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            ContactService contactService = new ContactService(objectContext, args.Tenant);
            contactService.Create(args.Contact);
        }
        private void UpdateContact(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            ContactService contactService = new ContactService(objectContext, args.Tenant);
            contactService.Update(args.Contact);
        }      
        private void CreateAgent(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Agent", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            AgentService myPartnerService = new AgentService(objectContext, args.Tenant);

            if (args.Agent.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.Agent.Addresses.Add(args.Address);
                }
            }

            if (args.Agent.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.Agent.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.Agent);

            if (args.Agent.Addresses.Count > 0)
            {
                args.AddressId = args.Agent.Addresses.FirstOrDefault().Id;
            }

            if (args.Agent.Contacts.Count > 0)
            {
                args.ContactId = args.Agent.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.Agent.Id;
        }
        private void CreateCustomer(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Customer", "NEW", args.Tenant);
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            CustomerService myPartnerService = new CustomerService(objectContext, args.Customer);

            if (args.Customer.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.Customer.Addresses.Add(args.Address);
                }
            }

            if (args.Customer.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.Customer.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create();

            if (args.Customer.Addresses.Count > 0)
            {
                args.AddressId = args.Customer.Addresses.FirstOrDefault().Id;
            }

            if (args.Customer.Contacts.Count > 0)
            {
                args.ContactId = args.Customer.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.Customer.Id;
        }        
        private void CreateCustomAgent(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("CustomAgent", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            CustomAgentService myPartnerService = new CustomAgentService(objectContext, args.Tenant);

            if (args.CustomAgent.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.CustomAgent.Addresses.Add(args.Address);
                }
            }

            if (args.CustomAgent.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.CustomAgent.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.CustomAgent);

            if (args.CustomAgent.Addresses.Count > 0)
            {
                args.AddressId = args.CustomAgent.Addresses.FirstOrDefault().Id;
            }

            if (args.CustomAgent.Contacts.Count > 0)
            {
                args.ContactId = args.CustomAgent.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.CustomAgent.Id;
        }
        private void CreateShippingAgent(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("ShippingAgent", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            ShippingAgentService myPartnerService = new ShippingAgentService(objectContext, args.Tenant);

            if (args.ShippingAgent.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.ShippingAgent.Addresses.Add(args.Address);
                }
            }

            if (args.ShippingAgent.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.ShippingAgent.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.ShippingAgent);

            if (args.ShippingAgent.Addresses.Count > 0)
            {
                args.AddressId = args.ShippingAgent.Addresses.FirstOrDefault().Id;
            }

            if (args.ShippingAgent.Contacts.Count > 0)
            {
                args.ContactId = args.ShippingAgent.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.ShippingAgent.Id;
        }
        private void CreateVendor(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Vendor", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            VendorService myPartnerService = new VendorService(objectContext, args.Tenant);

            if (args.Vendor.Addresses != null && args.Vendor.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.Vendor.Addresses.Add(args.Address);
                }
            }

            if (args.Vendor.Contacts != null && args.Vendor.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.Vendor.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.Vendor);

            if (args.Vendor.Addresses != null && args.Vendor.Addresses.Count > 0)
            {
                args.AddressId = args.Vendor.Addresses.FirstOrDefault().Id;
            }

            if (args.Vendor.Contacts != null &&  args.Vendor.Contacts.Count > 0)
            {
                args.ContactId = args.Vendor.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.Vendor.Id;
        }
        private void CreateWarehouse(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Warehouse", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            WarehouseService myPartnerService = new WarehouseService(objectContext, args.Tenant);

            if (args.Warehouse.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.Warehouse.Addresses.Add(args.Address);
                }
            }

            if (args.Warehouse.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.Warehouse.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.Warehouse);

            if (args.Warehouse.Addresses.Count > 0)
            {
                args.AddressId = args.Warehouse.Addresses.FirstOrDefault().Id;
            }

            if (args.Warehouse.Contacts.Count > 0)
            {
                args.ContactId = args.Warehouse.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.Warehouse.Id;
        }
        private void CreateAirline(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Airline", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            AirlineService myPartnerService = new AirlineService(objectContext, args.Tenant);

            myPartnerService.Create(args.Airline);
            args.PartnerId = args.Airline.Id;
        }
        private void CreateShippingLine(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("ShippingLine", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            ShippingLineService myPartnerService = new ShippingLineService(objectContext, args.Tenant);

            myPartnerService.Create(args.ShippingLine);
            args.PartnerId = args.ShippingLine.Id;
        }
        private void CreateTrucker(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("Trucker", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            TruckerService myPartnerService = new TruckerService(objectContext, args.Tenant);

            if (args.Trucker.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.Trucker.Addresses.Add(args.Address);
                }
            }

            if (args.Trucker.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.Trucker.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.Trucker);

            if (args.Trucker.Addresses.Count > 0)
            {
                args.AddressId = args.Trucker.Addresses.FirstOrDefault().Id;
            }

            if (args.Trucker.Contacts.Count > 0)
            {
                args.ContactId = args.Trucker.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.Trucker.Id;
        }
        private void UpdateAgent(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Agent", "UPDATE", args.Tenant);

                if (args.Agent.IsFirstContactToAdd)
                {
                    args.Agent.PrimaryContactId = args.ContactId;
                }

                AgentService myPartnerService = new AgentService(objectContext, args.Tenant);
                myPartnerService.Update(args.Agent);
            }
        }
        private void UpdateCustomer(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Customer", "UPDATE", args.Tenant);

                if (args.Customer.IsFirstContactToAdd)
                {
                    args.Customer.PrimaryContactId = args.ContactId;
                }

                CustomerService myPartnerService = new CustomerService(objectContext, args.Customer);
                myPartnerService.Update();
            }
        }
        private void UpdateCustomAgent(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("CustomAgent", "UPDATE", args.Tenant);

                if (args.CustomAgent.IsFirstContactToAdd)
                {
                    args.CustomAgent.PrimaryContactId = args.ContactId;
                }

                CustomAgentService myPartnerService = new CustomAgentService(objectContext, args.Tenant);
                myPartnerService.Update(args.CustomAgent);
            }
        }
        private void UpdateShippingAgent(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("ShippingAgent", "UPDATE", args.Tenant);

                if (args.ShippingAgent.IsFirstContactToAdd)
                {
                    args.ShippingAgent.PrimaryContactId = args.ContactId;
                }

                ShippingAgentService myPartnerService = new ShippingAgentService(objectContext, args.Tenant);
                myPartnerService.Update(args.ShippingAgent);
            }
        }
        private void UpdateVendor(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Vendor", "UPDATE", args.Tenant);

                if (args.Vendor.IsFirstContactToAdd)
                {
                    args.Vendor.PrimaryContactId = args.ContactId;
                }

                VendorService myPartnerService = new VendorService(objectContext, args.Tenant);
                myPartnerService.Update(args.Vendor);
            }
        }
        private void UpdateWarehouse(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Warehouse", "UPDATE", args.Tenant);

                if (args.Warehouse.IsFirstContactToAdd)
                {
                    args.Warehouse.PrimaryContactId = args.ContactId;
                }

                WarehouseService myPartnerService = new WarehouseService(objectContext, args.Tenant);
                myPartnerService.Update(args.Warehouse);
            }
        }
        private void UpdateAirline(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Airline", "UPDATE", args.Tenant);

                if (args.Airline.IsFirstContactToAdd)
                {
                    args.Airline.PrimaryContactId = args.ContactId;
                }

                AirlineService myPartnerService = new AirlineService(objectContext, args.Tenant);
                myPartnerService.Update(args.Airline);
            }
        }
        private void UpdateShippingLine(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("ShippingLine", "UPDATE", args.Tenant);

                if (args.ShippingLine.IsFirstContactToAdd)
                {
                    args.ShippingLine.PrimaryContactId = args.ContactId;
                }

                ShippingLineService myPartnerService = new ShippingLineService(objectContext, args.Tenant);
                myPartnerService.Update(args.ShippingLine);
            }
        }
        private void UpdateTrucker(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            this.UpdatePartnerAddress(args, objectContext);
            this.UpdatePartnerContact(args, objectContext);

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("Trucker", "UPDATE", args.Tenant);

                if (args.Trucker.IsFirstContactToAdd)
                {
                    args.Trucker.PrimaryContactId = args.ContactId;
                }

                TruckerService myPartnerService = new TruckerService(objectContext, args.Tenant);
                myPartnerService.Update(args.Trucker);
            }
        }
        private void UpdatePartnerAddress(PartnerServicePM args, ICommonDataContext objectContext)
        {
            if (args.Address != null)
            {
                if (args.IsAddressDirty)
                {
                    AddressService service = new AddressService(objectContext, args.Tenant);

                    if (args.Address.Id == null)
                    {
                        service.Create(args.Address);
                        args.AddressId = args.Address.Id;
                    }

                    else
                    {
                        service.Update(args.Address);
                    }
                }
            }
        }
        private void UpdatePartnerContact(PartnerServicePM args, ICommonDataContext objectContext)
        {
            if (args.Contact != null)
            {
                if (args.IsContactDirty)
                {
                    ContactService service = new ContactService(objectContext, args.Tenant);
                    
                    if (args.IsConnectingInactiveContact)
                    {
                        args.Contact.OldSimilarInactiveContactId = args.InactiveContactId;

                        if (args.IsReactivatingContact)
                        {
                            args.Contact.InActive = false;
                        }
                    }

                    if (args.Contact.Id == null)
                    {
                        SecurityUtility.CheckContactFeature("Contact", "NEW", args.Tenant);

                        service.Create(args.Contact);
                        args.ContactId = args.Contact.Id;
                    }

                    else
                    {
                        SecurityUtility.CheckContactFeature("Contact", "UPDATE", args.Tenant);
                        service.Update(args.Contact);
                        UpdateCustomerContactFields(args);
                    }
                }
            }
        }
        private void CreateAccountingPartner(PartnerServicePM args)
        {
            SecurityUtility.CheckContactFeature("AccountingPartner", "NEW", args.Tenant);

            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);
            AccountingPartnerService myPartnerService = new AccountingPartnerService(objectContext, args.Tenant);

            if (args.AccountingPartner.Addresses != null && args.AccountingPartner.Addresses.Count == 0)
            {
                if (args.Address != null)
                {
                    args.AccountingPartner.Addresses.Add(args.Address);
                }
            }

            if (args.AccountingPartner.Contacts != null && args.AccountingPartner.Contacts.Count == 0)
            {
                if (args.Contact != null)
                {
                    args.AccountingPartner.Contacts.Add(args.Contact);
                }
            }

            myPartnerService.Create(args.AccountingPartner);

            if (args.AccountingPartner.Addresses != null && args.AccountingPartner.Addresses.Count > 0)
            {
                args.AddressId = args.AccountingPartner.Addresses.FirstOrDefault().Id;
            }

            if (args.AccountingPartner.Contacts != null && args.AccountingPartner.Contacts.Count > 0)
            {
                args.ContactId = args.AccountingPartner.Contacts.FirstOrDefault().Id;
            }

            args.PartnerId = args.AccountingPartner.Id;

        }
        private void UpdateAccountingPartner(PartnerServicePM args)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(args.Tenant);

            if (args.Address != null)
            {
                if (args.IsAddressDirty)
                {
                    AddressService service = new AddressService(objectContext, args.Tenant);

                    if (args.Address.Id == null)
                    {
                        service.Create(args.Address);
                        args.AddressId = args.Address.Id;
                    }

                    else
                    {
                        service.Update(args.Address);
                    }
                }
            }

            if (args.Contact != null)
            {
                if (args.IsContactDirty)
                {
                    ContactService service = new ContactService(objectContext, args.Tenant);

                    if (args.Contact.Id == null)
                    {
                        SecurityUtility.CheckContactFeature("Contact", "NEW", args.Tenant);

                        service.Create(args.Contact);
                        args.ContactId = args.Contact.Id;
                    }

                    else
                    {
                        SecurityUtility.CheckContactFeature("Contact", "UPDATE", args.Tenant);

                        service.Update(args.Contact);
                    }
                }
            }

            if (args.IsPartnerDirty)
            {
                SecurityUtility.CheckContactFeature("AccountingPartner", "UPDATE", args.Tenant);

                if (args.AccountingPartner.IsFirstContactToAdd)
                {
                    args.AccountingPartner.PrimaryContactId = args.ContactId;
                }

                AccountingPartnerService myPartnerService = new AccountingPartnerService(objectContext, args.Tenant);
                myPartnerService.Update(args.AccountingPartner);
            }

        }

        public HttpResponseMessage PutPartnerExternalAccounts(PartnerExternalAccountsServicePM args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args != null)
                    {
                        ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                        string loggedUserId = null;
                        ContactRepository myContactRepository = new ContactRepository(objectContext);
                        ContactQuery myContactQuery = new ContactQuery(myContactRepository);
                        ContactPM myContactPM = myContactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                        if(myContactPM != null)
                        {
                            loggedUserId = myContactPM.Id;
                        }

                        CardRepository myCardRepository = new CardRepository(objectContext);
                        Card myCard = myCardRepository.GetSingleCard(args.CardId, tenant);
                        if (myCard.ExternalAccountingBusinessArea != args.BusinessArea || myCard.ExternalId2 != args.ExternalId2)
                        {
                            myCard.ExternalAccountingBusinessArea = args.BusinessArea;
                            myCard.ExternalId2 = args.ExternalId2;
                            myCard.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            myCard.UpdatedByUserId = loggedUserId;
                            myCardRepository.Update(myCard);
                            myCardRepository.SubmitChanges();

                            string eventTypeCode = null;                          
                            switch (myCard.PartnerTypeId)
                            {
                                case "AG": { eventTypeCode = "UPAG"; break; }
                                case "AL": { eventTypeCode = "UPAL"; break; }
                                case "CG": { eventTypeCode = "UPCA"; break; }
                                case "CS": { eventTypeCode = "CRCU"; break; }
                                case "PO": { eventTypeCode = "CRCU"; break; }
                                case "SG": { eventTypeCode = "UPSA"; break; }
                                case "SL": { eventTypeCode = "UPSL"; break; }
                                case "TR": { eventTypeCode = "UPTR"; break; }
                                case "VD": { eventTypeCode = "UPVD"; break; }
                                case "WH": { eventTypeCode = "UPWH"; break; }
                            }

                            if (!string.IsNullOrEmpty(eventTypeCode)) {
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = tenant,
                                    UserId = loggedUserId,
                                    EntityId = args.CardId,
                                    EventTypeCode = eventTypeCode,
                                    ObjectTableName = args.ObjectTableName,
                                });
                            }
                        }

                        CardExternalAccountsByProductRepository entityRepository = new CardExternalAccountsByProductRepository(objectContext);
                        List<CardExternalAccountsByProduct> allEntities = entityRepository.GetCardExternalAccountsByProductsByCardId(args.CardId, tenant).ToList();

                        CardExternalAccountsByProductService myService = new CardExternalAccountsByProductService(objectContext, tenant);
                        foreach(CardExternalAccountsByProductPM itemPM in args.Items)
                        {
                            if (string.IsNullOrEmpty(itemPM.Id))
                            {
                                myService.Create(itemPM);
                            }

                            else
                            {
                                CardExternalAccountsByProduct myEntity = allEntities.Where(d => d.Id == itemPM.Id).FirstOrDefault();
                                if(myEntity != null)
                                {
                                    if(myEntity.GLAccount != itemPM.GLAccount || myEntity.CostCenter != itemPM.CostCenter)
                                    {
                                        myService.Update(itemPM);
                                    }
                                }
                            }
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomerProductHistoryActualData(string customerId, string productTypeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService domainService = new PartnersDomainService();
                List<CustomerProductActualDataPM> myResult = domainService.GetCustomerProductHistoryActualData(customerId, productTypeCode,tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage GetCustomerProducts(string customerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService domainService = new PartnersDomainService();
                 List<CustomerProductPM> myResult = domainService.GetCustomerProducts(customerId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }



        }
        public HttpResponseMessage GetCustomersQuickSearch([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                string SearchText = filters.Filter10Value;

                PartnersDomainService domainService = new PartnersDomainService();
                List<CustomerList> myResult = domainService.GetCustomersQuickSearch(tenant, SearchText);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAirlinesForRequestedTenant(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                AirlineRepository airlineRepository = new AirlineRepository(MyContext);
                AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);

                IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
                IQueryable<AirlineList> listResult = airlineQuery.GetIQueryableEntityList(airlines);
                
                return Request.CreateResponse(HttpStatusCode.OK, listResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage GetAirlinesByFiltersAndTenant([FromUri] ApiQueryFilters filters, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Airline",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Airlines",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> AirlineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Airline", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";

                        ObjectField field = AirlineObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }

                        else
                        {
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = AirlineObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                AirlineRepository airlineRepository = new AirlineRepository(MyContext);
                IQueryable<Airline> entityPocos = airlineRepository.GetAirlines(tenant);

                AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                AirlineCustomFilter customfilters = new AirlineCustomFilter(tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);

                entityPocos = genericFilter.GetFilteredQuery<Airline>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<AirlineList> entityLists = airlineQuery.GetIQueryableEntityList(entityPocos);

                entityLists = genericFilter.GetFilteredQuery<AirlineList>(listQueryOperation, entityLists);

                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(AirlineList).GetProperty(queryOperations.SortByColumnName);

                    ObjectField objectField = (from a in AirlineObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<AirlineList, string>(queryOperations, entityLists);
                        }
                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<AirlineList, decimal>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderByDescending(d => d.EnglishName);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.Code);
                }
                
                entityLists = entityLists.Skip(skippedEntities);
                entityLists = entityLists.Take(queryOperations.PageSize);
                
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, entityLists);

                return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "all":
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }
        private string GetLoggedUserId(string loggedUserEmail, int tenant)
        {
            string loggedUserId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }

            if (loggedContactPM != null)
            {
                loggedUserId = loggedContactPM.Id;
            }

            return loggedUserId;
        }

        public HttpResponseMessage GetCardContactProducts(string cardId, string contactId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;                
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                CardContactRepository cardContactRepository = new CardContactRepository(commonDataContext);
                CardContact cardContact = cardContactRepository.GetCardContactByContactAndCard(cardId, contactId, tenant);
                CardContactProductRepository cardContactProductRepository = new CardContactProductRepository(commonDataContext);
                CardContactProductQuery cardContactProductQuery = new CardContactProductQuery(cardContactProductRepository);

                List<CardContactProductPM> cardContactProducts = new List<CardContactProductPM>();
                if(cardContact != null)
                {
                    cardContactProducts = cardContactProductQuery.GetCardContactProductPMsByCardContactId(cardContact.Id, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, cardContactProducts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllCarrierAreasByCarrierId(string carrierId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                CarrierAreaQuery carrierAreaQuery = new CarrierAreaQuery(tenant);
                List<CarrierAreaPM> carrierAreas = carrierAreaQuery.GetCarrierAreasPMsByCarrierId(carrierId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, carrierAreas);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRemoveCarrierAreaFromCarrier(string areaId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                CarrierAreaRepository carrierAreaRepository = new CarrierAreaRepository(commonDataContext);
                CarrierAreasPortRepository carrierAreasPortRepository = new CarrierAreasPortRepository(commonDataContext);

                CarrierArea carrierArea = carrierAreaRepository.GetSingleCarrierArea(areaId, tenant);

                if(carrierArea != null)
                {
                    List<CarrierAreasPort> areasPorts = carrierAreasPortRepository.GetCarrierAreasPortByAreaId(areaId, tenant);
                    if(areasPorts != null && areasPorts.Count > 0)
                    {
                        foreach (CarrierAreasPort item in areasPorts)
                        {
                            carrierAreasPortRepository.Remove(item);
                        }
                    }

                    carrierAreaRepository.Remove(carrierArea);
                    commonDataContext.SaveChanges();
                }               

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetInUseWarehouse(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                PartnersDomainService partnersDomain = new PartnersDomainService();

                bool myResult = false;
                
                CardRepository rep = new CardRepository(tenant);
                List<Card> cards = rep.GetWarehouseCards(tenant).ToList();

                if (!string.IsNullOrEmpty(code))
                {
                    if (cards.Where(p => p.Code == code).FirstOrDefault() != null)
                    {
                        myResult = true;
                    }
                    else
                    {
                        myResult = false;
                    }
                }
                else
                {
                    myResult = false;
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllTariffTranslationsByCarrierId(string carrierId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                TariffCarrierTranslationQuery myQuery = new TariffCarrierTranslationQuery(tenant);
                List<TariffCarrierTranslationPM> myResult = myQuery.GetTranslationsByCarrier(carrierId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRemoveTranslationFromCarrier(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                TariffCarrierTranslationRepository myRepository = new TariffCarrierTranslationRepository(commonDataContext);
                
                TariffCarrierTranslation translation = myRepository.GetSingleTariffCarrierTranslation(id, tenant);

                if (translation != null)
                {
                    myRepository.Remove(translation);
                    commonDataContext.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetWarehouseStoragePricingForWarehouse(string warehouseId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                WarehouseStoragePricingQuery warehouseStoragePricingQuery = new WarehouseStoragePricingQuery(tenant);
                List<WarehouseStoragePricingPM> myResult = warehouseStoragePricingQuery.GetWarehouseStoragePricingPMsByWarehouseId(warehouseId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        private void UpdateCustomerContactFields(PartnerServicePM args)
        {
            if (args.Customer != null && args.Contact != null)
            {
                args.Customer.PrimaryContactPhone = args.Contact.BusinessPhone;
                args.Customer.PrimaryContactName = args.Contact.EnglishName;
                args.IsPartnerDirty = true;
            }
        }

        public HttpResponseMessage GetCustomerProductItemHTSCodeByCountry(string productItemId, string dischargePortCountryId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                HTSCodeQuery hTSCodeQuery = new HTSCodeQuery(tenant);
                HTSCodePM hTSCode = hTSCodeQuery.GetSingleHTSCodeByProductItemAndCountry(productItemId, dischargePortCountryId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, hTSCode);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleCustomerProductItem(string productItemId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ProductItemQuery productItemQuery = new ProductItemQuery(tenant);
                ProductItemPM productItem = productItemQuery.GetSinglePM(productItemId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, productItem);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutCusttomerProductItem(ProductItemPM productItem)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Customer", "UPDATE", tenant);

                    if (productItem != null)
                    {
                        productItem.ChangeSetOp = ChangeSetOperation.Update;
                        ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                        CustomerRepository customerRepository = new CustomerRepository(commonContext);
                        CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                        CustomerPM customerPM = customerQuery.GetSinglePM(productItem.CustomerId, tenant);

                        if (customerPM != null)
                        {
                            customerPM.CustomerProductItems.Remove(customerPM.CustomerProductItems.Where(d => d.Id == productItem.Id).FirstOrDefault());
                            customerPM.CustomerProductItems.Add(productItem);
                            
                            List<ProductItemPM> productItemsChangeSet = customerPM.CustomerProductItems;
                            foreach (ProductItemPM itemPM in productItemsChangeSet)
                            {
                                switch (itemPM.ChangeSetOp)
                                {
                                    case ChangeSetOperation.Update:
                                        {
                                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                                            itemPM.HTSCodeChangeSet = itemPM.HTSCodes.ToList();
                                            break;
                                        }

                                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                                }
                            }

                            CustomerService service = new CustomerService(commonContext, customerPM);
                            service.SetChangeSet(customerPM.SalesNotes, customerPM.CustomerProducts, customerPM.CustomerCompetitors, customerPM.CustomerAdditionalServices, customerPM.CustomerSalesmanByProducts, customerPM.CustomerAccountManagerByProducts, customerPM.CustomerCustomsAgentByProducts, customerPM.CustomerForwarderByProducts, customerPM.CustomerMediatorByProducts, customerPM.CardExternalCodeByCurrencies, productItemsChangeSet);
                            service.Update();
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, productItem);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllCarrierServiceLinesByCarrierId(string carrierId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                CarrierServiceLineQuery carrierServiceLineQuery = new CarrierServiceLineQuery(tenant);
                List<CarrierServiceLinePM> serviceLinePMs = carrierServiceLineQuery.GetCarrierServiceLinePMsByCardId(carrierId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, serviceLinePMs);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRemoveServiceLineFromCarrier(string serviceLineId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                CarrierServiceLineRepository serviceLineRepository = new CarrierServiceLineRepository(commonDataContext);
                CarrierServiceLine serviceLine = serviceLineRepository.GetSingleCarrierServiceLine(serviceLineId, tenant);

                if (serviceLine != null)
                {
                    this.ValidateConnectedShipmentsToServiceLine(serviceLine);
                    serviceLineRepository.Remove(serviceLine);
                    commonDataContext.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void ValidateConnectedShipmentsToServiceLine(CarrierServiceLine serviceLine)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(serviceLine.Tenant);
            IQueryable<ShipmentMasterData> shipments = shipmentRepository.GetMasterByServiceLineId(serviceLine.Id, serviceLine.Tenant);

            if(shipments.Count() > 0)
            {
                throw new ApplicationException("Can't delete Service lines which are connected to shipments");
            }
        }
    }
}