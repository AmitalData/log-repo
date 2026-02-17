using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUTRANSPVALPM : EntityPM
    {
        public int FILENO { get; set; }

        public int LINENO { get; set; }

        public double? TRANSPVALFC { get; set; }

        public string CURRID { get; set; }

        public double? TRANSPVAL { get; set; }

        public string CURRIDN { get; set; }
    }
}

