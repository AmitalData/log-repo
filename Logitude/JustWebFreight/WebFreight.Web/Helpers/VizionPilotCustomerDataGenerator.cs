using Logitude.BL.ShipmentsModel.APIDataContract;
using Logitude.BL.ShipmentsModel.Tools.ContainerTracking;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class VizionPilotCustomerDataGenerator
    {
        private int myTenant;
        private ShipmentRepository shipmentRepository;
        public VizionPilotCustomerDataGenerator(int tenant)
        {
            myTenant = tenant;
            shipmentRepository = new ShipmentRepository(tenant);
        }

        public List<ShipmentsForAutomaticRequest> GetShipmentsForAutomaticRequest()
        {
            List<ShipmentsForAutomaticRequest> myResult = new List<ShipmentsForAutomaticRequest>();

            List<string> supportedCarriers = this.GetVizionCarriers();

            DateTime myDateFilter = DateTime.Now.AddDays(14);
            ShipmentRepository shipmentRepository = new ShipmentRepository(myTenant);

            IQueryable<ContainerTrackingRequest> previousReqesuts = shipmentRepository.context.ContainerTrackingRequests
            .Where(e => e.Tenant == myTenant && e.Status == "Active");

            IQueryable<Shipment> myShipments = shipmentRepository.GetShipmentsWithoutIncludes(myTenant);
            myShipments = myShipments.Where(d => !previousReqesuts.Where(r => !string.IsNullOrEmpty(r.Master) && string.IsNullOrEmpty(r.ContainerId)).Select(s => s.ShipmentId).Contains(d.Id));

            return (from shipment in myShipments
                        join sm in shipmentRepository.context.ShipmentMasterDatas
                        on shipment.MasterShipmentDataId equals sm.Id into shipmentJoin
                        from master in shipmentJoin.DefaultIfEmpty()
                        join sc in shipmentRepository.context.Containers
                        on shipment.Id equals sc.ShipmentId into containerJoin
                        from container in containerJoin.DefaultIfEmpty()
                        where shipment.NumberOfContainers > 0
                        && (shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "MyGO")
                        && !shipment.IsCancelled
                        && !shipment.IsOperationalClosed
                        && master.MainCarriageATA == null
                        && master.MainCarriageETA != null
                        && master.MainCarriageETA >= DateTime.Now
                        && master.MainCarriageETA <= myDateFilter
                        && !string.IsNullOrEmpty(container.ContainerNumber)
                        && supportedCarriers.Contains(master.MainCarriageCarrierCard.ShippingLine.SCACCode)
                        && !previousReqesuts.Where(r => !string.IsNullOrEmpty(r.ContainerId)).Select(s => s.ContainerId).Contains(container.Id)
                        select new ShipmentsForAutomaticRequest()
                        {
                            ShipmentId = shipment.Id,
                            ShipmentNumber = shipment.ShipmentNumber,
                            NumberOfContainers = shipment.NumberOfContainers,
                        }).Distinct().ToList();
        }
        private List<string> GetVizionCarriers()
        {
            VizionService vizionService = new VizionService();
            List<VizionCarrier> supportedCarriers = vizionService.GetAllCarriers();
            return supportedCarriers.Select(s => s.scac).ToList();
        }

        public List<ShipmentsForAutomaticRequest> SendVizionAutomaticRequests(string requestText)
        {
            List<ShipmentsForAutomaticRequest> muResult = new List<ShipmentsForAutomaticRequest>();
            List<string> shipmentsNumbers = this.GetShipmentsNumbersList(requestText);
            
            foreach (string number in shipmentsNumbers)
            {
                muResult.Add(this.HandleShipmentSending(number));                
            }

            return muResult;
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
                GeneralContainerTrackingService containerTrackingService = new GeneralContainerTrackingService(myArgs);
                containerTrackingService.GeneralContainerStatus();
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
            return new GeneralContainerTrackingArgs()
            {
                ContainerId = null,
                ContainerNumber = null,
                IsFromContainer = false,
                ShipmentId = shipmentId,
                Tenant = myTenant,
                IsSimulator = false,
                Data = null,
                ContainerStatusSourceCode = "VZN"
            };
        }
    }

    public class ShipmentsForAutomaticRequest
    {
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int? NumberOfContainers { get; set; }
        public bool SentSuccesfully { get; set; }
        public string ErrorMessage { get; set; }
    }
}