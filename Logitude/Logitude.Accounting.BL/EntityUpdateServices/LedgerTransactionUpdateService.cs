using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Data.SqlClient;
using Simplog.Data.Helpers;
using System.Data;
using Simplog.Data.CommonDataModel;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class LedgerTransactionUpdateService : EntityUpdateService<LedgerTransaction, LedgerTransactionPM, EntityPM>
    {
        protected override void OnCreating(LedgerTransactionPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.IsReconciled == null)
            {
                entityPM.IsReconciled = false;
            }
            //this.UpdateBankAccount(entityPM);

        }

        protected override void OnUpdating(LedgerTransactionPM entityPM)
        {
            if (entityPM.IsReconciled == null)
            {
                entityPM.IsReconciled = false;
            }
            JournalQueryService journalQueryService = new JournalQueryService((MainContext as IAccountingContext));
            JournalPM journal = journalQueryService.GetSingle(entityPM.JournalId, false, false);
            if (journal != null)
            {
                if (journal.AccountingEntityCode == "12")
                {
                    entityPM.Reference2 = journal.JournalNumber;
                }
            }
            if (entityPM.Mark == true && (entityPM.IsReconciled || entityPM.InReconcileProgress))
            {
                entityPM.Mark = false;
            }

            if(entityPM != null && journal != null)
            {
                // check payment terms and add days to due date if needed.
                ProcessGLAccountPaymentTerms(entityPM, journal);
            }

        }
       

        public bool _CancelledAction;
        protected override void OnUpdating(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO)
        {

            switch (entityPM.ChangeSetOp)
            {
                case Simplog.Server.Infrastructure.ChangeSetOperation.None:
                    return;
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                    throw new ApplicationException("cannot insert LedgerTransactionPM move to Store Procedure");
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                    break;
                case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                    throw new ApplicationException("cannot Delete LedgerTransactionPM !!!!!");
                    break;
                default:
                    break;
            }

            if (!_CancelledAction)
            {
                if (entityPM.OpenAmount != 0)
                {
                    if (entityPOCO.OpenAmount < 0 && entityPM.OpenAmount > 0)
                    {
                        throw new ApplicationException("(entityPOCO.OpenAmount < 0 && entityPM.OpenAmount > 0)");
                    }

                    if (entityPOCO.OpenAmount > 0 && entityPM.OpenAmount < 0)
                    {
                        throw new ApplicationException("(entityPOCO.OpenAmount > 0 && entityPM.OpenAmount < 0)");
                    }
                }
            }

            if (entityPM.OpenAmount != 0 && entityPM.OpenAmount != entityPOCO.OpenAmount)
            {
                // checking a local-currency reconciliation
                decimal localTransAmount = entityPM.LocalAmountDebit - entityPM.LocalAmountCredit;
                if (entityPM.OpenAmountCurrencyId != entityPM.CurrencyId && Math.Abs(entityPM.OpenAmount) > Math.Abs(localTransAmount))
                {
                    throw new ApplicationException("Open amount (" + entityPM.OpenAmount + ") cannot be greather than transaction local amount (" + localTransAmount + ")");
                }

                // checking a foreign-currency reconciliation
                decimal foreignTransAmount = entityPM.ForeignAmountDebit - entityPM.ForeignAmountCredit;
                if (entityPM.OpenAmountCurrencyId == entityPM.CurrencyId && Math.Abs(entityPM.OpenAmount) > Math.Abs(foreignTransAmount))
                {
                    throw new ApplicationException("Open amount (" + entityPM.OpenAmount + ") cannot be greather than transaction amount (" + foreignTransAmount + ")");
                }
            }




            if (entityPM.IsExternalReconcile != entityPOCO.IsExternalReconcile)
            {
                //this.UpdateBankAccount(entityPM);
            }
 
            base.OnUpdating(entityPM, entityPOCO);
        }

        public void DelSertOpenRecilationDrafts(List<LedgerTransactionPM> OpenRecilationDrafts)
        {

            string gLAccountId; int tenant ;
            var pairs=OpenRecilationDrafts.GroupBy(r => new { r.AccountId, r.Tenant });
                var pairsCount = pairs.Count();
                if (pairsCount > 1)
                {
                    throw new ApplicationException("only one combination allowed Of {AccountId +Tenant }");
                }
                if (pairsCount == 0)
                {
                    //to delete 
                    throw new ApplicationException("only one combination allowed Of {AccountId +Tenant } (pairsCount == 0) ==>No Items On Match List,To Delete ? ");
                }
            
            var repeateTrans= OpenRecilationDrafts.GroupBy(r => r.Id).Where( g=> g.Count()>1).Select( g=>g.Key).ToList();
            if (repeateTrans.Count>0)
            {
                throw new ApplicationException("Client Side should send send Unique Id List :" + string.Join(",",repeateTrans.ToArray()));
            }
  
            gLAccountId =pairs.First().Key.AccountId;
            tenant=pairs.First().Key.Tenant;

            using (var scope = TransactionFactory.GetTransaction())
            {
                (this.Repository as LedgerTransactionRepository).ResetDraftOpenReconciliation(gLAccountId, tenant);

                var transIdList=OpenRecilationDrafts.Select(r => r.Id).ToList();
                LedgerTransactionQueryService qs = new LedgerTransactionQueryService((MainContext as IAccountingContext));
                var listPM=qs.GetLedgerTransactionPMsByIdList(transIdList, tenant);
                foreach (var pm in listPM)
                {
                    if (pm.IsReconciled)
                    {
                        throw new ApplicationException("DelSertOpenRecilationDrafts but pm.IsReconciled " + pm.Id);
                    }
                    pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    pm.AmountToReconcile = OpenRecilationDrafts.First(r => r.Id == pm.Id).AmountToReconcile;
                    pm.Mark = true;
                }
                this.UpdateMulti(listPM, new List<LedgerTransactionPM>(), new EntityPM(), true);
                scope.Complete();
            }
        }
        internal void UpdateBankAccount(LedgerTransactionPM entityPM)
        {
            BankAccountQueryService qs = new BankAccountQueryService((MainContext as IAccountingContext));
            BankAccountUpdateService service = new BankAccountUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            var bankAccount = qs.GetByGLAccountId(entityPM.AccountId, entityPM.Tenant);
            if (bankAccount != null)
            {
                var totalOpenExternalTransactions = 0;
                if (entityPM.IsExternalReconcile)
                {
                    Int32.TryParse(bankAccount.TotalOpenExternalTransactions, out totalOpenExternalTransactions);
                    if(totalOpenExternalTransactions > 0)
                    {
                        totalOpenExternalTransactions--;
                    }
                    bankAccount.TotalOpenExternalTransactions = totalOpenExternalTransactions.ToString();
                }
                else
                {
                    Int32.TryParse(bankAccount.TotalOpenExternalTransactions, out totalOpenExternalTransactions);
                    if (totalOpenExternalTransactions > 0)
                    {
                        totalOpenExternalTransactions++;
                    }
                    bankAccount.TotalOpenExternalTransactions = totalOpenExternalTransactions.ToString();
                }
                bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(bankAccount, true);
            }
        }
        public static int UpdateInReconcileProgress(string journalId, int tenant, bool Value_inReconcileProgress)
        {

            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = 
                        "UPDATE LedgerTransactions SET InReconcileProgress= @Value_inReconcileProgress " +
                        "WHERE ID IN (" +
                        "    SELECT  LedgerTransactionId  from JournalReconciles " +
                        "     WHERE  JournalId=@journalId and tenant= @tenant " +
                        "            and LedgerTransactionId is not null" +
                        ")";
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@Value_inReconcileProgress", SqlDbType.Int);
                    command.Parameters["@Value_inReconcileProgress"].Value= Value_inReconcileProgress;

                    command.Parameters.Add("@tenant", SqlDbType.Int);
                    command.Parameters["@tenant"].Value = tenant;

                    command.Parameters.Add("@journalId", SqlDbType.VarChar);
                    command.Parameters["@journalId"].Value = journalId;


                    int rows = command.ExecuteNonQuery();
                    return rows ;
                }
            }
        }
        public static int Update_InProgressExternalReconcile(string journalId, int tenant, bool Value_ExternalReconcileInProgress)
        {

            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection connection = new SqlConnection(strConnString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE LedgerTransactions SET InProgressExternalReconcile= @Value_ExternalReconcileInProgress " +
                        "WHERE ID IN (" +
                        "    SELECT  LedgerTransactionId  from JournalReconciles " +
                        "     WHERE  JournalId=@journalId and tenant= @tenant " +
                        "            and LedgerTransactionId is not null" +
                        ")";
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add("@Value_ExternalReconcileInProgress", SqlDbType.Int);
                    command.Parameters["@Value_ExternalReconcileInProgress"].Value = Value_ExternalReconcileInProgress;

                    command.Parameters.Add("@tenant", SqlDbType.Int);
                    command.Parameters["@tenant"].Value = tenant;

                    command.Parameters.Add("@journalId", SqlDbType.VarChar);
                    command.Parameters["@journalId"].Value = journalId;


                    int rows = command.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        public void UpdateInReconcileProgress(List<String> listTransactionId,int tenant,bool Value_inReconcileProgress)
        {

            //using (var scope = TransactionFactory.GetTransaction())//we alreary in a scope !!-but while straming we r in Serlazed TRans
            {
                var ledgerTransactionQueryService = new LedgerTransactionQueryService(MainContext as IAccountingContext);
                var pmList = ledgerTransactionQueryService.GetLedgerTransactionPMsByIdList(listTransactionId, tenant);

                foreach (var item in pmList)
                {
                    if (Value_inReconcileProgress == true)//while prepare check while streaming do not check !!
                    {
                        if (item.InReconcileProgress)
                        {
                            throw new ApplicationException("יש התאמות בתהליך");
                        }
                    }
                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    item.InReconcileProgress = /*true*/ Value_inReconcileProgress;
                }
                bool supperssSaveOnUpdateMultiDueIsFaster = true;
                this.UpdateMulti(pmList, new List<LedgerTransactionPM>(), new EntityPM(), !supperssSaveOnUpdateMultiDueIsFaster);
                if (supperssSaveOnUpdateMultiDueIsFaster)
                {
                    this.SubmitChanges();
                }
                //scope.Complete();
            }
        }

        internal void Update_InProgressExternalReconcile(List<string> listTransactionId, int tenant, bool Value_ExternalReconcileInProgress)
        {
            var ledgerTransactionQueryService = new LedgerTransactionQueryService(MainContext as IAccountingContext);
            var pmList = ledgerTransactionQueryService.GetLedgerTransactionPMsByIdList(listTransactionId, tenant);

            foreach (var item in pmList)
            {
                if (Value_ExternalReconcileInProgress == true)//while prepare check while streaming do not check !!
                {
                    if (item.InProgressExternalReconcile)
                    {
                        throw new ApplicationException("יש התאמות בתהליך");
                    }
                }
                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                item.InProgressExternalReconcile = /*true*/ Value_ExternalReconcileInProgress;
            }
            bool supperssSaveOnUpdateMultiDueIsFaster = true;
            this.UpdateMulti(pmList, new List<LedgerTransactionPM>(), new EntityPM(), !supperssSaveOnUpdateMultiDueIsFaster);
            if (supperssSaveOnUpdateMultiDueIsFaster)
            {
                this.SubmitChanges();
            }
        }

        protected override void Trace(LedgerTransactionPM entityPM, LedgerTransaction entityPOCO, string changesXml)
        {
            if(entityPM.IsExternalReconcile != entityPOCO.IsExternalReconcile)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                bool showLocals = !contact.DontShowLocalLabels;
                var eventNotes = string.Concat("Line No:  "+ entityPM.JournalLineNumber + ", Is Externally Reconciled changed: ", TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals), entityPOCO.IsExternalReconcile, "\t", TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals), entityPM.IsExternalReconcile, "\n");

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.JournalId,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "Journal",
                    IsAddedManually = false,
                    EventTypeCode = "JUP",
                    Notes = eventNotes,

                });
            }
            
        }

        private void ProcessGLAccountPaymentTerms(LedgerTransactionPM ledgerTransaction, JournalPM journal)
        {
            try
            {
                GLAccountQueryService glAccountQueryService = new GLAccountQueryService((MainContext as IAccountingContext));
                GLAccountPM glAccount = glAccountQueryService.GetSingle(ledgerTransaction.AccountId, false, false);

                foreach (JournalLinePM journalLine in journal.JournalLines)
                {
                    // 3. כאשר נוצרת תנועה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
                    // (שהמקור  שלה הוא פקודת יומן חיצונית(ממערכת יוניפרייט
                    // Journals.ExternalSystem=UNIFREIGHT   >  מקור שלה ביוניפרייט
                    // Journals.AccountingEntityCode = 1 > מסוג פקודת יומן
                    // Journallines.actioncode=1  והתנועה היא מתוך שורת פקודת יומן בזכות
                    if (journal?.ExternalSystem == "UNIFREIGHT" && journal?.AccountingEntityCode == "1" && journalLine?.ActionCode == "1")
                    {
                        UpdateDueDate(ledgerTransaction, glAccount, journalLine);
                    }

                    //4. כאשר נוצרת שורה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
                    // glaccounts.ChartOfAccountsTypeCode = 4
                    // שהמקור שלה הוא חשבונית ספק
                    // Journals.AccountingEntityCode = 4 > מסוג חשבונית ספק
                    // Journallines.actioncode = 1 > והתנועה היא מתוך שורת פקודת יומן בזכות

                    if (glAccount?.ChartOfAccountsTypeCode == "4" && journal?.AccountingEntityCode == "4" && journalLine?.ActionCode == "1")
                    {
                        UpdateDueDate(ledgerTransaction, glAccount, journalLine);
                    }
                }
            }
            catch
            {
                throw new Exception();
            }

        }

        private void UpdateDueDate(LedgerTransactionPM ledgerTransaction, GLAccountPM glAccount, JournalLinePM journalLine)
        {
            if (glAccount?.PaymentTerms != null)
            {
                PaymentTermRepository paymentTermQueryService = new PaymentTermRepository((MainContext as ICommonDataContext));
                int paymentTermDays = paymentTermQueryService.GetSinglePaymentTerm(glAccount.PaymentTerms).Days;
                if (paymentTermDays != 0)
                {
                    journalLine.DueDate = ledgerTransaction.DocumentDate.AddDays(paymentTermDays);
                    // update journal line in db:
                    JournalLineUpdateService journalLineUpdateService = new JournalLineUpdateService(ledgerTransaction.Tenant);
                    journalLine.ChangeSetOp = ChangeSetOperation.Update;
                    journalLineUpdateService.Update(journalLine, true);

                }
            }
        }
    }
}
