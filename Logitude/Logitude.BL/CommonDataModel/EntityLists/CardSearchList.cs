using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
 public   class CardSearchList
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime RecordDate { get; set; }
        public string Keyword { get; set; }
        public string CardId { get; set; }
        public int Weight { get; set; }
        public string PartnerTypeId { get; set; }
        public bool InActive { get; set; }
        public bool IsCustomer { get; set; }

        
    }
}
