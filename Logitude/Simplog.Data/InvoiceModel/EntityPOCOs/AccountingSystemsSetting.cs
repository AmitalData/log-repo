using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class AccountingSystemsSetting
    {

       [Key]

       public string Id { get; set; }
       public int Tenant { get; set; }
       public int GetExternalCodeInterval { get; set; }
       public bool UpdateOnNextRequest { get; set; }
    }
}
