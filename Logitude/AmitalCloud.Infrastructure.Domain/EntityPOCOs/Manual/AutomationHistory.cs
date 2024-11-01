using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AutomationHistory 
    {

        [Key]
        public int Version { get; set; }

        [Key]
        public string AutomationsId { get; set; }

        public int Tenant { get; set; }
      
        public string AutomationXML { get; set; }

        public DateTime? CreateDate { get; set; }


        [ForeignKey("AutomationsId")]
        public virtual Automation Automation { get; set; }

    
    


    }
}
