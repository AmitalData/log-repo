using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccountingTests.Steps.Customer
{
    [Binding]
    public class CreateCustomerSteps
    {
        private readonly FullAccountingContext context;
        private readonly CustomerService customerService;
        public CreateCustomerSteps(FullAccountingContext context, CustomerService customerService)
        {
            this.context = context;
            this.customerService = customerService;
        }
        [Given(@"a customer with the following properties")]
        public void GivenACustomerWithTheFollowingProperties(Table table)
        {
            context.CustomerPartner = customerService.Create(table);
        }

        [When(@"create customer")]
        public void WhenCreateCustomer()
        {
            context.CustomerPartner =  APICaller.CallPost<Partner>(context.CustomerPartner, Urls.PartnersDomainController, UserTenant.Token)?.Data;
        }

        [Then(@"the customer should create successfully")]
        public void ThenTheCustomerShouldCreateSuccessfully()
        {
            context.CustomerPartner.Should().NotBeNull();
            context.CustomerPartner.PartnerId.Should().NotBeNull();
        }
    }
}
