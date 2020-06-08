using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CardCurrenciesAccountingList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string CurrencyId { get; set; }
        public string PayableDebitAccount { get; set; }
        public string ReceivableCreditAccount { get; set; }

    }
}
