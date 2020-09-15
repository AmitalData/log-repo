using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AutomationResultEmailRecipientList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AutomationsId { get; set; }
        public string RecipientValue { get; set; }
        public string RecipientType { get; set; }
        public string PartnerObjectFieldCode { get; set; }
        public bool IsNotifyBack { get; set; }

    }
}
