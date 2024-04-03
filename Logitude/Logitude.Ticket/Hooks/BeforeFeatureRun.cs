using Logitude.TicketTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.TicketTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new TicketDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Ticket")]
        public static void SetUpPrepareDataBeforeFeatureRunTicket()
        {
            new TicketDataPreparation().Prepar();
        }
    }
}
