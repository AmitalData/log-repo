using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ARInvoiceLineActionList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool Inactive { get; set; }
        public string SearchFields { get; set; }
    }
}
