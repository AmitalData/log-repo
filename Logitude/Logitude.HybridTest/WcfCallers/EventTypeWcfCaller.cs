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
    class EventTypeWcfCaller
    {
        public static Response CallEventTypeUpsert()
        {
            EventTypePM entityPM = new EventTypePM()
            {
                Code = HybridData.EventTypeCode,
                EnglishName = "Hybrid EventType",
                LocalName = "Hybrid EventType",
                ObjectTableName = "Shipment",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "EventType",
                ServiceOperation = "Upsert",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(EventTypePM), ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.EventTypeId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareEventType()
        {
            if(HybridData.EventTypeId == null)
            {
                return CallEventTypeUpsert();
            }
            return new Response();
        }
    }
}
