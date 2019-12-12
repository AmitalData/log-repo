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
                StateId = HybridData.StateIdAK,
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(cityPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.CityIdHCity = serviceResponse.Result;
        }

        [TestMethod]
        public void Test_City_GetCityListByCode()
        {
            //if (HybridData.CityIdHCity == null)
            //    Test_City_UPSERT();
            //InvokedProperties serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "City",
            //    ServiceOperation = "GetCityListByCode",
            //    ServiceResponseIndex = 3,
            //    ServiceType = typeof(CountryCityList),
            //    ServiceFilterType = null,
            //};
            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { HybridData.CityCodeHCity, HybridData.CountryCodeUS, EnvironmentGlobalParams.MainTenant, serviceResponse };
            //CountryCityList city = (CountryCityList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            //Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            //Assert.AreEqual(city.Id, HybridData.CityIdHCity, "Get Hybrid City Failed!");
        }
    }
}
