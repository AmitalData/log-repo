using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.CRM.BL.EntityPMs;

namespace Logitude.HybridTest.WcfCallers
{
    class ActivityWcfCaller
    {
        public static Response CallActivityUpsert()
        {
            ActivityPM entityPM = new ActivityPM()
            {
                Subject = "Hybrid Activity",
                Description = "Hybrid Activity",
                ActivityTypeCode = "TS", //TS:Task, AP:Appointment, CL:Phone Cell
                ActivityStatusCode = "N", //N:Not Started
                Tenant = TestEnvironmentGlobalParameters.Tenant
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Activity",
                ServiceOperation = "Upsert",
                ServiceType = typeof(ActivityPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, "angular@fnarsoft.com" };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.ActivityId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareActivity()
        {
            if(HybridData.ActivityId == null)
            {
                return CallActivityUpsert();
            }
            return new Response();
        }
    }
}
