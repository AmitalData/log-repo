using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class TaxReportLinesCounter
    {
        public int TaxableTransactions { get; set; }
        public int ExcemptTransactions { get; set; }

        public int AllTransaxtions
        {
            get { return (int)TaxableTransactions + ExcemptTransactions; }
        }

        public int InputEquipments { get; set; }
        public int InputOthers { get; set; }
        public int All
        {
            get { return (int)AllTransaxtions + InputEquipments + InputOthers; }
        }

    }
}
