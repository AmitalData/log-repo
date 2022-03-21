using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankAccount
{
    [Binding]
    public class CreateBankAccountSteps
    {
        private readonly FullAccountingContext context;
        private readonly BankAccountService bankAccountService;
        public CreateBankAccountSteps(FullAccountingContext context, BankAccountService bankAccountService)
        {
            this.context = context;
            this.bankAccountService = bankAccountService;
        }
        [Given(@"a bank account with the following properties")]
        public void GivenABankAccountWithTheFollowingProperties(Table table)
        {
            context.BankAccount = bankAccountService.Create(table);
        }

        [When(@"create bank account")]
        public void WhenCreateBankAccount()
        {
            context.AddedBankAccount = APICaller.CallPost<BankAccountPM>(context.BankAccount, Urls.BankAccountsController, UserTenant.Token)?.Data;
        }

        [Then(@"the bank account should create successfully")]
        public void ThenTheBankAccountShouldCreateSuccessfully()
        {
            context.AddedBankAccount.Id.Should().NotBeNullOrEmpty();
        }
    }
}
