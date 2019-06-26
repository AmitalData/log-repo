using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.BLExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reconcile
{
    public class AutoReconcileService : IAutoReconcileServiceExt
    {
        private GLAccountPM _GLAccountBillTO;
        private JournalPM _JournalARPayment;
        private List<AutoReconcileRecord>  _AutoReconcileRecordList;

        public void InitMust(GLAccountPM glAccountBillTO, JournalPM journalARPayment, List<AutoReconcileRecord> AutoReconcileRecordList)
        {
            this._GLAccountBillTO = glAccountBillTO;
            this._JournalARPayment = journalARPayment;
            _AutoReconcileRecordList = AutoReconcileRecordList;//theEntityPm.PaymentInvoices.Select(r => r.ARInvoiceId).ToList();
        }
        


   
        public void InsertJournalReconcile()
        {
           


            List<ReconciliationLinePM> ReconciliationLines = GetReconciliationLines(_AutoReconcileRecordList, _GLAccountBillTO.Id, _JournalARPayment .Tenant);
            
            //journal.JournalLines.GroupBy(r=>r.)
            if (_GLAccountBillTO.ReconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
        ) //Local Currency
            {
                   var sumLocal = _JournalARPayment.JournalLines.Where(r => r.CreditAccountId == _GLAccountBillTO.Id && r.ActionCode == "1").Sum(r => r.LocalAmount); // 1- Credit

                //if (sumLocal != ReconciliationLines.Sum(r => r.ReconciliationAmount))
                //{
                //    throw new Exception("(sumLocal != ReconciliationLines.Sum(r => r.ReconciliationAmount))");
                //}
            }
            else
            {
                string glAccountCurrencyId = _GLAccountBillTO.CurrencyId;
                var sumForeign = _JournalARPayment.JournalLines.Where(r => r.CreditAccountId == _GLAccountBillTO.Id && r.ActionCode == "1").Sum(r => r.ForeignAmount); // 1- Credit
                //if (sumForeign != ReconciliationLines.Sum(r => r.ReconciliationAmount))
                //{
                //    throw new Exception("(sumForeign != ReconciliationLines.Sum(r => r.ReconciliationAmount))");
                //}

                if (ReconciliationLines.Select(r => r.CurrencyId).Distinct().Count() > 1)
                {
                    throw new Exception("only 1 currency");
                }

            }
            var listJournalReconciles = (from item in ReconciliationLines
                                         select new JournalReconcilePM()
                                         {
                                             ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                             Tenant = _JournalARPayment .Tenant,
                                             JournalId = _JournalARPayment .Id,
                                             LedgerTransactionId = item.TransactionId,
                                             Line = item.Line,  // BASEL: COUNTER 
                                             CurrencyId = item.CurrencyId,
                                             ReconciliationAmount = item.ReconciliationAmount,
                                             IsPartial = item.IsPartial,

                                         }
                   );
            _JournalARPayment .JournalReconciles.AddRange(listJournalReconciles);
        }
        private List<ReconciliationLinePM> GetReconciliationLines(List<AutoReconcileRecord> autoReconcileRecordList, string glAccountBillTOId, int tenant)
        {
            if (!autoReconcileRecordList.Any())
            {
                return new List<ReconciliationLinePM>();
            }

            CheckIfAllRowsHaveMinRequiredReference(autoReconcileRecordList);

            FillRowsWithAccountingEntityOnly(autoReconcileRecordList, tenant);



            var myLedgerTransactionRepository = new LedgerTransactionRepository(tenant);

            FillRowsWithJournalId(autoReconcileRecordList, glAccountBillTOId, tenant, myLedgerTransactionRepository);


            if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID)))
            {
                throw new Exception(" why  not all autoReconcileRecordList  have  LedgerTransactionID /  After fetch LedgerTransaction from GetQByJournalIds ");
            }

            List<String> idList = autoReconcileRecordList.Select(r => r.LedgerTransactionID).ToList();
            var ledgerList= myLedgerTransactionRepository.GetLedgerTransactionsByIdList(idList, tenant);
            if (ledgerList.Count() != autoReconcileRecordList.Count())
            {
                throw new Exception(" why  (ledgerList.Count() != autoReconcileRecordList.Count())/  After myLedgerTransactionRepository.GetLedgerTransactionsByIdList");
            }

            var ledgerListNotReconcile =
            ledgerList
            .Where(r => !r.IsReconciled)
            .Where(r => !r.InReconcileProgress)
            .ToList();


            
            if (!ledgerListNotReconcile.Any())
            {
                throw new Exception($"Does have any Ledger/ Already Reconcile!! .GetQByJournalIds({idList}, tenant).Where(r => r.AccountId == {glAccountBillTOId}).Where(r => !r.IsReconciled).Where(r => !r.InReconcileProgress)");
            }

            var lineCounter = 1;
            var myReconciliationLines = new List<ReconciliationLinePM>();

            autoReconcileRecordList.ForEach(
                autoReconcileRecord
                =>
                {
                    var currentLedger = ledgerListNotReconcile.FirstOrDefault(r => r.Id == autoReconcileRecord.LedgerTransactionID);
                    if (currentLedger == null)
                    {
                        throw new Exception($"to {autoReconcileRecord.LedgerTransactionID} no found free ledger !!");
                    }


                    var reconciliationLine = new ReconciliationLinePM()
                    {

                        ChangeSetOp = ChangeSetOperation.Insert,
                        ReconciliationId = "new",
                        Tenant = tenant,
                        Line = lineCounter++,
                        CurrencyId = currentLedger.OpenAmountCurrencyId,
                        TransactionId = currentLedger.Id,
                        //ReconciliationAmount = current.Value, //selectedTransaction.OpenAmount,
                        //IsPartial = (current.Value < currentLedger.OpenAmount)//false//selectedTransaction.IsPartial;
                    };


                    if (_GLAccountBillTO.ReconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
        ) //Local Currency
                    {

                        if (Math.Abs(autoReconcileRecord.LocalAmountToReconcile) > Math.Abs(currentLedger.OpenAmount))
                        {
                            throw new Exception($"to {autoReconcileRecord.LedgerTransactionID} ==>Math.Abs( autoReconcileRecord.LocalAmountToReconcile)> Math.Abs(currentLedger.OpenAmount)!!");
                        }
                        reconciliationLine.ReconciliationAmount = autoReconcileRecord.LocalAmountToReconcile;

                    }
                    else
                    {
                        if (Math.Abs(autoReconcileRecord.ForeignAmountToReconcile) > Math.Abs(currentLedger.OpenAmount))
                        {
                            throw new Exception($"to {autoReconcileRecord.ForeignAmountToReconcile} ==>Math.Abs( autoReconcileRecord.LocalAmountToReconcile)> Math.Abs(currentLedger.OpenAmount)!!");
                        }
                        reconciliationLine.ReconciliationAmount = autoReconcileRecord.ForeignAmountToReconcile;

                    }

                    reconciliationLine.IsPartial = (currentLedger.OpenAmount - reconciliationLine.ReconciliationAmount) != 0;
                    myReconciliationLines.Add(reconciliationLine);


                }
                );


            return myReconciliationLines;

        }

        private static void FillRowsWithJournalId(List<AutoReconcileRecord> autoReconcileRecordList, string glAccountBillTOId, int tenant, LedgerTransactionRepository myLedgerTransactionRepository)
        {
            var rowsWithJournalIdOnly = autoReconcileRecordList
                                        .Where(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && !string.IsNullOrWhiteSpace(r.JournalId))
                                        .ToList();
            if (rowsWithJournalIdOnly.Count > 0)
            {

                var listOfJournalIds = rowsWithJournalIdOnly.Select(r => r.JournalId).ToList();


                var qLedgerOfBilltoByJournalId
                    = myLedgerTransactionRepository
                    .GetQByJournalIds(listOfJournalIds, tenant)
                    .Where(r => r.AccountId == glAccountBillTOId);
                var LedgerOfBilltoByJournalIdList = qLedgerOfBilltoByJournalId.ToList();

                LedgerOfBilltoByJournalIdList.ForEach(
                    l =>
                    {
                        var ListLdgerPerJournal = autoReconcileRecordList.Where(r => r.JournalId == l.JournalId).Select(r => r.LedgerTransactionID).ToList();
                        if (ListLdgerPerJournal.Count > 1)
                        {
                            throw new Exception("I DID NOT PLAN THAT I WILL FOUND FOR 1 JOURNAL MANY LEDGER FOR BILLTO - TODO add zero autoReconcileRecordList record !!!");
                        }

                        var autoReconcileRecord = autoReconcileRecordList.First(r => r.JournalId == l.JournalId);
                        autoReconcileRecord.LedgerTransactionID = l.Id;
                    });
                if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID)))
                {
                    throw new Exception("not all autoReconcileRecordList  have  LedgerTransactionID /  After fetch LedgerTransaction from GetQByJournalIds ");
                }
            }
        }

        private static void FillRowsWithAccountingEntityOnly(List<AutoReconcileRecord> autoReconcileRecordList, int tenant)
        {
            var rowsWithAccountEntityId = autoReconcileRecordList
                            .Where(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId) && !string.IsNullOrWhiteSpace(r.AccountingEntityId))
                            .ToList();
            if (rowsWithAccountEntityId.Count > 0)
            {
                var myJournalRepository = new JournalRepository(tenant);
                var entityIdS = rowsWithAccountEntityId.Select(r => r.AccountingEntityId).ToList();

                var qJournal = myJournalRepository.GetByJournalsAccountingEntityIds(entityIdS, tenant);
                var journalList = qJournal.ToList();
                journalList.ForEach(j =>
                {
                    var autoReconcileRecord = autoReconcileRecordList.First(r => r.AccountingEntityId == j.AccountingEntityId);
                    autoReconcileRecord.JournalId = j.Id;
                });

                if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId)))
                {
                   // throw new Exception("after GetByJournalsAccountingEntityIds  Not All rows have  JournalId ");
                }
            }
        }

        private static void CheckIfAllRowsHaveMinRequiredReference(List<AutoReconcileRecord> autoReconcileRecordList)
        {
            var rowsWithoutRefernce = autoReconcileRecordList
                            .Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId) && string.IsNullOrWhiteSpace(r.AccountingEntityId));
            if (rowsWithoutRefernce)
            {
                throw new Exception("rowsWithoutRefernce -set LedgerTransactionID/JournalId/AccountEntityId");
            }
        }
    }
}
