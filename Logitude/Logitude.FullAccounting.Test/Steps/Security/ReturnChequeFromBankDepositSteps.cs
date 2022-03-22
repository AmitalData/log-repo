using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;


namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class ReturnChequeFromBankDepositSteps
    {
        private readonly FullAccountingContext context;
        private readonly PostReturnChequeArgs postReturnChequeArgs;
        public ReturnChequeFromBankDepositSteps(FullAccountingContext context)
        {
            this.context = context;
            postReturnChequeArgs = new PostReturnChequeArgs()
            {
                ARPChequeId = FullAccountingData.ChequeBankDepositARPaymentChequeId,
                BankDepositId = FullAccountingData.BankDepositChequeId,
                ReturnType = BankDepositReturnTypeCodes.Cashbook,
                Notes = ""
            };
        }

        [When(@"post return cheque bank deposit by not authentication user")]
        public void WhenPostReturnChequeBankDepositByNotAuthenticationUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, FullAccountingUrls.PostReturnCheque(postReturnChequeArgs), "0");
        }

        [When(@"post return cheque bank deposit by not authorize user")]
        public void WhenPostReturnChequeBankDepositByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallPost<object>(null, FullAccountingUrls.PostReturnCheque(postReturnChequeArgs), UserTenant.Token);
        }

        [Then(@"the post return cheque bank deposit api should return you have no permissions")]
        public void ThenThePostReturnChequeBankDepositApiShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
