using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CardCurrenciesAccounting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PayableDebitAccount { get; set; }
        public string ReceivableCreditAccount { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
        public string CardId { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        public string CurrencyId { get; set; }
    }
}
