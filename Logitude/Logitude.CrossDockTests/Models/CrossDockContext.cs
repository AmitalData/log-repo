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
    }
}
