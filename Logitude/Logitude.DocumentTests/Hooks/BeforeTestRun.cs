using Logitude.DocumentTests.Models;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.Logitude.FullAccounting.Test.Hooks
{
    [Binding]
    public sealed class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupTimeManagementPreparation()
        {
            DocumentData.ShipmentObjectTableId = new ObjectTableService().GetIdByName("Shipment");
        }

    }
}
