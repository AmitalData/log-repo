using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TMProjectCategoryPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }
        public int ChangeSetOp { get; set; }
    }
}
