using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Models
{
    public enum StatusCodeEnum
    {
        Draft = 0,
        WaitingforApprove = 1,
        Approved = 2,
        Voided = 3,
        Failed = 4

    }
}
