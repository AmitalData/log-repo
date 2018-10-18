using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TarrifFromToList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public string PortId { get; set; }
        public string CountryId { get; set; }
        public string TarrifFromToTypeCode { get; set; }
    }
}