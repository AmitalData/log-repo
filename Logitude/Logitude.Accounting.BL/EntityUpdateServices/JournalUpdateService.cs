using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.EntityDataMappings;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Web;
using Logitude.Server.Tools.QueueService;
//using Microsoft.Practices.Unity.UnityContainerExtensions;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.BL.CloseTables;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.Accounting.BL.CoreBL.InterestTrans;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalUpdateService : EntityUpdateService<Journal, JournalPM, EntityPM>
        , IJournalUpdateService
    {
        class JournalLineUpdateServicePriv : JournalLineUpdateService
        {
            public JournalLineUpdateServicePriv(IContext mainContext, Dictionary<string, IContext> additionalContexts, int tenant)
                : base(mainContext, additionalContexts, tenant)
            { }
        }

        private class JournalRepositoryPriv : JournalRepository
        {

            public JournalRepositoryPriv(IAccountingContext mainContext)
                : base(mainContext)
            { }
        }
        
        protected StornoOverrideM _StornoOverrideM;

        protected override void AddContext(JournalPM myTEntityPM)
        {
            base.AddContext(myTEntityPM);
            this.Repository = GetJournalRepositoryPriv();
                //new JournalRepositoryPriv((IAccountingContext)this.MainContext);
            //this.entityRepository.SetInsureUsingOnlyByUpdateService();
        }

        protected override void OnCreating(JournalPM entityPM, EntityPM entityParentPM)
        {
            var JournalUpdateOnCreatingFactory = new JournalUpdateOnCreating.Factory();
            var JournalUpdateInsert = JournalUpdateOnCreatingFactory.Create(this.MainContext as IAccountingContext);
            JournalUpdateInsert.OnCreating(entityPM, entityParentPM);

          
        }

        protected override void UpdateComposition(JournalPM entityPM)
        {

            /// 
            var JournalLineUpdateServicePriv = new JournalLineUpdateServicePriv
           //JournalLineUpdateService journalLineUpdateService = new JournalLineUpdateService
           (MainContext, new Dictionary<string, IContext>(), Tenant);
            UpdateLedgerTransactionWithNewValuesFromJournalLines(entityPM);
            bool supperssSaveOnUpdateMultiDueIsFaster = true;
            JournalLineUpdateServicePriv.UpdateMulti(entityPM.JournalLines, entityPM.DeletedJournalLines, entityPM,

                !supperssSaveOnUpdateMultiDueIsFaster);
            if (supperssSaveOnUpdateMultiDueIsFaster)
            {
                this.SubmitChanges();
            }
            //GetIQueryableLedgerTransactionsByGLAccountIdsList
            //base.UpdateComposition(entityPM);
            //while insert do once insert JournalReconciles +  Update ledgerTrasaction to  InReconcileProgress !!!!
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                var journalReconcileUpdateService = new JournalReconcileUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                journalReconcileUpdateService.UpdateMulti(entityPM.JournalReconciles, entityPM.DeletedJournalReconciles, entityPM,
                    !supperssSaveOnUpdateMultiDueIsFaster);
                if (supperssSaveOnUpdateMultiDueIsFaster)
                {
                    this.SubmitChanges();
                }
                

                var listTransactionId = entityPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
                if (listTransactionId.Count > 0)
                {
                    var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
                    ledgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, Tenant, true);
                }
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                var journalReconcileUpdateService = new JournalExternalReconcileUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                journalReconcileUpdateService.UpdateMulti(entityPM.JournalExternalReconciles, entityPM.DeletedJournalExternalReconciles, entityPM,
                    !supperssSaveOnUpdateMultiDueIsFaster);
                if (supperssSaveOnUpdateMultiDueIsFaster)
                {
                    this.SubmitChanges();
                }


                var listTransactionId = entityPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList();
                if (listTransactionId.Count > 0)
                {
                    var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
                    ledgerTransactionUpdateService.Update_InProgressExternalReconcile(listTransactionId, Tenant, true);
                }
                var listReconcileExternalPageLineId = entityPM.JournalExternalReconciles.Select(r => r.ReconcileExternalPageLineId).ToList();
                if (listReconcileExternalPageLineId.Count > 0)
                {
                    var reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
                    reconcileExternalPageLineUpdateService.Update_InProgressExternalReconcile(listReconcileExternalPageLineId, Tenant, true);
                }

            }
        }

        private void UpdateLedgerTransactionWithNewValuesFromJournalLines(JournalPM entityPM)
        {
            foreach(JournalLinePM JournalLine in entityPM.JournalLines) {
                if (JournalLine != null)
                {
                    JournalLinePM oldJournalLine = GetOldJournalLineFromDB(JournalLine);

                    if (oldJournalLine != null)
                    {
                        CheckIfJournalLinesChangedAndUpdateRelatedLedgerTransaction(entityPM, JournalLine, oldJournalLine);
                    }
                }
            }
        }

        private static JournalLinePM GetOldJournalLineFromDB(JournalLinePM firstJournalLine)
        {
            JournalLineQueryService journalLineQueryService = new JournalLineQueryService(firstJournalLine.Tenant);
            JournalLinePM journalLine = journalLineQueryService.GetSingle(firstJournalLine.JournalId, firstJournalLine.Line, false, false);
            return journalLine;
        }

        private void CheckIfJournalLinesChangedAndUpdateRelatedLedgerTransaction(JournalPM entityPM, JournalLinePM JournalLine, JournalLinePM oldJournalLine)
        {
            List<LedgerTransactionPM> allTransactionsRelatedToJournal = GetAllTransactionsRelatedToJournal(entityPM, JournalLine);
            var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(this.MainContext as IAccountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            var isOneValueOfJournalLineChanged = JournalLine.Notes != oldJournalLine.Notes || JournalLine.Reference1 != oldJournalLine.Reference1 || JournalLine.Reference2 != oldJournalLine.Reference2 || JournalLine.Reference3 != oldJournalLine.Reference3;
            if (isOneValueOfJournalLineChanged)
            {
                UpdateAllLedgerTransactionsRelatedToJournalLine(JournalLine, allTransactionsRelatedToJournal, ledgerTransactionUpdateService);
            }
        }

        private static List<LedgerTransactionPM> GetAllTransactionsRelatedToJournal(JournalPM entityPM, JournalLinePM JournalLine)
        {
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(entityPM.Tenant);
            List<LedgerTransactionPM> allTransactionsRelatedToJournal = ledgerTransactionQueryService.GetByJournalLineIdAndLine(entityPM.Id, JournalLine.Line, entityPM.Tenant).ToList();
            return allTransactionsRelatedToJournal;
        }


        private static void UpdateAllLedgerTransactionsRelatedToJournalLine(JournalLinePM JournalLine, List<LedgerTransactionPM> allTransactionsRelatedToJournal, LedgerTransactionUpdateService ledgerTransactionUpdateService)
        {
            foreach (LedgerTransactionPM transaction in allTransactionsRelatedToJournal)
            {
                transaction.Notes = JournalLine.Notes;
                transaction.Reference1 = JournalLine.Reference1;
                transaction.Reference2 = JournalLine.Reference2;
                transaction.Reference3 = JournalLine.Reference3;

                transaction.ChangeSetOp = ChangeSetOperation.Update;
                ledgerTransactionUpdateService.Update(transaction, false, null);
            }
        }

       
        protected override void OnUpdating(JournalPM entityPM, Journal entityPOCO)
        {
            var journalUpdate = GetJournalOnUpdtatingObject();

            journalUpdate.OnUpdating(entityPM, entityPOCO, ChangeTrackingEntityPM);

            
        }
        private JournalRepository GetJournalRepositoryPriv()
        {
            return (new JournalRepositoryPriv((IAccountingContext)this.MainContext) as JournalRepository);
        }


        internal void UpdateWhileStreaming(int tenant, string id, Action<Journal> updatePoco)
        {
            var repoPriv = GetJournalRepositoryPriv();
            repoPriv.UpdateWhileStreaming(tenant, id, updatePoco);
        }

        internal void SetStatusCodeFailed(string seedJournalId, int tenant)
        {
            var repoPriv = GetJournalRepositoryPriv();
            var poco = repoPriv.GetSingle(seedJournalId, tenant);
            poco.StatusCode = ((int)Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Failed).ToString();
            repoPriv.Update(poco);
            
        }

        public virtual JournalUpdateOnUpdating GetJournalOnUpdtatingObject()
        {
            var journalUpdate = new JournalUpdateOnUpdating(this.MainContext as IAccountingContext);
          
            return journalUpdate;
        }

   
        protected override void Trace(JournalPM entityPM, Journal entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "Journal",
                    IsAddedManually = false,
                    EventTypeCode = "JCR",

                };
                EventTracer.CreateTraceEvent(eventTracerArgs);



                if (entityPM.StatusCode == "1") //Saved
                {

                    String notes = "Changed to: " + TraceIt_JournalStatusName(entityPM.StatusCode, entityPM.Tenant);

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "Journal",
                        IsAddedManually = false,
                        EventTypeCode = "JSV",

                    });
                }
                else if (entityPM.StatusCode == "2") //Approved
                {

                    String notes = "Changed to: " + TraceIt_JournalStatusName(entityPM.StatusCode, entityPM.Tenant);
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "Journal",
                        IsAddedManually = false,
                        EventTypeCode = "JAP",

                    });
                }
                if(entityPM.Copied)
                CreateCopyJournalEvent(entityPM, contact);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (entityPM.StatusCode != entityPOCO.StatusCode)
                {
                    if (entityPM.StatusCode == "1") //Save
                    {
                        ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                        Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                        // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                        String notes = "Previous status code: " + TraceIt_JournalStatusName(entityPOCO.StatusCode, entityPM.Tenant) + ", Changed to: " + TraceIt_JournalStatusName(entityPM.StatusCode, entityPM.Tenant);
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            EntityId = entityPM.Id,
                            Tenant = entityPM.Tenant,
                            UserId = contact.Id,
                            ObjectTableName = "Journal",
                            IsAddedManually = false,
                            EventTypeCode = "JSV",
                            Notes = notes,
                        });

                    }
                    else if (entityPM.StatusCode == "2")  //Approved
                    {
                        ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                        Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                        // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                        String notes = "Previous status code: " + TraceIt_JournalStatusName(entityPOCO.StatusCode, entityPM.Tenant) + ", Changed to: " + TraceIt_JournalStatusName(entityPM.StatusCode, entityPM.Tenant);
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            EntityId = entityPM.Id,
                            Tenant = entityPM.Tenant,
                            UserId = contact.Id,
                            ObjectTableName = "Journal",
                            IsAddedManually = false,
                            EventTypeCode = "JAP",
                            Notes = notes,

                        });

                    }
                    else if (entityPM.StatusCode == "3")  //Approved
                    {
                        ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                        Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                        // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                        String notes = "Previous status code: " + TraceIt_JournalStatusName(entityPOCO.StatusCode, entityPM.Tenant) + ", Changed to: " + TraceIt_JournalStatusName(entityPM.StatusCode, entityPM.Tenant);
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            EntityId = entityPM.Id,
                            Tenant = entityPM.Tenant,
                            UserId = contact.Id,
                            ObjectTableName = "Journal",
                            IsAddedManually = false,
                            EventTypeCode = "JVD",
                            Notes = notes,

                        });

                    }

                }
                else
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    bool showLocals = !contact.DontShowLocalLabels;
                    bool isJournalLineUpdated = false;
                    foreach (JournalLinePM journalLinePM in entityPM.JournalLines)
                    {
                        JournalLinePM oldJournalLine = GetOldJournalLineFromDB(journalLinePM);
                        if (oldJournalLine != null)
                        {
                            var isOneOfJournalLineValuesUpdated = journalLinePM.Notes != oldJournalLine.Notes || journalLinePM.Reference1 != oldJournalLine.Reference1 || journalLinePM.Reference2 != oldJournalLine.Reference2 || journalLinePM.Reference3 != oldJournalLine.Reference3;

                            if (isOneOfJournalLineValuesUpdated)
                            {
                                isJournalLineUpdated = true;
                                string journalNotes = SetJournalLineEventNotes(entityPM, showLocals, journalLinePM, oldJournalLine);
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "Journal",
                                    IsAddedManually = false,
                                    EventTypeCode = "JNUP",
                                    Notes = journalNotes,

                                });
                            }
                        }

                    }

                    if (!isJournalLineUpdated) { 
                    String notes = "Journal Updated";
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "Journal",
                        IsAddedManually = false,
                        EventTypeCode = "JUP",
                        Notes = notes,

                    });
                    }

                }


            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        private static string SetJournalLineEventNotes(JournalPM entityPM, bool showLocals, JournalLinePM journalLinePM, JournalLinePM oldJournalLine)
        {
            var eventNotes = string.Concat(TranslateTextsClass.Translate("Journal.M.Line", entityPM.Tenant, showLocals), ' ', oldJournalLine.Line, "\n");
            if(journalLinePM.Notes != oldJournalLine.Notes)
            {
                eventNotes += string.Concat(TranslateTextsClass.Translate("Journal.M.Note", entityPM.Tenant, showLocals),": ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), oldJournalLine.Notes, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), journalLinePM.Notes, "\n");
            }

            if (journalLinePM.Reference1 != oldJournalLine.Reference1)
            {
                eventNotes += string.Concat(TranslateTextsClass.Translate("Journal.CH.Reference1", entityPM.Tenant, showLocals), ": ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), oldJournalLine.Reference1, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), journalLinePM.Reference1, "\n");
            }

            if (journalLinePM.Reference2 != oldJournalLine.Reference2)
            {
                eventNotes += string.Concat(TranslateTextsClass.Translate("Journal.CH.Reference2", entityPM.Tenant, showLocals), ": ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), oldJournalLine.Reference2, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), journalLinePM.Reference2, "\n");
            }

            if (journalLinePM.Reference3 != oldJournalLine.Reference3)
            {
                eventNotes += string.Concat(TranslateTextsClass.Translate("Journal.CH.Reference3", entityPM.Tenant, showLocals), ": ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), oldJournalLine.Reference3, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), journalLinePM.Reference3, "\n");
            }

            return eventNotes;
        }

        private void CreateCopyJournalEvent(JournalPM journal, Contact loggedContact)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = journal.Id,
                Tenant = journal.Tenant,
                UserId = loggedContact.Id,
                ObjectTableName = "Journal",
                IsAddedManually = false,
                EventTypeCode = "CPJL",
                Notes = "Copied from Journal Number: " + journal.CopiedFrom
            }); ;
        }

        private string TraceIt_JournalStatusName(string statusCode, int tenant)
        {

            JournalStatusTypeQueryService journalStatusTypeQueryService = new JournalStatusTypeQueryService(tenant);
            JournalStatusTypePM type = journalStatusTypeQueryService.GetSingle(statusCode, false, false);
            return type.EnglishName;

        }

       
        public string oldsJournalStatus()
        {



            if (EntityPOCO != null)
            {
                if (!string.IsNullOrWhiteSpace(EntityPOCO.StatusCode))
                {
                    return EntityPOCO.StatusCode;
                }
            }

            return "0";


        }

        
     
        

         


        protected override void AfterUpdating(JournalPM entityPM, EntityPM entityParentPM)
        {
            string journalOldStatusCode = oldsJournalStatus();

            try
            {


                if (entityPM.StatusCodeEnum == JournalStatusTypePM.StatusCodeEnum.Approved  //== "2") //Pending Approval  
                    && string.IsNullOrWhiteSpace(entityPM.QueueId))
                {

                    //                    if (LogitudeSettings.QueueServiceMode != "db")
                    //                    {
                    //                        throw new Exception(@"I talked with Ihab he said it's about time to change all environment to DB QUEUE mode 
                    //Especially in Accounting ,By This our transaction will be include Opening the QUEUE (in AZURE Mode its possible only with DTC Server  )
                    //");
                    //                    }

                    ReCheckFromDBThrowIfNotValid(entityPM);

                    CreateInterestTransactionTo_RegularJournal(entityPM);
                    JournalApproveService.EnqueueDB(entityPM);


                }
            }
            catch (Exception) // return to old values 
            {
                entityPM.StatusCode = journalOldStatusCode;
                throw;
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                JournalAdditionalDataUpdateService additionalDataUpdateService = new JournalAdditionalDataUpdateService((IAccountingContext)this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                JournalAdditionalDataPM journalAdditionalDataPM = new JournalAdditionalDataPM()
                {
                    JournalId = entityPM.Id,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    TaxReportId = null,
                    TaxReportTransmitStatusCode = null,
                    Tenant = entityPM.Tenant,
                };

                additionalDataUpdateService.Update(journalAdditionalDataPM, true);
            }


        }

        public virtual void CreateInterestTransactionTo_RegularJournal(JournalPM entityPM)
        {
            var myRegularJournalInterestTransactionService = new RegularJournalInterestTransactionMapping();
            myRegularJournalInterestTransactionService.CreatelInterestTransactions(entityPM);
        }

        protected void ReCheckFromDBThrowIfNotValid(JournalPM entityPM)
        {
            var qs= new JournalQueryService(entityPM.Tenant);
            var aftreUpdateGetFromDBPm = qs.GetSingle(entityPM.Id, true, false);
            Validate(aftreUpdateGetFromDBPm);
        }
        
        protected override void Validate(JournalPM entityPM)
        {

            bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
            ValidationContext validContext = AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(MainContext as IAccountingContext, entityPM, SuppressCheckGLAccountIsMultiCurrencyWI40640);
            
            
            
            ValidationResult result = JournalValidator.IsJournalValid(entityPM, validContext);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }




      
    }

    public partial class JournalVoidUpdateService : JournalUpdateService
    {
        private JournalPM _JornalPmSource;
        public JournalVoidUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {

        }
        public override JournalUpdateOnUpdating GetJournalOnUpdtatingObject()
        {
            var newAccountingContextDueCreatedJournal = AccountingContext.GetContext(_JornalPmSource.Tenant);
            var _journalUpdateService = new JournalUpdateService(newAccountingContextDueCreatedJournal, new Dictionary<string, IContext>(), _JornalPmSource.Tenant);
            var myJournalStornoService = new JournalStornoService();
            IJournalStornoPrepareJReconcileService journalStornoPrepareJReconcileService = new JournalStornoPrepareJReconcileService();
            journalStornoPrepareJReconcileService.MustInitialize(newAccountingContextDueCreatedJournal, _JornalPmSource);
            myJournalStornoService.Init(_JornalPmSource, _StornoOverrideM, _journalUpdateService, journalStornoPrepareJReconcileService);
            var journalUpdate = new JournalUpdateOnUpdating(this.MainContext as IAccountingContext, myJournalStornoService);
            //if (this.GetType().Name == "JournalVoidUpdateService")//
            return journalUpdate;
        }
        public JournalPM VoidJournal(string JournalId, int requestTenant,
           StornoOverrideM stornoOverrideM, DateTime? APPaymentCanceledDate=null
           ) //Call from JournalOpController
        {

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                _StornoOverrideM = stornoOverrideM;
                var qs = new JournalQueryService(this.MainContext as IAccountingContext);
                _JornalPmSource = qs.GetSingle(JournalId, true, false);
                _JornalPmSource.APPaymentCancelDate = APPaymentCanceledDate;
                if (_JornalPmSource == null)
                {
                    throw new Exception("Journal id couldn't find in db" + JournalId);
                }
                if (_JornalPmSource.Tenant != requestTenant)
                { 
                    throw new Exception("(Journal.Tenant!= requestTenant)");
                }
                if (String.IsNullOrWhiteSpace( _JornalPmSource.QueueId ))
                {
                    throw new Exception(
                        //"I must/Need??? Ledger to Reconcile - but journal did not Stream yet ..."
                        "רישום הקבלה בהנהלת החשבונות טרם הסתיים , אנא נסה בעוד מספר דקות עד שיושלם התהליך"

                        );
                }

                _JornalPmSource.ChangeSetOp = ChangeSetOperation.Update;
                _JornalPmSource.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Voided;
                this.Update(_JornalPmSource, true);
                scope.Complete();
                return _JornalPmSource;
            }
            
        }

        protected override void AfterUpdating(JournalPM entityPM, EntityPM entityParentPM)
        {
            if (CanIMatchVoidReconciliation())
            {
            }
            else
            {
                base.AfterUpdating(entityPM, entityParentPM);
            }
            
        }

        private bool CanIMatchVoidReconciliation()
        {

            bool meanWhileDoOnBatch = true;
            if (meanWhileDoOnBatch)
            {
                return false;
            }

            //var repo = new LedgerTransactionRepository(this.MainContext as IAccountingContext);
            //repo.GetByJournalId(_JornalPmSource.Id, _JornalPmSource.Tenant);
            return true;
        }
        
    }

    public interface IJournalUpdateService 
    {
        void Update(JournalPM entityPM, bool commit, TimeSpan? transactionTimeout = null);
    }
    
}


    
    

