using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class EFIFILEMPM : EntityPM
    {
        public int FILENO { get; set; }

        public string SMP { get; set; }

        public int? SPEDNO { get; set; }

        public DateTime? FLIGHT_DATE { get; set; }

    }
}
