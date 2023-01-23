using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    
    public class Port
    {
        [Key]
        public string Id { get; set; }

        private string code;
        public string Code
        {
            get { return code; }
            set
            {
                code = value != null ? value.ToUpper() : value;
            }
        }
        
        public int Tenant { get; set; }
        
        public string EnglishName { get; set; }
        public string  LocalName { get; set; }
        public string Notes { get; set; }
        public string CountryId { get; set; }
        public bool InActive { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
        public bool AddedManually { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string SearchFields { get; set; }
        public string StateId { get; set; }
        public string StateName { get; set; }
        public string CombinedCode { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public string PortTimeZoneCode { get; set; }
        [ForeignKey("PortTimeZoneCode")]
        public virtual PortTimeZone PortTimeZone { get; set; }

        public string PortGroupId { get; set; }

        [ForeignKey("PortGroupId")]
        public virtual PortGroup PortGroup { get; set; }
    }
}
