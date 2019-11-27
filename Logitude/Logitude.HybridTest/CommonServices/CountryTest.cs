using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CountryTest
    {
        [TestMethod]
        public void Test_Country_UPSERT()
        {
            CountryPM countryPM = new CountryPM()
            {
                Code = HybridData.CountryCode,
                EnglishName = "Hybrid Country",
                LocalName = "Hybrid Country",
                GlobalZoneId = HybridData.GlobalZoneCode,
                AddedManually = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(countryPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }
        [TestMethod]
        public void Test_Country_GETLIST()
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
                SearchFields = HybridData.CountryCode
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CountryList[] countries = (CountryList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.AreEqual(countries[0].EnglishName, "Hybrid Country", "Get Hybrid Country Item From Countries Failed!");
        }
    }
}
