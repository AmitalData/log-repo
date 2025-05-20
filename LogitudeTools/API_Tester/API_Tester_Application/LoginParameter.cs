using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientApplication
{
    public class LoginParameter
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsUser { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public bool ByToken { get; set; }
        public bool IsMobileLogin { get; set; }
        public bool GetToken { get; set; }
         
    }
}
