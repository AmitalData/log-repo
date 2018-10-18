using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.Mevaker.EntityPMs
{
    public enum ApproveEnum
    {
        none,
        approve,
        DisAprove
    } 

    public class GenralFieldPM
    {
        public string Path { get; set; }

        public string MyValue { get; set; }

        public string OldValue { get; set; }

        public bool Mand { get; set; }

        public ApproveEnum ApproveType { get; set; }

        public DateTime OldValueUpdate { get; set; }
    }
}
