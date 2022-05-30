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

        private void Initializer()
        {
            var context = ShipmentsContext.GetContext(0);
            Source = context.ContainerTrackingProviders.Where(e => e.SourceCode == ContainerStatusSourceValues.Vizion).FirstOrDefault();
        }

        public VizionReferenceResponce SendRequest(GeneralContainerTrackingArgs containerStatusSimulatorArgs, Shipment shipment)
        {
            if (containerStatusSimulatorArgs.IsFromContainer)
            {
                return CallCreateReferenceViaCarrierCodeApi(containerStatusSimulatorArgs, shipment);
            }
            else
            {
                return CallCreateReferenceViaBillofLading(shipment);
            }
        }

        private VizionReferenceResponce CallCreateReferenceViaBillofLading(Shipment shipment)
        {
            var referenceViaBillOfLadingRequest = CreateCreateReferenceViaBillOfLadingRequest(shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(Source.ProviderURL + "/references", referenceViaBillOfLadingRequest, Method.POST);
            return result;

        }
        public List<VizionCarrier> GetAllCarriers()
        {
            var result = APICaller.CallApi<List<VizionCarrier>>(Source.ProviderURL+ "/carriers", null, Method.GET);
            return result;
        }

        private ReferenceViaCarrierCodeRequest CreateReferenceViaCarrierCodeRequest(GeneralContainerTrackingArgs containerStatusSimulatorArgs, Shipment shipment)
        {
            return new ReferenceViaCarrierCodeRequest()
            {
                callback_url = Source.CallbackURL,
                carrier_code = shipment.ShipmentMasterData.MainCarriageCarrierCard.Code,
                container_id = containerStatusSimulatorArgs.ContainerNumber
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

        private VizionReferenceResponce CallCreateReferenceViaCarrierCodeApi(GeneralContainerTrackingArgs containerStatusSimulatorArgs, Shipment shipment)
        {
            var referenceViaCarrierCodeRequest = CreateReferenceViaCarrierCodeRequest(containerStatusSimulatorArgs, shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(Source.ProviderURL+ "/references", referenceViaCarrierCodeRequest, Method.POST);
            return result;
        }

        public UnsubscribeResult Unsubscribe(ContainerTrackingRequestPM containerTrackingRequest)
        {
            var result = APICaller.CallApi<UnsubscribeResult>(Source.ProviderURL + "/references/"+ containerTrackingRequest.RequestId, null, Method.DELETE);
            return result;
        }
    }
}
