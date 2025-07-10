using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class UserLastLoginPM
    {
        [Key]
        public string Id { get; set; }
        [Key]
        public string ComputerId { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        [Key]
        public string WorkEnvironment { get; set; }

        public UserPM User { get; set; }
        public string IP { get; set; }
    }
}
