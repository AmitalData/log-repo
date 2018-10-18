using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GAQFILEDATAPM : EntityPM
    {
        public string ENTNAME { get; set; }

        public string PRIMARYNUM { get; set; }

        public string PATH { get; set; }

        public string FIELDID { get; set; }

        public string FIELDVALUE { get; set; }


        public string APPQID { get; set; }
    }
}
