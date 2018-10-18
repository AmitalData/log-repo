using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Accounting.BL.CoreBL
{
    class CreateReconciliationService
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
    }
}
