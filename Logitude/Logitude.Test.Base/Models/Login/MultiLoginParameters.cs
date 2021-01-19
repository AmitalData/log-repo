using System.Collections.Generic;

namespace Logitude.Test.Base.Models.Login
{
    public class MultiLoginParameters
    {
        public MultiLoginParameters()
        {
            Logins = new List<LoginParameters>();
        }

        public List<LoginParameters> Logins { get; set; }
    }
}