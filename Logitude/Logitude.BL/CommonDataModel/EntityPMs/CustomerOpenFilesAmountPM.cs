using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class CustomerOpenFilesAmountPM
    {
        [Key]
        public string CustomerId { get; set; }
        public decimal TotalOpenFilesAmount { get; set; }
    }
}
