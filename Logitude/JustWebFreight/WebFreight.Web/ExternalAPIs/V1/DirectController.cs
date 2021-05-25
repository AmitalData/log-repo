using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using WebFreight.Web.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Validating;

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

                        if (entity.TransportMode != null && entity.ShipmentType != null)
                        {
                            if (!IsInlandDomesticShipment(entity))
                            {
                                if (entity.TransportMode.Code != "A")
                                {
                                    if (entity.OceanOrInlandPackages != null && entity.OceanOrInlandPackages.Count > 0)
                                    {
                                        foreach (OceanOrInlandPackage item in entity.OceanOrInlandPackages)
                                        {
                                            if (item.InsidePackages != null && item.InsidePackages.Count > 0)
                                            {
                                                foreach (InsidePackage itemInside in item.InsidePackages)
                                                {
                                                    if (itemInside.PackageType != null)
                                                    {
                                                        Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PackageTypeQueryService PackageTypeService0 = new Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PackageTypeQueryService(authToken.Tenant);
                                                        PackageTypePM PackageTypePM = PackageTypeService0.PackageTypeDataMappingAndValidatin(itemInside.PackageType, authToken.Tenant);
                                                        if (PackageTypePM != null)
                                                        {
                                                            if (PackageTypePM.IsContainer == true)
                                                            {
                                                                throw new ApplicationException("Invalid Inside Package Type Code");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            itemInside.PackageType = null;
                                                        }
                                                    }
                                                }
                                            }

                                            if (item.PackageType == null)
                                            {
                                                string message = entity.ShipmentType.Code.Contains("LCL") ? "Package Type is required" : "Container Type is required";
                                                throw new ApplicationException(message);
                                            }

                                            else
                                            {
                                                if (entity.ShipmentType.Code.Contains("FCL") || entity.ShipmentType.Code.Contains("FTL"))
                                                {
                                                    if (item.Pieces == null || item.Pieces == 0)
                                                    {
                                                        item.Pieces = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }else
                            {
                                entity.OceanOrInlandPackages = null;
                            }
                        }

                        if (entity.TransportMode != null && entity.TransportMode.Code != "A")
                        {
                            if (entity.ShipmentType == null || (entity.ShipmentType != null && string.IsNullOrEmpty(entity.ShipmentType.Code)))
                            {
                                throw new ApplicationException("Missing Shipment Type");
                            }
                        }

                        if (entity.Receivables != null && entity.Receivables.Count > 0)
                        {
                            foreach (Receivable item in entity.Receivables)
                            {
                                if (item.ChargesType == null)
                                {
                                    throw new ApplicationException("Receivable Charges Type is required");
                                }

                                if (item.Currency == null)
                                {
                                    string currancy = null;

                                    if (item.ChargesType != null)
                                    {
                                        currancy = CheckReceivablesChargesTypeCurrency(item.ChargesType.Code, authToken.Tenant);
                                    }

                                    if (string.IsNullOrEmpty(currancy))
                                    {
                                        throw new ApplicationException("Receivable Currency is required");
                                    }
                                }
                            }
                        }

                        if (entity.Payables != null && entity.Payables.Count > 0)
                        {
                            foreach (Payable item in entity.Payables)
                            {
                                if (item.ChargesType == null)
                                {
                                    throw new ApplicationException("Payable Charges Type is required");
                                }


                                if (item.Currency == null)
                                {
                                    string currancy = null;

                                    if (item.ChargesType != null)
                                    {
                                        currancy = CheckPayablesChargesTypeCurrency(item.ChargesType.Code, authToken.Tenant);
                                    }

                                    if (string.IsNullOrEmpty(currancy))
                                    {
                                        throw new ApplicationException("Payable Currency is required");
                                    }
                                }
                            }
                        }
                        
                        if(entity.FromPort != null && entity.ToPort != null)
                        {
                            if(entity.MainCarriageLegs != null && entity.MainCarriageLegs.Count > 0)
                            {
                                throw new ApplicationException("You can't use the From Port/ To Port with the Main Carriage Legs");
                            }
                        }
                        else
                        {
                            if (entity.MainCarriageLegs == null || entity.MainCarriageLegs.Count == 0)
                            {
                                throw new ApplicationException("You must send the From Port/ To Port or the Main Carriage Legs");
                            }
                        }
                        
                        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                        DirectQueryService mappingService = new DirectQueryService(authToken.Tenant);
                        ShipmentPM entityPM = mappingService.DirectCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);
                        entityPM.IsExternalAPI = true;

                        this.ValidateUnitCodes(entityPM);
                        this.ValidateAirShipmentCarrier(entityPM, authToken.Tenant);
                        this.ValidateShipmentClosure(entity, entityPM, authToken.Tenant);

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        //this.SetPartnersAddresses(entityPM, addressRepository, authToken.Tenant);
                        this.ValidateAndSetCustomerData(entityPM, addressRepository, authToken.Tenant);

                      

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

                                    if (item.Width != null || item.Height != null || item.Length != null)
                                    {
                                        item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
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
                            APITransshipmentHelper aPITransshipmentHelper = new APITransshipmentHelper(entityPM, authToken.Tenant);
                            aPITransshipmentHelper.ValidateTransshipments();
                            aPITransshipmentHelper.MapTransshipments();
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
        private void ValidateUnitCodes(ShipmentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.VolumeUnitCode))
            {
                throw new ApplicationException("Missing volume unit code");
            }

            if (string.IsNullOrEmpty(entityPM.DimensionsUnitCode))
            {
                throw new ApplicationException("Missing dimensions unit code");
            }

            if (string.IsNullOrEmpty(entityPM.GrossWeightUnitCode))
            {
                throw new ApplicationException("Missing gross weight unit code");
            }

            if (string.IsNullOrEmpty(entityPM.ChargeableWeightUnitCode))
            {
                throw new ApplicationException("Missing chargeable weight unit code");
            }

            switch (entityPM.VolumeUnitCode)
            {
                case "CBF":
                    {
                        if (entityPM.DimensionsUnitCode == "Cm")
                        {
                            throw new ApplicationException("When volume unit is CBF, dimensions unit should be Inch or Ft");
                        }
                        break;
                    }

                case "CBI":
                    {
                        if (entityPM.DimensionsUnitCode != "Inc")
                        {
                            throw new ApplicationException("When volume unit is CBI, dimensions unit should be Inch");
                        }
                        break;
                    }

                case "CBM":
                    {
                        if (entityPM.DimensionsUnitCode != "Cm")
                        {
                            throw new ApplicationException("When volume unit is CBM, dimensions unit should be Cm");
                        }
                        break;
                    }
            }
        }
        private void ValidateAirShipmentCarrier(ShipmentPM entityPM, int tenant)
        {
            if (entityPM.TransportModeId == "A")
            {
                if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
                {
                    if (!string.IsNullOrEmpty(entityPM.Master) || !string.IsNullOrEmpty(entityPM.MainCarriageCarrierNumber))
                    {
                        throw new ApplicationException("Missing Main carriage carrier");
                    }
                }

                else
                {
                    AirlineRepository airlineRepository = new AirlineRepository(tenant);
                    Airline airline = airlineRepository.GetSingleAirline(entityPM.MainCarriageCarrierId, tenant);
                    if (airline != null)
                    {
                        entityPM.CarrierIsCheckDigit = airline.CheckDigit;
                        entityPM.CarrierIsLimitedLength = airline.LimitedLength;
                    }
                }
            }
        }
        private void ValidateShipmentClosure(Direct entity, ShipmentPM entityPM, int tenant)
        {
            if (entity.IsOperationalClosed)
            {
                string errorMessage = "";

                RulesValidator validator = new RulesValidator();
                validator.Initialize(tenant);
                List<ObjectTableRuleField> requiredFields = validator.ValidateAllRequiredFieldRules(entityPM, "Shipment", tenant);

                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                ObjectFieldRepository ObjectFieldRepository = new ObjectFieldRepository(webFreightContext);
                if (requiredFields.Count > 0)
                {
                    foreach (ObjectTableRuleField field in requiredFields)
                    {
                        ObjectField f = ObjectFieldRepository.GetSingleObjectFieldByCode(field.ObjectFieldCode, tenant);
                        errorMessage = errorMessage + ", " + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                    }
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = errorMessage.TrimStart(',');
                    throw new ApplicationException("Due to operational closed: " + errorMessage);
                }

                entityPM.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            }

            if (entity.IsAccountingClosed)
            {
                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(tenant);

                bool hasOpenPayables = false;
                bool hasOpenReceivables = false;
                if (entity.Receivables.Count() > 0)
                {
                    if (entity.Receivables.Where(p => p.Amount != null && p.Amount != 0).Any())
                    {
                        hasOpenReceivables = true;
                    }
                }

                if (accountingSetting != null && !accountingSetting.AllowClosureWithoutPayables)
                {
                    if (entity.Payables.Count() > 0)
                    {
                        if (entity.Payables.Where(p => p.Amount != null && p.Amount != 0).Any())
                        {
                            hasOpenPayables = true;
                        }
                    }
                }

                if (hasOpenPayables || hasOpenReceivables)
                {
                    throw new ApplicationException("can’t close for accounting if there are any open payables/receivables.");
                }

                if (!entity.IsOperationalClosed)
                {
                    throw new ApplicationException("Shipment shoud be closed operationally");
                }
                else
                {
                    entityPM.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }
            }
        }

        // Ayman Handled in Shipment Service (keep it DRY)
        //private void SetPartnersAddresses(ShipmentPM entityPM, AddressRepository addressRepository, int tenant)
        //{
        //    if (!string.IsNullOrEmpty(entityPM.ShipperId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.ShipperAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.ConsigneeAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.ShipperNotExporterId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperNotExporterId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.ShipperNotExporterAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.AgentId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.AgentId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.AgentAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.CustomAgentImportId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.CustomAgentImportId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.CustomAgentImportAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.ReleasingAgentId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.ReleasingAgentId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.ReleasingAgentAddressId = address.Id;
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(entityPM.FreightForwarderId))
        //    {
        //        Address address = addressRepository.GetMainAddressByCardId(entityPM.FreightForwarderId, tenant);
        //        if (address != null)
        //        {
        //            entityPM.FreightForwarderAddressId = address.Id;
        //        }
        //    }
        //}
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
                
                if(string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    throw new ApplicationException("The customer is required");
                }
            }            
        }
        private void SetCustomerTypeCode(ShipmentPM entityPM)
        {
            if(entityPM.CustomerId == entityPM.ShipperId)
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

        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }

        private bool IsInlandDomesticShipment(Direct entityPM)
        {
            bool isInland = false;
            bool isDomestic = false;
            if (entityPM.Direction != null)
            {
                isDomestic = entityPM.Direction.Code == "D" ? true : false;
            }
            if (entityPM.TransportMode != null)
            {
                isInland = entityPM.TransportMode.Code == "I" ? true : false;
            }
            return isDomestic && isInland;
        }

        private ShipmentPM ValidateInlandDomesticShipment(ShipmentPM entityPM)
        {

            entityPM = SetInlandDomesticShipmentPartners(entityPM);
            ValidateInlandDomesticShipmentPartnersAddesses(entityPM);
            ValidateInlandDomesticMainCarriageDates(entityPM);

            return entityPM;
        }
        private ShipmentPM SetInlandDomesticShipmentPartners(ShipmentPM entityPM)
        {

            if (entityPM.MainCarriageToPartnerId == null)
            {
                entityPM.MainCarriageToPartnerId = entityPM.ConsigneeId;
            }
            if (entityPM.MainCarriageFromPartnerId == null)
            {
                entityPM.MainCarriageFromPartnerId = entityPM.ShipperId;
            }
            return entityPM;

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

        private void ValidateInlandDomesticShipmentPartnersAddesses(ShipmentPM entityPM)
        {
            List<DomesticCountry> iDomesticCountries = new List<DomesticCountry>();
            AddDomesticAddress(iDomesticCountries, entityPM.MainCarriageFromAddressId, entityPM.Tenant);
            AddDomesticAddress(iDomesticCountries, entityPM.MainCarriageToAddressId, entityPM.Tenant);

            if (iDomesticCountries.GroupBy(g => g.CountryId).Count() > 1)
            {
                bool isAllPortsEC = iDomesticCountries.Where(d => d.CountryIsEC == false).Any() ? false : true;
                bool isAllPortsNA = iDomesticCountries.Where(d => d.CountryIsNorthAmerica == false).Any() ? false : true;

                if (!isAllPortsEC && !isAllPortsNA)
                {
                    throw new ApplicationException("Both Addresses must be in the same country since the direction is Domestic");
                }
            }
        }

        private void AddDomesticAddress(List<DomesticCountry> iDomesticCountries, string iAddressId, int iTenant)
        {
            if (!string.IsNullOrEmpty(iAddressId))
            {
                if (!iDomesticCountries.Where(d => d.Id == iAddressId).Any())
                {
                    AddressRepository addressRepository = new AddressRepository(iTenant);
                    Address iAddress = addressRepository.GetSingleAddress(iAddressId, iTenant);

                    if (iAddress != null)
                    {
                        iDomesticCountries.Add(new DomesticCountry()
                        {
                            Id = iAddress.Id,
                            CountryId = iAddress.CountryId,
                            CountryIsEC = iAddress.Country.EC,
                            CountryIsNorthAmerica = iAddress.Country.IsNorthAmerica,
                        });
                    }
                }
            }
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

        public HttpResponseMessage Put(Direct entity)
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
                                APITransshipmentHelper aPITransshipmentHelper = new APITransshipmentHelper(directPM, authToken.Tenant);
                                aPITransshipmentHelper.ValidateTransshipments();
                                aPITransshipmentHelper.MapTransshipments();

                                AddressRepository addressRepository = new AddressRepository(authToken.Tenant);
                                this.ValidateAndSetCustomerData(directPM, addressRepository, authToken.Tenant);
                            }
                            else
                            {
                                directPM = this.ValidateInlandDomesticShipment(directPM);
                            }
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

        private void MapDirectToEntityPM(ShipmentPM directPM, ShipmentPM entityPM, IShipmentsContext MyContext)
        {
            entityPM.ShipmentLevelCode = directPM.ShipmentLevelCode;
            entityPM.DirectionId = directPM.DirectionId;
            entityPM.TransportModeId = directPM.TransportModeId;
            entityPM.ShipperId = directPM.ShipperId;
            entityPM.ShipperReference1 = directPM.ShipperReference1;
            entityPM.ShipperReference2 = directPM.ShipperReference2;
            entityPM.ConsigneeId = directPM.ConsigneeId;
            entityPM.ConsigneeReference1 = directPM.ConsigneeReference1;
            entityPM.ConsigneeReference2 = directPM.ConsigneeReference2;
            entityPM.CustomerId = directPM.CustomerId;
            entityPM.FromPortId = directPM.FromPortId;
            entityPM.ToPortId = directPM.ToPortId;
            entityPM.GrossWeightUnitCode = directPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = directPM.ChargeableWeightUnitCode;
            entityPM.VolumeUnitCode = directPM.VolumeUnitCode;
            entityPM.IncotermCode = directPM.IncotermCode;
            entityPM.IncotermId = directPM.IncotermId;
            entityPM.DescriptionOfGoods = directPM.DescriptionOfGoods;
            entityPM.ShipmentPackages = directPM.ShipmentPackages;
            entityPM.BranchId = directPM.BranchId;
            entityPM.BranchId = directPM.BranchId;
            entityPM.DepartmentId = directPM.DepartmentId;
            entityPM.TEU = directPM.TEU;
            entityPM.NumberOfPackages = directPM.NumberOfPackages;
            entityPM.GrossWeight = directPM.GrossWeight;
            entityPM.Volume = directPM.Volume;
            entityPM.VolumetricWeight = directPM.VolumetricWeight;
            entityPM.ChargeableWeight = directPM.ChargeableWeight;
            entityPM.Master = directPM.Master;
            entityPM.MainCarriageCarrierCode = directPM.MainCarriageCarrierCode;
            entityPM.MainCarriageCarrierId = directPM.MainCarriageCarrierId;
            entityPM.MainCarriageFromPortId = directPM.MainCarriageFromPortId;
            entityPM.MainCarriageToPortId = directPM.MainCarriageToPortId;

        }

        private void RemovePackages(ShipmentPM directPM, ShipmentPM entityPM, IShipmentsContext MyContext)
        {
            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(MyContext));
            entityPM.ShipmentPackages = shipPackageQuery.GetShipmentPackages(entityPM.Id, entityPM.ShipmentNumber, entityPM.Tenant);
            foreach (ShipmentPackagePM package in entityPM.ShipmentPackages)
            {
                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }

            if (directPM.ShipmentPackages.Count > 0)
            {
                foreach (ShipmentPackagePM package in directPM.ShipmentPackages)
                {
                    package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    entityPM.ShipmentPackages.Add(package);
                }
            }

        }

        private void CheckEntityChanges(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (entityPM.IsOperationalClosed)
            {
                if (entityPM.ShipperId != entityPoco.ShipperId)
                {
                    throw new ApplicationException("You can't change shipper, because this direct is closed");
                }

                else if (entityPM.ConsigneeId != entityPoco.ConsigneeId)
                {
                    throw new ApplicationException("You can't change consignee, because this direct is closed");
                }

                else if (entityPM.CustomerId != entityPoco.CustomerId)
                {
                    throw new ApplicationException("You can't change customer, because this direct is closed");
                }

                else if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this direct is closed");
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this direct is closed");
                }

                else if (entityPM.ChargeableWeightUnitCode != entityPoco.ChargeableWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Chargeable Weight Unit, because this direct is closed");
                }

                else if (entityPM.GrossWeightUnitCode != entityPoco.GrossWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Gross Weight Unit, because this direct is closed");
                }

                else if (entityPM.VolumeUnitCode != entityPoco.VolumeUnitCode)
                {
                    throw new ApplicationException("You can't change Volume Unit, because this direct is closed");
                }

                else if (entityPM.HAWBDate != entityPoco.HAWBDate)
                {
                    throw new ApplicationException("You can't change HAWB Date, because this direct is closed");
                }

                else if (entityPM.DescriptionOfGoods != entityPoco.DescriptionOfGoods)
                {
                    throw new ApplicationException("You can't change Description Of Goods, because this direct is closed");
                }

                else if (entityPM.BranchId != entityPoco.BranchId)
                {
                    throw new ApplicationException("You can't change Branch, because this direct is closed");
                }

                else if (entityPM.DepartmentId != entityPoco.DepartmentId)
                {
                    throw new ApplicationException("You can't change Department, because this direct is closed");
                }

                else
                {
                    ShipmentPackageRepository packageRepository = new ShipmentPackageRepository(entityPM.Tenant);
                    List<ShipmentPackage> packages = packageRepository.GetShipmentPackagesForShipmentTenant(entityPM.Id, entityPM.Tenant).ToList();

                    if (packages.Count < entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't add packages, because this direct is closed");
                    }

                    else if (packages.Count > entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't delete packages, because this direct is closed");
                    }

                    //else
                    //{
                    //    foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                    //    {
                    //        ShipmentPackage item = packages.Where(d => d.Id == itemPM.Id).FirstOrDefault();

                    //        if (itemPM.PackageTypeId != item.PackageTypeId)
                    //        {
                    //            throw new ApplicationException("You can't change Package Type, because this direct is closed");
                    //        }

                    //        else if (itemPM.Length != item.Length)
                    //        {
                    //            throw new ApplicationException("You can't change Length, because this direct is closed");
                    //        }

                    //        else if (itemPM.Width != item.Width)
                    //        {
                    //            throw new ApplicationException("You can't change Width, because this direct is closed");
                    //        }

                    //        else if (itemPM.Height != item.Height)
                    //        {
                    //            throw new ApplicationException("You can't change Height, because this direct is closed");
                    //        }

                    //        else if (itemPM.Quantity != item.Quantity)
                    //        {
                    //            throw new ApplicationException("You can't change Quantity, because this direct is closed");
                    //        }

                    //        else if (itemPM.Volume != item.Volume)
                    //        {
                    //            throw new ApplicationException("You can't change Volume, because this direct is closed");
                    //        }

                    //        else if (itemPM.Weight != item.Weight)
                    //        {
                    //            throw new ApplicationException("You can't change Weight, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference1 != item.Reference1)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 1, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference2 != item.Reference2)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 2, because this direct is closed");
                    //        }

                    //        else if (itemPM.Reference3 != item.Reference3)
                    //        {
                    //            throw new ApplicationException("You can't change Reference 3, because this direct is closed");
                    //        }

                    //        else if (itemPM.CommodityNumber != item.CommodityNumber)
                    //        {
                    //            throw new ApplicationException("You can't change Commodity Number, because this direct is closed");
                    //        }

                    //        if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode))
                    //        {
                    //            if (itemPM.Seal != item.Seal)
                    //            {
                    //                throw new ApplicationException("You can't change Seal, because this direct is closed");
                    //            }

                    //            else if (itemPM.Seal2 != item.Seal2)
                    //            {
                    //                throw new ApplicationException("You can't change Seal 2, because this direct is closed");
                    //            }
                    //        }

                    //        if (!string.IsNullOrEmpty(entityPM.ShipmentLevelCode) && entityPM.ShipmentLevelCode != "FCLD")
                    //        {
                    //            if (itemPM.Harmonize != item.Harmonize)
                    //            {
                    //                throw new ApplicationException("You can't change Length, because this direct is closed");
                    //            }

                    //            else if (itemPM.Temperature != item.Temperature)
                    //            {
                    //                throw new ApplicationException("You can't change Temperature, because this direct is closed");
                    //            }

                    //            else if (itemPM.Ventilation != item.Ventilation)
                    //            {
                    //                throw new ApplicationException("You can't change Ventilation, because this direct is closed");
                    //            }

                    //            else if (itemPM.IsDangerous != item.IsDangerous)
                    //            {
                    //                throw new ApplicationException("You can't change Is Dangerous, because this direct is closed");
                    //            }

                    //            else if (itemPM.ClassNumber != item.ClassNumber)
                    //            {
                    //                throw new ApplicationException("You can't change Class Number, because this direct is closed");
                    //            }

                    //            else if (itemPM.UnNumber != item.UnNumber)
                    //            {
                    //                throw new ApplicationException("You can't change Un Number, because this direct is closed");
                    //            }

                    //            else if (itemPM.PackagingGroup != item.PackagingGroup)
                    //            {
                    //                throw new ApplicationException("You can't change Packaging Group, because this direct is closed");
                    //            }

                    //            else if (itemPM.IMDGCode != item.IMDGCode)
                    //            {
                    //                throw new ApplicationException("You can't change IMDG Code, because this direct is closed");
                    //            }

                    //            else if (itemPM.FlashPoint != item.FlashPoint)
                    //            {
                    //                throw new ApplicationException("You can't change Flash Point, because this direct is closed");
                    //            }

                    //            else if (itemPM.MaterialDescription != item.MaterialDescription)
                    //            {
                    //                throw new ApplicationException("You can't change Material Description, because this direct is closed");
                    //            }
                    //        }

                    //        if (entityPM.ShipmentLevelCode == "FCLD")
                    //        {
                    //            if (itemPM.Tare != item.Tare)
                    //            {
                    //                throw new ApplicationException("You can't change Tare, because this direct is closed");
                    //            }

                    //            else if (itemPM.MarksAndNumbers != item.MarksAndNumbers)
                    //            {
                    //                throw new ApplicationException("You can't change Marks And Numbers, because this direct is closed");
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            else if (entityPM.IsAccountingClosed)
            {
                throw new ApplicationException("Shipment Is Accounting closed, you can't do any change");
            }

            else if (entityPM.IsCancelled)
            {
                throw new ApplicationException("Shipment Is cancelled, you can't do any change");
            }
        }

        private string CheckReceivablesChargesTypeCurrency(string chargeTypeCode, int tenant)
        {
            string currency = null;
            if (!string.IsNullOrEmpty(chargeTypeCode))
            {
                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
                var chergeType = chargesTypeRepository.GetSingleChargesTypeByCode(chargeTypeCode, tenant);
                if (chergeType != null && !string.IsNullOrEmpty(chergeType.ReceivablesDefaultCurrencyId))
                {
                    currency = chergeType.ReceivablesDefaultCurrencyId;
                }
            }
            return currency;
        }

        private string CheckPayablesChargesTypeCurrency(string chargeTypeCode, int tenant)
        {
            string currency = null;
            if (!string.IsNullOrEmpty(chargeTypeCode))
            {
                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
                var chergeType = chargesTypeRepository.GetSingleChargesTypeByCode(chargeTypeCode, tenant);
                if (chergeType != null && !string.IsNullOrEmpty(chergeType.PayablesDefaultCurrencyId))
                {
                    currency = chergeType.PayablesDefaultCurrencyId;
                }
            }
            return currency;
        }
    }
}
