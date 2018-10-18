using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIPACKPM : EntityPM
    {
        public string CONTNO { get; set; }

        public int LINENO { get; set; }

        public long FILENO { get; set; }

        public string CONTTYPEID { get; set; }

        public string SEAL { get; set; }

        public double? WEIGHT { get; set; }

        public int? QTYADD { get; set; }

        public DateTime? AVAILABILITY { get; set; }

        public DateTime? EXITFROMPORT { get; set; }

        public string DAMAGE { get; set; }

        public string LACK { get; set; }

    }
}
