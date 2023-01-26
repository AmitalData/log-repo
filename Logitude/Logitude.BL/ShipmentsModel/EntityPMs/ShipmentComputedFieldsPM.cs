using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentComputedFieldsPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsMissingDocuments { get; set; }
        public string DocumentsSearchFields { get; set; }
        public DateTime? LastDocumentDateTime { get; set; }
        public int MissingDocumentsCount { get; set; }
        public string MissingDocumentsNames { get; set; }
        public bool IsRequestedDocuments { get; set; }
        public int RequestedDocumentsCount { get; set; }
        public int NumberOfHouses { get; set; }
        public Shipment Shipment { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool IsDepositionRequired { get; set; }
        public string ImporterDepositionRequestDetails { get; set; }
        public string Commodity { get; set; }
        public string FirstPickupLocation { get; set; }
        public string ContainersNumbers { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public DateTime? FirstPickupATD { get; set; }
        public DateTime? FirstPickupATA { get; set; }
        public DateTime? FinalDeliveryETD { get; set; }
        public DateTime? FinalDeliveryETA { get; set; }
        public DateTime? FinalDeliveryATD { get; set; }
        public DateTime? FinalDeliveryATA { get; set; }

        public string OperationallyClosedByUserId { get; set; }
        public int? NumberOfDeliveries { get; set; }
        public DateTime? LastPickupETA { get; set; }
        public DateTime? LastPickupETD { get; set; }
        public DateTime? LastPickupATA { get; set; }
        public DateTime? LastPickupATD { get; set; }
        public string DeliveryToPortId { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string PickupFrom { get; set; }
        public string PickupTo { get; set; }
        public string OperationallyClosedByUserName { get; set; }
        public bool CreatedFromDigital { get; set; }


        public string DeliveryTruckerId { get; set; }
        public string DeliveryTruckerNumber { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryTrailerNumber { get; set; }
        public string DeliveryNotes { get; set; }

        public string PickupTruckerId { get; set; }
        public string PickupTruckerNumber { get; set; }
        public string PickupDriver { get; set; }
        public string PickupTrailerNumber { get; set; }
        public string PickupNotes { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? OnHandDate { get; set; }
        public DateTime? PODDate { get; set; }


        public DateTime? BookingConfirmationSent { get; set; }
        public DateTime? PreAlertSent { get; set; }
        public DateTime? DeliveryNoticeSent { get; set; }
        public DateTime? ExpectedArrivalNoticeSent { get; set; }
        public DateTime? ArrivalNoticeSent { get; set; }
        public DateTime? T1Received { get; set; }
        public string AccountingClosedByUserId { get; set; }
        public string PackagesQuantityAndType { get; set; }
        public bool IsDocumentsNeedApprove { get; set; }
    }
}
