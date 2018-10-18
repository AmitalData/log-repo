using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUPAYHANDPM 
    {
        List<CCUPAYLINEFPM> _CCUPAYLINEFPMs;

        public List<CCUPAYLINEFPM> CCUPAYLINEFPMs
        {
            get { return _CCUPAYLINEFPMs = _CCUPAYLINEFPMs ?? new List<CCUPAYLINEFPM>(); }
            set { _CCUPAYLINEFPMs = value; }
        }

        public int CCUPAYLINEFPMLastLine { get; set; }

        private List<CCUPAYLINEFPM> _DeletedCCUPAYLINEFs;

        public List<CCUPAYLINEFPM> DeletedCCUPAYLINEFs
        {
            get { return _DeletedCCUPAYLINEFs = _DeletedCCUPAYLINEFs ?? new List<CCUPAYLINEFPM>(); }
            set
            {
                if (value == null)
                {
                    CCUPAYLINEFPMLastLine = 0;
                }
                _DeletedCCUPAYLINEFs = value;
            }
        }

        public string DeclarationId { get; set; }
    }
}
