using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUCARSCPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public int COUNTER { get; set; }

        public string VEHICLEFILE { get; set; }

        public string CARMODEL { get; set; }

        public string CHASSISNO { get; set; }

        public string ENGINENO { get; set; }

        public string WINDOWNO { get; set; }

        public long? FOB { get; set; }

        public long? GENERALTAX { get; set; }

        public long? BUYTAX { get; set; }

        public long? VATRESHIMON { get; set; }

        public string EXEMPTTYPE { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCHRONIZED { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
