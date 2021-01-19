using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class TaxReportHelper
    {
        private static List<string> checkDigitErrorCodes = new List<string>() { "2" };
        private static List<string> invoiceErrorCodes = new List<string>() { "3" };

        public static void CheckErrorsInLines(AuthenticationToken authToken, TaxReportPM taxreport)
        {
            var linesWithCheckDigitErrors = CheckIfLinesHasErrors(taxreport, checkDigitErrorCodes, GetCheckDigitErrorMessage(taxreport.Tenant));
            var linesWithWrongInvoiceStatusMessage = CheckIfLinesHasErrors(taxreport, invoiceErrorCodes, GetInvoiceStatusErrorMessage(taxreport.Tenant));

            ThrowErrors(linesWithCheckDigitErrors, linesWithWrongInvoiceStatusMessage);
        }

        private static void ThrowErrors(string linesWithCheckDigitErrors, string linesWithWrongInvoiceStatusMessage)
        {
            var errorMessages = string.Join(";", linesWithCheckDigitErrors, linesWithWrongInvoiceStatusMessage);
            throw new ApplicationException(errorMessages);
        }

        private static string CheckIfLinesHasErrors(TaxReportPM taxreport, List<string> errorsCodes, string checkDigitError)
        {
            int[] linesWithError = GetLinesHasErrors(taxreport, errorsCodes);
            if (linesWithError.Length > 0)
                return checkDigitError.Replace("#lines", string.Join(",", linesWithError));
            return "";
        }

        private static string GetCheckDigitErrorMessage(int tenant)
        {
            return TextCodesTranslator.TranslateText("TaxReport.O.CantDownload", tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant));
        }
        private static string GetInvoiceStatusErrorMessage(int tenant)
        {
            return TextCodesTranslator.TranslateText("TaxReport.O.InvoiceErrors", tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant));
        }

        private static int[] GetLinesHasErrors(TaxReportPM taxreport, List<string> errorsCodes)
        {
            TaxReportQueryService taxReportQuery = new TaxReportQueryService(taxreport.Tenant);
            int[] linesWithError = taxReportQuery.CheckErrorsInLines(taxreport.Id, taxreport.Tenant, errorsCodes).ToArray();
            return linesWithError;
        }

        public static void CheckWithoutTransmitLines(AuthenticationToken authToken, TaxReportPM taxreport)
        {
            bool IsWithoutTransmitLineExist = CheckIfWithoutTransmitLineExist(taxreport.Id, taxreport.Tenant);
            ContactPM loggedContact = GetLoggedContact(authToken.Email, taxreport.Tenant);
            bool showlocal = !loggedContact.DontShowLocal;
            if (IsWithoutTransmitLineExist)
            {
                throw new Exception(TextCodesTranslator.TranslateText("TaxReport.O.CantApprove", taxreport.Tenant, showlocal));
            }
        }

       private static bool CheckIfWithoutTransmitLineExist(string taxReportId, int tenant)
        {
            TaxReportQueryService taxReportQuery = new TaxReportQueryService(tenant);
            return taxReportQuery.CheckIfThereIsLineWithoutTransmit(taxReportId, tenant);

        }


        private static ContactPM GetLoggedContact(string loggedUserEmail, int tenant)
        {

            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }


            return loggedContactPM;
        }
    }
}