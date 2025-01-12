using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class InvalidEmailResetPassword
    {
        [Key]
        public string Id { get; set; }
        public string IP { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
    }
}
