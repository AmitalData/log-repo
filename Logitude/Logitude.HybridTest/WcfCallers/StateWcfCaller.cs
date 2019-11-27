using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class StateWcfCaller
    {
        public static Response CallStateUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                StatePM entityPM = new StatePM()
                {
                    Code = HybridData.StateCode,
                    EnglishName = "Hybrid State",
                    LocalName = "Hybrid State",
                    CountryId = HybridData.CountryCode,
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "State",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(StatePM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.StateId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareState()
        {
            if (HybridData.StateId == null)
            {
                return CallStateUpsert();
            }
            return new Response();
        }
    }
}
