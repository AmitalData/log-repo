using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class DepartmentWcfCaller
    {
        public static Response CallDepartmentUpsert()
        {
            DepartmentPM entityPM = new DepartmentPM()
            {
                Code = HybridData.DepartmentCode,
                EnglishName = "Hybrid Department",
                LocalName = "Hybrid Department",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Department",
                ServiceOperation = "Upsert",
                ServiceType = typeof(DepartmentPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.DepartmentId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareDepartment()
        {
            if(HybridData.DepartmentId == null)
            {
                return CallDepartmentUpsert();
            }
            return new Response();
        }
    }
}
