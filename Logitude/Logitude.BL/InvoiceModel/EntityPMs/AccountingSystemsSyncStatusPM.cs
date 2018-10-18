using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class AccountingSystemsSyncStatusPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime? ExternalCodesLastUpdate { get; set; }
        public DateTime? LastRequestDate { get; set; }
        public string LastError { get; set; }
        public int? SyncInterval { get; set; }
        public DateTime? LastErrorDate { get; set; }
    }
}