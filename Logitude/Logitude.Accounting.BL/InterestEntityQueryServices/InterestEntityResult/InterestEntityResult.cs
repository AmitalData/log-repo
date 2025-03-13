using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices 
{
    public class InterestEntityResult
    {
        public string EntityCode;
        public string AccountCode;
        public string EntityId;
        public string JournalId;
        public string JournalNumber;
        public string EntityNumber;
        public string EntityType;
        public string EntityTypeCode;
        public List<InterestEntityOriginalLineResult> OriginalLines;
    }
    public class InterestEntityOriginalLineResult
    {
        public int OriginalLineNumber;
        public string Reference1;
        public string Notes;
    }
}
