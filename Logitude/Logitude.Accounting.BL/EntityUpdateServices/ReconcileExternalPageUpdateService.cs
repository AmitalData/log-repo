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
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.CloseTables;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconcileExternalPageUpdateService : EntityUpdateService<ReconcileExternalPage, ReconcileExternalPagePM, EntityPM>
    {
        protected override void OnCreating(ReconcileExternalPagePM entityPM, EntityPM entityParentPM)
        {

            if (entityPM.Id == null || entityPM.Id == "")
            {
                entityPM.Id = IdCounter.GetNumber("ReconcileExternalPage", entityPM.Tenant);

            }

            //created by 
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            entityPM.CreatedByUserId = contact.Id;
            entityPM.CreatedByUserName = contact.LocalName;
            //

            //fill Status
            if (string.IsNullOrWhiteSpace(entityPM.StatusCode))
            {
                entityPM.StatusCode = "1"; // 1- Draft
            }



            BankAccountPM bankAccountPM = GetBankAccountById(entityPM.EntityId, entityPM.Tenant);

            entityPM.PageNo = GetNewPageNumber(bankAccountPM);

            SetBankAccountLastPage(entityPM, bankAccountPM);

        }

        private static void SetBankAccountLastPage(ReconcileExternalPagePM entityPM, BankAccountPM bankAccountPM)
        {
            //update BankAccount
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            BankAccountUpdateService bankService = new BankAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);

            bankAccountPM.LastPageNumber = entityPM.PageNo.ToString();
            bankAccountPM.LastPageCloseBalance = entityPM.CloseBalance;
            bankAccountPM.LastPageEndDate = entityPM.ToDate;
            bankAccountPM.IsBankPageEvent = true;
            bankAccountPM.ChangeSetOp = ChangeSetOperation.Update;
            bankService.Update(bankAccountPM, true);
        }

        private int GetNewPageNumber(BankAccountPM bankAccountPM)
        {
            int pageNumber;
            if (string.IsNullOrWhiteSpace(bankAccountPM.LastPageNumber))
            {
                pageNumber = 1;
            }
            else
            {
                // old
                //int lastNumber = Convert.ToInt32(bank.LastPageNumber);
                //entityPM.PageNo = (lastNumber + 1).ToString();

                //new
                int lastPageNumber = GetLastPageNumberForBankAccount(bankAccountPM.Id, bankAccountPM.Tenant);
                pageNumber = (lastPageNumber + 1);

            }

            return pageNumber;
        }

        private int GetLastPageNumberForBankAccount(string bankAccountId, int tenant)
        {
            ReconcileExternalPageQueryService pageQuery = new ReconcileExternalPageQueryService(tenant);
            int lastPageNumber = pageQuery.GetLastPageNo(bankAccountId,"BankAccount", tenant);
            return lastPageNumber;
        }

        private static BankAccountPM GetBankAccountById(string bankAccountId, int tenant)
        {
            BankAccountQueryService bankQuery = new BankAccountQueryService(tenant);
            BankAccountPM bank = bankQuery.GetSingle(bankAccountId, false, false);
            return bank;
        }

        protected override void OnUpdating(ReconcileExternalPagePM entityPM)
        {


        }



        protected override void OnUpdating(ReconcileExternalPagePM pagePM, ReconcileExternalPage pagePOCO)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(pagePM.Tenant);
            if (pagePOCO.StatusCode != null && (pagePM.StatusCode == "3" && pagePOCO.StatusCode != "3"))
            {
                pagePM.ReconcileExternalPageLines.ForEach(a=>{ a.ChangeSetOp = ChangeSetOperation.None; });
                ObjectTable objectTable = GetObjectTable(pagePM.ObjectTableId,pagePM.Tenant);

                // canceled!!
                //update bankaccount , last page fields
                ReconcileExternalPageQueryService pageQuery = new ReconcileExternalPageQueryService(pagePOCO.Tenant);
                ReconcileExternalPagePM prevPage = pageQuery.GetPreviousPageByNumber(pagePM.PageNo, pagePM.EntityId, objectTable.Name, pagePOCO.Tenant);

                BankAccountQueryService bankQuery = new BankAccountQueryService(pagePOCO.Tenant);
                BankAccountPM bankAccount = bankQuery.GetSingle(pagePOCO.EntityId, false, false);

             
                BankAccountUpdateService bankService = new BankAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), pagePM.Tenant);
           

              

                bankAccount.LastPageCloseBalance = prevPage==null ? 0 : prevPage.CloseBalance;
                bankAccount.LastPageEndDate = prevPage == null ? DateTime.Now : prevPage.ToDate;
                bankAccount.LastPageNumber = prevPage == null ? null : prevPage.PageNo.ToString();
                bankAccount.IsBankPageEvent = true;
                bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                bankService.Update(bankAccount, true);

            }

            if (pagePOCO.StatusCode != null && (pagePM.StatusCode == "1" && pagePOCO.StatusCode == "3"))
            {
                // page restored!
                BankAccountPM bankAccountPM = GetBankAccountById(pagePM.EntityId, pagePM.Tenant);
                SetBankAccountLastPage(pagePM, bankAccountPM);

            }

            PaymentChequeQueryService paymentChequeService = new PaymentChequeQueryService(pagePM.Tenant);
            PaymentChequeUpdateService paymentChequeUpdService = new PaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), pagePM.Tenant);
            if (pagePM.ReconcileExternalPageLines.Count > 0)
            {
                foreach (var item in pagePM.ReconcileExternalPageLines)
                {
                    PaymentChequePM paymentCheque = paymentChequeService.GetPaymentChequeByChequeNoAndBankAccount(pagePM.EntityId, item.Reference, pagePM.Tenant);
                    if (paymentCheque != null)
                    {
                        paymentCheque.PaymentChequeStatusCode = "3";
                        paymentCheque.ChangeSetOp = ChangeSetOperation.Update;
                        paymentChequeUpdService.Update(paymentCheque, true);
                    }
                }
            }
            base.OnUpdating(pagePM, pagePOCO);
        }

        protected override void UpdateComposition(ReconcileExternalPagePM entityPM)
        {
            ReconcileExternalPageLineUpdateService reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            reconcileExternalPageLineUpdateService.UpdateMulti(entityPM.ReconcileExternalPageLines, entityPM.DeletedReconcileExternalPageLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(ReconcileExternalPagePM entityPM, EntityPM entityParentPM)
        {
           
        }

        public void CreateTraceEvent(ReconcileExternalPagePM entityPM, ContactPM loggedContact, string code)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = entityPM.Tenant,
                EventTypeCode = code,
                UserId = loggedContact.Id,
                EntityId = entityPM.Id,
                ObjectTableName = "ReconcileExternalPage",
                Notes = ""
            });

            
            
        }
        protected override void Trace(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO, string changesXml)
        {
            ContactPM contact= GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CreateTraceEvent(entityPM, contact, "CREV");
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update )
            {

                CreateTraceEvent(entityPM, contact, "UPEV");
                if(entityPOCO.StatusCode !="3" && entityPM.StatusCode == "3")
                {
                    CreateTraceEvent(entityPM, contact, "CNEV");

                }
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
                return OverrideGetLoggedContactFunc(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        protected override void Validate(ReconcileExternalPagePM entityPM)
        {

            var result = ReconcileExternalPageValidator.IsReconciliationValid(entityPM, null);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        void Validate_Move2_ReconcileExternalPageValidator(ReconcileExternalPagePM entityPM)
        {

            List<string> errors = new List<string>();
            if (entityPM.StatusCode != "1") // 1- Draft
            {

                CheckPreviousPage(entityPM, errors);
                ValidateLines(entityPM, errors);

                if (entityPM.StatusCode == "2") // 2- Approved
                    CheckPageBalance(entityPM, errors);

            }
            ObjectTable objectTable = GetObjectTable(entityPM.ObjectTableId, entityPM.Tenant);

            ReconcileExternalPageQueryService query = new ReconcileExternalPageQueryService(entityPM.Tenant);
            ReconcileExternalPagePM prevPage = query.GetPreviousPageByNumber(entityPM.PageNo, entityPM.EntityId, objectTable.Name, entityPM.Tenant);
            if (prevPage != null)
            {
                bool avoidCheckReferenceDate = true;//ohad+ eyal

                if (avoidCheckReferenceDate)
                {

                }
                //else
                //{
                //    //1
                //    if (entityPM.FromDate.Date <= prevPage.ToDate.Date)
                //    {
                //        errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate", entityPM.Tenant, useLocal));
                //    }
                //}
                ////2
                //if (entityPM.ToDate.Date < entityPM.FromDate.Date)
                //{
                //    errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate", entityPM.Tenant, useLocal));
                //}
            }

            CheckCreditAndDebitFieldForLines(entityPM);

            if (errors.Count > 0)
            {
                string errorString = ParseErrors(errors);
                throw new ApplicationException(errorString);
            }


        }

        private static string ParseErrors(List<string> errors)
        {
            // Parse error msg
            string errorString = "";
            foreach (string error in errors)
                errorString = errorString + error + ";";
            errorString = errorString.Remove(errorString.Length - 1);
            //
            return errorString;
        }

        private static void CheckPageBalance(ReconcileExternalPagePM entityPM, List<string> errors)
        {
            decimal sum = entityPM.StartBalance;
            if (entityPM.ReconcileExternalPageLines.Count > 0)
            {
                foreach (ReconcileExternalPageLinePM line in entityPM.ReconcileExternalPageLines)
                {
                    sum -= line.DebitAmount;
                    sum += line.CreditAmount;
                }
            }
            if (sum != entityPM.CloseBalance)
            {
                bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
                errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceNotEqualEndBalance", entityPM.Tenant, useLocal));
            }
        }

        private static void ValidateLines(ReconcileExternalPagePM entityPM, List<string> errors)
        {
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
            //4 validate lines
            if (entityPM.ReconcileExternalPageLines.Count > 0)
            {
                foreach (ReconcileExternalPageLinePM line in entityPM.ReconcileExternalPageLines)
                {
                    string lineWord = TranslateTextsClass.Translate("Accounting.General.O.Line", entityPM.Tenant, useLocal);
                    if (line.ReferenceDate < entityPM.FromDate || line.ReferenceDate > entityPM.ToDate)
                    {
                        errors.Add(lineWord + line.LineNumber + ": " + TranslateTextsClass.Translate("ReconcileExternalPage.O.RefDateShouldBiggerOrSmaller", entityPM.Tenant, useLocal));
                    }
                }
            }
        }

        private void CheckPreviousPage(ReconcileExternalPagePM pagePM, List<string> errors)
        {
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(pagePM.Tenant);
            ObjectTable objectTable = GetObjectTable(pagePM.ObjectTableId, pagePM.Tenant);

            ReconcileExternalPageQueryService query = new ReconcileExternalPageQueryService(pagePM.Tenant);
            ReconcileExternalPagePM prevPage = query.GetPreviousPageByNumber(pagePM.PageNo, pagePM.EntityId, objectTable.Name, pagePM.Tenant);
            bool isSamePage = prevPage?.Id == pagePM.Id;
            if (prevPage != null && !isSamePage)
            {
                //1
                if (pagePM.FromDate.Date <= prevPage.ToDate.Date)
                {
                    errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate", pagePM.Tenant, useLocal));
                }

                //2
                if (pagePM.ToDate.Date < pagePM.FromDate.Date)
                {
                    errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate", pagePM.Tenant, useLocal));
                }

                //3
                if (pagePM.StartBalance != prevPage.CloseBalance)
                {
                    errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance", pagePM.Tenant, useLocal));
                }
            }
        }

        private static void CheckCreditAndDebitFieldForLines(ReconcileExternalPagePM entityPM)
        {
            foreach (ReconcileExternalPageLinePM line in entityPM.ReconcileExternalPageLines)
            {
                if (line.CreditAmount != 0 && line.DebitAmount != 0)
                {
                    bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
                    string msg = TextCodesTranslator.TranslateText("ReconcileExternalPage.O.NoCreditAndDebit", entityPM.Tenant, showLocal);
                    msg = msg.Replace("#lineNo", line.LineNumber.ToString());
                    throw new ApplicationException(msg);
                }

            }
        }

        private static ObjectTable GetObjectTable(string id,int tenant)
        {
            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = tableRepository.GetSingleObjectTable(id, tenant, false);
            return objectTable;
        }
    }
}
