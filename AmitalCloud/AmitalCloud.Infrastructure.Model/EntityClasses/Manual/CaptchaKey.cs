using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CaptchaKey
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }
        public string Activity { get; set; }
        public bool IsUsed { get; set; }
    }
}
