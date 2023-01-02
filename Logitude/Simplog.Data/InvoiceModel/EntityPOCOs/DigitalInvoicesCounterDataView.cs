using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class DigitalInvoicesCounterDataView
    {
        public int Tenant { get; set; }
        public string PartnerId { get; set; }
        public string BillToId { get; set; }
        public double? MaxOpenAmount { get; set; }
        public double? MinOpenAmount { get; set; }
        public double? MaxTotalAmount { get; set; }
        public double? MinTotalAmount { get; set; }
    }
}
