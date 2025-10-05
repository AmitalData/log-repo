using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class BankAccountView
    {
        [Key]
        public string BankAccountId { get; set; }
        public int Tenant { get; set; }
        public string LocalName { get; set; }
    }
}
