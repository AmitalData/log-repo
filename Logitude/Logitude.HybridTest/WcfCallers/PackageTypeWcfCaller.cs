using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class PackageTypeWcfCaller
    {
        public static Response CallPackageTypeUpsert()
        {
            PackageTypePM entityPM = new PackageTypePM()
            {
                Code = HybridData.PackageTypeCode,
                EnglishName = "Hybrid PackageType",
                LocalName = "Hybrid PackageType",
                AddedManually = true,
                PrintAs = "Hybrid PackageType",
                IsAir = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "PackageType",
                ServiceOperation = "Upsert",
                ServiceType = typeof(PackageTypePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.PackageTypeId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PreparePackageType()
        {
            if(HybridData.PackageTypeId == null)
            {
                return CallPackageTypeUpsert();
            }
            return new Response();
        }
    }
}
