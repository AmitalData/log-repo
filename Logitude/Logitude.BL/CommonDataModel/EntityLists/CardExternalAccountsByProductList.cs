using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CardExternalAccountsByProductList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string GLAccount { get; set; }
        public string CostCenter { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CardId { get; set; }
        public string ProductTypeCode { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
