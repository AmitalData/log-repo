using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Competitor
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string Opportunity { get; set; }
        public string Threat { get; set; }
        public string AddressId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }        
    }
}
