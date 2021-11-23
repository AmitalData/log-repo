using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class ETBAIRLINEPM : EntityPM
    {
        public string AIRLINEID { get; set; }

        public string TMPACCCARD { get; set; }

        public string AWBTOTPRT { get; set; }

        public string MAINPORT { get; set; }
        public string AWBADRPRT3 { get; set; }
        public string FILLER { get; set; }
        public string CHECKDIGIT { get; set; }
        public string AWBADRPRT1 { get; set; }
        public string AWBADRPRT2 { get; set; }
        public string CONSOLIDATOR { get; set; }
        public string NAMEHEB { get; set; }
        public string NAMEENG { get; set; }
        public string AIRLINENUM { get; set; }
        public string BLOCKRECORD { get; set; }
        public string SEARCHENG { get; set; }
        public int? MINQUAN { get; set; }
        public string STACKBRDEP { get; set; }
        public string AWBACTPRT { get; set; }
        public string PRINTRATE { get; set; }
        public string MESSAGETEXT { get; set; }

    }
}
