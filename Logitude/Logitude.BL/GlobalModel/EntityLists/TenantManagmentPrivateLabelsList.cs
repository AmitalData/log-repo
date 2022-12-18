using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.GlobalModel.EntityLists
{
    public class TenantManagmentPrivateLabelsList
    {
        [Key]
        public string Id { get; set; }
        public string PrivateLabelName { get; set; }
        public string PrivateLabelShortName { get; set; }
        public string PrivateLabelUrl { get; set; }
        public string PrivateLabelDomain { get; set; }
        public byte[] MainLogo { get; set; }
        public string ContactUsEmail { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public string HybridPartnerId { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public byte[] SmallLogo { get; set; }

        public string BackgroundImageId { get; set; }
        public string LoginImageId { get; set; }
        public string MainColor { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }
        public string SecondaryColor { get; set; }
        public bool HasLogboxAccess { get; set; }
        public string MainTabHighlightColor { get; set; }
        public string DocumentTypeHighlightColor { get; set; }

        public bool IsCustomsActivated { get; set; }
        public bool IsExportActivated { get; set; }
        public string QueryFiltersHighlightColor { get; set; }
        public bool CreateShipmentsWithoutDocs { get; set; }
        public bool CreateOShipmentsWithoutDocs { get; set; }
        public string FilingInboxDomain { get; set; }
        public string DistributorCode { get; set; }

    }
}