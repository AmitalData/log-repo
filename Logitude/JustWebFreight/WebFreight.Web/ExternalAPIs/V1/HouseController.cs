using Logitude.BL.CommonDataModel.APIDataContract.QueryService;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
using WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class HouseController : ApiController
    {
        public HttpResponseMessage GetSingleHouse(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                HouseQueryService Service = new HouseQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetHouseById(id, tenant);
                //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleHouseByNumber(string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticateAPICall(tenant);

                HouseQueryService Service = new HouseQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetHouseByShipmentNumber(number, tenant);
                //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(House entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);

                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                    string computingPartnerCode = "";
                    if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                    {
                        computingPartnerCode = entity.ComputingPartnerCode;
                    }

                    ExternalAPIXMLEntityValidator externalAPIXMLEntityValidator = new ExternalAPIXMLEntityValidator(authToken.Tenant);
                    externalAPIXMLEntityValidator.ValidateHouseEntity(entity, MyContext);
                    this.InitOceanOrInlandPackages(entity);

                    UnassignedDataQueryService unassignedDataQueryService = new UnassignedDataQueryService(authToken.Tenant, computingPartnerCode);
                    entity = unassignedDataQueryService.HandleUnassignedHouseShipmentData(entity);

                    HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.HouseCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);
                    entityPM.IsExternalAPI = true;

                    ExternalAPIShipmentValidator externalAPIShipmentValidator = new ExternalAPIShipmentValidator(entityPM, authToken.Tenant);
                    externalAPIShipmentValidator.ValidateUnitCodes();
                    externalAPIShipmentValidator.ValidatePickupDeliveryPackages();
                    externalAPIShipmentValidator.ValidatePartnersDueToDirection();

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        this.SetPrepaidCollectIds(entityPM);

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        this.ValidateAndSetCustomerData(entityPM, addressRepository, authToken.Tenant);
                        
                        if (entityPM.ShipmentPackages.Count > 0)
                        {
                            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                            {
                                bool hasContainerInsidePackages = false;
                                if (entityPM.ShipmentTypeId == "FCLD" || entityPM.ShipmentTypeId == "FTL")
                                {
                                    item.IsContainer = true;
                                }

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
                                       

                                        hasContainerInsidePackages = true;
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
                                if (!hasContainerInsidePackages)
                                {
                                    item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                                }
                                item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                                hasContainerInsidePackages = false;
                            }
                        }

                        ComputeHelper.ComputeTotals(entityPM);

                        APIReceivablePayableHelper receivablePayableHelper = new APIReceivablePayableHelper(entityPM, authToken.Tenant);
                        receivablePayableHelper.ValidateReceivablesAndPayables();
                        receivablePayableHelper.ComputeReceivablesPayablesTotals();
                        
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, authToken.Tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);
                        }
                     
                        if (entityPM.CustomsClearanceDate != null)
                        {
                            entityPM.IncludesCustoms = true;
                        }

                        ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(entityPM, authToken.Tenant);
                        externalAPIMainCarriageLegsHelper.ValidateRoutingsSeriesDates();

                        entityPM.HasUnassignedData = unassignedDataQueryService.HasUnassignedData;
                        entityPM = unassignedDataQueryService.AddHouseShipmentUnassignedAddress(entity, entityPM);

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

                        scope.Complete();
                    }
                    
                    var result = mappingService.GetHouseById(entityPM.Id, authToken.Tenant);
          
                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "House API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private void InitOceanOrInlandPackages(House entity)
        {
            if (entity.OceanOrInlandPackages == null)
            {
                return;
            }
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
        }

        private void ValidateAndSetCustomerData(ShipmentPM entityPM, AddressRepository addressRepository, int tenant)
        {
            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                if (entityPM.CustomerId == entityPM.ShipperId
                    || entityPM.CustomerId == entityPM.ConsigneeId
                    || entityPM.CustomerId == entityPM.ShipperNotExporterId
                    || entityPM.CustomerId == entityPM.AgentId
                    || entityPM.CustomerId == entityPM.CustomAgentImportId
                    || entityPM.CustomerId == entityPM.ReleasingAgentId
                    || entityPM.CustomerId == entityPM.FreightForwarderId)
                {
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
                    throw new ApplicationException("The sent customer is not one of the sent partners");
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

        public HttpResponseMessage Put(House entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    if (FeatureToggleHelper.HasFeatureToggle("API", authToken.Tenant))
                    {
                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);

                        UnassignedDataQueryService unassignedDataQueryService = new UnassignedDataQueryService(authToken.Tenant, "");
                        entity = unassignedDataQueryService.HandleUnassignedHouseShipmentData(entity);

                        ShipmentPM HousePM = mappingService.HouseDataMappingAndValidatin(entity, authToken.Tenant, "", true);

                        if (HousePM != null)
                        {
                            HousePM.ConcurrencyGUID = entity.ConcurrencyGUID;
                            HousePM.IsExternalAPI = true;

                            if (HousePM.IsOperationalClosed)
                            {
                                throw new ApplicationException("Can't update operationally closed shipments");
                            }

                            if (HousePM.IsCancelled)
                            {
                                throw new ApplicationException("Can't update cancelled shipments");
                            }

                            if (!string.IsNullOrEmpty(HousePM.MasterShipmentDataId))
                            {
                                throw new ApplicationException("Can't update house connected to master");
                            }

                            if (HousePM.CustomsClearanceDate != null && HousePM.IncludesCustoms == false)
                            {
                                HousePM.IncludesCustoms = true;
                            }

                            ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(HousePM, authToken.Tenant);
                            externalAPIMainCarriageLegsHelper.ValidateRoutingsSeriesDates();

                            AddressRepository addressRepository = new AddressRepository(authToken.Tenant);
                            this.ValidateAndSetCustomerData(HousePM, addressRepository, authToken.Tenant);

                            this.UpdatePartners(MyContext, HousePM);

                            HousePM.HasUnassignedData = unassignedDataQueryService.HasUnassignedData;
                            HousePM = unassignedDataQueryService.AddHouseShipmentUnassignedAddress(entity, HousePM);

                            ShipmentService service = new ShipmentService(MyContext, HousePM, SecurityUtility.GetAuthenticatedUser());
                            service.Update(true);
                        }

                        MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        mappingService = new HouseQueryService(authToken.Tenant);
                        var result = mappingService.GetHouseById(HousePM.Id, authToken.Tenant);
                        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", HousePM.Id, "House API", authToken.Tenant);
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
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        
        private void UpdatePartners(IShipmentsContext shipmentsContext, ShipmentPM shipmentPM)
        {
            Shipment shipmentPOCO = shipmentsContext.Shipments.Where(d => d.Id == shipmentPM.Id && d.Tenant == shipmentPM.Tenant).FirstOrDefault();
            if (shipmentPOCO != null)
            {
                this.UpdateNotify1Partner(shipmentPOCO, shipmentPM);
            }
        }
        private void UpdateNotify1Partner(Shipment shipmentPOCO, ShipmentPM shipmentPM)
        {
            if (shipmentPOCO.Notify1Id != shipmentPM.Notify1Id)
            {
                Card card = CardRepository.GetSingleCard(shipmentPM.Notify1Id, shipmentPM.Tenant, false);
                this.MapNotify1Fields(shipmentPM, card);
            }
        }
        private void MapNotify1Fields(ShipmentPM shipmentPM, Card card)
        {
            if (card != null)
            {
                AddressRepository addressRepository = new AddressRepository(shipmentPM.Tenant);
                shipmentPM.Notify1AddressId = addressRepository.GetMainAddressId(card.Id, shipmentPM.Tenant);
                shipmentPM.Notify1ContactId = card.PrimaryContactId;
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