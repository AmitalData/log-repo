using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentDocsField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsPODReceived { get; set; }
        public DateTime? PODReceivedDate { get; set; }
        public bool IsCommercialInvoiceReceived { get; set; } //(Document Code: 380)
        public DateTime? CommercialInvoiceReceivedDate { get; set; }
        public bool IsPackingListReceived { get; set; } //(Document Code: 721)
        public DateTime? PackingListReceivedDate { get; set; }
        public bool IsBOLReceived { get; set; } //(Document Code: 706)
        public DateTime? BOLReceivedDate { get; set; }
        public bool IsMasterBOLReceived { get; set; } //(Document Code: 704)
        public DateTime? MasterBOLReceivedDate { get; set; }
        public bool IsArrivalNoticeReceived { get; set; } //(Document Code: ARNT)
        public DateTime? ArrivalNoticeReceivedDate { get; set; }

        public Shipment Shipment { get; set; }
    }
}
