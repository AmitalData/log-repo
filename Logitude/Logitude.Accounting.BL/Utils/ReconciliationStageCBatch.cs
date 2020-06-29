using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using System.Text.RegularExpressions;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;

namespace Logitude.Accounting.BL.Utils
{
    public class ReconciliationStageCBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _NoLines;
        private List<string> _WrongAction;
        //private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;
        long _counter = 0;
        public const int LT_LinesMaximum_MIN = 5;
        public const int LT_LinesMaximum_MAX = 20;

        public ReconciliationStageCBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunReconciliationStageC(ReconciliationStageCArg reconciliationStageCArg)
        {
            try
            {
                DateTime fromDate = DateTime.MinValue;
                DateTime oldDate = DateTime.MinValue;
                int tenant = reconciliationStageCArg.Tenant;
                string myGLAccountId = reconciliationStageCArg.GLAccountId;
                DateTime myUpToAccountingDate = reconciliationStageCArg.UpToAccountingDate;
                BatchTaskExecutionPM batchTaskExecutionPM = reconciliationStageCArg.BatchTask;
                BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                if (batchTaskExecutionPM != null)
                {
                    batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                }

                IAccountingContext context = AccountingContext.GetContext(tenant);
                LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(context);
                if (String.IsNullOrEmpty(myGLAccountId))
                {
                    GetAllAccountArgs getAllAccountArgs = new GetAllAccountArgs
                    {
                        Tenant = tenant,
                        AccountTypeCode = reconciliationStageCArg.AccountTypeCode,
                        FromDate = fromDate,
                        UpToAccountingDate = reconciliationStageCArg.UpToAccountingDate,
                    };

                    var gLAccountIdList = ledgerTransactionListQueryService.GetGLAccountIdList__NotReconciled(getAllAccountArgs);

                    if (gLAccountIdList != null && gLAccountIdList.Count > 0)
                    {
                        gLAccountIdList.ForEach(accId =>
                        {
                            ReconciliationStageCArg innerArgs = reconciliationStageCArg;
                            innerArgs.GLAccountId = accId;
                            RunReconciliationStageC_OneAccount(innerArgs);
                        });
                    }
                }
                else
                {
                    RunReconciliationStageC_OneAccount(reconciliationStageCArg);
                }


            }

            catch (Exception e)
            {
                throw new Exception($"ReconciliationStageCBatch failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }


        private void RunReconciliationStageC_OneAccount(ReconciliationStageCArg reconciliationStageCArg)
        {
            try
            {
                DateTime fromDate = DateTime.MinValue;
                string fromId = "";
                DateTime oldDate = DateTime.MinValue;
                string oldId = "";
                bool runAgain = false;
                int tenant = reconciliationStageCArg.Tenant;
                string myGLAccountId = reconciliationStageCArg.GLAccountId;
                DateTime myUpToAccountingDate = reconciliationStageCArg.UpToAccountingDate;

                if (String.IsNullOrWhiteSpace(myGLAccountId))
                {
                    throw new Exception($"GLAccountId is missing");
                }
                else
                {
                    bool toContinue = true;
                    do
                    {
                        GetNextGroupArgs getNextGroupArgs = new GetNextGroupArgs
                        {
                            Tenant = tenant,
                            GLAccountId = myGLAccountId,
                            MIN = LT_LinesMaximum_MIN,
                            LT_LinesMaximum = reconciliationStageCArg.LT_LinesMaximum > LT_LinesMaximum_MAX ? LT_LinesMaximum_MAX : reconciliationStageCArg.LT_LinesMaximum,
                            RunAgain = runAgain,
                            OldDate = oldDate,
                            OldId = oldId,
                            FromDate = fromDate,
                            FromId = fromId,
                            MaximalDifference = reconciliationStageCArg.MaximalDifference,
                            UpToAccountingDate = reconciliationStageCArg.UpToAccountingDate,
                        };
                        List<LedgerTransaction> reconciableLT_List = GetNextReconciableLT_List(ref getNextGroupArgs);
                        if (reconciableLT_List == null || (reconciableLT_List.Count == 0 && getNextGroupArgs.OldDate == DateTime.MinValue))
                        {
                            toContinue = false;
                        }
                        if (reconciableLT_List.Count == 0)
                        {
                            fromDate = getNextGroupArgs.OldDate;
                            fromId = getNextGroupArgs.OldId;
                            runAgain = false;
                            oldDate = DateTime.MinValue;
                            oldId = "0";
                        }
                        else
                        {
                            ProcessOneReconciableLT_List(tenant, myGLAccountId, reconciableLT_List, reconciliationStageCArg.MaximalDifference);
                        }
                    } while (toContinue);

                }

            }

            catch (Exception e)
            {
                throw e;
            }
        }
        private void ProcessOneReconciableLT_List(int tenant, string myGLAccountId, List<LedgerTransaction> reconciableLT_List, decimal maximalDifference)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);
                IQueryable<JournalLineLedgerTransactionAccDTO> journalLine_LT_DTOs;
                journalLine_LT_DTOs = journalLineQueryService.GetQJournalLinesByLTList(tenant, reconciableLT_List);

                List<JournalLineLedgerTransactionAccDTO> journalLine_LT_DTOsList = journalLine_LT_DTOs.ToList().OrderBy(l => l.AccId).ToList();
                var journalLineGroups = journalLine_LT_DTOsList.GroupBy(l => l.AccId);
                // IQueryable<IGrouping<String, JournalLineLedgerTransactionDTO>> journalLineGroups = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);
                List<ReconciableGroup> reconciableGroupList = new List<ReconciableGroup>();
                List<string> badList = new List<string>();
                List<string> goodList = new List<string>();
                List<string> madeList = new List<string>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
                //   _WrongSum = new List<string>();
                _WrongSumToMatch = new List<string>();

                foreach (IGrouping<String, JournalLineLedgerTransactionAccDTO> group in journalLineGroups)
                {
                    string groupKey = group.Key;
                    List<JournalLineReco> journalLineList = new List<JournalLineReco>();
                    foreach (JournalLineLedgerTransactionAccDTO journalLineLedgerTransactionDTO in group)
                    {
                        journalLineList.Add(new JournalLineReco(journalLineLedgerTransactionDTO.JournalLine, journalLineLedgerTransactionDTO.LedgerTransaction));
                    }
                    if (IsGroupReconciable(journalLineList, groupKey, maximalDifference, context, tenant))
                    {
                        ReconciableGroup recoGroup = new ReconciableGroup(groupKey, journalLineList);
                        reconciableGroupList.Add(recoGroup);
                        goodList.Add(groupKey);
                    }
                    else
                    {
                        badList.Add(groupKey);
                    }
                }
                reconciableGroupList.Sort((x, y) => x._Acc.CompareTo(y._Acc));
                reconciableGroupList.ForEach(recoGroup =>
                {
                    string gLAccountId = GetGroupGLAccountId(recoGroup._LineGroup);
                    if (gLAccountId != myGLAccountId)
                    {
                        throw new Exception($"GLAccount Id error");
                    }
                    if (!String.IsNullOrWhiteSpace(gLAccountId))
                    {
                        using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                        {
                            ReconcileOneRef(recoGroup._LineGroup, gLAccountId, ledgerTransactionQueryService);
                            _counter++;
                            scope.Complete();
                        }
                        madeList.Add(recoGroup._Acc);
                    }
                });
                //   _ResponseText = $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}, No Lines: {String.Join(", ", _NoLines.ToArray())}, Wrong Action: {String.Join(", ", _WrongAction.ToArray())}, Wrong Sum: {String.Join(", ", _WrongSum.ToArray())}, Wrong Sum To Match: {String.Join(", ", _WrongSumToMatch.ToArray())}";
                _ResponseText = $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}, No Lines: {String.Join(", ", _NoLines.ToArray())}, Wrong Action: {String.Join(", ", _WrongAction.ToArray())}, Wrong Sum To Match: {String.Join(", ", _WrongSumToMatch.ToArray())}";
            }
            catch (Exception e)
            {
                throw new Exception($"ReconciliationStageCBatch failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }

        private List<LedgerTransaction> GetNextReconciableLT_List(ref GetNextGroupArgs getNextGroupArgs)
        {
            IAccountingContext context = AccountingContext.GetContext(getNextGroupArgs.Tenant);
            LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(context);
            List<LedgerTransaction> LT_List = ledgerTransactionListQueryService.GetLedgerTransactionsByAcc_NotReconciled(ref getNextGroupArgs);
            return LT_List;
        }

        private List<LedgerTransaction> GetLedger(List<JournalLineReco> journalLineRecoList)
        {
            List<LedgerTransaction> rv = new List<LedgerTransaction>();
            journalLineRecoList.ForEach(journalLineReco =>
            {
                // rv.AddRange(journalLineReco._oneLineLedger);
                rv.Add(journalLineReco._oneLineLedger);
            });
            return rv;
        }

        private string GetGroupGLAccountId(List<JournalLineReco> journalLineRecoList)
        {

            string rv = "";
            JournalLineReco creditLine = journalLineRecoList.Where(line => line._journalLine.ActionCode == "1").FirstOrDefault();
            if (creditLine != null)
            {
                rv = creditLine._journalLine.CreditAccountId;
            }
            else
            {
                JournalLineReco debitLine = journalLineRecoList.Where(line => line._journalLine.ActionCode == "2").FirstOrDefault();
                if (debitLine != null)
                {
                    rv = debitLine._journalLine.DebitAccountId;
                }
            }
            if (!String.IsNullOrWhiteSpace(rv) && journalLineRecoList.Exists(line => line._journalLine.ActionCode == "1" && line._journalLine.CreditAccountId != rv))
            {
                rv = "";
            }
            if (!String.IsNullOrWhiteSpace(rv) && journalLineRecoList.Exists(line => line._journalLine.ActionCode == "2" && line._journalLine.DebitAccountId != rv))
            {
                rv = "";
            }
            return rv;
        }

        private bool IsGroupReconciable(List<JournalLineReco> journalLineRecoList, string groupKey, decimal maximalDifference, IAccountingContext context, int tenant)
        {
            bool rv = true;
            string gLAccountId = GetGroupGLAccountId(journalLineRecoList);
            if (String.IsNullOrWhiteSpace(gLAccountId))
            {
                _NoLines.Add(groupKey);
                rv = false;
            }
            else
            {
                GLAccountListQueryService glaQuery = new GLAccountListQueryService(context);
                GLAccountList gla = glaQuery.GetByAccountId(gLAccountId, tenant);

                List<JournalLineReco> creditLines = journalLineRecoList.Where(line => line._journalLine.ActionCode == "1").ToList<JournalLineReco>();
                decimal credit_sum = 0m;
                if (gla.ReconcileMethodCode == "1") // Foreign Currency
                    if (creditLines != null) credit_sum = creditLines.Sum(line => line._journalLine.LocalAmount + (line._journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                else
                    if (creditLines != null) credit_sum = creditLines.Sum(line => line._journalLine.LocalAmount + (line._journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 

                List<JournalLineReco> debitLines = journalLineRecoList.Where(line => line._journalLine.ActionCode == "2").ToList<JournalLineReco>();
                decimal debit_sum = 0m;
                if (gla.ReconcileMethodCode == "1") // Foreign Currency
                    if (debitLines != null) debit_sum = debitLines.Sum(line => line._journalLine.LocalAmount - (line._journalLine.ExternalOpenAmount ?? 0m));
                else
                    if (debitLines != null) debit_sum = debitLines.Sum(line => line._journalLine.LocalAmount - (line._journalLine.ExternalOpenAmount ?? 0m));


                if (journalLineRecoList.Count == 0)
                {
                    _NoLines.Add(groupKey);
                    rv = false;
                }
                else if (journalLineRecoList.Count == 1)
                {
                    _WrongSumToMatch.Add(groupKey);
                    rv = false;
                }
                else if (journalLineRecoList.Exists(line => line._journalLine.ActionCode != "1" && line._journalLine.ActionCode != "2"))
                {
                    _WrongAction.Add(groupKey);
                    rv = false;
                }
                // else if (credit_sum - debit_sum != 0m)
                else if (Math.Abs(credit_sum - debit_sum) > maximalDifference)
                {
                    _WrongSumToMatch.Add(groupKey);
                    rv = false;
                }
                else if (journalLineRecoList.Exists(line => line._oneLineLedger == null))
                {
                    rv = false;
                }
                else
                {
                    rv = true;
                    //if (gla.ReconcileMethodCode != "1") // NOT a Foreign Currency
                    //{

                    //Decimal sum = 0m;
                    //sum = journalLineRecoList.Sum(line => line._valueToMatch);
                    //if (sum != 0m)
                    //{
                    //    _WrongSumToMatch.Add(groupKey);
                    //    rv = false;
                    //}
                    //}
                }
            }
            return rv;
        }

        private void ReconcileOneRef(List<JournalLineReco> journalLineRecoList, string gLAccountId, LedgerTransactionQueryService ledgerTransactionQueryService)
        {
            //           using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
            //           {
            try
            {
                List<LedgerTransaction> ledger = GetLedger(journalLineRecoList);
                List<LedgerTransactionPM> ledgerPMs = ledger.Select(poco => ledgerTransactionQueryService.GetEntityPM(poco)).ToList();
                CreateReconciliationService createReconciliationService = new CreateReconciliationService();
                ReconciliationPM reconciliationPM = createReconciliationService.GetReconciliation(ledgerPMs);
                reconciliationPM.CreatedByReconciliationStageB = true;
                CreateReconciliationService service = new CreateReconciliationService();
                RecoCallback recoCallback = service.CreateReconciliation(reconciliationPM);
                //                   scope.Complete();
            }
            catch (Exception e)
            {
                //scope.Dispose();
                throw new Exception("ReconciliationStageCBatch failed while performing ReconcileOneRef ", e);
            }
            //           }
        }

        private class ReconciableGroup
        {
            public string _Acc { get; set; }
            public List<JournalLineReco> _LineGroup { get; set; }
            public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
            {
                _LineGroup = lineGroup;
                _Acc = reference;
            }
        }


        private class JournalLineReco
        {

            public Decimal _valueToMatch { get; set; }
            public JournalLine _journalLine { get; set; }
            public LedgerTransaction _oneLineLedger { get; set; }
            public JournalLineReco(JournalLine journalLine, LedgerTransaction ledgerTransaction) // LedgerTransactionQueryService ledgerTransactionQueryService)
            {
                this._journalLine = journalLine;
                this._valueToMatch = 0m;
                if (journalLine.ActionCode == "1")
                {
                    //              this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
                    //              this._valueToMatch = -(journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                    this._valueToMatch = ledgerTransaction.OpenAmount; ///+ (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                }
                else if (journalLine.ActionCode == "2")
                {
                    // this._valueToMatch = -(journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
                    //       this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
                    this._valueToMatch = ledgerTransaction.OpenAmount; //+ (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
                }
                this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
                this._oneLineLedger.AmountToReconcile = _valueToMatch;
            }
        }

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }

    }
    public class ReconciliationStageCArg
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }

        public string AccountTypeCode { get; set; }

        public DateTime UpToAccountingDate { get; set; }

        public int LT_LinesMaximum { get; set; }

        public decimal MaximalDifference { get; set; }

        public BatchTaskExecutionPM BatchTask { get; set; }
    }
   

}