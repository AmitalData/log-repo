using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.CloseTables;
using Logitude.BL.ShipmentsModel.EntityPMs;
using RestSharp;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.ContainerTracking
{
    public class VizionService
    {
        ContainerTrackingProvider Source;
        public VizionService()
        {
            Initializer();
        }
        public VizionService(ContainerTrackingProvider source)
        {
            Source = source;
        }

        private void Initializer()
        {
            var context = ShipmentsContext.GetContext(0);
            Source = context.ContainerTrackingProviders.Where(e => e.SourceCode == "VZN").FirstOrDefault();
        }

        public VizionReferenceResponce SendRequest(GeneralContainerTrackingArgs containerTrackingArgs, Shipment shipment)
        {
            if (containerTrackingArgs.IsFromContainer)
            {
                return CallCreateReferenceViaCarrierCodeApi(containerTrackingArgs, shipment);
            }
            else
            {
                return CallCreateReferenceViaBillofLading(shipment);
            }
        }

        private VizionReferenceResponce CallCreateReferenceViaBillofLading(Shipment shipment)
        {
            var headers = GetHeaders();
            var referenceViaBillOfLadingRequest = CreateCreateReferenceViaBillOfLadingRequest(shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(Source.ProviderURL + "/references", referenceViaBillOfLadingRequest, Method.Post, headers);
            return result;

        }
        public List<VizionCarrier> GetAllCarriers()
        {
            var headers = GetHeaders();
            var result = APICaller.CallApi<List<VizionCarrier>>(Source.ProviderURL+ "/carriers", null, Method.Get, headers);
            return result;
        }

        private List<KeyValuePair<string, string>> GetHeaders()
        {
            var headers =  new List<KeyValuePair<string, string>>();
            headers.Add(new KeyValuePair<string, string>("X-API-Key", Source.APIKey));
            return headers;
        }

        private ReferenceViaCarrierCodeRequest CreateReferenceViaCarrierCodeRequest(GeneralContainerTrackingArgs containerTrackingArgs, Shipment shipment)
        {
            return new ReferenceViaCarrierCodeRequest()
            {
                callback_url = Source.CallbackURL,
                carrier_code = containerTrackingArgs.CarrierCode,
                bill_of_lading = shipment?.ShipmentMasterData?.Master,
                container_id = containerTrackingArgs.ContainerNumber
            };
        }
        private CreateReferenceViaBillOfLadingRequest CreateCreateReferenceViaBillOfLadingRequest(Shipment shipment)
        {
            return new CreateReferenceViaBillOfLadingRequest()
            {
                callback_url = Source.CallbackURL,
                carrier_code = shipment.ShipmentMasterData.MainCarriageCarrierCard.Code,
                bill_of_lading = shipment.ShipmentMasterData.Master
            };
        }

        private VizionReferenceResponce CallCreateReferenceViaCarrierCodeApi(GeneralContainerTrackingArgs containerTrackingArgs, Shipment shipment)
        {
            var headers = GetHeaders();
            var referenceViaCarrierCodeRequest = CreateReferenceViaCarrierCodeRequest(containerTrackingArgs, shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(Source.ProviderURL+ "/references", referenceViaCarrierCodeRequest, Method.Post, headers);
            return result;
        }

        public UnsubscribeResult Unsubscribe(ContainerTrackingRequestPM containerTrackingRequest)
        {
            var headers = GetHeaders();
            var result = APICaller.CallApi<UnsubscribeResult>(Source.ProviderURL + "/references/"+ containerTrackingRequest.RequestId, null, Method.Delete, headers);
            return result;
        }

        internal object GetActiveRequests()
        {
            var headers = GetHeaders();
            var result = APICaller.CallApi<List<ActiveRequest>>(Source.ProviderURL + "/references", null, Method.Get, headers);
            return result;
        }
    }
}
