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
            if(entityParentPM?.AccountingEntityCode == "12") 
            { 
               entityPM.Reference2 = entityParentPM.JournalNumber;
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

        private void ProcessGLAccountPaymentTerms(JournalLinePM journalLine, JournalPM journal)
        {
            try
            {
                string accountId = journalLine.ActionCode == "1" ? journalLine.CreditAccountId : journalLine.ActionCode == "2" ? journalLine.DebitAccountId : null;
                if(accountId != null)
                {
                    GLAccountQueryService glAccountQueryService = new GLAccountQueryService(journalLine.Tenant); 
                    GLAccountPM glAccount = glAccountQueryService.GetSingle(accountId, false, false);

                    // 3. כאשר נוצרת תנועה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
                    // (שהמקור  שלה הוא פקודת יומן חיצונית(ממערכת יוניפרייט
                    // Journals.ExternalSystem=UNIFREIGHT   >  מקור שלה ביוניפרייט
                    // Journals.AccountingEntityCode = 1 > מסוג פקודת יומן
                    // Journallines.actioncode=1  והתנועה היא מתוך שורת פקודת יומן בזכות
                    if (journal?.ExternalSystem == "UNIFREIGHT" && journal?.AccountingEntityCode == "1" && journalLine?.ActionCode == "1")
                    {
                        UpdateDueDate(glAccount, journalLine);
                    }

                    //4. כאשר נוצרת שורה בזכות מתוך שורת פקודת יומן בזכות  לכרטיס ספק
                    // glaccounts.ChartOfAccountsTypeCode = 4
                    // שהמקור שלה הוא חשבונית ספק
                    // Journals.AccountingEntityCode = 4 > מסוג חשבונית ספק
                    // Journallines.actioncode = 1 > והתנועה היא מתוך שורת פקודת יומן בזכות

                    else if (glAccount?.ChartOfAccountsTypeCode == "4" && journal?.AccountingEntityCode == "4" && journalLine?.ActionCode == "1")
                    {
                        UpdateDueDate(glAccount, journalLine);
                    }
                }                
            }
            catch
            {
                throw new Exception();
            }

        }

        private void UpdateDueDate( GLAccountPM glAccount, JournalLinePM journalLine)
        {

            PaymentTermRepository paymentTermQueryService = new PaymentTermRepository(journalLine.Tenant);
                
            string paymentTermsId = glAccount.PaymentTerms != null ? glAccount.PaymentTerms : glAccount.PaymentTermId != null ? glAccount.PaymentTermId : null;
            if (paymentTermsId != null)
            {
                PaymentTerm paymentTerm = paymentTermQueryService.GetSinglePaymentTerm(paymentTermsId);
                int paymentTermDays = paymentTerm.Days;
                if (paymentTermDays != 0)
                {
                    DateTime updatedDocumentDate = journalLine.DocumentDate;
                    if (paymentTerm.EndOfMonth) 
                    {
                        updatedDocumentDate = updatedDocumentDate.AddMonths(paymentTerm.NumberOfMonths);

                        updatedDocumentDate = GetLastDayOfMonth(updatedDocumentDate);
                    }

                    journalLine.DueDate = updatedDocumentDate.AddDays(paymentTermDays);
                }
            }
        }

        private DateTime GetLastDayOfMonth(DateTime currentDate)
        {
            // Use DaysInMonth to get the last day of the month
            int lastDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            
            // Create a new DateTime object with the same year and month, but the last day
            DateTime lastDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, lastDay);
            return lastDayOfMonth;
        }

    }
}
