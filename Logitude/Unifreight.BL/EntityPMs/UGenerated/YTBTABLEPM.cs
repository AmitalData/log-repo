using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class YTBTABLEPM : EntityPM
    {
        public string CUSTTB { get; set; }
 
        public string TBCODE { get; set; }

        public string NAMEHEB { get; set; }

        public string NAMEENG { get; set; }

        public string SEARCHENG { get; set; }

        public string BLOCKRECORD { get; set; }

        public DateTime? IIGUPDTDATE { get; set; }

        public long? TBCODENUM { get; set; }

    }
}
