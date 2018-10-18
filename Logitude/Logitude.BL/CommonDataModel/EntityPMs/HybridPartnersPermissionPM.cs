using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class HybridPartnersPermissionPM
    {
       
        public string HybridPartnerId { get; set; }
        public string AllowedByHybridPartnerId { get; set; }
        public bool InActive { get; set; }
    }
}
