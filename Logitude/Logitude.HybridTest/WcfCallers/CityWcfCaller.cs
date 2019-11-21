using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CityWcfCaller
    {
        public static Response CallCityUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                CountryCityPM entityPM = new CountryCityPM()
                {
                    Code = HybridData.CityCode,
                    EnglishName = "Hybrid City",
                    LocalName = "Hybrid City",
                    CountryId = HybridData.CountryCode,
                    AddedManually = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "City",
                    ServiceOperation = "Upsert",
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(CountryCityPM), ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.CityId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareCity()
        {
            if (HybridData.CityId == null)
            {
                return CallCityUpsert();
            }
            return new Response();
        }
    }
}
