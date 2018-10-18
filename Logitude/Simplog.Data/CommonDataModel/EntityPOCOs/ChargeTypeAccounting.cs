using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ChargeTypeAccounting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PayableDebitAccount { get; set; }
        public string PayableDebitGLAcountId { get; set; }

        public string ReceivableCreditAccount { get; set; }
        public string ReceivableCreditGLAccountId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
        public string VatTypeId { get; set; }

        [ForeignKey("ChargeTypeId")]
        public virtual ChargesType ChargeType { get; set; }
        public string ChargeTypeId { get; set; }

    }
}
