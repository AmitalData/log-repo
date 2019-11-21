using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class AccountingPartnerWcfCaller
    {
        public static Response CallAccountingPartnerUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                AccountingPartnerPM entityPM = new AccountingPartnerPM()
                {
                    Code = HybridData.AccountingPartnerCode,
                    EnglishName = "Hybrid AccountingPartner",
                    LocalName = "Hybrid AccountingPartner",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "AC",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "AccountingPartner",
                    ServiceOperation = "Upsert",
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(AccountingPartnerPM), ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.AccountingPartnerId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareAccountingPartner()
        {
            if (HybridData.AccountingPartnerId == null)
            {
                return CallAccountingPartnerUpsert();
            }
            return new Response();
        }
    }
}
