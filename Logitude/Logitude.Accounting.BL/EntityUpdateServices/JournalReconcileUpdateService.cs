
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
using Newtonsoft.Json;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalReconcileUpdateService : EntityUpdateService<JournalReconcile, JournalReconcilePM, JournalPM>
    {
        protected override void OnUpdating(JournalReconcilePM entityPM)
        {
            string[] stacklines = GetStack(0);

            string logtext = "JournalReconcileUpdateService.OnUpdating(), Point 1, JournalReconcilePM " + entityPM.JournalId + ", T=" + entityPM.Tenant.ToString();

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(JsonConvert.SerializeObject(stacklines));

            switch (entityPM.ChangeSetOp)
            {
                case ChangeSetOperation.None:
                case ChangeSetOperation.Insert:
                case ChangeSetOperation.Delete:
                    break;
                case ChangeSetOperation.Update:
                
                default:
                    throw new ApplicationException(@"JournalReconcilePM only insert allowed(so far 20180906 )
from 20210630 delet also allowed ");
                    break;
            }
            base.OnUpdating(entityPM);
        }

        private static string[] GetStack(int removeLines)

        {

            string[] stack = Environment.StackTrace.Split(

                new string[] { Environment.NewLine },

                StringSplitOptions.RemoveEmptyEntries);

            if (stack.Length <= removeLines)

                return new string[0];

            string[] actualResult = new string[stack.Length - removeLines];

            for (int i = removeLines; i < stack.Length; i++)

                // Remove 6 characters (e.g. "  at ") from the beginning of the line

                // This might be different for other languages and platforms

                actualResult[i - removeLines] = stack[i].Substring(6);

            return actualResult;

        }

    }
}
