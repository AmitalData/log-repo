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
        public string InvoiceNumber { get; set; }
        public string CustomerRef { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string MainEntityReference { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public double? TotalVATs { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public string ConcurrencyGUID { get; set; }
    }
}