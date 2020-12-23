using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class ReferencecArgs
    {
        public DataRow TableRow { get; set; }
        public DataTable DataTable { get; set; }
        public string SearchField { get; set; }
        public string CoulmnName { get; set; }
     }
}
