using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public partial class QuoteMapping
    {
        public static void MapQuotePrice(QuotePriceStepsPM itemPM, QuotePriceSteps itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.QuoteId = itemPM.QuoteId;
                itemPoco.QuoteChargeId = itemPM.QuoteChargeId;
            }

            itemPoco.CostUnitPrice = itemPM.CostUnitPrice;
            itemPoco.SaleUnitPrice = itemPM.SaleUnitPrice;
            itemPoco.Step = itemPM.Step;
            itemPoco.MarkupValue = itemPM.MarkupValue;
        }
    }
}
