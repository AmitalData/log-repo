using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVPAGEPM : EntityPM
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public int PAGENUM { get; set; }
        public int? WIDTH { get; set; }
        public int? HEIGHT { get; set; }
        public int? LEFTDATA { get; set; }
        public int? TOPDATA { get; set; }
        public int? WIDTHDATA { get; set; }
        public int? HEIGHTDATA { get; set; }
    }
}
