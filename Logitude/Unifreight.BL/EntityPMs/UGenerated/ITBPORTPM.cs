using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class ITBPORTPM : EntityPM
    {
        public string PORTID { get; set; }

        public string NAMEHEB { get; set; }

        public string NAMEENG { get; set; }

        public string BLOCKRECORD { get; set; }
        public string SEARCHENG { get; set; }
        public string COUNTRYID { get; set; }
        public string AGENTID { get; set; }
        public string TIMEZONE { get; set; }


    }
}
