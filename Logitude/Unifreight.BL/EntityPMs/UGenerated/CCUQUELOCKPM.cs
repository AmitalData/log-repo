using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs.UGenerated
{
    public partial class CCUQUELOCKPM : EntityPM
    {
        public string ENTNAME { get; set; }

        public string FILENO { get; set; }

        public int Tenant { get; set; }

        public bool IS_SYNCHRONIZED { get; set; }

        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}