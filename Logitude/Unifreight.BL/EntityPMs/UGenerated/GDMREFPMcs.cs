using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GDMREFPM : EntityPM
    {
        public string COMID  { get; set; }

        public string REFID  { get; set; }

        public string REFERENCE  { get; set; }
 
        public string METADATA  { get; set; }

        public string MDNEW { get; set; }
    }
}
