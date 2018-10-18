using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class QuickbooksSyncRequestTicketList
    {
        [Key]
        public string Ticket { get; set; }
        public int Tenant { get; set; }
        public string ReferenceNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RequestCount { get; set; }
        public int ExternalTablesRequestCount { get; set; }


    }
}