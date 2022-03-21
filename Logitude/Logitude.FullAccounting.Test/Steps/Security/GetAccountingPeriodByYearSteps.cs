using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class GetAccountingPeriodByYearSteps
    {
        private readonly FullAccountingContext context;
        public GetAccountingPeriodByYearSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get accounting period by year by not login user")]
        public void WhenGetAccountingPeriodByYearByNotLoginUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetAccountingPeriodByYear(2021,"type"), "0");
        }

        [When(@"get accounting period by year by not authorize user")]
        public void WhenGetAccountingPeriodByYearByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetAccountingPeriodByYear(2021, "type"), UserEmptyTenant.Token);
        }

        [Then(@"the get accounting period by year api should return you have no permissions")]
        public void ThenTheGetAccountingPeriodByYearApiShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
