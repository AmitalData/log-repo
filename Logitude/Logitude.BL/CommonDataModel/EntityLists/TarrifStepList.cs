using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TarrifStepList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string TarrifHeaderId { get; set; }
        public decimal? Step { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}