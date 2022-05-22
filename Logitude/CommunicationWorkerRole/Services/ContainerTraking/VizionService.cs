using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.APIDataContract;
using RestSharp;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.ContainerTraking
{
    public class VizionService
    {
        public VizionReferenceResponce SendRequest(GeneralContainerStatusSimulatorArgs containerStatusSimulatorArgs, ContainerTrackingProvider source, Shipment shipment)
        {
            if (containerStatusSimulatorArgs.IsFromContainer)
            {
                return CallCreateReferenceViaCarrierCodeApi(containerStatusSimulatorArgs, source, shipment);
            }
            else
            {
                return CallCreateReferenceViaBillofLading(source, shipment);
            }
        }

        private VizionReferenceResponce CallCreateReferenceViaBillofLading(ContainerTrackingProvider source, Shipment shipment)
        {
            var referenceViaBillOfLadingRequest = CreateCreateReferenceViaBillOfLadingRequest(source, shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(source.ProviderURL, referenceViaBillOfLadingRequest, Method.POST);
            return result;

        }

        private ReferenceViaCarrierCodeRequest CreateReferenceViaCarrierCodeRequest(GeneralContainerStatusSimulatorArgs containerStatusSimulatorArgs, ContainerTrackingProvider source, Shipment shipment)
        {
            return new ReferenceViaCarrierCodeRequest()
            {
                callback_url = source.CallbackURL,
                carrier_code = shipment.ShipmentMasterData.MainCarriageCarrierCard.Code,
                container_id = containerStatusSimulatorArgs.ContainerNumber
            };
        }
        private CreateReferenceViaBillOfLadingRequest CreateCreateReferenceViaBillOfLadingRequest(ContainerTrackingProvider source, Shipment shipment)
        {
            return new CreateReferenceViaBillOfLadingRequest()
            {
                callback_url = source.CallbackURL,
                carrier_code = shipment.ShipmentMasterData.MainCarriageCarrierCard.Code,
                bill_of_lading = shipment.ShipmentMasterData.Master
            };
        }

        private VizionReferenceResponce CallCreateReferenceViaCarrierCodeApi(GeneralContainerStatusSimulatorArgs containerStatusSimulatorArgs, ContainerTrackingProvider source, Shipment shipment)
        {
            var referenceViaCarrierCodeRequest = CreateReferenceViaCarrierCodeRequest(containerStatusSimulatorArgs, source, shipment);
            var result = APICaller.CallApi<VizionReferenceResponce>(source.ProviderURL, referenceViaCarrierCodeRequest, Method.POST);
            return result;
        }
    }
}
