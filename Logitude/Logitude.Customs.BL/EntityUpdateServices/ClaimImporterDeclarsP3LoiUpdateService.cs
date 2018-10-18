using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimImporterDeclarsP3LoiUpdateService
    {
        protected override void OnCreating(ClaimImporterDeclarsP3LoiPM entityPM, ClaimImporterDeclarsPage3PM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.ClaimId = entityParentPM.ClaimId;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.CounterKey = entityParentPM.LineNo;

            if (entityPM.LineNo == 0)
            {
                int line = 0;
                if (entityParentPM.ClaimImporterDeclarsP3Loi.Count > 0)
                {
                    line = entityParentPM.ClaimImporterDeclarsP3Loi.Max(d => d.LineNo);
                }
                entityPM.LineNo = line + 1;
            }

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
