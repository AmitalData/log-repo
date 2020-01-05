using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
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
            ShipmentVariables.ChargeTypeAFTId= await GetChargeTypeId("ZDT");

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
            HttpResponseMessage response = await RestClientService.GetAsync("chargestypeviews" + QueryFiltersPreparation.GetUrlParameters(chargeTypeCode,QueryFiltersPreparation.QueryfilterByCode(chargeTypeCode)));
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
    }
}
