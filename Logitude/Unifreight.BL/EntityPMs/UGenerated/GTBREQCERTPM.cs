using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GTBREQCERTPM : EntityPM
    {
        public string REQCERT { get; set; }

        public string ENTITY { get; set; }

        public string NAMEHEB { get; set; }

        public string NAMEENG { get; set; }

        public string BLOCKRECORD { get; set; }
    }
}

