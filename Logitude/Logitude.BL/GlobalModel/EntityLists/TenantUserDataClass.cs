using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.GlobalModel.EntityLists
{
    public class TenantUserDataClass
    {
        [Key]
        public string Id { get; set; }

        public bool DoBlocking { get; set; }
        public int TrailDaysLeft { get; set; }
        public int PaidDaysLeft { get; set; }
        public int ExpirationDaysLeft { get; set; }

        public bool IsTrial { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime? PaidUntilDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string BlockType { get; set; }

        public bool PaymentFailure { get; set; }
        public int SuspendDaysLeft { get; set; }
    }
}