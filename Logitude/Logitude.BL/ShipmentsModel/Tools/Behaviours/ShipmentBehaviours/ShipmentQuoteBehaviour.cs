using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentQuoteBehaviour : IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;
        private IQuotesContext quoteContext;
        private int? quoteUsageCount;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.quoteContext = QuotesContext.GetContext(this.initializer.Tenant);
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            InitializeFlags();
            UpdateQuoteUsage();
            UpdateQuoteConnectedToShipmentComputedField();
            SubmitQuoteChanges();            
        }

        string quoteId = null;
        bool isUpdatingUsage = false;
        bool isDisconnectingQoute = false;
        private void InitializeFlags()
        {
            if (initializer.IsNewEntity && !string.IsNullOrEmpty(initializer.EntityPM.QuoteId))
            {
                isUpdatingUsage = true;
                quoteId = initializer.EntityPM.QuoteId;
            }

            else if (!string.IsNullOrEmpty(initializer.EntityPM.QuoteId) && string.IsNullOrEmpty(initializer.EntityPOCO.QuoteId))
            {
                isUpdatingUsage = true;
                quoteId = initializer.EntityPM.QuoteId;
            }

            else if (string.IsNullOrEmpty(initializer.EntityPM.QuoteId) && !string.IsNullOrEmpty(initializer.EntityPOCO.QuoteId))
            {
                isDisconnectingQoute = true;
                quoteId = initializer.EntityPOCO.QuoteId;
            }
        }

        private void UpdateQuoteUsage()
        {
            if (isUpdatingUsage || isDisconnectingQoute)
            {
                QuoteRepository quoteRepository = new QuoteRepository(quoteContext);
                Quote quote = quoteRepository.GetSingleQuote(quoteId, initializer.Tenant);

                if (quote != null)
                {
                    if (isUpdatingUsage)
                    {
                        if (quote.UsageCount == null)
                        {
                            quote.UsageCount = 1;
                        }

                        else
                        {
                            quote.UsageCount += 1;
                        }

                        quote.LastUsageDate = initializer.TodayDateTime;
                        initializer.EntityPM.QuoteFreightExpirationDate = quote.ExpirationDate;
                    }

                    if (isDisconnectingQoute)
                    {
                        if (quote.UsageCount == 1)
                        {
                            quote.UsageCount = null;
                        }

                        else
                        {
                            quote.UsageCount -= 1;
                        }

                        initializer.EntityPM.QuoteFreightExpirationDate = null;
                    }
                    quoteUsageCount = quote.UsageCount;
                    quoteRepository.Update(quote);                    
                }
            }
        }

        private void UpdateQuoteConnectedToShipmentComputedField()
        {
            if (isUpdatingUsage || isDisconnectingQoute)
            {
                QuoteComputedFieldRepository quoteComputedFieldRepository = new QuoteComputedFieldRepository(quoteContext);
                QuoteComputedField quoteComputedField = quoteComputedFieldRepository.GetSingleQuoteComputedField(quoteId, initializer.Tenant);
                if (quoteComputedField != null)
                {
                    if (isUpdatingUsage)
                    {
                        quoteComputedField.ConnectedToShipment = true;
                    }
                    else if (isDisconnectingQoute && quoteUsageCount == null)
                    {
                        quoteComputedField.ConnectedToShipment = false;
                    }
                    quoteComputedFieldRepository.Update(quoteComputedField);                    
                }
            } 
        }

        private void SubmitQuoteChanges()
        {
            if (isUpdatingUsage || isDisconnectingQoute)
            {
            quoteContext.SaveChanges();
            }
        }
    }
}
