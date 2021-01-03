using System.Collections.Generic;

namespace Logitude.SecurityTests.Models.Login
{
    public class LoginsParameters
    {
        public LoginsParameters()
        {
            Logins = new List<LoginParameters>();
        }

        public List<LoginParameters> Logins { get; set; }
    }
}