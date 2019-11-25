using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.CustomBankCardServiceReference;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CustomBankCardWcfCaller
    {
        public static Response CallCustomBankCardUpsert()
        {
            CustomBankPM entityPM = new CustomBankPM()
            {
                BankCode = HybridData.BankCode,
                EnglishName = "Hybrid Custom Bank",
                LocalName = "Hybrid Custom Bank",
                CardId = "?",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CustomBankCard",
                ServiceOperation = "Upsert",
                ServiceType = typeof(CustomBankPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.CustomBanksCardId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareCustomBankCard()
        {
            if(HybridData.CustomBanksCardId == null)
            {
                return CallCustomBankCardUpsert();
            }
            return new Response();
        }
    }
}
