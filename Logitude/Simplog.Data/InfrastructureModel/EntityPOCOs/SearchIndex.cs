using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class SearchIndex
    {
        [Key]
        public string Index { get; set; }
        public string Indexer { get; set; }
        public bool IsView { get; set; }
        public string ObjectName { get; set; }
        public int TtlMonth { get; set; }
        public string TtlField { get; set; }
        public int BuildIntervalMin { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime LastRemove { get; set; }
    }
}
