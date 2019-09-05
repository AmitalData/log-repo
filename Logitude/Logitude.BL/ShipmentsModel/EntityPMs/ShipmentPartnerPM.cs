using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentPartnerPM
    {
        [Key]
        public string Id { get; set; }

        public string PartnerType { get; set; }
        public string PartnerName { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CountryName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string FlagSRC { get; set; }
        public string CityZipCode { get; set; }
        public string ReferenceVisibility { get; set; }
        public string Reference2Visibility { get; set; }
        public string ContactName { get; set; }
        public string TenantName { get; set; }
        public string ReferentName { get; set; }
        public string Telephone { get; set; }
        public bool IsCountManager { get; set; }
        public ShipmentPartnerPM()
        {
            this.FlagSRC = "";
            this.Email = "";
            this.ContactName = "";
            this.ReferenceVisibility = "visible";
            this.Reference2Visibility = "collapse";
        }
    }
}