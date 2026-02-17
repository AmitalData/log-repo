using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentComputedFields
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


        public DateTime? FirstPickupATD { get; set; }
        public DateTime? FirstPickupATA { get; set; }
        public DateTime? FinalDeliveryETD { get; set; }
        public DateTime? FinalDeliveryETA { get; set; }
        public DateTime? FinalDeliveryATD { get; set; }
        public DateTime? FinalDeliveryATA { get; set; }


    }
}
