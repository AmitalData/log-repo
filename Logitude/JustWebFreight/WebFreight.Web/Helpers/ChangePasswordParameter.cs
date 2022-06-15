using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ChangePasswordParameter
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string ContactId { get; set; }
        public string NewPassword { get; set; }

    }

}