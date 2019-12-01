using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PreparePackageTypes
    {
        public static void PreparePackageTypesVars()
        {
            GetPackageTypeIdContainerPC1();
            GetPackageTypeIdContainerPC2();
            GetPackageTypeIdContainerPP1();
            GetPackageTypeIdContainerPP2();
        }
        private static void GetPackageTypeIdContainerPC1()
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPC1 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePC1);
            else
                HybridData.PackageTypeIdPC1 = packageTypes[0].Id;
        }
        private static void GetPackageTypeIdContainerPC2()
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
                SearchFields = HybridData.PackageTypeCodePC2,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPC2 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePC2);
            else
                HybridData.PackageTypeIdPC2 = packageTypes[0].Id;
        }
        private static void GetPackageTypeIdContainerPP1()
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
                SearchFields = HybridData.PackageTypeCodePP1,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPP1 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePP1);
            else
                HybridData.PackageTypeIdPP1 = packageTypes[0].Id;
        }
        private static void GetPackageTypeIdContainerPP2()
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
                SearchFields = HybridData.PackageTypeCodePP2,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            PackageTypeList[] packageTypes = (PackageTypeList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (packageTypes.Length == 0)
                HybridData.PackageTypeIdPP2 = CreatePackageTypeIdContainer(HybridData.PackageTypeCodePP2);
            else
                HybridData.PackageTypeIdPP2 = packageTypes[0].Id;
        }
        private static string CreatePackageTypeIdContainer(string code)
        {
            PackageTypePM packageTypePM = new PackageTypePM()
            {
                Code = code,
                EnglishName = "ContainerId" + code,
                IsOcean = true,
                IsAir = false,
                IsInland = true,
                //IsContainer = true,
                //MeasurementId
                AddedManually = true,
                PrintAs = "PC'1",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = AssertResponse(packageTypePM);
            return serviceResponse.Result;
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare PackageTypes Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare PackageTypes Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
