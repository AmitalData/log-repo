using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CityTest
    {
        [TestMethod]
        public void Test_City_UPSERT()
        {
            CountryCityPM cityPM = new CountryCityPM()
            {
                Code = HybridData.CityCodeHCity,
                EnglishName = "Hybrid City",
                LocalName = "Hybrid City",
                CountryId = HybridData.CountryCodeUS,
                StateCode = HybridData.StateCodeAK,
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
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
            object[] serviceParameters = new object[] { HybridData.CityCodeHCity, HybridData.CountryCodeUS, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            CountryCityList city = (CountryCityList)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.AreEqual(city.Code, HybridData.CityCodeHCity, "Get Hybrid City Failed!");
        }
    }
}
