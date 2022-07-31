using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.Behaviours.QuoteBehaviours
{
    public class QuoteOpportunityBehaviour : IServiceBehaviour
    {
        private QuotePM entityPM;
        private QuoteServiceInitializer initializer;
        private ICRMContext crmContext;
        private int? numberOfQuotes;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.crmContext = CRMContext.GetContext(this.initializer.Tenant);
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            Initialize();
            UpdateOpportunityNumberOfQuotes();
            UpdateQuoteConnectedToOpportunityField();
            SubmitOpportunityChanges();
        }

        string opportunityId = null;
        bool isConnecting = false;
        bool isDisconnecting = false;
        private void Initialize()
        {
            if (initializer.IsNewEntity && !string.IsNullOrEmpty(initializer.EntityPM.OpportunityId))
            {
                isConnecting = true;
                opportunityId = initializer.EntityPM.OpportunityId;
            }

            else if (!string.IsNullOrEmpty(initializer.EntityPM.OpportunityId) && string.IsNullOrEmpty(initializer.EntityPOCO.OpportunityId))
            {
                isConnecting = true;
                opportunityId = initializer.EntityPM.OpportunityId;
            }

            else if (string.IsNullOrEmpty(initializer.EntityPM.OpportunityId) && !string.IsNullOrEmpty(initializer.EntityPOCO.OpportunityId))
            {
                isDisconnecting = true;
                opportunityId = initializer.EntityPOCO.OpportunityId;
            }
        }

        private void UpdateOpportunityNumberOfQuotes()
        {
            if (!this.UpdateOpportunity())
            {
                return;
            }

            OpportunityRepository opportunityRepository = new OpportunityRepository(crmContext);
            Opportunity opportunity = opportunityRepository.GetSingle(opportunityId, initializer.Tenant);

            if (opportunity == null)
            {
                return;
            }

            if (isConnecting)
            {
                SetNumberOnConnecting(opportunity);                
            }

            if (isDisconnecting)
            {
                SetNumberOnDisconnecting(opportunity);                
            }

            numberOfQuotes = opportunity.NumberOfConnectedQuotes;
            opportunityRepository.Update(opportunity);
        }
        private void SetNumberOnConnecting(Opportunity opportunity)
        {
            if (opportunity.NumberOfConnectedQuotes == null)
            {
                opportunity.NumberOfConnectedQuotes = 1;
            }

            else
            {
                opportunity.NumberOfConnectedQuotes += 1;
            }
        }
        private void SetNumberOnDisconnecting(Opportunity opportunity)
        {
            if (opportunity.NumberOfConnectedQuotes == 1)
            {
                opportunity.NumberOfConnectedQuotes = null;
            }

            else
            {
                opportunity.NumberOfConnectedQuotes -= 1;
            }
        }

        private void UpdateQuoteConnectedToOpportunityField()
        {
            if (!this.UpdateOpportunity())
            {
                return;
            }

            if (isConnecting)
            {
                entityPM.ConnectedToOpportunity = true;
            }
            else if (isDisconnecting && numberOfQuotes == null)
            {
                entityPM.ConnectedToOpportunity = false;
            }
        }

        private void SubmitOpportunityChanges()
        {
            if (this.UpdateOpportunity())
            {
                crmContext.SaveChanges();
            }
        }
        private bool UpdateOpportunity()
        {
            if (isConnecting)
            {
                return true;
            }

            else if (isDisconnecting)
            {
                return true;
            }

            return false;
        }
    }
}
