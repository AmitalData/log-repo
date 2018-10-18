
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ExternalSystemsMissingTranslation
    {
        [Key]

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string LogitudeTable { get; set; }
        public string LogitudeId { get; set; }
        public string ExternalCode { get; set; }
        public string Split1 { get; set; }
        public string Split2 { get; set; }
        public bool IsResolved { get; set; }
    }
}

