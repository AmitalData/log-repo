using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AutoSignupEmail
    {
        [Key]
        public string Id { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string Status { get; set; }
        public int Retries { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
