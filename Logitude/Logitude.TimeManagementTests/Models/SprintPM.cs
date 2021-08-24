using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class SprintPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public int ChangeSetOp { get; set; }
    }
}
