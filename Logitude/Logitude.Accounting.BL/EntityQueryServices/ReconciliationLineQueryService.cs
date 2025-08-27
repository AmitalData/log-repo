using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ReconciliationLineQueryService : EntityQueryService<ReconciliationLine, ReconciliationLineKeys, ReconciliationLinePM, ReconciliationPM, ReconciliationKeys>
    {
        public bool IsReconciledBy(int tenant, List<string> transactionIdList)
        {
            return this.repository.IsReconciledBy(tenant, transactionIdList);
        }

        public ReconciliationLine GetLineByTransactionId(string transId, int tenant)
        {
            ReconciliationLine recoLine = (from a in context.ReconciliationLines
                                           where a.TransactionId == transId && a.Tenant == tenant
                                           select a).FirstOrDefault();
            //var pm = GetEntityPM(recoLine);
            return recoLine;
        }
        public List<ReconciliationLine> GetLinesByReconciliationIdAndTenant(string reconciliationId, int tenant)
        {
            List<ReconciliationLine> recoLine = (from a in context.ReconciliationLines
                                           where a.ReconciliationId == reconciliationId && a.Tenant == tenant
                                           select a).ToList();
            return recoLine;
        }


        public List<ReconciliationLinePM> GetLinePMsByReconciliationIdAndTenant(string reconciliationId, int tenant)
        {
            return context.ReconciliationLines
                          .Where(a => a.ReconciliationId == reconciliationId && a.Tenant == tenant)
                          .ToList()
                          .Select(poco => GetEntityPM(poco))
                          .ToList();
        }


        public List<ReconciliationLinePM> GetLineByTransactionIds(List<string> transId, int tenant)
        {
            List<ReconciliationLine> recoLines = (from a in context.ReconciliationLines
                                           where transId.Contains(a.TransactionId) && a.Tenant == tenant
                                           select a).ToList();

            List<ReconciliationLinePM> pms = recoLines.Select(poco => GetEntityPM(poco)).ToList();

            return pms;
        }
        public List<ReconciliationLinePM> GetLinesByReconciledWithTransactionId(string transId, int tenant)
        {
            List<ReconciliationLine> recoLines = (from a in context.ReconciliationLines
                                                  where a.ReconciledWithTransactionId == transId && a.Tenant == tenant
                                                  select a).ToList();

            List<ReconciliationLinePM> pms = recoLines.Select(poco => GetEntityPM(poco)).ToList();

            return pms;
        }
        public List<ReconciliationLinePM> GetLinesByReconciledWithTransactionIdWithoutMapping(string transId, int tenant)
        {
            List<ReconciliationLinePM> recoLines = (from recLine in context.ReconciliationLines
                                                    join reco in context.Reconciliations on recLine.ReconciliationId equals reco.Id
                                                    where recLine.ReconciledWithTransactionId == transId && recLine.Tenant == tenant

                                                    select new ReconciliationLinePM()
                                                    {
                                                        ReconciliationId = recLine.ReconciliationId,
                                                        TransactionId = recLine.TransactionId,
                                                        CurrencyId = recLine.CurrencyId,
                                                        CurrencyName = recLine.Currency != null ? recLine.Currency.EnglishName : null,
                                                        CurrencyCode = recLine.Currency != null ? recLine.Currency.Code : null,
                                                        Line = recLine.Line,
                                                        IsPartial = recLine.IsPartial,
                                                        Tenant = recLine.Tenant,
                                                        ReconciliationAmount = recLine.ReconciliationAmount,
                                                        ReconciledWithTransactionId = recLine.ReconciledWithTransactionId,

                                                        IsRecoCancelled = reco.IsCancelled,

                                                    }).ToList();


            return recoLines;
        }
        public List<ReconciliationLinePM> GetLinesByTransactionId(string transId, int tenant)
        {
            List<ReconciliationLine> recoLines = (from a in context.ReconciliationLines
                                                  where a.TransactionId == transId && a.Tenant == tenant
                                                  select a).ToList();

            List<ReconciliationLinePM> pms = recoLines.Select(poco => GetEntityPM(poco)).ToList();

            return pms;
        }

        public List<ReconciliationLinePM> GetLinesByTransactionIds(List<string> transId, int tenant)
        {
            List<ReconciliationLine> recoLines = (from a in context.ReconciliationLines
                                                  where transId.Contains(a.TransactionId) && a.Tenant == tenant
                                                  select a).ToList();

            List<ReconciliationLinePM> pms = recoLines.Select(poco => GetEntityPM(poco)).ToList();

            return pms;
        }
        public List<ReconciliationLinePM> GetLinesByTransactionIdsWithoutMapping(List<string> transId, int tenant)
        {
            List<ReconciliationLinePM> recoLines = (from recLine in context.ReconciliationLines
                                                    join reco in context.Reconciliations on recLine.ReconciliationId equals reco.Id
                                                    where transId.Contains(recLine.TransactionId) && recLine.Tenant == tenant

                                                    select new ReconciliationLinePM()
                                                    {
                                                        ReconciliationId = recLine.ReconciliationId,
                                                        TransactionId = recLine.TransactionId,
                                                        CurrencyId = recLine.CurrencyId,
                                                        CurrencyName = recLine.Currency != null ? recLine.Currency.EnglishName : null,
                                                        CurrencyCode = recLine.Currency != null ? recLine.Currency.Code : null,
                                                        Line = recLine.Line,
                                                        IsPartial = recLine.IsPartial,
                                                        Tenant = recLine.Tenant,
                                                        ReconciliationAmount = recLine.ReconciliationAmount,
                                                        ReconciledWithTransactionId = recLine.ReconciledWithTransactionId,
                                                        
                                                        IsRecoCancelled = reco.IsCancelled,

                                                    }).ToList();


            return recoLines;
        }



    }
}
