using Logitude.TimeManagementTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.TimeManagementTests.Hooks
{
    [Binding]
    public sealed class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupTimeManagementPreparation()
        {
            new TimeManagementDataPreparation().Prepar();
        }
    }
}
