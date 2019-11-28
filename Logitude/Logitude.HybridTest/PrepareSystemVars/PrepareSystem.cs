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
            AuthSuccessfull();
            PrepareShipment.PrepareShipmentVars();
            ////Other necessary Vars:
            //UpsertGlobalZone();
            //UpsertDepartment();
            //UpsertBranch();
            //UpsertAgent();
            //UpsertUser();
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
                CountryCode = HybridData.CountryCodeUS,
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
