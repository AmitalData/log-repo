using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
  public class CustomerTenantAccessRequestStatusCount
    {
        [Key]
        public string Id { get; set; }

        public int WaitingCount { get; set; }
        public int InProgressCount { get; set; }
        public int AcceptedCount { get; set; }
        public int InactiveCount { get; set; }

    }
}
