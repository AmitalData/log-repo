using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class TenantManagmentPrivateLabels
    { 
        [Key]
        public string Id { get; set; }
        public string PrivateLabelName { get; set; }
        public string PrivateLabelShortName { get; set; }
        public string PrivateLabelUrl { get; set; }
        public string PrivateLabelDomain { get; set; }
        public byte[] MainLogo { get; set; }
        public byte[] SmallLogo { get; set; }
        public string ContactUsEmail { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public string HybridPartnerId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public int Tenant { get; set; }
        //[ForeignKey("MainLogoId")]
        //public virtual ImageDetail ImageDetail { get; set; }

    }
}