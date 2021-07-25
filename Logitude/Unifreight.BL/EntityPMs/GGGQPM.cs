using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GGGQPM : EntityPM
    {
        List<GGGQCPM> _GGGQCPMs;

        public List<GGGQCPM> GGGQCPMs
        {
            get { return _GGGQCPMs = _GGGQCPMs ?? new List<GGGQCPM>(); }
            set
            {

                _GGGQCPMs = value;
            }
        }
        

    }
}
