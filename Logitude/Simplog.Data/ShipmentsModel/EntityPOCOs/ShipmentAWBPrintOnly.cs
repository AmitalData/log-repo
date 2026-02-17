using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentAWBPrintOnly
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public string CurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        public double? Amount { get; set; }
        public string PrepaidCollectId { get; set; }
        public string DueTypeCode { get; set; }
        public string ShipmentId { get; set; }
        public string IATACodeId { get; set; }
        public string MeasurementId { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }

        [ForeignKey("IATACodeId")]
        public virtual IATACode IATACode { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        [ForeignKey("PrepaidCollectId")]
        public virtual PrepaidCollect PrepaidCollect { get; set; }

        [ForeignKey("DueTypeCode")]
        public virtual DueType DueType { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }


    }
}