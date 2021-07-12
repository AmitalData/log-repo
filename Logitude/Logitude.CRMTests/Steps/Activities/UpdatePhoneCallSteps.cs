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
    public class UpdatePhoneCallSteps
    {
        private readonly CRMContext crmContext;
        private readonly ActivityPhoneCallServices activityPhoneCallServices;

        public UpdatePhoneCallSteps(CRMContext crmContext, ActivityPhoneCallServices activityPhoneCallServices)
        {
            this.crmContext = crmContext;
            this.activityPhoneCallServices = activityPhoneCallServices;
        }

        [Given(@"phone call")]
        public void GivenPhoneCall()
        {
            crmContext.ActiviyPhoneCall = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityPhoneCallId), UserTenant.Token).Data;
        }

        [Given(@"following phone call properties")]
        public void GivenFollowingPhoneCallProperties(Table table)
        {
            crmContext.ActiviyPhoneCall = activityPhoneCallServices.UpdateInstance(table, crmContext.ActiviyPhoneCall);
        }
        
        [When(@"update phone call")]
        public void WhenUpdatePhoneCall()
        {
            crmContext.ActiviyPhoneCall = APICaller.CallPut<ActivityPM>(crmContext.ActiviyPhoneCall, Urls.ActivitiesController, UserTenant.Token)?.Data;
        }

        [Then(@"the phone call should update successfully")]
        public void ThenThePhoneCallShouldUpdateSuccessfully()
        {
            crmContext.ActiviyPhoneCall.Should().NotBeNull();
            crmContext.ActiviyPhoneCall.Id.Should().NotBeNull();
        }
    }
}
