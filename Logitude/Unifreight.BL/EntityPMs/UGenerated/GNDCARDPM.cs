using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs.UGenerated
{
    public partial class GNDCARDPM : EntityPM
    {
        public string CARDID { get; set; }

        public string NAMEHEB { get; set; }

        public string NAMEENG { get; set; }

        public string BLOCKRECORD { get; set; }

        public string OLDCARD { get; set; }

        public string COMPANYID { get; set; }

        public List<GNDADRPM> GNDADRPMs { get; set; }
    }
}