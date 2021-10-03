using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class PostCreatePeriodsForYearSteps
    {
        private readonly FullAccountingContext context;
        public PostCreatePeriodsForYearSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"post create periods for year by not login user")]
        public void WhenPostCreatePeriodsForYearByNotLoginUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, Urls.PostCreatePeriodsForYear("2021"), "0");
        }

        [When(@"post create periods for year by not authorize user")]
        public void WhenPostCreatePeriodsForYearByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, Urls.PostCreatePeriodsForYear("2021"), UserEmptyTenant.Token);
        }

        [Then(@"the post create periods for year api should return you have no permissions")]
        public void ThenThePostCreatePeriodsForYearApiShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
