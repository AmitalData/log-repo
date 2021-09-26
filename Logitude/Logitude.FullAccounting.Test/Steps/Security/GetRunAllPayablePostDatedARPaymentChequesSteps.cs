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
    public class GetRunAllPayablePostDatedARPaymentChequesSteps
    {
        private readonly FullAccountingContext context;
        public GetRunAllPayablePostDatedARPaymentChequesSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get run all payable post dated arpayment cheques by not login user")]
        public void WhenGetRunAllPayablePostDatedArpaymentChequesByNotLoginUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetRunAllPayablePostDatedARPaymentCheques(UserTenant.Tenant), "0");
        }

        [When(@"get run all payable post dated arpayment cheques by not authorize user")]
        public void WhenGetRunAllPayablePostDatedArpaymentChequesByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetRunAllPayablePostDatedARPaymentCheques(UserTenant.Tenant), UserEmptyTenant.Token);
        }

        [When(@"get run all payable post dated arpayment cheques from unauthorizes tenant")]
        public void WhenGetRunAllPayablePostDatedArpaymentChequesFromUnauthorizesTenant()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetRunAllPayablePostDatedARPaymentCheques(UserTenant.Tenant), UserOtherTenant.Token);
        }

        [Then(@"get run all payable post dated arpayment cheques api should return you have no permissions")]
        public void ThenGetRunAllPayablePostDatedArpaymentChequesApiShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
