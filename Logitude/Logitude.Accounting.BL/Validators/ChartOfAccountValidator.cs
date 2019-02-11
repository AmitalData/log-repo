using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

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
            ChartOfAccountPM parentPM = chartOfAccountQuery.GetSingle(parentId, false, false);

            // null
            if (parentPM == null)
            {
                return TextCodesTranslator.TranslateText("ChartOfAccounts.O.ParentDoesNotExist", tenant);
            }

            // parent type
            if (parentPM.TypeCode != childTypeCode)
            {
                return TextCodesTranslator.TranslateText("ChartOfAccounts.O.WrongParentType", tenant);
            }

            return null;
        }

        public static string CheckParentChild(string parentId, string childId, int tenant)
        {
            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountRepository repo = new ChartOfAccountRepository(accountingContext);
            ChartOfAccount parent = repo.GetSingle(parentId, tenant);

            if (parent != null && childId != null)
            {
                if (parent.Id == childId)
                    return TextCodesTranslator.TranslateText("ChartOfAccounts.O.ParentIsChild", tenant, showLocals);
                else
                {
                    if (parent.ParentId != null)
                        return CheckParentChild(parent.ParentId, childId, tenant);
                    else
                        return null;
                }
            }
            else
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


        public static ContactPM GetLoggedContact(int tenant)
        {

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }
}
