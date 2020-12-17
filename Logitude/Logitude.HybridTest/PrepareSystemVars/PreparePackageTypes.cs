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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PreparePackageTypesVars()
        {
            GetPackageTypeCodeContainer(HybridData.PackageTypeCodePC1);
            GetPackageTypeCodeContainer(HybridData.PackageTypeCodePC2);
            GetPackageTypeCodeContainer(HybridData.PackageTypeCodePP1);
            GetPackageTypeCodeContainer(HybridData.PackageTypeCodePP2);
        }
        private static void GetPackageTypeCodeContainer(string code)
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
                SearchFields = code,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            PackageTypeList[] packageTypes = (PackageTypeList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get PackageType Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get PackageType Failed! " + serviceOutcome.Response.Result);
            if (packageTypes.Length == 0)
                CreatePackageTypeIdContainer(code);
        }
        private static void CreatePackageTypeIdContainer(string code)
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
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(packageTypePM);
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare PackageTypes Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare PackageTypes Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome.Response;
        }
    }
}
