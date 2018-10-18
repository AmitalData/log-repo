using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial  class ClaimImporterDeclarsPage3AUpdateService
    {
        protected override void OnCreating(ClaimImporterDeclarsPage3APM entityPM, ClaimPM entityParentPM)
        {
            entityPM.ClaimId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            int line = 0;
            if (entityParentPM.ClaimImporterDeclarsPage3A.Count > 0)
            {
                line = entityParentPM.ClaimImporterDeclarsPage3A.Max(d => d.LineNo);
            }
            entityPM.LineNo = line + 1;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
