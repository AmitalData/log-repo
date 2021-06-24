using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public class CrossDockContext
    {
        public CrossDockEntryPM CrossDockEntry { get; set; }
        public CrossDockReleasePM CrossDockRelease { get; set; }
        public string CustomerRef1 { get; set; }
        public string House { get; set; }
        public List<WarehouseEntryPackagePM> WarehouseEntryPackages { get; set; }
    }
}
