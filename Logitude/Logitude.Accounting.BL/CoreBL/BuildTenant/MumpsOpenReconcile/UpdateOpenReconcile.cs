using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class UpdateOpenReconcile
    {
        //int TENANT_DYS = 3;
        public void DoAccount100(int TENANT_DYS,List<MMPSDataM> rows)
        {
            try
            {
                using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(2)))
                {


                    var currentContext = AccountingContext.GetContext(TENANT_DYS);

                    foreach (var gg in rows.GroupBy(r => r.InternalNumber))
                    {


                        var glaccount = currentContext.GLAccounts.FirstOrDefault(r => r.InternalNumber == gg.Key && r.Tenant == TENANT_DYS);
                        if (glaccount == null)
                        {
                            LogMessagingUtil.Instance.AppendLine($"Error InternalNumber not exit {gg.Key} count {gg.Count()}");
                            continue;
                        }

                        foreach (MMPSDataM row in gg)
                        {
                            var qLedger = currentContext.LedgerTransactions
                                .Where(r => r.Tenant == TENANT_DYS)
                                .Where(r => r.AccountId == glaccount.Id)
                                ;
                            if (!string.IsNullOrWhiteSpace(row.Ref1))
                            {
                                qLedger = qLedger.Where(r => r.Reference1 == row.Ref1);
                            }
                            if (!string.IsNullOrWhiteSpace(row.Ref2))
                            {
                                qLedger = qLedger.Where(r => r.Reference1 == row.Ref2);
                            }

                            if (glaccount.ReconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString())
                            {
                                qLedger = qLedger.Where(r => r.LocalAmountDebit - r.LocalAmountCredit == row.OriginAmount);
                            }
                            else
                            {
                                qLedger = qLedger.Where(r => r.ForeignAmountDebit - r.ForeignAmountCredit == row.OriginAmount);
                            }
                            var qJoin = (from l in qLedger
                                         join j in currentContext.Journals
                                         .Where(r => r.Tenant == TENANT_DYS)
                                         .Where(r => r.ExternalNo == row.ExternalNo)
                                         on l.JournalId equals j.Id
                                         select l
                                     );
                            var listOf = qJoin.ToList();
                            if (listOf.Count > 1)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error found {listOf.Count} many {row.TheLine} ");
                                continue;
                            }
                            if (listOf.Count == 0)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error not found {row.TheLine} ");
                                continue;
                            }
                            var theLedger = listOf.First();
                            if (!theLedger.IsReconciled)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error !IsReconciled {row.TheLine} ");
                                continue;
                            }
                            theLedger.IsReconciled = false;
                            theLedger.OpenAmount = row.OpenAmount;

                            currentContext.LedgerTransactions.Attach(theLedger);
                            currentContext.SetAsModified(theLedger);
                        }
                        currentContext.SaveChanges();
                        scope.Complete();
                    }

                }
            }
            catch (Exception ee)
            {

                throw;
            }
            finally
            {

            }
        }
    }
}
