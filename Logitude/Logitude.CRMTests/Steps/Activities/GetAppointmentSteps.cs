using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class GetAppointmentSteps
    {
        private readonly CRMContext crmContext;

        public GetAppointmentSteps(CRMContext crmContext)
        {
            this.crmContext = crmContext;
        }

        [When(@"get appointment with AppointmentId")]
        public void WhenGetAppointmentWithAppointmentId()
        {
            crmContext.ActiviyAppointment = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityAppointmentId), UserTenant.Token).Data;
        }
        
        [Then(@"appointment should be avaliable")]
        public void ThenAppointmentShouldBeAvaliable()
        {
            crmContext.ActiviyAppointment.Should().NotBeNull();
            crmContext.ActiviyAppointment.Id.Should().NotBeNull();
        }
    }
}
