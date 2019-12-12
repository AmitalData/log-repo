using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CurrencyTest
    {
        [TestMethod]
        public void Test_Currency_UPSERT()
        {
            CurrencyPM currencyPM = new CurrencyPM()
            {
                Code = HybridData.CurrencyCodeHCR,
                EnglishName = "Hybrid Currency",
                LocalName = "Hybrid Currency",
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(currencyPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Currency_GETLIST()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Currency",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CurrencyList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = HybridData.CurrencyCodeEUR
            };
            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            //CurrencyList[] currencies = (CurrencyList[]) WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            //Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            //Assert.AreEqual(currencies[0].Id, HybridData.CurrencyIdEUR, "Get Hybrid Currency Item From Currencies Failed!");
        }
    }
}
