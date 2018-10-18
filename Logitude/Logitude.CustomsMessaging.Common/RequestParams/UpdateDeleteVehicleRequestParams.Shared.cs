using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class UpdateDeleteVehicleRequestParams : RequestParamsBase
    {
        public string VehicleId { get; set; }
        public bool IsDelete {get; set;}
    }
}
