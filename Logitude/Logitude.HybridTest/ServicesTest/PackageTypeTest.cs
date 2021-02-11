using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class PackageTypeTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_PackageType_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                PackageTypePM packageTypePM = new PackageTypePM()
                {
                    Code = HybridData.PackageTypeCodeHPT,
                    EnglishName = "Hybrid Package Type",
                    IsOcean = true,
                    IsAir = false,
                    IsInland = true,
                    IsRefrigerated = true,
                    //IsContainer = true,
                    TEU = 1.00,
                    ContainerSize = 20,
                    Volume = 0,
                    MeasurementCode = HybridData.PackageTypeCodeHPT,
                    MeasurementShortName = HybridData.PackageTypeCodeHPT + " Hybrid Package Type",
                    AddedManually = true,
                    PrintAs = HybridData.PackageTypeCodeHPT,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(packageTypePM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_PackageType_GetPackageTypeList()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
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
                object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                PackageTypeList[] packageTypes = (PackageTypeList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
                //Assert.AreEqual(packageTypes[0].Code, HybridData.PackageTypeCodePC1, "Get Hybrid Package Type Item Failed!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
