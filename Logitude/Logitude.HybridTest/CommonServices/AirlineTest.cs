using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AirlineTest
    {
        [TestMethod]
        public void Test_Airline_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = AirlineWcfCaller.CallAirlineUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
