using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.APIDataContract
{
    public class StatusDetails
    {
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusDateTime { get; set; }
        public string StatusRemarks { get; set; }
    

    }
}
