using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityLists
{
    
    public partial class LedgerTransactionList
    {
        public decimal CumulativeOpenAmount { get; set; }

        public override string ToString()
        {
            return string.Concat(this.AccountingDate.ToShortDateString(), ":", this.Id
                , ":", this.CumulativeLocalAmount, ":", (this.LocalAmountCredit - this.LocalAmountCredit).ToString()
                , ":", this.CumulativeForeignAmount, ":", (this.ForeignAmountDebit - this.ForeignAmountCredit).ToString());
        }
        
        

    }
}
