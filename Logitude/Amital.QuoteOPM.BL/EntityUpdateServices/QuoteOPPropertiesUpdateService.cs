using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    partial class QuoteOPPropertiesUpdateService
    {
        protected override void OnCreating(QuoteOPPropertiesPM entityPM, QuoteOPPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("QuoteOPProperties", entityParentPM.Tenant).ToString();
            entityPM.QuoteID = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;            
            entityPM.IndexOrder = CodeCounter.GetNumber("QuoteOPProperties", entityParentPM.Tenant);


            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
