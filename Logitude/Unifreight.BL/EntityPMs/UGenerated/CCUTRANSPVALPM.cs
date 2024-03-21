using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTRANSPVALPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public long? TRANSPVALFC { get; set; }

        public string CURRID { get; set; }

        public long? TRANSPVAL { get; set; }

        public string CURRIDN { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCH { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}

