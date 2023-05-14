using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class HybridPartnerPM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int? PartnerTenant { get; set; }
        public string LocalName { get; set; }
        public string LogoId { get; set; }
        public string SearchFields { get; set; }
        public bool IsHasRequest { get; set; }
        public string StatusName { get; set; }
        public string SmallLogoId { get; set; }
        public int Tenant { get; set; }
        public bool IsMislakaActivated { get; set; }
        public bool IsExternalPartner { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public bool AllowSendingDocsToAgent { get; set; }
        public bool InActive { get; set; }
    }
}
