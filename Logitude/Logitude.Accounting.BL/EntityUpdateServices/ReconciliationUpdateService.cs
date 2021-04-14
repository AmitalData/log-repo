using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.Validators;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.CloseTables;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.CloseTables;
using System.Diagnostics;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.BL.CoreBL.Reports;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconciliationUpdateService : EntityUpdateService<Reconciliation, ReconciliationPM, EntityPM>
    {
        private bool _CancelledAction;

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
                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

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
                throw new Exception("ChangeSetOperation.Delete ???");
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
                    throw new Exception("BLException :Approved Reconciliation Can Only Change To IsCancelled Property");
                }
                if (entityPM.ReconciliationLines.Any(jl => jl.ChangeSetOp != ChangeSetOperation.None))
                {
                    throw new Exception("BLException :Approved Reconciliation Can Only Change To IsCancelled Property");
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

                // get transactions
                List<string> ledgerTransactionIds = entityPM.ReconciliationLines.Where(d => d.TransactionId != null).Select(d => d.TransactionId).ToList();
                List<LedgerTransactionPM> ledgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(ledgerTransactionIds, entityPM.Tenant);

                
                if (ledgerTransactions.Any(d=>d.SourceTypeCode == AccountingEntityValues.ARPayment))
                {
                    LedgerTransactionPM paymentTransaction = ledgerTransactions.Find(d => d.SourceTypeCode == AccountingEntityValues.ARPayment);
                    if (paymentTransaction == null) throw new ApplicationException("Cannot find payment transaction on reco lines");
                    foreach (ReconciliationLinePM recoLine in entityPM.ReconciliationLines)
                    {
                        if(recoLine.TransactionId != paymentTransaction.Id)
                        {
                            recoLine.ReconciledWithTransactionId = paymentTransaction.Id;
                        }
                    }

                }


                //foreach (var transactionPM in LedgerTransactions)
                //{

                //    if (transactionPM.SourceTypeCode == "3")
                //    {
                //        List<ARPaymentChequePM> aRPaymentChequePMs = aRPaymentChequeQueryService.GetListByPaymentId(transactionPM.SourceId, entityPM.Tenant);

                //        foreach (ARPaymentChequePM item in aRPaymentChequePMs)
                //        {
                //            item.StatusCode = "8";
                //        item.ChangeSetOp = ChangeSetOperation.Update;
                //            ARPaymentChequeUpdateService aRPaymentChequeUpdateService = new ARPaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                //            aRPaymentChequeUpdateService.Update(item, true);
                //        }
                //    }

                //}





                //var validContext = AccountingValidationContextServiceProvider.NewReconciliationValidatorContext((MainContext as IAccountingContext), entityPM);
                //var validationResult = ReconciliationValidator.IsReconciliationValid(entityPM, validContext);
                //if (validationResult != null)
                //{
                //    string errorText = validationResult.ErrorMessage;//+ ", Number=" + _JournalPM.ExternalNo + @"/" + _JournalPM.Id;
                //                                                     //ThrowException(errorText);
                //    throw new Exception(errorText);
                //}



                //if (entityPM.IsCancelled == null)
                //{
                //    entityPM.IsCancelled = false;
                //}
                if (entityPM.Number == "get")
                {
                    entityPM.Number = CodeCounter.GetNumber("Reconciliation.Number", entityPM.Tenant).ToString();
                    entityPM.SearchFields = entityPM.AccountId + "," + entityPM.Number;
                }


            }


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
                throw new Exception("Reconciliation already  Cancelled");
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
                throw new Exception("Delete Reconciliation Is not allowed (try to Cancell)");
            }
            if (entityPOCO.IsCancelled)
            {
                throw new Exception("Reconciliation Is Cancelled (update not allowed)");
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (!entityPOCO.IsCancelled && entityPM.IsCancelled)
                {
                    this._CancelledAction = true;
                }
                else
                {
                    throw new Exception("Updating Reconciliation allowed only to cancell");
                }
            }
            if (_CancelledAction == true)
            {
                //get journal of reconciliation - WI39779
                JournalQueryService journalQuery = new JournalQueryService(entityPM.Tenant);
                JournalPM journal = journalQuery
                    //.GetByAccountingEntityId(entityPM.Id, entityPM.Tenant);
                //10  התאמה Adjustment
                .GetByAccountingEntityIdAndAccountingEntityCode(entityPM.Id, "10", entityPM.Tenant);
                if (journal != null && IsMonthOpenForAccountingDate(journal.AccountingDate, entityPM.Tenant))
                {
                    // Void it!
                    var tenant = entityPM.Tenant;
                    IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                    var service = new JournalVoidUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    var StornoOverrideM = new StornoOverrideM()
                    {
                        AccountingEntityCode = journal.AccountingEntityCode,
                        AccountingEntityId = journal.AccountingEntityId,
                        AccountingEntityReference = journal.AccountingEntityReference,
                    };
                    service.VoidJournal(journal.Id, tenant, StornoOverrideM);
                    //Voided
                }
            }
            this.GLAccountRecocileDataUpSert(_CancelledAction, entityPM);
            base.OnUpdating(entityPM, entityPOCO);
        }

        private void GLAccountRecocileDataUpSert(bool cancelledAction, ReconciliationPM entityPM)
        {
            int tenant= entityPM.Tenant;
            string accountId= entityPM.AccountId; 
            DateTime? createDate =entityPM.CreateDate; 
            string createdByUserId= entityPM.CreatedByUserId;
        
            var gLAccountRecocileDataQueryService = new GLAccountRecocileDataQueryService(this.MainContext as IAccountingContext) ;
            var pm=gLAccountRecocileDataQueryService.GetSingle(accountId, false, false);
            ChangeSetOperation changeSetOperation = ChangeSetOperation.Insert;
            if (pm != null)
            {
                changeSetOperation = ChangeSetOperation.Update;
            }
            if (cancelledAction)
            {
                string cancelledReconciliationId = entityPM.Id;
                var reconciliationRepository = new ReconciliationRepository(this.MainContext as IAccountingContext);
                var last=reconciliationRepository.GetLastOpenReconciliation(tenant, accountId, cancelledReconciliationId);
                if (last==null)
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

            var typeregular = "1"; //1	Regular	רגיל	1,Regular,רגיל	0
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
                            var reconciliationQueryService = new ReconciliationQueryService(this.MainContext as IAccountingContext);
                            var pm = reconciliationQueryService.GetSingle(entityPM.Id, true, false);
                            entityPM.ReconciliationLines.AddRange(pm.ReconciliationLines);
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
                throw new Exception("Unable to UpdateLedgerTransaction  due there is any ReconciliationLines");
            }
            foreach (var reconciliationLine in entityPM.ReconciliationLines)
            {
                var ledgerTransactionPM = LedgerTransactionPMsUpdated.First(r => r.Id == reconciliationLine.TransactionId);

                ledgerTransactionPM.ChangeSetOp = ChangeSetOperation.Update;
                //ledgerTransactionPM.OpenAmount -= reconciliationLine.ReconciliationAmount;
                if (this._CancelledAction)
                {
                    ledgerTransactionPM.OpenAmount = ledgerTransactionPM.OpenAmount + reconciliationLine.ReconciliationAmount;
                    ledgerTransactionPM.IsReconciled = false;
                }
                else
                {
                    
                    if (entityPM.CreatedByReconciliationAfterConversion)
                    {
                        //var transactionIdList = entityPM.ReconciliationLines.Select(rec => rec.TransactionId).ToList();
                        var repo = new JournalLineRepository(entityPM.Tenant);
                        var jlList = repo.GetJournalLineByLedgerTransactionIdList(transactionIdList, entityPM.Tenant);
                        List<string> errorsList = new List<string>();
                        ReconciliationValidator.Validate_CreatedByReconciliationAfterConversion(errorsList, jlList);
                        if (errorsList.Count > 0)
                        {
                            var errLines = string.Join(Environment.NewLine, errorsList);
                            throw new Exception(errLines);
                        }
                        Debug.WriteLine("due CreatedByReconciliationAfterConversion do not   UpdateLedgerTransaction - dont change open Amount ");
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
            var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            ledgerTransactionUpdateService._CancelledAction = this._CancelledAction;
            ledgerTransactionUpdateService.UpdateMulti(LedgerTransactionPMsUpdated, new List<LedgerTransactionPM>(), entityPM, false);

            bool getNewContextWhileStreamingLedger = true;
            if (getNewContextWhileStreamingLedger)
            {
                var newContextWhileStreamingLedger = AccountingContext.GetContext(entityPM.Tenant);
                var reconciliationUpdateAgingService = new ReconciliationUpdateAgingService(newContextWhileStreamingLedger);
                var deltaGLAccountAgingDataPM = reconciliationUpdateAgingService.GetDelta(this._CancelledAction, entityPM);
                if (!string.IsNullOrWhiteSpace(deltaGLAccountAgingDataPM.AccountId))
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
            if (!SuppressResetDraftOpenReconciliation)
            {
                var repoLedger = new LedgerTransactionRepository(MainContext as IAccountingContext);
                repoLedger.ResetDraftOpenReconciliation(entityPM.AccountId, entityPM.Tenant);
            }

            // incase insert changeset: the accountCurrencyId is null, so I will fill it 
            if(entityPM.ChangeSetOp == ChangeSetOperation.Insert && entityPM.AccountId != null)
            {
                FillGLAccountsFields(entityPM);
            }

            // update connected ARPayment 
            ARPaymentReconciliationService arpRecoService = new ARPaymentReconciliationService(entityPM.Tenant);
            arpRecoService.UpdatePaymentOpenAmountAndStatusForReconciliaiton(entityPM);
            arpRecoService.UpdateConnectedInvoices(entityPM);

        }

        private static void FillGLAccountsFields(ReconciliationPM entityPM)
        {
            GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM account = query.GetSingle(entityPM.AccountId, false, false);
            if (account != null)
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

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
        
    }
}
