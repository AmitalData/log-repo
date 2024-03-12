using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ConfirmationNumberStatus
    {

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        public string LocalName { get; set; }
        public Boolean InActive { get; set; }


        public string SearchFields { get; set; }
    }
}
