using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CustomerCompetitor
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }

        public int Tenant { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("CompetitorId")]
        public virtual Competitor Competitor { get; set; } 
    }
}
