using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalTypeUpdateService : EntityUpdateService<JournalType, JournalTypePM, EntityPM>
    {
        protected override void OnCreating(JournalTypePM entityPM, EntityPM entityParentPM)
        {
            // entityPM.Id = IdCounter.GetNumber("Accounting.JournalActionType", entityPM.Tenant);
        }
    }
}
