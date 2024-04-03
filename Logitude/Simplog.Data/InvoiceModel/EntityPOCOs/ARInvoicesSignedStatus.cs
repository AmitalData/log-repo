using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
      public class ARInvoicesSignedStatus
    {
        [Key]
        public string Code { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }

        public string SearchFields { get; set; }



    }
}
