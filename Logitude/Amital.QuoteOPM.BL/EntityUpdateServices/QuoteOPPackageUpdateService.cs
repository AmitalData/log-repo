using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    partial class QuoteOPPackageUpdateService
    {
        protected override void OnCreating(QuoteOPPackagePM entityPM, QuoteOPPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("QuoteOPPackage", entityParentPM.Tenant).ToString();
            entityPM.QuoteOPId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
