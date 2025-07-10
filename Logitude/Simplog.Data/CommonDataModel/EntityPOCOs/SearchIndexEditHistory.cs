using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class SearchIndexEditHistory
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string Screen { get; set; }
        public string ScreenParam { get; set; }
        public string Entname { get; set; }
        public string KeyVal { get; set; }
    }
}
