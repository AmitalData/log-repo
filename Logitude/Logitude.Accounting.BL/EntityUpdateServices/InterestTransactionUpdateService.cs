using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class InterestTransactionUpdateService
    {

        protected override void OnCreating(InterestTransactionPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("InterestTransaction", entityPM.Tenant);
            entityPM.CreateDateTime = DateTime.UtcNow;
        }


        protected override void OnUpdating(InterestTransactionPM entityPM)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;
        }

    }
}
