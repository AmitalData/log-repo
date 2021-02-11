using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class HybridLabelsBrandingDataRequest
    {
        public string Id { get; set; }
        public string BackgroundImageId { get; set; }
        public string MainImageId { get; set; }
        public string LoginProgressImageId { get; set; }
        public string ForgetPasswordImageId { get; set; }  

    }
}