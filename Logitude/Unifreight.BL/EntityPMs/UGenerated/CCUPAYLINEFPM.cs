using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUPAYLINEFPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }
        
        public string ACCOUNTNAME { get; set; }
        
        public string BANKACCOUNT { get; set; }
        
        public string BANKBRANCH { get; set; }
        
        public int? BANKID { get; set; }
        
        public string HASHAVUTCODE { get; set; }
        
        public long? PAYAMOUNT { get; set; }
        
        public DateTime? PAYDATE { get; set; }

        public short? PAYEETYPE { get; set; }
        
        public string PAYMETHOD { get; set; }
        
        public int? PAYORDNO { get; set; }
        
        public string PAYREF { get; set; }
        
        public string TREATFILE { get; set; }
        
        public short? TYPE { get; set; }
        
        public string VATBANK { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
