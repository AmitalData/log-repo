using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class CustomerOpenFilesAmountList
    {
        [Key]
        public string CustomerId { get; set; }
        public decimal TotalOpenFilesAmount { get; set; }
        public int Tenant { get; set; }
        public string CustomerCode { get; set; }
    }
}
