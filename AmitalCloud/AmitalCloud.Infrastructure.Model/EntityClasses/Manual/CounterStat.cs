using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CounterStat
    {
        [Key]
        public int Id { get; set; }

        public int Tenant { get; set; }

        public string Prefix { get; set; }

        public int LastValue { get; set; }

        public string CounterId { get; set; }
        public string BranchCounterCode { get; set; }

        //[Include]
        //[Association("CounterCounterStat", "CounterId", "Id", IsForeignKey = true)]
        [ForeignKey("CounterId")]
        public virtual Counter Counter { get; set; }
    }
}
