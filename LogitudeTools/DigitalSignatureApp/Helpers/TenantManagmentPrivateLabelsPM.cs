using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Sign.App.Helpers
{
    public class TenantManagmentPrivateLabelsPM
    {
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
        public int Tenant { get; set; }
        public string BackgroundImageId { get; set; }
        public string LoginImageId { get; set; }
        public string MainColor { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }
        public string SecondaryColor { get; set; }
        public bool HasLogboxAccess { get; set; }

        public string MainTabHighlightColor { get; set; }
        public string DocumentTypeHighlightColor { get; set; }

        public bool IsExportActivated { get; set; }

        public bool IsCustomsActivated { get; set; }
        public string QueryFiltersHighlightColor { get; set; }
        public bool CreateShipmentsWithoutDocs { get; set; }
    }
}
