using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses
{
    public class CargoArgs
    {
        public CargoTable Table { get; set; }
        public string SourceConnectionString { get; set; }
        public string DestinationConnectionString { get; set; }
        public string Condition { get; set; }
        public TableStructureHelper InnerTableStructureHelper { get; set; }
        public TableStructureHelper MainTableStructureHelper { get; set; }
    }
}
