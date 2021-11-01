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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers;
using Simplog.Data.Helpers;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class DirectController : ApiController
    {
        public HttpResponseMessage GetSingleDirect(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                DirectQueryService Service = new DirectQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                Direct Result = Service.GetDirectById(id, tenant);
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleDirectByNumber(string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                DirectQueryService Service = new DirectQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                Direct Result = Service.GetDirectByShipmentNumber(number, tenant);                
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
                        ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                        string computingPartnerCode = "";
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            computingPartnerCode = entity.ComputingPartnerCode;
                        }

                        ExternalAPIXMLEntityValidator externalAPIXMLEntityValidator = new ExternalAPIXMLEntityValidator(authToken.Tenant);
                        externalAPIXMLEntityValidator.ValidateDirectEntity(entity);
                        this.InitOceanOrInlandPackages(entity);                        

                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                        ShipmentPM entityPM = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);
                        entityPM.IsExternalAPI = true;

                        ExternalAPIShipmentValidator externalAPIShipmentValidator = new ExternalAPIShipmentValidator(entityPM, authToken.Tenant);
                        externalAPIShipmentValidator.ValidateUnitCodes();
                        externalAPIShipmentValidator.ValidateAirShipmentCarrier();
                        externalAPIShipmentValidator.ValidateShipmentClosure();
                        externalAPIShipmentValidator.ValidatePickupDeliveryPackages();
                        externalAPIShipmentValidator.ValidatePartnersDueToDirection();

                        this.SetClosurePropertiers(entityPM);
                        this.SetMasterNumberProperties(entityPM);
                        this.SetPrepaidCollectIds(entityPM);

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        this.ValidateAndSetCustomerData(entityPM, addressRepository, authToken.Tenant);
                        this.ValidateCustomsFields(entityPM);
                        this.ValidateOnCarriageDates(entityPM);
                        this.ValidatePreCarriageDates(entityPM);

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
                                    this.ValidateShipmentPackageDimensionsAndVolume(item,entityPM);

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
                            entityPM = this.ValidateInlandDomesticShipment(entityPM);
                        }

                        ComputeHelper.ComputeTotals(entityPM);

                        APIReceivablePayableHelper receivablePayableHelper = new APIReceivablePayableHelper(entityPM, authToken.Tenant);
                        receivablePayableHelper.ValidateReceivablesAndPayables();
                        receivablePayableHelper.ComputeReceivablesPayablesTotals();
                

                        if (entityPM.MainCarriageLegs != null && entityPM.MainCarriageLegs.Count > 0)
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

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        var result = mappingService.GetDirectById(entityPM.Id, authToken.Tenant);
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

                    if (entity.TransportMode != null && entity.TransportMode.Code == "A")
                    {
                        this.ValidateMasterNumberAndCarrier(entity);
                    }

                    if (FeatureToggleHelper.HasFeatureToggle("API", authToken.Tenant))
                    {
                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                        ShipmentPM directPM = mappingService.DirectDataMappingAndValidatin(entity, authToken.Tenant, "", true);

                        if (directPM != null)
                        {
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
                                ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(directPM, authToken.Tenant);
                                externalAPIMainCarriageLegsHelper.ValidateMainCarriageLegs();
                                externalAPIMainCarriageLegsHelper.MapTransshipments();

                                AddressRepository addressRepository = new AddressRepository(authToken.Tenant);
                                this.ValidateAndSetCustomerData(directPM, addressRepository, authToken.Tenant);
                                this.ValidateUpdateShipmentPackages(directPM);
                            }
                            else
                            {
                                directPM = this.ValidateInlandDomesticShipment(directPM);
                            }

                            this.ValidateCustomsFields(directPM);
                            this.ValidateOnCarriageDates(directPM);
                            this.ValidatePreCarriageDates(directPM);
                            this.UpdatePartners(MyContext, directPM);
                            ComputeHelper.ComputeTotals(directPM);
                            ShipmentService service = new ShipmentService(MyContext, directPM, SecurityUtility.GetAuthenticatedUser());
                            service.Update(true);
                        }

                        MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        mappingService = new DirectQueryService(authToken.Tenant);
                        var result = mappingService.GetDirectById(directPM.Id, authToken.Tenant);
                        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", directPM.Id, "Direct API", authToken.Tenant);
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }

                    else
                    {
                        throw new ApplicationException("Update is not allowed");
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

        private ShipmentPM SetInlandDomesticShipmentFromPartners(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticFromTypeCode == "CASL")
            {
                entityPM.MainCarriageFromPartnerId = null;
                entityPM.MainCarriageFromPortId = null;
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PART")
            {
                entityPM.InlandDomesticFromCity = null;
                entityPM.InlandDomesticFromCountryId = null;
                entityPM.MainCarriageFromPortId = null;
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT")
            {
                entityPM.InlandDomesticFromCity = null;
                entityPM.InlandDomesticFromCountryId = null;
                entityPM.MainCarriageFromPartnerId = null;
            }
            return entityPM;
        }

        private ShipmentPM SetInlandDomesticShipmentToPartners(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticToTypeCode == "CASL")
            {
                entityPM.MainCarriageToPartnerId = null;
                entityPM.MainCarriageToPortId = null;
            }
            else if (entityPM.InlandDomesticToTypeCode == "PART")
            {
                entityPM.InlandDomesticToCity = null;
                entityPM.InlandDomesticToCountryId = null;
                entityPM.MainCarriageToPortId = null;
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT")
            {
                entityPM.InlandDomesticToCity = null;
                entityPM.InlandDomesticToCountryId = null;
                entityPM.MainCarriageToPartnerId = null;
            }
            return entityPM;
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

                Address address = addressRepository.GetMainAddressByCardId(entityPM.CustomerId, tenant);
                if (address != null)
                {
                    entityPM.CustomerAddressId = address.Id;
                }

                CardRepository cardRepository = new CardRepository(tenant);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, tenant);
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
                if (entityPM.
                    Id == "I")
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

        private ShipmentPM ValidateInlandDomesticShipment(ShipmentPM entityPM)
        {
            ValidateInlandDomesticShipmentFromTypeCode(entityPM);
            ValidateInlandDomesticShipmentToTypeCode(entityPM);
            entityPM = SetInlandDomesticShipmentFromPartners(entityPM);
            entityPM = SetInlandDomesticShipmentToPartners(entityPM);
            ValidateInlandDomesticMainCarriageDates(entityPM);

            return entityPM;
        }

        private void ValidateInlandDomesticShipmentToTypeCode(ShipmentPM entityPM)
        {
            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticToCity) || string.IsNullOrEmpty(entityPM.InlandDomesticToCountryId);
            string[] inlandDomesticToTypeCodes = { "CASL", "PART", "PORT" };
            if (string.IsNullOrEmpty(entityPM.InlandDomesticToTypeCode))
            {
                throw new ApplicationException("InlandDomesticToTypeCode Field is Required");
            }
            if (entityPM.InlandDomesticToTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new ApplicationException("InlandDomestic To City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PART" && string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
            {
                throw new ApplicationException("MainCarriageToPartner Field is Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                throw new ApplicationException("ToPort Field is Required");
            }
            else if (!inlandDomesticToTypeCodes.Contains(entityPM.InlandDomesticToTypeCode))
            {
                throw new ApplicationException("Invalid InlandDomesticToTypeCode");
            }
        }

        private void ValidateInlandDomesticShipmentFromTypeCode(ShipmentPM entityPM)
        {
            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticFromCity) || string.IsNullOrEmpty(entityPM.InlandDomesticFromCountryId);
            string[] inlandDomesticFromTypeCodes = { "CASL", "PART", "PORT" };
            if (string.IsNullOrEmpty(entityPM.InlandDomesticFromTypeCode))
            {
                throw new ApplicationException("InlandDomesticFromTypeCode Field is Required");
            }
            if (entityPM.InlandDomesticFromTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new ApplicationException("InlandDomesticFrom City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PART" && (string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId)))
            {
                throw new ApplicationException("MainCarriageFromPartner Field is Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                throw new ApplicationException("FromPort Field is Required");
            }
            else if (!inlandDomesticFromTypeCodes.Contains(entityPM.InlandDomesticFromTypeCode))
            {
                throw new ApplicationException("Invalid InlandDomesticFromTypeCode");
            }
        }        

        private void ValidateCustomsFields(ShipmentPM shipmentPM)
        {
            if (!this.IsAddingCustomsFields(shipmentPM))
            {
                return;
            }
            if (shipmentPM.CustomsClearanceDate != null)
            {
                shipmentPM.IncludesCustoms = true;
                return;
            }
            if (shipmentPM.DeclarationDate == null && !string.IsNullOrEmpty(shipmentPM.DeclarationNumber))
            {
                throw new ApplicationException("Declaration Date Field Is Required");
            }
            shipmentPM.IncludesCustoms = true;
        }
     
        private void ValidateOnCarriageDates(ShipmentPM entityPM)
        {
            if (!this.IsRoutingLegDatesValid(entityPM.OnCarriageETD, entityPM.OnCarriageETA))
            {
                throw new ApplicationException("On Carriage expected departure must be less than On Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(entityPM.OnCarriageATD, entityPM.OnCarriageATA))
            {
                throw new ApplicationException("On Carriage actual departure must be less than On Carriage actual arrival");
            }
        }

        private void ValidatePreCarriageDates(ShipmentPM entityPM)
        {
            if (!this.IsRoutingLegDatesValid(entityPM.PreCarriageETD, entityPM.PreCarriageETA))
            {
                throw new ApplicationException("Pre Carriage expected departure must be less than Pre Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(entityPM.PreCarriageATD, entityPM.PreCarriageATA))
            {
                throw new ApplicationException("Pre Carriage actual departure must be less than Pre Carriage actual arrival");
            }
        }

        private void ValidateInlandDomesticMainCarriageDates(ShipmentPM entityPM)
        {
            if (!this.IsRoutingLegDatesValid(entityPM.MainCarriageETD, entityPM.MainCarriageETA))
            {
                throw new ApplicationException("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(entityPM.MainCarriageATD, entityPM.MainCarriageATA))
            {
                throw new ApplicationException("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
            }

        }

        private void ValidateShipmentPackageDimensionsAndVolume(ShipmentPackagePM shipmentPackagePM, ShipmentPM entityPM)
        {
            double? calculatedVolume = ComputeHelper.ComputeVolume(shipmentPackagePM, entityPM);

            if (IsOneOfTheDimensionsNotNull(shipmentPackagePM))
            {
                shipmentPackagePM.Volume = calculatedVolume;
            }

        }

        private void ValidateUpdateShipmentPackages(ShipmentPM entityPM)
        {
            if (!(entityPM.ShipmentPackages.Count > 0))
            {
                return;
            }
            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
            {
                this.ValidateShipmentPackageItem(item, entityPM);
            }

        }

        private void ValidateShipmentPackageItem(ShipmentPackagePM item, ShipmentPM entityPM)
        {
            this.ValidateShipmentPackageDimensionsAndVolume(item, entityPM);

            if (!item.IsContainer)
            {
                if (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
                {
                    throw new ApplicationException("Inside Packages allowed in FCL/FTL shipments only");
                }
            }
            else
            {
                this.ValidateInsidePackage(item, entityPM);
            }
            item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
            item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        }

        private void ValidateInsidePackage(ShipmentPackagePM item, ShipmentPM entityPM)
        {
            if (!(item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0))
            {
                return;
            }

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
        private bool IsInlandDomesticShipment(Direct entity)
        {
            bool isInland = false;
            bool isDomestic = false;
            if (entity.Direction != null)
            {
                isDomestic = entity.Direction.Code == "D" ? true : false;
            }

            if (entity.TransportMode != null)
            {
                isInland = entity.TransportMode.Code == "I" ? true : false;
            }

            return isDomestic && isInland;
        }

        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }

        private bool IsAddingCustomsFields(ShipmentPM shipmentPM)
        {
            if (shipmentPM.CustomsClearanceDate != null)
            {
                return true;
            }
            if (shipmentPM.DeclarationDate != null)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(shipmentPM.DeclarationNumber))
            {
                return true;
            }

            return false;
        }

        private bool IsSentCustomerAShipmentPatrner(ShipmentPM entityPM)
        {
            if (entityPM.CustomerId == entityPM.ShipperId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.ConsigneeId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.AgentId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.CustomAgentImportId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.ReleasingAgentId)
            {
                return true;
            }
            if (entityPM.CustomerId == entityPM.FreightForwarderId)
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
            return false;
        }

        private bool IsRoutingLegDatesValid(DateTime? fisrtDate, DateTime? secondeDate)
        {
            bool isValid = true;

            if (fisrtDate != null && secondeDate != null)
            {
                if (fisrtDate > secondeDate.Value.AddHours(24))
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private bool IsOneOfTheDimensionsNotNull(ShipmentPackagePM shipmentPackagePM)
        {
            if (shipmentPackagePM.Width != null)
            {
                return true;
            }
            if (shipmentPackagePM.Height != null)
            {
                return true;
            }
            if (shipmentPackagePM.Length != null)
            {
                return true;
            }
            return false;
        }

        private void UpdatePartners(IShipmentsContext shipmentsContext, ShipmentPM shipmentPM)
        {
            Shipment shipmentPOCO = shipmentsContext.Shipments.Where(d => d.Id == shipmentPM.Id && d.Tenant == shipmentPM.Tenant).FirstOrDefault();
            if(shipmentPOCO != null)
            {
                this.UpdateNotify1Partner(shipmentPOCO, shipmentPM);
            }
        }
        private void UpdateNotify1Partner(Shipment shipmentPOCO, ShipmentPM shipmentPM)
        {
            if(shipmentPOCO.Notify1Id != shipmentPM.Notify1Id)
            {
                Card card = CardRepository.GetSingleCard(shipmentPM.Notify1Id, shipmentPM.Tenant, false);
                this.MapNotify1Fields(shipmentPM, card);                
            }
        }
        private void MapNotify1Fields(ShipmentPM shipmentPM, Card card)
        {
            if(card != null)
            {
                AddressRepository addressRepository = new AddressRepository(shipmentPM.Tenant);
                shipmentPM.Notify1AddressId = addressRepository.GetMainAddressId(card.Id, shipmentPM.Tenant);
                shipmentPM.Notify1ContactId = card.PrimaryContactId;
            }
        }
        private void InitOceanOrInlandPackages(Direct entity)
        {
            foreach (OceanOrInlandPackage item in entity.OceanOrInlandPackages)
            {
                if (item.PackageType != null)
                {
                    if (entity.ShipmentType != null && (entity.ShipmentType.Code.Contains("FCL") || entity.ShipmentType.Code.Contains("FTL")))
                    {
                        if (item.Pieces == null || item.Pieces == 0)
                        {
                            item.Pieces = 1;
                        }
                    }
                }
            }

            if (entity.ShipmentType != null)
            {
                if (this.IsInlandDomesticShipment(entity))
                {
                    entity.OceanOrInlandPackages = null;
                }
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
                Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(entityPM.IncotermId, entityPM.Tenant);
                if (myIncoterm != null)
                {
                    entityPM.FreightPrepaidCollectId = myIncoterm.Freight;
                    entityPM.OtherPrepaidCollectId = myIncoterm.OtherCharges;
                }
            }
        }
    }
}
