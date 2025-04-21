using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class Counter
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ObjectTableId { get; set; }

        public string ChangedByUserId { get; set; }
        public DateTime? ChangedDate { get; set; }

        //[Include]
        //[Association("CounterObjectTable", "ObjectTableId", "Id", IsForeignKey = true)]

        [ForeignKey("ObjectTableId")]
        public ObjectTable ObjectTable { get; set; }

        [ForeignKey("ChangedByUserId")]
        public User User { get; set; }

        // public List<CounterDefinition> CounterDefinitions { get; set; }

        // public List<CounterStat> CounterStats { get; set; }
    }

}
