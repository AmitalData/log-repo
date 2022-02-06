using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using Logitude.Server.Tools.QueueService;
using System.Threading;
using Logitude.SystemLogs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
//using AmitalCustomsWindowsService.Utils;

namespace Logitude.Accounting.BL.Utils
{
    public class RevaluationBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public RevaluationBatch()
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

        //private bool AnyAccountingQueued(IQueryable<string> gLAccountIDList, int tenant, IAccountingContext context)
        //{
        //    var myJournalQueryService = new JournalQueryService(context);
        //    var have = myJournalQueryService.GetAnyPendingApprovedDev(gLAccountIDList, tenant);
        //    return have;
        //}



        public void RunAllOpenRevaluations(int tenant)

        {
            IAccountingContext context = AccountingContext.GetContext(tenant);

 
            RevaluationListQueryService revaluationListQueryService = new RevaluationListQueryService(context);
            List<RevaluationList> revaluations = revaluationListQueryService.GetOpenRevaluationList(tenant);
            if (revaluations != null)
            {
                foreach (RevaluationList rev in revaluations)
                {
                    if (rev != null)
                    {
                        RunOneRevaluation(rev.Id, tenant);
                    }
                }
            }
        }

        public void RunOneRevaluation(int tenant, string revaluationId)

        {
            IAccountingContext context = AccountingContext.GetContext(tenant);


            RevaluationListQueryService revaluationListQueryService = new RevaluationListQueryService(context);
            List<RevaluationList> revaluations = revaluationListQueryService.GetOpenRevaluationList(tenant);
            if (revaluations != null)
            {
                bool no_open = true;
                foreach (RevaluationList rev in revaluations)
                {
                    if (rev != null && rev.Id == revaluationId)
                    {
                        no_open = false;
                        RunOneRevaluation(rev.Id, tenant);
                        break;
                    }
                }
                if (no_open)
                {
                    bool useLocal = true;
                    string errorMessage = TranslateTextsClassTranslate("Revaluations.Q.NotOpen", 0, useLocal);
                    if (String.IsNullOrEmpty(errorMessage)) errorMessage = "Revaluation is not open";
                    _ResponseText = errorMessage;
                    _StatusCode = HttpStatusCode.InternalServerError;
                }
            }
        }


        public void RunOneRevaluation(string id, int tenant)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                {
                   IAccountingContext context = AccountingContext.GetContext(tenant);
                   if (!String.IsNullOrEmpty(id))
                    {
                        bool useLocal = true;
                        RevaluationListQueryService revaluationListQueryService = new RevaluationListQueryService(context);
                        GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                        RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                        TenantQuery tenantQuery = new TenantQuery(tenant);
                        TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                        string accountingCurrencyId = tPM.CurrencyId;
                        List<RatesTablePM> ratesList = new List<RatesTablePM>(); // ratesList is one per revaluation 
                        List<JournalLineList> lineList = new List<JournalLineList>();
                        FullAccountingSettingQueryService settingQuery = new FullAccountingSettingQueryService(tenant);
                        //JournalUpdateService(context);
                        JournalUpdateService journalUpdateService = new JournalUpdateService(context, new Dictionary<string, IContext>(), tenant);
                        RevaluationList revaluation = revaluationListQueryService.GetSingle(id);
                        if (revaluation != null && revaluation.RevaluationDate != null)
                        {
                           FullAccountingSettingPM setting = settingQuery.GetSingleFullAccountingSetting(tenant);
                           bool createRevaluationJournalinDetail = setting.CreateRevaluationJournal;
                           string revaluationDiffAccountId = revaluation.RevaluationsGLAccountId;
                            if (String.IsNullOrEmpty(revaluationDiffAccountId))
                            {
                                string diffAccountId = "";
                                if (setting != null)
                                {
                                    diffAccountId = setting.ExchangeRateDiffGLAccountId;
                                }
                                if (String.IsNullOrEmpty(diffAccountId))
                                {
                                    string errorMessage = TranslateTextsClassTranslate("Revaluations.Q.DiffAccountNotDefined", 0, useLocal);
                                    throw new Exception(errorMessage);
                                }
                                revaluationDiffAccountId = diffAccountId;
                            }
                            List<GLAccountPM> gLAccountPMList = gLAccountQueryService.GetByRevaluationEnabled_OtherParams(revaluation.RevaluationEnabled, null, revaluation.ChartOfAccountsId, null, revaluation.GLAccountId, accountingCurrencyId, tenant);
                            if (gLAccountPMList != null)
                            {
                                IQueryable<string> gLAccountIDList = gLAccountPMList.Select(l => l.Id).AsQueryable();
                                //bool have = AnyAccountingQueued(gLAccountIDList, tenant, context);
                                foreach (GLAccountPM gLAccountPM in gLAccountPMList)
                                {
                                    RunOneAccount(gLAccountPM, gLAccountQueryService, journalUpdateService, ratesTableQuery, revaluation.RevaluationDate,
                                                    accountingCurrencyId, revaluationDiffAccountId, ratesList, lineList, revaluation, scope, createRevaluationJournalinDetail);
                                }
                                if (lineList.Count > 0)
                                {
                                    WriteJournal(journalUpdateService, lineList, revaluation);
                                    lineList.Clear();
                                }

                            }
                        }
                    }
                    UpdateRevaluationStatus(id, tenant, "2", "Success", context);

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
              //          UpdateRevaluationStatus(id, tenant, "2", errorMessage, context);
                    }
                    excScope.Complete();
                }

            }

        }

        private static void UpdateRevaluationStatus(string id, int tenant, string status, string message, IAccountingContext context)
        {
            RevaluationQueryService myRevaluationService = new RevaluationQueryService(context);
            RevaluationPM revaluationPM = myRevaluationService.GetSingle(id, false, false);
            if (revaluationPM != null)
            {
                revaluationPM.Status = status;
                revaluationPM.Message = message;
                revaluationPM.ChangeSetOp = ChangeSetOperation.Update;
                RevaluationUpdateService myRevaluationUpdateService = new RevaluationUpdateService(context, new Dictionary<string, IContext>(), tenant);
                myRevaluationUpdateService.Update(revaluationPM, true);
            }
        }

        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            if (OverrideITextCodeTranslator != null)
            {
                return OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private static void RunOneAccount(GLAccountPM gLAccountPM, GLAccountQueryService gLAccountQueryService, JournalUpdateService journalUpdateService,
            RatesTableQuery ratesTableQuery, DateTime revaluationDate, string accountingCurrencyId, string diffAccountId,
            List<RatesTablePM> ratesList, List<JournalLineList> lineList, RevaluationList revaluation, TransactionScope scope, bool createRevaluationJournalinDetail)
        {
            LogMessagingUtil.Instance.AppendLine("Revaluation " + revaluation.RevaluationNumber + " run one account: " + gLAccountPM.DisplayNumber);
            List<GLAccountCurrencyBalance> allBalances = gLAccountQueryService.GetCurrencyBalances(gLAccountPM, revaluationDate, gLAccountPM.Tenant);
            bool useLocal = true;

            if (allBalances != null && allBalances.Count != 0)
            {
                foreach (GLAccountCurrencyBalance item in allBalances)
                {
                    if (!String.IsNullOrEmpty(item.CurrencyId) && !String.IsNullOrEmpty(accountingCurrencyId) && item.CurrencyId != accountingCurrencyId) // && (decimal)item.ForeignAmount != 0m)
                    {
                        LogMessagingUtil.Instance.AppendLine(" Balance in " + item.CurrencyId + " = " + item.ForeignAmount);

                        RatesTablePM lastRate = ratesList.Where(d => d.ForeignCurrencyId == item.CurrencyId).FirstOrDefault(); // caching, ratesList is one per revaluation
                        if (lastRate == null)
                        {
                            lastRate = ratesTableQuery.GetLastRateByValueDate(gLAccountPM.Tenant, item.CurrencyId, accountingCurrencyId, revaluationDate);
                            if (lastRate != null)
                            {
                                ratesList.Add(lastRate);
                            }
                            else
                            {
                                CurrencyQuery currencyQuery = new CurrencyQuery(gLAccountPM.Tenant);
                                CurrencyPM curr = currencyQuery.GetSinglePM(item.CurrencyId, gLAccountPM.Tenant);
//                              string revError = TranslateTextsClass.Translate("Revaluations.Q.RevaluationError", gLAccountPM.Tenant);
//                              string rateNotFound = TranslateTextsClass.Translate("GLAccounts.Q.RateNotFound", gLAccountPM.Tenant);
                                string revError = TranslateTextsClassTranslate("Revaluations.Q.RevaluationError", 0, useLocal);
                                string rateNotFound = TranslateTextsClassTranslate("GLAccounts.Q.RateNotFound", 0, useLocal);
                                LogMessagingUtil.Instance.AppendLine(revError + curr.Code + rateNotFound + revaluationDate.ToShortDateString());
                                throw new Exception(revError + curr.Code + rateNotFound + revaluationDate.ToShortDateString());
                            }
                        }

                        LogMessagingUtil.Instance.AppendLine(" Rate = " + lastRate.Rate + " on " + revaluationDate.ToShortDateString());
                        double localFromForeign_double = (double)item.ForeignAmount * (double)lastRate.Rate;
                        localFromForeign_double = Math.Round(localFromForeign_double, 2);
                        decimal localFromForeign_decimal = (decimal)localFromForeign_double;
                        decimal difference = localFromForeign_decimal - (decimal)item.LocalAmount;
                        LogMessagingUtil.Instance.AppendLine(" Local(foreign) = " + localFromForeign_decimal);
                        LogMessagingUtil.Instance.AppendLine(" Local = " + (decimal)item.LocalAmount);
                        LogMessagingUtil.Instance.AppendLine(" Difference = " + difference);

                        if (difference != 0m)
                        {
                            JournalLineList journalLine_credit;
                            if (createRevaluationJournalinDetail)
                            {
                                journalLine_credit = lineList.FirstOrDefault<JournalLineList>(l => l.ActionCode == "1" && l.CurrencyId == item.CurrencyId && l.DebitAccountId == gLAccountPM.Id);
                            }
                            else
                            {
                                journalLine_credit = lineList.FirstOrDefault<JournalLineList>(l => l.ActionCode == "1" && l.CurrencyId == item.CurrencyId);
                            }
                           if (journalLine_credit == null)
                            {
                                journalLine_credit = new JournalLineList
                                {
                                    ActionCode = "1", // Credit
                                    AccountingDate = revaluationDate,
                                    Tenant = gLAccountPM.Tenant,
                                    CreditAccountId = diffAccountId,
                                    DocumentDate = revaluationDate,
                                    DueDate = revaluationDate,
                                    LocalAmount = difference,
                                    CurrencyId = item.CurrencyId, // was     ... = accountingCurrencyId,
                                    ForeignAmount = 0m, // was     ... = difference, 
                                    Reference1 = revaluation.RevaluationNumber.ToString(),
//                                  Notes = TranslateTextsClass.Translate("Revaluations.Q.Revaluation", gLAccountPM.Tenant),
                                    Notes = TranslateTextsClassTranslate("Revaluations.Q.Revaluation", 0, useLocal),
                                    DebitAccountId = createRevaluationJournalinDetail? gLAccountPM.Id:null,
                                };
                                LogMessagingUtil.Instance.AppendLine("Credit Difference = " + difference);
                                lineList.Add(journalLine_credit);
                            }
                            else
                            {
                                int index = lineList.FindIndex(l => l.ActionCode == "1" && l.CurrencyId == item.CurrencyId);
                                LogMessagingUtil.Instance.AppendLine("Credit Difference = " + journalLine_credit.LocalAmount + " += " + difference + " = " + (journalLine_credit.LocalAmount + difference));
                                journalLine_credit.LocalAmount += difference;
                                //journalLine_credit.ForeignAmount += difference; // now it is 0 
                                lineList[index] = journalLine_credit;
                            }

                            JournalLineList journalLine_debit = new JournalLineList
                            {
                                ActionCode = "2", // Debit
                                AccountingDate = revaluationDate,
                                Tenant = gLAccountPM.Tenant,
                                DebitAccountId = gLAccountPM.Id,
                                CreditAccountId = diffAccountId,
                                // DebitControlAccountId = gLAccountPM.ControlAccountId,
                                DocumentDate = revaluationDate,
                                DueDate = revaluationDate,
                                LocalAmount = difference,
                                CurrencyId = item.CurrencyId,
                                ForeignAmount = 0m,
                                Reference1 = revaluation.RevaluationNumber.ToString(),
//                              Notes = TranslateTextsClass.Translate("Revaluations.Q.Revaluation", gLAccountPM.Tenant),
                                Notes = TranslateTextsClass.Translate("Revaluations.Q.Revaluation", 0, useLocal),
                            };
                            LogMessagingUtil.Instance.AppendLine("Debit Difference = " + difference);
                            lineList.Add(journalLine_debit);


                            if (lineList.Count >= 100)
                            {
                                WriteJournal(journalUpdateService, lineList, revaluation);
                                lineList.Clear();
                              //  scope.Complete();
                            }
                        }
                    }
                }

                //if (lineList.Count > 0)
                //{
                //    WriteJournal(journalUpdateService, lineList, revaluation);
                //    lineList.Clear();
                //}
            }

        }
        private static void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingEntityJournalUpdateServiceExt service = ContainerAccessor.Container.Resolve(typeof(IAccountingEntityJournalUpdateServiceExt), "AccountingEntityJournalUpdateServiceExt", new ParameterOverride("", 1)) as IAccountingEntityJournalUpdateServiceExt;
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }
        private static void WriteJournal(JournalUpdateService journalUpdateService, List<JournalLineList> lineList, RevaluationList revaluation)
        {
            // Start
                JournalPM newJournal = new JournalPM();

            // Head
            newJournal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            newJournal.Tenant = lineList.First().Tenant;
            newJournal.CreateDate = DateTime.Now;
            newJournal.AccountingDate = lineList.First().AccountingDate;
            newJournal.TypeCode = "0"; //Manual
            newJournal.StatusCode = "2"; // Approved
            newJournal.CreatedByUserId = revaluation.CreatedByUserId;
            newJournal.AccountingEntityCode = "8"; //Revaluation
            newJournal.AccountingEntityId = revaluation.Id;
            newJournal.AccountingEntityReference = revaluation.RevaluationNumber.ToString();
            newJournal.ExternalNo = null;
            newJournal.UpdateDate = DateTime.Now;
            newJournal.UpdatedByUserId = revaluation.CreatedByUserId;
            newJournal.ApproveDate = revaluation.CreateDate;
            newJournal.ApprovedByUserId = revaluation.CreatedByUserId;

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
                  //  CreditControlAccountId = line.CreditControlAccountId,
                    CurrencyId = line.CurrencyId,
                    DebitAccountId = line.DebitAccountId,
                 //   DebitControlAccountId = line.DebitControlAccountId,
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
            AddAccountingEntitieJournal(newJournal, AccountingEntityJournalActions.RevaluationApprove);

            // End
            journalUpdateService.Update(newJournal, true);

        }



        public class RevaluationWorkerRole
        {


            public const string K_RevaluationWorkerRole = "RevaluationWorkerRole";
            public const string QP_Tenant = "Tenant";
            public const string QP_RevaluationNumber = "RevaluationNumber";
            public void EnQueue(int tenant, int revaluationNumber)
            {
                return;// YARON + IM  :USE BATCH EXECUCTION 
                var queueservice = new DbQueueService();
                queueservice.InitializeQueue(K_RevaluationWorkerRole, 0);
                queueservice.Send(new Dictionary<string, string>()
                    {
                        { QP_Tenant, tenant.ToString() },
                        { QP_RevaluationNumber, revaluationNumber.ToString() }
                    }, tenant);
            }

            public void WorkUntilQEmptyQueueDB()
            {



                QueueResponse response = null;
                while (true)
                {

                    DbQueueService queueservice = null;

                    try
                    {

                        queueservice = new DbQueueService(RevaluationWorkerRole.K_RevaluationWorkerRole, 0);

                        response = queueservice.Receive(new TimeSpan(0, 0, 0, 5));


                    }
                    catch (Exception)
                    {

                        throw;
                    }


                    if (response == null || (response != null && response.MessageId == null))
                    {
                        break;
                    }



                    ProcessMessage_Db(queueservice, response);
                    Thread.Sleep(10);//itzik - let other thread abilty to use GLAccout !!!
                }




            }

            private void ProcessMessage_Db(DbQueueService myDbQueueService, QueueResponse message)
            {
                string MessageId = "";
                int tenant = -1;
                string qpJournalId = null;
                try
                {



                    int.TryParse(message.MessageValues[QP_Tenant].ToString(), out tenant);
                    if (tenant == -1)
                    {
                        ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "RevaluationWorkerRole: ProcessMessage() Method :tenant==-1", null);
                        return;
                    }


                    //qpJournalId = message.MessageValues[QP_RevaluationNumber].ToString();
                    MessageId = message.MessageId;
                    LogMessagingUtil.Instance.AppendLine("receivedMessage.DeliveryCount =" + message.RetryNumber.ToString());

                    var myRevaluationBatch = new RevaluationBatch();
                    myRevaluationBatch.RunAllOpenRevaluations(tenant);

                    myDbQueueService.Complete();
                    
                }
                catch (Exception ex)
                {
                    var ex1 = new Exception("RevaluationWorkerRole.RunAllOpenRevaluations(" + tenant.ToString() + "," + MessageId.ToString() + ").SubmitApprove():FailDue=" + ex.ToString());
                    OnException(myDbQueueService, message, qpJournalId, tenant, ex);
                }
            }
        }
        private static void OnException(DbQueueService myDbQueueService, QueueResponse message, string seedJournalId, int tenant, Exception ex)
        {

            LogMessagingUtil.Instance.AppendLine(message?.MessageId?.ToString() + " " + ex.ToString());
            ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", "RevaluationWorkerRole: ProcessMessage() Method", null);
            if (message == null || message.RetryNumber > 5)
            {
                //var journalFailedService = new JournalFailedService(tenant, seedJournalId);
                //journalFailedService.MarkAsFailed();
                //if (myDbQueueService != null)
                {
                    myDbQueueService.Complete();
                }
            }
        }
    }
}
