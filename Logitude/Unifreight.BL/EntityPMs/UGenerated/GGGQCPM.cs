using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GGGQCPM : EntityPM
    {
        public string QUEID { get; set; }

        public string FIELDID { get; set; }

        public string FIELDVAL { get; set; }
        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }

    }
}
