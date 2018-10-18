

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class HybridTenantThresholdList
    {
      [Key]
      public int Tenant { get; set; }

      public int FailedThresold { get; set; }

      public int WaitingThresold { get; set; }
    }
}

