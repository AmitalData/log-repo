using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Opportunities
{
    [Binding]
    public class GetOpportunitySteps
    {
        private readonly CRMContext crmContext;

        public GetOpportunitySteps(CRMContext crmContext)
        {
            this.crmContext = crmContext;
        }

        [When(@"get opportunity with OpportunityId")]
        public void WhenGetOpportunityWithOpportunityId()
        {
            crmContext.Opportunity = APICaller.CallGet<OpportunityPM>(Urls.OpportunitySingle(CRMData.OpportunityId), UserTenant.Token).Data;
        }
        
        [Then(@"opportunity should be avaliable")]
        public void ThenOpportunityShouldBeAvaliable()
        {
            crmContext.Opportunity.Should().NotBeNull();
            crmContext.Opportunity.Id.Should().NotBeNull();
        }
    }
}
