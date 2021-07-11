using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class CreatePhoneCallSteps
    {

        private readonly CRMContext crmContext;
        private readonly ActivityPhoneCallServices activityPhoneCallServices;

        public CreatePhoneCallSteps(CRMContext activitesContext, ActivityPhoneCallServices activityPhoneCallServices)
        {
            this.crmContext = activitesContext;
            this.activityPhoneCallServices = activityPhoneCallServices;
        }

        [Given(@"a phone call with the following properties")]
        public void GivenAPhoneCallWithTheFollowingProperties(Table table)
        {
            crmContext.ActiviyPhoneCall = activityPhoneCallServices.CreateInstance(table);
        }
        
        [When(@"create phone call")]
        public void WhenCreatePhoneCall()
        {
            crmContext.ActiviyPhoneCall = APICaller.CallPost<ActivityPM>(crmContext.ActiviyPhoneCall, Urls.ActivitiesController, UserTenant.Token)?.Data;

        }

        [Then(@"the phone call should create successfully")]
        public void ThenThePhoneCallShouldCreateSuccessfully()
        {
            crmContext.ActiviyPhoneCall.Should().NotBeNull();
            crmContext.ActiviyPhoneCall.Id.Should().NotBeNull();
        }
    }
}
