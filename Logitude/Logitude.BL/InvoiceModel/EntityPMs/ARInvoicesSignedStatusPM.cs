using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    
        public class ARInvoicesSignedStatusPM
        {
            [Key]
            public string Code { get; set; }
            public string LocalName { get; set; }
            public string EnglishName { get; set; }
        }
   
}
