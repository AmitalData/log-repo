using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTSRUFOTPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public string TSRUFAID { get; set; }

        public int? QUANTITY { get; set; }

        public string TSRUFANO { get; set; }
    }
}
