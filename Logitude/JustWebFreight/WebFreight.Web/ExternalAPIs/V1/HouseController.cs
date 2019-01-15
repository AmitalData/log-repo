using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
                bool exist = SecurityUtility.CheckFeature("General", "EXTERNALAPIS", tenant);
                if (!exist)
                {

                }
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
                bool exist = SecurityUtility.CheckFeature("General", "EXTERNALAPIS", tenant);
                if (!exist)
                {

                }

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
                            if(item.ChargesType == null)
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
                    HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);
                    ShipmentPM entityPM = mappingService.HouseCustomDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        switch(entityPM.DirectionId)
                        {
                            case "I":
                                {
                                    if(string.IsNullOrEmpty(entityPM.ConsigneeId))
                                    {
                                        throw new ApplicationException("Consignee is required for import houses");
                                    }

                                    break;
                                }

                            case "E":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for export houses");
                                    }

                                    break;
                                }

                            case "D":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for domestic houses");
                                    }

                                    break;
                                }

                            case "R":
                                {
                                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                                    {
                                        throw new ApplicationException("Shipper is required for drop houses");
                                    }

                                    break;
                                }
                        }

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
                        if (!string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.CustomerId, authToken.Tenant);
                            if (address != null)
                            {
                                entityPM.CustomerAddressId = address.Id;
                            }

                            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                            Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                            if (customer != null)
                            {
                                entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                                entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                            }
                        }

                        else
                        {
                            throw new ApplicationException("Customer is missing");
                        }
                        
                        if (!string.IsNullOrEmpty(entityPM.ShipperId))
                        {
                            Address address = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, authToken.Tenant);
                            if(address != null)
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
            switch(type)
            {
                case "R":
                    {
                        if(!chargesType.IsReceivable)
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

            switch(temp.TransportModeId)
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

        public HttpResponseMessage Put(House entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        string token = HttpContext.Current.Request.Headers["Token"];
            //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            //        ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
            //        string computingPartnerCode = "";
            //        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode)) computingPartnerCode = entity.ComputingPartnerCode;//loggedContactInfo.ComputingPartnerCode;

            //        IShipmentsContext MyContext = ShipmentsContext.GetContext(authToken.Tenant);
            //        HouseQueryService mappingService = new HouseQueryService(authToken.Tenant);
            //        ShipmentPM entityPM = mappingService.HouseDataMappingAndValidatin(entity, authToken.Tenant, computingPartnerCode);

            //        using (TransactionScope scope = TransactionFactory.GetTransaction())
            //        {
            //            ShipmentRepository entityRepository = new ShipmentRepository(MyContext);
            //            Shipment entityPoco = null;
            //            entityPoco = entityRepository.GetSingleShipment(entityPM.Id, authToken.Tenant);

            //            if (entityPoco == null)
            //            {
            //                throw new ApplicationException("No shipment found");
            //            }

            //            else
            //            {
            //                if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId) || entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
            //                {
            //                    this.CheckEntityChanges(entityPM, entityPoco, MyContext, authToken.Tenant);
            //                }

            //                else
            //                {
            //                    this.DoUpdate(entityPM, MyContext, authToken.Tenant);
            //                }
            //            }


            //            scope.Complete();
            //        }

            //        var result = mappingService.GetHouseById(entityPM.Id, authToken.Tenant);
            //        APIHelper.AddCommunicationLog("D", entity, result, "Shipment", entityPM.Id, "House API", authToken.Tenant);
            //        return Request.CreateResponse(HttpStatusCode.OK, result);
            //    }

            //    catch (Exception ex)
            //    {
            //        var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
            //        APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            //        return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            //    }
            //}

            //else
            //{
            //    var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
            //    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Shipment", null, "House API");
            //    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);

            //}
        }

        private void CheckEntityChanges(ShipmentPM entityPM, Shipment entityPoco, IShipmentsContext context, int tenant)
        {
            if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId) && !entityPM.IsOperationalClosed && !entityPM.IsAccountingClosed && !entityPM.IsCancelled)
            {
                if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this house is connected to master");
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this house is connected to master");
                }

                else
                {
                    this.DoUpdate(entityPM, context, tenant);
                }
            }

            else if (entityPM.IsOperationalClosed || entityPM.IsAccountingClosed || entityPM.IsCancelled)
            {
                string message = "closed";
                if (entityPM.IsCancelled)
                {
                    message = "cancelled";
                }
                else if(entityPM.IsAccountingClosed)
                {
                    message = "accounting closed";
                }
                else if (entityPM.IsOperationalClosed)
                {
                    message = "operational closed";
                }

                if (entityPM.ShipperId != entityPoco.ShipperId)
                {
                    throw new ApplicationException("You can't change shipper, because this house is " + message);
                }

                else if (entityPM.ConsigneeId != entityPoco.ConsigneeId)
                {
                    throw new ApplicationException("You can't change consignee, because this house is " + message);
                }

                else if (entityPM.CustomerId != entityPoco.CustomerId)
                {
                    throw new ApplicationException("You can't change customer, because this house is " + message);
                }

                else if (entityPM.FromPortId != entityPoco.FromPortId)
                {
                    throw new ApplicationException("You can't change from port, because this house is " + message);
                }

                else if (entityPM.House != entityPoco.House)
                {
                    throw new ApplicationException("You can't change house, because this house is " + message);
                }

                else if (entityPM.IncotermId != entityPoco.IncotermId)
                {
                    throw new ApplicationException("You can't change incoterm, because this house is " + message);
                }

                else if (entityPM.ToPortId != entityPoco.ToPortId)
                {
                    throw new ApplicationException("You can't change to port, because this house is " + message);
                }

                else if (entityPM.ChargeableWeightUnitCode != entityPoco.ChargeableWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Chargeable Weight Unit, because this house is " + message);
                }

                else if (entityPM.GrossWeightUnitCode != entityPoco.GrossWeightUnitCode)
                {
                    throw new ApplicationException("You can't change Gross Weight Unit, because this house is " + message);
                }

                else if (entityPM.VolumeUnitCode != entityPoco.VolumeUnitCode)
                {
                    throw new ApplicationException("You can't change Volume Unit, because this house is " + message);
                }

                else if (entityPM.HAWBDate != entityPoco.HAWBDate)
                {
                    throw new ApplicationException("You can't change HAWB Date, because this house is " + message);
                }

                else if (entityPM.DescriptionOfGoods != entityPoco.DescriptionOfGoods)
                {
                    throw new ApplicationException("You can't change Description Of Goods, because this house is " + message);
                }

                else if (entityPM.BranchId != entityPoco.BranchId)
                {
                    throw new ApplicationException("You can't change Branch, because this house is " + message);
                }

                else if (entityPM.DepartmentId != entityPoco.DepartmentId)
                {
                    throw new ApplicationException("You can't change Department, because this house is " + message);
                }

                else
                {
                    ShipmentPackageRepository packageRepository = new ShipmentPackageRepository(entityPM.Tenant);
                    List<ShipmentPackage> packages = packageRepository.GetShipmentPackagesForShipmentTenant(entityPM.Id, entityPM.Tenant).ToList();

                    if (packages.Count < entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't add packages, because this house is " + message);
                    }

                    else if (packages.Count > entityPM.ShipmentPackages.Count)
                    {
                        throw new ApplicationException("You can't delete packages, because this house is " + message);
                    }

                    else
                    {
                        foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                        {
                            ShipmentPackage item = packages.Where(d => d.Id == itemPM.Id).FirstOrDefault();

                            if (itemPM.PackageTypeId != item.PackageTypeId)
                            {
                                throw new ApplicationException("You can't change Package Type, because this house is " + message);
                            }

                            else if (itemPM.Length != item.Length)
                            {
                                throw new ApplicationException("You can't change Length, because this house is " + message);
                            }

                            else if (itemPM.Width != item.Width)
                            {
                                throw new ApplicationException("You can't change Width, because this house is " + message);
                            }

                            else if (itemPM.Height != item.Height)
                            {
                                throw new ApplicationException("You can't change Height, because this house is " + message);
                            }

                            else if (itemPM.Quantity != item.Quantity)
                            {
                                throw new ApplicationException("You can't change Quantity, because this house is " + message);
                            }

                            else if (itemPM.Volume != item.Volume)
                            {
                                throw new ApplicationException("You can't change Volume, because this house is " + message);
                            }

                            else if (itemPM.Weight != item.Weight)
                            {
                                throw new ApplicationException("You can't change Weight, because this house is " + message);
                            }

                            else if (itemPM.Reference1 != item.Reference1)
                            {
                                throw new ApplicationException("You can't change Reference 1, because this house is " + message);
                            }

                            else if (itemPM.Reference2 != item.Reference2)
                            {
                                throw new ApplicationException("You can't change Reference 2, because this house is " + message);
                            }

                            else if (itemPM.Reference3 != item.Reference3)
                            {
                                throw new ApplicationException("You can't change Reference 3, because this house is " + message);
                            }

                            else if (itemPM.CommodityNumber != item.CommodityNumber)
                            {
                                throw new ApplicationException("You can't change Commodity Number, because this house is " + message);
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId))
                            {
                                if (itemPM.ShipperSeal != item.ShipperSeal)
                                {
                                    throw new ApplicationException("You can't change Shipper Seal, because this house is " + message);
                                }

                                else if (itemPM.CarrierSeal != item.CarrierSeal)
                                {
                                    throw new ApplicationException("You can't change Carrier Seal, because this house is " + message);
                                }
                            }

                            if (!string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.ShipmentTypeId != "FCLD")
                            {
                                if (itemPM.Harmonize != item.Harmonize)
                                {
                                    throw new ApplicationException("You can't change Length, because this house is " + message);
                                }

                                else if (itemPM.Temperature != item.Temperature)
                                {
                                    throw new ApplicationException("You can't change Temperature, because this house is " + message);
                                }

                                else if (itemPM.Ventilation != item.Ventilation)
                                {
                                    throw new ApplicationException("You can't change Ventilation, because this house is " + message);
                                }

                                else if (itemPM.IsDangerous != item.IsDangerous)
                                {
                                    throw new ApplicationException("You can't change Is Dangerous, because this house is " + message);
                                }

                                else if (itemPM.ClassNumber != item.ClassNumber)
                                {
                                    throw new ApplicationException("You can't change Class Number, because this house is " + message);
                                }

                                else if (itemPM.UnNumber != item.UnNumber)
                                {
                                    throw new ApplicationException("You can't change Un Number, because this house is " + message);
                                }

                                else if (itemPM.PackagingGroup != item.PackagingGroup)
                                {
                                    throw new ApplicationException("You can't change Packaging Group, because this house is " + message);
                                }

                                else if (itemPM.IMDGCode != item.IMDGCode)
                                {
                                    throw new ApplicationException("You can't change IMDG Code, because this house is " + message);
                                }

                                else if (itemPM.FlashPoint != item.FlashPoint)
                                {
                                    throw new ApplicationException("You can't change Flash Point, because this house is " + message);
                                }

                                else if (itemPM.MaterialDescription != item.MaterialDescription)
                                {
                                    throw new ApplicationException("You can't change Material Description, because this house is " + message);
                                }
                            }

                            if (entityPM.ShipmentTypeId == "FCLD")
                            {
                                if (itemPM.Tare != item.Tare)
                                {
                                    throw new ApplicationException("You can't change Tare, because this house is " + message);
                                }

                                else if (itemPM.MarksAndNumbers != item.MarksAndNumbers)
                                {
                                    throw new ApplicationException("You can't change Marks And Numbers, because this house is " + message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DoUpdate(ShipmentPM entityPM, IShipmentsContext MyContext, int tenant)
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

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (customer != null)
                {
                    entityPM.SalesmanUserId = string.IsNullOrEmpty(customer.SalesmanUserId) ? entityPM.CreatedByUserId : customer.SalesmanUserId;
                    entityPM.AccountManagerUserId = !string.IsNullOrEmpty(customer.Customer.AccountManagerUserId) ? customer.Customer.AccountManagerUserId : entityPM.CreatedByUserId;
                }
            }

            if (entityPM.ShipmentPackages.Count > 0)
            {
                foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                {
                    item.Volume = ComputeHelper.ComputeVolume(item, entityPM);
                    item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                }
            }

            ComputeHelper.ComputeTotals(entityPM);

            ShipmentService service = new ShipmentService(MyContext, entityPM, SecurityUtility.GetAuthenticatedUser());
            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(MyContext));
            List<ShipmentPackagePM> shipmentPackages = shipPackageQuery.GetShipmentPackages(entityPM.Id, entityPM.ShipmentNumber, tenant);
            foreach (ShipmentPackagePM package in shipmentPackages)
            {
                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                entityPM.ShipmentPackages.Add(package);
            }

            service.Update(true);
        }
    }
}