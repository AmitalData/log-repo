using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AuthenticationToken
    {
        [Key]
        public string Token { get; set; }
        public int Tenant { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public bool APIToken { get; set; }
        public string APICredentialID { get; set; }
        public string ClientType { get; set; }
        public bool InActive { get; set; }
        public string InActiveReason { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? InActiveDate { get; set; }

    }

}
