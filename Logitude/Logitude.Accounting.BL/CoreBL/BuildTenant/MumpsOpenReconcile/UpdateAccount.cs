using Logitude.Accounting.Data;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class UpdateAccount
    {
        public void DoAccount100(List<MMPSDataM> rows)
        {
            foreach(var gg in   rows.GroupBy(r => r.InternalNumber))
            {

                var currentContext = AccountingContext.GetContext(tenant);
                var glaccount=currentContext.GLAccounts.FirstOrDefault(r => r.InternalNumber == gg.Key);
                if (glaccount==null)
                {
                    LogMessagingUtil.Instance.AppendLine($"Error InternalNumber not exit {gg.Key} count {gg.Count()}");
                    continue;
                }
                glaccount.ReconcileMethodCode
                //gg.Key 

            }
        }
    }
}
