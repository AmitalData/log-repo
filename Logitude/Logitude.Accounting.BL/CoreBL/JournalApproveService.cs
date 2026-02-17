using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CloseTables;
using static Simplog.Server.Infrastructure.DbContextBase;
using System.Transactions;
using Simplog.Data.Helpers;
using System.Data;
using System.Data.SqlClient;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Server.Tools;
using System.Web;
using Logitude.Accounting.BL.CoreBL.Reports.Aging;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.CoreBL;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.Utils;
using Simplog.Data.InvoiceModel;
using Simplog.Data.CommonDataModel;
using Newtonsoft.Json;
using Microsoft.Practices.Unity;
using System.Collections;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools.Utils;
using CsvHelper.Configuration;
using Logitude.BL.CommonDataModel.EntityQueries;


namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalApproveService
    {

        public const string K_AccountingJournalApproveMutliThreadingWR = "AccountingJournalApproveMutliThreadingWR";
        public const string K_AccountingJournalApproveWR = "AccountingJournalApproveWR";
        public const string K_AccountingConversionJournalApproveWR = "AccountingConversionJournalApproveWR";
        public const string ApproveMethod = "JournalApproveService";

        public const string QP_JournalTenant = "JournalTenant";
        public const string QP_JournalId = "JournalId";
        const string CashARPaymentAccountingMethod = "CA";
        const string ChequeARPaymentAccountingMethod = "CH";
        const string BankTransferARPaymentAccountingMethod = "BT";
        const string ARPaymentApprovedStatusCode = "AD";
        const string ARPaymentClosedStatusCode = "CL";
        const string ARPaymentVoidedStatusCode = "VD";
        const string ReturnedToCustomer = "5";
        private const string CreditLineNotes = "החזרת שיק ללקוח";
        private string _QMessageId;
        private string _SelectedQueue;
        int _Tenant;
        string _SeedJournalId;
        private IAccountingContext _AccountingContext;
        private JournalPM _JournalPM;
        private JournalApproveParser _JournalApproveParser = null;


        public JournalApproveService(int tenant, string seedJournalId, string MessageId, string selectedQueue)
        {
            _Tenant = tenant;
            _SeedJournalId = seedJournalId;
            this._QMessageId = MessageId;
            this._SelectedQueue = selectedQueue;
        }




        public ResultApproveJournalM SubmitApprove(MyActions actions)
        {
            var sw = Stopwatch.StartNew();
            _ExecAsSP = true;
            try
            {


                ResultApproveJournalM myResultApproveJournalM = null; ;
                List<LedgerTransactionPM> myLedgerTransactionsWithCounters = null;

                //Do name the zero value of flags enumerations None. For a flags enumeration, the value must always mean all flags are cleared.
                if (actions == MyActions.None) return new ResultApproveJournalM() { Success = false, FailDue = "actions== MyActions.None" };
                if (actions.HasFlag(MyActions.BuildLedgerTransaction) && actions.HasFlag(MyActions.FixLedgerTransaction)) return new ResultApproveJournalM() { Success = false, FailDue = "Or BuildLedgerTransaction Or FixLedgerTransaction" };


                List<GLAccountAgingDataPM> gLAccountAgingDataPMs = null;
                myResultApproveJournalM = CheckJournal(actions, ref myLedgerTransactionsWithCounters, out gLAccountAgingDataPMs);
                if (myResultApproveJournalM != null)
                {
                    return myResultApproveJournalM;
                }
                //

                AccountingStreamingInNewSerializableTransaction(actions, myLedgerTransactionsWithCounters, gLAccountAgingDataPMs);
                CalculateTotalFutureOpenChequesForCreditGlAccount(myLedgerTransactionsWithCounters);

                return new ResultApproveJournalM()
                {
                    Success = true
                };
            }
			catch (Exception ex)
			{
				LogMessagingUtil.Instance.AppendLine($"[usp_AccountingStreaming] Error in AccountingStreamingInNewSerializableTransaction! JournalPM?.Id={_JournalPM?.Id} | Exception: {ex.Message}");

				NetCommonHelper.Logger.DevLog.Instance.WriteError(
				$"[usp_AccountingStreaming] Error in AccountingStreamingInNewSerializableTransaction! JournalPM?.Id={_JournalPM?.Id} | Exception: {ex}");

				if (_JournalPM?.StatusCode == "6" && !_JournalPM.IsLedgerCreated)
				{
					try
					{
						using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
						{
							var updater = new JournalUpdateService(_AccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _Tenant);
							updater.SetStatusCodeFailed(_SeedJournalId, _Tenant);
                             scope.Complete();
						}
                        var journalQueryService = new JournalQueryService(_AccountingContext);
                        journalQueryService.FixFailedReconcileJournals(_Tenant);


                    }
                    catch (Exception updateEx)
					{
						NetCommonHelper.Logger.DevLog.Instance.WriteError(
							$"[usp_AccountingStreaming] Failed to update journal status after primary exception. JournalId={_JournalPM?.Id} | Update Exception: {updateEx}");
					}
				}
				return new ResultApproveJournalM()
				{
					Success = false,
                    FailDue = ex.Message
				};
			}
			finally
            {
                LogMessagingUtil.Instance.AppendLine("SubmitApprove(" + _SeedJournalId + ") took:" + sw.Elapsed.ToString());
            }
        }
        private void CreateInterestTransactions(JournalPM journalPM, IAccountingContext context, bool isTester = false)
        {
            if (journalPM is null || journalPM.IsLedgerCreated == true)
            {
                return;
            }
            if (journalPM.AccountingEntityCode == AccountingEntityValues.ARPayment)
            {
                ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(journalPM.Tenant);
                ARPaymentPM aRPaymentPM = aRPaymentQuery.GetSinglePM(journalPM.AccountingEntityId, journalPM.Tenant);

                if (aRPaymentPM.AccountingPaymentMethodCode == CashARPaymentAccountingMethod)
                {
                    CreateInterestTrascntionsForCashARPayment(journalPM, aRPaymentPM, context);
                }
                if ((aRPaymentPM.AccountingPaymentMethodCode == ChequeARPaymentAccountingMethod || aRPaymentPM.AccountingPaymentMethodCode == BankTransferARPaymentAccountingMethod)
                    && aRPaymentPM.StatusCode == ARPaymentVoidedStatusCode)
                {
                    CancelInterestTrascntionsForChequeOrBankTranasfersARPayment(journalPM, aRPaymentPM, context);
                }
                if (aRPaymentPM.AccountingPaymentMethodCode == ChequeARPaymentAccountingMethod &&
                    (aRPaymentPM.StatusCode == ARPaymentApprovedStatusCode || aRPaymentPM.StatusCode == ARPaymentClosedStatusCode))
                {
                    CreateInterestTrascntionsForChequeARPayment(journalPM, aRPaymentPM, context);
                }

                if (aRPaymentPM.AccountingPaymentMethodCode == BankTransferARPaymentAccountingMethod &&
                    (aRPaymentPM.StatusCode == ARPaymentApprovedStatusCode || aRPaymentPM.StatusCode == ARPaymentClosedStatusCode))
                {
                    CreateInterestTrascntionsForBankTransfersARPayment(journalPM, aRPaymentPM, context);
                }
            }
            else if (journalPM.AccountingEntityCode == AccountingEntityValues.Journal || journalPM.AccountingEntityCode == AccountingEntityValues.Adjustment
               || journalPM.AccountingEntityCode == AccountingEntityValues.BankAdjustment)
            {
                var journalUpdateService = new JournalUpdateService(context, new Dictionary<string, IContext>(), journalPM.Tenant);
                journalUpdateService.CreateInterestTransactionTo_RegularJournal(journalPM, isTester);
            }
            else if (journalPM.AccountingEntityCode == AccountingEntityValues.Revaluation)
            {
                RevaluationBatch revaluationBatch = new RevaluationBatch();
                revaluationBatch.AddInterestTransactions(journalPM, context);
            }
            else if (journalPM.AccountingEntityCode == AccountingEntityValues.ARInvoice)
            {
                IInvoiceContext _invoiceContext = InvoiceContext.GetContext(journalPM.Tenant);
                ARInvoiceService aRInvoiceService = new ARInvoiceService(_invoiceContext, journalPM.Tenant);
                aRInvoiceService.CreateARInvoiceInterestTransactions(journalPM.AccountingEntityId, journalPM.Tenant);
            }
        }

        private void CreateInterestTrascntionsForCashARPayment(JournalPM journalPM, ARPaymentPM aRPaymentPM, IAccountingContext context)
        {
            ARPaymentService service = new ARPaymentService(null, journalPM.Tenant);
            var interestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), journalPM.Tenant);
            var interestTranasction = service.MapInterestTransactionPMFromARPaymentPM(aRPaymentPM, aRPaymentPM.StatusCode == ARPaymentVoidedStatusCode , journalPM);
            if (!CheckIfInterestTransactionCreated(interestTranasction))
                interestTransactionUpdateService.Update(interestTranasction, true);
        }

        private bool CheckIfInterestTransactionCreated(InterestTransactionPM interestTransactionPM)
        {
            if (interestTransactionPM == null)
            {
                return true;
            }
            InterestTransactionUniqueConstraintFields uniqueConstraintFields = new InterestTransactionUniqueConstraintFields()
            {
                GLAccountId = interestTransactionPM.GLAccountId,
                Tenant = interestTransactionPM.Tenant,
                InterestEntityTypeCode = interestTransactionPM.InterestEntityTypeCode,
                EntityId = interestTransactionPM.EntityId,
                OriginalEntityLineNumber = interestTransactionPM.OriginalEntityLineNumber
            };

            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(interestTransactionPM.Tenant);
            var interestTransaction = interestTransactionQueryService.GetTransactionByUniqueConstraintFields(uniqueConstraintFields);
            if (interestTransaction == null) return false;
            else return true;

        }
        private void CancelInterestTrascntionsForChequeOrBankTranasfersARPayment(JournalPM journalPM, ARPaymentPM aRPaymentPM, IAccountingContext context)
        {
            ARPaymentService service = new ARPaymentService(null, journalPM.Tenant);
            List<InterestTransactionPM> interestTranactions = service.GetARPaymentInterestTransactionsForCancellation(aRPaymentPM, journalPM);
            var interestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), journalPM.Tenant);
            foreach (var interestTransaction in interestTranactions)
            {
                interestTransactionUpdateService.Update(interestTransaction, true);
            }
        }

        private void CreateInterestTrascntionsForChequeARPayment(JournalPM journalPM, ARPaymentPM aRPaymentPM, IAccountingContext context)
        {
            var interestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), journalPM.Tenant);
            ARPaymentChequeQueryService aRPaymentChequeQuery = new ARPaymentChequeQueryService(journalPM.Tenant);
            FullAccountingARPaymentApproveService fullAccountingARPaymentApproveService = new FullAccountingARPaymentApproveService(aRPaymentPM, journalPM.Tenant, false, false);

            List<ARPaymentChequePM> arPaymentCheques = aRPaymentChequeQuery.GetListByPaymentId(aRPaymentPM.Id, journalPM.Tenant);

            if (journalPM.JournalLines.Any(x => x.Notes == CreditLineNotes))
            {
                var chequeNumber = journalPM.JournalLines.FirstOrDefault()?.Reference1;
                var returnedCheque = arPaymentCheques.FirstOrDefault(x => x.ChequeNumber == chequeNumber);
                int returnedChequeLineNumber = arPaymentCheques.Max(x => x.LineNumber) + returnedCheque.LineNumber;
                var interestTranasction = fullAccountingARPaymentApproveService.GetInterestTransactionLineForCheque(returnedCheque, aRPaymentPM, journalPM);
                if (interestTranasction != null)
                {
                    interestTranasction.OriginalEntityLineNumber = returnedChequeLineNumber;
                    if (!CheckIfInterestTransactionCreated(interestTranasction))
                        interestTransactionUpdateService.Update(interestTranasction, true);
                }
            }
            else
            {
                foreach (var cheque in arPaymentCheques)
                {
                    var interestTranasction = fullAccountingARPaymentApproveService.GetInterestTransactionLineForCheque(cheque, aRPaymentPM, journalPM);
                    if (!CheckIfInterestTransactionCreated(interestTranasction))
                        interestTransactionUpdateService.Update(interestTranasction, true);
                }
            }
        }

        private void CreateInterestTrascntionsForBankTransfersARPayment(JournalPM journalPM, ARPaymentPM aRPaymentPM, IAccountingContext context)
        {
            var interestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), journalPM.Tenant);
            FullAccountingARPaymentApproveService fullAccountingARPaymentApproveService = new FullAccountingARPaymentApproveService(aRPaymentPM, journalPM.Tenant, false, false);

            foreach (var bankTranfer in aRPaymentPM.ARPaymentBankTranfers)
            {
                var interestTranasction = fullAccountingARPaymentApproveService.GetInterestTransactionLineForBankTransfer(bankTranfer, aRPaymentPM , journalPM);
                if (!CheckIfInterestTransactionCreated(interestTranasction))
                    interestTransactionUpdateService.Update(interestTranasction, true);
            }
        }

        private void TryCreateStornoAutoReconcile()
        {
            throw new NotImplementedException();
        }


        private static string GetTenantCurrencyId(int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            var tenantPM = tenantQuery.GetSinglePM(tenant);
            return tenantPM.CurrencyId;
        }

        private ResultApproveJournalM CheckJournal(MyActions actions, ref List<LedgerTransactionPM> myLedgerTransactionsWithCounters, out List<GLAccountAgingDataPM> gLAccountAgingDataPMs)
        {
            gLAccountAgingDataPMs = new List<GLAccountAgingDataPM>();
            using (var scope = TransactionFactory.GetTransaction())
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);
                var journalQueryService = new JournalQueryService(_AccountingContext);
                _JournalPM = journalQueryService.GetSingle(_SeedJournalId, true, false);
                bool restreamVoidedJournalDueBug = false;
                if (_JournalPM.StatusCodeEnum == JournalStatusTypePM.StatusCodeEnum.Voided)
                {
                    if (actions.HasFlag(MyActions.DueBugReStreamAllJournalAgain2Accounting))
                    {
                        restreamVoidedJournalDueBug = true;
                    }
                }
                //if (!restreamVoidedJournalDueBug && _JournalPM.StatusCodeEnum != JournalStatusTypePM.StatusCodeEnum.Approved)
                //{
                //    return new ResultApproveJournalM()
                //    {
                //        Success = false,
                //        FailDue = "_JournalPM.MyStatusCodeEnum != JournalStatusTypePM.StatusCodeEnum.Approved"
                //    };
                //}
                ;
                bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                _JournalApproveParser = new JournalApproveParser(_JournalPM, false,
                    AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(_AccountingContext, _JournalPM,
                    SuppressCheckGLAccountIsMultiCurrencyWI40640)
                    );
                _JournalApproveParser.StreamingJournalAlreadChecked = false;
                _JournalApproveParser.ParseIt();
                //_JournalApproveParser.CopyGLAccountTotalByMonthsToControl(_AccountingContext);

                if (actions.HasFlag(MyActions.BuildLedgerTransaction))
                {
                    var ledgerTransactionQueryService = new LedgerTransactionQueryService(_AccountingContext);
                    var ledgerTransactionid = ledgerTransactionQueryService.GetAnyLedgerTransactionByJournalId(_SeedJournalId, _Tenant);
                    if (!string.IsNullOrWhiteSpace(ledgerTransactionid))
                    {
                        return new ResultApproveJournalM()
                        {
                            Success = false,
                            FailDue =
                            string.Format("BuildLedgerTransaction():Journalid:{0} connect to ledgerTransactionid:{1}", _SeedJournalId, ledgerTransactionid)
                        };

                    }
                    myLedgerTransactionsWithCounters = _JournalApproveParser.LedgerTransactions
                        .OrderBy(rec => rec.JournalId).ThenBy(rec => rec.JournalLineNumber)
                        .ToList();
                    if (this._SelectedQueue == JournalApproveService.K_AccountingJournalApproveWR)
                    {
                        var ledgerTransactionsAgingBuilderService = new LedgerTransactionsAgingBuilderService(_AccountingContext);
                        gLAccountAgingDataPMs = ledgerTransactionsAgingBuilderService.GetAgingPMs(myLedgerTransactionsWithCounters);

                    }


                    //if (!_ExecAsSP)
                    {
                        FillIdCountersUseNewDBTransaction(myLedgerTransactionsWithCounters);
                    }
                    string tenant_curr_id = JournalApproveService.GetTenantCurrencyId(_JournalPM.Tenant);
                    if (myLedgerTransactionsWithCounters != null && myLedgerTransactionsWithCounters.Count > 0)
                    {
                        myLedgerTransactionsWithCounters.ForEach(lt =>
                        {
                            if (lt.OpenAmount == 0m && 
                                    ((lt.LocalAmountDebit == lt.LocalAmountCredit && lt.OpenAmountCurrencyId == tenant_curr_id) || 
                                     (lt.ForeignAmountDebit == lt.ForeignAmountCredit && lt.OpenAmountCurrencyId != tenant_curr_id))) 
                                lt.IsReconciled = true; 
                            // else the value of lt.IsReconciled is conserved
                        });
                    }
                }

                //scope.Complete();//Please do not commit !!!!
            }
            return null;
        }
        //
        private void CalculateTotalFutureOpenChequesForCreditGlAccount(List<LedgerTransactionPM> ledgerTrasnctions)
        {

            if ((_JournalPM.AccountingEntityCode == "3" || _JournalPM.AccountingEntityCode == "1") && _JournalPM.ExternalSystem?.ToUpper() != "AMITAL")
            {

                if (ledgerTrasnctions.Count > 0)
                {
                    GLAccountMoreDataRepository gLAccountMoreDataRepository = new GLAccountMoreDataRepository(_Tenant);
                    GLAccountMoreDataQueryService gLAccountMoreDataQueryService = new GLAccountMoreDataQueryService(_Tenant);
                    List<LedgerTransactionList> allChecks = gLAccountMoreDataRepository.GetAllChecks(ledgerTrasnctions[0].AccountId, _Tenant, false, withoutDate: true);

                    GLAccountMoreData glAccountMoreData = gLAccountMoreDataRepository.GetSingle(ledgerTrasnctions[0].AccountId, _Tenant);
                    GLAccountMoreDataPM moreDataPM = gLAccountMoreDataQueryService.GetEntityPM(glAccountMoreData);

                    moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                    moreDataPM.TotFutureOpenChequesInLocalCur = allChecks.Where(x => x.PaymentValueDate > DateTime.Today).Sum(x => (decimal?)x.CalculatedLocalAmount) ?? 0;
                    moreDataPM.TotalOpenChequesInLocalCur = allChecks.Where(x => x.PaymentValueDate <= DateTime.Today).Sum(x => (decimal?)x.CalculatedLocalAmount) ?? 0;


                    GLAccountMoreDataUpdateService gLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _Tenant);
                    gLAccountMoreDataUpdateService.Update(moreDataPM, true);

                }

            }
            //foreach (var trasnction in ledgerTrasnctions.Where(trasnction => trasnction.LocalAmountCredit != 0))
            //{
            //    var card = GetBillToByGLAccountId(_Tenant, trasnction.AccountId);
            //    if (card != null)
            //    {
            //        GLAccountChequesTotalCalculator chequesTotalCalculator = new GLAccountChequesTotalCalculator(_Tenant);
            //        chequesTotalCalculator.RecalculateChequesTotalForBillToAccount(card.Id);
            //    }
            //}
        }
        private Card GetBillToByGLAccountId(int tenant, string glAccountId)
        {
            CardRepository cardRepo = new CardRepository(tenant);
            var card = cardRepo.GetCardByGLAccountId(glAccountId, tenant);
            return card;
        }

        private static DateTime GetCurrentDate(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            todayDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 11, 59, 59);
            return todayDate;
        }
        private void AccountingStreamingInNewSerializableTransaction(MyActions actions, List<LedgerTransactionPM> myLedgerTransactionsWithCounters, List<GLAccountAgingDataPM> gLAccountAgingDataPMs)
        {

            const string TransferCardId = "1-717294";
            const int TransferCardTenant = 10;

            /// orian 300000 trans in a month >> 1 journal 6 transaction no more then 6 GLAccountTotalByMonths >  in a secound 

            /*
            To reduce the chance of a deadlock:

            Minimize the size of transaction and transaction times.
            Always access server objects in the same order each time in application.
            Avoid cursors, while loops, or process which requires user input while it is running.
            Reduce lock time in application.
            Use query hints to prevent locking if possible (NoLock, RowLock)
            Select deadlock victim by using SET DEADLOCK_PRIORITY.
             */

            var sw = Stopwatch.StartNew();
            //_ConnectedControlGLAccountTotalByMonths = _ConnectedControlGLAccountTotalByMonths ?? new List<GLAccountTotalByMonthPM>();
            var allGLAccountTotalByMonths = _JournalApproveParser.ControlGLAccountTotalByMonths.Union(_JournalApproveParser.GLAccountTotalByMonths);
            var defaultRecord = allGLAccountTotalByMonths.FirstOrDefault();
            if (allGLAccountTotalByMonths.FirstOrDefault(rec => !rec.Tenant.Equals(defaultRecord.Tenant)) != null)
            {
                throw new ApplicationException("in 1 tenent only ");
            }
            var inshureNoDuplicateKeys_MayBeCrash = allGLAccountTotalByMonths.ToDictionary(rec => string.Concat(rec.AccountId, rec.DateTypeCode, rec.Year, rec.Month, rec.CurrencyId));
            allGLAccountTotalByMonths = allGLAccountTotalByMonths.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.DateTypeCode).ThenBy(rec => rec.Year).ThenBy(rec => rec.Month).ThenBy(rec => rec.CurrencyId);

            TimeSpan? timeOut = GetTimeout(myLedgerTransactionsWithCounters, _JournalPM.JournalReconciles.Count > 0);
            timeOut = null;//ihab: no need to use timeout  - but have to be fast (statistic) !!!
            StringBuilder stringBuilderWhyTransferCardBadBalance = new StringBuilder();
            LogMessagingUtil.Instance.AppendLine("GetNewSerializableTransaction(timeOut):insec" + timeOut.GetValueOrDefault().TotalSeconds.ToString());
            using (var scope = GetTransactionScope(timeOut))
            {
                IDbContextLogger logger = null;
                try
                {
                    _AccountingContext = AccountingContext.GetContext(_Tenant);
                    logger = (_AccountingContext as DbContextBase).CreateLogger();

                    bool supperssSaveOnUpdateMultiDueIsFaster = false;
                    //if (_ExecAsSP)
                    //{
                    this.Exec_usp_AccountingStreaming(myLedgerTransactionsWithCounters, allGLAccountTotalByMonths.ToList(), gLAccountAgingDataPMs);
                    // update BalanceInLocalCurrency
                    var allGLAccountTotalByMonthsForAccountingOnly = allGLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.AccountingDate);
                    var glAccountsToUpdate = (from tot in allGLAccountTotalByMonthsForAccountingOnly
                                              group tot by tot.AccountId into groupByAccId
                                              select new
                                              {
                                                  AccountId = groupByAccId.Key,
                                                  LocalAmountDifference = groupByAccId.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit)
                                              }
                                       )
                                       .ToList();


                    var gLAccountMoreDataQueryService = new GLAccountMoreDataQueryService(_AccountingContext);
                    var gLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _Tenant);
                    var glAccounts = allGLAccountTotalByMonthsForAccountingOnly.Select(x => x.AccountId).ToList();
                    var gLAccountMoreDataPMList = gLAccountMoreDataQueryService.GetByGLAccountsIdList(glAccounts, _Tenant);
                    glAccountsToUpdate.ForEach(x =>
                    {
                        var glAccountMoreDataPM = gLAccountMoreDataPMList.Where(acc => acc.AccountId == x.AccountId).First();
                        //if (_Tenant == 99 && x.AccountId == "1-1405813")//"Id":"1-1405813","Tenant":99,
                        if (_Tenant == TransferCardTenant && x.AccountId == TransferCardId)//"Id":"1-1405813","Tenant":99,
                        {
                            stringBuilderWhyTransferCardBadBalance.AppendLine($"now:{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff tt")}");
                            stringBuilderWhyTransferCardBadBalance.AppendLine($"b4:{x.AccountId}:BalanceInLocalCurrency={glAccountMoreDataPM.BalanceInLocalCurrency}");
                        }
                        glAccountMoreDataPM.BalanceInLocalCurrency = glAccountMoreDataPM.BalanceInLocalCurrency + x.LocalAmountDifference;
                        glAccountMoreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                        gLAccountMoreDataUpdateService.Update(glAccountMoreDataPM, true);
                        //if (_Tenant == 99 && x.AccountId == "1-1405813")//"Id":"1-1405813","Tenant":99,
                        if (_Tenant == TransferCardTenant && x.AccountId == TransferCardId)//"Id":"1-1405813","Tenant":99,
                        {
                            stringBuilderWhyTransferCardBadBalance.AppendLine($"after:{x.AccountId}:BalanceInLocalCurrency={glAccountMoreDataPM.BalanceInLocalCurrency}");
                        }

                    });

                    Impersonate();
                    //CreateReconcileFromStorno(myLedgerTransactionsWithCounters);
                    ICreateAutoReconcileWhileStreamingService myCreateAutoReconcileWhileStreamingService = new CreateAutoReconcileWhileStreamingService();
                    myCreateAutoReconcileWhileStreamingService.MustInit(_AccountingContext, _JournalPM, myLedgerTransactionsWithCounters);
                    myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming(true);
                    if (myCreateAutoReconcileWhileStreamingService.ReconciliationList != null &&
                        myCreateAutoReconcileWhileStreamingService.ReconciliationList.Count > 0)
                    {

                        var toUpdateInReconcileProgressToFalse = true;// i think its not happened - have to test b4 
                        if (toUpdateInReconcileProgressToFalse)
                        {
                            UpdateInReconcileProgressToFalse();//Task 138958: לוגיקה בסרביס של התאמות כרטיס - התייחסות ל InRecocileProgress
                        }

                        var myReconciliationUpdateService = new ReconciliationUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
                        myReconciliationUpdateService.SuppressResetDraftOpenReconciliation = true;

                        myReconciliationUpdateService.UpdateMulti(myCreateAutoReconcileWhileStreamingService.ReconciliationList, new List<ReconciliationPM>(), _JournalPM, !supperssSaveOnUpdateMultiDueIsFaster);
                        if (supperssSaveOnUpdateMultiDueIsFaster)
                        {
                            _AccountingContext.SaveChanges();
                        }

                        //var toUpdateInReconcileProgressToFalse = true;// i think its not happened - have to test b4 
                        //if (toUpdateInReconcileProgressToFalse)
                        //{
                        //    UpdateInReconcileProgressToFalse();
                        //}
                        UpdateJournalWithReconcileNumber(myCreateAutoReconcileWhileStreamingService);
                        UpdateTaxReportWithReconcileNumber(myCreateAutoReconcileWhileStreamingService.ReconciliationList);

                    }
                    //}

                    bool featureTested = true;

                    if (featureTested)
                    {
                        var myCreateAutoExternalReconcileWhileStreamingService = new CreateAutoExternalReconcileWhileStreamingService();
                        var providor = new ExternalReconcileDataProvider(_AccountingContext);
                        myCreateAutoExternalReconcileWhileStreamingService.MustInit(providor, _JournalPM, myLedgerTransactionsWithCounters);
                        myCreateAutoExternalReconcileWhileStreamingService.CreateAutoExternalReconcileWhileStreaming();
                        if (myCreateAutoExternalReconcileWhileStreamingService.ExternalReconciliationList != null &&
                        myCreateAutoExternalReconcileWhileStreamingService.ExternalReconciliationList.Count > 0)
                        {
                            var myExternalReconciliationUpdateService = new ExternalReconciliationUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);

                            myExternalReconciliationUpdateService.UpdateMulti(myCreateAutoExternalReconcileWhileStreamingService.ExternalReconciliationList, new List<ExternalReconciliationPM>(), _JournalPM, !supperssSaveOnUpdateMultiDueIsFaster);
                            if (supperssSaveOnUpdateMultiDueIsFaster)
                            {
                                _AccountingContext.SaveChanges();
                            }
                            var toUpdateInReconcileProgressToFalse = true;// next sprint
                            if (toUpdateInReconcileProgressToFalse)
                            {
                                UpdateInExternalReconcileProgressToFalse();
                            }
                            UpdateJournalWithExternalReconcileNumber(myCreateAutoExternalReconcileWhileStreamingService);
                        }
                    }

#if false
                    else
                    {
                        if (string.IsNullOrWhiteSpace(_QMessageId))
                        {
                            _QMessageId = DateTime.UtcNow.GetHashCode().ToString();
                        }
                        
                        var journalRepository = new JournalRepository(_AccountingContext);
                        journalRepository.LockNoWaitUpdateQueueId(_JournalPM.Tenant, _JournalPM.Id, _QMessageId);

                        AccountingStreamingBL(actions, myLedgerTransactionsWithCounters, allGLAccountTotalByMonths, reduceDeadlock);



                    }
#endif

                    if (DateTime.Now < new DateTime(2022, 06, 19) && stringBuilderWhyTransferCardBadBalance.Length > 0)
                    {
                        WriteLogWhyTransferCardBadBalance(_Tenant, _JournalPM.Id, this._AccountingContext, stringBuilderWhyTransferCardBadBalance);
                    }

                    CreateInterestTransactions(_JournalPM, _AccountingContext);
                    this._AccountingContext.SaveChanges();//due myJournalRepository.UpdateWhileStreaming 
                    scope.Complete();

                }
                catch (Exception e)
                {
					scope.Dispose();
					LogMessagingUtil.Instance.AppendLine($"AccountingStreamingInNewSerializableTransaction! _JournalPM?.Id={_JournalPM?.Id} | Exception: {e.Message}");
					NetCommonHelper.Logger.DevLog.Instance.WriteError("AccountingStreamingInNewSerializableTransaction! _JournalPM?.Id" + _JournalPM?.Id + " Err:" + e );
                    throw e;

                }
                finally
                {
                    //logger.ToString();
                    logger.Dispose();
                    ///if (reduceDeadlock)
                    {
                        //var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)_AccountingContext;
                        //var objectContext = adapter.ObjectContext;
                        //dc.ExecuteCommand("set deadlock_priority normal"); //normal prio
                    }

                    LogMessagingUtil.Instance.AppendLine("AccountingStreamingInNewSerializableTransaction:Took:" + sw.Elapsed.ToString());
                }

                
            }
        }
        public void CreateInterestTransactionsByDate(DateTime date)
        {
            _AccountingContext = AccountingContext.GetContext(_Tenant);
            var ListOfJournals = _AccountingContext.Journals.Where(j => j.ExternalSystem == "AMITAL" && j.DocumentDate >= date && j.CreateDate < date).ToList();
            foreach (var journal in ListOfJournals)
            {
                var journalQueryService = new JournalQueryService(_AccountingContext);
                _JournalPM = journalQueryService.GetSingle(journal.Id, true, false);
                CreateInterestTransactions(_JournalPM, _AccountingContext, true);
                this._AccountingContext.SaveChanges();

            }
        }
        private TransactionScope GetTransactionScope(TimeSpan? timeout)
        {
            if (FeatureToggleHelper.HasFeatureToggle("JAM", _Tenant))
                return TransactionFactory.GetNewReadCommittedTransaction(timeout);
            return TransactionFactory.GetNewSerializableTransaction(timeout);
        }

        private void WriteLogWhyTransferCardBadBalance(int tenant, string seedJournalId, IAccountingContext accountingContext, StringBuilder sb)
        {
            try
            {

                JournalFailedService journalFailedService = new JournalFailedService(tenant, seedJournalId);
                journalFailedService.InsertJournalMoreData(accountingContext, sb);
            }
            catch (Exception)
            {

                //throw;
            }


        }

        private void Impersonate()
        {
            try
            {
                var
                userIdentityNameb4 = AuthenticationUtil.ResolveUserIdentityName(_JournalPM.Tenant);
                if (HttpContext.Current != null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(_JournalPM.CreatedByUserId))
                {
                    LogMessagingUtil.Instance.AppendLine("no Impersonate");
                    return;
                }
                ContactRepository contactRep = new ContactRepository(_JournalPM.Tenant);
                var contact = contactRep.GetSingleContact(_JournalPM.CreatedByUserId, _JournalPM.Tenant);
                if (contact == null)
                {
                    LogMessagingUtil.Instance.AppendLine("no Impersonate contact == null");
                    return;
                }

                if (string.IsNullOrEmpty(contact.Email))
                {
                    LogMessagingUtil.Instance.AppendLine("no Impersonate");
                    return;
                }
                LogMessagingUtil.Instance.AppendLine($"Impersonate to {contact.Email}");
                AuthenticationUtil.Impersonate(_JournalPM.Tenant, contact.Email, "");

                var userIdentityNameafter = AuthenticationUtil.ResolveUserIdentityName(_JournalPM.Tenant);
                LogMessagingUtil.Instance.AppendLine($"Impersonate to {contact.Email}");

            }
            catch (Exception ee)
            {

                LogMessagingUtil.Instance.AppendLine($"no Impersonate Exception ee{ee.Message}");
            }

        }

        private void UpdateInExternalReconcileProgressToFalse()
        {
            var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);

            var listTransactionId = _JournalPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList();
            if (listTransactionId.Count > 0)
            {
                //LedgerTransactionUpdateService.Update_InProgressExternalReconcile(_JournalPM.Id, _JournalPM.Tenant, false);
                myLedgerTransactionUpdateService.Update_InProgressExternalReconcile(listTransactionId, _JournalPM.Tenant, false);

            }

            var listReconcileExternalPageLineId = _JournalPM.JournalExternalReconciles.Select(r => r.ReconcileExternalPageLineId).ToList();
            if (listReconcileExternalPageLineId.Count > 0)
            {
                var reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(_AccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _JournalPM.Tenant);
                reconcileExternalPageLineUpdateService.Update_InProgressExternalReconcile(listReconcileExternalPageLineId, _JournalPM.Tenant, false);
            }

        }

        private void UpdateInReconcileProgressToFalse()
        {
            var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);

            var listTransactionId = _JournalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
            if (listTransactionId.Count > 0)
            {
                myLedgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, _JournalPM.Tenant, false);
                //LedgerTransactionUpdateService.UpdateInReconcileProgress(_JournalPM.Id, _JournalPM.Tenant, false);
                _AccountingContext.SaveChanges();// >>VALIDATION SHOULD NOT FAIL
            }
        }

        private void UpdateJournalWithExternalReconcileNumber(CreateAutoExternalReconcileWhileStreamingService myCreateAutoExternalReconcileWhileStreamingService)
        {
            if (
                                        this._JournalPM.AccountingEntityCode == "12"// - Reconciliation
                                        ///OnCreate There is A fill ?!?!?  --- && String.IsNullOrWhiteSpace(this._JournalPM.AccountingEntityId)
                                        &&
                                        this._JournalPM.JournalExternalReconciles.Count > 0
                                        &&
                                        myCreateAutoExternalReconcileWhileStreamingService.ExternalReconciliationList.Count >= 1

                                        )
            {
                var myExternalReconciliation = myCreateAutoExternalReconcileWhileStreamingService.ExternalReconciliationList.Last();

                var myJournalUpdateService = new JournalUpdateService(this._AccountingContext, new Dictionary<string, IContext>(), this._JournalPM.Tenant);

                //var myJournalRepository = //new JournalRepository(this._AccountingContext);
                myJournalUpdateService.
            //.GetJournalRepositoryPriv();
            //myJournalRepository.
            UpdateWhileStreaming(this._JournalPM.Tenant, this._JournalPM.Id,
                (poco) =>
                {
                    this._JournalPM.AccountingEntityId = myExternalReconciliation.Id;
                    this._JournalPM.AccountingEntityReference = myExternalReconciliation.ReconciliationNumber.ToString();

                    poco.AccountingEntityReference = myExternalReconciliation.ReconciliationNumber.ToString();
                    poco.AccountingEntityId = myExternalReconciliation.Id;
                });

            }
        }
        private void UpdateJournalWithReconcileNumber(ICreateAutoReconcileWhileStreamingService myCreateAutoReconcileWhileStreamingService)
        {
            if (
                                        this._JournalPM.AccountingEntityCode == "10"// - Reconciliation
                                        ///OnCreate There is A fill ?!?!?  --- && String.IsNullOrWhiteSpace(this._JournalPM.AccountingEntityId)
                                        &&
                                        String.IsNullOrWhiteSpace(this._JournalPM.AccountingEntityReference)
                                        &&
                                        this._JournalPM.JournalReconciles.Count > 0
                                        &&
                                        myCreateAutoReconcileWhileStreamingService.ReconciliationList.Count == 1

                                        )
            {
                var myReconciliation = myCreateAutoReconcileWhileStreamingService.ReconciliationList.First();

                var myJournalUpdateService = new JournalUpdateService(this._AccountingContext, new Dictionary<string, IContext>(), this._JournalPM.Tenant);

                //var myJournalRepository = //new JournalRepository(this._AccountingContext);
                myJournalUpdateService.
            //.GetJournalRepositoryPriv();
            //myJournalRepository.
            UpdateWhileStreaming(this._JournalPM.Tenant, this._JournalPM.Id,
                (poco) =>
                {
                    this._JournalPM.AccountingEntityId = myReconciliation.Id;
                    this._JournalPM.AccountingEntityReference = myReconciliation.Number;

                    poco.AccountingEntityReference = myReconciliation.Number;
                    poco.AccountingEntityId = myReconciliation.Id;
                });

            }
        }


        private void UpdateTaxReportWithReconcileNumber(List<ReconciliationPM> reconciliationsList)
        {
            if (this._JournalPM.AccountingEntityCode == AccountingEntityValues.TaxReport && this._JournalPM.JournalReconciles.Count > 0 &&
                reconciliationsList.Count > 0)
            {
                var taxReportPM = GetTaxReportPM(_JournalPM.AccountingEntityId);


                var reconciliationNumbersList = reconciliationsList.Select(x => x.Number).ToList();
                taxReportPM.ReconciliationsNumbers = String.Join(",", reconciliationNumbersList);
                TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(this._AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
                taxReportPM.ChangeSetOp = ChangeSetOperation.Update;
                taxReportUpdateService.Update(taxReportPM, true);
            }

        }

        private TaxReportPM GetTaxReportPM(string taxReportId)
        {
            TaxReportQueryService taxReportQueryService = new TaxReportQueryService(_JournalPM.Tenant);
            return taxReportQueryService.GetSingle(taxReportId, false, false);
        }

        private static TimeSpan? GetTimeout(List<LedgerTransactionPM> myLedgerTransactionsWithCounters, bool HaveJournalReconciles)
        {
            //bool reduceDeadlock = false;

            int? iTimeOut = null;
            //if (reduceDeadlock)
            {
                //timeOut = TimeSpan.FromSeconds(2);
                iTimeOut = (600);
                //if (myLedgerTransactionsWithCounters.Count < 100)
                //{
                //    iTimeOut = (1);
                //}
                //else if (myLedgerTransactionsWithCounters.Count < 1000)
                //{
                //    iTimeOut = (5);
                //}
                //else if (myLedgerTransactionsWithCounters.Count < 10000)
                //{
                //    iTimeOut = (10);
                //}


                //if (!iTimeOut.HasValue)
                //{
                //    iTimeOut = (20);
                //}
                if (Debugger.IsAttached)
                {
                    //System.Diagnostics.Debugger.Break();
                    iTimeOut = (120);
                }
            }
            TimeSpan? timeOut = null;

            if (HaveJournalReconciles)
            {
                iTimeOut = iTimeOut + (5);
            }
            return TimeSpan.FromSeconds(iTimeOut.Value);
        }

        private static void ThrowException(string mess)
        {
            throw new ApplicationException(mess);
        }


        public virtual DateTime GetToday()
        {
            return DateTime.UtcNow.Date;
        }

        private void FillIdCountersUseNewDBTransaction(List<LedgerTransactionPM> myLedgerTransactionsWithOutCounters)
        {
            var sw = Stopwatch.StartNew();
            using (var newScope = GetTransactionScope(null)) /// inside IdCounter.GetNumber there is --- GetNewReadCommittedTransaction
            {
                foreach (var entityPM in myLedgerTransactionsWithOutCounters)
                {
                    if (String.IsNullOrWhiteSpace(entityPM.Id))
                    {
                        entityPM.Id = IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
                    }
                }
                newScope.Complete();
            }
            LogMessagingUtil.Instance.AppendLine($"FillIdCountersUseNewDBTransaction:took:{sw.Elapsed}");
        }

        private void FixLedgerTransaction()
        {
            throw new NotImplementedException();
        }


        public const bool FeatureJournalApproveInBatch = true;
        private System.ComponentModel.DataAnnotations.ValidationContext _JournalValidatorContext;
        private bool _ExecAsSP;
        private bool _done;

        //private List<GLAccountTotalByMonthPM> _ConnectedControlGLAccountTotalByMonths;


        public static void WorkWithoutQueueStatus4(int SeedTenant, string SeedJournalId, ref List<string> Last_journalBufferKeys)
        {
            var sw = new Stopwatch();

            List<string> journalBufferKeys = null;
            //List<string> Last_journalBufferKeys = null;
            sw.Restart();

            string logtext = "JournalApproveService.WorkWithoutQueueStatus4(), Point 2, SeedTenant " + SeedTenant.ToString();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);



            using (var scope = TransactionFactory.GetTransaction())
            // maybe to do GetNewSerializableTransaction Lock ?!?!?!
            {

                var journalQS = new JournalQueryService(SeedTenant);

#if true
                journalBufferKeys = journalQS
                    .GetJournalWhileStreamingHadErrorWithStatus4
                    (SeedTenant, SeedJournalId, Last_journalBufferKeys);
#else
                    journalPm =journalQS.GetSinglePendingApproved(this.SeedTenant.Value, true);
#endif
            }
            sw.Stop();
            if (sw.Elapsed > TimeSpan.FromSeconds(2))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Improve SQL Query Performance !!!");
            }
            if (journalBufferKeys == null)
            {

                Thread.Sleep(500);
                return;
            }
            if (journalBufferKeys.Count == 0)
            {

                Thread.Sleep(500);
                return;
            }
            Last_journalBufferKeys = new List<string>(journalBufferKeys);
            JournalApproveService.MyActions actions =
                JournalApproveService.MyActions.BuildLedgerTransaction | JournalApproveService.MyActions.BuildGLAccountTotalByMonths
                | JournalApproveService.MyActions.DueBugReStreamAllJournalAgain2Accounting;
            foreach (var journalId in journalBufferKeys)
            {


                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    IAccountingContext MyContext = AccountingContext.GetContext(SeedTenant);
                    var jqs = new JournalQueryService(MyContext);
                    var pm = jqs.GetSingle(journalId, true, false);
                    var jus = //new JournalUpdateService(SeedTenant);
    new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), SeedTenant);

                    pm.StatusCode = "6";
                    pm.ChangeSetOp = ChangeSetOperation.Update;
                    jus.Update(pm, true);
                    scope.Complete();
                }
                try
                {
                    var guid = Guid.NewGuid().GetHashCode().ToString();
                    var approveJournalService = new JournalApproveService(SeedTenant, journalId, guid, K_AccountingJournalApproveWR);
                    approveJournalService.SubmitApprove(actions);
                }
                catch (Exception eee)
                {
					LogMessagingUtil.Instance.AppendLine($"WorkWithoutQueueStatus4 | Exception: {eee.Message}");

					OnException(null, null, journalId, SeedTenant, eee);
                    //LogMessagingUtil.Instance.AppendLine(journalId.ToString() + " " + eee.Message);
                    //ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "JournalApproveWorkerRole", "approveJournalService.SubmitApprove", null);
                    Thread.Sleep(500);

                    //throw;
                }

            }




        }

        public static void WorkWithoutQueue(int SeedTenant, string SeedJournalId, ref List<string> Last_journalBufferKeys)
        {
            var sw = new Stopwatch();

            List<string> journalBufferKeys = null;
            //List<string> Last_journalBufferKeys = null;
            sw.Restart();


            string logtext = "JournalApproveService.WorkWithoutQueue(), Point 2, SeedTenant " + SeedTenant.ToString();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);



            using (var scope = TransactionFactory.GetTransaction())
            // maybe to do GetNewSerializableTransaction Lock ?!?!?!
            {

                var journalQS = new JournalQueryService(SeedTenant);

#if true
                journalBufferKeys = journalQS
                    .GetApprovedJournalWithoutLedgerTransaction
                    (SeedTenant, SeedJournalId, Last_journalBufferKeys);
#else
                    journalPm =journalQS.GetSinglePendingApproved(this.SeedTenant.Value, true);
#endif
            }
            sw.Stop();
            if (sw.Elapsed > TimeSpan.FromSeconds(2))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Improve SQL Query Performance !!!");
            }
            if (journalBufferKeys == null)
            {

                Thread.Sleep(500);
                return;
            }
            if (journalBufferKeys.Count == 0)
            {

                Thread.Sleep(500);
                return;
            }
            Last_journalBufferKeys = new List<string>(journalBufferKeys);
            JournalApproveService.MyActions actions =
                JournalApproveService.MyActions.BuildLedgerTransaction | JournalApproveService.MyActions.BuildGLAccountTotalByMonths
                | JournalApproveService.MyActions.DueBugReStreamAllJournalAgain2Accounting;
            foreach (var journalId in journalBufferKeys)
            {
                try
                {
                    var guid = Guid.NewGuid().GetHashCode().ToString();
                    var approveJournalService = new JournalApproveService(SeedTenant, journalId, guid, K_AccountingJournalApproveWR);
                    approveJournalService.SubmitApprove(actions);
                }
                catch (Exception eee)
                {
					LogMessagingUtil.Instance.AppendLine($"WorkWithoutQueue | Exception: {eee.Message}");

					OnException(null, null, journalId, SeedTenant, eee);
                    //LogMessagingUtil.Instance.AppendLine(journalId.ToString() + " " + eee.Message);
                    //ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "JournalApproveWorkerRole", "approveJournalService.SubmitApprove", null);
                    Thread.Sleep(500);

                    //throw;
                }

            }




        }
        public static void ReturnToQueue(int SeedTenant, bool AllTenants)
        {
            var sw = new Stopwatch();

            List<JournalPM> waitingJournal = null;
            //List<string> Last_journalBufferKeys = null;
            sw.Restart();
            using (var scope = TransactionFactory.GetTransaction())
            // maybe to do GetNewSerializableTransaction Lock ?!?!?!
            {

                var journalQS = new JournalQueryService(SeedTenant);

                waitingJournal = journalQS.GetJournalByTenant(SeedTenant, AllTenants).ToList();

            }
            sw.Stop();
            if (sw.Elapsed > TimeSpan.FromSeconds(2))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Improve SQL Query Performance !!!");
            }
            if (waitingJournal == null)
            {

                Thread.Sleep(500);
                return;
            }
            if (waitingJournal.Count == 0)
            {

                Thread.Sleep(500);
                return;
            }
            waitingJournal = new List<JournalPM>(waitingJournal);
            foreach (var journal in waitingJournal)
            {
                try
                {
                    if (journal.QueueId != null)
                    {
                        QueueMessageRepository QueueMessageRepository = new QueueMessageRepository(SeedTenant);
                        var status = QueueMessageRepository.GetSingleQueueMessage(journal.QueueId)?.Status;
                        if (status != 1)
                            continue;
                    }


                    JournalPM pm = null;
                    JournalApproveService.MyActions actions =
    JournalApproveService.MyActions.BuildLedgerTransaction | JournalApproveService.MyActions.BuildGLAccountTotalByMonths
    | JournalApproveService.MyActions.DueBugReStreamAllJournalAgain2Accounting;
                    string journalId = journal.Id;

                    using (var scope2 = TransactionFactory.GetNewTransaction())
                    {
                        IAccountingContext MyContext = AccountingContext.GetContext(SeedTenant);
                        var jqs = new JournalQueryService(MyContext);
                        pm = jqs.GetSingle(journalId, true, false);
                        var jus = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), SeedTenant);

                        try
                        {
                            if (pm.StatusCode != "6")
                            {
                                pm.StatusCode = "6";
                                pm.ChangeSetOp = ChangeSetOperation.Update;
                                jus.Update(pm, true);
                            }


                            if (FeatureToggleHelper.HasFeatureToggle("JAM", pm.Tenant))
                                JournalApproveService.EnqueueMultiThreadedDB(pm);
                            else
                                JournalApproveService.EnqueueDB(pm);
                            if (scope2 != null)
                                scope2.Complete();
                        }
                        catch (Exception eee2)
                        {
							LogMessagingUtil.Instance.AppendLine($"ReturnToQueue journalId= {journalId.ToString()} | Exception: {eee2.Message}");
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(eee2, journalId.ToString());

                            Logitude.SystemLogs.ExceptionHandler.HandleException(eee2, DateTime.Now, 0, "", "", "JournalApproveService.ReturnToQueue()" + eee2.Message, null);

                            Thread.Sleep(100);

                            //throw;
                            //}
                        }
                        finally
                        {
                            if (scope2 != null)
                                scope2.Dispose();
                        }
                        //}

                    }
                }
                catch (Exception eee)
                {
					LogMessagingUtil.Instance.AppendLine($"ReturnToQueue2 journalId= {journal?.Id} | Exception: {eee.Message}");

					OnException(null, null, journal?.Id, SeedTenant, eee);
                    //LogMessagingUtil.Instance.AppendLine(journalId.ToString() + " " + eee.Message);
                    //ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "JournalApproveWorkerRole", "approveJournalService.SubmitApprove", null);
                    Thread.Sleep(500);

                    //throw;
                }

            }




        }

        [Flags]
        public enum MyActions
        {
            None = 0,
            BuildLedgerTransaction = 1,
            FixLedgerTransaction = 2,
            BuildGLAccountTotalByMonths = 4,
            DueBugReStreamAllJournalAgain2Accounting = 8
        }

        public static QueueClient CreateNewQueueClient(string myClass)
        {
            QueueDescription _QueueDescription;
            QueueClient _QueueClient;
            string emailQueueName = WebFreightEntryPoint.GetQueueByEnviroment(myClass); //Amitalqueue

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
            {
                _QueueDescription = new QueueDescription(emailQueueName);
                _QueueDescription.MaxSizeInMegabytes = 5120;
                _QueueDescription.LockDuration = TimeSpan.FromMinutes(5);//due debug + raise after 5 min !!
                //_QueueDescription.MaxDeliveryCount = 100;
                _QueueDescription.MaxDeliveryCount = 20;

                StorageAcountDetails.NameSpaceManager.CreateQueue(_QueueDescription);
            }



            _QueueClient = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            return _QueueClient;
        }


        public static void EnqueueDB(JournalPM entityPM)
        {
            var queueService = new DbQueueService();
            string queueCode = JournalApproveService.K_AccountingJournalApproveWR;
            if (entityPM.ConversionJournal)
            {
                queueCode = JournalApproveService.K_AccountingConversionJournalApproveWR;

            }
            queueService.InitializeQueue(queueCode, entityPM.Tenant);
            Dictionary<string, string> messageProperties = new Dictionary<string, string>();

            messageProperties.Add(QP_JournalTenant, entityPM.Tenant.ToString());
            messageProperties.Add(QP_JournalId, entityPM.Id);
            try
            {

                var test = false;
                if (test)
                {
                    ThrowException("test");
                }
                //queueClient.Send(mQueue);
                queueService.Send(messageProperties, entityPM.Tenant);
            }
            catch (Exception e)
            {
				LogMessagingUtil.Instance.AppendLine($"EnqueueDB | Exception: {e.Message}");

				Logitude.SystemLogs.ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "", "QueueSendService.Send()" + messageProperties.ToString(), null);
                throw;
            }
        }

        public static void EnqueueMultiThreadedDB(JournalPM entityPM)
        {
            var queueService = new DbQueueService();
            string queueCode = JournalApproveService.K_AccountingJournalApproveMutliThreadingWR;
            queueService.InitializeQueue(queueCode, entityPM.Tenant);
            Dictionary<string, string> messageProperties = new Dictionary<string, string>();

            messageProperties.Add(QP_JournalTenant, entityPM.Tenant.ToString());
            messageProperties.Add(QP_JournalId, entityPM.Id);
            try
            {
                queueService.Send(messageProperties, entityPM.Tenant);
            }
            catch (Exception e)
            {
				LogMessagingUtil.Instance.AppendLine($"EnqueueMultiThreadedDB | Exception: {e.Message}");

				Logitude.SystemLogs.ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "", "QueueSendService.Send()" + messageProperties.ToString(), null);
                throw;
            }
        }

        //private static void ProcessMessage(BrokeredMessage message)
        private static bool ProcessMessage_Db(DbQueueService myDbQueueService, QueueResponse message, string selectedQueue)
        {
            string MessageId = "";
            int tenant = -1;
            string qpJournalId = null;
            bool isSubmitApprove = false;
            try
            {



                int.TryParse(message.MessageValues[QP_JournalTenant].ToString(), out tenant);
                if (tenant == -1)
                {
                    ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "CustomsMessagingSheetWR: ProcessMessage() Method :tenant==-1", null);
                    return isSubmitApprove;
                }


                qpJournalId = message.MessageValues[QP_JournalId].ToString();
                MessageId = message.MessageId;
                LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.RetryNumber.ToString());


                JournalApproveService.MyActions actions =
            JournalApproveService.MyActions.BuildLedgerTransaction | JournalApproveService.MyActions.BuildGLAccountTotalByMonths;
                var myJournalApproveService = new JournalApproveService(tenant, qpJournalId, MessageId, selectedQueue);
                var res = myJournalApproveService.SubmitApprove(actions);

                if (res.Success)
                {

                    myDbQueueService.Complete();
                    isSubmitApprove = true;
                }
                else
                {
                    var ex1 = new Exception("AccountingJournalApproveWR.JournalApproveService(" + tenant.ToString() + "," + MessageId.ToString() + ").SubmitApprove():FailDue=" + res.FailDue);
                    //ExceptionHandler.HandleException(ex1, DateTime.Now, 0, "", "WorkerRole", "AccountingJournalApproveWR: ProcessMessage() Method", null);
                    OnException(myDbQueueService, message, qpJournalId, tenant, ex1);
                }




            }
            catch (JournalApproveException ex)
            {
				LogMessagingUtil.Instance.AppendLine($"JournalApproveException {qpJournalId} | Exception: {ex.Message}");

				switch (ex.WhatTODO)
                {

                    case WhatTODOJournalApproveEnum.ClearQueue:
                        LogMessagingUtil.Instance.AppendLine("WhatTODOJournalApproveEnum.ClearQueue:" + MessageId.ToString() + " " + ex.Message);
                        myDbQueueService.Complete();
                        break;
                    case WhatTODOJournalApproveEnum.MakeItFailed:
                    default:
                        {
                            OnException(myDbQueueService, message, qpJournalId, tenant, ex);
                        }

                        break;
                }
                
            }
            catch (Exception ex)
            {
				LogMessagingUtil.Instance.AppendLine($"ProcessMessage_Db {qpJournalId} | Exception: {ex.Message}");

				OnException(myDbQueueService, message, qpJournalId, tenant, ex);
            }
            return isSubmitApprove;

        }

        private static void SetTenantIdle(int tenant)
        {
            TenantIdleStatusRepository tenantRepository = new TenantIdleStatusRepository(tenant);
            TenantIdleStatus tenantObj = tenantRepository.GetAllByObjectTable(tenant,  "Journal").FirstOrDefault();
            tenantObj.Idle = false;
            tenantObj.UpdateDate = DateTime.Now;
            tenantRepository.Update(tenantObj);
            tenantRepository.SubmitChanges();

        }

        private static void UpdateGLAccountAgingData(string communicationLogId, DbQueueService queueservice, int tenant)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
            if (commLog != null)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = commLog.Document.Id,
                            FolderName = commLog.Document.Folder,
                            Extension = commLog.Document.Extension,
                            Tenant = commLog.Document.Tenant,
                            FileSize = commLog.Document.FileSize,
                        };
                        Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                        byte[] objectData = storageservice.Read(fileInfo);
                        var jsonObject = System.Text.Encoding.Default.GetString(objectData);
                        var reconciliations = JsonConvert.DeserializeObject<List<ReconciliationPM>>(jsonObject);

                        var accountingContext = AccountingContext.GetContext(tenant);
                        ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                        foreach (var item in reconciliations)
                        {
                            service.UpdateGLaccountAgingData(item);
                        }

                        commLog.CommunicationStatusTypeCode = "D";
                        commLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.DoneDateUTC = DateTime.UtcNow;
                        commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(commLog.Tenant);
                        commLog.LastStatusDateUTC = DateTime.UtcNow;
                        communicationLogRep.Update(commLog);
                        communicationLogRep.SubmitChanges();
                        scope.Complete();
                    }
                    queueservice.Complete();
                }
                catch (Exception ex)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, "F", null + DateTime.Now.ToString(), ex.Message);
                    queueservice.CompleteAsFailed();
                }
            }
        }


        private static void OnException(DbQueueService myDbQueueService, QueueResponse message, string seedJournalId, int tenant, Exception ex)
        {

            LogMessagingUtil.Instance.AppendLine(message?.MessageId?.ToString() + " " + ex.ToString());
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "AccountingJournalApproveWR", "AccountingJournalApproveWR: ProcessMessage() Method", null);
            if (message.RetryNumber >= 2 && message.RetryNumber <= 7)
            {
                myDbQueueService.Delay(new TimeSpan(0, 0, 0, 50));
            }
            if (message == null || message.RetryNumber > 6)
            {
                var journalFailedService = new JournalFailedService(tenant, seedJournalId);
                journalFailedService.MarkAsFailed(ex );
                if (myDbQueueService != null)
                {
                    myDbQueueService.Complete();
                }
                var accountingContext = AccountingContext.GetContext(tenant);
               var journalQueryService = new JournalQueryService(accountingContext);
                journalQueryService.FixFailedReconcileJournals(tenant);

            }
        }
        /// <summary>
        /// Task 52385: Journals & Transactions - after the Journal transform to LedgerTransaction >Update field IsLedgerCreated = True in Journals
        /// </summary>
        /// <param name="myLedgerTransactionsWithCounters"></param>
        /// <param name="allGLAccountTotalByMonths"></param>
        private void Exec_usp_AccountingStreaming(List<LedgerTransactionPM> myLedgerTransactionsWithCounters, List<GLAccountTotalByMonthPM> allGLAccountTotalByMonths, List<GLAccountAgingDataPM> gLAccountAgingDataPMs)
        {

            //_JournalPM.Tenant, _JournalPM.Id, _QMessageId

            var stringBuilder = new StringBuilder();
            bool complete = false;
            try
            {


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    string strConnString = TenantServerConfigration.GetDbConnection(_Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {
                        throw new ApplicationException();
                    }

                    using (SqlConnection myConnection = new SqlConnection(strConnString))
                    {


                        myConnection.InfoMessage += (sender, e) =>
                        {
                            stringBuilder.AppendLine(e.Message);
                        };


                        SqlCommand cmd = new SqlCommand("[dbo].[usp_AccountingStreaming]", myConnection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter journalIdPar = new SqlParameter("@pJournalId", SqlDbType.VarChar);
                        journalIdPar.Direction = ParameterDirection.Input;
                        journalIdPar.Value = _JournalPM.Id;

                        SqlParameter pTenantPar = new SqlParameter("@pTenant", SqlDbType.Int);
                        pTenantPar.Direction = ParameterDirection.Input;
                        pTenantPar.Value = _JournalPM.Tenant;


                        SqlParameter messageIdPar = new SqlParameter("@pQMessageId", SqlDbType.VarChar);
                        messageIdPar.Direction = ParameterDirection.Input;
                        messageIdPar.Value = _QMessageId;

                        var DBTypeLedgerTransactionsWithCounters = myLedgerTransactionsWithCounters
                            .Select(r =>
                          new DBTypeLedgerTransaction()
                          {
                              //Id = r.JournalId,

                              Tenant = r.Tenant,
                              JournalId = r.JournalId,

                              JournalLineNumber = r.JournalLineNumber,
                              Id = r.Id,

                              CreateDate = r.CreateDate ?? DateTime.MinValue,
                              ControlAccountId = r.ControlAccountId,
                              AccountId = r.AccountId,

                              DocumentDate = r.DocumentDate,
                              DueDate = r.DueDate,
                              AccountingDate = r.AccountingDate,
                              LocalAmountDebit = r.LocalAmountDebit,
                              LocalAmountCredit = r.LocalAmountCredit,
                              CurrencyId = r.CurrencyId,
                              ForeignAmountDebit = r.ForeignAmountDebit,
                              ForeignAmountCredit = r.ForeignAmountCredit,
                              ExchangeRate = r.ExchangeRate,
                              Reference1 = r.Reference1,
                              Reference2 = r.Reference2,
                              Reference3 = r.Reference3,
                              OpenAmount = r.OpenAmount,

                              OppositeAccountId = r.OppositeAccountId,
                              SearchFields = r.SearchFields,
                              OpenAmountCurrencyId = r.OpenAmountCurrencyId,
                              Notes = r.Notes,
                              AmountToReconcile = r.AmountToReconcile,

                              Mark = r.Mark,
                              //IsReconciled = 
                              //(this._SelectedQueue == K_AccountingJournalApproveWR && r.OpenAmount == 0) 
                              //? true : r.IsReconciled,
                              IsReconciled = r.IsReconciled,
                              IsExternalReconcile = r.IsExternalReconcile
                          }
                    ).ToList();

                        var tableLTRans = DBTypeLedgerTransactionsWithCounters.ToDataTable();

                        SqlParameter tLedgerTransactionsTypePar = new SqlParameter("@tLedgerTransactionsType", SqlDbType.Structured);
                        tLedgerTransactionsTypePar.Direction = ParameterDirection.Input;
                        tLedgerTransactionsTypePar.Value = tableLTRans;

                        var listGLAccountTotalByMonth =
                            allGLAccountTotalByMonths.Select(r => new DBTypeAccountTotalByMonth()
                            {
                                AccountId = r.AccountId,
                                DateTypeCode = r.DateTypeCode,
                                Year = r.Year,
                                Month = r.Month,
                                CurrencyId = r.CurrencyId,
                                Tenant = r.Tenant,
                                LocalAmountCredit = r.LocalAmountCredit,
                                LocalAmountDebit = r.LocalAmountDebit,
                                ForeignAmountCredit = r.ForeignAmountCredit,
                                ForeignAmountDebit = r.ForeignAmountDebit,


                            }).ToList();
                        var tableGLAccountTotalByMonths = listGLAccountTotalByMonth.ToDataTable();

                        SqlParameter tGLAccountTotalByMonthsTypePar = new SqlParameter("@tGLAccountTotalByMonthsType", SqlDbType.Structured);
                        tGLAccountTotalByMonthsTypePar.Direction = ParameterDirection.Input;
                        tGLAccountTotalByMonthsTypePar.Value = tableGLAccountTotalByMonths;
                        //tLedgerTransactionsTypePar.




                        SqlParameter tGLAccountAgingDataType = GettGLAccountAgingDataType(gLAccountAgingDataPMs);

                        cmd.Parameters.Add(journalIdPar);
                        cmd.Parameters.Add(pTenantPar);

                        cmd.Parameters.Add(messageIdPar);
                        cmd.Parameters.Add(tGLAccountTotalByMonthsTypePar);
                        cmd.Parameters.Add(tLedgerTransactionsTypePar);
                        cmd.Parameters.Add(tGLAccountAgingDataType);






                        myConnection.Open();
                        var output = cmd.ExecuteNonQuery();
                        myConnection.Close();


                    }


                    scope.Complete();
                    complete = true;
                }
            }
            catch (Exception ex)
            {
				LogMessagingUtil.Instance.AppendLine($"usp_AccountingStreaming AccountingStreamingInNewSerializableTransaction! _JournalPM?.Id {_JournalPM?.Id} | Err: {ex.Message}");

				NetCommonHelper.Logger.DevLog.Instance.WriteError(" usp_AccountingStreaming AccountingStreamingInNewSerializableTransaction! _JournalPM?.Id" + _JournalPM?.Id + " Err:" + ex);
                if (_JournalPM?.StatusCode == "6" && !_JournalPM.IsLedgerCreated)
                {
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            var up = new JournalUpdateService(_AccountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), _Tenant);
                            up.SetStatusCodeFailed(_SeedJournalId, _Tenant);
                            scope.Complete();
                        }
                        var journalQueryService = new JournalQueryService(_AccountingContext);
                        journalQueryService.FixFailedReconcileJournals(_Tenant);


                    }
                    catch (Exception updateEx)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError("Failed to update journal status in exception handling. JournalId: " + _JournalPM?.Id + " Err:" + updateEx);
                    }
                }
            }
            finally
            {

                if (!complete)
                {
                    LogMessagingUtil.Instance.AppendLine("Not streamed !!");
                    LogMessagingUtil.Instance.AppendLine(stringBuilder.ToString());
                }
                else
                {

                    this.CreateJournalAdditionalDataWhenApprovingJournal(this._JournalPM);

                }

            }

        }
        private void CreateJournalAdditionalDataWhenApprovingJournal(JournalPM journal)
        {
            CreateJournalAdditionalDataForEachDebitInputLine(journal);
            CreateJournalAdditionalDataForARInvoiceJournal(journal);
        }
        private void CreateJournalAdditionalDataForEachDebitInputLine(JournalPM journal)
        {
            if (journal.AccountingEntityCode != JournalAccountingEntities.ARInvoice && (String.IsNullOrEmpty(journal.ExternalSystem) || journal.ExternalSystem != "AMITAL"))
            {
                List<JournalLinePM> jourlDebitInputLines = SelectJournalDebitLinesFromJournalLines(journal);
                foreach (JournalLinePM journalLine in jourlDebitInputLines)
                {
                    JournalAdditionalDataPM journalAdditionalDataPM = MapJournalAdditionalDataFields(journalLine, journal);
                    if (!this.CheckIfExistInDb(journalAdditionalDataPM.JournalId, journalAdditionalDataPM.JournalLineNumber, journalAdditionalDataPM.Tenant))
                    {
                        SaveJournalAdditionalData(journalAdditionalDataPM);
                    }

                }
            }
        }
        private Boolean CheckIfExistInDb(string journalId, int JournalLineNumber, int tenant)
        {
            JournalAdditionalDataQueryService journalAdditionalDataQueryService = new JournalAdditionalDataQueryService(tenant);
            return journalAdditionalDataQueryService.CheckIfJournalAdditionalDataExist(journalId, JournalLineNumber, tenant);
        }
        private void CreateJournalAdditionalDataForARInvoiceJournal(JournalPM journal)
        {
            if (journal.AccountingEntityCode == JournalAccountingEntities.ARInvoice && (String.IsNullOrEmpty(journal.ExternalSystem) || journal.ExternalSystem != "AMITAL"))
            {
                JournalAdditionalDataPM journalAdditionalDataPM = MapJournalAdditionalDataFields(null, journal);
                if (!this.CheckIfExistInDb(journalAdditionalDataPM.JournalId, journalAdditionalDataPM.JournalLineNumber, journalAdditionalDataPM.Tenant))
                {
                    SaveJournalAdditionalData(journalAdditionalDataPM);
                }
            }
        }
        private void SaveJournalAdditionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(journalAdditionalDataPM.Tenant);
            JournalAdditionalDataUpdateService additionalDataUpdateService = new JournalAdditionalDataUpdateService((IAccountingContext)MyContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            additionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }
        private List<JournalLinePM> SelectJournalDebitLinesFromJournalLines(JournalPM journal)
        {
            FullAccountingSettingPM setting = GetFullAccountingSetting(journal.Tenant);

            return journal.JournalLines.Where(d => d.ActionTypeCode == JournalActionTypes.Debit && d.DebitAccountId == setting.VATInputsGLAccountId).ToList();
        }

        private JournalAdditionalDataPM MapJournalAdditionalDataFields(JournalLinePM journalLine, JournalPM journal)
        {
            return new JournalAdditionalDataPM()
            {
                JournalId = journal.Id,
                ChangeSetOp = ChangeSetOperation.Insert,
                TaxReportId = null,
                TaxReportTransmitStatusCode = null,
                Tenant = journal.Tenant,
                JournalLineNumber = journalLine != null ? journalLine.Line : 1,
            };
        }
        private FullAccountingSettingPM GetFullAccountingSetting(int tenant)
        {
            FullAccountingSettingQueryService settingQueryService = new FullAccountingSettingQueryService(tenant);
            return settingQueryService.GetSingleFullAccountingSetting(tenant);
        }
        private static SqlParameter GettGLAccountAgingDataType(List<GLAccountAgingDataPM> gLAccountAgingDataPMs)
        {
            var listDBTypeGLAccountAgingData =
                gLAccountAgingDataPMs.Select(r => new DBTypeGLAccountAgingData()
                {
                    AccountId = r.AccountId,
                    Tenant = r.Tenant,
                    PeriodPast = r.PeriodPast.GetValueOrDefault(),
                    Period0 = r.Period0.GetValueOrDefault(),
                    Period1 = r.Period1.GetValueOrDefault(),
                    Period2 = r.Period2.GetValueOrDefault(),
                    Period3 = r.Period3.GetValueOrDefault(),
                    Period4 = r.Period4.GetValueOrDefault(),
                    Period5 = r.Period5.GetValueOrDefault(),
                    //Period6 = r.Period6,
                    PeriodFuture = r.PeriodFuture.GetValueOrDefault(),
                    TotalOpenTransactions = r.TotalOpenTransactions.GetValueOrDefault()



                }).ToList();
            var tablelistDBTypeGLAccountAgingData = listDBTypeGLAccountAgingData.ToDataTable();
            SqlParameter tGLAccountAgingDataType = new SqlParameter("@tGLAccountAgingDataType", SqlDbType.Structured);
            tGLAccountAgingDataType.Direction = ParameterDirection.Input;
            tGLAccountAgingDataType.Value = tablelistDBTypeGLAccountAgingData;
            return tGLAccountAgingDataType;
        }


        public class JournalApproveWorker
        {
            private static DateTime _NextDueDoneAt = DateTime.MinValue;
            private static Dictionary<int, DateTime> _NextDueDoneDict = new Dictionary<int, DateTime>();
            private static DateTime _freeTenantsDateTime = DateTime.Now;
            static JournalApproveWorker()
            {
                //_NextDueDoneAt = DateTime.UtcNow.Date.AddDays(1);//tomorrow at 00:00
                _NextDueDoneAt = DateTime.UtcNow.Date;//today already done - do next day =tomorrow at 00:00 ///
            }
            public Action<int> LogDoneItemInMemoryAction { get; set; }
            public Action SetLastActivate { get; set; }

            public void WorkUntilQEmptyQueueDB(TimeSpan? timeSpan = null, string selectedQueue = null)
            {

                string[] stacklines = JournalApproveWorker.GetStack(0);
                string logtext = "JournalApproveService.cs JournalApproveWorker.WorkUntilQEmptyQueueDB(), Point 1, selectedQueue " + selectedQueue;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(JsonConvert.SerializeObject(stacklines));

                selectedQueue = selectedQueue ?? JournalApproveService.K_AccountingJournalApproveWR;
                Stopwatch stopwatch = null;
                if (timeSpan != null)
                {
                    stopwatch = Stopwatch.StartNew();
                }

                QueueResponse response = null;
                // 1. Main Loop Start 
                while (true)
                {
                    if (stopwatch != null && timeSpan != null)
                    {
                        if (stopwatch.Elapsed > timeSpan)
                        {
                            return;
                        }
                    }


                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("JournalApprove inloop selected queue: " + selectedQueue
                        + ", workerRoleName: " + LogitudeSettings.WorkerRoleName
                        + ", _NextDueDoneAt.Date" + _NextDueDoneAt.Date.ToString()
                        + ", _NextDueDoneAt.Time" + _NextDueDoneAt.TimeOfDay.ToString());

                    DbQueueService queueservice = null;
                    //ThrowNewException("BrokeredMessage receivedMessage = _QueueClient.Receive(TimeSpan.FromSeconds(5));");
                    try
                    {
                        queueservice = new DbQueueService(selectedQueue, 0);
                        response = queueservice.Receive(new TimeSpan(0, 0, 0, 5));
                    }
                    catch (Exception) // in the main loop
                    {

                        throw;
                    }


                    if (response == null || (response != null && response.MessageId == null))
                    {
                        break;
                    }
                    CheckCreateIntegrity();

                    if (response.MessageValues.ContainsKey("communicationLogId"))
                    {
                        string communicationLogId = response.MessageValues["communicationLogId"].ToString();
                        int tenant = 0;
                        int.TryParse(response.MessageValues["tenant"].ToString(), out tenant);
                        UpdateGLAccountAgingData(communicationLogId, queueservice, tenant);
                    }
                    else
                    {
                        SetLastActivate?.Invoke();
                        if (ProcessMessage_Db(queueservice, response, selectedQueue))
                        {
                            LogDoneItemInMemoryAction?.Invoke(1);

                        }

                    }
                    Thread.Sleep(10);//itzik - let other thread abilty to use GLAccout !!!
                }
                // Main Loop Finish 


                if (DateTime.UtcNow.Date > _NextDueDoneAt.Date) // 2. Checking and logging after the main loop 
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("JournalApprove beforeAddBatchTask selected queue: " + selectedQueue
                          + ", workerRoleName: " + LogitudeSettings.WorkerRoleName
                          + ",time" + DateTime.Now.ToString());
                }

                // 3. Queue selector
                if (selectedQueue == JournalApproveService.K_AccountingJournalApproveMutliThreadingWR)
                {

                    try
                    {
                        string workerRoleName = "";
                        if (!string.IsNullOrEmpty(LogitudeSettings.WorkerRoleName))
                        {
                            workerRoleName = LogitudeSettings.WorkerRoleName;


                            if (DateTime.UtcNow.Date > _NextDueDoneAt.Date && workerRoleName != "staging")// _NextDueDoneAt DateTime.UtcNow.TimeOfDay < TimeSpan.FromHours(6) ) 
                            {
                                if (DateTime.Now < new DateTime(2050, 06, 01))
                                {
                                    CreateBatchAccountingIntegrityCheck();
                                }
                                _NextDueDoneAt = DateTime.UtcNow.Date;
                                var myDueLocalBalanceService = new DueLocalBalanceService();
                                myDueLocalBalanceService.RunAllTenants();

                                var dailyRebuildAgingService = new DailyRebuildAgingService();
                                dailyRebuildAgingService.RunAllAgingTenants();

                            }
                        }
                    }
                    catch (Exception) // In the queue selector
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug("JornalApprove beforeAddBatchTask selected queue: {0}, workerRoleName: {1}  , time:{2} ",
                           null, selectedQueue, LogitudeSettings.WorkerRoleName, DateTime.Now);

                    }

                }
            }


                private static string[] GetStack(int removeLines)
                {
                    string[] stack = Environment.StackTrace.Split(
                        new string[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (stack.Length <= removeLines)
                        return new string[0];

                    string[] actualResult = new string[stack.Length - removeLines];
                    for (int i = removeLines; i < stack.Length; i++)
                        // Remove 6 characters (e.g. "  at ") from the beginning of the line
                        // This might be different for other languages and platforms
                        actualResult[i - removeLines] = stack[i].Substring(6);

                    return actualResult;
                }
                public void WorkUntilQEmptyQueueDBMultiThreaded(TimeSpan? timeSpan = null, string selectedQueue = null)
                {

                    string[] stacklines = JournalApproveWorker.GetStack(0);
                    string logtext = "JournalApproveService.cs JournalApproveWorker.WorkUntilQEmptyQueueDBMultiThreaded(), Point 1, selectedQueue " + selectedQueue;
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(logtext);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(JsonConvert.SerializeObject(stacklines));

                    selectedQueue = selectedQueue ?? JournalApproveService.K_AccountingJournalApproveMutliThreadingWR;
                    Stopwatch stopwatch = null;
                    if (timeSpan != null)
                    {
                        stopwatch = Stopwatch.StartNew();
                    }

                    QueueResponse response = null;
                    while (true)
                    {

                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug("WorkUntilQEmptyQueueDBMultiThreaded inloop selected queue: " + selectedQueue
        + ", workerRoleName: " + LogitudeSettings.WorkerRoleName
        + ", _NextDueDoneAt.Date" + _NextDueDoneAt.Date.ToString()
        + ", _NextDueDoneAt.Time" + _NextDueDoneAt.TimeOfDay.ToString());

                        if (stopwatch != null && timeSpan != null)
                        {
                            if (stopwatch.Elapsed > timeSpan)
                            {
                                return;
                            }
                        }
                        DbQueueService queueservice = null;
                        try
                        {
                            queueservice = new DbQueueService(selectedQueue, 0);
                            if (DateTime.Now.Subtract(_freeTenantsDateTime) >= TimeSpan.FromMinutes(10))
                            {
                                _freeTenantsDateTime = DateTime.Now;

                            queueservice.FreeTenants("Journal");
                            }

                        response = queueservice.ReceiveDetailsByTenant("Journal",new TimeSpan(0, 0, 0, 5));
                        }
                        catch (Exception)
                        {

                            throw;
                        }


                        if (response == null || (response != null && response.MessageId == null))
                        {
                            break;
                        }

                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 0, tenant {0}, selectedQueue {1}", response.Tenant, selectedQueue));

                        if (selectedQueue == JournalApproveService.K_AccountingJournalApproveMutliThreadingWR
                            && response != null && response.Tenant != 0)
                        {
                            try
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 1, tenant {0}", response.Tenant));

                                if (_NextDueDoneDict == null) _NextDueDoneDict = new Dictionary<int, DateTime>();
                                if (!_NextDueDoneDict.ContainsKey(response.Tenant))
                                    _NextDueDoneDict.Add(response.Tenant, DateTime.MinValue);

                                if (DateTime.UtcNow.Date > _NextDueDoneDict[response.Tenant].Date)
                                {
                                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 2, tenant {0}, date {1} ", response.Tenant, _NextDueDoneDict[response.Tenant].Date));

                                    _NextDueDoneDict[response.Tenant] = DateTime.UtcNow.Date;
                                    var myDueLocalBalanceService = new DueLocalBalanceService();
                                    myDueLocalBalanceService.RunOneTenantFast(response.Tenant);
                                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 3, tenant {0}", response.Tenant));
                                }
                            }
                            catch (Exception)
                            {
                                SetTenantIdle(response.Tenant);
                                throw;
                            }
                        }
                        CheckCreateIntegrity();


                        if (response.MessageValues.ContainsKey("communicationLogId"))
                        {
                            string communicationLogId = response.MessageValues["communicationLogId"].ToString();
                            int tenant = 0;
                            int.TryParse(response.MessageValues["tenant"].ToString(), out tenant);
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 4, tenant {0}", response.Tenant));
                            UpdateGLAccountAgingData(communicationLogId, queueservice, tenant);
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(String.Format("JournalApproveService, Point 5, tenant {0}", response.Tenant));
                            SetTenantIdle(response.Tenant);
                        }
                        else
                        {
                            SetLastActivate?.Invoke();
                            if (ProcessMessage_Db(queueservice, response, selectedQueue))
                            {
                                LogDoneItemInMemoryAction?.Invoke(1);
                            }
                            SetTenantIdle(response.Tenant);
                        }

                        Thread.Sleep(10);//itzik - let other thread abilty to use GLAccout !!!
                    }
                }
                public void CheckCreateIntegrity()
                {



                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("in selectedQueue == JournalApproveService.K_AccountingJournalApproveMutliThreadingWR);");

                    try
                    {
                        string workerRoleName = "";
                        if (!string.IsNullOrEmpty(LogitudeSettings.WorkerRoleName))
                        {
                            workerRoleName = LogitudeSettings.WorkerRoleName;
                        }
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug(" if (DateTime.UtcNow.Date > _NextDueDoneAt.Date && workerRoleName != \"staging\")" + workerRoleName);
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug(" _NextDueDoneAt.Date" + _NextDueDoneAt.Date);

                        if (DateTime.UtcNow.Date > _NextDueDoneAt.Date && workerRoleName != "staging")
                        {

                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(" _NextDueDoneAt.Date 2" + _NextDueDoneAt.Date);

                            if (DateTime.Now < new DateTime(2050, 06, 01))
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("CreateBatchAccountingIntegrityCheck");
                                CreateBatchAccountingIntegrityCheck();
                            }
                            _NextDueDoneAt = DateTime.UtcNow.Date;
                            var myDueLocalBalanceService = new DueLocalBalanceService();
                            myDueLocalBalanceService.RunAllTenants();

                            var dailyRebuildAgingService = new DailyRebuildAgingService();
                            dailyRebuildAgingService.RunAllAgingTenants();

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                public void CreateBatchAccountingIntegrityCheck()
                {
                    try
                    {

                        int year = DateTime.Now.Year;
                        var repo = new GLAccountTotalByMonthRepository(0);
                        var activeTenants = repo.GetActiveTenantPerYear(year);
                        var myTenantRepository = new TenantRepository(0);
                        var prodTenant = myTenantRepository.GetTenants().Where(r => r.IsTestTenant == false).ToList();
                        int iCount = 0;
                        foreach (int tenant in activeTenants)
                        {
                            if (prodTenant.FirstOrDefault(r => r.Id == tenant) == null)
                            {
                                continue;//IsTestTenant
                            }
                            using (var scope = TransactionFactory.GetNewTransaction())
                            {
                                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                                AccountingIntegrityCheckUpdateService service = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

                                var paramsObj = new AccountingIntegrityInParam()
                                {
                                    Tenant = tenant,
                                    FromMonthInclusive = new DateTime(year, 1, 1),
                                    ToMonthInclusive = DateTime.Now,
                                };

                                // serialize
                                string xmlString = LogitudeXmlSerializer.SerializeObjectToXmlElementString<AccountingIntegrityInParam>(paramsObj);

                                service.DelayQueueInMinutes = iCount * 10;
                                service.Update(new AccountingIntegrityCheckPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Insert,
                                    Tenant = tenant,
                                    CreateDateTimeUTC = DateTime.UtcNow,
                                    FromMonthInclusive = new DateTime(year, 1, 1),
                                    ToMonthInclusive = DateTime.Now,
                                    StatusCode = "1",
                                    SendEmailWhileError = true,
                                    ParametersXML = xmlString,

                                }
                                , true);
                                scope.Complete();
                                iCount++;//more 10 min
                            }

                        }






                    }
                    catch (Exception ee)
                    {

                        ExceptionHandler.HandleException(ee, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                        //throw;
                    }
                }
            }
        }

        internal class DBTypeLedgerTransaction
        {
            public DBTypeLedgerTransaction()
            {
            }
            public string JournalId { get; set; }
            public int JournalLineNumber { get; set; }
            public string Id { get; set; }
            public int Tenant { get; set; }
            public DateTime CreateDate { get; set; }
            public string ControlAccountId { get; set; }

            public string AccountId { get; set; }
            public DateTime AccountingDate { get; set; }
            public DateTime DocumentDate { get; set; }
            public DateTime DueDate { get; set; }

            public decimal LocalAmountDebit { get; internal set; }
            public decimal LocalAmountCredit { get; set; }
            public string CurrencyId { get; set; }
            public decimal ForeignAmountDebit { get; set; }
            public decimal ForeignAmountCredit { get; set; }
            public decimal ExchangeRate { get; set; }
            public string Reference1 { get; set; }
            public string Reference2 { get; set; }
            public string Reference3 { get; set; }
            public decimal OpenAmount { get; set; }
            public string OppositeAccountId { get; set; }
            public string SearchFields { get; set; }
            public string OpenAmountCurrencyId { get; set; }
            public string Notes { get; set; }

            public decimal AmountToReconcile { get; set; }

            public bool Mark { get; set; }

            public bool IsReconciled { get; set; }


            public bool IsExternalReconcile { get; set; }

        }

        internal class DBTypeAccountTotalByMonth
        {
            public DBTypeAccountTotalByMonth()
            {
            }

            public string AccountId { get; set; }
            public string DateTypeCode { get; set; }

            public int Year { get; set; }
            public int Month { get; set; }
            public string CurrencyId { get; set; }

            public int Tenant { get; set; }

            public decimal LocalAmountDebit { get; set; }
            public decimal LocalAmountCredit { get; set; }


            public decimal ForeignAmountDebit { get; set; }
            public decimal ForeignAmountCredit { get; set; }




#if false
        private void AccountingStreamingBL(MyActions actions, List<LedgerTransactionPM> myLedgerTransactionsWithCounters, IEnumerable<GLAccountTotalByMonthPM> allGLAccountTotalByMonths, bool reduceDeadlock)
        {
            if (reduceDeadlock)
            {
                //var adapter = (System.Data.Entity.Infrastructure.IObjectContextAdapter)_AccountingContext;
                //var objectContext = adapter.ObjectContext;
                //objectContext.ExecuteCommand("set deadlock_priority -10"); //lowest prio
            }
            //1st try ---to lock all GLAccountTotalByMonthUpdateService due High frequency lock !!!!
            if (actions.HasFlag(MyActions.BuildGLAccountTotalByMonths))
            {






                var myGLAccountTotalByMonthsUpdateServices = new GLAccountTotalByMonthUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
                /////////Update GLAccountTotalByMonths for the GLAccounts
                myGLAccountTotalByMonthsUpdateServices.UpsertDelta(_JournalApproveParser.GLAccountTotalByMonths);
                if (_JournalApproveParser.ControlGLAccountTotalByMonths.Any())
                {
                    //////////Update GLAccountTotalByMonths also for the ControlAccount that are connected to the GLAAccounts
                    myGLAccountTotalByMonthsUpdateServices.UpsertDelta(_JournalApproveParser.ControlGLAccountTotalByMonths);
                }







                UpdateGLAccountBalances(allGLAccountTotalByMonths, myLedgerTransactionsWithCounters);


            }


            if (actions.HasFlag(MyActions.BuildLedgerTransaction))
            {
                //Translate each of the JournalLines into LedgerTransaction line/s 
                var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
                myLedgerTransactionUpdateService.UpdateMulti(myLedgerTransactionsWithCounters, new List<LedgerTransactionPM>(), _JournalPM, true);

                CreateReconcileFromStorno(myLedgerTransactionsWithCounters);
            }
            else if (actions.HasFlag(MyActions.FixLedgerTransaction))
            {
                FixLedgerTransaction();
            }
        }
        private void CreateReconcileFromStorno(List<LedgerTransactionPM> myLedgerTransactionsWithCounters)
        {
            try
            {
                IJournalStornoReconcileService journalStornoReconcileService = new JournalStornoReconcileService();
                journalStornoReconcileService.MustInitialize(this._AccountingContext, _JournalPM, myLedgerTransactionsWithCounters);
                if (journalStornoReconcileService.CreateReconcileFromStorno())
                {
                    var myReconciliationUpdateService = new ReconciliationUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
                    myReconciliationUpdateService.SuppressResetDraftOpenReconciliation = true;
                    myReconciliationUpdateService.UpdateMulti(journalStornoReconcileService.Reconciliations2Insert, new List<ReconciliationPM>(), _JournalPM, true);
                }
            }
            catch (Exception)
            {

                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
                var retrhow = false;
                if (retrhow)//Remark It Side Affect its good but not must 
                {
                    throw;//Remark It Side Affect its good but not must     
                }

            }
        }

        private void UpdateGLAccountBalances(
            IEnumerable<GLAccountTotalByMonthPM> allGLAccountTotalByMonths,
            List<LedgerTransactionPM> myLedgerTransactionsWithCounters)
        {
            DateTime? myNullableDateTime = null;
            /////////Update field BalanceInLocalCurrency in GLAccount records that was updated in GLAccountTotalByMonths 
            var onlyAccountingTot = allGLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate);
            var glAccountsToUpdate = (from tot in onlyAccountingTot
                                      group tot by tot.AccountId into groupByAccId
                                      //where groupByAccId.Sum( r=> r.LocalAmountDebit - r.LocalAmountCredit)!=0m
                                      select new
                                      {
                                          AccountId = groupByAccId.Key,
                                          JournalDelta = groupByAccId.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),

                                          //DueDateDelta=0,
                                          //NextDueDate = myNullableDateTime
                                      }
                               )
                               .Where(g => !g.JournalDelta.Equals(0))
                               .ToList();

            DateTime today = GetToday().Date;
            myNullableDateTime = null;
            var dueDateLedgerTransactions =
                (from trans in myLedgerTransactionsWithCounters
                 group trans by trans.AccountId into groupByAccId
                 select new
                 {
                     AccountId = groupByAccId.Key,
                     //JournalDelta = 0,
                     DueDateDelta = groupByAccId.Where(trans => trans.DueDate.Date <= today).Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                     NextDueDate = groupByAccId.Where(trans => trans.DueDate.Date > today).DefaultIfEmpty(new LedgerTransactionPM()).Min(r => r.DueDate != DateTime.MinValue ? r.DueDate.Date : myNullableDateTime),
                 }
                    )
                    .Where(g => !g.DueDateDelta.Equals(0) || !g.NextDueDate.Equals(null))
                    .ToList();





            //.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit)
            var listAccountId = glAccountsToUpdate.Select(rec => rec.AccountId).Distinct().ToList();
            var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
            var glAccountToUpdate = myGLAccountQueryService.GetByGLAccountsIdList(listAccountId, _JournalPM.Tenant);

            var myGLAccountUpdateService = new GLAccountUpdateServiceBalancePriv(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);
            foreach (var glAccount in glAccountToUpdate)
            {
                glAccount.ChangeSetOp = ChangeSetOperation.Update;
                decimal? deltaLocalBalanceInDue = 0;
                DateTime? nextDueDate =
#if GLAccMoreData
                    glAccount.NextDueDate;
#endif
                    null;
                var journalDelta =
                    //allGLAccountTotalByMonths.Where(r => r.AccountId == glAccount.Id).Sum(r => r.LocalAmountDebit - r.LocalAmountCredit);
                    glAccountsToUpdate.First(r => r.AccountId == glAccount.Id).JournalDelta;

                //glAccount.CurrentContextTag = journalDelta;
                if (glAccount.AccountTypeCode == "2" || //AccoutTypeCode= 2 or 3 (clients/Vendors)
                    glAccount.AccountTypeCode == "3"
                    )
                {
                    var dueLedgerTransaction = dueDateLedgerTransactions.FirstOrDefault(r => r.AccountId == glAccount.Id);
                    if (dueLedgerTransaction != null)
                    {
                        deltaLocalBalanceInDue = dueLedgerTransaction.DueDateDelta;
                        if (dueLedgerTransaction.NextDueDate != null)//if null do not change current next Date 
                        {
                            nextDueDate = dueLedgerTransaction.NextDueDate;
                        }

                    }

                }

                myGLAccountUpdateService.Init(journalDelta, deltaLocalBalanceInDue, nextDueDate);
#if GLAccMoreData
                glAccount.BalanceInLocalCurrency = glAccount.BalanceInLocalCurrency.GetValueOrDefault() + journalDelta;
                glAccount.LocalBalanceInDue = glAccount.LocalBalanceInDue.GetValueOrDefault() + deltaLocalBalanceInDue.GetValueOrDefault();
                glAccount.NextDueDate = nextDueDate;
#endif
                myGLAccountUpdateService.Update(glAccount, true);
            }
        }



#endif




        }

        internal class DBTypeGLAccountAgingData
        {
            public DBTypeGLAccountAgingData()
            {

            }

            public string AccountId { get; set; }
            public int Tenant { get; set; }
            public decimal PeriodPast { get; set; }
            public decimal Period0 { get; set; }

            public decimal Period1 { get; set; }
            public decimal Period2 { get; set; }
            public decimal Period3 { get; set; }
            public decimal Period4 { get; set; }
            public decimal Period5 { get; set; }
            //public object Period6 { get; set; }
            public decimal PeriodFuture { get; set; }
            public int TotalOpenTransactions { get; set; }

        }

        public class ResultApproveJournalM
        {
            public bool Success { get; set; }

            public string FailDue { get; set; }
        }
    }
 
