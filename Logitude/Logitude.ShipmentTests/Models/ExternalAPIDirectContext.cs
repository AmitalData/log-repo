using System;

namespace Logitude.ShipmentTests.Models
{
    public class ExternalAPIDirectContext
    {
        public ExternalAPIDirectContext()
        {
            Direct = new Direct();
        }

        public Direct Direct { get; set; }
        public string ExceptionMessage { get; set; }
        public Action act { get; set; }
    }
}