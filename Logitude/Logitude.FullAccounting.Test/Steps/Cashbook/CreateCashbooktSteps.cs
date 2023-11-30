using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Cashbook
{
    [Binding]
    public class CreateCashbooktSteps
    {
        private readonly FullAccountingContext context;
        private readonly CashbooktService cashbooktService;
        public CreateCashbooktSteps(FullAccountingContext context, CashbooktService cashbooktService)
        {
            this.context = context;
            this.cashbooktService = cashbooktService;
        }
        [Given(@"a cashbook with the following properties")]
        public void GivenACashbookWithTheFollowingProperties(Table table)
        {
            context.CashbookCreated = cashbooktService.Create(table);
        }

        [When(@"create cashbook")]
        public void WhenCreateCashbook()
        {
            context.AddedCashbook = APICaller.CallPost<CashBookPM>(context.CashbookCreated, Urls.CashBooksController, UserTenant.Token)?.Data;
        }

        [Then(@"the cashbook should create successfully")]
        public void ThenTheCashbookShouldCreateSuccessfully()
        {
            context.AddedCashbook.Id.Should().NotBeNullOrEmpty();
        }
    }
}
