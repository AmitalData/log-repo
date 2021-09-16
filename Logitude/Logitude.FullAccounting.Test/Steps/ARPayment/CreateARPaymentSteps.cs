using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.ARPayment
{
    [Binding]
    public class CreateARPaymentSteps
    {
        private readonly FullAccountingContext context;
        private readonly ARPaymentService arPaymentService;
        public CreateARPaymentSteps(FullAccountingContext context, ARPaymentService arPaymentService)
        {
            this.context = context;
            this.arPaymentService = arPaymentService;
        }

        [Given(@"a ar payment with the following properties")]
        public void GivenAArPaymentWithTheFollowingProperties(Table table)
        {
            context.ARPaymentPM = arPaymentService.Create(table);
        }

        [When(@"create ar payment")]
        public void WhenCreateArPayment()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the ar payment should create successfully")]
        public void ThenTheArPaymentShouldCreateSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
