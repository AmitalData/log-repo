using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public static class CrossDockData
    {
        public static string CrossDockEntryId { get; set; }
        public static string CrossDockReleaseId { get; set; }
        public static List<WarehouseEntryPackagePM> WarehouseEntryPackages { get; set; }
    }
}
