using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUSIGNUMPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENOMSHGR { get; set; }

        public int LINENOSIGN { get; set; }

        public string SIGNNUM { get; set; }
    }
}
