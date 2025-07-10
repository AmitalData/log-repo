using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AutomationLastUpdate
    {


        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ObjectTableId { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool HasAutomation { get; set; }




    }
}
