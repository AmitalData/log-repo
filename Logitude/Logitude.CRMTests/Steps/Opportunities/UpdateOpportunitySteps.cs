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
    public class UpdateOpportunitySteps
    {
        private readonly CRMContext crmContext;
        private readonly OpportunityServices opportunityServices;

        public UpdateOpportunitySteps(CRMContext crmContext, OpportunityServices opportunityServices)
        {
            this.crmContext = crmContext;
            this.opportunityServices = opportunityServices;
        }

        [Given(@"an opportunity")]
        public void GivenAnOpportunity()
        {
            crmContext.Opportunity = APICaller.CallGet<OpportunityPM>(Urls.OpportunitySingle(CRMData.OpportunityId), UserTenant.Token).Data;
        }

        [Given(@"following opportunity properties")]
        public void GivenFollowingOpportunityProperties(Table table)
        {
            crmContext.Opportunity = opportunityServices.UpdateInstance(table, crmContext.Opportunity);
        }
        
        [When(@"update opportunity")]
        public void WhenUpdateOpportunity()
        {
            crmContext.Opportunity = APICaller.CallPut<OpportunityPM>(crmContext.Opportunity, Urls.OpportunitiesController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the opportunity should update successfully")]
        public void ThenTheOpportunityShouldUpdateSuccessfully()
        {
            crmContext.Opportunity.Should().NotBeNull();
            crmContext.Opportunity.Id.Should().NotBeNull();
        }
    }
}
