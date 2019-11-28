using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
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
            CountryCityPM cityPM = new CountryCityPM()
            {
                Code = HybridData.CityCode,
                EnglishName = "Hybrid City",
                LocalName = "Hybrid City",
                CountryId = HybridData.CountryCode,
                AddedManually = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(cityPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
        [TestMethod]
        public void Test_City_GetCityListByCode()
        {
            Test_City_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "City",
                ServiceOperation = "GetCityListByCode",
                ServiceResponseIndex = 3,
                ServiceType = typeof(CountryCityList),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.CityCode, HybridData.CountryCode, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CountryCityList city = (CountryCityList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.AreEqual(city.EnglishName, "Hybrid City", "Get Hybrid City Failed!");
        }
    }
}
