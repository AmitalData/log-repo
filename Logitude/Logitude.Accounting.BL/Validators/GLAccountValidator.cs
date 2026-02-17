using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.Validators
{
    public partial class GLAccountValidator
    {

        public static ValidationResult IsGLAccountValid(GLAccountPM myGLAccountPM)
        {
            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(myGLAccountPM.Tenant);
            bool showLocals = !contact.DontShowLocal;


            if (String.IsNullOrWhiteSpace(myGLAccountPM.AccountTypeCode))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.AccountTypeCodeMissing", myGLAccountPM.Tenant, showLocals));
            }
            
            if (myGLAccountPM.IsMultiCurrency != true && String.IsNullOrWhiteSpace(myGLAccountPM.CurrencyId))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.CurrencyOrMulti", myGLAccountPM.Tenant, showLocals));
            }

            if (myGLAccountPM.IsMultiCurrency == true && !String.IsNullOrWhiteSpace(myGLAccountPM.CurrencyId))
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.AccountIsaMulti", myGLAccountPM.Tenant, showLocals));
            }
            
            bool exists = CheckDisplayNumber(myGLAccountPM.DisplayNumber, myGLAccountPM.InternalNumber, myGLAccountPM.Tenant);
            if (exists == true)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.DisplayNumberAlreadyExists", myGLAccountPM.Tenant, showLocals));
            }

            bool internalExists = CheckInternalNumber(myGLAccountPM.InternalNumber, myGLAccountPM.Id, myGLAccountPM.Tenant);
            if (internalExists == true)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.InternalNumberAlreadyExists", myGLAccountPM.Tenant, showLocals));
            }

            //if (!String.IsNullOrWhiteSpace(myGLAccountPM.ClientId))
            //{
            //    bool clientExists = CheckClientAndCurrency(myGLAccountPM.ClientId, myGLAccountPM.CurrencyId, myGLAccountPM.InternalNumber, myGLAccountPM.Tenant);
            //    if (clientExists == true)
            //    {
            //        if (String.IsNullOrWhiteSpace(myGLAccountPM.CurrencyId))
            //        {
            //            return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.ClientMultiAlreadyExists", myGLAccountPM.Tenant));
            //        }
            //        else
            //        {
            //            return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.ClientCurrencyAlreadyExists", myGLAccountPM.Tenant) + myGLAccountPM.CurrencyCode);
            //        }
            //    }
            //}

            //if (!String.IsNullOrWhiteSpace(myGLAccountPM.VendorId))
            //{
            //    bool vendorExists = CheckVendorAndCurrency(myGLAccountPM.VendorId, myGLAccountPM.CurrencyId, myGLAccountPM.InternalNumber, myGLAccountPM.Tenant);
            //    if (vendorExists == true)
            //    {
            //        if (String.IsNullOrWhiteSpace(myGLAccountPM.CurrencyId))
            //        {
            //            return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.VendorMultiAlreadyExists", myGLAccountPM.Tenant));
            //        }
            //        else
            //        {
            //            return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.VendorCurrencyAlreadyExists", myGLAccountPM.Tenant) + myGLAccountPM.CurrencyCode);
            //        }
            //    }
            //}


            if (!String.IsNullOrWhiteSpace(myGLAccountPM.CurrencyId))
            {
                bool otherCurrencyExists = CheckIfLedgerTransactionOtherCurrencyExist(myGLAccountPM.Id, myGLAccountPM.CurrencyId, myGLAccountPM.Tenant);
                if (otherCurrencyExists == true)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("ThereTransactions4GLAwithexistingCurrency", 0));
                }
            }


            if (!String.IsNullOrWhiteSpace(myGLAccountPM.CustomerGLAccountId))
            {
                bool customerExists = CheckIfGLAccountExists(myGLAccountPM.CustomerGLAccountId, myGLAccountPM.Tenant);
                if (customerExists != true)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.CustomerAccountNotFound", myGLAccountPM.Tenant, showLocals));
                }
            }

            if (!String.IsNullOrWhiteSpace(myGLAccountPM.ChartOfAccountsId))
            {
                string chartError = CheckChartOfAccounts(myGLAccountPM.ChartOfAccountsId, myGLAccountPM.ChartOfAccountsTypeCode, myGLAccountPM.Tenant);
                if (!String.IsNullOrEmpty(chartError))
                {
                    return new ValidationResult(chartError);
                }
            }

            


            if (myGLAccountPM.AccountTypeCode == "1" && myGLAccountPM.RevenueExpenseType != "1" && myGLAccountPM.RevenueExpenseType != "2")
            {
                //return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.CardRevExpOnly", myGLAccountPM.Tenant));
            }
            else if (myGLAccountPM.AccountTypeCode != "1" && myGLAccountPM.RevenueExpenseType != "3")
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.RevExpOther", myGLAccountPM.Tenant, showLocals));
            }
            else if (myGLAccountPM.AccountTypeCode == "5")
            {
                if (String.IsNullOrWhiteSpace(myGLAccountPM.CustomerGLAccountId) && !String.IsNullOrWhiteSpace(myGLAccountPM.CustomerGLAccountInternalNumber))
                {
                    String acc_id = GetCustomerGLAccountIdByInternalNumber(myGLAccountPM.CustomerGLAccountInternalNumber, myGLAccountPM.Tenant);
                    if (!String.IsNullOrWhiteSpace(acc_id))
                    {
                        myGLAccountPM.CustomerGLAccountId = acc_id;
                    }
                }

                if (String.IsNullOrWhiteSpace(myGLAccountPM.CustomerGLAccountId))
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.CustomerAccountMissing", myGLAccountPM.Tenant, showLocals));
                }
                else
                {
                    string customerError = CheckCustomerGLAccount(myGLAccountPM.CustomerGLAccountId, myGLAccountPM.Tenant);
                    if (!String.IsNullOrEmpty(customerError))
                    {
                        return new ValidationResult(customerError);
                    }

                }
            }

            if (myGLAccountPM.IsMultiCurrency == true && myGLAccountPM.ReconcileMethodCode != "0")
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("GLAccounts.O.LocalCurrencyErr", myGLAccountPM.Tenant, showLocals));
            }
            if (myGLAccountPM.IsControlAccount.GetValueOrDefault())
            {
                var fullPM =FullAccountingSettingQueryService.Get(myGLAccountPM.Tenant);
                if (fullPM==null)
                {
                    return null;
                }
                var list = new List<string> ()
                {
                        
                        fullPM.AirExportJobControlAccountId ,
                        fullPM.AirImportJobControlAccountId ,
                        fullPM.CustomerControlAccountId ,
                        fullPM.OceanExportJobControlAccountId ,
                        fullPM.OceanImportJobControlAccountId,
                        fullPM.VendorControlAccountId,
                        fullPM.FileControlAccountId,
                        
                };
                if (list.Contains(myGLAccountPM.Id) == false)
                {
                    return new ValidationResult(
                       // "The Account is defined as a Control Account but is not connected to the Full Accounting Settings"
                        TextCodesTranslator.TranslateText("GLAccounts.O.ControlAccountNotDefined", myGLAccountPM.Tenant)
                        );
                }
            }
            else if (String.IsNullOrWhiteSpace(myGLAccountPM.ControlAccountId) && (myGLAccountPM.AccountTypeCode == "4" || myGLAccountPM.AccountTypeCode == "5" || myGLAccountPM.AccountTypeCode == "2" || myGLAccountPM.AccountTypeCode == "3"))
            {
                return new ValidationResult(//"Control Account is missing"
                    TextCodesTranslator.TranslateText("GLAccounts.O.ControlAccountMissing", myGLAccountPM.Tenant));
            }
            else if (!String.IsNullOrWhiteSpace(myGLAccountPM.ControlAccountId)) // check TypeCode vs Control Account TypeCode
            {
                GLAccountQueryService query = new GLAccountQueryService(myGLAccountPM.Tenant);
                GLAccountPM controlAccount = query.GetSingle(myGLAccountPM.ControlAccountId, false, true);
                if (controlAccount == null)
                {
                    return new ValidationResult(//"Control Account is not found"
                        TextCodesTranslator.TranslateText("GLAccounts.O.ControlAccountNotFound", myGLAccountPM.Tenant));
                }
                else if (controlAccount.ChartOfAccountsTypeCode != myGLAccountPM.ChartOfAccountsTypeCode)
                {
                    return new ValidationResult(//"Control Account's Chart of Accounts Type differs from this GL Account"
                        TextCodesTranslator.TranslateText("GLAccounts.O.WrongControlChartType", myGLAccountPM.Tenant));
                }
                if (controlAccount != null)
                {
                    var rescontrolAccountValid = GLAccountValidator.IsGLAccountValid(controlAccount);
                    if (rescontrolAccountValid != null)
                    {
                        return rescontrolAccountValid;
                    }
                }
	
                

            }

            //
            // Check parent
            if (!string.IsNullOrWhiteSpace(myGLAccountPM.ParentAccountId))
            {
                string errorMessage = CheckParent(myGLAccountPM, myGLAccountPM.ParentAccountId, myGLAccountPM.Tenant);
                if (!string.IsNullOrEmpty(errorMessage))
                    return new ValidationResult(errorMessage);
            }

            

            return null;
        }


        // server side validations
        public static bool CheckDisplayNumber(string displayNo, string internalNumber, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService gLAccountQuery = new GLAccountQueryService(accountingContext);
            return gLAccountQuery.CheckIfDisplayNumberExists(displayNo, internalNumber, tenant);
        }

        public static bool CheckInternalNumber(string internalNumber, string id, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService gLAccountQuery = new GLAccountQueryService(accountingContext);
            return gLAccountQuery.CheckIfInternalNumberExists(internalNumber, id, tenant);
        }

        //public static bool CheckClientAndCurrency(string clientId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(clientId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
        //        GLAccountQueryService gLAccountQuery = new GLAccountQueryService(accountingContext);
        //        return gLAccountQuery.CheckIfClientAndCurrencyExist(clientId, currencyId, internalNumber, tenant);
        //    }
        //}

        //public static bool CheckVendorAndCurrency(string vendorId, string currencyId, string internalNumber, int tenant)
        //{
        //    if (String.IsNullOrEmpty(vendorId))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
        //        GLAccountQueryService gLAccountQuery = new GLAccountQueryService(accountingContext);
        //        return gLAccountQuery.CheckIfVendorAndCurrencyExist(vendorId, currencyId, internalNumber, tenant);
        //    }
        //}

        public static bool CheckIfLedgerTransactionOtherCurrencyExist(string gLAccountId, string currencyId, int tenant)
        {
            if (String.IsNullOrEmpty(currencyId))
            {
                return false;
            }
            else
            {
                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                LedgerTransactionQueryService ledgerTransactionQuery = new LedgerTransactionQueryService(accountingContext);
                return ledgerTransactionQuery.CheckIfLedgerTransactionOtherCurrencyExist(gLAccountId, currencyId, tenant);
            }
        }


        public static bool CheckIfGLAccountExists(string id, int tenant)
        {
            if (String.IsNullOrEmpty(id))
            {
                return true; // no id - no problem 
            }
            else
            {
                IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
                GLAccountListQueryService gLAccountListQueryService = new GLAccountListQueryService(accountingContext);
                GLAccountList gLAccountList = gLAccountListQueryService.GetSingle(id);
                if (gLAccountList != null)
                {
                    return true;
                }
                else
                {
                    return false; // the problem is only when non-existent id is given 
                }
            }
        }



        public static string CheckChartOfAccounts(string chartId, string accountChartTypeCode, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountQueryService chartOfAccountQuery = new ChartOfAccountQueryService(accountingContext);
            ChartOfAccountPM chart = chartOfAccountQuery.GetSingle(chartId, false, true);
            if (chart == null)
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.ChartOfAccountsNotFound", tenant);
            }
            else if (chart.TypeCode != accountChartTypeCode)
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.WrongParentType", tenant);
            }
            return null;
        }


        public static string CheckCustomerGLAccount(string id, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService query = new GLAccountQueryService(accountingContext);
            GLAccountPM acc = query.GetSingle(id, false, true);
            if (acc == null)
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.CustomerAccountNotFound", tenant);
            }
            else if (acc.AccountTypeCode != "2")
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.WrongCustomerAccountType", tenant);
            }
            return null;
        }

        public static string GetCustomerGLAccountIdByInternalNumber(String internalNumber, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService query = new GLAccountQueryService(accountingContext);
            GLAccountPM acc = query.GetByInternalNumber(internalNumber, tenant).FirstOrDefault<GLAccountPM>();
            if (acc != null)
            {
                return acc.Id;
            }
            else
            {
                return null;
            }
        }

        public static string CheckParent(GLAccountPM glaccountPM, string parentId, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            GLAccountQueryService query = new GLAccountQueryService(accountingContext);
            GLAccountPM parentPM = query.GetSingle(parentId, false, true);

            if (parentPM == null || glaccountPM == null)
            {
                return "Account not found!";
            }
            else if (parentPM.Id == glaccountPM.Id)
            {
                return "Parent cannot be the account itself!";
            }
            else if (parentPM.ChartOfAccountsTypeCode != glaccountPM.ChartOfAccountsTypeCode)
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.GLAParentValidation1",0);
                //return "GLAccount and its parent must be same chart of account type!";
            }
            else if (parentPM.ChartOfAccountsId != glaccountPM.ChartOfAccountsId)
            {
                return TextCodesTranslator.TranslateText("GLAccounts.O.GLAParentValidation2",0);
                //return "GLAccount and its parent must be same chart of account!";
            }
            else if (parentPM.AccountTypeCode != glaccountPM.AccountTypeCode)
            {
                return "GLAccount and its parent must be same account type!";
            }
            else if (parentPM.AccountTypeCode != glaccountPM.AccountTypeCode)
            {
                return "GLAccount and its parent must be same account type!";
            }
            else if (!string.IsNullOrWhiteSpace(parentPM.ParentAccountId))
            {
                return "Cannot connect GL Account to parent account that has parent (multi level is not allowd)!"; //לא ניתן לקשר כרטיס לכרטיס אב שיש לו כרטיס אב
            }
            return null;
        }

        private static ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }
}
