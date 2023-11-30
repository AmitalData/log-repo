using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class UpdateContainersServiceArgs
    {
        public int Tenant { get; set; }
        public string FileName { get; set; }
    }
}
