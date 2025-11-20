using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using GLAccountQueryService = Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService;
using JournalQueryService = Logitude.Accounting.BL.EntityQueryServices.JournalQueryService;
using LedgerTransaction = Logitude.Accounting.Data.EntityPOCOs.LedgerTransaction;
using Logitude.Accounting.Data.EntityListQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconciliationUpdateService : EntityUpdateService<Reconciliation, ReconciliationPM, EntityPM>
    {
        private bool _CancelledAction;
        public bool updateGLAccountAgingDataUsingWR = false;
        protected override void OnCreating(ReconciliationPM entityPM, EntityPM entityParentPM)
        {
            if (String.IsNullOrEmpty(entityPM.Id) || entityPM.Id == "new") entityPM.Id = IdCounter.GetNumber("Reconciliation", entityPM.Tenant);
            if (entityPM.IsCancelled == null)
            {
                entityPM.IsCancelled = false;
            }
            if (entityPM.ReconciliationLines.Count > 0)
            {
                foreach (ReconciliationLinePM item in entityPM.ReconciliationLines)
                {
                    item.ReconciliationId = entityPM.Id;
                }

            }
            if (String.IsNullOrEmpty(entityPM.CreatedByUserId) || entityPM.CreatedByUserId == "new")
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                entityPM.CreatedByUserId = contact.Id;
            }

            if (entityPM.Number == "get") entityPM.Number = CodeCounter.GetNumber("Reconciliation.Number", entityPM.Tenant).ToString();
            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            entityPM.SearchFields = ///Task:25238 GetentityPM.AccountId + "," + 
                entityPM.Number + ",";

        }

        protected override void OnUpdating(ReconciliationPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                throw new ApplicationException("ChangeSetOperation.Delete ???");
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                var cancelledProp = new List<string>(){
                    ReconciliationDataMapping.PMPropertyNames.IsCancelled.ToString()
                     };

                List<NotifyPropertyChangeValues> propChanged = ChangeTrackingEntityPM.ChangedProperties.Where(f =>
                    !cancelledProp.Contains(f.PropertyName)).ToList();


                //update 
                if (propChanged.Count == 1 && propChanged.First().PropertyName == "SearchFields")
                {
                    // SearchFields updated after reconcile saved, why? in order to get saved reconciliation lines
                }
                else if (propChanged.Any())
                {
                    throw new ApplicationException("BLException :Approved Reconciliation Can Only Change To IsCancelled Property");
                }
                if (entityPM.ReconciliationLines.Any(jl => jl.ChangeSetOp != ChangeSetOperation.None))
                {
                    throw new ApplicationException("BLException :Approved Reconciliation Can Only Change To IsCancelled Property");
                }
            }
            else
            {
                LedgerTransactionRepository transactionRepository = new LedgerTransactionRepository(entityPM.Tenant);

                // set searchfields
                foreach (ReconciliationLinePM recoLine in entityPM.ReconciliationLines)
                {
                    //get transaction
                    LedgerTransaction transaction = transactionRepository.GetSingle(recoLine.TransactionId, recoLine.Tenant);
                    recoLine.SearchFields = transaction.SearchFields;

                    if (transaction != null)
                        PushSearchFieldText(entityPM, transaction.SearchFields);

                }


                // fill ReconciledWithTransactionId field 
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
                LedgerTransactionUpdateService transactionService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                EntityQueryServices.ARPaymentChequeQueryService aRPaymentChequeQueryService = new EntityQueryServices.ARPaymentChequeQueryService(entityPM.Tenant);



                List<string> ledgerTransactionIds = entityPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(d => d.TransactionId).ToList();
                IQueryable<string> jIds = GetReconcileTransactionJournalIds(ledgerTransactionIds, entityPM.Tenant);
               
                if (ExistsAccEntCodeByJIds(CloseTables.AccountingEntityValues.ARPayment, jIds, entityPM.Tenant))
                {
                    string aRPaymentTransId = GetFirstIdByAccEntCodeByJIds(CloseTables.AccountingEntityValues.ARPayment, jIds, entityPM.Tenant);
                    if (String.IsNullOrEmpty(aRPaymentTransId)) throw new ApplicationException("Cannot find A/R payment transaction on reco lines");

                    foreach (ReconciliationLinePM recoLine in entityPM.ReconciliationLines)
                    {
                        if (recoLine.TransactionId != aRPaymentTransId)
                        {
                            recoLine.ReconciledWithTransactionId = aRPaymentTransId;
                        }
                    }
                }


                if (ExistsAccEntCodeByJIds(CloseTables.AccountingEntityValues.APPayment, jIds, entityPM.Tenant))
                {
                    string aPPaymentTransId = GetFirstIdByAccEntCodeByJIds(CloseTables.AccountingEntityValues.APPayment, jIds, entityPM.Tenant);
                    if (String.IsNullOrEmpty(aPPaymentTransId)) throw new ApplicationException("Cannot find A/P payment transaction on reco lines");

                    foreach (ReconciliationLinePM recoLine in entityPM.ReconciliationLines)
                    {
                        if (recoLine.TransactionId != aPPaymentTransId)
                        {
                            recoLine.ReconciledWithTransactionId = aPPaymentTransId;
                        }
                    }

                }


                if (FeatureToggleHelper.HasFeatureToggle("PSR", entityPM.Tenant))
                {
                    if (ExistsAccEntCodeByJIds(CloseTables.AccountingEntityValues.APInvoice, jIds, entityPM.Tenant))
                    {
                        IInvoiceContext invoiceContext = InvoiceContext.GetContext(entityPM.Tenant);

                        List<LedgerTransactionJournalLineLT> aPInvoiceLTs = GetAPInvoiceLedgerTransactionsByIdList(ledgerTransactionIds, entityPM.Tenant);
                        if (aPInvoiceLTs == null || aPInvoiceLTs.Count == 0) throw new ApplicationException("Cannot find A/P Invoice transaction on reco lines");

                        JournalLineRepository journalLineRepository = new JournalLineRepository(entityPM.Tenant);

                        APInvoiceQuery aPInvoiceQuery = new APInvoiceQuery(entityPM.Tenant);
                        foreach (LedgerTransactionJournalLineLT lt in aPInvoiceLTs)
                        {
                            APInvoicePM aPInvoicePM = aPInvoiceQuery.GetSinglePM(lt.SourceId, entityPM.Tenant);
                            if (aPInvoicePM == null) throw new ApplicationException("Cannot find A/P Invoice " + lt.SourceNumber);
                            switch (aPInvoicePM.StatusCode)
                            {
                                case "WA":
                                case "VD":
                                case "AC":
                                        break;
                                case "AD":
                                case "PD":
                                case "PP":
                                    {
                                        string newStatus = "";
                                        bool newIsClosed = false;
                                        if ((lt.LocalAmountDebit != 0m && lt.OpenAmount == lt.LocalAmountDebit)
                                            || (lt.LocalAmountDebit == 0m && lt.OpenAmount == lt.LocalAmountCredit * -1))
                                        {
                                            newStatus = "AD";
                                        }
                                        else if (lt.OpenAmount != 0m)
                                        {
                                            newStatus = "PP";
                                        }
                                        else if (lt.OpenAmount == 0m)
                                        {
                                            newStatus = "PD";
                                            newIsClosed = true;
                                        }
                                        else
                                        {
                                            throw new ApplicationException("A/P Invoice " + lt.SourceNumber + " cannot compute status");
                                        }
                                        if (newStatus != aPInvoicePM.StatusCode && !String.IsNullOrEmpty(newStatus))
                                        {
                                            DateTime stopLogAt = new DateTime(2023, 06, 01);
                                            string text = "ReconciliationUpdateService.OnUpdating(*1*) Set APInvoicePM.StatusCode: " + aPInvoicePM.Id + " old : " + aPInvoicePM.StatusCode + " new : " + newStatus;
                                            ULog(text, stopLogAt);

                                            aPInvoicePM.StatusCode = newStatus;
                                            aPInvoicePM.IsClosed = newIsClosed;
                                            APInvoiceService aPInvoiceService = new APInvoiceService(invoiceContext, entityPM.Tenant);
                                            aPInvoiceService.Update(aPInvoicePM, true);
                                        }
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }

                    }
                }




                if (entityPM.RevalOnForeignReco == true  && FeatureToggleHelper.HasFeatureToggle("RFR", entityPM.Tenant))
                {
                    var glaccountQueryService = new GLAccountQueryService(entityPM.Tenant);
                    var glAccountPM = glaccountQueryService.GetSingle(entityPM.AccountId, false, true);
                    if (glAccountPM != null
                        && glAccountPM.ReconcileMethodCode == ReconcileMethodValues.ForeignCurrency)
                    {
                        if (Math.Round(entityPM.ReconciliationLines
                            .Where(x => x.CurrencyRate != null)
                            .Sum(x => x.ReconciliationAmount * x.CurrencyRate) ?? 0, 2) != 0)
                        {
                            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
                            var tenantCurrencyId = tenantQuery.GetLocalCurrencyFromTenant(entityPM.Tenant);
                            CreateRevaluationJournal(entityPM, glAccountPM.ControlAccountId, tenantCurrencyId, entityPM.RevalJrnlRef1);
                        }
                    }
                }



                if (entityPM.Number == "get")
                {
                    entityPM.Number = CodeCounter.GetNumber("Reconciliation.Number", entityPM.Tenant).ToString();
                    entityPM.SearchFields = entityPM.AccountId + "," + entityPM.Number;
                }


            }


        }


        public void CreateRevaluationJournal(ReconciliationPM reconciliationPM, string controlAccountId, string tenantCurrencyId, string revalJrnlRef1)
        {
            try
            {
                FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(this.MainContext as IAccountingContext);
                var settings = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(reconciliationPM.Tenant);
                var diffAccountId = settings.ExchangeRateDiffGLAccountId;
                if (String.IsNullOrEmpty(diffAccountId))
                {
                    throw new ApplicationException("Exchange Rate Diff GLA. is not set");
                }

                // Start
                JournalPM newJournal = new JournalPM();

                newJournal.Tenant = reconciliationPM.Tenant;
                newJournal.CreateDate = DateTime.Now;
                newJournal.AccountingDate = DateTime.Now.Date;
                newJournal.TypeCode = JournalTypeValues.Regular;
                newJournal.StatusCode = JournalStatusTypeValues.InProcessing;
                newJournal.CreatedByUserId = reconciliationPM.CreatedByUserId;
                newJournal.AccountingEntityId = reconciliationPM.Id;
                newJournal.AccountingEntityReference = reconciliationPM.Number.ToString();
                newJournal.ExternalNo = null;
                newJournal.UpdateDate = DateTime.Now;
                newJournal.UpdatedByUserId = reconciliationPM.CreatedByUserId;
                newJournal.ApproveDate = reconciliationPM.CreateDate;
                newJournal.ApprovedByUserId = reconciliationPM.CreatedByUserId;
                newJournal.JournalLines = new List<JournalLinePM>();
                newJournal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                int lineCounter = 0;

                // Lines one pair for each currency
                var groupedReconciliationLines = reconciliationPM.ReconciliationLines
                    .GroupBy(line => line.CurrencyId);

                var newJournalReconcilesHS = new HashSet<JournalReconcilePM>();

                foreach (var group in groupedReconciliationLines)
                {
                    var groupLocalRecoAmount = Math.Round(group
                        .Where(x => x.CurrencyRate != null)
                        .Sum(x => x.ReconciliationAmount * x.CurrencyRate) ?? 0, 2);

                    if (groupLocalRecoAmount != 0)
                    {
                        newJournal.JournalLines.Add(new JournalLinePM()
                        {
                            Tenant = reconciliationPM.Tenant,
                            Line = ++lineCounter,
                            AccountingDate = DateTime.Now.Date,
                            DocumentDate = DateTime.Now.Date,
                            DueDate = DateTime.Now.Date,
                            ActionTypeCode = JournalActionTypes.Debit,
                            DebitAccountId = reconciliationPM.AccountId,
                            CreditAccountId = diffAccountId,
                            DebitControlAccountId = controlAccountId,
                            CreditControlAccountId = null,
                            LocalAmount = groupLocalRecoAmount,
                            ForeignAmount = 0,
                            CurrencyId = group.Key,
                            Reference1 = revalJrnlRef1,
                            Notes = "Revaluation on Foreign Currency Reco.",
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        });

                        newJournal.JournalLines.Add(new JournalLinePM()
                        {
                            Tenant = reconciliationPM.Tenant,
                            Line = ++lineCounter,
                            AccountingDate = DateTime.Now.Date,
                            DocumentDate = DateTime.Now.Date,
                            DueDate = DateTime.Now.Date,
                            ActionTypeCode = JournalActionTypes.Credit,
                            DebitAccountId = null,
                            CreditAccountId = diffAccountId,
                            DebitControlAccountId = null,
                            CreditControlAccountId = null,
                            LocalAmount = groupLocalRecoAmount,
                            ForeignAmount = 0,
                            CurrencyId = group.Key,
                            Reference1 = revalJrnlRef1,
                            Notes = "Revaluation on Foreign Currency Reco.",
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        });


                        List<ReconciliationLinePM> recoLines = reconciliationPM.ReconciliationLines
                            .Where(x => x.CurrencyId == group.Key)
                            .ToList();
                        newJournalReconcilesHS.Union(from item in recoLines
                                                     select new JournalReconcilePM()
                                                     {
                                                         ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                                         Tenant = reconciliationPM.Tenant,
                                                         LedgerTransactionId = item.TransactionId,
                                                         Line = item.Line,
                                                         CurrencyId = tenantCurrencyId,
                                                         ReconciliationAmount = groupLocalRecoAmount,
                                                         IsPartial = item.IsPartial,

                                                     }).ToHashSet();


                    }
                }

                newJournal.JournalReconciles = newJournalReconcilesHS.ToList();
                JournalUpdateService journalUpdateService = new JournalUpdateService(this.MainContext, this.AdditionalContexts, reconciliationPM.Tenant);
                journalUpdateService.Update(newJournal, true);


            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to create revaluation journal", ex);
            }
        }





        private static IQueryable<String> GetReconcileTransactionJournalIds(List<string> ledgerTransactionIds, int tenant)
        {
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(tenant);
            IQueryable<String> jIds = transactionQueryService.GetJournalIdsByIdList(ledgerTransactionIds, tenant);
            return jIds;
        }

        private static bool ExistsAccEntCodeByJIds(string accEntCode, IQueryable<string> jIds, int tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            bool rv = journalQueryService.ExistsAccEntCodeByJIds(accEntCode, jIds, tenant);
            return rv;
        }

        private static string GetFirstIdByAccEntCodeByJIds(string accEntCode, IQueryable<string> jIds, int tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            string firstId = journalQueryService.GetFirstIdByAccEntCodeByJIds(accEntCode, jIds, tenant);
            return firstId;
        }


        public static List<LedgerTransactionJournalLineLT> GetAPInvoiceLedgerTransactionsByIdList(List<String> ledgerTransactionIds, int tenant)
        {
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(tenant);
            List<LedgerTransactionJournalLineLT> ledgerTransactionLineLTs = transactionQueryService.GetAPInvoiceLedgerTransactionsByIdList(ledgerTransactionIds, tenant);
            return ledgerTransactionLineLTs;
        }

        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "ReconciliationUpdateService", new DateTime(2023, 6, 1));
        }
        void PushSearchFieldText(ReconciliationPM entityPM, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string trimmedText = text.ToLower().Trim();

                if (entityPM.SearchFields == null)
                {
                    entityPM.SearchFields = "";
                }
                else
                {
                    bool isTextFoundInField = entityPM.SearchFields.Contains(trimmedText);
                    if (isTextFoundInField == false)
                    {
                        entityPM.SearchFields += trimmedText + ",";
                    }
                }
            }
        }

        public ReconciliationPM CancellReconciliation(string reconciliationId, int tenant)
        {
            var reconciliationQueryService = new ReconciliationQueryService(this.MainContext as IAccountingContext);
            var pm = reconciliationQueryService.GetSingle(reconciliationId, true, false);
            if (pm == null)
            {
                return null;
            }
            if (pm.Tenant != tenant)
            {
                return null;
            }
            if (pm.IsCancelled)
            {
                throw new ApplicationException("Reconciliation already  Cancelled");
            }
            pm.IsCancelled = true;
            pm.ChangeSetOp = ChangeSetOperation.Update;
            this.Update(pm, true);

            return pm;
        }


        protected override void OnUpdating(ReconciliationPM entityPM, Reconciliation entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                throw new ApplicationException("Delete Reconciliation Is not allowed (try to cancel)");
            }
            if (entityPOCO.IsCancelled)
            {
                throw new ApplicationException("Reconciliation " + entityPOCO.Number + " is Cancelled (update not allowed)");
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (!entityPOCO.IsCancelled && entityPM.IsCancelled)
                {
                    this._CancelledAction = true;
                }
                else
                {
                    throw new ApplicationException("Updating Reconciliation allowed only to cancel");
                }
            }


            this.GLAccountRecocileDataUpSert(_CancelledAction, entityPM);
            base.OnUpdating(entityPM, entityPOCO);
        }

        private void GLAccountRecocileDataUpSert(bool cancelledAction, ReconciliationPM entityPM)
        {
            int tenant = entityPM.Tenant;
            string accountId = entityPM.AccountId;
            DateTime? createDate = entityPM.CreateDate;
            string createdByUserId = entityPM.CreatedByUserId;

            var gLAccountRecocileDataQueryService = new GLAccountRecocileDataQueryService(this.MainContext as IAccountingContext);
            ChangeSetOperation changeSetOperation = ChangeSetOperation.Insert;
            if (gLAccountRecocileDataQueryService.Exists(accountId, tenant))
            {
                changeSetOperation = ChangeSetOperation.Update;
            }
            if (cancelledAction)
            {
                string cancelledReconciliationId = entityPM.Id;
                var reconciliationRepository = new ReconciliationRepository(this.MainContext as IAccountingContext);
                var last = reconciliationRepository.GetLastOpenReconciliation(tenant, accountId, cancelledReconciliationId);
                if (last == null)
                {
                    createDate = null;
                    createdByUserId = null;

                }
                else
                {
                    createDate = last.CreateDate;
                    createdByUserId = last.CreatedByUserId;

                }
            }
            var gLAccountRecocileDataUpdateService = new GLAccountRecocileDataUpdateService(this.MainContext, new Dictionary<string, IContext>(), tenant);
            gLAccountRecocileDataUpdateService.Update(new GLAccountRecocileDataPM()
            {
                AccountId = accountId,
                Tenant = tenant,
                ChangeSetOp = changeSetOperation,
                LastReconcileDateTime = createDate,
                LastReconciledByUserId = createdByUserId

            }, true);
        }

        private bool IsMonthOpenForAccountingDate(DateTime accountingDate, int tenant)
        {

            var typeregular = "1"; //1 Regular רגיל        1,Regular,רגיל   0
            var accountingPeriodQueryService = new AccountingPeriodQueryService(tenant);
            var accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodByType(typeregular, tenant); ;

            return
            JournalValidatorNotStatic
                 .IsMonthOpenForAccountingDate(
                accountingPeriodsByTypeRegular.AsQueryable(),
                 new DateTime(accountingDate.Year, accountingDate.Month, 1)
                 );

        }

        protected override void UpdateComposition(ReconciliationPM entityPM)
        {
            try
            {

                if (this._CancelledAction)
                {
                    bool angular_DoNotUse_CancellReconciliation_Method = true;
                    if (angular_DoNotUse_CancellReconciliation_Method)
                    {
                        if (entityPM.ReconciliationLines.Count == 0)
                        {
                            var reconciliationLineQueryService = new ReconciliationLineQueryService(this.MainContext as IAccountingContext);
                            var linePMs = reconciliationLineQueryService.GetLinePMsByReconciliationIdAndTenant(entityPM.Id, entityPM.Tenant);
                            entityPM.ReconciliationLines.AddRange(linePMs);
                        }
                    }
                    UpdateLedgerTransaction(entityPM);
                    return;
                }

                if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    if (entityPM.ReconciliationLines.Count > 0)
                    {
                        int i = 0;
                        foreach (ReconciliationLinePM item in entityPM.ReconciliationLines)
                        {
                            if (item.Line > i) i = item.Line;
                        }

                        foreach (ReconciliationLinePM item in entityPM.ReconciliationLines)
                        {
                            item.ReconciliationId = entityPM.Id;
                            if (item.Line == 0) item.Line = ++i;
                        }
                    }



                    UpdateLedgerTransaction(entityPM);

                    ReconciliationLineUpdateService reconciliationLineUpdateService = new ReconciliationLineUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    reconciliationLineUpdateService.UpdateMulti(entityPM.ReconciliationLines, entityPM.DeletedReconciliationLines, entityPM, false);
                }

            }
            finally
            {
                base.UpdateComposition(entityPM);
            }

        }

        private void UpdateLedgerTransaction(ReconciliationPM entityPM)
        {
            var transactionIdList = entityPM.ReconciliationLines.Select(rec => rec.TransactionId).ToList();
            var qs = new LedgerTransactionQueryService(entityPM.Tenant);
            var LedgerTransactionPMsUpdated = qs.GetLedgerTransactionPMsByIdList(transactionIdList, entityPM.Tenant);
            if (transactionIdList.Count() == 0)
            {
                throw new ApplicationException("Unable to UpdateLedgerTransaction  due there is any ReconciliationLines");
            }
            foreach (var reconciliationLine in entityPM.ReconciliationLines)
            {
                var ledgerTransactionPM = LedgerTransactionPMsUpdated.First(r => r.Id == reconciliationLine.TransactionId);

                ledgerTransactionPM.ChangeSetOp = ChangeSetOperation.Update;

                if (this._CancelledAction)
                {
                    ledgerTransactionPM.OpenAmount = ledgerTransactionPM.OpenAmount + reconciliationLine.ReconciliationAmount;
                    ledgerTransactionPM.IsReconciled = false;
                }
                else
                {

                    if (entityPM.CreatedByReconciliationAfterConversion)
                    {
                        
                        var repo = new JournalLineRepository(entityPM.Tenant);
                        var jlList = repo.GetJournalLineByLedgerTransactionIdList(transactionIdList, entityPM.Tenant);
                        List<string> errorsList = new List<string>();
                        ReconciliationValidator.Validate_CreatedByReconciliationAfterConversion(errorsList, jlList);
                        if (errorsList.Count > 0)
                        {
                            var errLines = string.Join(Environment.NewLine, errorsList);
                            throw new ApplicationException(errLines);
                        }
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("due CreatedByReconciliationAfterConversion do not   UpdateLedgerTransaction - dont change open Amount ");
                    }
                    else
                    {
                        ledgerTransactionPM.OpenAmount = ledgerTransactionPM.OpenAmount - reconciliationLine.ReconciliationAmount;
                    }
                    reconciliationLine.IsPartial = true;
                    if (ledgerTransactionPM.OpenAmount == 0)
                    {
                        reconciliationLine.IsPartial = false;
                    }

                    ledgerTransactionPM.IsReconciled = !reconciliationLine.IsPartial;
                }
            }
            foreach (var ledgerTransactionGroup in LedgerTransactionPMsUpdated
                                .Where(x => x.SourceTypeCode == "4" || x.SourceTypeCode == "2")
                                .GroupBy(x => new { x.AccountId, x.JournalId }))
            {
                    var ledgerTransactionPM = ledgerTransactionGroup.First();
                    bool isAPInvoice = ledgerTransactionPM.SourceTypeCode == "4";

                    IInvoiceContext invoiceContext = InvoiceContext.GetContext(ledgerTransactionPM.Tenant);
                    dynamic invoiceQuery = null;
                    dynamic invoiceService = null;
                    if (isAPInvoice)
                    {
                        var invoiceRepository = new APInvoiceRepository(invoiceContext);
                        invoiceQuery = new APInvoiceQuery(invoiceRepository);
                        invoiceService = new APInvoiceService(invoiceContext, ledgerTransactionPM.Tenant);
                    }
                    else
                    {
                        var invoiceRepository = new ARInvoiceRepository(invoiceContext);
                        invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                        invoiceService = new ARInvoiceService(invoiceContext, ledgerTransactionPM.Tenant);
                    }

                    var invoice = invoiceQuery.GetSinglePM(ledgerTransactionPM.SourceId, ledgerTransactionPM.Tenant);
                    string relevantGLAccountId = isAPInvoice ? invoice.VendorGLAccountId : invoice.BillToGLAccountId;


                    var updatedLedgerTransactions = ledgerTransactionGroup.Where(t => t.AccountId == relevantGLAccountId).ToList();

                    var ledgerTransactionQueryService = new LedgerTransactionQueryService(ledgerTransactionPM.Tenant);
                    var dbTransactions = ledgerTransactionQueryService.GetByJournalAndAccountId(
                        ledgerTransactionPM.JournalId, relevantGLAccountId, ledgerTransactionPM.Tenant);

                    var transactionIds = updatedLedgerTransactions.Select(t => t.Id).ToList();
                    var unmatchedTransactions = dbTransactions.Where(x => !transactionIds.Contains(x.Id)).ToList();

                    if (!unmatchedTransactions.Any() && !updatedLedgerTransactions.Any())
                        continue;

                    decimal totalOpenAmount = unmatchedTransactions.Sum(x => x.OpenAmount) + updatedLedgerTransactions.Sum(x => x.OpenAmount);

                    var glAccountQueryService = new GLAccountQueryService(entityPM.Tenant);
                    var reconcileMethodCode = glAccountQueryService.GetSingle(entityPM.AccountId, false, false).ReconcileMethodCode;

                    decimal totalAmount = reconcileMethodCode == ReconcileMethodValues.LocalCurrency
                        ? unmatchedTransactions.Sum(x => x.LocalAmountCredit) + updatedLedgerTransactions.Sum(x => x.LocalAmountCredit)
                        : unmatchedTransactions.Sum(x => x.ForeignAmountCredit) + updatedLedgerTransactions.Sum(x => x.ForeignAmountCredit);

                    invoice.IsClosed = totalOpenAmount == 0;
                    if (invoice.StatusCode != "VD" && invoice.StatusCode !="AC" && invoice.StatusCode != "AR") invoice.StatusCode = totalOpenAmount == 0 ? "PD"
                        : Math.Abs(ledgerTransactionPM.OpenAmount) < totalAmount ? "PP" : "AD";

                    SecurityUtility.IsWorkerRoleCall = true;

                    invoiceService.Update(invoice, true);

            }

            var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            ledgerTransactionUpdateService._CancelledAction = this._CancelledAction;
            ledgerTransactionUpdateService.UpdateMulti(LedgerTransactionPMsUpdated, new List<LedgerTransactionPM>(), entityPM, false);

            if (this._CancelledAction || !updateGLAccountAgingDataUsingWR)
            {
                UpdateGLaccountAgingData(entityPM);
            }
        }

        public void UpdateGLaccountAgingData(ReconciliationPM entityPM) {
            var newContextWhileStreamingLedger = AccountingContext.GetContext(entityPM.Tenant);
            var reconciliationUpdateAgingService = new ReconciliationUpdateAgingService(newContextWhileStreamingLedger);
            var deltaGLAccountAgingDataPM = reconciliationUpdateAgingService.GetDelta(this._CancelledAction, entityPM);
            if (!string.IsNullOrWhiteSpace(deltaGLAccountAgingDataPM.AccountId))
            {
                if (
                    deltaGLAccountAgingDataPM.Tenant == 127 &&
                    Math.Abs(deltaGLAccountAgingDataPM.TotalOpenTransactions.GetValueOrDefault()) > 10_000
                    )
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Ohad: Given Reconciliation Update And tenant == Ship2u and the Delta of TotalOpenTransactions > 10,000 ,Do not Update (Cause lock cause Fail Journal Streaming  ) .... ");
                }
                else
                {
                    reconciliationUpdateAgingService.UpdateDelta(deltaGLAccountAgingDataPM, false);
                    newContextWhileStreamingLedger.SaveChanges();// MUST SAVE DUE NEW CONTEXT !!!

                }
            }
        }

        public bool SuppressResetDraftOpenReconciliation { get; set; }
        protected override void AfterUpdating(ReconciliationPM entityPM, EntityPM entityParentPM)
        {
            // Draft reconciliation
            //

            // incase insert changeset: the accountCurrencyId is null, so I will fill it 
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert && entityPM.AccountId != null)
            {
                FillGLAccountsFields(entityPM);
            }

            // update connected ARPayment 
            ARPaymentReconciliationService arpRecoService = new ARPaymentReconciliationService(entityPM.Tenant);
            arpRecoService.UpdatePaymentOpenAmountAndStatusForReconciliaitonLT(entityPM);
            arpRecoService.UpdateConnectedInvoicesLT(entityPM);

        }

        private static void FillGLAccountsFields(ReconciliationPM entityPM)
        {
            GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM account = query.GetSingle(entityPM.AccountId, false, false);
            if (account != null && !(account.IsMultiCurrency == true && account.ReconcileMethodCode == "1"))
            {
                entityPM.AccountName = account.LocalName;
                entityPM.AccountNumber = account.DisplayNumber;
                entityPM.AccountCurrencyId = account.CurrencyId;
                entityPM.CurrencyCode = account.CurrencyCode;
                entityPM.AccountReconcileMethodCode = account.ReconcileMethodCode;
            }
        }

        protected override void Validate(ReconciliationPM entityPM)
        {
            var validContext = AccountingValidationContextServiceProvider.NewReconciliationValidatorContext((MainContext as IAccountingContext), entityPM);
            ValidationResult result = ReconciliationValidator.IsReconciliationValid(entityPM, validContext);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        protected override void Trace(ReconciliationPM entityPM, Reconciliation entityPOCO, string changesXml)
        {
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "Reconciliation",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else
            {
                if (entityPM.IsCancelled != entityPOCO.IsCancelled)
                {
                    //create trace event with created type.
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = loggedContact.Id,
                        ObjectTableName = "Reconciliation",
                        IsAddedManually = false,
                        EventTypeCode = "CNCL",
                        Notes = "",
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }

            base.Trace(entityPM, entityPOCO, changesXml);
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }


            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}
