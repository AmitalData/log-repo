using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    partial class QuoteOPPriceStepsUpdateService
    {
        protected override void OnCreating(QuoteOPPriceStepsPM entityPM, QuoteOPChargePM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("QuoteOPPriceSteps", entityParentPM.Tenant).ToString();
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
