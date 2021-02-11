using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TenantManagmentPrivateLabelsPM
    {
        [Key]
        public string Id { get; set; }
        public string PrivateLabelName { get; set; }
        public string PrivateLabelShortName { get; set; }
        public string PrivateLabelUrl { get; set; }
        public byte[] MainLogo { get; set; }
        public string ContactUsEmail { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public string HybridPartnerId { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public byte[] SmallLogo { get; set; }
        public int Tenant { get; set; }
        public string BackgroundImageId { get; set; }
        public string MainImageId { get; set; }
        public string MainColor { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }

    }
}
