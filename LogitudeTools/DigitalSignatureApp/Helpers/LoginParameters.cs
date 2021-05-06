using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Sign.App.Helpers
{
    public class LoginParameters
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsUser { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public bool ByToken { get; set; }
        public bool IsMobileLogin { get; set; }
        public bool GetToken { get; set; }
        public int MobileVersion { get; set; }
        public bool IsAngularLogin { get; set; }

    }

    public class LoginTokenParameter
    {
        public string Token { get; set; }
        public int Tenant { get; set; }
        public string ContactId { get; set; }
        public string CardId { get; set; }
        public bool IsMobileLogin { get; set; }
        public int MobileVersion { get; set; }
    }
}
