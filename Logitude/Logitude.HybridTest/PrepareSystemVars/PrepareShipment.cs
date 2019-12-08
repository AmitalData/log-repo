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
            GetCurrencyIdEUR();
            GetIncotermIdCIF();
            GetChargeTypeIdAFT();
            PreparePorts.PreparePortsVars();
            PrepareCountries.PrepareCountriesVars();
            UpsertStateIdAK();
            UpsertAgentTest();
            UpsertContactTest();
            PrepareCustomers.PrepareCustomersVars();
            PrepareAirlines.PrepareAirlinesVars();
            PrepareShippingLines.PrepareShippingLinesVars();
            PrepareTruckers.PrepareTruckersVars();
            UpsertVesselIdHV();
            PreparePackageTypes.PreparePackageTypesVars();
            UpsertVendorIdHVEN();
        }
        private static void GetCurrencyIdEUR()
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
            CurrencyList[] currencies = (CurrencyList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            if (currencies.Length == 0)
            {
                serviceParameters = new object[] { filters, 0, serviceResponse };
                currencies = (CurrencyList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                CopyCurrencyFromTenant0ToTestTenant(currencies[0]);
            }
            HybridData.CurrencyIdEUR = currencies[0].Id;
        }
        private static void CopyCurrencyFromTenant0ToTestTenant(CurrencyList currencyPM)
        {
            currencyPM.Tenant = EnvironmentGlobalParams.MainTenant;
            currencyPM.AddedManually = true;
            AssertResponse(currencyPM);
        }
        private static void GetIncotermIdCIF()
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
            IncotermList[] incoterms = (IncotermList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Incoterms Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Incoterms Failed! " + serviceResponse.Result);
            if (incoterms.Length == 0)
                Assert.Inconclusive("There Isn't Any Incoterm!");
            IncotermList CIFIncoterm = IncotermExist(incoterms, HybridData.IncotermCodeCIF);
            if (CIFIncoterm != null)
                HybridData.IncotermIdCIF = CIFIncoterm.Id;
            else
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
            IncotermList[] incoterms = (IncotermList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Incoterms Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Incoterms Failed! " + serviceResponse.Result);
            IncotermList LDEIncoterm = IncotermExist(incoterms, HybridData.IncotermCodeLDE);
            if (LDEIncoterm != null)
                HybridData.IncotermIdLDE = LDEIncoterm.Id;
            else
                HybridData.IncotermIdLDE = CreateIncotermIdLDE();
        }
        private static string CreateIncotermIdLDE()
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
            return serviceResponse.Result;
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
        private static void GetChargeTypeIdAFT()
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
            ChargesTypeList[] chargesTypes = (ChargesTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
            if (chargesTypes.Length == 0)
                Assert.Inconclusive("There Isn't Charge Types!");

            ChargesTypeList AFTChargeType = ChargeTypeExist(chargesTypes);
            if (AFTChargeType != null)
            {
                HybridData.ChargeTypeIdAFT = AFTChargeType.Id;
                HybridData.ChargeTypeIATACodeId = AFTChargeType.IATACodeId;
                HybridData.ChargeTypeVatTypeId = AFTChargeType.VatTypeId;
            }
            else
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
        private static void UpsertStateIdAK()
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
            HybridData.StateIdAK = serviceResponse.Result;
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
            agentPM.Addresses.Add(new AddressPM {
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
            HybridData.StateIdAK = serviceResponse.Result;
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
            HybridData.StateIdAK = serviceResponse.Result;
        }
        private static void UpsertVesselIdHV()
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
            HybridData.VesselIdHV = serviceResponse.Result;
        }
        private static void UpsertVendorIdHVEN()
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
            HybridData.VendorIdHVEN = serviceResponse.Result;
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
