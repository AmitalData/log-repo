using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFICONNPM : EntityPM
    {
        public string FILENO { get; set; }

        public long CUSTOMFILE { get; set; }
    }
}
