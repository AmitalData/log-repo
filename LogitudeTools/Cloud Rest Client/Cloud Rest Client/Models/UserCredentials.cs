using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Rest_Client.Models
{
    public class UserCredentials
    { 
        public string Email { get; set; }
        public string Password { get; set; }

        public int Tenant { get; set; } // optional
    }
}
