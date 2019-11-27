using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CountryWcfCaller
    {
        public static Response CallCountryUpsert()
        {
            Response prepareResponse = GlobalZoneWcfCaller.PrepareGlobalZone();
            if (!prepareResponse.HasError)
            {
                CountryPM entityPM = new CountryPM()
                {
                    Code = HybridData.CountryCode,
                    EnglishName = "Hybrid Country",
                    LocalName = "Hybrid Country",
                    GlobalZoneId = HybridData.GlobalZoneCode,
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Country",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(CountryPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.CountryId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareCountry()
        {
            if(HybridData.CountryId == null)
            {
                return CallCountryUpsert();
            }
            return new Response();
        }
    }
}
