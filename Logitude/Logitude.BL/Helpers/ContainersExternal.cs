using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class ContainersExternal
    {
        public ContainersExternalData ContainersExternalData_New { get; set; }
        public ContainersExternalData ContainersExternalData_DB { get; set; }
        public bool IsNew { get; set; }
        public bool IsFromOceanInsights { get; set; }
    }
}
