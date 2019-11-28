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
                EnglishName = HybridData.PackageTypeCode,
                IsOcean = true,
                IsAir = false,
                IsInland = true,
                IsContainer = true,
                //MeasurementId
                AddedManually = true,
                PrintAs = HybridData.PackageTypeCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(packageTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_PackageType_GetPackageTypeList()
        {
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
                SearchFields = HybridData.PackageTypeCodePC1,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(packageTypes[0].Id, HybridData.PackageTypeIdPC1, "Get Hybrid Package Type Item Failed!");
        }
    }
}
