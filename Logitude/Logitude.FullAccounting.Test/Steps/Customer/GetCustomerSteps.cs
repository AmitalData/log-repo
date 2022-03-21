using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccountingTests.Steps.Customer
{
    [Binding]
    public class GetCustomerSteps
    {
        private readonly FullAccountingContext context;
        public GetCustomerSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get customer with customerId")]
        public void WhenGetCustomerWithCustomerId()
        {
            context.Customer = APICaller.CallGet<CustomerPM>(Urls.GetCustomerById(FullAccountingData.CustomerId), UserTenant.Token).Data;
        }

        [Then(@"customer should be available")]
        public void ThenCustomerShouldBeAvailable()
        {
            context.Customer.Should().NotBeNull();
            context.Customer.Id.Should().NotBeNull();
        }
    }
}
