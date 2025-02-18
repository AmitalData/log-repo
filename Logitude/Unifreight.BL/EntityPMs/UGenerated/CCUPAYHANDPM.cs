using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    [DataContract]
    public partial  class CCUPAYHANDPM : EntityPM
    {
        [DataMember]
        public int FILENO { get; set; }
        [DataMember]
        public string AGENTEXPLAIN { get; set; }
        [DataMember]
        public string BONDEDNAME { get; set; }
        [DataMember]
        public DateTime? DATE7 { get; set; }
        [DataMember]
        public int? DRAFTSTATUS { get; set; }
        [DataMember]
        public int? ENTRYID { get; set; }
        [DataMember]
        public DateTime? HANDDATE { get; set; }
        [DataMember]
        public int? HANDTYPE { get; set; }
        [DataMember]
        public string IMPORTERNAME { get; set; }
        [DataMember]
        public string OBJECTIONEXPLAIN { get; set; }
        [DataMember]
        public decimal? PAYTAX { get; set; }
        [DataMember]
        public string PROCESSWANT { get; set; }
        [DataMember]
        public decimal? REJECTTAX { get; set; }
        [DataMember]
        public int? REQUESTCODE { get; set; }
        [DataMember]
        public string RESHIMONSIGN { get; set; }
        [DataMember]
        public int? RESHIMONSIGNTYPE { get; set; }
        [DataMember]
        public string SIGNERID { get; set; }
        [DataMember]
        public DateTime? TIME7 { get; set; }
        [DataMember]
        public decimal? TOTALPAYDEPOSIT { get; set; }
        [DataMember]
        public decimal? TOTALPAYTAX { get; set; }
        [DataMember]
        public string TRANSIMPORTERNAME { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
