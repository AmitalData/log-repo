using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentQuoteUsageBehaviour : IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            this.InitializeFlags();
            this.UpdateQuoteUsage();
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
                QuoteRepository quoteRepository = new QuoteRepository(initializer.Tenant);
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

                        quote.LastUsageDate = TenantServerConfigration.GetCurrentDateTime(initializer.Tenant);
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
                    }

                    quoteRepository.Update(quote);
                    quoteRepository.SubmitChanges();
                }
            }
        }

    }
}
