using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class UpdateAppointmentSteps
    {


        private readonly CRMContext crmContext;
        private readonly ActivityAppointmentServices appointmentServices;

        public UpdateAppointmentSteps(CRMContext crmContext, ActivityAppointmentServices appointmentServices)
        {
            this.crmContext = crmContext;
            this.appointmentServices = appointmentServices;
        }

        [Given(@"an appointment")]
        public void GivenAnAppointment()
        {
            crmContext.ActiviyAppointment = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityAppointmentId), UserTenant.Token).Data;
        }

        [Given(@"following appointment properties")]
        public void GivenFollowingAppointmentProperties(Table table)
        {
            crmContext.ActiviyAppointment = appointmentServices.UpdateInstance(table, crmContext.ActiviyAppointment);
        }
        
        [When(@"update appointment")]
        public void WhenUpdateAppointment()
        {
            crmContext.ActiviyAppointment = APICaller.CallPut<ActivityPM>(crmContext.ActiviyAppointment, Urls.ActivitiesController, UserTenant.Token)?.Data;
        }

        [Then(@"the appointment should update successfully")]
        public void ThenTheAppointmentShouldUpdateSuccessfully()
        {
            crmContext.ActiviyAppointment.Should().NotBeNull();
            crmContext.ActiviyAppointment.Id.Should().NotBeNull();
        }
    }
}
