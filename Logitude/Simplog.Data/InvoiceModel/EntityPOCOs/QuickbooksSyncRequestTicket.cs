using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class QuickbooksSyncRequestTicket
    {
       [Key]
       public string Ticket { get; set; }
       public int Tenant { get; set; }
       public string ReferenceNumber { get; set; }
       public string UserName { get; set; }
       public string Password { get; set; }
       public int RequestCount { get; set; }
       public int ExternalTablesRequestCount { get; set; }
       public bool IsCurrentInvoiceChecked { get; set; }

    }
}
