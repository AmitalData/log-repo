using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.BLExt;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL.Reconcile
{
    public class AutoReconcileService : IAutoReconcileServiceExt
    {
        public GLAccountPM _GLAccountBillTO;
        private JournalPM _JournalARPayment;
        private List<AutoReconcileRecord>  _AutoReconcileRecordList;
        private string _accountingEntityCode;
        private string _paymentCurrencyId;
        private string _reconcileMethodCode;

        public void InitMust(GLAccountPM glAccountBillTO, JournalPM journalARPayment, List<AutoReconcileRecord> AutoReconcileRecordList, string accountingEntityCode, string paymentCurrencyId)
        {
            this._GLAccountBillTO = glAccountBillTO;
            this._JournalARPayment = journalARPayment;
            _AutoReconcileRecordList = AutoReconcileRecordList;
            _accountingEntityCode = accountingEntityCode;
            _paymentCurrencyId= paymentCurrencyId;
            _reconcileMethodCode = (bool)glAccountBillTO.IsMultiCurrency ? GetReconcileMethodCode(glAccountBillTO, paymentCurrencyId) : glAccountBillTO.ReconcileMethodCode;
        }
        
       public string GetReconcileMethodCode(GLAccountPM glAccountBillTO, string paymentCurrencyId)
        {
            GLAccountCurrencyQueryService gLAccountCurrencyQuery = new GLAccountCurrencyQueryService(glAccountBillTO.Tenant);
            var result = gLAccountCurrencyQuery.GetReconcileMethodCodeByCurrencyAndGLAccountId(glAccountBillTO.Id, paymentCurrencyId, glAccountBillTO.Tenant);
            return string.IsNullOrEmpty(result) ? glAccountBillTO.ReconcileMethodCode : result;
        }

   
        public void InsertJournalReconcile()
        {
           


            List<ReconciliationLinePM> ReconciliationLines = GetReconciliationLines(_AutoReconcileRecordList, _GLAccountBillTO.Id, _JournalARPayment .Tenant,_GLAccountBillTO.IsMultiCurrency, _paymentCurrencyId);
            
           
            if (_reconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
        ) 
            {
                   var sumLocal = _JournalARPayment.JournalLines.Where(r => r.CreditAccountId == _GLAccountBillTO.Id && r.ActionCode == "1").Sum(r => r.LocalAmount); // 1- Credit
                              
            }
            else
            {
                string glAccountCurrencyId = _GLAccountBillTO.CurrencyId;
                var sumForeign = _JournalARPayment.JournalLines.Where(r => r.CreditAccountId == _GLAccountBillTO.Id && r.ActionCode == "1").Sum(r => r.ForeignAmount); // 1- Credit
              
                if (ReconciliationLines.Select(r => r.CurrencyId).Distinct().Count() > 1)
                {
                    throw new ApplicationException("only 1 currency");
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
        private List<ReconciliationLinePM> GetReconciliationLines(List<AutoReconcileRecord> autoReconcileRecordList, string glAccountBillTOId, int tenant,bool? isMultiCurrency,string currencyId)
        {
            if (!autoReconcileRecordList.Any())
            {
                return new List<ReconciliationLinePM>();
            }

            CheckIfAllRowsHaveMinRequiredReference(autoReconcileRecordList);

            FillRowsWithAccountingEntityOnly(autoReconcileRecordList, tenant, _accountingEntityCode);



            var myLedgerTransactionRepository = new LedgerTransactionRepository(tenant);

            FillRowsWithJournalId(autoReconcileRecordList, glAccountBillTOId, tenant, myLedgerTransactionRepository, isMultiCurrency, currencyId);


            if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID)))
            {
                throw new ApplicationException(" why  not all autoReconcileRecordList  have  LedgerTransactionID /  After fetch LedgerTransaction from GetQByJournalIds ");
            }

            List<String> idList = autoReconcileRecordList.Select(r => r.LedgerTransactionID).ToList();
            var ledgerList= myLedgerTransactionRepository.GetLedgerTransactionsByIdList(idList, tenant);
            if (ledgerList.Count() != autoReconcileRecordList.Count())
            {
                throw new ApplicationException(" why  (ledgerList.Count() != autoReconcileRecordList.Count())/  After myLedgerTransactionRepository.GetLedgerTransactionsByIdList");
            }

            var ledgerListNotReconcile =
            ledgerList
            .Where(r => !r.IsReconciled)
            .Where(r => !r.InReconcileProgress)
            .ToList();



            var lineCounter = 1;
            var myReconciliationLines = new List<ReconciliationLinePM>();

            autoReconcileRecordList.ForEach(
                autoReconcileRecord
                =>
                {
                    var currentLedger = ledgerListNotReconcile.FirstOrDefault(r => r.Id == autoReconcileRecord.LedgerTransactionID);
                    if (currentLedger == null)
                    {
                        throw new ApplicationException($"to {autoReconcileRecord.LedgerTransactionID} no found free ledger !!");
                    }


                    var reconciliationLine = new ReconciliationLinePM()
                    {

                        ChangeSetOp = ChangeSetOperation.Insert,
                        ReconciliationId = "new",
                        Tenant = tenant,
                        Line = lineCounter++,
                        CurrencyId = currentLedger.OpenAmountCurrencyId,
                        TransactionId = currentLedger.Id,
                        CurrencyRate = currentLedger.ExchangeRate,

                    };


                    if (_reconcileMethodCode == ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()) 
                    {

                        if (Math.Abs(autoReconcileRecord.LocalAmountToReconcile) > Math.Abs(currentLedger.OpenAmount))
                        {
                            throw new ApplicationException($"to {autoReconcileRecord.LedgerTransactionID} ==>Math.Abs( autoReconcileRecord.LocalAmountToReconcile)> Math.Abs(currentLedger.OpenAmount)!!");
                        }
                        reconciliationLine.ReconciliationAmount = autoReconcileRecord.LocalAmountToReconcile;

                    }
                    else
                    {
                        if (Math.Abs(autoReconcileRecord.ForeignAmountToReconcile) > Math.Abs(currentLedger.OpenAmount))
                        {
                            throw new ApplicationException($"to {autoReconcileRecord.ForeignAmountToReconcile} ==>Math.Abs( autoReconcileRecord.LocalAmountToReconcile)> Math.Abs(currentLedger.OpenAmount)!!");
                        }
                        reconciliationLine.ReconciliationAmount = autoReconcileRecord.ForeignAmountToReconcile;

                    }

                    reconciliationLine.IsPartial = (currentLedger.OpenAmount - reconciliationLine.ReconciliationAmount) != 0;
                    myReconciliationLines.Add(reconciliationLine);


                }
                );


            return myReconciliationLines;

        }

        private static void FillRowsWithJournalId(List<AutoReconcileRecord> autoReconcileRecordList, string glAccountBillTOId, int tenant, LedgerTransactionRepository myLedgerTransactionRepository,bool? isMultiCurrency,string currencyId)
        {
            var rowsWithJournalIdOnly = autoReconcileRecordList
                                        .Where(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && !string.IsNullOrWhiteSpace(r.JournalId))
                                        .ToList();
            if (rowsWithJournalIdOnly.Count > 0)
            {
                List<string> listOfGlaccountIds = new List<string>();
                var listOfJournalIds = rowsWithJournalIdOnly.Select(r => r.JournalId).ToList();
                if ((bool)isMultiCurrency)
                {
                    GLAccountCurrencyQueryService gLAccountCurrencyQuery = new GLAccountCurrencyQueryService(tenant);
                    GLAccountCurrencyPM gLAccountCurrency = gLAccountCurrencyQuery.GetEntityByCurrencyAndGLAccountId(glAccountBillTOId, currencyId, tenant);
                    listOfGlaccountIds.Add(gLAccountCurrency?.GLAccountId);
                }
                listOfGlaccountIds.Add(glAccountBillTOId);


                var qLedgerOfBilltoByJournalId
                    = myLedgerTransactionRepository
                    .GetQByJournalIds(listOfJournalIds, tenant)
                    .Where(r => listOfGlaccountIds.Contains(r.AccountId));
                var LedgerOfBilltoByJournalIdList = qLedgerOfBilltoByJournalId.ToList();
                if (!LedgerOfBilltoByJournalIdList.Any())
                {
                    throw new ApplicationException("could not found any ledger-to the source journal  with the ARPayment  BillTo ");
                }
                foreach(var l in LedgerOfBilltoByJournalIdList)
                {
                    var ListLdgerPerJournal = autoReconcileRecordList.Where(r => r.JournalId == l.JournalId).Select(r => r.LedgerTransactionID).ToList();
                    if (ListLdgerPerJournal.Count > 1)
                    {
                        throw new ApplicationException("I DID NOT PLAN THAT I WILL FOUND FOR 1 JOURNAL MANY LEDGER FOR BILLTO - TODO add zero autoReconcileRecordList record !!!");
                    }

                    var autoReconcileRecord = autoReconcileRecordList.First(r => r.JournalId == l.JournalId);
                    autoReconcileRecord.LedgerTransactionID = l.Id;
                    foreach (AutoReconcileRecord record in autoReconcileRecordList)
                    {
                        if (record.JournalId == l.JournalId)
                        {
                            record.LedgerTransactionID = l.Id;

                        }
                    }
                }

                //LedgerOfBilltoByJournalIdList.ForEach(
                //    l =>
                //    {
                //        var ListLdgerPerJournal = autoReconcileRecordList.Where(r => r.JournalId == l.JournalId).Select(r => r.LedgerTransactionID).ToList();
                //        if (ListLdgerPerJournal.Count > 1)
                //        {
                //            throw new ApplicationException("I DID NOT PLAN THAT I WILL FOUND FOR 1 JOURNAL MANY LEDGER FOR BILLTO - TODO add zero autoReconcileRecordList record !!!");
                //        }

                //        var autoReconcileRecord = autoReconcileRecordList.First(r => r.JournalId == l.JournalId);
                //        autoReconcileRecord.LedgerTransactionID = l.Id;
                //        foreach(AutoReconcileRecord record in autoReconcileRecordList)
                //        {
                //            if (record.JournalId == l.JournalId)
                //            {
                //                record.LedgerTransactionID = l.Id;
                             
                //            }
                //        }
                        
                //    });
                if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID)))
                {
                    throw new ApplicationException("not all autoReconcileRecordList  have  LedgerTransactionID /  After fetch LedgerTransaction from GetQByJournalIds ");
                }
            }
        }

        private static void FillRowsWithAccountingEntityOnly(List<AutoReconcileRecord> autoReconcileRecordList, int tenant, string accountingEntityCode)
        {
            var rowsWithAccountEntityId = autoReconcileRecordList
                            .Where(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId) && !string.IsNullOrWhiteSpace(r.AccountingEntityId))
                            .ToList();
            if (rowsWithAccountEntityId.Count > 0)
            {
                var myJournalRepository = new JournalRepository(tenant);
                var entityIdS = rowsWithAccountEntityId.Select(r => r.AccountingEntityId).ToList();

                var qJournal = myJournalRepository.GetByJournalsAccountingEntityIds(entityIdS, tenant, accountingEntityCode);
                var journalList = qJournal.ToList();
                journalList.ForEach(j =>
                {
                    var autoReconcileRecord = autoReconcileRecordList.First(r => r.AccountingEntityId == j.AccountingEntityId);
                    autoReconcileRecord.JournalId = j.Id;
                });

                if (autoReconcileRecordList.Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId)))
                {
                   // throw new ApplicationException("after GetByJournalsAccountingEntityIds  Not All rows have  JournalId ");
                }
            }
        }

        private static void CheckIfAllRowsHaveMinRequiredReference(List<AutoReconcileRecord> autoReconcileRecordList)
        {
            var rowsWithoutRefernce = autoReconcileRecordList
                            .Any(r => string.IsNullOrWhiteSpace(r.LedgerTransactionID) && string.IsNullOrWhiteSpace(r.JournalId) && string.IsNullOrWhiteSpace(r.AccountingEntityId));
            if (rowsWithoutRefernce)
            {
                throw new ApplicationException("rowsWithoutRefernce -set LedgerTransactionID/JournalId/AccountEntityId");
            }
        }
    }
}
