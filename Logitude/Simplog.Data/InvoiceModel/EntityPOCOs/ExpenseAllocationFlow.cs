using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public  class ExpenseAllocationFlow
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime RunDate { get; set; }
        public string SettingId { get; set; }
        public string Status { get; set; }
        public string JournalId { get; set; }

    }
}
