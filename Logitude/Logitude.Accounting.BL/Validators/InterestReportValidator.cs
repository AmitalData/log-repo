using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Server.Infrastructure;
using System.Runtime.InteropServices;

namespace Logitude.Accounting.BL.Validators
{
    public partial class InterestReportValidator
    {
        // server side validations
        public static ValidationResult IsInterestReportValid(InterestReportPM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                ValidateIsCardHasGLAccount(entityPM, showLocals);
                ValidateIsAlreadyHasSameValue(entityPM, showLocals);
                ValidateIsGLAccountActiveForInterest(entityPM, showLocals);

            }
            //if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            //{
            //    ValidateCanUpdaeReport(entityPM, showLocals);
            //}
           
            ValidateIfThereIsARecentInvoicedOrClosedReport(entityPM, showLocals);

            return null;
        }

        private static List<string> GetStatusesThatAllowEditing()
        {
            List<string> StatusesThatAllowEditing = new List<string>();
            StatusesThatAllowEditing.Add("1");
            StatusesThatAllowEditing.Add("6");
            StatusesThatAllowEditing.Add("9");
            StatusesThatAllowEditing.Add(null);
            

            return StatusesThatAllowEditing;
        }


        private static void ValidateCanUpdaeReport(InterestReportPM entityPM, bool showLocals)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            string InterestReportStatusCode = interestReportRepository.GetInterestReportStatusCode(entityPM.Id , entityPM.Tenant);
            List<string> StatusesThatAllowEditing = GetStatusesThatAllowEditing();

            if (!StatusesThatAllowEditing.Contains(InterestReportStatusCode) && entityPM.InterestReportStatusCode !="3")
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.UpdatingInvoicepermitted", entityPM.Tenant, showLocals));
            }
        }

        private static void ValidateIfThereIsARecentInvoicedOrClosedReport(InterestReportPM entityPM, bool showLocals)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByGraterInterestCalculationDate(entityPM.CustomerId, entityPM.Id, entityPM.InterestCalculationDate, entityPM.Tenant);
            if (interestReport != null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customeralreadyhasarecent", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);
            }
        }

        private static void ValidateIsCardHasGLAccount(InterestReportPM entityPM, bool showLocals)
        {
            CardRepository cardRepository = new CardRepository(entityPM.Tenant);
            Card card = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
            if (card.GLAccountId == null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotconnected", entityPM.Tenant, showLocals));
            }
            entityPM.GLAccountId = card.GLAccountId;

        }

        private static void ValidateIsGLAccountActiveForInterest(InterestReportPM entityPM, bool showLocals)
        {
            GLAccountRepository gLAccountRepository = new GLAccountRepository(entityPM.Tenant);
            GLAccount gLAccount = gLAccountRepository.GetSingle(entityPM.GLAccountId, entityPM.Tenant);
            if (gLAccount.ActiveForInterest == false)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotdefined", entityPM.Tenant, showLocals));
            }
            entityPM.GLAccountInterestCreditLimit = gLAccount.InterestCreditLimit;

        }

        private static void ValidateIsAlreadyHasSameValue(InterestReportPM entityPM, bool showLocals)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByCusstomerAndStatudDraft(entityPM.CustomerId, entityPM.Tenant);
            if (interestReport != null)
            {
                if (interestReport.InterestReportStatusCode =="1")
                {
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.CustomeralreadyhasaDraftinterest", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);

                }
                else
                {
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customeralreadyhasaninprogress", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);
                }
            }

            //interestReport = interestReportRepository.GetSingleByGraterInterestCalculationDate(entityPM.CustomerId, entityPM.InterestCalculationDate, entityPM.Tenant);
            //if (interestReport != null)
            //{
            //    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customeralreadyhasarecent", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);
            //}

        }



        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}