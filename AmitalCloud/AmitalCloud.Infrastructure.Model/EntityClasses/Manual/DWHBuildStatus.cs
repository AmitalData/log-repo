using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class DWHBuildStatus
    {
        [Key]
        public string Id { get; set; }
        public DateTime? LastIncrementalDWUpdateDate { get; set; }
        public DateTime? DWNextRunTime { get; set; }
        public bool IsFullBuildDWRunning { get; set; }
        public bool IsIncrementalDWRunning { get; set; }

    }
}
