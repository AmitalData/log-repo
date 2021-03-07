using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteSettingMapping
    {
        public static void MapEntity(QuoteSettingPM entityPM, QuoteSetting entityPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPoco.Id = entityPM.Id;
                entityPoco.Tenant = entityPM.Tenant;
            }

            entityPoco.CopyShipper = entityPM.CopyShipper;
            entityPoco.CopyConsignee = entityPM.CopyConsignee;
            entityPoco.CopyMainCarriage = entityPM.CopyMainCarriage;
            entityPoco.CopyPickup = entityPM.CopyPickup;
            entityPoco.CopyDelivery = entityPM.CopyDelivery;
            entityPoco.CopyChargesTypes = entityPM.CopyChargesTypes;
            entityPoco.CopyChargesCost = entityPM.CopyChargesCost;
            entityPoco.CopyChargesSale = entityPM.CopyChargesSale;
            entityPoco.EditMainCarriage = entityPM.EditMainCarriage;
            entityPoco.CopyAgent = entityPM.CopyAgent;
            entityPoco.CopyNotify = entityPM.CopyNotify;
            entityPoco.IsSaleAsCostCurrency = entityPM.IsSaleAsCostCurrency;
            entityPoco.CopyExchangeRates = entityPM.CopyExchangeRates;
            entityPoco.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
            entityPoco.IsMultiCurrency = entityPM.IsMultiCurrency;
            entityPoco.QuoteExpirationDays = entityPM.QuoteExpirationDays;
        }
    }
}
