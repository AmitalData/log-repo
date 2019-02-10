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
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.Validators
{
    public partial class CashBookValidator
    {


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }



        public static ValidationResult IsCashBookValid(CashBookPM myCashBookPM)
        {
            ContactPM currenctUser = GetLoggedContact(myCashBookPM.Tenant);
            bool useLocal = true;
            if (currenctUser != null)
                useLocal = !currenctUser.DontShowLocal;

            if (myCashBookPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                // Check if there is cashbook with current currency and type
                if (!String.IsNullOrWhiteSpace(myCashBookPM.CurrencyId))
                {
                    CashBookRepository repo = new CashBookRepository(myCashBookPM.Tenant);
                    bool exist = repo.GetByCurrencyAndTypeAndBranch(myCashBookPM.CurrencyId, myCashBookPM.CashBookTypeCode,myCashBookPM.BranchId, myCashBookPM.Tenant).Any();
                    if (exist)
                    {
                        return new ValidationResult(TranslateTextsClass.Translate("Accounting.General.O.Thereischashbookwithcurrencytypebranch", 0, useLocal)); // There is chashbook with the choosen currency, type & branch
                    }
                }
                else
                {
                    return new ValidationResult("Select a currency!");
                }
            }


            // get account, currency
            GLAccountQueryService accountQuery = new GLAccountQueryService(myCashBookPM.Tenant);
            GLAccountPM account = accountQuery.GetSingle(myCashBookPM.AccountId, false, false);


            if (!string.IsNullOrWhiteSpace(account.CurrencyId) && !string.IsNullOrWhiteSpace(myCashBookPM.CurrencyId))
                if (account.CurrencyId != myCashBookPM.CurrencyId)
                    return new ValidationResult(TranslateTextsClass.Translate("Accounting.General.O.GLAccountcurnotmatchcashbookcur", 0, useLocal));

            return null;

        }

    }
}
