using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class PasswordResetRequest
    {
        [Key]
        public string RequestNumber { get; set; }
        public string Email { get; set; }
        public bool IsDone { get; set; }

        public bool IsMobileOnly { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string VerificationCode { get; set; }
        public string Type { get; set; }


    }
}
