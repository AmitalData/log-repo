using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimImporterDeclarsPage3BUpdateService
    {
        protected override void OnCreating(ClaimImporterDeclarsPage3BPM entityPM, ClaimPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }

            entityPM.ClaimId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            int line = 0;
            if (entityParentPM.ClaimImporterDeclarsPage3B.Count > 0)
            {
                line = entityParentPM.ClaimImporterDeclarsPage3B.Max(d => d.LineNo);
            }
            entityPM.LineNo = line + 1;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
