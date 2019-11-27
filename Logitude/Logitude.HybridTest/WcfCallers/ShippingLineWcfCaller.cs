using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class ShippingLineWcfCaller
    {
        public static Response CallShippingLineUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                ShippingLinePM entityPM = new ShippingLinePM()
                {
                    Code = HybridData.ShippingLineCode,
                    SCACCode = HybridData.ShippingLineCode,
                    EnglishName = "Hybrid ShippingLine",
                    LocalName = "Hybrid ShippingLine",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    CarrierTypeId = "SL",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "ShippingLine",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(ShippingLinePM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.ShippingLineId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareShippingLine()
        {
            if (HybridData.ShippingLineId == null)
            {
                return CallShippingLineUpsert();
            }
            return new Response();
        }
    }
}
