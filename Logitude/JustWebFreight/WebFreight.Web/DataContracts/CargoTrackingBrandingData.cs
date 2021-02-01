using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CargoTrackingBrandingData
    {
        public int Tenant { get; set; }
        public string Domain { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public string BackgroundId { get; set; }
        public string BackgroundImg { get; set; }
        public string BackgroundURL { get; set; }
        public string ComapnylogoId { get; set; }
        public string ComapnylogoImg { get; set; }
        public string ComapnylogoURL { get; set; }
        public string BrowserIconId { get; set; }
        public string BrowserIconImg { get; set; }
        public string BrowserIconURL { get; set; }
        public string ShipmentHeaderImageId { get; set; }
        public string ShipmentHeaderImage { get; set; }
        public string ShipmentHeaderURL { get; set; }

        public string CustomerURL { get; set; }
        public byte[] BrowserIconBytes { get; set; }
        public byte[] ComapnylogoBytes { get; set; }
        public byte[] BackgroundBytes { get; set; }
        public byte[] ShipmentHeaderBytes { get; set; }

        
    }
}