using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class ContactWcfCaller
    {
        public static Response CallContactUpsert()
        {
            ContactPM entityPM = new ContactPM()
            {
                EnglishName = HybridData.ContactCode,
                LocalName = "Hybrid Contact",
                Email = "HybridContact@logitudeworld.com",
                Password = "!H0",
                ExternalId = HybridData.ContactCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "Upsert",
                ServiceType = typeof(ContactPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.ContactId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareContact()
        {
            if(HybridData.ContactId == null)
            {
                return CallContactUpsert();
            }
            return new Response();
        }
    }
}
