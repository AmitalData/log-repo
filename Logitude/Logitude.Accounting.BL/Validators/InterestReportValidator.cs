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
                ValidateIfPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(entityPM, showLocals);

            }
            ValidateIfThereIsARecentInvoicedOrClosedReport(entityPM, showLocals);
            return null;
        }

        private static void ValidateIfThereIsARecentInvoicedOrClosedReport(InterestReportPM entityPM, bool showLocals)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(entityPM.Tenant);
            InterestReport interestReport = interestReportRepository.GetSingleByGraterInterestCalculationDate(entityPM.CustomerId, entityPM.InterestCalculationDate, entityPM.Tenant);
            if (interestReport != null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customeralreadyhasarecent", entityPM.Tenant, showLocals) + " " + interestReport.ReportNumber);
            }
        }

        private static void ValidateIfPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(InterestReportPM entityPM, bool showLocals)
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(entityPM.Tenant);
            InterestReportPM interestReportPM = interestReportQueryService.GetPreviousInvoicedOrCloseWithoutInvoicedtInterestReportForCustomer(
                                                entityPM.CustomerId, entityPM.Tenant, entityPM.InterestCalculationDate);
            if (interestReportPM != null)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.CannotCreateReportWithCalculationDateLess", entityPM.Tenant, showLocals) + " " + interestReportPM.ReportNumber + " " + TextCodesTranslator.TranslateText("InterestReport.O.on", entityPM.Tenant, showLocals) + " " + interestReportPM.InterestCalculationDate.ToString("dd/MM/yyyy") + TextCodesTranslator.TranslateText("InterestReport.O.AlreadyExists", entityPM.Tenant, showLocals));
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
            if (gLAccount.ActiveForInterest ==null || gLAccount.ActiveForInterest == false)
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

        public static bool IsCreateInvoicedValid(InterestReportPM InterestReportPM, int tenant)
        {
            bool IsValid = true;
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            string InterestReportStatusCode = interestReportQueryService.GetInterestReportStatusCode(InterestReportPM.Id, tenant);
            if (InterestReportStatusCode != "1" && InterestReportStatusCode != "9")
            {
                IsValid = false;
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.CreatingInvoicepermitted", tenant, showLocals));
            }


            return IsValid;

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