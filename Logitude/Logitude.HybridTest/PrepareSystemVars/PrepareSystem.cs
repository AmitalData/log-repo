using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] //0 means use as many workers as possible

namespace Logitude.HybridTest.WcfCallers
{
    [TestClass]
    class PrepareSystem
    {
        [AssemblyInitialize]
        public static void PrepareSystemVars(TestContext context)
        {
            GetAuthenticationMainToken();
            //GetAuthenticationsecondaryToken();
            //PrepareShipment.PrepareShipmentVars();

            ////Other necessary Vars:
            //UpsertGlobalZone();
            //UpsertDepartment();
            //UpsertBranch();
            //UpsertUser();
            //UpsertCardContact();
        }
        private static void GetAuthenticationMainToken()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentGlobalParams.MainTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentGlobalParams.MainTenant_APICredential_SecondaryKey, Tenant = EnvironmentGlobalParams.MainTenant };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            if (!serviceOutcome.Response.HasError)
                EnvironmentGlobalParams.MainToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }
        private static void GetAuthenticationsecondaryToken()
        {
            var apiCred = new APICredentialsParameters() { PrimaryKey = EnvironmentGlobalParams.SecondaryTenant_APICredential_PrimaryKey, SecondaryKey = EnvironmentGlobalParams.SecondaryTenant_APICredential_SecondaryKey, Tenant = EnvironmentGlobalParams.SecondaryTenant };
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Login",
                ServiceOperation = "LoginByCredential",
                ServiceType = typeof(APICredentialsParameters),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "", apiCred };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            if (!serviceOutcome.Response.HasError)
                EnvironmentGlobalParams.SecondaryToken = serviceOutcome.Response.Result;
            else
                Assert.Fail("Login Failed");
            Assert.IsNotNull(serviceOutcome.Response.Result, "The Token returned is null " + serviceOutcome.Response.ErrorMessage);
        }
        private static void UpsertGlobalZone()
        {
            GlobalZonePM globalZonePM = new GlobalZonePM()
            {
                Code = HybridData.GlobalZoneCodeHZ,
                EnglishName = "Hybrid GlobalZone",
                LocalName = "Hybrid GlobalZone",
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                BusinessUnitId = EnvironmentGlobalParams.MainTenant.ToString(),
                BranchId = HybridData.BranchCodeHBRA,
                DepartmentId = HybridData.DepartmentCodeHDEP,
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                Tenant = EnvironmentGlobalParams.MainTenant,
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
