using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUCARLPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public int COUNTER { get; set; }

        public string RIHBIT { get; set; }

        public int? ENGINEVOL { get; set; }

        public string SHEILDNO { get; set; }

        public string MNFDATE { get; set; }

        public int? A { get; set; }

        public int? B { get; set; }

        public int? E { get; set; }

        public int? MEMIRTYPE { get; set; }

        public decimal? MADADRATE { get; set; }

        public string FUELTYPE { get; set; }

        public decimal? WEIGHT { get; set; }

        public string FFU1 { get; set; }

        public string FFU2 { get; set; }

        public decimal? ABSDEDUCT { get; set; }

        public int? KARITDEDUCT { get; set; }

        public decimal? BAKARADEDUCT { get; set; }

        public decimal? MEMIRDEDUCT { get; set; }

        public decimal? MADADDEDUCT { get; set; }

        public int? HYBRID { get; set; }
        
        public int Tenant { get; set; }
        
        public bool IS_SYNCH { get; set; }
        
        public DateTime? LAST_UPDATE_DT { get; set; }
    }
}
