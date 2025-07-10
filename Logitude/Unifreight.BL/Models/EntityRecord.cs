using System;

namespace Unifreight.BL.Models
{
    public class EntityRecord
    {
        public string Entname { get; set; }
        public string Key { get; set; }
        public string TrigAction { get; set; }
        public string RecordAsJson { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CraeteDate { get; set; }
    }
}
