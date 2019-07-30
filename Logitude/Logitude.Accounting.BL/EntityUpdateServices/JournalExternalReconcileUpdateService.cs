using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    
        public partial class JournalExternalReconcileUpdateService : EntityUpdateService<JournalExternalReconcile, JournalExternalReconcilePM, JournalPM>
    {
        protected override void OnUpdating(JournalExternalReconcilePM entityPM)
        {
            switch (entityPM.ChangeSetOp)
            {
                case ChangeSetOperation.None:
                case ChangeSetOperation.Insert:
                    break;
                case ChangeSetOperation.Update:
                case ChangeSetOperation.Delete:
                default:
                    throw new Exception("JournalExternalReconcilePM only insert allowed(so far 20190729 )");
                    break;
            }
            base.OnUpdating(entityPM);
        }
    }
}
