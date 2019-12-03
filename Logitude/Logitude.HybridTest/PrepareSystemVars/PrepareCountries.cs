using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareCountries
    {
        public static void PrepareCountriesVars()
        {
            GetCountryIdGB();
            GetCountryIdUS();
        }
        private static void GetCountryIdGB()
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
                SearchFields = HybridData.CountryCodeGB
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            CountryList[] countries = (CountryList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            HybridData.CountryIdGB = countries[0].Id;
        }
        private static void GetCountryIdUS()
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
            CountryList[] countries = (CountryList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            HybridData.CountryIdUS = countries[0].Id;
        }
    }
}
