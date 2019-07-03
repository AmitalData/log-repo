using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Simplog.Data.Helpers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class GLAccountUpdateService : EntityUpdateService<GLAccount, GLAccountPM, EntityPM>
    {
        private class GLAccountRepositoryPriv : GLAccountRepository
        {

            public GLAccountRepositoryPriv(IAccountingContext mainContext)
                : base(mainContext)
            { }
        }
        protected override void AddContext(GLAccountPM myTEntityPM)
        {
            base.AddContext(myTEntityPM);
            SetPriv();
        }

        private void SetPriv()
        {
            this.Repository = (this.Repository as GLAccountRepositoryPriv) ?? new GLAccountRepositoryPriv((IAccountingContext)this.MainContext);
        }

        public void AddPocoFromBuildTenant(GLAccount poco)
        {
            SetPriv();
            this.Repository.Add(poco);

        }
        protected override void OnCreating(GLAccountPM entityPM, EntityPM entityParentPM)

        {
            AddAcitivityLog(entityPM, "N");
            entityPM.SearchFields = entityPM.DisplayNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName;

            ContactPM loggedUser = GetLoggedContact(entityPM.Tenant);
            entityPM.CreatedByUserId = loggedUser?.Id;

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsId) && (entityPM.ChartOfAccountsId.ToLower() == "bla" || entityPM.ChartOfAccountsId.ToLower() == "get")) entityPM.ChartOfAccountsId = null;
            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsTypeCode) && (entityPM.ChartOfAccountsTypeCode.ToLower() == "bla" || entityPM.ChartOfAccountsTypeCode.ToLower() == "get")) entityPM.ChartOfAccountsTypeCode = null;


            if (entityPM.AccountTypeCode == "4") // Job
            {
                // first get application
                string application = entityPM.Application;
                if (String.IsNullOrWhiteSpace(application))
                {
                    if (!String.IsNullOrWhiteSpace(entityPM.DisplayNumber) && entityPM.DisplayNumber.Length > 4 && entityPM.DisplayNumber.Substring(0, 3) == "SPD")
                    {
                        application = entityPM.DisplayNumber.Substring(3, 1);
                    }
                }
                if (!String.IsNullOrWhiteSpace(application))
                {
                    // get Job Control Account
                    string jobControlAccountId = "";
                    FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
                    FullAccountingSettingPM fullAccountingSetting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(entityPM.Tenant);
                    if (fullAccountingSetting != null)
                    {
                        switch (application)
                        {
                            case "E":
                                jobControlAccountId = fullAccountingSetting.AirExportJobControlAccountId;
                                break;
                            case "M":
                                jobControlAccountId = fullAccountingSetting.OceanExportJobControlAccountId;
                                break;
                            case "I":
                                jobControlAccountId = fullAccountingSetting.AirImportJobControlAccountId;
                                break;
                            case "R":
                                jobControlAccountId = fullAccountingSetting.OceanImportJobControlAccountId;
                                break;
                            default:
                                break;
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(jobControlAccountId))
                    {
                        entityPM.ControlAccountId = jobControlAccountId;
                    }
                }

            }
            else if (entityPM.AccountTypeCode == "5") // File
            {
                // first get File Control Account
                string fileControlAccountId = "";
                FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
                FullAccountingSettingPM fullAccountingSetting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(entityPM.Tenant);
                if (fullAccountingSetting != null)
                {
                    fileControlAccountId = fullAccountingSetting.FileControlAccountId;
                }
                if (!String.IsNullOrWhiteSpace(fileControlAccountId))
                {
                    entityPM.ControlAccountId = fileControlAccountId;
                }
                string application = "";
                if (!String.IsNullOrWhiteSpace(entityPM.DisplayNumber) && entityPM.DisplayNumber.Length > 4 && entityPM.DisplayNumber.Substring(0, 3) == "MTK")
                {
                    application = entityPM.DisplayNumber.Substring(3, 1);
                }
                if (!String.IsNullOrWhiteSpace(application) && application == "C" && !String.IsNullOrWhiteSpace(entityPM.CustomerCode))
                {
                    // get Cards.Code
                    CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                    Card card = cardRepository.GetSingleCardByCode(entityPM.CustomerCode, entityPM.Tenant, true);
                    if (card != null && !String.IsNullOrWhiteSpace(card.GLAccountId))
                    {
                        entityPM.CustomerGLAccountId = card.GLAccountId;
                    }
                }
            }
            else if (entityPM.AccountTypeCode == "2") // Customer
            {
                string customerControlAccountId = "";
                FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
                FullAccountingSettingPM fullAccountingSetting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(entityPM.Tenant);
                if (fullAccountingSetting != null)
                {
                    customerControlAccountId = fullAccountingSetting.CustomerControlAccountId;
                }
                if (!String.IsNullOrWhiteSpace(customerControlAccountId))
                {
                    entityPM.ControlAccountId = customerControlAccountId;
                }
            }
            else if (entityPM.AccountTypeCode == "3") // Vendor
            {
                string vendorControlAccountId = "";
                FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
                FullAccountingSettingPM fullAccountingSetting = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(entityPM.Tenant);
                if (fullAccountingSetting != null)
                {
                    vendorControlAccountId = fullAccountingSetting.VendorControlAccountId;
                }
                if (!String.IsNullOrWhiteSpace(vendorControlAccountId))
                {
                    entityPM.ControlAccountId = vendorControlAccountId;
                }
            }
            if (entityPM.AccountTypeCode == "4" || entityPM.AccountTypeCode == "5") // Job,file
                                                                                    //            if (entityPM.AccountTypeCode == "4" || entityPM.AccountTypeCode == "5" || entityPM.AccountTypeCode == "2" || entityPM.AccountTypeCode == "3") // Job,file,customer,vendor
            {
                // get Chart of Accounts and its type code
                if (!String.IsNullOrWhiteSpace(entityPM.ControlAccountId))
                {
                    GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                    GLAccountPM controlAccount = query.GetSingle(entityPM.ControlAccountId, false, true);
                    if (controlAccount != null)
                    {
                        if (!String.IsNullOrWhiteSpace(controlAccount.ChartOfAccountsId))
                        {
                            entityPM.ChartOfAccountsId = controlAccount.ChartOfAccountsId;
                        }
                        if (!String.IsNullOrWhiteSpace(controlAccount.ChartOfAccountsTypeCode))
                        {
                            entityPM.ChartOfAccountsTypeCode = controlAccount.ChartOfAccountsTypeCode;
                        }
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(entityPM.CurrencyCode) && String.IsNullOrWhiteSpace(entityPM.CurrencyId))
            {
                CurrencyQuery queryService = new CurrencyQuery(entityPM.Tenant);
                CurrencyPM currency = queryService.GetSingleCurrencyByCode(entityPM.CurrencyCode, entityPM.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyId = currency.Id;
                    entityPM.CurrencyName = currency.EnglishName;
                }
            }


            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsCode)) ;// && String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsId))
            {
                ChartOfAccountQueryService queryService = new ChartOfAccountQueryService(entityPM.Tenant);
                ChartOfAccountPM chart = queryService.GetByCode(entityPM.ChartOfAccountsCode, entityPM.Tenant).FirstOrDefault();
                if (chart != null)
                {
                    entityPM.ChartOfAccountsId = chart.Id;
                    entityPM.ChartOfAccountsName = chart.EnglishName;
                }
            }

            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsId) && String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsTypeCode))
            {
                ChartOfAccountQueryService queryService = new ChartOfAccountQueryService(entityPM.Tenant);
                ChartOfAccountPM chart = queryService.GetSingle(entityPM.ChartOfAccountsId, false, true);
                if (chart != null)
                {
                    entityPM.ChartOfAccountsTypeCode = chart.TypeCode;
                    entityPM.ChartOfAccountsTypeName = chart.TypeName;
                }
            }

            if (String.IsNullOrWhiteSpace(entityPM.CustomerGLAccountId) && !String.IsNullOrWhiteSpace(entityPM.CustomerGLAccountInternalNumber))
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                GLAccountPM acc = query.GetByInternalNumber(entityPM.CustomerGLAccountInternalNumber, entityPM.Tenant).FirstOrDefault<GLAccountPM>();
                if (acc != null)
                {
                    entityPM.CustomerGLAccountId = acc.Id;
                }
            }

            if (String.IsNullOrWhiteSpace(entityPM.DeductionTypeId) && !String.IsNullOrWhiteSpace(entityPM.DeductionTypeCode))
            {
                AccountingCompanyTypeQueryService queryService = new AccountingCompanyTypeQueryService(entityPM.Tenant);
                AccountingCompanyTypePM ctype = queryService.GetByCode(entityPM.DeductionTypeCode, entityPM.Tenant);
                if (ctype != null)
                {
                    entityPM.DeductionTypeId = ctype.Id;
                    if (!String.IsNullOrWhiteSpace(ctype.LocalName))
                    {
                        entityPM.DeductionTypeName = ctype.LocalName;
                    }
                    {
                        entityPM.DeductionTypeName = ctype.EnglishName;
                    }
                }
            }



            if (String.IsNullOrWhiteSpace(entityPM.AssessingOfficeCode) && !String.IsNullOrWhiteSpace(entityPM.AssessingOfficeNumber))
            {
                TaxWithholdingAssessOfficeQueryService queryService = new TaxWithholdingAssessOfficeQueryService(entityPM.Tenant);
                TaxWithholdingAssessOfficePM office = queryService.GetByNumber(entityPM.AssessingOfficeNumber, entityPM.Tenant);
                if (office != null)
                {
                    entityPM.AssessingOfficeCode = office.Id;
                    if (!String.IsNullOrWhiteSpace(office.LocalName))
                    {
                        entityPM.AssessingOfficeName = office.LocalName;
                    }
                    {
                        entityPM.AssessingOfficeName = office.Name;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(entityPM.DeductionFileTypeId) && !String.IsNullOrWhiteSpace(entityPM.DeductionFileTypeCode))
            {
                WithholdingTaxDeductionTypeQueryService queryService = new WithholdingTaxDeductionTypeQueryService(entityPM.Tenant);
                WithholdingTaxDeductionTypePM ftype = queryService.GetByCode(entityPM.DeductionFileTypeCode, entityPM.Tenant);
                if (ftype != null)
                {
                    entityPM.DeductionFileTypeId = ftype.Id;
                    if (!String.IsNullOrWhiteSpace(ftype.LocalName))
                    {
                        entityPM.DeductionFileTypeName = ftype.LocalName;
                    }
                    {
                        entityPM.DeductionFileTypeName = ftype.EnglishName;
                    }
                }
            }




            if (entityPM.InternalNumber != null)
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                GLAccountPM acc = query.GetByInternalNumber(entityPM.InternalNumber, entityPM.Tenant).FirstOrDefault<GLAccountPM>();
                if (acc != null)
                {
                    throw new Exception("Existing GLAccount found with Internal No. " + entityPM.InternalNumber + " (Display No. " + acc.DisplayNumber + ")");
                }
            }

            if (entityPM.InternalNumber == null || entityPM.InternalNumber == "") entityPM.InternalNumber = /*CodeCounter*/(new CodeCounterWrapper(true)).GetNumber("GLAccount", entityPM.Tenant).ToString();
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("GLAccount", entityPM.Tenant);
            if (entityPM.ReconcileMethodCode == null || entityPM.ReconcileMethodCode == "") entityPM.ReconcileMethodCode = "0";
            GLAccountCurrencyPM gLAccountCurrency = null;
            if (!String.IsNullOrWhiteSpace(entityPM.ParentAccountByCurrency))
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                GLAccountPM acc = query.GetByInternalNumber(entityPM.ParentAccountByCurrency, entityPM.Tenant).FirstOrDefault<GLAccountPM>();
                if (acc != null)
                {
                    gLAccountCurrency = new GLAccountCurrencyPM()
                    {
                        CurrencyId = entityPM.CurrencyId,
                        Tenant = entityPM.Tenant,
                        GLAccountId = entityPM.Id,
                        MainGLAccountId = acc.Id,
                        Id = IdCounter.GetNumber("GLAccountCurrency", entityPM.Tenant),
                    };
                    gLAccountCurrency.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;


                }
                else
                {
                    throw new Exception("Parent Account by Currency " + entityPM.ParentAccountByCurrency + " not found");
                }

            }





            entityPM.Inactive = entityPM.Inactive ?? false;//Why there isn't init

            //    SubmitChanges();
            //if (gLAccountCurrency != null) {
            //    IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            //    GLAccountCurrencyUpdateService currencyUpdateService = new GLAccountCurrencyUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            //    currencyUpdateService.Update(gLAccountCurrency, true);
            //}


            base.OnCreating(entityPM, entityParentPM);

        }
        protected override void UpdateComposition(GLAccountPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                InsertGLAccountMoreData(entityPM);
            }

            var gLAccountWithholdingTaxUpdateService = new GLAccountWithholdingTaxUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            gLAccountWithholdingTaxUpdateService.UpdateMulti(entityPM.GLAccountWithholdingTaxes, entityPM.DeletedGLAccountWithholdingTaxes, entityPM, true);


            base.UpdateComposition(entityPM);
        }
        private void InsertGLAccountMoreData(GLAccountPM entityPM)
        {
            var myGLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myGLAccountMoreDataUpdateService.Update(new GLAccountMoreDataPM()
            {
                AccountId = entityPM.Id,
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert

            }, false);
        }

        private static string GetLoggedContactId(GLAccountPM entityPM)
        {
            string contactId = null;

            if (entityPM.PassedFromAPI)
            {
                UserQuery userQuery = new UserQuery(entityPM.Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + entityPM.Tenant + ".com", entityPM.Tenant, false);
            }

            else
            {
                contactId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            }

            return contactId;
        }

        protected override void OnUpdating(GLAccountPM entityPM, GLAccount entityPOCO)
        {

            ContactPM loggedUser = GetLoggedContact(entityPM.Tenant);
            bool useLocal = !((bool)loggedUser?.DontShowLocal);

            entityPM.UpdatedByUserId = loggedUser?.Id;
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


            entityPM.IsControlAccount = entityPM.IsControlAccount ?? false;//Task 47485: GLAccount - Update Service - Set Null fields as 0 (False)
            entityPM.IsMultiCurrency = entityPM.IsMultiCurrency ?? false;//Task 47485: GLAccount - Update Service - Set Null fields as 0 (False)

            if (entityPM.AccountTypeCode == "5") // File
            {
                string application = "";
                if (!String.IsNullOrWhiteSpace(entityPM.DisplayNumber) && entityPM.DisplayNumber.Length > 4 && entityPM.DisplayNumber.Substring(0, 3) == "MTK")
                {
                    application = entityPM.DisplayNumber.Substring(3, 1);
                }
                if (!String.IsNullOrWhiteSpace(application) && application == "C" && !String.IsNullOrWhiteSpace(entityPM.CustomerCode))
                {
                    // get Cards.Code
                    CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                    Card card = cardRepository.GetSingleCardByCode(entityPM.CustomerCode, entityPM.Tenant, true);
                    if (card != null && !String.IsNullOrWhiteSpace(card.GLAccountId))
                    {
                        entityPM.CustomerGLAccountId = card.GLAccountId;
                    }
                }
            }

            // Chart of Account change validation
            if(entityPOCO.ChartOfAccountsId != entityPM.ChartOfAccountsId)
            {
                // ChartOfAccount changed
                // get transactions in closed period
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPOCO.Tenant);
                AccountingPeriodQueryService periodQuery = new AccountingPeriodQueryService(entityPOCO.Tenant);
                List<AccountingPeriodPM> tenantPeriods = periodQuery.GetAccountingPeriodsByTenantAndType("1", entityPOCO.Tenant); // 1- Accounting, Regular
                if (tenantPeriods.Count() > 0)
                {
                    //
                    // note: the opened periods may be found in a different years, 
                    //       so I fetch the opened periods over years and check its closed transactions
                    //

                    DateTime closedDate;
                    DateTime openDate;

                    //List<AccountingPeriodPM> periodsWithOpenedMonths = tenantPeriods.Where(d => d.ClosedMonth != d.OpenMonth).ToList();
                    foreach (AccountingPeriodPM period in tenantPeriods)
                    {
                        // prepare closed month date
                        if (period.ClosedMonth == null)
                            closedDate = new DateTime(period.Year, 1, 1, 0, 0, 0);
                        else
                            closedDate = new DateTime(period.Year, period.ClosedMonth.Value, DateTime.DaysInMonth(period.Year, period.ClosedMonth.Value), 23, 59, 59);

                        // prepare open month date
                        openDate = new DateTime(period.Year, period.OpenMonth, 1, 0, 0, 0);

                        // get transactions in closed period
                        IQueryable<LedgerTransaction> transactions = transQuery.GetClosedPeriodTransactions(entityPOCO.Id, closedDate, openDate, entityPOCO.Tenant);
                        if (transactions.Count() > 0)
                        {
                            throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccounts.O.ChartOfAccountCantChangedGLAhaveTrans", entityPOCO.Tenant, useLocal));
                        }
                    }

                }

            }

            //CurrencyQuery currencyQuery = new CurrencyQuery(entityPM.Tenant);
            //if (entityPM.ConnectedItems != null)
            //{
            //    entityPM.ConnectedItems = entityPM.ConnectedItems.Substring(0, entityPM.ConnectedItems.Length - 1);

            //    string[] items = entityPM.ConnectedItems.Split(',');

            //    GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
            //    GLAccountPM controlAccount = query.GetSingle(entityPM.ControlAccountId, false, true);
            //    foreach (var item in items)
            //    {
            //        CurrencyPM currency = currencyQuery.GetSingleCurrencyByCode(item, entityPM.Tenant);
            //        string currencyId = null;
            //        if (currency != null)
            //        {
            //            currencyId = currency.Id;
            //        }
            //        GLAccountPM newAccount = new GLAccountPM()
            //        {
            //            InternalNumber = CodeCounter.GetNumber("GLAccount", entityPM.Tenant).ToString(),
            //            AccountTypeCode = "2",
            //            DisplayNumber = entityPM.DisplayNumber + "\\" + item,
            //            LocalName = entityPM.LocalName + "\\" + item,
            //            EnglishName = entityPM.EnglishName + "\\" + item,
            //            IsMultiCurrency = false,
            //            Id = IdCounter.GetNumber("GLAccount", entityPM.Tenant),
            //            Tenant = entityPM.Tenant,
            //            ChangeSetOp = ChangeSetOperation.Insert,
            //            CurrencyId = currencyId,
            //            RevenueExpenseType = "3",
            //            CustomerGLAccountId = entityPM.Id,
            //            IsControlAccount=false,
            //            ChartOfAccountsId= entityPM.ChartOfAccountsId,
            //            ChartOfAccountsTypeCode= controlAccount.ChartOfAccountsTypeCode,
            //            ReconcileMethodCode= entityPM.ReconcileMethodCode,
            //            AutomaticReconcileId= entityPM.AutomaticReconcileId,
            //            ControlAccountId = entityPM.ControlAccountId


            //        };
            //        this.Update(newAccount, true);
            //        SubmitChanges();
            //        GLAccountCurrencyPM newGLAccountCurrencyPM = new GLAccountCurrencyPM()
            //        {
            //            MainGLAccountId = entityPM.Id,
            //            CurrencyId = currencyId,
            //            GLAccountId = newAccount.Id,
            //            ChangeSetOp = ChangeSetOperation.Insert,
            //            Tenant = entityPM.Tenant,


            //        };
            //        IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            //        GLAccountCurrencyUpdateService currencyUpdateService = new GLAccountCurrencyUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            //        currencyUpdateService.Update(newGLAccountCurrencyPM, true);


            //    }
            //}



            OnUpdatingCheckBalance(entityPM, entityPOCO);
            OnUpdatingCheckReconcileMethod(entityPM, entityPOCO);

            ValidateCurrency(entityPM, entityPOCO);
        }

        protected virtual void OnUpdatingCheckBalance(GLAccountPM entityPM, GLAccount entityPOCO)
        {
#if GLAccMoreData


            var messnoprivtochangeBalanceInLocalCurrency = "no priv to change BalanceInLocalCurrency";
            if (entityPOCO == null)
            {
                return;
            }
            var delta = entityPM.BalanceInLocalCurrency.GetValueOrDefault() - entityPOCO.BalanceInLocalCurrency.GetValueOrDefault();
            if (delta!=0)
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
            delta = entityPM.LocalBalanceInDue.GetValueOrDefault() - entityPOCO.LocalBalanceInDue.GetValueOrDefault();
            if (delta != 0)
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
            if (entityPM.NextDueDate.GetValueOrDefault() != entityPOCO.NextDueDate.GetValueOrDefault())
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
#endif
        }

        protected virtual void OnUpdatingCheckReconcileMethod(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            // Task 45932: GLAccounts: new validation for the field reconcile method

            // GET logged contact, RTL
            IAccountingContext accountingContext = AccountingContext.GetContext(entityPOCO.Tenant);
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant);
            bool showLocals = !contact.DontShowLocal;


            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (entityPM.ReconcileMethodCode != entityPOCO.ReconcileMethodCode && entityPM.IsMultiCurrency == false)
                {
                    //check glaccount transactions
                    LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(accountingContext);
                    LedgerTransactionPM trans = transQuery.GetFirstLedgerTransaction(entityPM.Id, entityPM.Tenant);
                    if(trans != null)
                        throw new Exception(TextCodesTranslator.TranslateText("GLAccounts.O.ReconcileMethodcantUpdated",0,showLocals));
                }
            }
            
        }

        protected override void OnUpdating(GLAccountPM entityPM)
        {
            AddAcitivityLog(entityPM,"U");

            if (!String.IsNullOrWhiteSpace(entityPM.CurrencyCode) && String.IsNullOrWhiteSpace(entityPM.CurrencyId))
            {
                CurrencyQuery queryService = new CurrencyQuery(entityPM.Tenant);
                CurrencyPM currency = queryService.GetSingleCurrencyByCode(entityPM.CurrencyCode, entityPM.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyId = currency.Id;
                    entityPM.CurrencyName = currency.EnglishName;
                }
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsId) && String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsTypeCode))
            {
                ChartOfAccountQueryService queryService = new ChartOfAccountQueryService(entityPM.Tenant);
                ChartOfAccountPM chart = queryService.GetSingle(entityPM.ChartOfAccountsId, false, true);
                if (chart != null)
                {
                    entityPM.ChartOfAccountsTypeCode = chart.TypeCode;
                    entityPM.ChartOfAccountsTypeName = chart.TypeName;
                }
            }

            if (String.IsNullOrWhiteSpace(entityPM.DeductionTypeId) && !String.IsNullOrWhiteSpace(entityPM.DeductionTypeCode))
            {
                AccountingCompanyTypeQueryService queryService = new AccountingCompanyTypeQueryService(entityPM.Tenant);
                AccountingCompanyTypePM ctype = queryService.GetByCode(entityPM.DeductionTypeCode, entityPM.Tenant);
                if (ctype != null)
                {
                    entityPM.DeductionTypeId = ctype.Id;
                    if (!String.IsNullOrWhiteSpace(ctype.LocalName))
                    {
                        entityPM.DeductionTypeName = ctype.LocalName;
                    }
                    {
                        entityPM.DeductionTypeName = ctype.EnglishName;
                    }
                }
            }


            if (String.IsNullOrWhiteSpace(entityPM.AssessingOfficeCode) && !String.IsNullOrWhiteSpace(entityPM.AssessingOfficeNumber))
            {
                TaxWithholdingAssessOfficeQueryService queryService = new TaxWithholdingAssessOfficeQueryService(entityPM.Tenant);
                TaxWithholdingAssessOfficePM office = queryService.GetByNumber(entityPM.AssessingOfficeNumber, entityPM.Tenant);
                if (office != null)
                {
                    entityPM.AssessingOfficeCode = office.Id;
                    if (!String.IsNullOrWhiteSpace(office.LocalName))
                    {
                        entityPM.AssessingOfficeName = office.LocalName;
                    }
                    {
                        entityPM.AssessingOfficeName = office.Name;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(entityPM.DeductionFileTypeId) && !String.IsNullOrWhiteSpace(entityPM.DeductionFileTypeCode))
            {
                WithholdingTaxDeductionTypeQueryService queryService = new WithholdingTaxDeductionTypeQueryService(entityPM.Tenant);
                WithholdingTaxDeductionTypePM ftype = queryService.GetByCode(entityPM.DeductionFileTypeCode, entityPM.Tenant);
                if (ftype != null)
                {
                    entityPM.DeductionFileTypeId = ftype.Id;
                    if (!String.IsNullOrWhiteSpace(ftype.LocalName))
                    {
                        entityPM.DeductionFileTypeName = ftype.LocalName;
                    }
                    {
                        entityPM.DeductionFileTypeName = ftype.EnglishName;
                    }
                }
            }


            if (String.IsNullOrWhiteSpace(entityPM.CustomerGLAccountId) && !String.IsNullOrWhiteSpace(entityPM.CustomerGLAccountInternalNumber))
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                GLAccountPM acc = query.GetByInternalNumber(entityPM.CustomerGLAccountInternalNumber, entityPM.Tenant).FirstOrDefault<GLAccountPM>();
                if (acc != null)
                {
                    entityPM.CustomerGLAccountId = acc.Id;
                }
            }

            entityPM.SearchFields = entityPM.DisplayNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            base.OnUpdating(entityPM);
        }

        public virtual void AddAcitivityLog(GLAccountPM entityPM, string activityTypeCode)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("GLAccount", 0, true);

            var loggedContactId = GetLoggedContactId(entityPM);
            if (loggedContactId != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, activityTypeCode, loggedContactId);
            }
        }



        protected override void Trace(GLAccountPM entityPM, GLAccount entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "GLAccount",
                    IsAddedManually = false,
                    EventTypeCode = "ACR",
                    

                });


                if (entityPM.Type == "ADDED")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.Added", entityPM.Tenant,true);
                    string[] text = s.Split('-');

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.CustomerGLAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "ADD",
                        Notes = text[0] + "- " + entityPM.DisplayNumber + " " + text[1],

                    });
                }

            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                if (entityPM.GLAccountWithholdingTaxes.Count > 0)
                {
                    foreach (var line in entityPM.GLAccountWithholdingTaxes)
                    {
                        if (line.Changed)
                        {
                            var currentContextTag = line.CurrentContextTag ?? "";
                            if (currentContextTag.ToString() == GLAccountWithholdingTaxUpdateService.RaiseEventWBLKConst)
                            {
                                String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingBlocked", entityPM.Tenant).Replace(":", line.LineNumber + ":");
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "GLAccount",
                                    IsAddedManually = false,
                                    EventTypeCode = "AWBK",
                                    Notes = notes,
                                });
                            }
                            else if (currentContextTag.ToString() == GLAccountWithholdingTaxUpdateService.RaiseEventWLDAConst)
                            {
                                String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingLineDisabled", entityPM.Tenant).Replace(":", line.LineNumber + ":");
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "GLAccount",
                                    IsAddedManually = false,
                                    EventTypeCode = "AWDA",
                                    Notes = notes,
                                });
                            }
                            else if (currentContextTag.ToString() == GLAccountWithholdingTaxUpdateService.RaiseEventAWNCConst)
                            {
                                String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingLineCreated", entityPM.Tenant).Replace(":", line.LineNumber + ":");
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "GLAccount",
                                    IsAddedManually = false,
                                    EventTypeCode = "AWNC",
                                    Notes = notes,
                                });
                            }

                            if (line.Inactive)
                            {
                                string s = TranslateTextsClass.Translate("Accounting.O.LineDeactivated", entityPM.Tenant, true);
                                string[] text = s.Split('-');
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "GLAccount",
                                    IsAddedManually = false,
                                    EventTypeCode = "DETV",
                                    Notes = text[0] + " " + line.LineNumber + " " + text[1],

                                });
                            }
                            else
                            {
                                string s = TranslateTextsClass.Translate("Accounting.O.LineActivated", entityPM.Tenant, true);
                                string[] text = s.Split(' ');
                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    EntityId = entityPM.Id,
                                    Tenant = entityPM.Tenant,
                                    UserId = contact.Id,
                                    ObjectTableName = "GLAccount",
                                    IsAddedManually = false,
                                    EventTypeCode = "LIAC",
                                    Notes = text[0] + " " + line.LineNumber + " " + text[1],

                                });
                            }
                        }
                    }
                }
               
                //create trace event with updated type.
                if (entityPM.DisplayNumber != entityPOCO.DisplayNumber && (!String.IsNullOrEmpty(entityPM.DisplayNumber) || !String.IsNullOrEmpty(entityPOCO.DisplayNumber)))
                {
                
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.DisplayNumber.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.DisplayNumber.ToString();
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "ACH",
                        Notes=notes,

                    });
                    
                  //  EntityPOCO.PreviousNumber = entityPOCO.DisplayNumber;
                  //  EntityPOCO.PreviousNumberChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {

                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    string oldname = null;
                    string newName = null;
                    if (entityPOCO.EnglishName == null)
                    {
                        oldname = entityPOCO.LocalName;
                    }
                    else oldname = entityPOCO.EnglishName;


                    if (entityPM.EnglishName == null)
                    {
                        newName = entityPM.LocalName;
                    }
                    else newName = entityPM.EnglishName;


                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldname + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + newName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "ENCH",
                        Notes = notes,

                    });
                 //   EntityPOCO.PreviousEnglishName = entityPOCO.EnglishName;
                 //   EntityPOCO.PreviousEnglishNameChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {

                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    string oldName = null;
                    string newName = null;
                    if (entityPOCO.EnglishName == null)
                    {
                        oldName = entityPOCO.LocalName;
                    }
                    else oldName = entityPOCO.EnglishName;


                    if (entityPM.EnglishName == null)
                    {
                        newName = entityPM.LocalName;
                    }
                    else newName = entityPM.EnglishName;
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + newName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "LCCH",
                        Notes = notes,

                    });
                 //   EntityPOCO.PreviousLocalName = entityPOCO.LocalName;
                  //  EntityPOCO.PreviousLocalNameChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                GLAccountRepository glAccountRepo = new GLAccountRepository(entityPM.Tenant);

                if (entityPM.ParentAccountId != entityPOCO.ParentAccountId && (!String.IsNullOrEmpty(entityPM.ParentAccountId) || !String.IsNullOrEmpty(entityPOCO.ParentAccountId)))
                {
                    String notes = "";
                    GLAccount oldParent = glAccountRepo.GetSingle(entityPOCO.ParentAccountId, entityPM.Tenant);
                  

                    if (entityPM.ParentAccountId == null)
                    {
                        //parent deleted
                        notes = "Parent deleted: " + oldParent.LocalName;

                    }
                    else
                    {
                        GLAccount newParent = glAccountRepo.GetSingle(entityPM.ParentAccountId, entityPM.Tenant);
                        string oldName = null;
                        string newName = null;
                        if(oldParent != null)
                        {
                            if (oldParent.EnglishName == null)
                            {
                                oldName = oldParent.LocalName;
                            }
                            else oldName = oldParent.EnglishName;



                        }
                        else
                        {
                            oldName = null;
                        }


                        if (newParent != null)
                        {
                            if (newParent.EnglishName == null)
                            {
                                newName = newParent.LocalName;
                            }

                            else newName = newParent.EnglishName;
                        }

                        else { newName = null; }
                        notes = (oldParent == null ? "New Value: " : (TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldName) + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0)) + newName;

                    }





                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "GLPC",
                        Notes = notes,

                    });
                }
                if (entityPM.Inactive != entityPOCO.Inactive && entityPM.Inactive == true)
                {
                  
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    //String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "BLK",
                       

                    });
                }
                if (entityPM.Inactive != entityPOCO.Inactive && entityPM.Inactive == false)
                {
                  
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    //String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "UBLK",
                        
                    });

                }
                if (entityPM.ChartOfAccountsId != entityPOCO.ChartOfAccountsId && (!String.IsNullOrEmpty(entityPM.ChartOfAccountsId) || !String.IsNullOrEmpty(entityPOCO.ChartOfAccountsId)))
                {
                  
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
                    ChartOfAccountQueryService query = new ChartOfAccountQueryService(accountingContext);
                    ChartOfAccountPM oldChart = query.GetSingle(entityPOCO.ChartOfAccountsId, false, true);
                    ChartOfAccountPM newChart = query.GetSingle(entityPM.ChartOfAccountsId, false, true);
                    string oldName = null;
                    string newName = null;
                    if (oldChart != null)
                    {
                        if (oldChart.EnglishName == null)
                        {
                            oldName = oldChart.LocalName;
                        }
                        else oldName = oldChart.EnglishName;



                    }
                    else
                    {
                        oldName = null;
                    }


                    if (newChart != null)
                    {
                        if (newChart.EnglishName == null)
                        {
                            newName = newChart.LocalName;
                        }

                        else newName = newChart.EnglishName;
                    }

                    else { newName = null; }
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + newName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CHCH",
                        Notes = notes,

                    });

                    //     EntityPOCO.PreviousChartOfAccountsId = entityPOCO.ChartOfAccountsId;
                    //     EntityPOCO.PreviousChartOfAccountsChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }


                if (entityPM.ChartOfAccountsTypeCode != entityPOCO.ChartOfAccountsTypeCode && (!String.IsNullOrEmpty(entityPM.ChartOfAccountsTypeCode) || !String.IsNullOrEmpty(entityPOCO.ChartOfAccountsTypeCode)))
                {
                   
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
                    ChartOfAccountsTypeQueryService query = new ChartOfAccountsTypeQueryService(accountingContext);
                    ChartOfAccountsTypePM oldChart = query.GetSingle(entityPOCO.ChartOfAccountsTypeCode, false, true);
                    ChartOfAccountsTypePM newChart = query.GetSingle(entityPM.ChartOfAccountsTypeCode, false, true);
                    string oldName = null;
                    string newName = null;
                    if (oldChart != null)
                    {
                        if (oldChart.EnglishName == null)
                        {
                            oldName = oldChart.LocalName;
                        }
                        else oldName = oldChart.EnglishName;



                    }
                    else
                    {
                        oldName = null;
                    }


                    if (newChart != null)
                    {
                        if (newChart.EnglishName == null)
                        {
                            newName = newChart.LocalName;
                        }

                        else newName = newChart.EnglishName;
                    }

                    else { newName = null; }
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + newName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CTCH",
                        Notes = notes,

                    });
                }

                if (entityPM.InternalNumber != entityPOCO.InternalNumber && (!String.IsNullOrEmpty(entityPM.InternalNumber) || !String.IsNullOrEmpty(entityPOCO.InternalNumber)))
                {
                 
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.InternalNumber + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.InternalNumber;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "UPEV",
                        Notes = notes,

                    });
                }
                if(entityPM.Type == "ADDED")
                {

                    string s = TranslateTextsClass.Translate("Accounting.General.O.Added", entityPM.Tenant,true);
                    string[] text = s.Split('-');
                 

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.CustomerGLAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "ADD",
                        Notes = text[0] +"- " + entityPM.DisplayNumber + " "+ text[1],

                    });
                }
                if (entityPM.Type == "CHILD")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.ChildAdded", entityPM.Tenant,true);
                    string[] text = s.Split('-');

               

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.ParentAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CHID",
                        Notes = text[0] +"- " + entityPM.DisplayNumber + " "+text[1],

                    });
                }

                if (entityPM.Type == "INACTIVE")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.Deactivated", entityPM.Tenant,true);
                    string[] text = s.Split('-');

               

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.CustomerGLAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "INGL",
                        Notes = text[0] + "- " + entityPM.DisplayNumber + " " + text[1],

                    });
                }

                if (entityPM.Type == "ACTIVE")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.Activated", entityPM.Tenant, true);
                    string[] text = s.Split('-');

                 

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.CustomerGLAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "SGAC",
                        Notes = text[0] + "- " + entityPM.DisplayNumber + " " + text[1],

                    });
                }
                if (entityPM.Type != null)
                {
                    if (entityPM.Type.Contains("DISC"))
                    {
                        string xs = TranslateTextsClass.Translate("Accounting.General.O.Disconnected", entityPM.Tenant, true);
                        string[] text = xs.Split('-');

                        string[] s = entityPM.Type.Split(',');
                      

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            EntityId = s[1],
                            Tenant = entityPM.Tenant,
                            UserId = contact.Id,
                            ObjectTableName = "GLAccount",
                            IsAddedManually = false,
                            EventTypeCode = "DISC",
                            Notes = text[0] + "- " + entityPM.DisplayNumber + " " + text[1],

                        });

                    }
                }
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }


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


        protected override void Validate(GLAccountPM entityPM)
        {
            ValidationResult result = GLAccountValidator.IsGLAccountValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        private void UpdateCardGLAccountId(int tenant, string cardId, string glAccountId)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                CardQuery query = new CardQuery(tenant);
                CardService service = new CardService(CommonDataContext.GetContext(tenant), tenant);
                CardPM card = query.GetSinglePM(cardId, tenant);
                card.GLAccountId = glAccountId;
                service.Update(card);
            }
        }

        protected override void AfterUpdating(GLAccountPM entityPM, EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);
            // Update Card GLAccountId [Maheera]
            UpdateCardGLAccountId(entityPM.Tenant, entityPM.NewGLAccountCardId, entityPM.Id);
        }

        private void ValidateCurrency(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPOCO.Tenant);
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(MyContext);
            ContactPM currenctUser = GetLoggedContact(entityPM.Tenant);
            bool useLocal = true;
            if(currenctUser != null)
                useLocal= !currenctUser.DontShowLocal;

            // CASES: currency changed
            // 1- from single to multi
            // 2- from multi to single
            // 3- from single to single
            //

            // [1] from single to multi
            if (entityPOCO.IsMultiCurrency == false && entityPM.IsMultiCurrency == true)
            {
                // Allow to change 
            }

            // [2] from multi to single
            else if(entityPOCO.IsMultiCurrency == true && entityPM.IsMultiCurrency == false)
            {
                List<LedgerTransactionList> openTransactions = transactionListQuery.GetOpenByAccountId(entityPM.Id, entityPM.Tenant);

                if(openTransactions.Count > 0)
                {
                    //check currency, if all transaction have same currency, continu, otherwise throw error
                    bool isOneCurrency = false;

                    var groupedByCurrencyTrans = (from trans in openTransactions
                                                  group trans by trans.CurrencyId into g
                                                  select new { CurrencyId = g.Key, Res = g.ToList() });

                    isOneCurrency = groupedByCurrencyTrans.Count() == 1;

                    if (isOneCurrency)
                    {
                        // check if the transactions currency same as entered account currency
                        var transactionsCurrencyId = groupedByCurrencyTrans.First().CurrencyId;

                        if (transactionsCurrencyId != entityPM.CurrencyId)
                        {
                            // throw error 
                            // the transactions currency does not equal entered account currency 
                            throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.ThereOpenTransaction", 0, useLocal));
                        }
                        else
                        {
                            //same currency, continue

                        }
                    }
                    else
                    {
                        // throw error
                        // not all with same currency
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.ThereOpenTransaction", 0, useLocal));
                    }
                }
                else
                {
                    // no transactions
                    // so we can continue
                }

            }

            // [3] from single to single
            else if( (entityPOCO.IsMultiCurrency == false && entityPM.IsMultiCurrency == false) && entityPOCO.CurrencyId != entityPM.CurrencyId)
            {
                List<LedgerTransactionList> openTransactions = transactionListQuery.GetOpenByAccountId(entityPM.Id, entityPM.Tenant);

                // check if the currenct account transaction have only one currency or more?
                bool isOneCurrency = false;
                
                var groupedByCurrencyTrans = (from trans in openTransactions
                           group trans by trans.CurrencyId into g
                           select new { CurrencyId = g.Key, Res = g.ToList() });

                isOneCurrency = groupedByCurrencyTrans.Count() == 1;

                if (isOneCurrency)
                {
                    // check if the transactions currency same as entered account currency
                    
                    var transactionsCurrencyId = groupedByCurrencyTrans.First().CurrencyId;

                    if(transactionsCurrencyId != entityPM.CurrencyId)
                    {
                        // throw error
                        // the transactions currency does not equal entered account currency 
                        throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.ThereTransactions4GLAwithexistingCurrency", 0, useLocal));
                    }
                    else
                    {
                        //same currency, continue

                    }

                }
                else
                {
                    // [!] this case shouldn't be entered, why? because the account now is single currency, so can't add different currencies transactions
                    //
                    // throw error
                    // not all transaction have same currency, the transactions have different currencies
                    throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.ThereOpenTransaction", 0, useLocal));
                }


            }

            // splitted glaccount validation
            if (entityPOCO.IsMultiCurrency == true && entityPM.IsMultiCurrency == false)
            {
                IAccountingContext ccc = AccountingContext.GetContext(entityPM.Tenant);
                GLAccountCurrencyListQueryService GLAccountCurrencyQuery = new GLAccountCurrencyListQueryService(ccc);
                GLAccountCurrencyList glAccountCurrencyList = GLAccountCurrencyQuery.GetByAccountNumber(entityPM.Id, entityPM.Tenant);
                if(glAccountCurrencyList != null)
                {
                    throw new ApplicationException("This GLAccount have splitted GLAccounts by currency, Deactivate these GLAccounts before doing these action");
                }
            }


        }

    }


    public class GLAccountUpdateServiceBalancePriv:GLAccountUpdateService
    {
        private decimal? _deltaBalanceInLocalCurrency;
        private decimal? _deltaLocalBalanceInDue;
        private DateTime? _nextDueDate;
        public GLAccountUpdateServiceBalancePriv(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            :base(mainContext,additionalContexts, tenant)
        {

        }
        public void Init(decimal? deltaBalanceInLocalCurrency, decimal? deltaLocalBalanceInDue, DateTime? nextDueDate)
        {
            _deltaBalanceInLocalCurrency = deltaBalanceInLocalCurrency;
            _deltaLocalBalanceInDue = deltaLocalBalanceInDue;
            _nextDueDate = nextDueDate;
        }
        protected override void OnUpdatingCheckBalance(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            var messnoprivtochangeBalanceInLocalCurrency = "no priv to change BalanceInLocalCurrency";
            if (entityPOCO == null)
            {
                return;
            }
            decimal? delta=null;
#if GLAccMoreData
            var delta = entityPM.BalanceInLocalCurrency.GetValueOrDefault() - entityPOCO.BalanceInLocalCurrency.GetValueOrDefault();
            if (delta != _deltaBalanceInLocalCurrency.GetValueOrDefault())
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
            delta = entityPM.LocalBalanceInDue.GetValueOrDefault() - entityPOCO.LocalBalanceInDue.GetValueOrDefault();

            if (delta != _deltaLocalBalanceInDue.GetValueOrDefault())
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
            if (entityPM.NextDueDate.GetValueOrDefault() != _nextDueDate.GetValueOrDefault())
            {
                throw new Exception(messnoprivtochangeBalanceInLocalCurrency);
            }
#endif
            ///base.OnUpdatingCheckBalance(entityPM, entityPOCO);
        }
        public override void AddAcitivityLog(GLAccountPM entityPM, string activityTypeCode)
        {
            //while Journal Aprove Update Balance- no need to  write AddAcitivityLog !!
            //base.AddAcitivityLog(entityPM, activityTypeCode);
        }
    }
}
