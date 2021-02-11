using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class BluesnapTransaction
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string DocumentId { get; set; }
        public string LogitudeAmital { get; set; }
        public double? InvoiceAmountInUSD { get; set; }
        public double? TaxAmountInUSD { get; set; }
        public string ContractNumber { get; set; }


    }
}
