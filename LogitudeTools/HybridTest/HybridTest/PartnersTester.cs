using HypredTest.AgentProxy;
using HypredTest.AirlineProxy;
using HypredTest.ShippingAgentProxy;
using HypredTest.ShippingLineProxy;
using HypredTest.TruckerProxy;
using HypredTest.VendorProxy;
using HypredTest.VesselProxy;
using HypredTest.WarehouseProxy;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypredTest
{
    public class PartnersTester
    {
        public Response TestAgentService(string token)
        {

            var service = new AgentWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                AgentProxy.AgentPM newEntity = new AgentProxy.AgentPM()
                {
                    Code = "HBRDAGNT",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                    PartnerTypeId = "VD",
                    VatNumber = "98956454",
                    CountryCode = "IL",
                };


                var response = service.Upsert(newEntity, false);

                if (response.HasError)
                    return response;
                
                var entityPM = service.GetAgentPM("HBRDAGNT", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestAirlineService(string token)
        {

            AirlineProxy.AirlineWcfServiceClient service = new AirlineWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                AirlineProxy.AirlinePM newEntity = new AirlineProxy.AirlinePM()
                {
                    Code = "HB",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                    //PartnerTypeId = "AL",
                    VatNumber = "98956454",
                    CountryCode = "IL",
                    Prefix  ="HB"
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;


                var entityPM = service.GetAirlinePM("HB", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestShippingAgentService(string token)
        {

            var service = new ShippingAgentWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                var newEntity = new ShippingAgentProxy.ShippingAgentPM()
                {
                    Code = "HBRDSAG",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                    PartnerTypeId = "SG",
                    VatNumber = "98956454",
                    CountryCode = "IL",
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;

                
                var entityPM = service.GetShippingAgentPM("HBRDSAG", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestShippingLineService(string token)
        {

            var service = new ShippingLineWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                var newEntity = new ShippingLineProxy.ShippingLinePM()
                {
                    Code = "HBLL",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                    //PartnerTypeId = "SL",
                    VatNumber = "98956454",
                    CountryCode = "IL",
                    CBSA = "11",
                    SCACCode = "11"
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;


                var entityPM = service.GetShippingLinePM("HBLL", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestTruckerService(string token)
        {

            var service = new TruckerWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                var newEntity = new TruckerProxy.TruckerPM()
                {
                    Code = "HBTR",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                    //PartnerTypeId = "TR",
                    VatNumber = "98956454",
                    CountryCode = "IL",
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;

                var entityPM = service.GetTruckerPM("HBTR", 1, ref response);
                entityPM.Remark = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }
        }

        public Response TestVesselService(string token)
        {

            var service = new VesselWcfServcieClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                var newEntity = new VesselProxy.VesselPM()
                {
                    Code = "HBVSL",
                    EnglishName = "hybrid entity",
                    Tenant = 1,
                   
                    //CountryId = "IL",
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;

                var entityPM = service.GetVesselPM("HBVSL", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestWarehouseService(string token)
        {

            var service = new WarehouseWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)service.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                var newEntity = new WarehouseProxy.WarehousePM()
                {
                    Code = "HBWH",
                    EnglishName = "hybrid entity",
                    Tenant = 1,

                    //CountryId = "IL",
                };


                var response = service.Upsert(newEntity, false);
                if (response.HasError)
                    return response;
                
                var entityPM = service.GetWarehousePM("HBWH", 1, ref response);
                entityPM.Notes = "Hybrid update";

                var response2 = service.Upsert(entityPM, false);

                return response2;

            }


        }

        public Response TestVendorService(string token)
        {

            VendorProxy.VendorWcfServiceClient vendorService = new VendorWcfServiceClient();

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)vendorService.InnerChannel))
            {

                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", token);

                Response resultResponse = new Response();
                // CustomerPM pm = customerservice.GetCustomerPM(new CustomerApiFilters() { ByCode = true, SearchCode = "10107933" }, 8, ref resultResponse);
                VendorProxy.VendorPM newEntity = new VendorProxy.VendorPM()
                {
                    Code = "HBRDVNDR",
                    EnglishName = "hybrid Vendor",
                    Tenant = 1,
                    PartnerTypeId = "VD",

                    VatNumber = "98956454",
                    CountryCode = "IL",
                };


                //};

                //newCustomer.CustomerSalesmanByProducts = salesmanbyproducts;

                var response = vendorService.Upsert(newEntity, false);

                if (response.HasError)
                    return response;


                var result = vendorService.GetVendorPM("HBRDVNDR", 1, ref response);

                var response2 = vendorService.Upsert(result, false);

                return response2;
             
            }

          
        }
    }
}
