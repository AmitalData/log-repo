using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class PackageTypeTest
    {
        [TestMethod]
        public void Test_PackageType_UPSERT()
        {
            PackageTypePM packageTypePM = new PackageTypePM()
            {
                Code = HybridData.PackageTypeCode,
                EnglishName = "Hybrid PackageType",
                LocalName = "Hybrid PackageType",
                AddedManually = true,
                PrintAs = "Hybrid PackageType",
                IsAir = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(packageTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.PackageTypeId = serviceResponse.Result;
        }

        [TestMethod]
        public void Test_PackageType_GetPackageTypeList()
        {
            if(HybridData.PackageTypeId == null)
                Test_PackageType_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "GetPackageTypeList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PackageTypeList),
                ServiceFilterType = typeof(PackageTypeServiceReference.PackageTypeApiFilters),
            };
            PackageTypeServiceReference.PackageTypeApiFilters filters = new PackageTypeServiceReference.PackageTypeApiFilters
            {
                Take = 10,
                SearchFields = HybridData.PackageTypeCode,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(packageTypes[0].Code, "HPT", "Get Hybrid Package Type Item Failed!");
        }
    }
}
