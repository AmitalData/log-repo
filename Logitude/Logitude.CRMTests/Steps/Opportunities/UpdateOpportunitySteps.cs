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
        private OpportunityPM updatedOportunity;

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
           opportunityServices.UpdateInstance(table, crmContext.Opportunity);
        }
        
        [When(@"update opportunity")]
        public void WhenUpdateOpportunity()
        {
            updatedOportunity = APICaller.CallPut<OpportunityPM>(crmContext.Opportunity, Urls.OpportunitiesController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the opportunity should update successfully")]
        public void ThenTheOpportunityShouldUpdateSuccessfully()
        {
            updatedOportunity.Should().NotBeNull();
            updatedOportunity.Id.Should().NotBeNull();
            updatedOportunity.Subject.Should().Equals(crmContext.Opportunity.Subject);
            updatedOportunity.OpportunityTypeId.Should().Equals(crmContext.Opportunity.OpportunityTypeId);
            updatedOportunity.NumberOfShipments.Should().Equals(crmContext.Opportunity.NumberOfShipments);
            updatedOportunity.StageId.Should().Equals(crmContext.Opportunity.StageId);
            updatedOportunity.RatingCode.Should().Equals(crmContext.Opportunity.RatingCode);
        }
    }
}
