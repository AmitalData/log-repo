using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Vessel
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string IMOCode { get; set; }
        public string CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}