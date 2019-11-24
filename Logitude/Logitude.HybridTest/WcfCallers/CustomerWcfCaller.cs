using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CustomerWcfCaller
    {
        public static Response CallCustomerUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                CustomerPM entityPM = new CustomerPM()
                {
                    Code = HybridData.CustomerCode,
                    EnglishName = "Hybrid Customer",
                    LocalName = "Hybrid Customer",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "CS",
                    IsCustomer = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Customer",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(CustomerPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.CustomerId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareCustomer()
        {
            if (HybridData.CustomerId == null)
            {
                return CallCustomerUpsert();
            }
            return new Response();
        }
    }
}
