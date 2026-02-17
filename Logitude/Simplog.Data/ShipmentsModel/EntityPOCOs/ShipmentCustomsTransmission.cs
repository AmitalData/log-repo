using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentCustomsTransmission
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public DateTime? LastSendDate { get; set; }
        public string SentByUserId { get; set; }
        public string CommunicationLogId { get; set; }
        public string Error { get; set; }
        public string MessageCode { get; set; }
        public string Status { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }

        [ForeignKey("CommunicationLogId")]
        public virtual CommunicationLog CommunicationLog { get; set; }

        [ForeignKey("MessageCode")]
        public virtual ShipmentCustomsMessageType ShipmentCustomsMessageType { get; set; }

        [ForeignKey("Status")]
        public virtual CustomsTransmissionsStatus CustomsTransmissionsStatus { get; set; }

        [ForeignKey("SentByUserId")]
        public virtual User SendByUser { get; set; }
    }
}
