using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class InterestTransactionUniqueConstraintFields
    {
        public int Tenant { get; set; }
        public string InterestEntityTypeCode { get; set; }
        public string EntityId { get; set; }
        public int OriginalEntityLineNumber { get; set; }
        public string GLAccountId { get; set; }
        public decimal ForeignAmount { get; set; }
        public decimal LocalAmount { get; set; }
        public string CurrencyId { get; set; }
        public int IT_JlineNumber { get; set; }
        public int JournalLineNumber { get; set; }
    }
}
