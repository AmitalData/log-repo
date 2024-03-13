using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ConfirmationNumberDefault
    {

        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        public DateTime FromDate { get; set; }
        public string SearchFields { get; set; }

        public int AmountForConfirmationNumber { get; set; }
    }
}
