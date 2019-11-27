using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class AddressWcfCaller
    {
        public static Response CallAddressUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                //prepareResponse = CustomerWcfCaller.PrepareCustomer();
                if (!prepareResponse.HasError)
                {
                    AddressPM entityPM = new AddressPM()
                    {
                        ExternalId = HybridData.AddressCode,
                        Name = "Hybrid Address",
                        City = "Hybrid City",
                        AddressTypeId = "M",
                        Description = "Main Address",
                        CountryId = HybridData.CountryCode,
                        CardId = HybridData.CustomerCode,
                        Tenant = TestEnvironmentGlobalParameters.Tenant,
                    };

                    InvokedProperties serviceProperties = new InvokedProperties
                    {
                        ServiceName = "Address",
                        ServiceOperation = "Upsert",
                        ServiceType = typeof(AddressPM),
                        ServiceFilterType = null,
                    };

                    Response serviceResponse = new Response();
                    object[] serviceParameters = new object[] { entityPM, false };
                    WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                    if (!serviceResponse.HasError && serviceResponse.Result != null)
                        HybridData.AddressId = serviceResponse.Result;
                    return serviceResponse;
                }
            }
            return prepareResponse;
        }
        public static Response PrepareAddress()
        {
            if (HybridData.AddressId == null)
            {
                return CallAddressUpsert();
            }
            return new Response();
        }
    }
}
