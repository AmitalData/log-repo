using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AutomationLastUpdate
    {


        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ObjectTableId { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool HasAutomation  { get; set; }
       



    }
}
