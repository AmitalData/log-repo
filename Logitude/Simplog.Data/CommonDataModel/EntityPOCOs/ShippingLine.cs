using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ShippingLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool AddedManually { get; set; }
        public string OurCreditNumber { get; set; }
        public string ShippingAgentId { get; set; }
        public string SCACCode { get; set; }
        public bool IsINTTRARegistered { get; set; }
        public string INTTRARegistrationNotes { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }

        public virtual Card Card { get; set; }

        [ForeignKey("ShippingAgentId")]
        public virtual ShippingAgent ShippingAgent { get; set; }

    }
}
