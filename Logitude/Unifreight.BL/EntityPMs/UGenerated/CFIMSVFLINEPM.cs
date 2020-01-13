using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIMSVFLINEPM : EntityPM
    {
        public long FILENO { get; set; }

        public string COMID { get; set; }

        public int LINENUM { get; set; }

        public string ITEMNAME { get; set; }

        public double? ITEMVALUE { get; set; }

        public string ITEMTYPE { get; set; }

    }
}
