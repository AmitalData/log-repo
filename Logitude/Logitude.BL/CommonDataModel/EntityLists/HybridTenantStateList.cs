using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class HybridTenantStateList
    {
        [Key]
       
        public int Tenant { get; set; }
        public int FailedQueue { get; set; }
      
        public int WaitingQueue { get; set; }

        public string LastUpdateDateTimeTextColor { get; set; }
        public string WaitingQueueTextColor { get; set; }
        public string FailedQueueTextColor { get; set; }
        public string TenantName { get; set; }



        public DateTime LastUpdateDateTime { get; set; }
        public DateTime? LastQueueDateTime { get; set; }
    }
}