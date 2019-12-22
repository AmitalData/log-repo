using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class CashbookChequesCounter
    {
        public int CashChequesCount { get; set; }
        public int PostdatedChequesCount { get; set; }
        public int AllChequesCount
        {
            get
            {
                return CashChequesCount + PostdatedChequesCount;
            }
        }
    }
}
