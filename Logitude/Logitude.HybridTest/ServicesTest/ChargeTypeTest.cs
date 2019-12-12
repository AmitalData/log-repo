using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ChargeTypeTest
    {
        [TestMethod]
        public void Test_ChargeType_GetChargesTypes()
        {
            //InvokedProperties serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "ChargeType",
            //    ServiceOperation = "GetChargesTypes",
            //    ServiceResponseIndex = 3,
            //    ServiceType = typeof(ChargesTypeList),
            //    ServiceFilterType = null,
            //};
            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, 0, 10, serviceResponse };
            //ChargesTypeList[] chargesTypes = (ChargesTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //Assert.IsFalse(serviceResponse.HasError, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
            //Assert.IsNull(serviceResponse.Result, "Get Charge Types Failed! " + serviceResponse.ErrorMessage);
            //if(chargesTypes.Length == 0)
            //    Assert.Inconclusive("There Isn't Charge Types!");
        }
    }
}
