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
                entityPM.Number;
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

                var propChanged = ChangeTrackingEntityPM.ChangedProperties.Where(f =>
                    !cancelledProp.Contains(f.PropertyName)).ToList();
                //update 
                if (propChanged.Any())
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


                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
                    LedgerTransactionUpdateService transactionService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                    ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);

                    //List<string> LedgerTransactionIds = EntityPM.ReconciliationLines.Where(d => d.TransactionId  != null).Select(d => d.TransactionId).ToList();

                //List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, entityPM.Tenant);
              
             

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

                 


                
                var validContext = AccountingValidationContextServiceProvider.NewReconciliationValidatorContext((MainContext as IAccountingContext), entityPM);
                var validationResult = ReconciliationValidator.IsReconciliationValid(entityPM, validContext);
                if (validationResult != null)
                {
                    string errorText = validationResult.ErrorMessage;//+ ", Number=" + _JournalPM.ExternalNo + @"/" + _JournalPM.Id;
                                                                     //ThrowException(errorText);
                    throw new Exception(errorText);
                }



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
            if(_CancelledAction == true)
            {
                //get journal of reconciliation - WI39779
                JournalQueryService journalQuery = new JournalQueryService(entityPM.Tenant);
                JournalPM journal = journalQuery.GetByAccountingEntityId(entityPM.Id, entityPM.Tenant);
                if(journal != null)
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

            base.OnUpdating(entityPM, entityPOCO);
        }

        protected override void UpdateComposition(ReconciliationPM entityPM)
        {
            try
            {

                if (this._CancelledAction)
                {
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
                    ledgerTransactionPM.OpenAmount = ledgerTransactionPM.OpenAmount - reconciliationLine.ReconciliationAmount;
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
        }

        public bool SuppressResetDraftOpenReconciliation { get; set; }
        protected override void AfterUpdating(ReconciliationPM entityPM, EntityPM entityParentPM)
        {
            if (SuppressResetDraftOpenReconciliation)
            {
                return;
            }
            var repoLedger = new LedgerTransactionRepository(MainContext as IAccountingContext);
            repoLedger.ResetDraftOpenReconciliation(entityPM.AccountId, entityPM.Tenant);


            // set searchfields
            foreach (ReconciliationLinePM recoLine in entityPM.ReconciliationLines)
            {
                //get transaction
                LedgerTransaction transaction = repoLedger.GetSingle(recoLine.TransactionId, recoLine.Tenant);
                if(transaction != null)
                    PushSearchFieldText(entityPM, transaction.SearchFields);

            }
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            ReconciliationUpdateService recoUpdateService = new ReconciliationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = ChangeSetOperation.Update;
            recoUpdateService.Update(entityPM, false);


        }

        //protected override void Trace(ReconciliationPM entityPM, Reconciliation entityPOCO, string changesXml)
        //{
        //    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
        //    {
        //        //create trace event with created type.
        //    }
        //    else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
        //    {


        //    }
        //    base.Trace(entityPM, entityPOCO, changesXml);
        //}



        //private ContactPM LoggedContact(int tenant)
        //{
        //    ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
        //    if (loggedContact == null)
        //    {
        //        loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
        //    }
        //    return loggedContact;
        //}

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
