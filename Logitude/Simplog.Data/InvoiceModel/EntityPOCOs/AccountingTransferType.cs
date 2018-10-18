using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class AccountingTransferType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<AccountingTransferHeader> AccountingTransferHeaders { get; set; }
    }
}
