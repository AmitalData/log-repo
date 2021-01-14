using Logitude.ShipmentTests.TestData;
using System.Collections.Generic;

namespace Logitude.ShipmentTests.TestDataMapping
{
    public class ShipmentMapping
    {
        public static readonly Dictionary<string, string> Ports = new Dictionary<string, string>(){
            {"LHR", ShipmentTestData.PortLHRId},
            {"MIA", ShipmentTestData.PortMIAId},
            {"JFK", ShipmentTestData.PortJFKId},
            {"SOU", ShipmentTestData.PortSOUId},
            {"NYC", ShipmentTestData.PortNYCId},
            {"LON", ShipmentTestData.PortLONId},
            {"MAN", ShipmentTestData.PortMANId}
        };
    }
}
