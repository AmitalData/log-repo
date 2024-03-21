using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUCARPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }
        public Nullable<int> ABSDEDUCT { get; set; }
        public Nullable<int> KARITDEDUCT { get; set; }
        public Nullable<int> BAKARADEDUCT { get; set; }

        public Nullable<decimal> MEMIRDEDUCT { get; set; }
        public Nullable<decimal> MADADDEDUCT { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
