using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ShippingAgent
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ForwarderAccountNumber { get; set; }
        public string ForwarderCreditNumber { get; set; }
        public string LocalCustomsCode { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        public virtual List<ShippingLine> ShippingLines { get; set; }      
        public virtual Card Card { get; set; }                 
    }
}
