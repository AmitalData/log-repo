using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ExternalSystemsTablesCodePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string LogitudeTable { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SearchFields { get; set; }

    }
}