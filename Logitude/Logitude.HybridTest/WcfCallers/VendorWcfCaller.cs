using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class VendorWcfCaller
    {
        public static Response CallVendorUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                VendorPM entityPM = new VendorPM()
                {
                    Code = HybridData.VendorCode,
                    EnglishName = "Hybrid Vendor",
                    LocalName = "Hybrid Vendor",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "VD",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Vendor",
                    ServiceOperation = "Upsert",
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(VendorPM), ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.VendorId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareVendor()
        {
            if(HybridData.VendorId == null)
            {
                return CallVendorUpsert();
            }
            return new Response();
        }
    }
}
