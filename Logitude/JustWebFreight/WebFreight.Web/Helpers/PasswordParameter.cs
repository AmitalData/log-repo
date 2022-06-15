using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class PasswordParameter
    {
        public string Password { get; set; }
        public bool isHashPassword { get; set; }
        public bool IsOneTimePassword { get; set; }

    }

}