using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Test.Base.Services
{
    public static class DataPreparation
    {
        #region Get Locations Variables

        public static LocationsVariables GetLocationsVariables()
        {
            return new LocationsVariables
            {
                PortLHRId = GetPortId("GBLHR", null, true),
                PortLASDomesticId = GetPortId("LAS", "US"),
                PortMIADomesticId = GetPortId("MIA", "US"),
                PortAirJFKId = GetPortId("JFK", "US"),
                PortOceanNYCId = GetPortId("USNYC", null, true),
                PortOceanSOUId = GetPortId("USSOU", null, true),
                PortInlandNYCId = GetPortId("NYC", null),
                PortLONId = GetPortId("LON", null),
                PortMANId = GetPortId("MAN", null),
                StateAKId = GetStateId("AK"),
                CountryUSId = GetCountryId("US"),
                CountryGBId = GetCountryId("GB"),
                CountryTSId = GetCountryId("TS"),
                CityAnchorageId = GetCityId("Anchorage", "US", "AK"),
                CityManchesterId = GetCityId("Manchester", "GB"),
                SpecialServicesTypeTSId = GetSpecialServicesTypeTSId("TS")
            };
        }

        #endregion

        #region SpecialServicesType
        private static string GetSpecialServicesTypeTSId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            string UserTenantSSTypeId = GetSpecialServicesTypeIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantSSTypeId))
            {
                UserTenantSSTypeId = CreateSpecialServicesTypeForUserTenant(code);
            }

            return UserTenantSSTypeId;
        }

        private static string GetSpecialServicesTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<SpecialServicesTypePM>> response = APICaller.CallGetByFilters<IEnumerable<SpecialServicesTypePM>>(Urls.SpecialServicesTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string CreateSpecialServicesTypeForUserTenant(string code)
        {
            SpecialServicesTypePM type = new SpecialServicesTypePM
            {
                Tenant = UserTenant.Tenant,
                Code = code,
                EnglishName = "TestSpecialServicesTypes",
                InActive = false,
                IsHybrid=false,
                IsSecured=false,
                LocalName= null,
                SearchFields=null
            };

            ApiResponse<SpecialServicesTypePM> response = APICaller.CallPost<SpecialServicesTypePM>(type, Urls.SpecialServicesTypesController, UserTenant.Token);
            return response.Data?.Id;
        }

        #endregion

        #region Get Partners Variables

        public static PartnersVariables GetPartnersVariables()
        {
            return new PartnersVariables
            {
                VendorId = GetPartnerId(new PartnerParameters { TypeCode = "VD", Name = "TestVendor" }),
                AgentId = GetPartnerId(new PartnerParameters { TypeCode = "AG", Name = "TestAgentExport" }),
                AgentCode = GetPartnerCode(new PartnerParameters { TypeCode = "AG", Name = "TestAgentExport" }),
                CustomerId = GetPartnerId(new PartnerParameters { TypeCode = "CS", Name = "TestCustomer", IsCustomer = true }),
                PotentialCustomerId = GetPartnerId(new PartnerParameters { TypeCode = "PO", Name = "TestPotentialCustomer" }),
                CustomAgentId = GetPartnerId(new PartnerParameters { TypeCode = "CG", Name = "TestCustomAgent" }),
                ShippingAgentId = GetPartnerId(new PartnerParameters { TypeCode = "SG", Name = "TestShippingAgent" }),
                TruckerTLONId = GetPartnerId(new PartnerParameters { TypeCode = "TR", Name = "TestTLONTrucker", Code = "TLON" }),
                TruckerTNYCId = GetPartnerId(new PartnerParameters { TypeCode = "TR", Name = "TestTNYCTrucker", Code = "TNYC" }),
                ShipperExportId = GetPartnerId(new PartnerParameters { TypeCode = "CS", Name = "TestShipperExport" }),
                ShipperExportCode = GetPartnerCode(new PartnerParameters { TypeCode = "CS", Name = "TestShipperExport" }),
                ShipperImportId = GetPartnerId(new PartnerParameters { TypeCode = "CS", Name = "TestShipperImport" }),
                ShipperImportCode = GetPartnerCode(new PartnerParameters { TypeCode = "CS", Name = "TestShipperImport" }),
                ConsigneeExportId = GetPartnerId(new PartnerParameters { TypeCode = "CS", Name = "TestConsigneeExport" }),
                ConsigneeImportId = GetPartnerId(new PartnerParameters { TypeCode = "CS", Name = "TestConsigneeImport" }),
                AirlineAAId = GetPartnerId(new PartnerParameters { TypeCode = "AL", Name = "TestAAAirline", Code = "AA", CopyFromTenantZero = true }),
                AirlineBAId = GetPartnerId(new PartnerParameters { TypeCode = "AL", Name = "TestBAAirline", Code = "BA", CopyFromTenantZero = true }),
                ShippingLineMAEUId = GetPartnerId(new PartnerParameters { TypeCode = "SL", Name = "TestMAEUShippingLine", Code = "MAEU", CopyFromTenantZero = true }),
                ShippingLineMSCUId = GetPartnerId(new PartnerParameters { TypeCode = "SL", Name = "TestMSCUShippingLine", Code = "MSCU", CopyFromTenantZero = true }),
                ShippingLineYMLUId = GetPartnerId(new PartnerParameters { TypeCode = "SL", Name = "TestYMLUShippingLine", Code = "YMLU", CopyFromTenantZero = true }),
                WarehouseId = GetPartnerId(new PartnerParameters { TypeCode = "WH", Name = "TestWarehouse", Code = "TSWHE" })
            };
        }

        #endregion

        #region Get Billings Variables
        public static BillingVariables GetBillingVariables()
        {
            return new BillingVariables
            {
                CurrencyEURId = GetCurrencyId("EUR"),
                MeasurementGRWTId = GetMeasurementId("GRWT"),
                ChargeTypeAFTId = GetChargeTypeId("AFT"),
                IncotermLDEId = GetIncotermId("LDE"),
                PaymentTermCashId = GetPaymentTermId("Cash"),
                VATTypeZeroId = GetVATTypeId("ZERO"),
                CreditCardTSId = GetCreditCardTypeId("TS")
            };
        }

        #endregion

        #region Locations Preparation Variables

        #region Ports
        private static string GetPortId(string code, string countryCode, bool isCombined = false)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, countryCode);
            if (isCombined)
            {
                apiQueryFilters.Filter1Name = "CombinedCode";
            }
            string UserTenantPortId = GetPortIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantPortId))
            {
                string ZeroTenantPortId = GetPortIdFromZeroTenant(apiQueryFilters);
                UserTenantPortId = GetCopiedPortFromTenantZero(ZeroTenantPortId);
            }

            return UserTenantPortId;
        }

        private static string GetPortIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetPortIdFromZeroTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.PortViewsGetTenantImportByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCopiedPortFromTenantZero(string portId)
        {
            ApiResponse<Port> response = APICaller.CallGet<Port>(Urls.CommonDomainGetPortCopyToCurrentTenant(portId), UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Countries
        private static string GetCountryId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            string UserTenantCountryId = GetCountryIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantCountryId))
            {
                UserTenantCountryId = CreateCountryForUserTenant(code);
            }

            return UserTenantCountryId;
        }

        private static string GetCountryIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Country>> response = APICaller.CallGetByFilters<IEnumerable<Country>>(Urls.CountryViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string CreateCountryForUserTenant(string code)
        {
            Country country = new Country
            {
                Tenant = UserTenant.Tenant,
                Code = code,
                EnglishName = "Test",
                GlobalZoneId = GetGlobalZoneId("AS")
            };

            ApiResponse<Country> response = APICaller.CallPost<Country>(country, Urls.CountriesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static string GetGlobalZoneId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            return GetGetGlobalZoneIdFromUserTenant(apiQueryFilters);
        }

        private static string GetGetGlobalZoneIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<GlobalZone>> response = APICaller.CallGetByFilters<IEnumerable<GlobalZone>>(Urls.GlobalZoneViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region States
        private static string GetStateId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(code, null);
            string UserTenantStateId = GetStateIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantStateId))
            {
                UserTenantStateId = CreateStateForUserTenant(code);
            }

            return UserTenantStateId;
        }

        private static string GetStateIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Port>> response = APICaller.CallGetByFilters<IEnumerable<Port>>(Urls.StateViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string CreateStateForUserTenant(string code)
        {
            State state = new State
            {
                Tenant = UserTenant.Tenant,
                Code = code,
                EnglishName = code + " State",
                CountryId = GetCountryId("US")
            };

            ApiResponse<State> response = APICaller.CallPost<State>(state, Urls.StatesController, UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Cities
        private static string GetCityId(string cityCode, string countryCode, string stateCode = null)
        {
            string countryId = GetCountryId(countryCode);
            string stateId = stateCode == null ? null : GetStateId(stateCode);

            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Operator("equals")
                .Filter1Value(cityCode)
                .Filter2Name("CountryId")
                .Filter2Operator("equals")
                .Filter2Value(countryId)
                .Build(); 

            string userTenantCityId = GetCityIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(userTenantCityId))
            {
                userTenantCityId = CreateCityForUserTenant(cityCode, countryId, stateId);
            }

            return userTenantCityId;
        }

        private static string GetCityIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<City>> response = APICaller.CallGetByFilters<IEnumerable<City>>(Urls.CountryCityViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string CreateCityForUserTenant(string cityCode, string countryId, string stateId)
        {
            City city = new City
            {
                Tenant = UserTenant.Tenant,
                Code = cityCode,
                EnglishName = cityCode,
                CountryId = countryId,
                StateId = stateId
            };

            ApiResponse<City> response = APICaller.CallPost<City>(city, Urls.CountryCities, UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Build ApiQueryFilters
        private static ApiQueryFilters BuildApiQueryFilters(string code, string countryCode)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Operator("equals")
                .Filter1Value(code)
                .Filter2Name("CountryCode")
                .Filter2Operator("contains")
                .Filter2Value(countryCode)
                .Build(); 
        }

        private static ApiQueryFilters BuildApiQueryFiltersForBillings(string code, string SearchFieldsCode)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Operator("equals")
                .Filter1Value(code)
                .Filter2Name("SearchFields")
                .Filter2Operator("Contains")
                .Filter2Value(SearchFieldsCode)
                .Build();
        }
        #endregion

        #endregion

        #region Partners Data Preparation

        private static string GetPartnerId(PartnerParameters partnerParameters)
        {
            string userTenantPartnerId = GetPartnerIdFromTenant(partnerParameters, false);
            if (string.IsNullOrEmpty(userTenantPartnerId))
            {
                if (partnerParameters.CopyFromTenantZero)
                {
                    string tenantZeroPartnerId = GetPartnerIdFromTenant(partnerParameters, true);
                    userTenantPartnerId = GetCopiedPartnerIdFromTenantZero(tenantZeroPartnerId);
                    if (partnerParameters.Code == "AA")//can be set to update all the airlines not only AA
                    {
                        UpdateAirline(partnerParameters);
                    }
                }
                else
                {
                    userTenantPartnerId = CreatePartnerForUserTenant(partnerParameters);
                }
            }

            return userTenantPartnerId;
        }

        private static string GetPartnerCode(PartnerParameters partnerParameters)
        {
            string userTenantPartnerId = GetPartnerId(partnerParameters);
            string requestUrl = GetUrlForUserTenantPartnerRequest(partnerParameters.TypeCode);

            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Id")
                .Filter1Operator("equals")
                .Filter1Value(userTenantPartnerId)
                .Build(); 

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(requestUrl, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Code"];
        }

        private static string GetPartnerIdFromTenant(PartnerParameters partnerParameters, bool getFromTenantZero)
        {
            string requestUrl = getFromTenantZero ? Urls.CarrierViewsGetTenantImportByFilters : GetUrlForUserTenantPartnerRequest(partnerParameters.TypeCode);

            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name(string.IsNullOrEmpty(partnerParameters.Code) ? "EnglishName" : "Code")
                .Filter1Operator("equals")
                .Filter1Value(string.IsNullOrEmpty(partnerParameters.Code) ? partnerParameters.Name : partnerParameters.Code)
                .Build(); 

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(requestUrl, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        private static string CreatePartnerForUserTenant(PartnerParameters partnerParameters)
        {
            Partner partner = BuildPartner(partnerParameters);
            ApiResponse<Partner> response = APICaller.CallPost<Partner>(partner, Urls.PartnersDomainController, UserTenant.Token);
            return response.Data?.PartnerId;
        }

        private static string GetCopiedPartnerIdFromTenantZero(string tenantZeroPartnerId)
        {
            string requestUrl = Urls.PartnersDomainGetCarrierCopyToCurrentTenant(tenantZeroPartnerId);
            ApiResponse<dynamic> response = APICaller.CallGet<dynamic>(requestUrl, UserTenant.Token);
            return response.Data?["Id"];
        }

        private static void UpdateAirline(PartnerParameters partnerParameters)
        {
            string userTenantPartnerId = GetPartnerIdFromTenant(partnerParameters, false);
            string getRequestUrl = Urls.AirlineGetSingle(userTenantPartnerId);
            string putRequestUrl = Urls.Airlines;

            ApiResponse<AirlinePM> response = APICaller.CallGet<AirlinePM>(getRequestUrl, UserTenant.Token);
            AirlinePM airline = response.Data;
            airline.LimitedLength = false;
            airline.CheckDigit = false;
            ApiResponse<AirlinePM> responseUpdate = APICaller.CallPut<AirlinePM>(airline, putRequestUrl, UserTenant.Token);
        }

        private static Partner BuildPartner(PartnerParameters partnerParameters)
        {
            Partner partner = new Partner
            {
                Tenant = UserTenant.Tenant,
                PartnerTypeId = partnerParameters.TypeCode,
                Address = BuildPartnerAddress(partnerParameters.Name),
                Contact = BuildPartnerContact(partnerParameters.Name)
            };

            PartnerInformation partnerInformation = BuidPartnerInformation(partnerParameters);

            partner = SetPartnerInformation(partner, partnerInformation, partnerParameters.TypeCode);
            return partner;
        }

        private static Address BuildPartnerAddress(string partnerName)
        {
            return new Address
            {
                Tenant = UserTenant.Tenant,
                Name = partnerName + " Address",
                Description = "Main Address",
                AddressTypeId = "M",
                Address1 = "Test Address",
                StateId = GetStateId("AK"),
                CountryId = GetCountryId("US"),
                IsCreatedWithPartner = true
            };
        }

        private static Contact BuildPartnerContact(string partnerName)
        {
            return new Contact
            {
                Tenant = UserTenant.Tenant,
                EnglishName = partnerName + " Contact",
                Email = partnerName.ToLower() + "@test.com",
                Mobile = "9999999999",
                Fax = "999999",
                SetAsPrimaryForCard = true,
                IsCreatedWithPartner = true
            };
        }

        private static PartnerInformation BuidPartnerInformation(PartnerParameters partnerParameters)
        {
            return new PartnerInformation
            {
                Tenant = UserTenant.Tenant,
                EnglishName = partnerParameters.Name,
                LocalName = partnerParameters.Name,
                PartnerTypeId = partnerParameters.TypeCode,
                Code = partnerParameters.Code,
                IsCustomer = partnerParameters.IsCustomer
            };
        }

        private static Partner SetPartnerInformation(Partner partner, PartnerInformation partnerInformation, string partnerTypeCode)
        {
            switch (partnerTypeCode)
            {
                case "VD":
                    partner.Vendor = partnerInformation;
                    return partner;
                case "AG":
                    partner.Agent = partnerInformation;
                    return partner;
                case "CS":
                case "PO":
                    partner.Customer = partnerInformation;
                    return partner;
                case "CG":
                    partner.CustomAgent = partnerInformation;
                    return partner;
                case "SG":
                    partner.ShippingAgent = partnerInformation;
                    return partner;
                case "TR":
                    partner.Trucker = partnerInformation;
                    partner.Trucker.CarrierTypeId = partnerTypeCode;
                    return partner;
                case "WH":
                    partner.Warehouse = partnerInformation;
                    return partner;
                default:
                    return null;
            }
        }

        private static string GetUrlForUserTenantPartnerRequest(string partnerTypeCode)
        {
            switch (partnerTypeCode)
            {
                case "VD":
                    return Urls.VendorViewsGetByFilters;
                case "AG":
                    return Urls.AgentViewsGetByFilters;
                case "CS":
                case "PO":
                    return Urls.CustomerViewsGetByFilters;
                case "CG":
                    return Urls.CustomAgentViewsGetByFilters;
                case "SG":
                    return Urls.ShippingAgentViewsGetByFilters;
                case "TR":
                    return Urls.TruckerViewsGetByFilters;
                case "AL":
                    return Urls.AirlineViewsGetByFilters;
                case "WH":
                    return Urls.WarehouseViewsGetByFilters;
                case "SL":
                    return Urls.ShippingLineViewsGetByFilters;
                default:
                    return null;
            }
        }

        #endregion

        #region Billings Data Preparation

        #region Currency
        private static string GetCurrencyId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(code, null);

            string UserTenantCurrencyId = GetCurrencyIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantCurrencyId))
            {
                string ZeroTenantCurrencyId = GetCurrencyIdFromZeroTenant(apiQueryFilters);
                UserTenantCurrencyId = GetCopiedCurrencyFromTenantZero(ZeroTenantCurrencyId);
            }
            return UserTenantCurrencyId;
        }

        private static string GetCurrencyIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Currency>> response = APICaller.CallGetByFilters<IEnumerable<Currency>>(Urls.CurrencyViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }
        private static string GetCurrencyIdFromZeroTenant(ApiQueryFilters apiQueryFilters)
        {
            apiQueryFilters.Tenant = 0;
            ApiResponse<IEnumerable<Currency>> response = APICaller.CallGetByFilters<IEnumerable<Currency>>(Urls.CurrencyViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCopiedCurrencyFromTenantZero(string currencyId)
        {
            ApiResponse<Currency> response = APICaller.CallGet<Currency>(Urls.CommonDomainGetCopyCurrencyToTenant(currencyId), UserTenant.Token);
            return response.Data?.Id;
        }
        #endregion

        #region Measurement

        private static string GetMeasurementId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(code, null);

            string UserTenantMeasurementId = GetMeasurementIdFromUserTenant(apiQueryFilters);
            return UserTenantMeasurementId;
        }

        private static string GetMeasurementIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Measurement>> response = APICaller.CallGetByFilters<IEnumerable<Measurement>>(Urls.MeasurementViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region ChargeType

        private static string GetChargeTypeId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(code, null);

            string UserTenantChargeTypeId = GetChargeTypeIdFromUserTenant(apiQueryFilters);
            return UserTenantChargeTypeId;
        }

        private static string GetChargeTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<ChargeType>> response = APICaller.CallGetByFilters<IEnumerable<ChargeType>>(Urls.ChargeTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            ChargeType chargeType = response.Data?.FirstOrDefault();
            if (chargeType != null && !chargeType.IsCustoms)
            {
                UpdateChargeType(chargeType);
            }
            return chargeType?.Id;
        }

        private static void UpdateChargeType(ChargeType chargeType)
        {
            chargeType.IsCustoms = true;
            APICaller.CallPut<ChargeType>(chargeType, Urls.ChargesTypes, UserTenant.Token);
        }

        #endregion

        #region Incoterm
        private static string GetIncotermId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(code, null);

            string UserTenantIncotermId = GetIncotermIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantIncotermId))
            {
                UserTenantIncotermId = GetCreatedIncotermFromTenantZero(code);
            }

            return UserTenantIncotermId;
        }

        private static string GetIncotermIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<Incoterm>> response = APICaller.CallGetByFilters<IEnumerable<Incoterm>>(Urls.IncotermViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedIncotermFromTenantZero(string code)
        {
            Incoterm incoterm = CreateIncotermPM(code);
            ApiResponse<Incoterm> response = APICaller.CallPost<Incoterm>(incoterm, Urls.IncotermsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static Incoterm CreateIncotermPM(string code)
        {
            Incoterm incoterm = new Incoterm();
            incoterm.Tenant = UserTenant.Tenant;
            incoterm.Code = code;
            incoterm.Name = code + " Incoterm";
            incoterm.Freight = "P";
            incoterm.OtherCharges = "P";
            return incoterm;
        }

        #endregion

        #region PaymentTerm

        private static string GetPaymentTermId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(null, code);

            string UserTenantPaymentTermId = GetPaymentTermIdFromUserTenant(apiQueryFilters);
            return UserTenantPaymentTermId;
        }

        private static string GetPaymentTermIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<PaymentTerm>> response = APICaller.CallGetByFilters<IEnumerable<PaymentTerm>>(Urls.PaymentTermViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region VATType

        private static string GetVATTypeId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(null, code);
            apiQueryFilters.GetAll = false;
            apiQueryFilters.ForceCacheRefresh = false;
            apiQueryFilters.GetCount = true;
            string UserTenantVATTypeId = GetVATTypeIdFromUserTenant(apiQueryFilters);
            return UserTenantVATTypeId;
        }

        private static string GetVATTypeIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<VATType>> response = APICaller.CallGetByFilters<IEnumerable<VATType>>(Urls.VatTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        #endregion

        #region Credit Card Type
        private static string GetCreditCardTypeId(string code)
        {
            ApiQueryFilters apiQueryFilters = BuildApiQueryFiltersForBillings(code, null);

            string UserTenantCreditCardId = GetCreditCardIdFromUserTenant(apiQueryFilters);
            if (string.IsNullOrEmpty(UserTenantCreditCardId))
            {
                UserTenantCreditCardId = GetCreatedCreditCardFromTenantZero(code);
            }

            return UserTenantCreditCardId;
        }

        private static string GetCreditCardIdFromUserTenant(ApiQueryFilters apiQueryFilters)
        {
            ApiResponse<IEnumerable<CreditCardTypePM>> response = APICaller.CallGetByFilters<IEnumerable<CreditCardTypePM>>(Urls.CreditCardTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private static string GetCreatedCreditCardFromTenantZero(string code)
        {
            CreditCardTypePM CreditCard = CreateCreditCardPM(code);
            ApiResponse<CreditCardTypePM> response = APICaller.CallPost<CreditCardTypePM>(CreditCard, Urls.CreditCardController, UserTenant.Token);
            return response.Data?.Id;
        }

        private static CreditCardTypePM CreateCreditCardPM(string code)
        {
            CreditCardTypePM CreditCard = new CreditCardTypePM();
            CreditCard.Tenant = UserTenant.Tenant;
            CreditCard.Code = code;
            CreditCard.Name = "TestCreditCardType";
            return CreditCard;
        }
        #endregion

        #endregion
    }
}