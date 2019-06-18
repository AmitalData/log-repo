using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVREMPM: EntityPM
    {
        public long FILENO { get; set; }

        public string COMID { get; set; }

        public int PAGENUM { get; set; }

        public int TOP { get; set; }

        public int LEFT { get; set; }

        public int HEIGHT { get; set; }

        public int WIDTH { get; set; }

        public string REMARK { get; set; }

    }
}
