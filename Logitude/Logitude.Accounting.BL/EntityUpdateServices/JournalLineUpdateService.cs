using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalLineUpdateService : EntityUpdateService<JournalLine, JournalLinePM, JournalPM>
    {

        private class JournalLineRepositoryPriv : JournalLineRepository
        {

            public JournalLineRepositoryPriv(IAccountingContext mainContext)
                : base(mainContext)
            { }
        }
        protected override void AddContext(JournalLinePM myTEntityPM)
        {
            base.AddContext(myTEntityPM);
            this.Repository = new JournalLineRepositoryPriv((IAccountingContext)this.MainContext);

        }

        protected override void OnCreating(JournalLinePM entityPM, JournalPM entityParentPM)
        {
            var myName = this.NameOf();
            if (myName!="JournalLineUpdateServicePriv")
            {
                throw new ApplicationException("JournalLineUpdateService must be used only from JournalUpdateService(force check Approved Journal Can Only Change To Voided)");
            }

            // check payment terms and add days to due date if needed.
            ProcessGLAccountPaymentTerms(entityPM, entityParentPM);

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(JournalLinePM entityPM)
        {
            var myName = this.NameOf();
            //object entityAncestorPOCO; object entityAncestorPM; object entityParentPM;
            //GetAncestor(out entityAncestorPOCO, out entityAncestorPM, out entityParentPM);
            //var journalPM = entityAncestorPM as JournalPM;
            //if (journalPM == null)
            if (myName != "JournalLineUpdateServicePriv")
            {
                throw new ApplicationException("JournalLineUpdateService must be used only from JournalUpdateService(force check Approved Journal Can Only Change To Voided)");
                //throw new ApplicationException("BLException :Approved Journal Can Only Change To Voided");
            }

            base.OnUpdating(entityPM);
        }

        private void ProcessGLAccountPaymentTerms(JournalLinePM entityPM,JournalPM entityParentPM)
        {
            
            JournalPM journal = entityParentPM;


            GLAccountQueryService glAccountQueryService = new GLAccountQueryService((MainContext as IAccountingContext));
            GLAccountPM glAccount = glAccountQueryService.GetGlaAccountByJouranlIdAndJournalLineNumber(entityPM.Tenant, entityPM.JournalId, entityPM.Line);

            // 3. כאשר נוצרת תנועה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
            // (שהמקור  שלה הוא פקודת יומן חיצונית(ממערכת יוניפרייט
            // Journals.ExternalSystem=UNIFREIGHT   >  מקור שלה ביוניפרייט
            // Journals.AccountingEntityCode = 1 > מסוג פקודת יומן
            // Journallines.actioncode=1  והתנועה היא מתוך שורת פקודת יומן בזכות

            if (journal.ExternalSystem == "UNIFREIGHT" && journal.AccountingEntityCode == "1" && entityPM.ActionCode == "1")
            {
                UpdateDueDate(glAccount, entityPM);
            }


            //4. כאשר נוצרת שורה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
            // glaccounts.ChartOfAccountsTypeCode = 4
            // שהמקור שלה הוא חשבונית ספק
            // Journals.AccountingEntityCode = 4 > מסוג חשבונית ספק
            // Journallines.actioncode = 1 > והתנועה היא מתוך שורת פקודת יומן בזכות


            if (glAccount != null && glAccount.ChartOfAccountsTypeCode == "4" && journal.AccountingEntityCode == "4" && entityPM.ActionCode == "1" )
            {
                UpdateDueDate(glAccount, entityPM);
            }
        }


        private void UpdateDueDate(GLAccountPM glAccount, JournalLinePM entityPM)
        {
            if (glAccount.PaymentTerms != null)
            {
                PaymentTermRepository paymentTermQueryService = new PaymentTermRepository((MainContext as ICommonDataContext));
                int paymentTermDays = paymentTermQueryService.GetSinglePaymentTerm(glAccount.PaymentTerms).Days;
                if(paymentTermDays != 0)
                {
                    LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService((MainContext as IAccountingContext));
                    LedgerTransactionPM ledgerTransaction = ledgerTransactionQueryService.GetLedgerTransactionPMByJournalId(entityPM.JournalId, entityPM.Tenant);
                    entityPM.DueDate = ledgerTransaction.DocumentDate.AddDays(paymentTermDays);
                }
            }
        }
    }
}
