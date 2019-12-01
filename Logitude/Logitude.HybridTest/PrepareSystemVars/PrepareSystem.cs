using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{
    [TestClass]
    class PrepareSystem
    {
        [AssemblyInitialize]
        public static void PrepareSystemVars(TestContext context)
        {
            GetAuthenticationToken1();
            GetAuthenticationToken2();
            PrepareShipment.PrepareShipmentVars();

            //Other necessary Vars:
            UpsertGlobalZone();
            UpsertDepartment();
            UpsertBranch();
            UpsertUser();
            UpsertCardContact();
        }
        private static void GetAuthenticationToken1()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey1, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey1, Tenant = TestEnvironmentGlobalParameters.Tenant1 };
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
                TestEnvironmentGlobalParameters.Token1 = loginResponse.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(loginResponse.Result, "The Token returned is null " + loginResponse.ErrorMessage);
        }
        private static void GetAuthenticationToken2()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = TestEnvironmentGlobalParameters.APICredential_PrimaryKey2, SecondaryKey = TestEnvironmentGlobalParameters.APICredential_SecondaryKey2, Tenant = TestEnvironmentGlobalParameters.Tenant2 };
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
                TestEnvironmentGlobalParameters.Token2 = loginResponse.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(loginResponse.Result, "The Token returned is null " + loginResponse.ErrorMessage);
        }
        private static void UpsertGlobalZone()
        {
            GlobalZonePM globalZonePM = new GlobalZonePM()
            {
                Code = HybridData.GlobalZoneCodeHZ,
                EnglishName = "Hybrid GlobalZone",
                LocalName = "Hybrid GlobalZone",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            AssertResponse(globalZonePM);
        }
        private static void UpsertDepartment()
        {
            DepartmentPM departmentPM = new DepartmentPM()
            {
                Code = HybridData.DepartmentCodeHDEP,
                EnglishName = "Hybrid Department",
                LocalName = "Hybrid Department",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            AssertResponse(departmentPM);
        }
        private static void UpsertBranch()
        {
            BranchPM branchPM = new BranchPM()
            {
                Code = HybridData.BranchCodeHBRA,
                EnglishName = "Hybrid Branch",
                LocalName = "Hybrid Branch",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            AssertResponse(branchPM);
        }
        private static void UpsertAgent()
        {
            AgentPM agentPM = new AgentPM()
            {
                Code = HybridData.AgentCodeHAgent,
                EnglishName = "Hybrid Agent",
                LocalName = "Hybrid Agent",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCodeUS,
                PartnerTypeId = "AG",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            AssertResponse(agentPM);
        }
        private static void UpsertUser()
        {
            UserPM userPM = new UserPM()
            {
                Code = HybridData.UserCodeHU,
                EnglishName = "Hybrid User",
                LocalName = "Hybrid User",
                Email = "Hybrid@fnarsoft.com",
                Password = "!H0",
                BusinessUnitId = TestEnvironmentGlobalParameters.Tenant1.ToString(),
                BranchId = HybridData.BranchCodeHBRA,
                DepartmentId = HybridData.DepartmentCodeHDEP,
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
                DocumentFilingInbox = "HybridInbox"
            };
            AssertResponse(userPM);
        }
        private static void UpsertCardContact()
        {
            CardContactPM cardContactPM = new CardContactPM()
            {
                IsAll = true,
                ContactId = HybridData.ContactCode,
                CardId = HybridData.AgentCodeHAgent,
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            AssertResponse(cardContactPM);
        }
        private static void AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare System Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare System Vars Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
