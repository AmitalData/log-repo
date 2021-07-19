using Logitude.CRMTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new ActivityTaskDataPreparation().Prepar();
            new ActivityAppointmentDataPreparation().Prepar();
            new ActivityPhoneCallDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Activity-Task")]
        public static void SetUpPrepareDataBeforeFeatureRunActivityTask()
        {
            new ActivityTaskDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Activity-Appointment")]
        public static void SetUpPrepareDataBeforeFeatureRunActivityAppointment()
        {
            new ActivityAppointmentDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Activity-PhoneCall")]
        public static void SetUpPrepareDataBeforeFeatureRunActivityPhoneCall()
        {
            new ActivityPhoneCallDataPreparation().Prepar();
        }
    }
}
