using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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

        public static void CheckErrorsInLines(AuthenticationToken authToken, TaxReportPM taxreport)
        {
            List<string> errorsCodes = new List<string>() { "2" };
            TaxReportQueryService taxReportQuery = new TaxReportQueryService(taxreport.Tenant);
            int[] linesWithError = taxReportQuery.CheckErrorsInLines(taxreport.Id, taxreport.Tenant, errorsCodes).ToArray();
            if (linesWithError.Length > 0)
            {
                ContactPM loggedContact = GetLoggedContact(authToken.Email, taxreport.Tenant);
                bool showlocal = !loggedContact.DontShowLocal;
                string error = TextCodesTranslator.TranslateText("TaxReport.O.CantDownload", taxreport.Tenant, showlocal);
                string[] errorParts = error.Split(',');
                string lines = null;
                for (int x = 0; x < linesWithError.Length; x++)
                {
                    lines = lines + linesWithError[x].ToString() + ',';

                }
                throw new Exception(errorParts[0] + " ( " + lines.TrimEnd(',') + " ) " + errorParts[1]);
            }
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