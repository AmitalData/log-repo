using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Text;
using Logitude.Customs.BL.Messaging.Customs;


//using AmitalCustomsWindowsService.Utils;

namespace Logitude.Accounting.BL.Utils
{
    public class PostDatedChequesRedemptionBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private string _AggregateKey;
        private StringBuilder _logger;

        public PostDatedChequesRedemptionBatch()
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





        public void RunAllPayablePostDatedARPaymentCheques(int tenant)
        {
            List<ARPaymentChequeList> aRPaymentCheques = null;

            IAccountingContext context = AccountingContext.GetContext(tenant);
            ARPaymentChequeListQueryService aRPaymentChequeListQueryService = new ARPaymentChequeListQueryService(context);
            aRPaymentCheques = aRPaymentChequeListQueryService.GetPayablePostDatedARPaymentChequeList(tenant);
            
            var uniqueCheques = aRPaymentCheques.Distinct();


            if (aRPaymentCheques != null)
            {
                foreach (ARPaymentChequeList chq in uniqueCheques)
                {
                    if (chq != null)
                    {
                        RunOnePayableARPaymentCheque(chq.Id, tenant);
                    }
                }
            }
        }


        public void RunOnePayableARPaymentCheque(string id, int tenant)
        {
            IAccountingContext context = AccountingContext.GetContext(tenant);
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(3)))
                {
                    try
                    {
                        _AggregateKey = "ARPaymentChequeRedemption-" + id; // VarChar 128 
                        LockIt(tenant);
                    }
                    catch (Exception eee)
                    {
                        AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Information);

                        if (eee.ToString().Contains("ORA-00054"))
                        {
                            throw new CustomsRequestsSheetDomainModelServiceException(CustomsRequestsSheetDomainModelServiceException.WhereEnum.AggregateDCAAnalyzerLockIt, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.RetryQueue, "GeneralLock is locked in another thread", eee);
                        }
                        //ActivityLogger
                        throw;
                    }

                    if (!String.IsNullOrEmpty(id))
                    {
                        ARPaymentChequeListQueryService aRPaymentChequeListQueryService = new ARPaymentChequeListQueryService(context);
                        ARPaymentChequeList aRPaymentCheque = aRPaymentChequeListQueryService.GetSingle(id);
                        AccountingLogger.LogMe("ARPaymentCheque - run one cheque: " + aRPaymentCheque.ChequeNumber, false, "CHQ");


                        BankDepositLineListQueryService bankDepositLineListQueryService = new BankDepositLineListQueryService(context);
                        List<BankDepositLineList> bankDepositLines = bankDepositLineListQueryService.GetByARPaymentChequeId(id, "", tenant);
                        if (bankDepositLines == null)
                        {
                            string errorMessage = "E1: " + TranslateTextsClass.Translate("Cheques.Q.ChequeNotDeposited", tenant);
                            throw new Exception(errorMessage);
                        }
                        //BankDepositLineList bankDepositLineList = bankDepositLines.FirstOrDefault();
                        //if (bankDepositLineList == null || String.IsNullOrEmpty(bankDepositLineList.DepositId))
                        //{
                        //    string errorMessage = "E2: " + TranslateTextsClass.Translate("Cheques.Q.ChequeNotDeposited", tenant);
                        //    throw new Exception(errorMessage);
                        //}
                        List<string> depositIdList = bankDepositLines.Select(c => c.DepositId).Distinct().ToList();
                        BankDepositListQueryService bankDepositListQueryService = new BankDepositListQueryService(context);
                        BankDepositList bankDeposit = bankDepositListQueryService.GetLastBankDepositByIdList(tenant, depositIdList);
                        if (bankDeposit == null)
                        {
                            string errorMessage = "E3: " + TranslateTextsClass.Translate("Cheques.Q.ChequeNotDeposited", tenant);
                            throw new Exception(errorMessage);
                        }

                        BankAccountListQueryService bankAccountListQueryService = new BankAccountListQueryService(context);
                        BankAccountList bankAccount = bankAccountListQueryService.GetSingle(bankDeposit.DepositBankAccountId);
                        if (bankAccount == null)
                        {
                            string errorMessage = "E4: " + TranslateTextsClass.Translate("Cheques.Q.ChequeNotDeposited", tenant);
                            throw new Exception(errorMessage);
                        }

                        bool useLocal = true;
                        bool doJournal = true;
                        if (bankAccount.DeferredGLAccountId == bankAccount.GLAccountId)
                            doJournal = false;
                        //    var user = GetLoggedContact(tenant);
                        //    if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);
                        if (doJournal)
                        {
                            JournalUpdateService journalUpdateService = new JournalUpdateService(context, new Dictionary<string, IContext>(), tenant);
                            List<JournalLineList> lineList = new List<JournalLineList>();
                            JournalLineList journalLine_credit = new JournalLineList
                            {
                                ActionCode = "1", // Credit
                                AccountingDate = DateTime.Now.Date,
                                Tenant = aRPaymentCheque.Tenant,
                                CreditAccountId = bankAccount.DeferredGLAccountId,
                                DocumentDate = aRPaymentCheque.ValueDate.Date,
                                DueDate = aRPaymentCheque.ValueDate.Date,
                                LocalAmount = aRPaymentCheque.LocalAmount,
                                // CurrencyId = aRPaymentCheque.CurrencyId,
                                CurrencyCode = aRPaymentCheque.CurrencyCode,
                                ForeignAmount = aRPaymentCheque.ForeignAmount,
                                Reference1 = aRPaymentCheque.ChequeNumber,
                                Reference2 = bankDeposit.DepositNumber.ToString(),
                                Reference3 = aRPaymentCheque.PaymentNumber,
                                Notes = TranslateTextsClassTranslate("Accounting.General.O.PostdatedChequeRedemption", 0, useLocal),
                            };
                            AccountingLogger.LogMe("Credit Cheque = " + aRPaymentCheque.ChequeNumber, false, "CHQ");
                            lineList.Add(journalLine_credit);

                            JournalLineList journalLine_debit = new JournalLineList
                            {
                                ActionCode = "2", // Debit
                                AccountingDate = DateTime.Now.Date,
                                Tenant = aRPaymentCheque.Tenant,
                                DebitAccountId = bankAccount.GLAccountId,
                                CreditAccountId = bankAccount.DeferredGLAccountId,
                                //  DebitControlAccountId = gLAccountPM.ControlAccountId,
                                DocumentDate = aRPaymentCheque.ValueDate.Date,
                                DueDate = aRPaymentCheque.ValueDate.Date,
                                LocalAmount = aRPaymentCheque.LocalAmount,
                                // CurrencyId = aRPaymentCheque.CurrencyId,
                                CurrencyCode = aRPaymentCheque.CurrencyCode,
                                ForeignAmount = aRPaymentCheque.ForeignAmount,
                                Reference1 = aRPaymentCheque.ChequeNumber,
                                Reference2 = bankDeposit.DepositNumber.ToString(),
                                Reference3 = aRPaymentCheque.PaymentNumber,
                                Notes = TranslateTextsClassTranslate("Accounting.General.O.PostdatedChequeRedemption", 0, useLocal),
                            };
                            AccountingLogger.LogMe("Debit Cheque = " + aRPaymentCheque.ChequeNumber, false, "CHQ");
                            lineList.Add(journalLine_debit);

                            if (aRPaymentCheque != null && aRPaymentCheque.ValueDate != null)
                            {

                                WriteJournal(journalUpdateService, lineList, aRPaymentCheque, bankDeposit);
                                lineList.Clear();

                            }
                        }
                    }
                    UpdateARPaymentChequeStatus(id, tenant, "3", context);
                    if (!String.IsNullOrEmpty(_AggregateKey))
                    {
                        TryDeleteLockRow(tenant);
                    }
                    scope.Complete();
                }//using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
            }
            catch (Exception e)
            {
                using (TransactionScope excScope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(1)))
                {
                    {
                        string errorMessage = e.Message.Split(new[] { '\r', '\n' }).FirstOrDefault();
                        _ResponseText = errorMessage;
                        _StatusCode = HttpStatusCode.InternalServerError;
                 //       UpdateARPaymentChequeStatus(id, tenant, "2", context);
                    }
                    if (!String.IsNullOrEmpty(_AggregateKey))
                    { 
                        TryDeleteLockRow(tenant);
                    }
                    excScope.Complete();
                }

            }

        }


        private void TryDeleteLockRow(int tenant)
        {
            //GeneralLock
            {
                try
                {

                    var repo = new GeneralLockRepository(tenant);

                    repo.FastDelete(_AggregateKey, tenant);
                    _AggregateKey = "";
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }

        private void LockIt(int tenant)
        {
            var repo = new GeneralLockRepository(tenant);
            
            var lockPoco = repo.GetSingleGeneralLockNOWAIT(_AggregateKey, tenant);
            if (lockPoco == null)
            {

                    repo.Add(new GeneralLock()
                    {
                        Tenant = tenant,
                        GeneralKey = _AggregateKey,
                        CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant)
                    });
                  //  _logger.AppendLine("add GeneralLock");
                    repo.SubmitChanges();

                lockPoco = repo.GetSingleGeneralLockNOWAIT(_AggregateKey, tenant);
            }

            if (lockPoco == null)
            {
                throw new Exception("lockPoco ==null");
            }
            else
            {
              //  _logger.AppendLine("Lock it ");
            }

        }



        private static void UpdateARPaymentChequeStatus(string id, int tenant, string status, IAccountingContext context)
        {
            ARPaymentChequeQueryService myARPaymentChequeService = new ARPaymentChequeQueryService(context);
            ARPaymentChequePM aRPaymentChequePM = myARPaymentChequeService.GetSingle(id, false, false);
            if (aRPaymentChequePM != null)
            {
                aRPaymentChequePM.StatusCode = status;
                aRPaymentChequePM.ChangeSetOp = ChangeSetOperation.Update;
                ARPaymentChequeUpdateService myARPaymentChequeUpdateService = new ARPaymentChequeUpdateService(context, new Dictionary<string, IContext>(), tenant);
                myARPaymentChequeUpdateService.Update(aRPaymentChequePM, true);
            }
        }



        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant,getLocalDefaultText); //, getLocalDefaultText);
        }


        private static void WriteJournal(JournalUpdateService journalUpdateService, List<JournalLineList> lineList, ARPaymentChequeList aRPaymentCheque, BankDepositList bankDeposit)
        {
            // Start
            JournalPM newJournal = new JournalPM();

            // Head
            newJournal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            newJournal.Tenant = lineList.First().Tenant;
            newJournal.CreateDate = DateTime.Now;
            newJournal.AccountingDate = DateTime.Now.Date; //lineList.First().AccountingDate;
            newJournal.TypeCode = "0"; //Regular
            newJournal.StatusCode = "2"; // Approved
            ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(aRPaymentCheque.Tenant);
            if (!String.IsNullOrEmpty(aRPaymentCheque.PaymentId))
            {
                ARPayment aRPayment = aRPaymentQuery.GetSingleARPayment(aRPaymentCheque.PaymentId, aRPaymentCheque.Tenant);
                if (aRPayment != null)
                {
                    newJournal.CreatedByUserId = aRPayment.CreatedByUserId;
                }
            }
            newJournal.AccountingEntityCode = "6";// Cheque Deposit [former value is "1"; //Journal ]
            newJournal.AccountingEntityId = bankDeposit.Id;
            newJournal.ExternalNo = null;
            newJournal.UpdateDate = DateTime.Now;
            newJournal.UpdatedByUserId = newJournal.CreatedByUserId;
            newJournal.ApproveDate = DateTime.Now;
            newJournal.ApprovedByUserId = newJournal.CreatedByUserId;

            // Lines
            int LineNumber = 0;
            foreach (JournalLineList line in lineList)
            {
                LineNumber++;
                JournalLinePM newJournalLine = new JournalLinePM
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = newJournal.Tenant,
                    Line = LineNumber,
                    ActionCode = line.ActionCode,
                    CreditAccountId = line.CreditAccountId,
                    CreditControlAccountId = line.CreditControlAccountId,
                    // CurrencyId = line.CurrencyId,
                    CurrencyCode = line.CurrencyCode,
                    DebitAccountId = line.DebitAccountId,
                    DebitControlAccountId = line.DebitControlAccountId,
                    DocumentDate = line.DocumentDate,
                    AccountingDate = newJournal.AccountingDate,
                    DueDate = line.DueDate,
                    ExchangeRate = line.ExchangeRate,
                    ForeignAmount = line.ForeignAmount,
                    LocalAmount = line.LocalAmount,
                    Notes = line.Notes,
                    Reference1 = line.Reference1,
                    Reference2 = line.Reference2,
                    Reference3 = line.Reference3,
                };
                newJournal.JournalLines.Add(newJournalLine);

            }
            // End
            journalUpdateService.Update(newJournal, true);

            AddAccountingEntityJournal(newJournal, AccountingEntityJournalActions.BankDepositChequeRedemption, aRPaymentCheque.Id);

        }


        public static void AddAccountingEntityJournal(JournalPM journal, string actionName, string childEntityId = null)
        {
            IAccountingContext context = AccountingContext.GetContext(journal.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, IContext>(), journal.Tenant);
            service.AddAccountingEntitieJournal(journal, actionName, childEntityId);
        }



    }
}
