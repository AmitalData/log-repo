using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerDepositionPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomsShipperId { get; set; }
        public string DepositionNumber { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public DateTime CreateDate { get; set; }
    
    }



}
