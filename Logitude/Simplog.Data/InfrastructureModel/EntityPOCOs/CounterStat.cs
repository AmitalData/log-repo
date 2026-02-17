using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class CounterStat
    {
        [Key]
        public int Id { get; set; }

        public int Tenant { get; set; }

        public string Prefix { get; set; }

        public int LastValue { get; set; }

        public string CounterId { get; set; }

        //[Include]
        //[Association("CounterCounterStat", "CounterId", "Id", IsForeignKey = true)]
        [ForeignKey("CounterId")]
        public virtual Counter Counter { get; set; }
    }
}
