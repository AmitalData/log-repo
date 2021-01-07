using Logitude.Test.Base.Models.Login;
using System.Collections.Generic;

namespace Logitude.ShipmentTests.Models
{
    public class ExternalAPIDirectContext
    {
        public ExternalAPIDirectContext()
        {
            Direct = new Direct();
        }

        public Direct Direct { get; set; }
        public User User { get; set; }
    }
}