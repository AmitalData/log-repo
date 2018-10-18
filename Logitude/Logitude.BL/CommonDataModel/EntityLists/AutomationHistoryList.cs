using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class AutomationHistoryList
    {
        [Key]
        public int Version { get; set; }

        [Key]
        public string AutomationsId { get; set; }

        public int Tenant { get; set; }

        public string AutomationXML { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
