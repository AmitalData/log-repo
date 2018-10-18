using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
  public  class HybridPartnerList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int PartnerTenant { get; set; }
        public string LocalName { get; set; }
        public string LogoId { get; set; }
        public string SearchFields { get; set; }
        public bool IsHasRequest { get; set; }
        public string StatusName { get; set; }
        public string SmallLogoId { get; set; }
        public string ReqId { get; set; }
        public bool IsMislakaActivated { get; set; }
        public bool IsExternalPartner { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public bool AllowSendingDocsToAgent { get; set; }

    }
}
