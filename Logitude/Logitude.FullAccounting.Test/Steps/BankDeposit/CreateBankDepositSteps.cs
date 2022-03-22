using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankDeposit
{
    [Binding]
    public class CreateBankDepositSteps
    {
        private readonly FullAccountingContext context;
        private readonly BankDepositService bankDepositService;
        public CreateBankDepositSteps(FullAccountingContext context, BankDepositService bankDepositService)
        {
            this.context = context;
            this.bankDepositService = bankDepositService;
        }
        [Given(@"a bank deposit with the following properties")]
        public void GivenABankDepositWithTheFollowingProperties(Table table)
        {
            context.BankDeposit = bankDepositService.Create(table);
        }

        [When(@"create bank deposit")]
        public void WhenCreateBankDeposit()
        {
            context.AddedBankDeposit = APICaller.CallPost<BankDepositPM>(context.BankDeposit, Urls.BankDepositsController, UserTenant.Token)?.Data;
        }

        [Then(@"the bank deposit should create successfully")]
        public void ThenTheBankDepositShouldCreateSuccessfully()
        {
            context.AddedBankDeposit.Id.Should().NotBeNullOrEmpty();
        }
    }
}
