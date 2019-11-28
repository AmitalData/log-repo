using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{
    [TestClass]
    class HybridTestInitializer
    {
        [AssemblyInitialize]
        public static void PrepareSystemVars(TestContext context)
        {
            AuthSuccessfull();
            GetCurrencyIdEUR();
            GetIncotermIdCIF();
            GetChargeTypeIdAFT();
            GetPortIdLHR();
            GetPortIdLAS();
            GetPortIdAirJFK();
            GetPortIdOceanSOU();
            GetPortIdInlandNYC();
            GetPortIdLON();
            GetPortIdMAN();


            UpsertGlobalZone();
            UpsertCountry();
            UpsertVendor();
            UpsertCurrency();
            UpsertFromPort();
            UpsertToPort();
            UpsertDepartment();
            UpsertBranch();
            UpsertAgent();
            UpsertUser();
        }
        private static void AuthSuccessfull()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey, Tenant = TestEnvironmentGlobalParameters.Tenant };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            Response loginResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if (!loginResponse.HasError)
                TestEnvironmentGlobalParameters.Token = loginResponse.Result;
            else
                Assert.Fail("Login Failed");
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
                CopyCurrencyFromTenant0ToTestTenant(ref currencies[0]);
            }
            HybridData.CurrencyIdEUR = currencies[0].Id;
        }
        private static void CopyCurrencyFromTenant0ToTestTenant(ref CurrencyList currencyPM)
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
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(incotermPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
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
        private static void GetPortIdLHR()
        {
            PortList LHRport = GetPortId(HybridData.PortCodeLHR);
            HybridData.PortIdLHR = LHRport.Id;
            HybridData.CountryIdForPortLHR = LHRport.CountryId;
        }
        private static void GetPortIdLAS()
        {
            PortList LASport = GetPortId(HybridData.PortCodeLAS+" ");
            HybridData.PortIdLAS = LASport.Id;
        }
        private static void GetPortIdMIA()
        {
            PortList MIAport = GetPortId(HybridData.PortCodeMIA);
            HybridData.PortIdMIA = MIAport.Id;
        }
        private static void GetPortIdAirJFK()
        {
            PortList JFKport = GetPortId(HybridData.PortCodeAirJFK);
            HybridData.PortIdAirJFK = JFKport.Id;
            HybridData.CountryIdForPortJFK = JFKport.CountryId;
        }
        private static void GetPortIdOceanSOU()
        {
            PortList SOUport = GetPortId(HybridData.PortCodeOceanSOU+ "tham");
            HybridData.PortIdOceanSOU = SOUport.Id;
        }
        private static void GetPortIdInlandNYC()
        {
            PortList NYCport = GetPortId(HybridData.PortCodeInlandNYC);
            HybridData.PortIdInlandNYC = NYCport.Id;
        }
        private static void GetPortIdLON()
        {
            PortList LONport = GetPortId(HybridData.PortCodeLON + "don");
            HybridData.PortIdLON = LONport.Id;
        }
        private static void GetPortIdMAN()
        {
            PortList SOUport = GetPortId(HybridData.PortCodeMAN + "ch");
            HybridData.PortIdMAN = SOUport.Id;
        }
        private static PortList GetPortId(string code)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Port",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PortList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = code
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PortList[] ports = (PortList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (ports.Length == 0)
            {
                serviceParameters = new object[] { filters, 0, serviceResponse };
                ports = (PortList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                CopyPortFromTenant0ToTestTenant(ref ports[0]);
            }
            return ports[0];
        }
        private static void CopyPortFromTenant0ToTestTenant(ref PortList portPM)
        {
            portPM.Tenant = TestEnvironmentGlobalParameters.Tenant;
            portPM.AddedManually = true;
            AssertResponse(portPM);
        }



        private static void UpsertGlobalZone()
        {
            GlobalZonePM entityPM = new GlobalZonePM()
            {
                Code = HybridData.GlobalZoneCode,
                EnglishName = "Hybrid GlobalZone",
                LocalName = "Hybrid GlobalZone",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertCountry()
        {
            CountryPM entityPM = new CountryPM()
            {
                Code = HybridData.CountryCode,
                EnglishName = "Hybrid Country",
                LocalName = "Hybrid Country",
                GlobalZoneId = HybridData.GlobalZoneCode,
                AddedManually = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertVendor()
        {
            VendorPM entityPM = new VendorPM()
            {
                Code = HybridData.VendorCode,
                EnglishName = "Hybrid Vendor",
                LocalName = "Hybrid Vendor",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                PartnerTypeId = "VD",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertCurrency()
        {
            CurrencyPM entityPM = new CurrencyPM()
            {
                Code = HybridData.CurrencyCode,
                EnglishName = "Hybrid Currency",
                LocalName = "Hybrid Currency",
                AddedManually = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertFromPort()
        {
            PortPM entityPM = new PortPM()
            {
                Code = HybridData.ToPortCode,
                EnglishName = "Hybrid To Port",
                LocalName = "Hybrid To Port",
                CountryCode = HybridData.CountryCode,
                CountryId = HybridData.CountryCode,
                AddedManually = true,
                IsAir = true,
                IsOcean = true,
                IsInland = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertToPort()
        {
            PortPM entityPM = new PortPM()
            {
                Code = HybridData.ToPortCode,
                EnglishName = "Hybrid To Port",
                LocalName = "Hybrid To Port",
                CountryCode = HybridData.CountryCode,
                CountryId = HybridData.CountryCode,
                AddedManually = true,
                IsAir = true,
                IsOcean = true,
                IsInland = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertDepartment()
        {
            DepartmentPM entityPM = new DepartmentPM()
            {
                Code = HybridData.DepartmentCode,
                EnglishName = "Hybrid Department",
                LocalName = "Hybrid Department",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertBranch()
        {
            BranchPM entityPM = new BranchPM()
            {
                Code = HybridData.BranchCode,
                EnglishName = "Hybrid Branch",
                LocalName = "Hybrid Branch",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertAgent()
        {
            AgentPM entityPM = new AgentPM()
            {
                Code = HybridData.AgentCode,
                EnglishName = "Hybrid Agent",
                LocalName = "Hybrid Agent",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                PartnerTypeId = "AG",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            AssertResponse(entityPM);
        }
        private static void UpsertUser()
        {
            UserPM entityPM = new UserPM()
            {
                Code = HybridData.UserCode,
                EnglishName = "Hybrid User",
                LocalName = "Hybrid User",
                Email = "Hybrid@fnarsoft.com",
                Password = "!H0",
                BusinessUnitId = TestEnvironmentGlobalParameters.Tenant.ToString(),
                BranchId = HybridData.BranchCode,
                DepartmentId = HybridData.DepartmentCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
                DocumentFilingInbox = "HybridInbox"
            };
            AssertResponse(entityPM);
        }
        private static void AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare System Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare System Vars Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
