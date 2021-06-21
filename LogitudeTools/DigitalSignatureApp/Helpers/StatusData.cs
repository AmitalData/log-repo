using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Sign.App.Helpers
{
    public class StatusData
    {
        public int Tenant { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastStatusDate { get; set; }
        public bool IsLogged { get; set; }
        public string LoggedByUserEmail { get; set; }
        public bool IsValidCert { get; set; }
    }
}
