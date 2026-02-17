using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CreditLimitSettingPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCreditLimitEnabled { get; set; }
        public bool InvoiceCreationWarning { get; set; }
        public bool InvoiceCreationBlock { get; set; }
        public bool ShipmentCreationBlock { get; set; }
    }
}
