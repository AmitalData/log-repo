using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.Tools.ContainerTracking;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Newtonsoft.Json;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class VizionPilotCustomerDataGenerator : BatchTaskExecutionsService
    {
        private int myTenant;
        private ShipmentRepository shipmentRepository;
        private BatchTaskExecutionPM batchTaskExecution;

        public VizionPilotCustomerDataGenerator(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            this.batchTaskExecution = batchTaskExecution;
        }

        public override void RunCode()
        {
            myTenant = batchTaskExecution.Tenant;
            shipmentRepository = new ShipmentRepository(myTenant);
            List<ShipmentsForAutomaticRequest> myResult = SendVizionAutomaticRequests(batchTaskExecution.PrametersXml);
            batchTaskExecution.PrametersXml = JsonConvert.SerializeObject(myResult);
        }

        private List<ShipmentsForAutomaticRequest> SendVizionAutomaticRequests(string requestText)
        {
            List<ShipmentsForAutomaticRequest> myResult = new List<ShipmentsForAutomaticRequest>();
            List<string> shipmentsNumbers = this.GetShipmentsNumbersList(requestText);

            foreach (string number in shipmentsNumbers)
            {
                myResult.Add(this.HandleShipmentSending(number));
            }

            return myResult;
        }
        private List<string> GetShipmentsNumbersList(string requestText)
        {
            List<string> shipmentsNumbers = new List<string>();

            foreach (string number in requestText.Split(','))
            {
                shipmentsNumbers.Add(number.Trim());
            }

            return shipmentsNumbers;
        }
        private ShipmentsForAutomaticRequest HandleShipmentSending(string shipmentNumber)
        {
            ShipmentsForAutomaticRequest automaticRequest = new ShipmentsForAutomaticRequest()
            {
                ShipmentNumber = shipmentNumber,
            };

            Shipment shipment = shipmentRepository.GetSingleShipmentByShipmentNumber(shipmentNumber, myTenant);
            bool isValid = this.ValidateShipment(shipment, automaticRequest);
            if (isValid) this.SendShipmentRequest(shipment.Id, automaticRequest);

            return automaticRequest;
        }
        private bool ValidateShipment(Shipment shipment, ShipmentsForAutomaticRequest automaticRequest)
        {
            if (shipment == null)
            {
                automaticRequest.ErrorMessage = "Shipment not found";                
            }

            else if (shipment.ShipmentTypeId != "FCLD" && shipment.ShipmentTypeId != "MyGO")
            {
                automaticRequest.ErrorMessage = "Shipment type is not supported for sending";
            }

            else if (shipment.IsCancelled)
            {
                automaticRequest.ErrorMessage = "Shipment is cancelled";
            }

            else if (shipment.NumberOfContainers == null || shipment.NumberOfContainers == 0)
            {
                automaticRequest.ErrorMessage = "Shipment has no containers";
            }

            else if (shipment.IsOperationalClosed)
            {
                automaticRequest.ErrorMessage = "Shipment is operational closed";
            }

            return string.IsNullOrEmpty(automaticRequest.ErrorMessage);
        }
        private void SendShipmentRequest(string shipmentId, ShipmentsForAutomaticRequest automaticRequest)
        {
            try
            {
                GeneralContainerTrackingArgs myArgs = this.CreateGeneralContainerTrackingArgs(shipmentId);
                automaticRequest.ContainerNumber = myArgs.ContainerNumber;
                GeneralContainerTrackingService containerTrackingService = new GeneralContainerTrackingService(myArgs);
                containerTrackingService.TrackContainer();
                this.CheckForErrorsAfterSending(myArgs.Errors, automaticRequest);
            }

            catch(Exception ex)
            {
                automaticRequest.ErrorMessage = ex.Message;
            }
        }

        private void CheckForErrorsAfterSending(List<string> errors, ShipmentsForAutomaticRequest automaticRequest)
        {
            automaticRequest.SentSuccesfully = errors.Count == 0;

            foreach(string error in errors)
            {
                if (string.IsNullOrEmpty(automaticRequest.ErrorMessage)) automaticRequest.ErrorMessage = error;
                else automaticRequest.ErrorMessage += ", " + error;
            }
        }

        private GeneralContainerTrackingArgs CreateGeneralContainerTrackingArgs(string shipmentId)
        {
            Container container = this.GetContainerForSinding(shipmentId);

            return new GeneralContainerTrackingArgs()
            {
                ContainerId = container?.Id,
                ContainerNumber = container?.ContainerNumber,
                IsFromContainer = (container != null),
                ShipmentId = shipmentId,
                Tenant = myTenant,
                IsSimulator = false,
                Data = null,
                ContainerStatusSourceCode = "2",                
            };
        }
        private Container GetContainerForSinding(string shipmentId)
        {
            IQueryable<Container> containers = shipmentRepository.context.Containers.Where(d => d.Tenant == myTenant && d.ShipmentId == shipmentId);
            IQueryable<ContainerTrackingRequest> previousReqesuts = shipmentRepository.context.ContainerTrackingRequests.Where(e => e.Tenant == myTenant && e.Status == "Active" && !string.IsNullOrEmpty(e.ContainerId));

            return (from a in containers
                    where !previousReqesuts.Select(s => s.ContainerId).Contains(a.Id)
                    select a).FirstOrDefault();
        }
    }

    public class ShipmentsForAutomaticRequest
    {
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ContainerNumber { get; set; }
        public int? NumberOfContainers { get; set; }
        public bool SentSuccesfully { get; set; }
        public string ErrorMessage { get; set; }
    }
}