using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers;
using Simplog.Data.Helpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.APIDataContract.QueryService;
using Logitude.BL.CommonDataModel.EntityLists;
using Marvin.JsonPatch;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class DirectController : ApiController
    {
        public HttpResponseMessage GetSingleDirect(string id, string include = "")
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Direct", authToken.Tenant);

                DirectQueryService Service = new DirectQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                Direct Result = Service.GetDirectById(id, tenant, include);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleDirectByNumber(string number, string include = "")
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("Direct", authToken.Tenant);

                DirectQueryService Service = new DirectQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                Direct Result = Service.GetDirectByShipmentNumber(number, tenant, include);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Direct entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        SecurityUtility.AuthenticateAccessibleAPI("Direct", authToken.Tenant);

                        Logitude.BL.Security.ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);

                        string computingPartnerCode = "";
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            computingPartnerCode = entity.ComputingPartnerCode;
                        }

                        ExternalAPIXMLEntityValidator externalAPIXMLEntityValidator = new ExternalAPIXMLEntityValidator(authToken.Tenant);
                        externalAPIXMLEntityValidator.ValidateDirectEntity(entity);
                        this.InitOceanOrInlandPackages(entity);
                        this.InitContainers(entity);

                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);

                        APIUnassignedDataHandler apiUnassignedDataHandler = new APIUnassignedDataHandler(authToken.Tenant, computingPartnerCode);
                        entity = apiUnassignedDataHandler.HandleUnassignedDirectShipmentData(entity);

                        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                        ShipmentPM entityPM = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);
                        entityPM.IsExternalAPI = true;

                        ExternalAPIShipmentValidator externalAPIShipmentValidator = new ExternalAPIShipmentValidator(entityPM, authToken.Tenant);
                        externalAPIShipmentValidator.ValidateUnitCodes();
                        externalAPIShipmentValidator.ValidateAirShipmentCarrier();
                        externalAPIShipmentValidator.ValidateShipmentClosure();
                        externalAPIShipmentValidator.ValidatePickupDeliveryPackages();
                        externalAPIShipmentValidator.ValidatePartnersDueToDirection();
                        externalAPIShipmentValidator.ValidatePreAndOnCarrageFields();
                        externalAPIShipmentValidator.ValidateCustomsFields(entityPM);
                        externalAPIShipmentValidator.ValidateInActiveCarriers(entityPM);
                        this.SetClosurePropertiers(entityPM);
                        this.SetMasterNumberProperties(entityPM);
                        this.SetPrepaidCollectIds(entityPM);

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        this.ValidateAndSetCustomerData(entityPM, addressRepository, authToken.Tenant);

                        if (!IsInlandDomesticShipment(entityPM))
                        {
                            if (entityPM.ShipmentPackages.Count > 0)
                            {
                                foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                                {
                                    if (entityPM.ShipmentTypeId == "FCLD" || entityPM.ShipmentTypeId == "FTL")
                                    {
                                        item.IsContainer = true;
                                    }
                                    externalAPIShipmentValidator.ValidateShipmentPackageDimensionsAndVolume(item, entityPM);

                                    if (!item.IsContainer)
                                    {
                                        if (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
                                        {
                                            throw new ApplicationException("Inside Packages allowed in FCL/FTL shipments only");
                                        }
                                    }

                                    else
                                    {
                                        if (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
                                        {
                                            item.Weight = 0;
                                            item.Volume = 0;
                                            item.InsideShipmentPackages.ForEach(inside =>
                                            {
                                                if (inside.Weight != null)
                                                {
                                                    item.Weight += inside.Weight;
                                                }

                                                if (inside.Volume != null)
                                                {
                                                    item.Volume += inside.Volume;
                                                }

                                                if (inside.Quantity == null)
                                                {
                                                    throw new ApplicationException("Inside Packages Quantity is required");
                                                }

                                                inside.Volume = ComputeHelper.ComputeInsideVolume(inside, entityPM);
                                                inside.VolumetricWeight = ComputeHelper.ComputeInsideVolumetricWeight(inside, entityPM);
                                            });
                                        }
                                    }
                                    item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                                }
                            }
                        }
                        else
                        {
                            entityPM = externalAPIShipmentValidator.ValidateInlandDomesticShipment(entityPM);
                            this.UpdateRoutingPartnersAddresses(entityPM);
                        }

                        ComputeHelper.ComputeTotals(entityPM);

                        APIReceivablePayableHelper receivablePayableHelper = new APIReceivablePayableHelper(entityPM, authToken.Tenant);
                        receivablePayableHelper.ValidateReceivablesAndPayables();
                        receivablePayableHelper.ComputeReceivablesPayablesTotals();


                        if (!IsInlandDomesticShipment(entityPM) && entityPM.MainCarriageLegs != null && entityPM.MainCarriageLegs.Count > 0)
                        {
                            ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(entityPM, authToken.Tenant);
                            externalAPIMainCarriageLegsHelper.ValidateMainCarriageLegs();
                            externalAPIMainCarriageLegsHelper.MapTransshipments();
                        }

                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, authToken.Tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);
                        }

                        entityPM.HasUnassignedData = apiUnassignedDataHandler.HasUnassignedData;
                        entityPM = apiUnassignedDataHandler.AddDirectShipmentUnassignedData(entity, entityPM);
                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        if (entity.AddManualEvents != null && entity.AddManualEvents.Count > 0)
                        {
                            EventQueryService eventQueryService = new EventQueryService(authToken.Tenant);
                            eventQueryService.CreateShipmentTraceEvents(entityPM, entity.AddManualEvents, true, computingPartnerCode);
                        }

                        var result = mappingService.GetDirectById(entityPM.Id, authToken.Tenant, null);
                        //this.MapInlandDomesticStates(result, authToken.Tenant);
                        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Direct API", authToken.Tenant);
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Direct entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.AuthenticateAccessibleAPI("Direct", authToken.Tenant);

                    return Request.CreateResponse(HttpStatusCode.OK, this.UpdateDirectShipment(entity, authToken.Tenant));
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Patch(string id, JsonPatchDocument<Direct> directPatchEntity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Request.CreateResponse(HttpStatusCode.OK, this.UpdateDirectShipmentByPatchUpdate(id, directPatchEntity));
                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", directPatchEntity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", directPatchEntity, apiExceptionResult.Exception, "Shipment", null, "Direct API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private Direct UpdateDirectShipmentByPatchUpdate(string id, JsonPatchDocument<Direct> directPatchEntity)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticateAccessibleAPI("Direct", authToken.Tenant);

            Direct directShipment = this.GetDirectShipment(id, authToken.Tenant);
            directPatchEntity.ApplyTo(directShipment);

            return this.UpdateDirectShipment(directShipment, authToken.Tenant);
        }

        private Direct UpdateDirectShipment(Direct entity, int tenant)
        {
            string computingPartnerCode = "";
            if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
            {
                computingPartnerCode = entity.ComputingPartnerCode;
            }
            if (entity.TransportMode != null && entity.TransportMode.Code == "A")
            {
                this.ValidateMasterNumberAndCarrier(entity);
            }

            if (FeatureToggleHelper.HasFeatureToggle("API", tenant))
            {
                IShipmentsContext MyContext = ShipmentsContext.GetContext(tenant);
                DirectQueryService mappingService = new DirectQueryService(tenant);

                this.InitOceanOrInlandPackages(entity);
                this.InitContainers(entity);

                APIUnassignedDataHandler apiUnassignedDataHandler = new APIUnassignedDataHandler(tenant, computingPartnerCode);
                entity = apiUnassignedDataHandler.HandleUnassignedDirectShipmentData(entity);

                ShipmentPM directPM = mappingService.DirectDataMappingAndValidatin(entity, tenant, computingPartnerCode, true);

                if (directPM != null)
                {
                    ExternalAPIShipmentValidator externalAPIShipmentValidator = new ExternalAPIShipmentValidator(directPM, tenant);
                    directPM.ConcurrencyGUID = entity.ConcurrencyGUID;
                    directPM.IsExternalAPI = true;

                    if (directPM.IsOperationalClosed)
                    {
                        throw new ApplicationException("Can't update operationally closed shipments");
                    }

                    if (directPM.IsCancelled)
                    {
                        throw new ApplicationException("Can't update cancelled shipments");
                    }
                    if (!IsInlandDomesticShipment(directPM))
                    {
                        ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(directPM, tenant);
                        externalAPIMainCarriageLegsHelper.ValidateMainCarriageLegs();
                        externalAPIMainCarriageLegsHelper.MapTransshipments();

                        AddressRepository addressRepository = new AddressRepository(tenant);
                        this.ValidateAndSetCustomerData(directPM, addressRepository, tenant);
                        externalAPIShipmentValidator.ValidatePreAndOnCarrageFields();
                    }
                    else
                    {
                        directPM = externalAPIShipmentValidator.ValidateInlandDomesticShipment(directPM);
                        this.UpdateRoutingPartnersAddresses(directPM);
                    }

                    externalAPIShipmentValidator.ValidateUpdateShipmentPackages(directPM);
                    externalAPIShipmentValidator.UpdatePickupDeliveryPackagesChangeSet(directPM);
                    externalAPIShipmentValidator.UpdatePayablesChangeSet(directPM);
                    externalAPIShipmentValidator.UpdateReceivablesChangeSet(directPM);
                    externalAPIShipmentValidator.ValidateCustomsFields(directPM);
                    externalAPIShipmentValidator.ValidateInActiveCarriers(directPM);
                    directPM = this.UpdatePartners(MyContext, directPM);
                    ComputeHelper.ComputeTotals(directPM);
                    mappingService.UpdatePickups(directPM);
                    mappingService.UpdateDeliveries(directPM);

                    directPM.HasUnassignedData = apiUnassignedDataHandler.HasUnassignedData;
                    directPM = apiUnassignedDataHandler.AddDirectShipmentUnassignedData(entity, directPM);

                    ShipmentService service = new ShipmentService(MyContext, directPM, SecurityUtility.GetAuthenticatedUser());
                    service.Update(true);

                    if (entity.AddManualEvents != null && entity.AddManualEvents.Count > 0)
                    {
                        EventQueryService eventQueryService = new EventQueryService(tenant);
                        eventQueryService.CreateShipmentTraceEvents(directPM, entity.AddManualEvents, true, "");
                    }
                }

                var result = mappingService.GetDirectById(directPM.Id, tenant, null);
                //this.MapInlandDomesticStates(result, tenant);
                APIHelper.AddCommunicationLog("D", entity, result, "Shipment", directPM.Id, "Direct API", tenant);
                return result;
            }
            else
            {
                throw new ApplicationException("Update is not allowed");
            }
        }

        private void UpdateRoutingPartnersAddresses(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticFromTypeCode == "PART" && !string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId))
            {
                this.SetFromPartnerAddress(entityPM);
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT" && !string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                this.SetFromPortAddress(entityPM);
            }

            if (entityPM.InlandDomesticToTypeCode == "PART" && !string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
            {
                this.SetToPartnerAddress(entityPM);
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT" && !string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                this.SetToPortAddress(entityPM);
            }
        }
        private void SetFromPartnerAddress(ShipmentPM entityPM)
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            CardQuery cardQuery = new CardQuery(entityPM.Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(entityPM.MainCarriageFromPartnerId, entityPM.Tenant);

            if (card != null)
            {
                CardList cardList = cardQuery.GetSingleCardList(card);
                if (cardList != null)
                {
                    if (!string.IsNullOrEmpty(cardList.PickAddressId))
                    {
                        entityPM.MainCarriageFromAddressId = cardList.PickAddressId;
                    }

                    else
                    {
                        entityPM.MainCarriageFromAddressId = cardList.MainAddressId;
                    }
                }
            }
        }
        private void SetFromPortAddress(ShipmentPM entityPM)
        {
            PortRepository portRepository = new PortRepository(entityPM.Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Port port = portRepository.GetSinglePort(entityPM.MainCarriageFromPortId, entityPM.Tenant);
            if (port != null)
            {
                entityPM.MainCarriageFromPortAddress = "Port Of: " + port.EnglishName;
            }
        }
        private void SetToPartnerAddress(ShipmentPM entityPM)
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            CardQuery cardQuery = new CardQuery(entityPM.Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(entityPM.MainCarriageToPartnerId, entityPM.Tenant);

            if (card != null)
            {
                CardList cardList = cardQuery.GetSingleCardList(card);
                if (cardList != null)
                {
                    if (!string.IsNullOrEmpty(cardList.PickAddressId))
                    {
                        entityPM.MainCarriageToAddressId = cardList.PickAddressId;
                    }

                    else
                    {
                        entityPM.MainCarriageToAddressId = cardList.MainAddressId;
                    }
                }
            }
        }
        private void SetToPortAddress(ShipmentPM entityPM)
        {
            PortRepository portRepository = new PortRepository(entityPM.Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Port port = portRepository.GetSinglePort(entityPM.MainCarriageToPortId, entityPM.Tenant);
            if (port != null)
            {
                entityPM.MainCarriageToPortAddress = "Port Of: " + port.EnglishName;
            }
        }
        private void SetCustomerTypeCode(ShipmentPM entityPM)
        {
            if (entityPM.CustomerId == entityPM.ShipperId)
            {
                entityPM.ShipmentCustomerTypeCode = "SHI";
            }

            else if (entityPM.CustomerId == entityPM.ConsigneeId)
            {
                entityPM.ShipmentCustomerTypeCode = "CON";
            }

            else if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
            {
                entityPM.ShipmentCustomerTypeCode = "SNE";
            }

            else if (entityPM.CustomerId == entityPM.AgentId)
            {
                entityPM.ShipmentCustomerTypeCode = "AGT";
            }

            else if (entityPM.CustomerId == entityPM.CustomAgentImportId)
            {
                entityPM.ShipmentCustomerTypeCode = "CAI";
            }

            else if (entityPM.CustomerId == entityPM.ReleasingAgentId)
            {
                entityPM.ShipmentCustomerTypeCode = "REA";
            }

            else if (entityPM.CustomerId == entityPM.FreightForwarderId)
            {
                entityPM.ShipmentCustomerTypeCode = "FOR";
            }

            else if (entityPM.CustomerId == entityPM.Notify1Id)
            {
                entityPM.ShipmentCustomerTypeCode = "NT1";
            }

            else if (entityPM.CustomerId == entityPM.Notify2Id)
            {
                entityPM.ShipmentCustomerTypeCode = "NT2";
            }
        }
        private void ValidateMasterNumberAndCarrier(Direct entity)
        {
            bool validate = false;
            if (!string.IsNullOrEmpty(entity.Master))
            {
                if (entity.MainCarriageCarrier == null)
                {
                    validate = true;
                }

                else
                {
                    if (string.IsNullOrEmpty(entity.MainCarriageCarrier.Code))
                    {
                        validate = true;
                    }
                }
            }

            if (validate)
            {
                throw new ApplicationException("Main Carriage Carrier is required when sending MAWB");
            }
        }
        private void ValidateAndSetCustomerData(ShipmentPM entityPM, AddressRepository addressRepository, int tenant)
        {
            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                if (!IsSentCustomerAShipmentPatrner(entityPM))

                {
                    throw new ApplicationException("The sent customer is not one of the sent partners");
                }

                this.SetCustomerTypeCode(entityPM);

                Simplog.Data.CommonDataModel.EntityPOCOs.Address address = addressRepository.GetMainAddressByCardId(entityPM.CustomerId, tenant);
                if (address != null)
                {
                    entityPM.CustomerAddressId = address.Id;
                }

                CardRepository cardRepository = new CardRepository(tenant);
                Simplog.Data.CommonDataModel.EntityPOCOs.Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, tenant);
                if (customer != null)
                {
                    if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
                    {
                        entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                    }

                    if (customer.Customer != null)
                    {
                        if (string.IsNullOrEmpty(entityPM.AccountManagerUserId))
                        {
                            entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                        }
                    }
                }
            }

            else
            {
                if (entityPM.DirectionId == "I")
                {
                    entityPM.CustomerId = entityPM.ConsigneeId;
                    entityPM.ShipmentCustomerTypeCode = "CON";
                }

                else
                {
                    entityPM.CustomerId = entityPM.ShipperId;
                    entityPM.ShipmentCustomerTypeCode = "SHI";
                }

                if (string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    throw new ApplicationException("The customer is required");
                }
            }
        }
        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }
        private bool IsSentCustomerAShipmentPatrner(ShipmentPM entityPM)
        {
            if (entityPM.CustomerId == entityPM.ShipperId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.ConsigneeId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.AgentId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.CustomAgentImportId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.ReleasingAgentId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.FreightForwarderId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.CustomAgentExportId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.ConsigneeNotImporterId)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.Notify1Id)
            {
                return true;
            }
            else if (entityPM.CustomerId == entityPM.Notify2Id)
            {
                return true;
            }
            return false;
        }
        private ShipmentPM UpdatePartners(IShipmentsContext shipmentsContext, ShipmentPM shipmentPM)
        {
            ExternalAPIShipmentPartnersModifier externalAPIShipmentPartnersUpdate = new ExternalAPIShipmentPartnersModifier(shipmentsContext, shipmentPM);
            return externalAPIShipmentPartnersUpdate.UpdatePartners();
        }
        private void InitOceanOrInlandPackages(Direct entity)
        {
            if (entity.OceanOrInlandPackages == null)
            {
                return;
            }
            foreach (OceanOrInlandPackage item in entity.OceanOrInlandPackages)
            {
                if (item.PackageType != null)
                {
                    if (entity.ShipmentType != null && (entity.ShipmentType.Code.Contains("LCL") || entity.ShipmentType.Code.Contains("LTL")))
                    {
                        if (item.Pieces == null || item.Pieces == 0)
                        {
                            item.Pieces = 1;
                        }
                    }
                }
            }
        }
        private void InitContainers(Direct entity)
        {
            if (entity.Containers == null)
            {
                return;
            }

            foreach (Logitude.BL.ShipmentsModel.APIDataContract.ApiV1.Container item in entity.Containers)
            {
                item.Pieces = 1;
            }
        }
        private void SetClosurePropertiers(ShipmentPM entityPM)
        {
            if (entityPM.IsOperationalClosed)
            {
                entityPM.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            if (entityPM.IsAccountingClosed)
            {
                entityPM.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }
        private void SetMasterNumberProperties(ShipmentPM entityPM)
        {
            if (entityPM.TransportModeId == "A")
            {
                AirlineRepository airlineRepository = new AirlineRepository(entityPM.Tenant);
                Airline airline = airlineRepository.GetSingleAirline(entityPM.MainCarriageCarrierId, entityPM.Tenant);
                if (airline != null)
                {
                    entityPM.CarrierIsCheckDigit = airline.CheckDigit;
                    entityPM.CarrierIsLimitedLength = airline.LimitedLength;
                }
            }
        }
        private void SetPrepaidCollectIds(ShipmentPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.IncotermId))
            {
                IncotermRepository myIncotermRepository = new IncotermRepository(entityPM.Tenant);
                Simplog.Data.CommonDataModel.EntityPOCOs.Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(entityPM.IncotermId, entityPM.Tenant);
                if (myIncoterm != null)
                {
                    entityPM.FreightPrepaidCollectId = myIncoterm.Freight;
                    entityPM.OtherPrepaidCollectId = myIncoterm.OtherCharges;
                }
            }
        }
        private Direct GetDirectShipment(string id, int tenant)
        {
            if (id == null)
                throw new Exception("ShipmentId is required");

            DirectQueryService Service = new DirectQueryService(tenant);
            Direct directShipment = Service.GetDirectById(id, tenant, "");

            return directShipment;
        }
        private void MapInlandDomesticStates(Direct result, int tenant)
        {
            CountryCityRepository countryCityRepository = new CountryCityRepository(tenant);
            if (result.InlandDomesticFromTypeCode != null && result.InlandDomesticFromTypeCode.Code == "CASL")
            {
                CountryCity countryCity = countryCityRepository.GetSingleCountryCityByNameAndCountry(result.InlandDomesticFromCity, result.InlandDomesticFromCountry.Id, tenant);
                result.InlandDomesticFromState = this.GetStateDataContract(countryCity);                
            }

            if (result.InlandDomesticToTypeCode != null && result.InlandDomesticToTypeCode.Code == "CASL")
            {
                CountryCity countryCity = countryCityRepository.GetSingleCountryCityByNameAndCountry(result.InlandDomesticToCity, result.InlandDomesticToCountry.Id, tenant);
                result.InlandDomesticToState = this.GetStateDataContract(countryCity);                
            }
        }

        private Logitude.BL.CommonDataModel.APIDataContract.ApiV1.State GetStateDataContract(CountryCity countryCity)
        {
            Logitude.BL.CommonDataModel.APIDataContract.ApiV1.State state = null;
            if (countryCity != null)
            {
                StateQueryService stateQueryService = new StateQueryService(countryCity.Tenant);
                state = stateQueryService.GetStateById(countryCity.StateId, countryCity.Tenant, "");
            }

            return state;
        }
    }
}
