using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class ExternalSystemsSyncStatus
    {
       [Key]
       public string Id { get; set; }
       public int Tenant {get; set;}
       public string Subject { get; set; }
       public string ProgressDetails { get; set; }
       public string Status { get; set; }
       public DateTime StatusDate { get; set; }

    }
}
