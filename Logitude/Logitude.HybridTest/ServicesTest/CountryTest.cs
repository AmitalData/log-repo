using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CountryTest
    {
        [TestMethod]
        public void Test_Country_UPSERT()
        {
            CountryPM countryPM = new CountryPM()
            {
                Code = HybridData.CountryCodeHC,
                EnglishName = "Hybrid Country",
                LocalName = "Hybrid Country",
                GlobalZoneId = HybridData.GlobalZoneCodeHZ,
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
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
                SearchFields = HybridData.CountryCodeUS
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CountryList[] countries = (CountryList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.AreEqual(countries[0].Code, HybridData.CountryCodeUS, "Get Hybrid Country Item From Countries Failed!");
        }
    }
}
