using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class UpdateOpenReconcileService
    {
        //int TENANT_DYS = 3;

        public void DoAccount(int tenant, List<MMPSDataM> accountRows)
        {

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var ggg = accountRows.GroupBy(r => r.InternalNumber);
                if (ggg.Count() > 1)
                {
                    LogMessagingUtil.Instance.AppendLine($"Error transaction per account !!!");
                    return;
                }

                using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(1)))
                {

                    string myInternalNumber = accountRows.First().InternalNumber;
                    var currentContext = AccountingContext.GetContext(tenant);
                    var glaccount = currentContext.GLAccounts.FirstOrDefault(r => r.InternalNumber == myInternalNumber && r.Tenant == tenant);
                    if (glaccount == null)
                    {
                        LogMessagingUtil.Instance.AppendLine($"Error InternalNumber not exit {myInternalNumber} count {accountRows.Count()}");
                        return;//do not save  & commit ;
                    }
                    var realAmount=RealOpenAmountService.MyList.FirstOrDefault(r => r.Key == glaccount.Id);
                    if (realAmount==null)
                    {
                        LogMessagingUtil.Instance.AppendLine($"Error no totalopenamount in DB {myInternalNumber} count {accountRows.Count()}");
                        return;//do not save  & commit ;

                    }
                    var mumpsOpenAmount=accountRows.Sum(r => r.OpenAmount);
                    if (Math.Abs( realAmount.Tot- mumpsOpenAmount)>0.01m)
                    {
                        LogMessagingUtil.Instance.AppendLine($"Math.Abs( realAmount.Tot- mumpsOpenAmount)>0.01m  {myInternalNumber}");
                        return;//do not save  & commit ;

                    }

                    int saveCounter = 0;
                    foreach (var oneAcount in ggg)
                    {


                      
                        foreach (MMPSDataM row in oneAcount)
                        {
                            var qLedger = currentContext.LedgerTransactions
                                .Where(r => r.Tenant == tenant)
                                .Where(r => r.AccountId == glaccount.Id)
                                .Where(r => r.JournalLineNumber == row.JLineNumber)
                                ;
                            if (!string.IsNullOrWhiteSpace(row.Ref1))
                            {
                                qLedger = qLedger.Where(r => r.Reference1 == row.Ref1);
                            }
                            //if (!string.IsNullOrWhiteSpace(row.Ref2))
                            //{
                            //    qLedger = qLedger.Where(r => r.Reference2 == row.Ref2);
                            //}
                            string ReconcileMethodCode = "";
                            if (glaccount.ReconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString())
                            {
                                ReconcileMethodCode = "LocalCurrency";
                                qLedger = qLedger.Where(r => r.LocalAmountDebit - r.LocalAmountCredit == row.OriginAmount);
                            }
                            else
                            {
                                ReconcileMethodCode = "ForeignCurrency";
                                qLedger = qLedger.Where(r => r.ForeignAmountDebit - r.ForeignAmountCredit == row.OriginAmount);
                            }
                            var qJoin = (from l in qLedger
                                         join j in currentContext.Journals
                                         .Where(r => r.Tenant == tenant)
                                         .Where(r => r.ExternalNo == row.ExternalNo)

                                         on l.JournalId equals j.Id
                                         select l
                                     );
                            var listOf = qJoin.ToList();
                            if (listOf.Count > 1)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error found {listOf.Count} many {row.TheLine} ");
                                return;//do not save  & commit 
                            }
                            if (listOf.Count == 0)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error not found {row.TheLine}  ReconcileMethodCode ={ReconcileMethodCode} ");
                                return;//do not save  & commit 
                            }
                            var theLedger = listOf.First();
                            if (!theLedger.IsReconciled)
                            {
                                LogMessagingUtil.Instance.AppendLine($"Error !IsReconciled {row.TheLine} ");
                                return;//do not save  & commit 
                            }
                            saveCounter++;
                            theLedger.IsReconciled = false;
                            theLedger.OpenAmount = row.OpenAmount;

                        }
                    }

                    bool savecommit = true;
                    if (savecommit)
                    {
                        currentContext.SaveChanges();
                        scope.Complete();

                    }
                    LogMessagingUtil.Instance.AppendLine($"submit {savecommit} recourd:{saveCounter}  took {stopwatch.Elapsed}");

                }
            }
            catch (Exception ee)
            {
                bool throwIt = true;
                if (throwIt)
                {
                    throw;
                }
                
            }
            finally
            {

            }
        }

        public void DoAccount100(int TENANT_DYS,List<MMPSDataM> rows)
        {

            var stopwatch = Stopwatch.StartNew();
            try
            {
                using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(2)))
                {


                    var currentContext = AccountingContext.GetContext(TENANT_DYS);
                    int saveCounter = 0;
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
                                qLedger = qLedger.Where(r => r.Reference2 == row.Ref2);
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
                            saveCounter++;
                            theLedger.IsReconciled = false;
                            theLedger.OpenAmount = row.OpenAmount;

                            currentContext.LedgerTransactions.Attach(theLedger);
                            currentContext.SetAsModified(theLedger);
                        }
                    }
                    
                    bool savecommit = false;
                    if (savecommit)
                    {
                        currentContext.SaveChanges();
                        scope.Complete();

                    }
                    LogMessagingUtil.Instance.AppendLine($"submit {savecommit} recourd:{saveCounter}  took {stopwatch.Elapsed}");

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
