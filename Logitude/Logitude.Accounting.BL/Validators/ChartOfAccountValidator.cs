using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.Validators
{
    public partial class ChartOfAccountValidator
    {

        public static ValidationResult IsChartOfAccountValid(ChartOfAccountPM myChartOfAccountPM)
        {
            // Check Connected GLAccounts
            if (!String.IsNullOrWhiteSpace(myChartOfAccountPM.TypeCode))
            {
                    bool connected = CheckConnected(myChartOfAccountPM);
                    if (connected == true)
                    {
                        return new ValidationResult(TextCodesTranslator.TranslateText("Accounting.General.O.ConnectedToGLA", myChartOfAccountPM.Tenant));
                    }
            }

          //  bool valid = true;
            if (!String.IsNullOrWhiteSpace(myChartOfAccountPM.ParentId) && !String.IsNullOrWhiteSpace(myChartOfAccountPM.Id) && myChartOfAccountPM.ParentId == myChartOfAccountPM.Id)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("ChartOfAccounts.O.CannotBeItself", myChartOfAccountPM.Tenant));
            }

            if (!String.IsNullOrWhiteSpace(myChartOfAccountPM.ParentId))
            {
                string parentError = CheckParent(myChartOfAccountPM.ParentId, myChartOfAccountPM.TypeCode, myChartOfAccountPM.Tenant);
                if (!String.IsNullOrEmpty(parentError))
                {
                    return new ValidationResult(parentError);
                }
            }




            bool exists = CheckCode(myChartOfAccountPM.Code, myChartOfAccountPM.Id, myChartOfAccountPM.Tenant);
            if (exists == true)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("ChartOfAccounts.O.CodeAlreadyExists", myChartOfAccountPM.Tenant));
            }
            return null;
        }


        // server side validations
        public static bool CheckCode(string code, string id, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountQueryService chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            return chartOfAccountQuery.CheckWhetherCodeExists(code, id, tenant);
        }


        public static string CheckParent(string parentId, string childTypeCode, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountQueryService chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            ChartOfAccountPM parentPM = chartOfAccountQuery.GetSingle(parentId, false, true);
            if (parentPM == null)
            {
                return TextCodesTranslator.TranslateText("ChartOfAccounts.O.ParentDoesNotExist", tenant);
            }
            else if (parentPM.TypeCode != childTypeCode)
            {
                return TextCodesTranslator.TranslateText("ChartOfAccounts.O.WrongParentType", tenant);
            }
            return null;
        }

        public static bool CheckConnected(ChartOfAccountPM entityPm)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPm.Tenant);
            ChartOfAccountQueryService chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            ChartOfAccountPM oldPm = chartOfAccountQuery.GetSingle(entityPm.Id, false, false);
            if (oldPm != null)
            {
                if (entityPm.Inactive != oldPm.Inactive || entityPm.TypeCode != oldPm.TypeCode)
                {
                    return chartOfAccountQuery.CheckWhetherConnected(entityPm.Code, entityPm.Id, entityPm.Tenant);
                }
            }
            return false;


        }

    }
}
