using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class AutomationResultEmailRecipient
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AutomationsId { get; set; }
        public string RecipientType { get; set; }
        public string RecipientValue{ get; set; }

        [ForeignKey("AutomationsId")]
        public virtual Automation Automation { get; set; }
    }
}
