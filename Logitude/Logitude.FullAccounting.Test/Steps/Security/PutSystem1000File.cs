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
    public class PutSystem1000FileSteps
    {
        private readonly FullAccountingContext context;
        public PutSystem1000FileSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"put system file by not login user")]
        public void WhenPutSystemFileByNotLoginUser()
        {
            context.Action = () => APICaller.CallPut<object>(null,Urls.PutSystem1000File, "0");
        }

        [When(@"put system file by not authorize user")]
        public void WhenPutSystemFileByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallPut<object>(null, Urls.PutSystem1000File, UserEmptyTenant.Token);
        }

        [Then(@"the put system file api should return you have no permissions")]
        public void ThenThePutSystemFileAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
