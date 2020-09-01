using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract
{
   public class GLAccountTotalsByCurrency
    {
        public string CurrencyId { get; set; }
        public decimal? BalanceForeign { get; set; }
        public decimal? BalanceLocal { get; set; }
    }
}
