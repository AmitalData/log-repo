using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class OneTimePassword
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UsageDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsUsed { get; set; }


    }
}
