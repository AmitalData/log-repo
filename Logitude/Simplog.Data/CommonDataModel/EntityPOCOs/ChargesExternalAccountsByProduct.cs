using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ChargesExternalAccountsByProduct
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PayablesGLAccount { get; set; }
        public string PayablesCostCenter { get; set; }
        public string ReceivablesGLAccount { get; set; }
        public string ReceivablesCostCenter { get; set; }
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("ChargesTypeId")]
        public virtual ChargesType ChargesType { get; set; }
        public string ChargesTypeId { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }
        public string ProductTypeCode { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }
    }
}
