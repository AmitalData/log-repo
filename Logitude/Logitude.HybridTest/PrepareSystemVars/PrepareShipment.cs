using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareShipment
    {
        public static void PrepareShipmentVars()
        {
            GetCurrencyCodeEUR();
            GetIncotermCodeCIF();
            GetChargeTypeCodeAFT();
            PreparePorts.PreparePortsVars();
            PrepareCountries.PrepareCountriesVars();
            UpsertStateCodeAK();
            UpsertAgentTest();
            UpsertContactTest();
            PrepareCustomers.PrepareCustomersVars();
            PrepareAirlines.PrepareAirlinesVars();
            PrepareShippingLines.PrepareShippingLinesVars();
            PrepareTruckers.PrepareTruckersVars();
            UpsertVesselCodeHV();
            PreparePackageTypes.PreparePackageTypesVars();
            UpsertVendorCodeHVEN();
        }
        private static void GetCurrencyCodeEUR()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Currency",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CurrencyList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = HybridData.CurrencyCodeEUR,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CurrencyList[] currencies = (CurrencyList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            if (currencies.Length == 0)
            {
                serviceParameters = new object[] { filters, 0, serviceResponse };
                serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
                currencies = (CurrencyList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                CopyCurrencyFromTenant0ToTestTenant(currencies[0]);
            }
        }
        private static void CopyCurrencyFromTenant0ToTestTenant(CurrencyList currencyPM)
        {
            CurrencyPM newCurrencyPM = new CurrencyPM()
            {
                Code = currencyPM.Code,
                EnglishName = currencyPM.EnglishName,
                LocalName = currencyPM.LocalName,
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };

            AssertResponse(currencyPM);
        }
        private static void GetIncotermCodeCIF()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Incoterm",
                ServiceOperation = "GetIncoterms",
                ServiceResponseIndex = 0,
                ServiceType = typeof(IncotermList),
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            IncotermList[] incoterms = (IncotermList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Incoterms Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Incoterms Failed! " + serviceOutcome.Response.Result);
            if (incoterms.Length == 0)
                Assert.Inconclusive("There Isn't Any Incoterm!");
            IncotermList CIFIncoterm = IncotermExist(incoterms, HybridData.IncotermCodeCIF);
            if (CIFIncoterm == null)
                Assert.Fail("Prepare IncotermIdCIF Failed!");
        }
        private static void GetIncotermLDE()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Incoterm",
                ServiceOperation = "GetIncoterms",
                ServiceResponseIndex = 0,
                ServiceType = typeof(IncotermList),
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            IncotermList[] incoterms = (IncotermList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Incoterms Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Incoterms Failed! " + serviceOutcome.Response.Result);
            IncotermList LDEIncoterm = IncotermExist(incoterms, HybridData.IncotermCodeLDE);
            if (LDEIncoterm == null)
            {
                serviceResponse = CreateIncotermCodeLDE();
                Assert.IsFalse(serviceResponse.HasError, "Create Incoterm Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Create Incoterm Failed! " + serviceResponse.Result);
            }
        }
        private static Response CreateIncotermCodeLDE()
        {
            IncotermPM incotermPM = new IncotermPM()
            {
                Code = HybridData.IncotermCodeLDE,
                Name = "LDE Incoterm",
                Freight = "P",
                OtherCharges = "C",
                Tenant = EnvironmentGlobalParams.MainTenant
            };
            Response serviceResponse = AssertResponse(incotermPM);
            return serviceResponse;
        }
        private static IncotermList IncotermExist(IncotermList[] incoterms, string code)
        {
            for (int i = 0; i < incoterms.Length; i++)
            {
                if (incoterms[i].Code == code)
                    return incoterms[i];
            }
            return null;
        }
        private static void GetChargeTypeCodeAFT()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "ChargeType",
                ServiceOperation = "GetChargesTypes",
                ServiceResponseIndex = 3,
                ServiceType = typeof(ChargesTypeList),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, 0, 10, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ChargesTypeList[] chargesTypes = (ChargesTypeList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Charge Types Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Charge Types Failed! " + serviceOutcome.Response.ErrorMessage);
            if (chargesTypes.Length == 0)
                Assert.Inconclusive("There Isn't Charge Types!");

            ChargesTypeList AFTChargeType = ChargeTypeExist(chargesTypes);
            if (AFTChargeType == null)
                Assert.Fail("Prepare ChargeTypeIdAFT Failed!");
        }
        private static ChargesTypeList ChargeTypeExist(ChargesTypeList[] chargesTypes)
        {
            for (int i = 0; i < chargesTypes.Length; i++)
            {
                if (chargesTypes[i].Code == HybridData.ChargeTypeCodeAFT)
                    return chargesTypes[i];
            }
            return null;
        }
        private static void UpsertStateCodeAK()
        {
            StatePM statePM = new StatePM()
            {
                Code = HybridData.StateCodeAK,
                EnglishName = "Alaska",
                LocalName = "Alaska",
                CountryId = HybridData.CountryCodeUS,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(statePM);
        }
        private static void UpsertAgentTest()
        {
            AgentPM agentPM = new AgentPM()
            {
                Code = HybridData.AgentCodeHAgent,
                EnglishName = "TestAgentExport1",
                LocalName = "Hybrid Agent",
                CityName = "Washnton",
                CountryCode = HybridData.CountryCodeUS,
                PartnerTypeId = "AG",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            agentPM.Addresses.Add(new AddressPM
            {
                Tenant = EnvironmentGlobalParams.MainTenant,
                Description = "Main Address",
                City = "Washnton",
                StateCode = HybridData.StateCodeAK,
                ZipCode = "000970",
                FaxNumber = "1234123",
                PhoneNumber = "12341234",
                AddressTypeId = "M",
                Address1 = "adddresss 111",
                Address2 = "adddresss 3222",
                Name = "TestAgentExport1",
                CountryCode = HybridData.CountryCodeUS,
                CardCode = "new",

            });
            Response serviceResponse = AssertResponse(agentPM);
        }
        private static void UpsertContactTest()
        {
            ContactPM contactPM = new ContactPM()
            {
                EnglishName = HybridData.ContactCode,
                LocalName = "Hybrid Contact",
                Email = "HybridContact@logitudeworld.com",
                Password = "!H0",
                ExternalId = HybridData.ContactCode,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(contactPM);
        }
        private static void UpsertVesselCodeHV()
        {
            VesselPM vesselPM = new VesselPM()
            {
                Code = HybridData.VesselCodeHV,
                EnglishName = "Hybrid Vessel",
                LocalName = "Hybrid Vessel",
                IMOCode = "IMOCode HV",
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(vesselPM);
        }
        private static void UpsertVendorCodeHVEN()
        {
            VendorPM vendorPM = new VendorPM()
            {
                Code = HybridData.VendorCodeHVEN,
                EnglishName = "Hybrid Vendor",
                LocalName = "Hybrid Vendor",
                CountryCode = HybridData.CountryCodeUS,
                PartnerTypeId = "VD",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            vendorPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Name = "vendor address",
                Description = "Main Address",
                City = "New York",
                CountryCode = HybridData.CountryCodeUS,
                CardCode = "new",
            });
            Response serviceResponse = AssertResponse(vendorPM);
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare Shipment Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare Shipment Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
