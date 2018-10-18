using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CTBPACKTYPEPM : EntityPM
    {
        public string PACKTYPEID { get; set; }

        public string NAMEHEB { get; set; }
 
        public string NAMEENG { get; set; }

        public string BLOCKRECORD { get; set; }
    }
}
