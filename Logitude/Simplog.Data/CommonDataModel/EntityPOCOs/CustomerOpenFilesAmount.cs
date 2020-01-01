using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class CustomerOpenFilesAmount
    {
        [Key]
        public string CustomerId { get; set; }
        public decimal TotalOpenFilesAmount { get; set; }
       
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }






    }
}
