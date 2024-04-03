using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.Validators
{
    public class ReconcileExternalPageValidator
    {
        private const string approvedBankPageStatusCode = "2";
        public static ValidationResult IsReconciliationValid(ReconcileExternalPagePM entityPM, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            List<string> errorsList = new List<string>();
            if (entityPM.StatusCode != "1") // 1- Draft
            {

                ContactPM currenctUser = GetLoggedContact(entityPM.Tenant);
                bool useLocal = true;
                if (currenctUser != null)
                    useLocal = !currenctUser.DontShowLocal;

                ObjectTable objectTable = GetObjectTable(entityPM);

                ReconcileExternalPageQueryService query = new ReconcileExternalPageQueryService(entityPM.Tenant);
                ReconcileExternalPagePM prevPage = query.GetPreviousPageByNumber(entityPM.PageNo, entityPM.EntityId, objectTable.Name, entityPM.Tenant);
                if (prevPage != null)
                {

                    bool avoidCheckReferenceDate = true;//ohad+ eyal

                    if (avoidCheckReferenceDate)
                    {

                    }
                    else
                    {
                        //1
                        if (entityPM.FromDate.Date <= prevPage.ToDate.Date)
                        {
                            errorsList.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate", entityPM.Tenant, useLocal));
                        }
                    }


                    //2
                    if (entityPM.ToDate.Date < entityPM.FromDate.Date)
                    {
                        errorsList.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate", entityPM.Tenant, useLocal));
                    }

                    //3
                    if (entityPM.StartBalance != prevPage.CloseBalance)
                    {
                        errorsList.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance", entityPM.Tenant, useLocal));
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
                            errorsList.Add(lineWord + line.LineNumber + ": " + TranslateTextsClass.Translate("ReconcileExternalPage.O.RefDateShouldBiggerOrSmaller", entityPM.Tenant, useLocal));
                        }
                    }
                }

                if (entityPM.StatusCode == approvedBankPageStatusCode)
                {
                    CheckPageDifference(entityPM, errorsList, useLocal);
                }




                //if (errorsList.Count > 0)
                //{
                //    // Parse error msg
                //    string errorString = "";
                //    foreach (string error in errorsList)
                //        errorString = errorString + error + ";";
                //    errorString = errorString.Remove(errorString.Length - 1);
                //    //

                //    throw new ApplicationException(errorString);
                //}
            }

            // credit and debit
            CheckCreditAndDebitFieldForLines(entityPM);
            if (errorsList.Count == 0)
            {
                return ValidationResult.Success;
            }

            else
            {
                string errorString = String.Empty;
                foreach (string error in errorsList)
                {
                    errorString = errorString + error + ";";
                }

                errorString = errorString.Remove(errorString.Length - 1);
                return new ValidationResult("ReconcileExternalPage Not Valid" + ": " + errorString, errorsList);
            }
        }

        private static void CheckPageDifference(ReconcileExternalPagePM entityPM, List<string> errorsList, bool useLocal)
        {
            decimal calculatedClosedBalance = CalculatePageClosedBalance(entityPM);

            if (calculatedClosedBalance != entityPM.CloseBalance)
                errorsList.Add(TranslateTextsClass.Translate("ReconcileExternalPage.O.StartBalanceNotEqualEndBalance", entityPM.Tenant, useLocal));
        }

        private static decimal CalculatePageClosedBalance(ReconcileExternalPagePM entityPM)
        {
            decimal pageTotal = entityPM.StartBalance;
            var notDeletedPageLines = entityPM.ReconcileExternalPageLines.Where(page => page.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            foreach (ReconcileExternalPageLinePM line in notDeletedPageLines)
            {
                pageTotal -= line.DebitAmount;
                pageTotal += line.CreditAmount;
            }

            return pageTotal;
        }

        private static ObjectTable GetObjectTable(ReconcileExternalPagePM entityPM)
        {
            ObjectTableRepository tableRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = tableRepository.GetSingleObjectTable(entityPM.ObjectTableId, entityPM.Tenant, false);
            return objectTable;
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
                return OverrideGetLoggedContactFunc(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
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
