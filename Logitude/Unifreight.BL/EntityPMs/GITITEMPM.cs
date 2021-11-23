using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GITITEMPM
    {
        List<GITITEMCRPM> _GITITEMCRPMs;
        
        public List<GITITEMCRPM> GITITEMCRPMs
        {
            get { return _GITITEMCRPMs = _GITITEMCRPMs ?? new List<GITITEMCRPM>(); }
            set
            {
                _GITITEMCRPMs = value;
            }
        }

        
    }
}