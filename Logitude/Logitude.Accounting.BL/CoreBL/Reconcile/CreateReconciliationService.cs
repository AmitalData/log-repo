using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Accounting.BL.CoreBL
{
    public class CreateReconciliationService
    {
        public ReconciliationPM GetReconciliation(List<LedgerTransactionPM> myMatchLedgerTransactionList)
        {
            LedgerTransactionPM firstMatch = myMatchLedgerTransactionList[0];
            var myReconciliationPM = new ReconciliationPM();
            myReconciliationPM.Id = "new";
            myReconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            myReconciliationPM.Tenant = firstMatch.Tenant;
            myReconciliationPM.AccountId = firstMatch.AccountId;
            myReconciliationPM.Number = "get";
            myReconciliationPM.CreateDate = DateTime.UtcNow;
            myReconciliationPM.CreatedByUserId = null;
            myReconciliationPM.CreatedByUserId = null;

            myReconciliationPM.ReconciliationLines = new List<ReconciliationLinePM>();

            for (var i = 0; i < myMatchLedgerTransactionList.Count; i++)
            {
                var currLedgerTrans = myMatchLedgerTransactionList[i];
                var myReconciliationLinePM = new ReconciliationLinePM();
                myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
                myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
                myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
                myReconciliationLinePM.Line = i;
                myReconciliationLinePM.CurrencyId = currLedgerTrans.OpenAmountCurrencyId;
                myReconciliationLinePM.TransactionId = currLedgerTrans.Id;
                myReconciliationLinePM.ReconciliationAmount = //currLedgerTrans.OpenAmount;
                            currLedgerTrans.AmountToReconcile;

                //ReconciliationLinePM.IsPartial = currLedgerTrans.OpenAmount;
                myReconciliationLinePM.GroupNumber = currLedgerTrans.GroupMatch;

                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);
            }

            return myReconciliationPM;

        }

        public RecoCallback CreateReconciliation(ReconciliationPM reconciliationPM)
        {
            var accountingContext = AccountingContext.GetContext(reconciliationPM.Tenant);
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), reconciliationPM.Tenant);

            // changeset
            if (reconciliationPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                throw new Exception("Meanwhile Only Insert Enable ");
            }
            reconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            foreach (var item in reconciliationPM.ReconciliationLines)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
            }

            //multi-group split
            int groupsCount = reconciliationPM.ReconciliationLines.GroupBy(d => d.GroupNumber).Count();
            if (groupsCount > 1)
            {
                List<ReconciliationPM> recoPMs = SplitReconciliationByGroup(reconciliationPM);
                foreach (ReconciliationPM recoPM in recoPMs)
                {
                    service.Update(recoPM, true);
                }

                return new RecoCallback() { isSplitted = true, splittedRecoCount = recoPMs.Count };
            }
            else
            {
                service.Update(reconciliationPM, true);
            }

            return new RecoCallback(reconciliationPM);
        }

        List<ReconciliationPM> SplitReconciliationByGroup(ReconciliationPM originalRecoPM)
        {
            List<ReconciliationPM> recoPMs = new List<ReconciliationPM>();

            List<IGrouping<int, ReconciliationLinePM>> groups = originalRecoPM.ReconciliationLines.GroupBy(d => d.GroupNumber).ToList();

            foreach (IGrouping<int, ReconciliationLinePM> group in groups)
            {
                //header
                ReconciliationPM recoPM = new ReconciliationPM()
                {
                    Tenant = originalRecoPM.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,

                    AccountId = originalRecoPM.AccountId,
                    CreatedByUserId = originalRecoPM.CreatedByUserId,
                    Number = originalRecoPM.Number,
                    CreateDate = originalRecoPM.CreateDate,
                    SearchFields = originalRecoPM.SearchFields,
                    IsCancelled = originalRecoPM.IsCancelled,
                    CurrencyCode = originalRecoPM.CurrencyCode,
                    AccountName = originalRecoPM.AccountName,
                    AccountNumber = originalRecoPM.AccountNumber,
                    CreatedByUserName = originalRecoPM.CreatedByUserName,
                };

                //lines
                List<ReconciliationLinePM> groupLines = group.ToList();
                foreach (ReconciliationLinePM line in groupLines)
                {
                    line.ChangeSetOp = ChangeSetOperation.Insert;
                    recoPM.ReconciliationLines.Add(line);
                }

                //add to list
                recoPMs.Add(recoPM);
            }

            return recoPMs;
        }



    }

    public class RecoCallback
    {
        public RecoCallback(ReconciliationPM reco = null)
        {
            if (reco != null)
                reconciliationPM = reco;
        }


        public ReconciliationPM reconciliationPM;
        //public List<ReconciliationPM> splittedRecoPMs;

        public bool isSplitted = false;
        public int splittedRecoCount = 0;
    }
}
