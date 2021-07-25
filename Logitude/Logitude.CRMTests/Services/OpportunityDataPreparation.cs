using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;


namespace Logitude.CRMTests.Services
{
    public class OpportunityDataPreparation
    {

        public void Prepar()
        {
            try
            {
                ApiResponse<OpportunityPM> response = APICaller.CallPost<OpportunityPM>(GetValidOpportunity(), Urls.OpportunitiesController, UserTenant.Token);
                OpportunityDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Opportunity Before Feature Run :" + e.InnerException);
            }
        }

        private OpportunityPM GetValidOpportunity()
        {
            OpportunityServices opportunityServices = new OpportunityServices();
            return new OpportunityBuilder()
               .WithDefualtValues()
               .Subject("pre specflow sub")
               .NumberOfShipments(4)
               .OpportunityTypeId(opportunityServices.GetOpportunityTypeIdByName("Expansion"))
               .StageId(opportunityServices.GetStageIdByName("Qualification"))
               .RatingCode("N")
               .Build();
        }

        private void OpportunityDataMap(OpportunityPM opportunity)
        {
            CRMData.OpportunityId = opportunity.Id;
        }
    }
}
