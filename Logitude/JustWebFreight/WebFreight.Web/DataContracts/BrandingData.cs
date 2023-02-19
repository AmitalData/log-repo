using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class BrandingData
    {
        public int Tenant { get; set; }
        public string Email { get; set; }
        public string MainColor { get; set; }
        public string SecondaryColor { get; set; }
        public string TertiaryColor { get; set; }
        public string BackgroundId { get; set; }
        public string MobileBackgroundId { get; set; }
        public string BrowserIconId { get; set; }
        public string ComapnylogoId { get; set; }
        public string InvertedLogoId { get; set; }
        public string CustomerURL { get; set; }
        public bool ActivatePrivateSite { get; set; }
        public bool EnableExportToExcel { get; set; }
        public byte[] BackgroundBytes { get; set; }
        public byte[] MobileBackgroundBytes { get; set; }
        public byte[] ComapnylogoBytes { get; set; }
        public byte[] BrowserIconBytes { get; set; }
        public byte[] InvertedLogoBytes { get; set; }
    }
}