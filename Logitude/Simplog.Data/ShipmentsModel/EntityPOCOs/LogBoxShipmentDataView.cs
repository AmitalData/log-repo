using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class LogBoxShipmentDataView
    {
        private string id;
        [Key]
        public string Id
        {
            get
            {

                return id;
            }
            set
            {
                id = value;
            }
        }
        public string PartnerName { get; set; }
        public string CarrierNumber { get; set; }
        public string TruckNumber { get; set; }
        public string ContainersNumbersandTypesArray { get; set; }
        public int Tenant { get; set; }
        public string ShipmentNumber { get; set; }
        public string DirectionId { get; set; }
        public string StatusName { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusLocation { get; set; }
        public string TransportModeId { get; set; }
        public string Shipper { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeName { get; set; }
        public string PartnerLogoId { get; set; }
        public bool IsShipmentOrder { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public string CustomerReference3 { get; set; }
        public string ShipperName { get; set; }
        public string DocumentsSearchFields { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? LastDocumentDateTime { get; set; }
        public bool IsRequestedDocuments { get; set; }
        public int RequestedDocumentsCount { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool IsDepositionRequired { get; set; }
        public bool IsImporterApprovalRequried { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? MainCarriageExpectedOrActual { get; set; }
        public bool IsOperationalClosed { get; set; }
        public string SearchFields { get; set; } 
        public bool IsCancelled { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public string PrivateLabelAgentName { get; set; }
        public string ForwarderPartnerId { get; set; }
    }
}