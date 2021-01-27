using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CargoTrackingBrandingDataRequest
    {
        public string Domain { get; set; }
        public string BackgroundId { get; set; }
        public string ComapnylogoId { get; set; }
        public string InvertedLogoId { get; set; }
        public string BrowserIconId { get; set; }

    }
}