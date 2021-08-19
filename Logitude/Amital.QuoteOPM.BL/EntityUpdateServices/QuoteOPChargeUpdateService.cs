using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    partial class QuoteOPChargeUpdateService
    {
        protected override void OnCreating(QuoteOPChargePM entityPM, QuoteOPPM entityParentPM)
        {
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.QuoteOPId = entityParentPM.Id;
            entityPM.Id= IdCounter.GetNumber("QuoteOPCharge", entityParentPM.Tenant).ToString();


            if (!string.IsNullOrEmpty(entityPM.TariffId))
            {
                //NO TARIFF !!!this.UpdateTariffUsedDate(itemPM.TariffId);
            }
            base.OnCreating(entityPM, entityParentPM);
        }
        
        protected override void UpdateComposition(QuoteOPChargePM entityPM)
        {
            if (entityPM.IsChargeBySteps)//????  
            {
                QuoteOPPriceStepsUpdateService quoteOPPriceStepsUpdateService = new QuoteOPPriceStepsUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                quoteOPPriceStepsUpdateService.UpdateMulti(entityPM.QuoteOPChargePriceSteps, entityPM.DeletedQuoteOPChargePriceSteps, entityPM, false);
            }
            base.UpdateComposition(entityPM);
        }
    }
}
