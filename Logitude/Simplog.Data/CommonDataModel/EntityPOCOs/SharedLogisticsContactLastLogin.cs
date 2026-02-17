using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class SharedLogisticsContactLastLogin
    {
        [Key]
        public string ContactId { get; set; }

        [Key]
        public string CardId { get; set; }
        [Key]
        public string PartnerTypeId { get; set; }
        [Key]
        public string Via { get;  set; }

        public DateTime? LoginDateTime { get; set; }
        public int Tenant { get; set; }

      
        

        public virtual Contact Contact { get; set; }
       
    }
}
