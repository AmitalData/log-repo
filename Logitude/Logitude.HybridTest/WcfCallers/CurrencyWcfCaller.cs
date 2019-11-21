using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CurrencyWcfCaller
    {
        public static Response CallCurrencyUpsert()
        {
            CurrencyPM entityPM = new CurrencyPM()
            {
                Code = HybridData.CurrencyCode,
                EnglishName = "Hybrid Currency",
                LocalName = "Hybrid Currency",
                AddedManually = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Currency",
                ServiceOperation = "Upsert",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(CurrencyPM), ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.CurrencyId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareCurrency()
        {
            if(HybridData.CurrencyId == null)
            {
                return CallCurrencyUpsert();
            }
            return new Response();
        }
    }
}
