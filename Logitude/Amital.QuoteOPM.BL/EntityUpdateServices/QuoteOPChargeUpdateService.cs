using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
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

            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void UpdateComposition(QuoteOPChargePM entityPM)
        {
            //if (itemPM.IsChargeBySteps)
            //{
            //    if (itemPM.QuoteChargePriceSteps != null)
            //    {
            //        foreach (QuotePriceStepsPM insideItemPM in itemPM.QuoteChargePriceSteps)
            //        {
            //            insideItemPM.QuoteId = entityPM.Id;
            //            insideItemPM.QuoteChargeId = itemPM.Id;
            //            this.CreateQuotePriceSteps(insideItemPM);
            //        }
            //    }
            //}
            base.UpdateComposition(entityPM);
        }
    }
}
