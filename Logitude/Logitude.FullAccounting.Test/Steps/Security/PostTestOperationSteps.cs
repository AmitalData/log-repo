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
    public class PostTestOperationSteps
    {
        private readonly FullAccountingContext context;
        public PostTestOperationSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"post test operation by not authentication user")]
        public void WhenPostTestOperationByNotAuthenticationUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, Urls.PostTestOperation, "0");
        }
        
        [When(@"post test operation by not authorize user")]
        public void WhenPostTestOperationByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, Urls.PostTestOperation, UserEmptyTenant.Token);
        }
        
        [Then(@"the post test operation api should return you have no permissions")]
        public void ThenThePostTestOperationApiShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
