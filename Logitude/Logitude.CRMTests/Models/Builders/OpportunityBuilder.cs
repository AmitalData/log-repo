using Logitude.Base.Models.Partners;
using Logitude.Base.Models.UserTenant;
using System;

namespace Logitude.CRMTests.Models.Builders
{
    public class OpportunityBuilder
    {
        private OpportunityPM _opportunity;
        public OpportunityBuilder()
        {
            this.Reset();
        }

        public OpportunityPM Build()
        {
            OpportunityPM result = _opportunity;
            this.Reset();
            return result;
        }


        private void Reset()
        {
            _opportunity = new OpportunityPM();
        }

        public OpportunityBuilder Subject(string subject)
        {
            _opportunity.Subject = subject;
            return this;
        } 
        
        public OpportunityBuilder NumberOfShipments(int numberOfShipments)
        {
            _opportunity.NumberOfShipments = numberOfShipments;
            return this;
        }

        public OpportunityBuilder OpportunityTypeId(string opportunityTypeId)
        {
            _opportunity.OpportunityTypeId = opportunityTypeId;
            return this;
        }

        public OpportunityBuilder StageId(string stageId)
        {
            _opportunity.StageId = stageId;
            return this;
        }
        public OpportunityBuilder RatingCode(string ratingCode)
        {
            _opportunity.RatingCode = ratingCode;
            return this;
        }

        public OpportunityBuilder WithModel(OpportunityPM opportunity)
        {
            _opportunity = opportunity;
            return this;
        }

        public OpportunityBuilder WithDefualtValues()
        {
            _opportunity = new OpportunityPM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                ContactId = PartnersData.CustomerContactId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                OwnerId = UserTenant.UserId,
                BusinessUnitId = UserTenant.BusinessUnitId
            };
            return this;
        }

    }
}
