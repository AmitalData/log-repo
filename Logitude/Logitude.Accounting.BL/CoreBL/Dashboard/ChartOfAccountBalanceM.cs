using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Accounting.BL.CoreBL.Dashboard
{
    public class ChartOfAccountBalanceM
    {

        public string Parentid { get; set; }
        public string ParentName { get; set; }
        public string ChildId { get; set; }
        public string ChildName { get; set; }
        public string CurrencyId { get; set; }

        public decimal? LeafLocalAmount { get; set; }

    }

}
