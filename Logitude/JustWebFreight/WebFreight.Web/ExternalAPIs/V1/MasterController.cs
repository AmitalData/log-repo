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

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class MasterController : ApiController
    {
        public HttpResponseMessage GetSingleMaster(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                MasterQueryService Service = new MasterQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetMasterById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
        //public HttpResponseMessage GetSingleMasterByNumber(string number)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        int tenant = authToken.Tenant;
        //        MasterQueryService Service = new MasterQueryService(tenant);
        //        ServiceResponse response = new ServiceResponse();
        //        var Result = Service.GetMasterByMaster(number, tenant);
        //        //string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
        //        return Request.CreateResponse(HttpStatusCode.OK, Result);
        //    }
        //    catch (Exception ex)
        //    {
        //        var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
        //        return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        //    }
        //}
        public HttpResponseMessage Post(Master entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                    string computingPartnerCode = "";
                    if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                    {
                        computingPartnerCode = entity.ComputingPartnerCode;//loggedContactInfo.ComputingPartnerCode;
                    }

                    if (entity.Receivables != null && entity.Receivables.Count > 0)
                    {
                        foreach (Receivable item in entity.Receivables)
                        {
                            if (item.ChargesType == null)
                            {
                                throw new ApplicationException("Receivable Charges Type is required");
                            }

                            if (item.Measurement == null)
                            {
                                throw new ApplicationException("Receivable Measurement is required");
                            }

                            if (item.Currency == null)
                            {
                                throw new ApplicationException("Receivable Currency is required");
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

                            if (item.Measurement == null)
                            {
                                throw new ApplicationException("Payable Measurement is required");
                            }

                            if (item.Currency == null)
                            {
                                throw new ApplicationException("Payable Currency is required");
                            }
                        }
                    }

                    IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
                    MasterQueryService mappingService = new MasterQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.MasterCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

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
                            AirlineRepository airlineRepository = new AirlineRepository(authToken.Tenant);
                            Airline airline = airlineRepository.GetSingleAirline(entityPM.MainCarriageCarrierId, authToken.Tenant);
                            if(airline != null)
                            {
                                entityPM.CarrierIsCheckDigit = airline.CheckDigit;
                                entityPM.CarrierIsLimitedLength = airline.LimitedLength;
                            }
                        }
                    }

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    { 
                        if (entity.IsOperationalClosed)
                        {
                            string errorMessage = "";

                            RulesValidator validator = new RulesValidator();
                            validator.Initialize(authToken.Tenant);
                            List<ObjectTableRuleField> requiredFields = validator.ValidateAllRequiredFieldRules(entityPM, "Shipment", authToken.Tenant);

                            IWebFreightContext webFreightContext = WebFreightContext.GetContext(authToken.Tenant);
                            ObjectFieldRepository ObjectFieldRepository = new ObjectFieldRepository(webFreightContext);
                            if (requiredFields.Count > 0)
                            {
                                foreach (ObjectTableRuleField field in requiredFields)
                                {
                                    ObjectField f = ObjectFieldRepository.GetSingleObjectFieldById(field.ObjectFieldId, authToken.Tenant);
                                    errorMessage = errorMessage + ", " + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                                }
                            }

                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = errorMessage.TrimStart(',');
                                throw new ApplicationException("Due to operational closed: " + errorMessage);
                            }

                            entityPM.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                        }

                        if (entity.IsAccountingClosed)
                        {
                            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(authToken.Tenant);
                            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(authToken.Tenant);

                            bool hasOpenPayables = false;
                            bool hasOpenReceivables = false;
                            if (entity.Receivables.Count() > 0)
                            {
                                if(entity.Receivables.Where(p => p.Amount != null && p.Amount != 0).Any())                               
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
                                entityPM.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant);
                            }
                        }
                        
                        if (entity.Houses.Count > 0)
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(authToken.Tenant);
                            foreach (House item in entity.Houses)
                            {
                                bool isValid = true;
                                Shipment myDataBaseShipment = null;

                                if (!string.IsNullOrEmpty(item.Id))
                                {
                                    myDataBaseShipment = shipmentRepository.GetSingleShipment(item.Id, authToken.Tenant);
                                }

                                if (myDataBaseShipment == null)
                                {
                                    if (!string.IsNullOrEmpty(item.ShipmentNumber))
                                    {
                                        myDataBaseShipment = shipmentRepository.GetSingleShipmentByShipmentNumber(item.ShipmentNumber, authToken.Tenant);
                                    }
                                }

                                if (myDataBaseShipment == null)
                                {
                                    isValid = false;
                                    throw new ApplicationException("Shipment with ShipmentNumber " + item.ShipmentNumber + " doesn't exist");
                                }

                                if (!string.IsNullOrEmpty(item.Id) && !string.IsNullOrEmpty(item.ShipmentNumber))
                                {
                                    if (myDataBaseShipment != null)
                                    {
                                        if (myDataBaseShipment.ShipmentNumber != item.ShipmentNumber)
                                        {
                                            isValid = false;
                                            throw new ApplicationException("The sent Id and Shipment Number are not matching");
                                        }
                                    }
                                }

                                else if (myDataBaseShipment.ShipmentLevelCode != "H")
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment is not house");
                                }

                                else if (!string.IsNullOrEmpty(myDataBaseShipment.MasterShipmentDataId))
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment connected to another master");
                                }

                                else if (myDataBaseShipment.DirectionId != entityPM.DirectionId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment direction not matches the master direction");
                                }

                                else if (myDataBaseShipment.TransportModeId != entityPM.TransportModeId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment transport mode not matches the master transport mode");
                                }

                                else if (myDataBaseShipment.FromPortId != entityPM.FromPortId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment from port not matches the master from port");
                                }

                                else if (myDataBaseShipment.ToPortId != entityPM.ToPortId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment to port not matches the master to port");
                                }

                                else if (myDataBaseShipment.BranchId != entityPM.BranchId)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent shipment branch not matches the master branch");
                                }

                                else if (myDataBaseShipment.IsCancelled)
                                {
                                    isValid = false;
                                    throw new ApplicationException("You can't connect cancelled house");
                                }

                                else if (myDataBaseShipment.IsOperationalClosed)
                                {
                                    isValid = false;
                                    throw new ApplicationException("You can't connect operational closed house");
                                }

                                else if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
                                {
                                    switch (entityPM.ShipmentTypeId.ToUpper())
                                    {
                                        case "MYGO":
                                            {
                                                if (myDataBaseShipment.ShipmentTypeId != "LCLD")
                                                {
                                                    isValid = false;
                                                    throw new ApplicationException("The sent shipment type should be LCL");
                                                }
                                                break;
                                            }

                                        case "MYGI":
                                            {
                                                if (myDataBaseShipment.ShipmentTypeId != "LTL")
                                                {
                                                    isValid = false;
                                                    throw new ApplicationException("The sent shipment type should be LTL");
                                                }
                                                break;
                                            }
                                    }
                                }

                                bool hasOpenPayables = false;
                                bool hasOpenReceivables = false;
                                if (entity.IsAccountingClosed && entity.IsOperationalClosed)
                                {
                                    List<ShipmentReceivable> houseReceivables = MyContext.ShipmentReceivables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();
                                    List<ShipmentPayable> housePayables = MyContext.ShipmentPayables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();

                                    if (!hasOpenReceivables)
                                    {
                                        #region
                                        if (houseReceivables.Count > 0)
                                        {
                                            foreach (ShipmentReceivable recitem in houseReceivables)
                                            {
                                                if (recitem.ShipmentReceivableLineStatusCode != "ACCT" && recitem.ShipmentReceivableLineStatusCode != "EMPT")
                                                {
                                                    if (recitem.TotalAmount != null && recitem.TotalAmount != 0)
                                                    {
                                                        hasOpenReceivables = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }

                                    if (!hasOpenPayables)
                                    {
                                        #region
                                        if (housePayables.Count > 0)
                                        {
                                            foreach (ShipmentPayable payaitem in housePayables)
                                            {
                                                if (payaitem.ShipmentPayableLineStatusCode != "ACCT" && payaitem.ShipmentPayableLineStatusCode != "EMPT" && payaitem.ShipmentPayableParentId == null)
                                                {
                                                    if (payaitem.ShipmentPayableAmountTypeCode == "NEXP")
                                                    {
                                                        if (payaitem.AccountedAmount != null && payaitem.AccountedAmount != 0)
                                                        {
                                                            hasOpenPayables = true;
                                                            break;
                                                        }
                                                    }

                                                    else
                                                    {
                                                        if (payaitem.ExpectedAmount != null && payaitem.ExpectedAmount != 0)
                                                        {
                                                            hasOpenPayables = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }

                                if (hasOpenPayables || hasOpenReceivables)
                                {
                                    isValid = false;
                                    throw new ApplicationException("Can’t close for accounting: House #" + myDataBaseShipment.ShipmentNumber + " has open receivables/ payables");
                                }

                                if (isValid)
                                {
                                    // connect to master
                                    ConsoleShipmentPM myConsole = new ConsoleShipmentPM()
                                    {
                                        Id = myDataBaseShipment.Id,
                                    };

                                    entityPM.ShipmentConsoleShipments.Add(myConsole);
                                }
                            }
                        }
                        
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

                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        if (!string.IsNullOrEmpty(entityPM.AgentId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.AgentId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.AgentAddressId = address.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(entityPM.ShipperId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.ShipperAddressId = address.Id;
                            }
                        }

                        if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.ConsigneeAddressId = address.Id;
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
                        this.ValidateReceivablesAndPayables(entityPM, authToken.Tenant);
                        ComputeHelper.ComputeReceivablesPayablesTotals(entityPM);

                        ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
                        service.Create();

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
                                }

                                entityRepository.Update(item);
                            }

                            entityRepository.SubmitChanges();
                        }
                        scope.Complete();
                    }

                    var result = mappingService.GetMasterById(entityPM.Id, authToken.Tenant);
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

        private void ValidateReceivablesAndPayables(ShipmentPM temp, int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM MyTenantPM = tenantQuery.GetSinglePM(tenant);

            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
            MeasurementRepository measurementRepository = new MeasurementRepository(tenant);

            #region Receivables
            foreach (ShipmentReceivablePM item in temp.ShipmentReceivables)
            {
                item.CreatedByUserId = temp.CreatedByUserId;
                item.UpdateByUserId = temp.UpdatedByUserId;
                item.ShipmentReceivableLineStatusCode = "EMPT";

                if (!string.IsNullOrEmpty(item.ChargesTypeId))
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.ChargesType chargesType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant);
                    if (chargesType != null)
                    {
                        this.ValidateChargesType(chargesType, temp, "R");

                        item.DueTypeCode = chargesType.DueTypeCode;
                        item.VatTypeId = chargesType.VatTypeId;
                        item.IATACodeId = chargesType.IATACodeId;
                        item.IsExpense = chargesType.IsExpense;

                        if (string.IsNullOrEmpty(item.PrepaidCollectId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT")
                            {
                                item.PrepaidCollectId = temp.FreightPrepaidCollectId;
                            }

                            else
                            {
                                item.PrepaidCollectId = temp.OtherPrepaidCollectId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.CurrencyId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                            {
                                item.CurrencyId = MyTenantPM.FreightCurrencyId;
                            }

                            else
                            {
                                item.CurrencyId = MyTenantPM.OtherChargesCurrencyId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(temp.TransportModeId, temp.ShipmentTypeId))
                            {
                                item.MeasurementId = chargesType.MeasurementId;
                            }

                            else
                            {
                                item.MeasurementId = chargesType.ContainerMeasurementId != null ? chargesType.ContainerMeasurementId : chargesType.MeasurementId;
                            }
                        }
                    }
                }

                RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                if (item.Rate == null)
                {
                    if (MyTenantPM.CurrencyId == item.CurrencyId)
                    {
                        item.Rate = 1;
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(MyTenantPM.CurrencyId) && !string.IsNullOrEmpty(item.CurrencyId))
                        {
                            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, MyTenantPM.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                            if (lastRate != null && lastRate.Rate != 0)
                            {
                                item.Rate = lastRate.Rate;
                            }
                        }
                    }
                }

                if (item.ProfitCurrencyExchangeRate == null)
                {
                    if (MyTenantPM.CurrencyId == temp.ProfitCurrencyId)
                    {
                        item.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, temp.ProfitCurrencyId, MyTenantPM.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                        if (lastRate != null)
                        {
                            item.ProfitCurrencyExchangeRate = lastRate.Rate;
                        }
                    }
                }


                string measurementCode = "";
                Simplog.Data.CommonDataModel.EntityPOCOs.Measurement measurement = measurementRepository.GetSingleMeasurement(item.MeasurementId, tenant);
                if (measurement != null)
                {
                    measurementCode = measurement.Code;
                }

                if (item.Quantity == null)
                {
                    switch (measurementCode)
                    {
                        case "GRWT": { item.Quantity = temp.GrossWeight; break; }
                        case "CHWT": { item.Quantity = temp.ChargeableWeight; break; }
                        case "VOLU": { item.Quantity = temp.Volume; break; }
                        case "BTEU": { item.Quantity = temp.TEU; break; }
                        case "FIXD": { item.Quantity = 1; break; }
                        case "GWTN": { item.Quantity = temp.GrossWeightPerTon; break; }
                        case "QTY": { item.Quantity = MethodHelper.IsLCLEntity(temp.TransportModeId, temp.ShipmentTypeId) ? temp.NumberOfPackages : temp.NumberOfContainers; break; }

                        case "PRVL":
                            {
                                item.Quantity = temp.ValueOfGoods;
                                break;
                            }

                        case "PRFR":
                            {
                                item.Quantity = temp.ShipmentReceivables.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.TotalAmount);
                                break;
                            }
                    }
                }

                if (item.Quantity != null && item.UnitPrice != null)
                {
                    if (item.ShipmentReceivableLineStatusCode != "OAMT")
                    {
                        item.ShipmentReceivableLineStatusCode = "OAMT";
                    }
                }

                else
                {
                    if (item.ShipmentReceivableLineStatusCode != "EMPT")
                    {
                        item.ShipmentReceivableLineStatusCode = "EMPT";
                    }
                }

                if (item.TotalAmount == null)
                {
                    double? iAmount = null;
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        if (measurementCode == "PRVL" || measurementCode == "PRFR")
                        {
                            double? price = item.UnitPrice / 100;
                            iAmount = item.Quantity * price;
                        }

                        else
                        {
                            iAmount = item.Quantity * item.UnitPrice;
                        }
                    }

                    /* MinMax */
                    if (iAmount != null)
                    {
                        if (item.QuoteSaleMinAmount != null)
                        {
                            if (iAmount < item.QuoteSaleMinAmount)
                            {
                                iAmount = item.QuoteSaleMinAmount;
                            }
                        }

                        if (item.QuoteSaleMaxAmount != null)
                        {
                            if (iAmount > item.QuoteSaleMaxAmount)
                            {
                                iAmount = item.QuoteSaleMaxAmount;
                            }
                        }
                    }

                    if (iAmount != null)
                    {
                        item.TotalAmount = ComputeHelper.Round(iAmount.Value, 2);
                    }
                }

                else
                {
                    double? price = null;
                    if (item.Quantity != null)
                    {
                        if (item.Quantity == 0)
                        {
                            price = 0;
                        }

                        else
                        {
                            price = item.TotalAmount / item.Quantity;
                        }
                    }

                    if (price != null)
                    {
                        item.UnitPrice = ComputeHelper.Round(price.Value, 3);
                    }
                }

                if (item.TotalAmount != null && item.Rate != null)
                {
                    item.TotalAmountLocal = ComputeHelper.Round(item.TotalAmount.Value * item.Rate.Value, 2);
                }

                if (item.CurrencyId == temp.ProfitCurrencyId)
                {
                    item.AmountInProfitCurrency = item.TotalAmount;
                }

                else
                {
                    item.AmountInProfitCurrency = (item.TotalAmountLocal / item.ProfitCurrencyExchangeRate);
                }
            }
            #endregion

            #region Payables
            foreach (ShipmentPayablePM item in temp.ShipmentPayables)
            {
                item.CreatedByUserId = temp.CreatedByUserId;
                item.UpdateByUserId = temp.UpdatedByUserId;
                item.ShipmentPayableLineStatusCode = "EMPT";
                item.ShipmentPayableAmountTypeCode = "ACCU";
                item.ShipmentPayableAmountTypeName = "Accrual";

                if (!string.IsNullOrEmpty(item.ChargesTypeId))
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.ChargesType chargesType = chargesTypeRepository.GetSingleChargesType(item.ChargesTypeId, tenant);
                    if (chargesType != null)
                    {
                        this.ValidateChargesType(chargesType, temp, "P");

                        item.DueTypeCode = chargesType.DueTypeCode;
                        item.VatTypeId = chargesType.VatTypeId;
                        item.IATACodeId = chargesType.IATACodeId;

                        if (string.IsNullOrEmpty(item.PrepaidCollectId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT")
                            {
                                item.PrepaidCollectId = temp.FreightPrepaidCollectId;
                            }

                            else
                            {
                                item.PrepaidCollectId = temp.OtherPrepaidCollectId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.CurrencyId))
                        {
                            if (chargesType.ChargesGroupCode == "FRT" || chargesType.ChargesGroupCode == "SCH")
                            {
                                item.CurrencyId = MyTenantPM.FreightCurrencyId;
                            }

                            else
                            {
                                item.CurrencyId = MyTenantPM.OtherChargesCurrencyId;
                            }
                        }

                        if (string.IsNullOrEmpty(item.MeasurementId))
                        {
                            if (MethodHelper.IsLCLEntity(temp.TransportModeId, temp.ShipmentTypeId))
                            {
                                item.MeasurementId = chargesType.MeasurementId;
                            }

                            else
                            {
                                item.MeasurementId = chargesType.ContainerMeasurementId != null ? chargesType.ContainerMeasurementId : chargesType.MeasurementId;
                            }
                        }
                    }
                }

                RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                if (item.Rate == null)
                {
                    if (MyTenantPM.CurrencyId == item.CurrencyId)
                    {
                        item.Rate = 1;
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(MyTenantPM.CurrencyId) && !string.IsNullOrEmpty(item.CurrencyId))
                        {
                            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, MyTenantPM.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                            if (lastRate != null && lastRate.Rate != 0)
                            {
                                item.Rate = lastRate.Rate;
                            }
                        }
                    }
                }

                if (item.ProfitCurrencyExchangeRate == null)
                {
                    if (MyTenantPM.CurrencyId == temp.ProfitCurrencyId)
                    {
                        item.ProfitCurrencyExchangeRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, temp.ProfitCurrencyId, MyTenantPM.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                        if (lastRate != null)
                        {
                            item.ProfitCurrencyExchangeRate = lastRate.Rate;
                        }
                    }
                }


                string measurementCode = "";
                Simplog.Data.CommonDataModel.EntityPOCOs.Measurement measurement = measurementRepository.GetSingleMeasurement(item.MeasurementId, tenant);
                if (measurement != null)
                {
                    measurementCode = measurement.Code;
                }

                if (item.Quantity == null)
                {
                    switch (measurementCode)
                    {
                        case "GRWT": { item.Quantity = temp.GrossWeight; break; }
                        case "CHWT": { item.Quantity = temp.ChargeableWeight; break; }
                        case "VOLU": { item.Quantity = temp.Volume; break; }
                        case "BTEU": { item.Quantity = temp.TEU; break; }
                        case "FIXD": { item.Quantity = 1; break; }
                        case "GWTN": { item.Quantity = temp.GrossWeightPerTon; break; }
                        case "QTY": { item.Quantity = MethodHelper.IsLCLEntity(temp.TransportModeId, temp.ShipmentTypeId) ? temp.NumberOfPackages : temp.NumberOfContainers; break; }

                        case "PRVL":
                            {
                                item.Quantity = temp.ValueOfGoods;
                                break;
                            }

                        case "PRFR":
                            {
                                item.Quantity = temp.ShipmentReceivables.Where(d => d.ChargesGroupCode == "FRT").Sum(s => s.TotalAmount);
                                break;
                            }
                    }
                }

                string StatusCode = "EMPT";
                if (item.ShipmentPayableAmountTypeCode == "NEXP")
                {
                    StatusCode = "ACCT";
                }

                else if (item.Quantity == null || item.UnitPrice == null)
                {
                    StatusCode = "EMPT";
                }

                else
                {
                    if (item.OpenAmount == null)
                    {
                        item.OpenAmount = 0;
                    }

                    if (item.AccountedAmount == null)
                    {
                        item.AccountedAmount = 0;
                    }

                    if (item.OpenAmount != 0 && item.AccountedAmount != 0)
                    {
                        StatusCode = "PACC";
                    }

                    else if (item.OpenAmount != 0)
                    {
                        StatusCode = "OAMT";
                    }

                    else if (item.AccountedAmount != 0)
                    {
                        StatusCode = "ACCT";
                    }
                }

                if (StatusCode == "EMPT")
                {
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        StatusCode = "OAMT";
                    }
                }

                item.ShipmentPayableLineStatusCode = StatusCode;

                if (item.ExpectedAmount == null)
                {
                    double? iAmount = null;
                    if (item.Quantity != null && item.UnitPrice != null)
                    {
                        if (measurementCode == "PRVL" || measurementCode == "PRFR")
                        {
                            double? price = item.UnitPrice / 100;
                            iAmount = item.Quantity * price;
                        }

                        else
                        {
                            iAmount = item.Quantity * item.UnitPrice;
                        }
                    }

                    if (iAmount != null)
                    {
                        item.ExpectedAmount = ComputeHelper.Round(iAmount.Value, 2);
                    }
                }

                else
                {
                    double? price = null;
                    if (item.Quantity != null)
                    {
                        if (item.Quantity == 0)
                        {
                            price = 0;
                        }

                        else
                        {
                            price = item.ExpectedAmount / item.Quantity;
                        }
                    }

                    if (price != null)
                    {
                        item.UnitPrice = ComputeHelper.Round(price.Value, 3);
                    }
                }

                if (item.ExpectedAmount != null && item.Rate != null)
                {
                    item.ExpectedAmountLocal = ComputeHelper.Round(item.ExpectedAmount.Value * item.Rate.Value, 2);
                }

                if (item.CurrencyId == temp.ProfitCurrencyId)
                {
                    item.ExpectedAmountInProfitCurrency = item.ExpectedAmount;
                }

                else
                {
                    item.ExpectedAmountInProfitCurrency = (item.ExpectedAmountLocal / item.ProfitCurrencyExchangeRate);
                }

                if (item.ShipmentPayableLineStatusCode == "EMPT" || item.ShipmentPayableLineStatusCode == "OAMT")
                {
                    item.OpenAmount = item.ExpectedAmount;
                    item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                    item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                }
            }
            #endregion
        }

        private void ValidateChargesType(ChargesType chargesType, ShipmentPM temp, string type)
        {
            switch (type)
            {
                case "R":
                    {
                        if (!chargesType.IsReceivable)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in receivables should be marked as Receivable");
                        }
                        break;
                    }

                case "P":
                    {
                        if (!chargesType.IsPayable)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in payables should be marked as Payable");
                        }
                        break;
                    }
            }

            switch (temp.TransportModeId)
            {
                case "A":
                    {
                        if (!chargesType.IsAir)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Air shipments should be marked as Air");
                        }
                        break;
                    }

                case "I":
                    {
                        if (!chargesType.IsInland)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Inland shipments should be marked as Inland");
                        }
                        break;
                    }

                case "O":
                    {
                        if (!chargesType.IsOcean)
                        {
                            throw new ApplicationException("Charge type " + chargesType.Code + " used in Ocean shipments should be marked as Ocean");
                        }
                        break;
                    }
            }
        }

        public HttpResponseMessage Put(Master entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "Master API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }
    }
}