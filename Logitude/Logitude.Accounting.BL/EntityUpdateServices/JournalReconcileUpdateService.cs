
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalReconcileUpdateService : EntityUpdateService<JournalReconcile, JournalReconcilePM, JournalPM>
    {
        protected override void OnUpdating(JournalReconcilePM entityPM)
        {
            switch (entityPM.ChangeSetOp)
            {
                case ChangeSetOperation.None:
                case ChangeSetOperation.Insert:
                    break;
                case ChangeSetOperation.Update:
                case ChangeSetOperation.Delete:
                default:
                    throw new Exception("JournalReconcilePM only insert allowed(so far 20180906 )");
                    break;
            }
            base.OnUpdating(entityPM);
        }
    }
}
