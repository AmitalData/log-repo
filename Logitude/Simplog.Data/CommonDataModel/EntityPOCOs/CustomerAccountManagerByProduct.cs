using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class CustomerAccountManagerByProduct
    {

        [Key]
        public string ProductTypeCode { get; set; }

        public string AccountManagerId { get; set; }
          [Key]
        public string CustomerId { get; set; }
     
        public int Tenant { get; set; }

        [ForeignKey("AccountManagerId")]
        public virtual User AccountManagerUser { get; set; }
    }
}
