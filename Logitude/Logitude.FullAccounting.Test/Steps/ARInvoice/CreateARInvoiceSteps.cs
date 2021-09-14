using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.ARInvoice
{
    [Binding]
    public class CreateARInvoiceSteps
    {
        [Given(@"a AR invoice with the following properties")]
        public void GivenAARInvoiceWithTheFollowingProperties(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"create AR invoice")]
        public void WhenCreateARInvoice()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the AR invoice should create successfully")]
        public void ThenTheARInvoiceShouldCreateSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
