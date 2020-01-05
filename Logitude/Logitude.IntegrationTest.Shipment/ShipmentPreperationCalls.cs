using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment
{
    class ShipmentPreperationCalls
    {
        public static async Task PrepareVariables()
        {
            ShipmentVariables.CurrencyEURId = await GetCurrencyId("GBP");
            ShipmentVariables.IncotermLDEId = await GetIncotermId("LDE");
            ShipmentVariables.MeasurmentGRWTId = await GetMeasurmentId("GRWT");
            var chargeGroup = new ChargesGroupList();
            chargeGroup = await GetChargeGroup("COMM");
            ShipmentVariables.ChargeGroupCOMMId = chargeGroup.Id;
            ShipmentVariables.ChargeGroupCOMMCode = chargeGroup.Code;
            ShipmentVariables.ChargeTypeAFTId = await GetChargeTypeId("AFT");

            ShipmentVariables.PortLHRId = await GetPortId("LHR");
            ShipmentVariables.PortMIAId = await GetPortId("MIA");
            ShipmentVariables.PortJFKId = await GetPortId("JFK");
            ShipmentVariables.PortSOUId = await GetPortId("SOU");
            ShipmentVariables.PortNYCId = await GetPortId("NYC");
            ShipmentVariables.PortLONId = await GetPortId("LON");
            ShipmentVariables.PortMANId = await GetPortId("MAN");
            ShipmentVariables.GlobalZoneEUId = await GetGlobalZoneId("EU");
            ShipmentVariables.CountryGBId = await GetCountrytId("GB");
            ShipmentVariables.CountryUSId = await GetCountrytId("US");
            ShipmentVariables.StateAKId = await GetStateId("AK");
            ShipmentVariables.AirlineAAId = await GetAirlineId("AA");
            ShipmentVariables.AirlineBAId = await GetAirlineId("BA");
            ShipmentVariables.ShippingLineMSCUId = await GetShippingLineId("MSCU");
            ShipmentVariables.ShippingLineMAEUId = await GetShippingLineId("MAEU");
            ShipmentVariables.MoveTypeMTAId = await GetMoveTypeId("MCD", "A");
            ShipmentVariables.MoveTypeMTOId = await GetMoveTypeId("MTO", "O");
            ShipmentVariables.VesselPTId = await GetVesselId("PT");
            ShipmentVariables.PackageTypePC1Id = await GetPackageTypeId("PC1", "O", true);
            ShipmentVariables.PackageTypePC2Id = await GetPackageTypeId("PC2", "O", true);
            ShipmentVariables.PackageTypePP1Id = await GetPackageTypeId("PP1", "A", false);
            ShipmentVariables.PackageTypePP2Id = await GetPackageTypeId("PP2", "A", false);
            ShipmentVariables.PaymentTermCashId = await GetPaymentTermId("Cash");
            ShipmentVariables.VATTypeZeroId = await GetVATTypeId("ZERO");
            ShipmentVariables.QuoteStageQTDRId = await GetQuoteStageId("QuTDR");


        }
        public static async Task<string> GetCurrencyId(string currencyCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("currencyviews" + QueryFiltersPreparation.GetUrlParameters(currencyCode, QueryFiltersPreparation.QueryfilterByCode(currencyCode)));
            CurrencyList currentTenantCurrencyList = RestClientService.ParseResponse<CurrencyList>(response);
            if (currentTenantCurrencyList == null)
            {
                currentTenantCurrencyList = await GetCurrencyFromTenant0(currencyCode);
            }
            return currentTenantCurrencyList.Id;
        }
        public static async Task<CurrencyList> GetCurrencyFromTenant0(string currencyCode)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("currencyviews/getbyfilters?Tenant=0&PageSize=50&Filter1Name=Code&Filter1Operator=equals&Filter1Value=" + currencyCode);
            CurrencyList tenantZeroCurrencyList = RestClientService.ParseResponse<CurrencyList>(respnse);
            CurrencyList currentTenantCurrencyList = new CurrencyList();
            if (tenantZeroCurrencyList != null)
            {
                currentTenantCurrencyList = await CopyCurrencyToTenant(tenantZeroCurrencyList.Id);
            }
            return currentTenantCurrencyList;
        }
        public static async Task<CurrencyList> CopyCurrencyToTenant(string currencyId)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("CommonDomain/GetCopyCurrencyToTenant?CurrencyId=" + currencyId + "&CurrencyRate=4&RateDate=2019-6-24%2015:2:53.564");
            CurrencyList currencyList = RestClientService.ParseResponse<CurrencyList>(respnse);
            return currencyList;
        }
        public static async Task<string> GetIncotermId(string incotermCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("incotermviews" + QueryFiltersPreparation.GetUrlParameters(incotermCode, QueryFiltersPreparation.QueryfilterByCode(incotermCode)));
            IncotermPM newIncotermPM = RestClientService.ParseResponse<IncotermPM>(response);
            if (newIncotermPM == null)
            {
                newIncotermPM = await CreateIncoterm(incotermCode);
            }
            return newIncotermPM.Id;
        }
        public static async Task<IncotermPM> CreateIncoterm(string incotermCode)
        {
            IncotermPM incotermObjectPM = CreateIncotermPM(incotermCode);
            HttpResponseMessage response = await RestClientService.PostAsync(incotermObjectPM, "incoterms");
            IncotermPM incotermPM = RestClientService.ParseResponse<IncotermPM>(response);
            return incotermPM;
        }
        public static IncotermPM CreateIncotermPM(string incotermCode)
        {
            IncotermPM incotermPM = new IncotermPM();
            incotermPM.Tenant = IntegrationTestLoginParameters.Tenant;
            incotermPM.Code = incotermCode;
            incotermPM.Name = incotermCode + " Incoterm";
            incotermPM.Freight = "P";
            incotermPM.OtherCharges = "P";

            return incotermPM;
        }
        public static async Task<string> GetMeasurmentId(string measurmentCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("measurementviews" + QueryFiltersPreparation.GetUrlParameters(measurmentCode, QueryFiltersPreparation.QueryfilterByCode(measurmentCode)));
            MeasurementList measurementList = RestClientService.ParseResponse<MeasurementList>(response);
            return measurementList != null ? measurementList.Id : null;
        }
        public static async Task<ChargesGroupList> GetChargeGroup(string chargeGroupCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("chargesgroupviews" + QueryFiltersPreparation.GetUrlParameters(chargeGroupCode, QueryFiltersPreparation.QueryfilterByCode(chargeGroupCode)));
            ChargesGroupList chargesGroupList = RestClientService.ParseResponse<ChargesGroupList>(response);
            return chargesGroupList != null ? chargesGroupList : null;
        }
        public static async Task<string> GetChargeTypeId(string chargeTypeCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("chargestypeviews" + QueryFiltersPreparation.GetUrlParameters(chargeTypeCode, QueryFiltersPreparation.QueryfilterByCode(chargeTypeCode)));
            ChargesTypePM chargesTypePM = RestClientService.ParseResponse<ChargesTypePM>(response);
            if (chargesTypePM == null)
            {
                chargesTypePM = await CreateChargeType(chargeTypeCode);
            }
            return chargesTypePM.Id;
        }
        public static async Task<ChargesTypePM> CreateChargeType(string chargeTypeCode)
        {
            ChargesTypePM chargesTypePM = CreateChargeTypePM(chargeTypeCode);
            HttpResponseMessage response = await RestClientService.PostAsync(chargesTypePM, "chargestypes");
            chargesTypePM = RestClientService.ParseResponse<ChargesTypePM>(response);
            return chargesTypePM;
        }
        public static ChargesTypePM CreateChargeTypePM(string chargeTypeCode)
        {
            ChargesTypePM chargesTypePM = new ChargesTypePM();
            chargesTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            chargesTypePM.Code = chargeTypeCode;
            chargesTypePM.EnglishName = chargeTypeCode + " Charge Type";
            chargesTypePM.ChargesGroupId = ShipmentVariables.ChargeGroupCOMMId;
            chargesTypePM.ChargesGroupCode = ShipmentVariables.ChargeGroupCOMMCode;
            chargesTypePM.MeasurementId = ShipmentVariables.MeasurmentGRWTId;
            chargesTypePM.IsAutoDisplayInShipment = true;
            chargesTypePM.IsAutoDisplayInQuote = true;
            chargesTypePM.IsInland = true;
            chargesTypePM.IsAir = true;
            chargesTypePM.IsOcean = true;
            chargesTypePM.IsInland = true;
            chargesTypePM.IsExport = true;
            chargesTypePM.IsImport = true;
            chargesTypePM.IsDrop = true;
            chargesTypePM.IsDomestic = true;

            return chargesTypePM;
        }
        public static async Task<string> GetPortId(string portCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("PortViews" + QueryFiltersPreparation.GetUrlParameters(portCode, QueryFiltersPreparation.QueryfilterByCode(portCode)));
            //HttpResponseMessage response = await RestClientService.GetAsync("PortViews" + QueryFiltersPreparation.GetUrlParameters(portCode));

            PortList CurrentTenantport = RestClientService.ParseResponse<PortList>(response);
            if (CurrentTenantport == null)
            {
                CurrentTenantport = await GetPortFromTenant0(portCode);
            }
            return CurrentTenantport.Id;
        }
        public static async Task<PortList> GetPortFromTenant0(string portCode)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("PortViews/getTenantImportByFilters?Filter1Name=Code&Filter1Operator=equals&Filter1Value=" + portCode + "&PageSize=13");
            PortList tenantZeroPort = RestClientService.ParseResponse<PortList>(respnse);
            PortList currentTenantPort = new PortList();
            if (tenantZeroPort != null)
            {
                currentTenantPort = await CopyPortsToTenant(tenantZeroPort.Id);
            }
            return currentTenantPort;
        }
        public static async Task<PortList> CopyPortsToTenant(string portId)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("CommonDomain/GetPortCopyToCurrentTenant?entityId=" + portId);
            PortList portList = RestClientService.ParseResponse<PortList>(respnse);
            return portList;

        }
        public static async Task<string> GetGlobalZoneId(string globalZoneCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("globalzoneviews" + QueryFiltersPreparation.GetUrlParameters(globalZoneCode, QueryFiltersPreparation.QueryfilterByCode(globalZoneCode)));
            GlobalZonePM currentTenantGlobalZonePM = RestClientService.ParseResponse<GlobalZonePM>(response);
            if (currentTenantGlobalZonePM == null)
            {
                currentTenantGlobalZonePM = await CreateGlobalZone(globalZoneCode);
            }
            return currentTenantGlobalZonePM.Id;
        }
        public static async Task<GlobalZonePM> CreateGlobalZone(string globalZoneCode)
        {
            GlobalZonePM globalZonePM = CreateGlobalZonePM(globalZoneCode);
            HttpResponseMessage response = await RestClientService.PostAsync(globalZonePM, "globalzones");
            globalZonePM = RestClientService.ParseResponse<GlobalZonePM>(response);
            return globalZonePM;
        }
        public static GlobalZonePM CreateGlobalZonePM(string globalZoneCode)
        {
            GlobalZonePM globalZonePM = new GlobalZonePM();
            globalZonePM.Tenant = IntegrationTestLoginParameters.Tenant;
            globalZonePM.Code = globalZoneCode;
            globalZonePM.EnglishName = globalZoneCode + " Global Zone";
            return globalZonePM;
        }
        public static async Task<string> GetCountrytId(string countryCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("countryviews" + QueryFiltersPreparation.GetUrlParameters(countryCode, QueryFiltersPreparation.QueryfilterByCode(countryCode)));
            CountryPM CurrentTenantCountry = RestClientService.ParseResponse<CountryPM>(response);
            if (CurrentTenantCountry == null)
            {
                CurrentTenantCountry = await CreateCountry(countryCode);
            }
            return CurrentTenantCountry.Id;
        }
        public static async Task<CountryPM> CreateCountry(string countryCode)
        {
            CountryPM countryPM = CreateCountryPM(countryCode);
            HttpResponseMessage response = await RestClientService.PostAsync(countryPM, "countries");
            countryPM = RestClientService.ParseResponse<CountryPM>(response);
            return countryPM;
        }
        public static CountryPM CreateCountryPM(string countryCode)
        {
            CountryPM countryPM = new CountryPM();
            countryPM.Tenant = IntegrationTestLoginParameters.Tenant;
            countryPM.Code = countryCode;
            countryPM.EnglishName = countryCode + " Country";
            countryPM.GlobalZoneId = ShipmentVariables.GlobalZoneEUId;
            return countryPM;
        }
        public static async Task<string> GetStateId(string stateCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("stateviews" + QueryFiltersPreparation.GetUrlParameters(stateCode, QueryFiltersPreparation.QueryfilterByCode(stateCode)));
            StatePM currentTenantStatePM = RestClientService.ParseResponse<StatePM>(response);
            if (currentTenantStatePM == null)
            {
                currentTenantStatePM = await CreateState(stateCode);
            }
            return currentTenantStatePM.Id;
        }
        public static async Task<StatePM> CreateState(string stateCode)
        {
            StatePM statePM = CreateStatePM(stateCode);
            HttpResponseMessage response = await RestClientService.PostAsync(statePM, "states");
            statePM = RestClientService.ParseResponse<StatePM>(response);
            return statePM;
        }
        public static StatePM CreateStatePM(string stateCode)
        {
            StatePM statePM = new StatePM();
            statePM.Tenant = IntegrationTestLoginParameters.Tenant;
            statePM.Code = stateCode;
            statePM.EnglishName = stateCode + " State";
            statePM.CountryId = ShipmentVariables.CountryUSId;
            return statePM;
        }
        public static async Task<string> GetAirlineId(string airlineCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("airlineviews" + QueryFiltersPreparation.GetUrlParameters(airlineCode, QueryFiltersPreparation.QueryfilterByCode(airlineCode)));

            AirlineList CurrentTenantAirline = RestClientService.ParseResponse<AirlineList>(response);
            if (CurrentTenantAirline == null)
            {
                CurrentTenantAirline = await GetAirlineFromTenant0(airlineCode);
            }
            return CurrentTenantAirline.Id;
        }
        public static async Task<AirlineList> GetAirlineFromTenant0(string airlineCode)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("carrierviews/getTenantImportByFilters?Filter1Name=Code&Filter1Operator=equals&PageSize=13&Filter1Value=" + airlineCode);
            AirlineList tenantZeroAirline = RestClientService.ParseResponse<AirlineList>(respnse);
            AirlineList currentTenantAirline = new AirlineList();
            if (tenantZeroAirline != null)
            {
                currentTenantAirline = await CopyAirlineToTenant(tenantZeroAirline.Id);
            }
            return currentTenantAirline;
        }
        public static async Task<AirlineList> CopyAirlineToTenant(string airlineId)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("PartnersDomain/GetCarrierCopyToCurrentTenant?entityId=" + airlineId);
            AirlineList airlineList = RestClientService.ParseResponse<AirlineList>(respnse);
            return airlineList;
        }
        public static async Task<string> GetShippingLineId(string shippingLineCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("shippinglineviews" + QueryFiltersPreparation.GetUrlParameters(shippingLineCode, QueryFiltersPreparation.QueryfilterByCode(shippingLineCode)));
            ShippingLineList CurrentTenantShippingLine = RestClientService.ParseResponse<ShippingLineList>(response);
            if (CurrentTenantShippingLine == null)
            {
                CurrentTenantShippingLine = await GetShippingLineFromTenant0(shippingLineCode);
            }
            return CurrentTenantShippingLine.Id;
        }
        public static async Task<ShippingLineList> GetShippingLineFromTenant0(string shippingLineCode)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("carrierviews/getTenantImportByFilters?Filter1Name=Code&Filter1Operator=equals&PageSize=13&Filter1Value=" + shippingLineCode);
            ShippingLineList tenantZeroShippingLine = RestClientService.ParseResponse<ShippingLineList>(respnse);
            ShippingLineList currentTenantShippingLine = new ShippingLineList();
            if (tenantZeroShippingLine != null)
            {
                currentTenantShippingLine = await CopyShippingLineToTenant(tenantZeroShippingLine.Id);
            }
            return currentTenantShippingLine;
        }
        public static async Task<ShippingLineList> CopyShippingLineToTenant(string shippingLineId)
        {
            HttpResponseMessage respnse = await RestClientService.GetAsync("PartnersDomain/GetCarrierCopyToCurrentTenant?entityId=" + shippingLineId);
            ShippingLineList shippingLineList = RestClientService.ParseResponse<ShippingLineList>(respnse);
            return shippingLineList;
        }
        public static async Task<string> GetMoveTypeId(string moveTypeCode, string moveTypeTransportMode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("movetypeviews" + QueryFiltersPreparation.GetUrlParameters(moveTypeCode, QueryFiltersPreparation.QueryfilterByCode(moveTypeCode)));
            MoveTypePM currentTenantMoveTypePM = RestClientService.ParseResponse<MoveTypePM>(response);
            if (currentTenantMoveTypePM == null)
            {
                currentTenantMoveTypePM = await CreateMoveType(moveTypeCode, moveTypeTransportMode);
            }
            return currentTenantMoveTypePM.Id;
        }
        public static async Task<MoveTypePM> CreateMoveType(string moveTypeCode, string moveTypeTransportMode)
        {
            MoveTypePM moveTypePM = CreateMoveTypePM(moveTypeCode, moveTypeTransportMode);
            HttpResponseMessage response = await RestClientService.PostAsync(moveTypePM, "movetypes");
            moveTypePM = RestClientService.ParseResponse<MoveTypePM>(response);
            return moveTypePM;
        }
        public static MoveTypePM CreateMoveTypePM(string moveTypeCode, string moveTypeTransportMode)
        {
            MoveTypePM moveTypePM = new MoveTypePM();
            moveTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            moveTypePM.Code = moveTypeCode;
            moveTypePM.MoveTypeEnglishName = "TestMoveTypeId" + moveTypeCode;
            moveTypePM.MoveTypeLocalName = moveTypeCode + " Move Type LocalName";
            if (moveTypeTransportMode == "A")
            {
                moveTypePM.IsAir = true;
                moveTypePM.IsOcean = false;
                moveTypePM.IsInland = false;
                moveTypePM.TransportModeId = "A";

            }
            else if (moveTypeTransportMode == "O")
            {
                moveTypePM.IsAir = false;
                moveTypePM.IsOcean = true;
                moveTypePM.IsInland = false;
                moveTypePM.TransportModeId = "O";
            }
            else
            {
                moveTypePM.IsAir = false;
                moveTypePM.IsOcean = false;
                moveTypePM.IsInland = true;
                moveTypePM.TransportModeId = "I";
            }
            return moveTypePM;
        }
        public static async Task<string> GetVesselId(string vesselCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("vesselviews" + QueryFiltersPreparation.GetUrlParameters(vesselCode, QueryFiltersPreparation.QueryfilterByCode(vesselCode)));
            VesselPM currentTenantVesselPM = RestClientService.ParseResponse<VesselPM>(response);
            if (currentTenantVesselPM == null)
            {
                currentTenantVesselPM = await CreateVessel(vesselCode);
            }
            return currentTenantVesselPM.Id;
        }
        public static async Task<VesselPM> CreateVessel(string vesselCode)
        {
            VesselPM vesselPM = CreateVesselPM(vesselCode);
            HttpResponseMessage response = await RestClientService.PostAsync(vesselPM, "vessels");
            vesselPM = RestClientService.ParseResponse<VesselPM>(response);
            return vesselPM;
        }
        public static VesselPM CreateVesselPM(string vesselCode)
        {
            VesselPM vesselPM = new VesselPM();
            vesselPM.Tenant = IntegrationTestLoginParameters.Tenant;
            vesselPM.Code = vesselCode;
            vesselPM.EnglishName = " vesselId" + vesselCode;
            vesselPM.IMOCode = "IMOCode " + vesselCode;
            return vesselPM;
        }
        public static async Task<string> GetPackageTypeId(string packageTypeCode, string packageTypeTransportMode, bool isContainer)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("packagetypeviews" + QueryFiltersPreparation.GetUrlParameters(packageTypeCode, QueryFiltersPreparation.QueryfilterByCode(packageTypeCode)));
            PackageTypePM currentTenantPackageTypePM = RestClientService.ParseResponse<PackageTypePM>(response);
            if (currentTenantPackageTypePM == null)
            {
                currentTenantPackageTypePM = await CreatePackageType(packageTypeCode, packageTypeTransportMode, isContainer);
            }
            return currentTenantPackageTypePM.Id;
        }
        public static async Task<PackageTypePM> CreatePackageType(string packageTypeCode, string packageTypeTransportMode, bool isContainer)
        {
            PackageTypePM packageTypePM = CreatePackageTypePM(packageTypeCode, packageTypeTransportMode, isContainer);
            HttpResponseMessage response = await RestClientService.PostAsync(packageTypePM, "packagetypes");
            packageTypePM = RestClientService.ParseResponse<PackageTypePM>(response);
            return packageTypePM;
        }
        public static PackageTypePM CreatePackageTypePM(string packageTypeCode, string packageTypeTransportMode, bool isContainer)
        {
            PackageTypePM packageTypePM = new PackageTypePM();
            packageTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            packageTypePM.Code = packageTypeCode;
            packageTypePM.EnglishName = "ContainerId" + packageTypeCode;
            packageTypePM.PrintAs = packageTypeCode;
            if (packageTypeTransportMode == "A")
            {
                packageTypePM.IsAir = true;
            }
            else if (packageTypeTransportMode == "O")
            {
                packageTypePM.IsOcean = true;
            }
            else
            {
                packageTypePM.IsInland = true;
            }
            if (isContainer == true)
                packageTypePM.IsContainer = true;

            return packageTypePM;
        }
        public static async Task<string> GetPaymentTermId(string paymentTermName)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("paymenttermviews" + QueryFiltersPreparation.GetUrlParameters(paymentTermName));
            PaymentTermPM currentTenantPaymentTermPM = RestClientService.ParseResponse<PaymentTermPM>(response);
            if (currentTenantPaymentTermPM == null)
            {
                currentTenantPaymentTermPM = await CreatePaymentTerm(paymentTermName);
            }
            return currentTenantPaymentTermPM.Id;
        }
        public static async Task<PaymentTermPM> CreatePaymentTerm(string paymentTermName)
        {
            PaymentTermPM paymentTermPM = CreatePaymentTermPM(paymentTermName);
            HttpResponseMessage response = await RestClientService.PostAsync(paymentTermPM, "paymentterms");
            paymentTermPM = RestClientService.ParseResponse<PaymentTermPM>(response);
            return paymentTermPM;
        }
        public static PaymentTermPM CreatePaymentTermPM(string paymentTermName)
        {
            PaymentTermPM paymentTermPM = new PaymentTermPM();
            paymentTermPM.Tenant = IntegrationTestLoginParameters.Tenant;
            paymentTermPM.EnglishName = paymentTermName;
            paymentTermPM.LocalName = paymentTermName;
            paymentTermPM.FromDateTypeCode = "SHI";
            return paymentTermPM;
        }
        public static async Task<string> GetVATTypeId(string vatTypeCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("vattypeviews" + QueryFiltersPreparation.GetUrlParameters(vatTypeCode, QueryFiltersPreparation.QueryfilterByCode(vatTypeCode)));
            VatTypePM currentTenantVatTypePM = RestClientService.ParseResponse<VatTypePM>(response);
            if (currentTenantVatTypePM == null)
            {
                currentTenantVatTypePM = await CreateVatType(vatTypeCode);
            }
            return currentTenantVatTypePM.Id;
        }
        public static async Task<VatTypePM> CreateVatType(string vatTypeCode)
        {
            VatTypePM vatTypePM = CreateVatTypePM(vatTypeCode);
            HttpResponseMessage response = await RestClientService.PostAsync(vatTypePM, "vattypes");
            vatTypePM = RestClientService.ParseResponse<VatTypePM>(response);
            return vatTypePM;
        }
        public static VatTypePM CreateVatTypePM(string vatTypeCode)
        {
            VatTypePM vatTypePM = new VatTypePM();
            vatTypePM.Tenant = IntegrationTestLoginParameters.Tenant;
            vatTypePM.Code = vatTypeCode;
            vatTypePM.EnglishName = vatTypeCode;
            vatTypePM.LocalName = vatTypeCode;
            vatTypePM.NewEntityPercentage = 0;
            vatTypePM.NewEntityPercentageDate = DateTime.Now;
            return vatTypePM;
        }
        public static async Task<string> GetQuoteStageId(string quoteStageCode)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("quotestageviews" + QueryFiltersPreparation.GetUrlParameters(quoteStageCode, QueryFiltersPreparation.QueryfilterByCode(quoteStageCode)));
            QuoteStageList currentTenantQuoteStageList = RestClientService.ParseResponse<QuoteStageList>(response);
            return currentTenantQuoteStageList != null ? currentTenantQuoteStageList.Id : null;
        }
        public static async Task<string> GetVendorId(string vendorName)
        {
            HttpResponseMessage response = await RestClientService.GetAsync("vendorviews" + QueryFiltersPreparation.GetUrlParameters(vendorName));
            VendorPM currenctVendorPM = RestClientService.ParseResponse<VendorPM>(response);
            if (currenctVendorPM == null)
            {
                currenctVendorPM = await CreateVendor(vendorName);
            }
            return currenctVendorPM.Id;
        }
        public static async Task<VendorPM> CreateVendor(string vendorName)
        {
            //VendorPM vendorPM = CreateVendorPM(vendorName);
            //HttpResponseMessage response = await RestClientService.PostAsync(vendorPM, "PartnersDomain");
            //vendorPM = RestClientService.ParseResponse<VendorPM>(response);
            //return vendorPM;
        }
        //public static VendorPM CreateVendorPM(string vendorName)
        //{
        //    VendorPM vendorPM = new VendorPM();
        //    vendorPM.Tenant = IntegrationTestLoginParameters.Tenant;
        //    vendorPM.EnglishName = "TestVendor" + vesselCode;
        //    vendorPM.IMOCode = "IMOCode " + vesselCode;
        //    return vendorPM;
        //}
    }
}
