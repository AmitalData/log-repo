using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CFIGOODDESCPM : EntityPM
    {
        public long FILENO { get; set; }

        public string GOODDESC { get; set; }
  
        public string CUSTOMERID { get; set; }
    }
}
