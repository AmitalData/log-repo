using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class SearchIndexTenantHistory
    {
        [Key]
        public string Index { get; set; }
        public int Tenant { get; set; }
        public int TtlMonth { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
