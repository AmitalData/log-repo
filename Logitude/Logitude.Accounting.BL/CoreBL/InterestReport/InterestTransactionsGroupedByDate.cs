using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestTransactionsGroupedByDate
    {
        public DateTime GroupInterestValueDate { get; set; }
        public decimal TotalLocalAmount { get; set; }
    }
}
