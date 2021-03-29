using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class PrivateLabelsBrandingData
    { 
        public string Id { get; set; }
        public string PrivateLabelName { get; set; }
        public string PrivateLabelShortName { get; set; }
        public string PrivateLabelUrl { get; set; }
        public byte[] MainLogo { get; set; }
        public byte[] SmallLogo { get; set; }
        public string ContactUsEmail { get; set; }
        public bool ReceiveAllStatuses { get; set; }
        public string HybridPartnerId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public int Tenant { get; set; }
        public string BackgroundImageId { get; set; }
        public string LoginImageId { get; set; }
        public string MainColor { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }

        public byte[] BackgroundImageBytes { get; set; }
        public byte[] LoginImageBytes { get; set; } 
        public byte[] LoginProgressImageBytes { get; set; }
        public byte[] ForgetPasswordImageBytes { get; set; }
        public string SecondaryColor { get; set; }



    }
}