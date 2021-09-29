using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Country
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string SearchFields { get; set; }
        public bool EC { get; set; }
        public bool HasStates { get; set; }
        public bool IsStateRequired { get; set; }
        public bool HasCitiesList { get; set; }
        public bool IsNorthAmerica { get; set; }
        public bool IsGreaterChina { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }
        public string GlobalZoneId { get; set; }
        [ForeignKey("GlobalZoneId")]
        public virtual GlobalZone GlobalZone { get; set; }

    }
}
