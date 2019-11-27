using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CardContactWcfCaller
    {
        public static Response CallCardContactUpsert()
        {
            CardContactPM entityPM = new CardContactPM()
            {
                IsAll = true,
                ContactId = HybridData.ContactCode,
                CardId = HybridData.AgentCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CardContact",
                ServiceOperation = "Upsert",
                ServiceType = typeof(CardContactPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if (!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.CardContactId = serviceResponse.Result;
            return serviceResponse;

        }
        public static Response PrepareCardContact()
        {
            if (HybridData.CardContactId == null)
            {
                return CallCardContactUpsert();
            }
            return new Response();
        }
    }
}
