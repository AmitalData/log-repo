using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CardExternalAccountsByProduct
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string GLAccount { get; set; }
        public string CostCenter { get; set; }
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
        public string CardId { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }
        public string ProductTypeCode { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
