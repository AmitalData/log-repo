using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TarrifHeaderList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string TarrifTypeCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string TransitTimeNotes { get; set; }
        public DateTime CreateDate { get; set; }
    }
}