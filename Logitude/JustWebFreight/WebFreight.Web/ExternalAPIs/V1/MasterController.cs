using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebFreight.Web.DataContracts;
using System.Net;
using System.Web.Http;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using WebFreight.Web.Helpers.APIHelpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Logitude.Server.Tools.Helpers;
using System.Reflection;
using SilverlightExpressions;
using WebFreight.Web.Validators;
using Simplog.Data.Helpers;
using WebFreight.Web.ShipmentsModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class MasterController : ApiController
    {
        public HttpResponseMessage GetSingleMaster(string id, string include)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				MasterQueryService Service = new MasterQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetMasterById(id, tenant, include);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage GetSingleMasterByNumber(string number, string include)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                MasterQueryService Service = new MasterQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetMasterByShipmentNumber(number, tenant, include);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Master entity)
        {
            if (ModelState.IsValid)
            {
                try
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
                    externalAPIXMLEntityValidator.ValidateMasterEntity(entity);                    

                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                    MasterQueryService mappingService = new MasterQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.MasterCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);
                    entityPM.IsExternalAPI = true;

                    ExternalAPIShipmentValidator externalAPIShipmentValidator = new ExternalAPIShipmentValidator(entityPM, authToken.Tenant);
                    externalAPIShipmentValidator.ValidateUnitCodes();
                    externalAPIShipmentValidator.ValidateAirShipmentCarrier();
                    externalAPIShipmentValidator.ValidateShipmentClosure();
                    externalAPIShipmentValidator.ValidatePickupDeliveryPackages();
                    externalAPIShipmentValidator.ValidateConnectedHouses(entity, MyContext);

                    this.SetClosurePropertiers(entityPM);
                    this.SetMasterNumberProperties(entityPM);
                    this.SetPrepaidCollectIds(entityPM);

                    if (entityPM.CustomsClearanceDate != null)
                    {
                        entityPM.IncludesCustoms = true;
                    }


                    using (TransactionScope scope = TransactionFactory.GetTransaction())
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

                        if (entityPM.ShipmentPackages.Count > 0)
                        {
                            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                            {
                                item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                                item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                            }
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

                        if (entity.AddManualEvents != null && entity.AddManualEvents.Count > 0)
                        {
                            EventQueryService eventQueryService = new EventQueryService(authToken.Tenant);
                            eventQueryService.CreateShipmentTraceEvents(entityPM, entity.AddManualEvents, true, computingPartnerCode);
                        }

                        ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
                        List<Shipment> allHouses = entityRepository.GetHouseShipmentsForMaster(entityPM.Id, authToken.Tenant);

                        if (allHouses.Count > 0)
                        {
                            ShipmentComputedFieldsRepository shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(MyContext);
                            ShipmentComputedFields entityComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(entityPM.Id, entityPM.Tenant);
                            if(entityComputedFields != null)
                            {
                                entityComputedFields.NumberOfHouses = allHouses.Count;
                                shipmentComputedFieldsRepository.Update(entityComputedFields);
                                shipmentComputedFieldsRepository.SubmitChanges();
                            }

                            foreach (Shipment item in allHouses)
                            {
                                if (entityPM.IsOperationalClosed)
                                {
                                    item.IsOperationalClosed = true;
                                    item.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);

                                    if (item.FirstOperationalCloseDate == null)
                                    {
                                        item.FirstOperationalCloseDate = item.OperationalCloseDate;
                                    }
                                }

                                if (entityPM.IsAccountingClosed)
                                {
                                    item.IsAccountingClosed = true;
                                    item.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);

                                    if (item.FirstAccountingCloseDate == null)
                                    {
                                        item.FirstAccountingCloseDate = item.AccountingCloseDate;
                                    }
                                }

                                entityRepository.Update(item);
                            }

                            entityRepository.SubmitChanges();
                        }
                        scope.Complete();
                    }

                    var result = mappingService.GetMasterById(entityPM.Id, authToken.Tenant, null);
                    APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "Master API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        
        public HttpResponseMessage Put(Master entity)
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
                        MasterQueryService mappingService = new MasterQueryService(authToken.Tenant);
                        ShipmentPM MasterPM = mappingService.MasterDataMappingAndValidatin(entity, authToken.Tenant, "", true);

                        if (MasterPM != null)
                        {
                            MasterPM.ConcurrencyGUID = entity.ConcurrencyGUID;
                            MasterPM.IsExternalAPI = true;

                            if (MasterPM.IsOperationalClosed)
                            {
                                throw new ApplicationException("Can't update operationally closed shipments");
                            }

                            if (MasterPM.IsCancelled)
                            {
                                throw new ApplicationException("Can't update cancelled shipments");
                            }

                            if (MasterPM.CustomsClearanceDate != null && MasterPM.IncludesCustoms == false)
                            {
                                MasterPM.IncludesCustoms = true;
                            }

                            ExternalAPIMainCarriageLegsHelper externalAPIMainCarriageLegsHelper = new ExternalAPIMainCarriageLegsHelper(MasterPM, authToken.Tenant);
                            externalAPIMainCarriageLegsHelper.ValidateMainCarriageLegs();
                            externalAPIMainCarriageLegsHelper.MapTransshipments();

                            this.UpdatePartners(MyContext, MasterPM);

                            ShipmentService service = new ShipmentService(MyContext, MasterPM, SecurityUtility.GetAuthenticatedUser());
                            service.Update(true);

                            if (entity.AddManualEvents != null && entity.AddManualEvents.Count > 0)
                            {
                                EventQueryService eventQueryService = new EventQueryService(authToken.Tenant);
                                eventQueryService.CreateShipmentTraceEvents(MasterPM, entity.AddManualEvents, true, "");
                            }
                        }

                        MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        mappingService = new MasterQueryService(authToken.Tenant);
                        var result = mappingService.GetMasterById(MasterPM.Id, authToken.Tenant, null);
                        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", MasterPM.Id, "Master API", authToken.Tenant);
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
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private void ValidateMasterNumberAndCarrier(Master entity)
        {
            bool validate = false;
            if (!string.IsNullOrEmpty(entity.MasterNumber))
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