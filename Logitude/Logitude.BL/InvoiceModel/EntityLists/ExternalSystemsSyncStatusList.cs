using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ExternalSystemsSyncStatusList
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Subject { get; set; }
        public string ProgressDetails { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }

    }
}