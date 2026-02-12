using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs.UGenerated
{
    public partial class CCUSUPITEMSIPM : EntityPM
    {
        public int FILENO { get; set; }

        public int ACCLINENO { get; set; }

        public int LINENO { get; set; }

        public int SICOUNTER { get; set; }

        public int LINEID { get; set; }

        public string MOREDATA { get; set; }
        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}

