using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CityTest
    {
        [TestMethod]
        public void Test_City_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CityWcfCaller.CallCityUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_City_GetCityListByCode()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CityWcfCaller.PrepareCity();
            Assert.IsFalse(prepareResponse.HasError, "Prepare City Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "City",
                ServiceOperation = "GetCityListByCode",
                ServiceResponseIndex = 3,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.CityCode, HybridData.CountryCode, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CountryCityList city = (CountryCityList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(CountryCityList), ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsTrue(CheckResult(city), "Get Hybrid City Failed!");
        }
        public bool CheckResult(CountryCityList city)
        {
            return city.EnglishName == "Hybrid City";
        }
    }
}
