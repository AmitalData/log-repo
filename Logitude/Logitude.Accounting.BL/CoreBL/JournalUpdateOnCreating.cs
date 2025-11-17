using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalUpdateOnCreating : IJournalUpdateInsert
    {
        private IAccountingContext _MainContext;
        public JournalUpdateOnCreating(IAccountingContext mainContext)
        {
            this._MainContext = mainContext;
        }
        
        public void OnCreating(JournalPM entityPM, EntityPM entityParentPM)
        {

            if (!String.IsNullOrWhiteSpace(entityPM.ExternalNo) && !String.IsNullOrWhiteSpace(entityPM.ExternalSystem))
            {
                JournalQueryService journalQuery = new JournalQueryService(entityPM.Tenant);
                String oldjournalNumber = "";
                if (journalQuery.CheckIfExternalNoAndSystemExist(entityPM.ExternalNo, entityPM.ExternalSystem, out oldjournalNumber, entityPM.Tenant))
                {
                    string basic_text_ExternalExist =
                        //"There is a Journal ("+ journalNumber + ") with the same ExternalNo And ExternalSystem";
                        JournalValidator.M_ExternalNoAlreadyExists_1 + oldjournalNumber + JournalValidator.M_ExternalNoAlreadyExists_2;
                    throw new Exception(basic_text_ExternalExist);
                }
            }
    
            entityPM.Id = //IdCounter.GetNumber(

                // new IdCounterWrapper().GetNumber(
                //GetNumberJournal(), 
                IdCounterWrapperGetNumber(entityPM.Tenant);
            MatchPaymentCommandTransactions(entityPM, entityPM.Tenant);

            //entityPM.JournalNumber =
            //    //(new CodeCounterWrapper()).GetNumber(GetCodeNumberJournal(), 
            //    CodeCounterWrapperGetNumber(
            //    entityPM.Tenant).ToString();


            string loggedContactId = GetLogContactId(entityPM);
            if (string.IsNullOrWhiteSpace(loggedContactId))
            {
                throw new ApplicationException("Logged Contact Id is required ");
            }
            string ObjectTableId = GetObjectTableId(entityPM);

            AddAcitivityLog(entityPM, loggedContactId, ObjectTableId);


            var myAccountingEntityDetails = new AccountingEntityDetails();
            var myAccEntityReconciliation10 = myAccountingEntityDetails
                .GetAll()
                .FirstOrDefault(r => r.EnglishName =="Adjustment");


            //if (entityPM.TypeCode == "0" && entityPM.AccountingEntityReference == null) // Manual
            //{
            //    if (myAccEntityReconciliation10.Code == entityPM.AccountingEntityCode)
            //    {
            //        //entityPM.AccountingEntityReference = will be enter WhileStreaming ;
 
            //    }
            //    else
            //    {
            //        entityPM.AccountingEntityReference = entityPM.JournalNumber;
            //    }
            //}
            var DateTimeNow = GetDateTimeNow();

            if (entityPM.CreateDate == null)
                entityPM.CreateDate = DateTimeNow;

            if (entityPM.UpdateDate == null)
                entityPM.UpdateDate = DateTimeNow;

            ClearDMYByUserId(entityPM, loggedContactId);
            if (entityPM.UpdatedByUserId == null)
                entityPM.UpdatedByUserId = loggedContactId;

            if (entityPM.CreatedByUserId == null)
                entityPM.CreatedByUserId = loggedContactId;

            entityPM.IsVoided = entityPM.IsVoided ?? false;
            if (String.IsNullOrWhiteSpace(entityPM.AccountingEntityId))
            {
                if (myAccEntityReconciliation10.Code == entityPM.AccountingEntityCode)
                {
                    //do not set  entityPM.AccountingEntityId!!! will be enter WhileStreaming 
 
                }
                else
                {
                    entityPM.AccountingEntityId = entityPM.Id;
                }
                
            }
            if (String.IsNullOrWhiteSpace(entityPM.TypeCode)) entityPM.TypeCode = "0"; //Manual
            if (String.IsNullOrWhiteSpace(entityPM.AccountingEntityCode)) entityPM.AccountingEntityCode = "1"; //Journal
            if (String.IsNullOrWhiteSpace(entityPM.CreatedByUserId)) entityPM.CreatedByUserId = AuthenticationUtil.GetAuthenticatedUser();// "1-14733"; //Alex //COMPILE//
            if (entityPM.StatusCode == "6" && String.IsNullOrWhiteSpace(entityPM.ApprovedByUserId)) entityPM.ApprovedByUserId = entityPM.CreatedByUserId;
            if (entityPM.JournalLines != null)
            {
                //if (entityPM.CreateDate == DateTime.MinValue) entityPM.CreateDate = DateTime.Now;
                entityPM.CreateDate = DateTime.Now; //eyal 

                var originalLines = entityPM.JournalLines.ToList();
                for (int i = 0; i < originalLines.Count; i++)
                {
                    JournalLinePM item = originalLines[i];

                    JournalUpdateService journalUpdateService = new JournalUpdateService(this._MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    var newJournalLinePM = journalUpdateService.CheckJournalActionCodeAndSplitedIt(item, entityPM.JournalLines);
                    OnCreateLine(entityPM, item);

                    if (newJournalLinePM != null)
                    {
                        entityPM.JournalLines.Add(newJournalLinePM);
                        OnCreateLine(entityPM, newJournalLinePM);
                    }
                }

                foreach (var item in entityPM.JournalReconciles)
                {

                    item.JournalId = entityPM.Id;

                }
                foreach (var item in entityPM.JournalExternalReconciles)
                {

                    item.JournalId = entityPM.Id;

                }
                var renumber = true;
                if (renumber)
                {
                    int i = 1;
                    foreach (JournalLinePM item in entityPM.JournalLines)
                    {
                        item.Line = i++;

                    }
                }

            }



            //if (entityPM.StatusCode == "2")
            //{
            //    Case_2(entityPM);
            //}

        }
        private static void MatchPaymentCommandTransactions(JournalPM journalPM, int tenant)
        {
            if (journalPM == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError("journalPM is null , tenant=" + tenant);
                return;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(journalPM.InvoicesXml))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"No InvoicesXml found for Journal {journalPM.Id}, tenant={tenant}");
                    return;
                }
                var accountingContext = AccountingContext.GetContext(tenant);
                JournalQueryService journalQueryService = new JournalQueryService(accountingContext);
                JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                 LedgerTransactionQueryService ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
                APInvoiceQuery aPInvoiceQuery = new APInvoiceQuery(tenant);
                Tenant loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
              
                    List<APIDataContract.ApiV1.Invoice> invoices = null;

                    try
                    {
                    invoices = JsonSerializer.Deserialize<List<APIDataContract.ApiV1.Invoice>>(journalPM.InvoicesXml);

                    }
                    catch (Exception jex)
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to deserialize InvoicesXml for Journal {journalPM.Id}. Content={journalPM.InvoicesXml}, Error={jex.Message}");
                        return;
                    }
                    if (invoices == null || !invoices.Any())
                    {
                        NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"No invoices parsed for Journal {journalPM.Id}");
                        return;
                    }
                    foreach (APIDataContract.ApiV1.Invoice invoice in invoices)
                    {
                        if (string.IsNullOrWhiteSpace(invoice?.Key))
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"Invoice with empty key skipped in Journal {journalPM.Id}");
                            continue;
                        }
                        APInvoicePM aPInvoicePM = aPInvoiceQuery.GetSingleInvoiceByExternlaEntityId(invoice.Key, tenant);

                        if (aPInvoicePM == null)
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"No aPInvoicePM found for Invoice {invoice.Key}, Journal {journalPM.Id}");
                            continue;
                        }
                        List<LedgerTransactionPM> ledgerTransactions = ledgerTransactionQuery
                            .GetTransactionBySourceEntity(aPInvoicePM.Id, AccountingEntityValues.APInvoice, tenant)?
                            .Where(a =>
                                (a.LocalAmountCredit != 0m || a.ForeignAmountCredit != 0m) &&
                                a.OpenAmount != 0m &&
                                !a.InReconcileProgress &&
                                !a.IsReconciled
                            ).ToList();
                        if (ledgerTransactions == null || !ledgerTransactions.Any())
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"No ledger transactions found for Invoice {invoice.Key}, Journal {journalPM.Id}");
                            continue;
                        }

                        foreach (var transaction in ledgerTransactions)
                        {
                            JournalPM journal = journalQueryService.GetSingle(transaction.JournalId, true, false);
                            var line = journal.JournalLines.Where(a => a.Line == transaction.JournalLineNumber).FirstOrDefault();
                            if (line == null)
                            {
                                NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"No journal line found for Transaction {transaction.Id}, Journal {journalPM.Id}");
                                continue;
                            }
                            var amount = (journalPM.CurrencyId == loggedTenant?.CurrencyId ? invoice.LocalAmount : invoice.ForeignAmount);
                            AddJournalReconciles(transaction, line, journal, amount, journalPM);
                        }
                    }

                


            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError(ex.Message + " ," + journalPM.Id);
            }
        }

        private static void AddJournalReconciles(LedgerTransactionPM transaction, JournalLinePM journalLinePM, JournalPM journalPM, decimal reconciliationAmount, JournalPM baseJournal)
        {
            if (transaction == null || journalLinePM == null || journalPM == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning(
                           "AddJournalReconciles: One or more required parameters are null. " +
                           $"transaction={(transaction == null ? "null" : "ok")}, " +
                           $"journalLinePM={(journalLinePM == null ? "null" : "ok")}, " +
                           $"journalPM={(journalPM == null ? "null" : "ok")}");

                return;

            }

            if (Math.Abs(reconciliationAmount) > Math.Abs(transaction.OpenAmount))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteWarning($"Reconciliation skipped: transaction {transaction.Id} openAmount={transaction.OpenAmount} < reconciliationAmount={reconciliationAmount}");
                return;
            }

            if (baseJournal.JournalReconciles == null)
                journalPM.JournalReconciles = new List<JournalReconcilePM>();

            baseJournal.JournalReconciles.Add(new JournalReconcilePM()
            {
                Tenant = journalPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                JournalId = journalPM.Id,
                Line = journalLinePM.Line,
                LedgerTransactionId = transaction.Id,
                CurrencyId = transaction.OpenAmountCurrencyId,
                ReconciliationAmount = transaction.OpenAmount,
                IsPartial =  false
            });
        }


        public virtual void ClearDMYByUserId(JournalPM entityPM, string loggedContactId)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);


            var contact = contactRep.GetSingleContact(entityPM.UpdatedByUserId, entityPM.Tenant);
            if (contact == null)
            {
                entityPM.UpdatedByUserId = null;
            }

            contact = contactRep.GetSingleContact(entityPM.CreatedByUserId, entityPM.Tenant);
            if (contact == null)
            {
                entityPM.CreatedByUserId = null;
            }


            
        }

        public virtual void AddAcitivityLog(JournalPM entityPM, string loggedContactId, string ObjectTableId)
        {
            var myActivityLogger = ContainerAccessor.Container.ResolveSafe<IActivityLogger>() ?? 
                new ActivityLoggerWrapper();
            myActivityLogger.AddAcitivityLog(entityPM.Id, ObjectTableId, entityPM.Tenant, "N", loggedContactId);
        }

        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    GetNumberJournal(), Tenant);
        }
        //public virtual int CodeCounterWrapperGetNumber(int Tenant)
        //{
        //    return CodeCounter.GetNumber(GetCodeNumberJournal(), Tenant, false);
        //}
        public virtual void OnCreateLine(JournalPM entityPM, JournalLinePM journalLinePM)
        {
            var journalLineUpdateInsert = new JournalLineOnUpdate(this._MainContext);
            journalLineUpdateInsert.OnUpdate(journalLinePM, entityPM);
            journalLinePM.ChangeSetOp = ChangeSetOperation.Insert;
        }

        public virtual DateTime GetDateTimeNow()
        {
            return DateTime.Now;
        }
        public static string GetCodeNumberJournal()
        {
            return "Journal.JournalNumber";
        }

        public static string GetNumberJournal()
        {
            return "Journal";
        }

        public virtual string GetLogContactId(JournalPM entityPM)
        {

            return AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            //string email = HttpContext.Current.User.Identity.Name;


            //var contactRepository = ContainerAccessor.Container.ResolveSafe<IContactRepository>() ?? new ContactRepository(entityPM.Tenant);
            //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            //if (loggedContact != null)
            //{
            //    return loggedContact.Id;
            //}
            //return null;


        }
        public virtual string GetObjectTableId(JournalPM entityPM)
        {

            var objectTableRepository = ContainerAccessor.Container.ResolveSafe<IObjectTableRepository>() ?? new ObjectTableRepository(entityPM.Tenant);
            var pocoObjectTable = //this.UpdateServiceProvider.GetObjectTableId(entityPM.Tenant);
                objectTableRepository.GetObjectTableByName(
                GetNumberJournal()///=="Journal"
            , 0, true);
            return pocoObjectTable.Id;
        }

        public class Factory
        {
            private IJournalUpdateInsert customManager = null;
            public IJournalUpdateInsert Create(IAccountingContext mainContext)
            {
                if (customManager != null) return customManager;

                return new JournalUpdateOnCreating(mainContext) as IJournalUpdateInsert;
            }
            public void SetManager(IJournalUpdateInsert mgr)
            {
                customManager = mgr;
            }
        }

    }
    public interface IJournalUpdateInsert
    {
        void OnCreating(JournalPM entityPM, EntityPM entityParentPM);
        string GetObjectTableId(JournalPM entityPM);
        string GetLogContactId(JournalPM entityPM);
    }
}
