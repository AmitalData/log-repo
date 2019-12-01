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
            GetCountryIdGB();
            GetCountryIdUS();
            UpsertStateIdAK();
            UpsertAgentTest();
            PrepareCustomers.PrepareCustomersVars();
            UpsertAirlineIdHA();
            UpsertAirlineIdHL();
            UpsertShippingLineIdHSL();
            UpsertShippingLineIdHSL2();
            UpsertTruckerIdHT();
            UpsertTruckerIdHT2();
            //GetLoggedTenantDB();
            //MoveType();
            UpsertVesselIdHV();
            GetPackageTypeIdContainerPC1();
            GetPackageTypeIdContainerPC2();
            GetPackageTypeIdContainerPP1();
            UpsertVendorIdHVEN();
            GetPackageTypeIdContainerPP2();
            //GetChargeTypeIdAirFreight
            //GetQuoteStage
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
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
            currencyPM.Tenant = TestEnvironmentGlobalParameters.Tenant;
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
                Tenant = TestEnvironmentGlobalParameters.Tenant
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
            object[] serviceParameters = new object[] { TestEnvironmentGlobalParameters.Tenant, 0, 10, serviceResponse };
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
        private static void GetCountryIdGB()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Country",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CountryList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = HybridData.CountryCodeGB
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CountryList[] countries = (CountryList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            HybridData.CountryIdGB = countries[0].Id;
        }
        private static void GetCountryIdUS()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Country",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CountryList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = HybridData.CountryCodeUS
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CountryList[] countries = (CountryList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            HybridData.CountryIdUS = countries[0].Id;
        }
        private static void UpsertStateIdAK()
        {
            StatePM statePM = new StatePM()
            {
                Code = HybridData.StateCodeAK,
                EnglishName = "Alaska",
                LocalName = "Alaska",
                CountryId = HybridData.CountryCodeUS,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
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
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            agentPM.Addresses.Add(new AddressPM {
                Tenant = TestEnvironmentGlobalParameters.Tenant,
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
        private static void UpsertAirlineIdHA()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHA,
                EnglishName = "Hybrid Airlines",
                LocalName = "Hybrid Airlines",
                CarrierTypeId = "AL",
                Prefix = "999",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(airlinePM);
            HybridData.AirlineIdHA = serviceResponse.Result;
        }
        private static void UpsertAirlineIdHL()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHL,
                EnglishName = "Hybrid 2 Airlines",
                LocalName = "Hybrid 2 Airlines",
                CarrierTypeId = "AL",
                Prefix = "998",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(airlinePM);
            HybridData.AirlineIdHL = serviceResponse.Result;
        }
        private static void UpsertShippingLineIdHSL()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCodeHSL,
                SCACCode = HybridData.ShippingLineCodeHSL,
                EnglishName = "Hybrid ShippingLine",
                LocalName = "Hybrid ShippingLine",
                CarrierTypeId = "SL",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(shippingLinePM);
            HybridData.ShippingLineIdHSL = serviceResponse.Result;
        }
        private static void UpsertShippingLineIdHSL2()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCodeHSL2,
                SCACCode = HybridData.ShippingLineCodeHSL2,
                EnglishName = "Hybrid 2 ShippingLine",
                LocalName = "Hybrid 2 ShippingLine",
                CarrierTypeId = "SL",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(shippingLinePM);
            HybridData.ShippingLineIdHSL = serviceResponse.Result;
        }
        private static void UpsertTruckerIdHT()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid Trucker",
                LocalName = "Hybrid Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            truckerPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                Name = "Hybrid City",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "Hybrid City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            Response serviceResponse = AssertResponse(truckerPM);
            HybridData.TruckerIdHT = serviceResponse.Result;
        }
        private static void UpsertTruckerIdHT2()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid 2 Trucker",
                LocalName = "Hybrid 2 Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            truckerPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                Name = "Hybrid 2 City",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "Hybrid 2 City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            Response serviceResponse = AssertResponse(truckerPM);
            HybridData.TruckerIdHT2 = serviceResponse.Result;
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
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(vesselPM);
            HybridData.VesselIdHV = serviceResponse.Result;
        }
        private static void GetPackageTypeIdContainerPC1()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "GetPackageTypeList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PackageTypeList),
                ServiceFilterType = typeof(PackageTypeServiceReference.PackageTypeApiFilters),
            };
            PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters
            {
                Take = 10,
                SearchFields = HybridData.PackageTypeCodePC1,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPC1 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePC1);
            else
                HybridData.PackageTypeIdPC1 = packageTypes[0].Id;
        }
        private static void GetPackageTypeIdContainerPC2()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "GetPackageTypeList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PackageTypeList),
                ServiceFilterType = typeof(PackageTypeServiceReference.PackageTypeApiFilters),
            };
            PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters
            {
                Take = 10,
                SearchFields = HybridData.PackageTypeCodePC2,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPC2 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePC2);
            else
                HybridData.PackageTypeIdPC2 = packageTypes[0].Id;
        }
        private static void GetPackageTypeIdContainerPP1()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "GetPackageTypeList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PackageTypeList),
                ServiceFilterType = typeof(PackageTypeServiceReference.PackageTypeApiFilters),
            };
            PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters
            {
                Take = 10,
                SearchFields = HybridData.PackageTypeCodePP1,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPP1 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePP1);
            else
                HybridData.PackageTypeIdPP1 = packageTypes[0].Id;
        }
        private static string CreatePackageTypeIdContainer(string code)
        {
            PackageTypePM packageTypePM = new PackageTypePM()
            {
                Code = code,
                EnglishName = "ContainerId"+code,
                IsOcean = true,
                IsAir = false,
                IsInland = true,
                IsContainer = true,
                //MeasurementId
                AddedManually = true,
                PrintAs = "PC'1",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = AssertResponse(packageTypePM);
            return serviceResponse.Result;
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
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            vendorPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                City = "New York",
                CountryCode = HybridData.CountryCodeUS,
                CardCode = "new",
            });
            Response serviceResponse = AssertResponse(vendorPM);
            HybridData.VendorIdHVEN = serviceResponse.Result;
        }
        private static void GetPackageTypeIdContainerPP2()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "GetPackageTypeList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PackageTypeList),
                ServiceFilterType = typeof(PackageTypeServiceReference.PackageTypeApiFilters),
            };
            PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters
            {
                Take = 10,
                SearchFields = HybridData.PackageTypeCodePP2,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPP2 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePP2);
            else
                HybridData.PackageTypeIdPP2 = packageTypes[0].Id;
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
