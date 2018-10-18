using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUMSHGRPM
    {
        List<CCUSIGNUMPM> _CCUSIGNUMPMs;
        public int CCUSIGNUMPMsLastLine { get; set; }

        public List<CCUSIGNUMPM> CCUSIGNUMPMs
        {
            get { return _CCUSIGNUMPMs = _CCUSIGNUMPMs ?? new List<CCUSIGNUMPM>(); }
            set
            {
                if (value == null)
                {
                    CCUSIGNUMPMsLastLine = 0;
                }
                _CCUSIGNUMPMs = value;
            }
        }

        private List<CCUSIGNUMPM> _DeletedCCUSIGNUMPMs;
        public List<CCUSIGNUMPM> DeletedCCUSIGNUMPMs
        {
            get { return _DeletedCCUSIGNUMPMs = _DeletedCCUSIGNUMPMs ?? new List<CCUSIGNUMPM>(); ; }
            set { _DeletedCCUSIGNUMPMs = value; }
        }
    }
}