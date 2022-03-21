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
    public class CreateAppointmentSteps
    {

        private readonly CRMContext crmContext;
        private readonly ActivityAppointmentServices activityAppointmentServices;

        public CreateAppointmentSteps(CRMContext crmContext, ActivityAppointmentServices activityAppointmentServices)
        {
            this.crmContext = crmContext;
            this.activityAppointmentServices = activityAppointmentServices;
        }

        [Given(@"a appointment with the following properties")]
        public void GivenAAppointmentWithTheFollowingProperties(Table table)
        {
            crmContext.ActiviyAppointment = activityAppointmentServices.CreateInstance(table);
        }
        
        [When(@"create appointment")]
        public void WhenCreateAppointment()
        {
            crmContext.ActiviyAppointment = APICaller.CallPost<ActivityPM>(crmContext.ActiviyAppointment, Urls.ActivitiesController, UserTenant.Token)?.Data;
        }

        [Then(@"the appointment should create successfully")]
        public void ThenTheAppointmentShouldCreateSuccessfully()
        {
            crmContext.ActiviyAppointment.Should().NotBeNull();
            crmContext.ActiviyAppointment.Id.Should().NotBeNull();
        }
    }
}
