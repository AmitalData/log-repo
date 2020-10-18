using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CargoTrackingBrandingData
    {
        public int Tenant { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public string BackgroundId { get; set; }
        public string BackgroundImg { get; set; }
    }
}