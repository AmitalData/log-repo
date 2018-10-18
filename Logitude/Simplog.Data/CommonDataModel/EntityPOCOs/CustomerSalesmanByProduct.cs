using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class CustomerSalesmanByProduct
    {
       [Key]
       public string ProductTypeCode { get; set; }

       public string SalesmanUserId { get; set; }
       [Key]
       public string CustomerId { get; set; }
   
       public int Tenant { get; set; }

       [ForeignKey("SalesmanUserId")]
       public virtual User SalesmanUser { get; set; }
  
     
    }
}
