using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class MonitorServiceLastUpdate
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public string SearchFields { get; set; }
    }
}
