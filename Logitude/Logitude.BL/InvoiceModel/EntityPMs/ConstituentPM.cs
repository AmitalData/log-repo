using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ConstituentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ConsolidationInvoiceId { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}