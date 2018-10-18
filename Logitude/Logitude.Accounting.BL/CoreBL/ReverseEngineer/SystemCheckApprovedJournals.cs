using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ReverseEngineer
{
    public class SystemCheckApprovedJournals
    {
    

        public List<string> StartCheck(int tenant, DateTime? seedDate, string JournalId)
        {
            if (!string.IsNullOrWhiteSpace(JournalId))
            {
                return GetJournalUnbalancedPerPeriod(tenant, JournalId, DateTime.MinValue, DateTime.MaxValue);
            }
            else if (seedDate.HasValue)
            {
                
                var start = new DateTime(seedDate.GetValueOrDefault().Date.Year, seedDate.GetValueOrDefault().Date.Month, 1);
                var end = start.AddMonths(1).AddMinutes(-1);
                return GetJournalUnbalancedPerPeriod(tenant, JournalId, start, end);
            }
            //else
            
                var repoJournalLine = new JournalLineRepository(tenant);
                var j1st = repoJournalLine.GetAll(tenant).OrderBy( r=>r.AccountingDate).FirstOrDefault();
                var jLast = repoJournalLine.GetAll(tenant).OrderByDescending(r => r.AccountingDate).FirstOrDefault();
                if (j1st == null)
                {
                    return new List<string>();
                }


            var yy1st = j1st.AccountingDate.Date.Year;
            var yyLast= jLast.AccountingDate.Date.Year;
            while(yy1st<= yyLast)
            {
                for (int mm = 1; mm <= 12; mm++)
                {
                    
                    var start = new DateTime(yy1st, mm, 1);
                    var end = start.AddMonths(1).AddMinutes(-1);
                    var BadJournalLinePerPeriod =GetJournalUnbalancedPerPeriod(tenant, JournalId, start, end);
                    if (BadJournalLinePerPeriod.Count > 0)
                    {
                        return BadJournalLinePerPeriod;
                    }
                }
                yy1st++;
            }
            return new List<string>();


        }

        private static List<string> GetJournalUnbalancedPerPeriod(int tenant, string JournalId, DateTime start, DateTime end)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {

                var qsJournalLine = new JournalLineQueryService(tenant);
                var qJLAll = qsJournalLine.GetJournalLineAsLedgerTransaction(start, end, tenant
                    , JournalId);



                if (!string.IsNullOrWhiteSpace(JournalId))
                {
                    qJLAll = qJLAll.Where(r => r.JournalId == JournalId);
                }


                var badJournalsQ =
                //qJLAll
                //.GroupBy(r => r.JournalId)
                //.Where(g => g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit) != 0)
                //.SelectMany(r => r);


                qJLAll
                    .GroupBy(r => r.JournalId)
                    .Select(g => new { g.Key, LocalAmountDebit = g.Sum(r => r.LocalAmountDebit), LocalAmountCredit = g.Sum(r => r.LocalAmountCredit) })

                    .Where(r => r.LocalAmountDebit != r.LocalAmountCredit)
                    ;
                badJournalsQ =
                    badJournalsQ
                    .Where(r => Math.Abs(r.LocalAmountDebit - r.LocalAmountCredit) > 0.001)//fuck the round !!!!(for VAT !!!)
                    ;

                var badJournalsTotalsAnynomouse = badJournalsQ.ToList();
                string specifier = "0,0.000";
                var badJournalsTotals =
                    badJournalsTotalsAnynomouse
                    .Select(r =>
                    //"Journal=" + r.Key + ",TotLocalAmountDebit=" + r.LocalAmountDebit.ToString(specifier) + ",TotLocalAmountCredit=" + r.LocalAmountCredit.ToString(specifier)
                    $"Journal={r.Key},TotLocalAmountDebit={r.LocalAmountDebit:00.000},TotLocalAmountCredit={r.LocalAmountCredit:00.000}"

                    );




                //badJournalsQ =
                //qJLAll
                //   .GroupBy(r => r.JournalId)
                //   .Select(g => new { g.Key, tot = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit) })
                //   .Select(r => r.Key + ":" + r.tot.ToString());

                var badJournals = badJournalsTotals.ToList();
                ;

                //badJournals = badJournals
                //    .Where(jlG => Math.Abs(jlG.LocalAmountDebit - jlG.LocalAmountCredit) > 0.001)//fuck the round !!!!(for VAT !!!)
                //    .ToList();
                return badJournals;

            }
        }
    }
}
