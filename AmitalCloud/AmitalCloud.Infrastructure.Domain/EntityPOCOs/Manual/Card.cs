using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public partial class Card
    {
        public string SATForeignRFC { get; set; }
        public string PartnerTypeName { get; set; }
        public string MetodoPagoCode { get; set; }

        [ForeignKey("MetodoPagoCode")]
        public virtual MetodoPago MetodoPago { get; set; }
        public virtual Trucker Trucker { get; set; }
        public virtual Airline Airline { get; set; }
        public virtual ShippingLine ShippingLine { get; set; }
        public virtual CustomAgent CustomAgent { get; set; }
        public virtual ShippingAgent ShippingAgent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual CustomsShipper CustomsShipper { get; set; }
        public virtual AccountingPartner AccountingPartner { get; set; }

    }
}
