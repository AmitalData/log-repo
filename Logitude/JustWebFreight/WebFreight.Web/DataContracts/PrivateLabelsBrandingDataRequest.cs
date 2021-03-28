using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class PrivateLabelsBrandingDataRequest
    {
        public string PrivateLabelUrl { get; set; }
        public string BackgroundImageId { get; set; }
        public string LoginImageId { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }  

    }
}