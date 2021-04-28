using Logitude.Accounting.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class RealOpenAmountService
    {
        public static List<RealOpenAmountM> MyList { get; private set; }

        public static void Get(int tenant)
        {
            var currentContext = AccountingContext.GetContext(tenant);


            //var q = (from l in currentContext.LedgerTransactions.Where(r => r.Tenant == tenant)
            //         join a in currentContext.GLAccounts
            //         .Where(r => r.Tenant == tenant)
            //         .Where(a => a.AccountTypeCode == "2" || a.AccountTypeCode == "3")

            //         on l.AccountId equals a.Id

            //         select l.Id);
           
            var qLocalCurrency = (
                     from a in currentContext.GLAccounts
                     .Where( r=>r.ReconcileMethodCode =="0")
                     .Where(r => r.Tenant == tenant)
                     .Where(a => a.AccountTypeCode == "2" || a.AccountTypeCode == "3" || a.AccountTypeCode == "1")
                     select a.Id);

            var qGLocalCurrency = currentContext.LedgerTransactions
                .Where(r => r.Tenant == tenant).Where( r=> qLocalCurrency.Contains(r.AccountId))
                .GroupBy(r=>r.AccountId)
                .Select(lGroup =>  new RealOpenAmountM { Key = lGroup.Key, Tot = lGroup.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit) }
                    );
            MyList = qGLocalCurrency.ToList();

            var qForeignCurrency = (
                 from a in currentContext.GLAccounts
                 .Where(r => r.ReconcileMethodCode == "1")
                 .Where(r => r.Tenant == tenant)
                 .Where(a => a.AccountTypeCode == "2" || a.AccountTypeCode == "3" || a.AccountTypeCode == "1")
                 select a.Id);

            var qGForeignCurrency = currentContext.LedgerTransactions
                .Where(r => r.Tenant == tenant).Where(r => qForeignCurrency.Contains(r.AccountId))
                .GroupBy(r => r.AccountId)
                .Select(lGroup => new RealOpenAmountM { Key = lGroup.Key, Tot = lGroup.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit) }
                    );
            var foreign = qGForeignCurrency.ToList();
            MyList.AddRange(foreign);
        }
    }
    public class RealOpenAmountM
    {
        public string Key { get; internal set; }
        public decimal Tot { get; internal set; }
    }
}
