
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class ExternalReconciliationUpdateService
   {
        protected override void OnCreating(ExternalReconciliationPM entityPM, EntityPM entityParentPM)
        {
            //Create ID
            if (entityPM.Id == null || entityPM.Id == "" || entityPM.Id == "new")
                entityPM.Id = IdCounter.GetNumber("ExternalReconciliation", entityPM.Tenant);

            //Create Code Number
            entityPM.ReconciliationNumber = CodeCounter.GetNumber("ExternalReconciliation", entityPM.Tenant);

            //Fill created by fields
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = loggedContact.Id;

            //Update line parent id
            List<string> ledgerIds = new List<string>();
            foreach (var line in entityPM.ExternalReconciliationLines)
            {
                line.ReconciliationId = entityPM.Id;
                ledgerIds.Add(line.LedgerTransactionId);
            }
         


            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(ExternalReconciliationPM entityPM)
        {

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert) 
            {
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
                LedgerTransactionUpdateService transactionService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                ReconcileExternalPageLineUpdateService pageLineService = new ReconcileExternalPageLineUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);

                List<string> LedgerTransactionIds = EntityPM.ExternalReconciliationLines.Where(d => d.LedgerTransactionId != null).Select(d=>d.LedgerTransactionId).ToList();
                List<string> PageLineIds = EntityPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();

                List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, entityPM.Tenant);
                List<ReconcileExternalPageLinePM> PageLines = pageLineQuery.GetPageLinesPMsByIdList(PageLineIds, entityPM.Tenant);

                BankDepositQueryService bankDepositQueryService = new BankDepositQueryService(entityPM.Tenant);
                foreach (var transactionPM in LedgerTransactions)
                {
                    transactionPM.ChangeSetOp = ChangeSetOperation.Update;
                    transactionPM.IsExternalReconcile = true;
                    if (transactionPM.SourceTypeCode == "6")
                    {
                        List<ARPaymentChequePM> aRPaymentChequePMs = bankDepositQueryService.GetListByPaymentId(transactionPM.SourceId, entityPM.Tenant);

                        foreach (ARPaymentChequePM item in aRPaymentChequePMs)
                        {
                            item.StatusCode = "6";
                            item.ChangeSetOp = ChangeSetOperation.Update;
                            ARPaymentChequeUpdateService aRPaymentChequeUpdateService = new ARPaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                            aRPaymentChequeUpdateService.Update(item, true);
                        }
                    }
                    transactionService.Update(transactionPM, false);
                }

                foreach (var pageLinePM in PageLines)
                {
                    pageLinePM.ChangeSetOp = ChangeSetOperation.Update;
                    pageLinePM.IsReconciled = true;
                    pageLineService.Update(pageLinePM, false);
                }


            }

            base.OnUpdating(entityPM);
        }
        protected override void OnUpdating(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO)
        {

            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            if (entityPOCO.IsCancelled == false && entityPM.IsCancelled == true)
            {
                // canceled!!
                // Update for each transaction(ReconciliationLines.TransactionId): IsExternalReconcile.LegderTransactions = False
                UpdateLedgerTransactions(entityPM);

                // Update for each bank transaction(In table ReconcileExternalPageLines): IsReconciled=False
                UpdateBankPages(entityPM);


            }


            base.OnUpdating(entityPM, entityPOCO);
        }
        protected override void UpdateComposition(ExternalReconciliationPM entityPM)
        {
            ExternalReconciliationLineUpdateService lineUpdateService = new ExternalReconciliationLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            lineUpdateService.UpdateMulti(entityPM.ExternalReconciliationLines, entityPM.DeletedExternalReconciliationLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }
        protected override void Trace(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO, string changesXml)
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
                    ObjectTableName = "ExternalReconciliation",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else
            {
                if(entityPM.IsCancelled != entityPOCO.IsCancelled)
                {
                    //create trace event with created type.
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = loggedContact.Id,
                        ObjectTableName = "ExternalReconciliation",
                        IsAddedManually = false,
                        EventTypeCode = "ERCN",
                        Notes = "",
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }

            base.Trace(entityPM, entityPOCO, changesXml);
        }
        protected override void Validate(ExternalReconciliationPM entityPM)
        {

            CheckDifferenc(entityPM);

            base.Validate(entityPM);
        }

        void CheckDifferenc(ExternalReconciliationPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            //Validate lines total
            decimal LedgerLinesSum = 0;
            decimal BankLinesSum = 0;

            ExternalReconciliationQueryService query = new ExternalReconciliationQueryService(context);

            List<string> listOfLedgerLinesIds = entityPM.ExternalReconciliationLines.FindAll(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
            List<LedgerTransaction> ledgerLines = query.GetLedgerTransactionByIds(listOfLedgerLinesIds, entityPM.Tenant);
            //LedgerLinesSum = ledgerLines.Sum(a => a.OpenAmount);
            //LedgerLinesSum = ledgerLines.Sum(a => (a.ForeignAmountDebit == 0 ? a.ForeignAmountDebit : a.ForeignAmountCredit));
            LedgerLinesSum = ledgerLines.Sum(a => a.ForeignAmountDebit);
            LedgerLinesSum += ledgerLines.Sum(a => a.ForeignAmountCredit);

            List<string> listOfBankLinesIds = entityPM.ExternalReconciliationLines.FindAll(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();
            List<ReconcileExternalPageLine> pageLines = query.GetBankPagesByIds(listOfBankLinesIds, entityPM.Tenant);
            BankLinesSum = pageLines.Sum(a => a.Amount);


            var def = (BankLinesSum - LedgerLinesSum);
            var totalDifference = def < 0 ? def * -1 : def;

            if (totalDifference != 0)
            {
                ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
                bool showLocal = !loggedContact.DontShowLocal;

                var msg = TranslateTextsClass.Translate("Accounting.General.O.DifferenceMustEqual0",0, showLocal);
                throw new ApplicationException(msg);
            }

        }

        private void UpdateLedgerTransactions(ExternalReconciliationPM entityPM)
        {
            var transactionIdList = entityPM.ExternalReconciliationLines.Select(rec => rec.LedgerTransactionId).ToList();
            var qs = new LedgerTransactionQueryService(entityPM.Tenant);
            var LedgerTransactionPMsUpdated = qs.GetLedgerTransactionPMsByIdList(transactionIdList, entityPM.Tenant);

            foreach (var line in LedgerTransactionPMsUpdated)
            {
                line.ChangeSetOp = ChangeSetOperation.Update;
                line.IsExternalReconcile = false;
            }
            var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            ledgerTransactionUpdateService.UpdateMulti(LedgerTransactionPMsUpdated, new List<LedgerTransactionPM>(), entityPM, false);
        }
        private void UpdateBankPages(ExternalReconciliationPM entityPM)
        {
            var pageLinesIdList = entityPM.ExternalReconciliationLines.Select(rec => rec.ExternalPageLineId).ToList();
            var qs = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
            var pageLinesPMsUpdated = qs.GetPageLinesPMsByIdList(pageLinesIdList, entityPM.Tenant);

            foreach (var line in pageLinesPMsUpdated)
            {
                line.ChangeSetOp = ChangeSetOperation.Update;
                line.IsReconciled = false;
            }
            var reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            reconcileExternalPageLineUpdateService.UpdateMulti(pageLinesPMsUpdated, new List<ReconcileExternalPageLinePM>(), new ReconcileExternalPagePM(), false);
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {

            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(
                //SecurityUtility.GetAuthenticatedUser()
                AuthenticationUtil.ResolveUserIdentityName(tenant)
                , tenant);
            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(tenant).GetContactByEmailOnly("system@tenant" + tenant + ".com", tenant);
            }
            loggedContact = loggedContact ?? new Logitude.BL.CommonDataModel.EntityPMs.ContactPM() { DontShowLocal = true };
            return loggedContact;
        }

    }
   
}
	 