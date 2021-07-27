using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.CRMTests.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Opportunities
{
    [Binding]
    public class CreateOpportunitySteps
    {

        private readonly CRMContext crmContext;
        private readonly OpportunityServices opportunityServices;

        public CreateOpportunitySteps(CRMContext crmContext, OpportunityServices opportunityServices)
        {
            this.crmContext = crmContext;
            this.opportunityServices = opportunityServices;
        }
        [Given(@"a opportunity with the following properties")]
        public void GivenAOpportunityWithTheFollowingProperties(Table table)
        {
            crmContext.Opportunity = opportunityServices.CreateInstance(table);
        }
        
        [When(@"create opportunity")]
        public void WhenCreateOpportunity()
        {
            crmContext.Opportunity = APICaller.CallPost<OpportunityPM>(crmContext.Opportunity, Urls.OpportunitiesController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the opportunity should create successfully")]
        public void ThenTheOpportunityShouldCreateSuccessfully()
        {
            crmContext.Opportunity.Should().NotBeNull();
            crmContext.Opportunity.Id.Should().NotBeNull();
        }
    }
}
