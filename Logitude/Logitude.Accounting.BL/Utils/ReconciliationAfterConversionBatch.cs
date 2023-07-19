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
using System.Diagnostics;
using Microsoft.ServiceBus;

namespace Logitude.Accounting.BL.Utils
{
    public class ReconciliationAfterConversionBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _NoLines;
        private List<string> _WrongAction;
      //  private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;
        private string _current = "";
        private string _currentLower = "";
        private string _currentUpper = "";

        public ReconciliationAfterConversionBatch()
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

        public void RunReconciliationAfterConversion(ReconciliationAfterConversionArg reconciliationAfterConversionArg, int timeoutinmin, ref bool retry)
        {
            var sw = Stopwatch.StartNew();
            int SUB_BATCH_SIZE = 50; // 100;
            string returnedMessage = "";
            int tenant = reconciliationAfterConversionArg.Tenant;
            string fromExtNum = reconciliationAfterConversionArg.FromExtNum;
            string toExtNum = reconciliationAfterConversionArg.ToExtNum;
            BatchTaskExecutionPM batchTaskExecutionPM = reconciliationAfterConversionArg.BatchTask;
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
            if (batchTaskExecutionPM != null)
            { 
                batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant); 
            }

            long fromExt_long = Convert.ToInt64(fromExtNum);
            long toExt_long = Convert.ToInt64(toExtNum);
            if (toExt_long >= 10 * SUB_BATCH_SIZE)
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);
                string maxExt = journalLineQueryService.GetMaxExternalRecoNum(tenant);
                if (!String.IsNullOrEmpty(maxExt) && toExt_long > Convert.ToInt64(maxExt))
                {
                    toExtNum = maxExt;
                    toExt_long = Convert.ToInt64(maxExt);
                }
            }

            for (long lower = fromExt_long; ;)
            {

                long upper = lower + SUB_BATCH_SIZE - 1;
                if (upper > toExt_long)
                {
                    upper = toExt_long;
                }
                string lower_str = "";
                if (lower <= 0)
                {
                    lower_str = "000000000000001";
                }
                else
                {
                    lower_str = Regex.Replace(lower.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));
                }

                string upper_str = "";
                upper_str = Regex.Replace(upper.ToString(), @"\d+", n => n.Value.PadLeft(15, '0'));
                try
                {
                    RunReconciliationAfterConversionInner(tenant, lower_str, upper_str, batchTaskExecutionPM);
                }
                catch (Exception e)
                {
                    string message = e.Message + System.Environment.NewLine;
                    if (batchTaskExecutionPM != null)
                    {
                        // there to put message into the batch task log
                        batchTaskExecutionPM.ErrorLog = batchTaskExecutionPM.ErrorLog + " " + message;
                    }
                    else
                    {
                        returnedMessage = returnedMessage + " " + message;
                    }
                }
                if (batchTaskExecutionPM != null)
                {
                    double percentage_double = Convert.ToDouble(upper) / Convert.ToDouble(toExt_long) * 100.0;
                    int percentage = Convert.ToInt32(Math.Round(percentage_double));
                    batchTaskExecutionPM.ProgressPercentage = percentage;
                    // there to put percentage into the batch task status 
                }


                if (lower + SUB_BATCH_SIZE - 1 >= toExt_long)
                {
                    break;
                }
                else
                {
                    lower += SUB_BATCH_SIZE;
                    if (lower > toExt_long)
                    {
                        lower = toExt_long;
                    }
                }

                if (sw.Elapsed.TotalMinutes >= timeoutinmin)
                {
                    retry = true;
                    break;
                }
            }
            if (batchTaskExecutionPM != null)
            {
                batchTaskExecutionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                if (batchTaskExecutionUpdateService != null)
                { 
                    batchTaskExecutionUpdateService.Update(batchTaskExecutionPM, true); 
                }
            }
            else 
            { 
                if (!String.IsNullOrEmpty(returnedMessage))
                {
                    throw new Exception(returnedMessage);
                }
            }

        }

        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }

        public void RunReconciliationAfterConversionInner(int tenant, string fromExtNum, string toExtNum, BatchTaskExecutionPM batchTaskExecutionPM)
        {
            try
            {
                IAccountingContext context = AccountingContext.GetContext(tenant);
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);
                //  IQueryable<JournalLineLedgerTransactionDTO> journalLine_LT_DTOs = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                IEnumerable<JournalLineLedgerTransactionDTO> journalLine_LT_DTOs = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                List<JournalLineLedgerTransactionDTO> journalLine_LT_DTOsList = journalLine_LT_DTOs.ToList().OrderBy(rec => rec.JournalLine.ExternalReconcileNumber).ToList();
                var journalLineGroups = journalLine_LT_DTOsList.GroupBy(rec => rec.JournalLine.ExternalReconcileNumber);
                _currentLower = fromExtNum;
                _currentUpper = toExtNum;
                //IQueryable<IGrouping<String, JournalLineLedgerTransactionDTO>> journalLineGroups = journalLineQueryService.GetQGJournalLinesByExternalRecoFromTo(tenant, fromExtNum, toExtNum);
                LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(context);
                List<ReconciableGroup> reconciableGroupList = new List<ReconciableGroup>();
                List<string> badList = new List<string>();
                List<string> goodList = new List<string>();
                List<Int64> madeList = new List<Int64>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
            //    _WrongSum = new List<string>();
                _WrongSumToMatch = new List<string>();
                long _counter = 0;

                foreach (IGrouping<String, JournalLineLedgerTransactionDTO> group in journalLineGroups)
                {
                    string groupKey = group.Key;
                    List<JournalLineReco> journalLineList = new List<JournalLineReco>();
                    foreach (JournalLineLedgerTransactionDTO journalLineLedgerTransactionDTO in group)
                    {
                        journalLineList.Add(new JournalLineReco(journalLineLedgerTransactionDTO.JournalLine, journalLineLedgerTransactionDTO.LedgerTransaction));
                    }
                    if (IsGroupReconciable(journalLineList, groupKey, context, tenant))
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
                reconciableGroupList.Sort((x, y) => x._Ref.CompareTo(y._Ref));
                reconciableGroupList.ForEach(recoGroup =>
                {
                    string gLAccountId = GetGroupGLAccountId(recoGroup._LineGroup);
                    if (!String.IsNullOrWhiteSpace(gLAccountId))
                    {
                        try
                        {
                            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                            {
                                _current = recoGroup._Ref.ToString();
                                ReconcileOneRef(recoGroup._LineGroup, gLAccountId, ledgerTransactionQueryService);
                                _counter++;
                                //      if (_counter%100 == 0)
                                //      {
                                scope.Complete();
                                //      }
                            }

                            madeList.Add(recoGroup._Ref);
                        }
                        catch (Exception ex) 
                        {
                            if (ex.Message == "The underlying provider failed on Open." || ex.InnerException.Message == "Exception has been thrown by the target of an invocation.")
                            {
                                badList.Add(_current);
                                if (batchTaskExecutionPM != null)
                                {
                                    // there to put message into the batch task log
                                    string logtext = $"ReconciliationAfterConversionBatch failure on Ref. {_current} ({_currentLower},{_currentUpper}) {ex.Message} Inner Exception: {ex.InnerException.Message}";
                                    batchTaskExecutionPM.ErrorLog = batchTaskExecutionPM.ErrorLog + " " + logtext;
                                }
                            }
                            else
                                throw;
                        }
                    }
                });
                //     scope.Complete();
            //    _ResponseText += $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}, No Lines: {String.Join(", ", _NoLines.ToArray())}, Wrong Action: {String.Join(", ", _WrongAction.ToArray())}, Wrong Sum: {String.Join(", ", _WrongSum.ToArray())}, Wrong Sum To Match: {String.Join(", ", _WrongSumToMatch.ToArray())}";
                _ResponseText += $"Good: {goodList.Count},  Bad: {badList.Count},   Made: {madeList.Count}, No Lines: {String.Join(", ", _NoLines.ToArray())}, Wrong Action: {String.Join(", ", _WrongAction.ToArray())}, Wrong Sum To Match: {String.Join(", ", _WrongSumToMatch.ToArray())}";
            }
            catch (Exception e)
            {
                throw new Exception($"ReconciliationAfterConversionBatch failure on Ref. {_current} ({_currentLower},{_currentUpper}) {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
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

        private bool IsGroupReconciable(List<JournalLineReco> journalLineRecoList, string groupKey, IAccountingContext context, int tenant)
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
                if (creditLines != null)
                {
                    if (gla.ReconcileMethodCode == "1") // Foreign Currency
                        credit_sum = creditLines.Sum(line => line._journalLine.ForeignAmount + (line._journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                    else
                        credit_sum = creditLines.Sum(line => line._journalLine.LocalAmount + (line._journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                }

                List<JournalLineReco> debitLines = journalLineRecoList.Where(line => line._journalLine.ActionCode == "2").ToList<JournalLineReco>();
                decimal debit_sum = 0m;
                if (debitLines != null)
                {
                    if (gla.ReconcileMethodCode == "1") // Foreign Currency
                        debit_sum = debitLines.Sum(line => line._journalLine.ForeignAmount - (line._journalLine.ExternalOpenAmount ?? 0m));
                    else
                        debit_sum = debitLines.Sum(line => line._journalLine.LocalAmount - (line._journalLine.ExternalOpenAmount ?? 0m));
                }


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
                else if (journalLineRecoList.Exists(line => line._oneLineLedger.IsReconciled == true))
                {
                    _WrongAction.Add(groupKey);
                    rv = false;
                }
                //           else if (!journalLineRecoList.Exists(line => line._journalLine.ActionCode == "1") ||  !journalLineRecoList.Exists(line => line._journalLine.ActionCode == "2"))
                //  else if ((journalLineRecoList.Sum(line => line._journalLine.LocalAmount) != 0m))
                else if (credit_sum - debit_sum != 0m)
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
                    if (gla.ReconcileMethodCode != "1") // NOT a Foreign Currency
                    {
                        Decimal sum = 0m;
                        sum = journalLineRecoList.Sum(line => line._valueToMatch);
                        if (sum != 0m)
                        {
                            _WrongSumToMatch.Add(groupKey);
                            rv = false;
                        }
                    }
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
                    reconciliationPM.CreatedByReconciliationAfterConversion = true;
                    CreateReconciliationService service = new CreateReconciliationService();
                    RecoCallback recoCallback = service.CreateReconciliation(reconciliationPM);
 //                   scope.Complete();
                }
                catch (Exception e)
                {
                    //scope.Dispose();
                    throw new Exception("ReconciliationAfterConversionBatch failed while performing ReconcileOneRef ", e);
                }
 //           }
        }

        private class ReconciableGroup
        {
            public Int64 _Ref { get; set; }
            public List<JournalLineReco> _LineGroup { get; set; }
            public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
            {
                _LineGroup = lineGroup;
                _Ref = Int64.Parse(reference);
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
                    //                   this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
                    // this._valueToMatch = -(journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                    this._valueToMatch = ledgerTransaction.OpenAmount; ///+ (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
                }
                else if (journalLine.ActionCode == "2")
                {
 //                   this._valueToMatch = -(journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
           //         this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
                    this._valueToMatch = ledgerTransaction.OpenAmount; //+ (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
                }
                this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
                this._oneLineLedger.AmountToReconcile = _valueToMatch;
            }
        }
    }
    public class ReconciliationAfterConversionArg
    {
        public int Tenant { get; set; }
        public string FromExtNum { get; set; }
        public string ToExtNum { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}