using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class ITBPCKTYPM : EntityPM
    {
        public string PACKTYPE_ID { get; set; }

        public string PACKTYPEID { get; set; }

        public string NAMEHEB { get; set; }

        public string NAMEENG { get; set; }

        public string SEPARPRC { get; set; }

        public string BLOCKRECORD { get; set; }

        public string SEARCHENG { get; set; }

        public global::System.Nullable<decimal> CONTSIZE { get; set; }

        public string CONTTYPE { get; set; }

        public string REFRI { get; set; }

        public string VENTY { get; set; }

        public global::System.Nullable<double> TEU { get; set; }

        public string MODEOFTRANSP { get; set; }

        public global::System.Nullable<double> DEFTARA { get; set; }

        public global::System.Nullable<double> DEFVOLUME { get; set; }
    }
}

