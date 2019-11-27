using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class EntityStatusWcfCaller
    {
        public static Response PrepareEntityStatus()
        {
            if (HybridData.EntityStatusId == null)
            {
                return CallEntityStatusUpsert();
            }
            return new Response();
        }
        public static Response CallEntityStatusUpsert()
        {
            EntityStatusPM entityPM = new EntityStatusPM()
            {
                Code = HybridData.EntityStatusCode,
                Name = "Hybrid EntityStatus",
                DisplayName = "Hybrid EntityStatus",
                InActive = false,
                ObjectTableName = "Shipment",
                StatusWeight = 0,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "EntityStatus",
                ServiceOperation = "Upsert",
                ServiceType = typeof(EntityStatusPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.EntityStatusId = serviceResponse.Result;
            return serviceResponse;
        }
    }
}
