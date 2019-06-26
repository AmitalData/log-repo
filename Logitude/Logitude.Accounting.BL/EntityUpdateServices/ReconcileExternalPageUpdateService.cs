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


            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            BankAccountUpdateService bankService = new BankAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            BankAccountQueryService bankQuery = new BankAccountQueryService(entityPM.Tenant);
            ReconcileExternalPageQueryService pageQuery = new ReconcileExternalPageQueryService(entityPM.Tenant);
            BankAccountPM bank = bankQuery.GetSingle(entityPM.BankAccountId, false, false);


            //fill new PageNo
            if (string.IsNullOrWhiteSpace(bank.LastPageNumber))
            {
                entityPM.PageNo = 1;
            }
            else
            {
                // old
                //int lastNumber = Convert.ToInt32(bank.LastPageNumber);
                //entityPM.PageNo = (lastNumber + 1).ToString();

                //new
                int lastPageNumber = pageQuery.GetLastPageNo(entityPM.BankAccountId, entityPM.Tenant);
                entityPM.PageNo = (lastPageNumber + 1);

            }

            //entityPM.prev = bank.LastPageNumber;

            //update BankAccount
            bank.LastPageNumber = entityPM.PageNo.ToString();
            bank.LastPageCloseBalance = entityPM.CloseBalance;
            bank.LastPageEndDate = entityPM.ToDate;
            bank.IsBankPageEvent = true;
            bank.ChangeSetOp = ChangeSetOperation.Update;
            bankService.Update(bank, true);


        }

        protected override void OnUpdating(ReconcileExternalPagePM entityPM)
        {


        }



        protected override void OnUpdating(ReconcileExternalPagePM entityPM, ReconcileExternalPage entityPOCO)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            if (entityPOCO.StatusCode != null && (entityPM.StatusCode == "3" && entityPOCO.StatusCode != "3"))
            {
                // canceled!!
                //update bankaccount , last page fields
                ReconcileExternalPageQueryService pageQuery = new ReconcileExternalPageQueryService(entityPOCO.Tenant);
                ReconcileExternalPagePM prevPage = pageQuery.GetPrevPageNoByPageNo(entityPM.PageNo, entityPM.BankAccountId, entityPOCO.Tenant);

                BankAccountQueryService bankQuery = new BankAccountQueryService(entityPOCO.Tenant);
                BankAccountPM bankAccount = bankQuery.GetSingle(entityPOCO.BankAccountId, false, false);

             
                BankAccountUpdateService bankService = new BankAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
           

              

                bankAccount.LastPageCloseBalance = prevPage==null ? 0 : prevPage.CloseBalance;
                bankAccount.LastPageEndDate = prevPage == null ? DateTime.Now : prevPage.ToDate;
                bankAccount.LastPageNumber = prevPage == null ? null : prevPage.PageNo.ToString();
                bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                bankService.Update(bankAccount, true);

            }

            PaymentChequeQueryService paymentChequeService = new PaymentChequeQueryService(entityPM.Tenant);
            PaymentChequeUpdateService paymentChequeUpdService = new PaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            if (entityPM.ReconcileExternalPageLines.Count > 0)
            {
                foreach (var item in entityPM.ReconcileExternalPageLines)
                {
                    PaymentChequePM paymentCheque = paymentChequeService.GetPaymentChequeByChequeNoAndBankAccount(entityPM.BankAccountId, item.Reference, entityPM.Tenant);
                    if (paymentCheque != null)
                    {
                        paymentCheque.PaymentChequeStatusCode = "3";
                        paymentCheque.ChangeSetOp = ChangeSetOperation.Update;
                        paymentChequeUpdService.Update(paymentCheque, true);
                    }
                }
            }
            base.OnUpdating(entityPM, entityPOCO);
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
                CreateTraceEvent(entityPM, contact, "CNEV");
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                CreateTraceEvent(entityPM, contact, "UPEV");
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
            if (entityPM.StatusCode != "1") // 1- Draft
            {

                ContactPM currenctUser = GetLoggedContact(entityPM.Tenant);
                bool useLocal = true;
                if (currenctUser != null)
                    useLocal = !currenctUser.DontShowLocal;


                List<string> errors = new List<string>();
                ReconcileExternalPageQueryService query = new ReconcileExternalPageQueryService(entityPM.Tenant);
                ReconcileExternalPagePM prevPage = query.GetPrevPageNoByPageNo(entityPM.PageNo, entityPM.BankAccountId, entityPM.Tenant);
                if (prevPage != null)
                {
                    //1
                    if (entityPM.FromDate.Date <= prevPage.ToDate.Date)
                    {
                        errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate", entityPM.Tenant, useLocal));
                    }

                    //2
                    if (entityPM.ToDate.Date < entityPM.FromDate.Date)
                    {
                        errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate", entityPM.Tenant, useLocal));
                    }

                    //3
                    if (entityPM.StartBalance != prevPage.CloseBalance)
                    {
                        errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance", entityPM.Tenant, useLocal));
                    }
                }

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

                //5 approve logic
                if (entityPM.StatusCode == "2") // 2- Approved
                {
                    decimal sum = entityPM.StartBalance;
                    if (entityPM.ReconcileExternalPageLines.Count > 0)
                    {
                        foreach (ReconcileExternalPageLinePM line in entityPM.ReconcileExternalPageLines)
                        {
                            sum += line.DebitAmount;
                            sum -= line.CreditAmount;
                        }
                    }
                    if (sum != entityPM.CloseBalance)
                    {
                        errors.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceNotEqualEndBalance", entityPM.Tenant, useLocal));
                    }
                }




                if (errors.Count > 0)
                {
                    // Parse error msg
                    string errorString = "";
                    foreach (string error in errors)
                        errorString = errorString + error + ";";
                    errorString = errorString.Remove(errorString.Length - 1);
                    //

                    throw new ApplicationException(errorString);
                }
            }

            // credit and debit
            CheckCreditAndDebitFieldForLines(entityPM);

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
    }
}
