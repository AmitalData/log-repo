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
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Reflection;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.BL.Utils;
using System.Xml.Serialization;
using System.IO;
using Logitude.Accounting.BL.CoreBL.Batch;

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
        protected override void OnCreating(GLAccountPM entityPM, EntityPM entityParentPM)
        {
            if (!string.IsNullOrWhiteSpace(EntityPM.ExternalDisplayNumber))
            {
                    entityPM.DisplayNumber = EntityPM.ExternalDisplayNumber;
            }
            else
            {
                if (entityPM.ChartOfAccountsTypeCode != "3" && entityPM.ChartOfAccountsTypeCode != "4" && entityPM.ChartOfAccountsTypeCode != "6")
                    SetDisplayNumber(entityPM);

            }
            AddAcitivityLog(entityPM, "N");

            FillSearchFields(entityPM);


            ContactPM loggedUser = GetLoggedContact(entityPM.Tenant);
            entityPM.CreatedByUserId = loggedUser?.Id;

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsId) && (entityPM.ChartOfAccountsId.ToLower() == "bla" || entityPM.ChartOfAccountsId.ToLower() == "get")) entityPM.ChartOfAccountsId = null;
            if (!String.IsNullOrWhiteSpace(entityPM.ChartOfAccountsTypeCode) && (entityPM.ChartOfAccountsTypeCode.ToLower() == "bla" || entityPM.ChartOfAccountsTypeCode.ToLower() == "get")) entityPM.ChartOfAccountsTypeCode = null;
            if (!String.IsNullOrWhiteSpace(entityPM.DeductionTypeId) && (entityPM.DeductionTypeId.ToLower() == "bla" || entityPM.DeductionTypeId.ToLower() == "get")) entityPM.DeductionTypeId = null;
            if (!String.IsNullOrWhiteSpace(entityPM.AssessingOfficeCode) && (entityPM.AssessingOfficeCode.ToLower() == "bla" || entityPM.AssessingOfficeCode.ToLower() == "get")) entityPM.AssessingOfficeCode = null;
            if (!String.IsNullOrWhiteSpace(entityPM.DeductionFileTypeId) && (entityPM.DeductionFileTypeId.ToLower() == "bla" || entityPM.DeductionFileTypeId.ToLower() == "get")) entityPM.DeductionFileTypeId = null;


            if (entityPM.AccountTypeCode == "4") // Job
            {
                // first get application
                if (entityPM.Application == "F" || entityPM.Application == "J") entityPM.Application = "";
                string application = entityPM.Application;
                if (String.IsNullOrWhiteSpace(application))
                {
                    if (!String.IsNullOrWhiteSpace(entityPM.InternalNumber) && entityPM.InternalNumber.Length > 4 && entityPM.InternalNumber.Substring(0, 3) == "SPD")
                    {
                        if (entityPM.InternalNumber.Substring(3, 1) == "I")
                        {
                            application = entityPM.InternalNumber.Substring(3, 2);
                        }
                        else
                        {
                            application = entityPM.InternalNumber.Substring(3, 1);
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(entityPM.InternalNumber) && entityPM.InternalNumber.Length > 5 && entityPM.InternalNumber.Substring(0, 2) == "A-" && entityPM.InternalNumber.Substring(3, 2) == "SP")
                    {
                        if (entityPM.InternalNumber.Substring(0, 5) == "A-ISP")
                        {
                            application = "I" + entityPM.InternalNumber.Substring(5, 1); //A-ISPO00001234 
                        }
                        else
                        {
                            application = entityPM.InternalNumber.Substring(2, 1); //A-MSP1234
                        }
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
                            case "IA":
                                jobControlAccountId = fullAccountingSetting.AirImportJobControlAccountId;
                                break;
                            case "R":
                            case "IO":
                            case "IL":
                            case "II":
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
                GLAccountPM acc = query.GetByInternalNumber(entityPM.CustomerGLAccountInternalNumber, entityPM.Tenant)/*.FirstOrDefault<GLAccountPM>()*/;
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
                GLAccountPM acc = query.GetByInternalNumber(entityPM.InternalNumber, entityPM.Tenant)/*.FirstOrDefault<GLAccountPM>()*/;
                if (acc != null)
                {
                    throw new ApplicationException("Existing GLAccount found with Internal No. " + entityPM.InternalNumber + " (Display No. " + acc.DisplayNumber + ")");
                }
            }

            if (entityPM.InternalNumber == null || entityPM.InternalNumber == "") entityPM.InternalNumber = /*CodeCounter*/(new CodeCounterWrapper(true)).GetNumber("GLAccount", entityPM.Tenant).ToString();
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounter.GetNumber("GLAccount", entityPM.Tenant);
            if (entityPM.ReconcileMethodCode == null || entityPM.ReconcileMethodCode == "") entityPM.ReconcileMethodCode = "0";
            GLAccountCurrencyPM gLAccountCurrency = null;
            if (!String.IsNullOrWhiteSpace(entityPM.ParentAccountByCurrency))
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPM.Tenant);
                GLAccountPM acc = query.GetByInternalNumber(entityPM.ParentAccountByCurrency, entityPM.Tenant)/*.FirstOrDefault<GLAccountPM>()*/;
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
                    throw new ApplicationException("Parent Account by Currency " + entityPM.ParentAccountByCurrency + " not found");
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

        private void SetDisplayNumber(GLAccountPM entityPM)
        {
            GLAccountCounterService gLAccountCounterService = new GLAccountCounterService(entityPM.Tenant);
            string _displayNumber = gLAccountCounterService.GetNewDisplayNumber(entityPM);

            //check exist
            GLAccountQueryService gLAccountQuery = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM gla = gLAccountQuery.GetByDisplayNumber(_displayNumber, entityPM.Tenant).FirstOrDefault();
            if (gla == null)
            {
                entityPM.DisplayNumber = _displayNumber;
            }
            else
            {
                //skip this counter, get next
                SetDisplayNumber(entityPM);
            }
        }

 
        private void setAccountingTypeCodeByChartofAccountTypeCode(GLAccountPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.AccountTypeCode))
            {
                switch (entityPM.ChartOfAccountsTypeCode)
                {
                    case "3":
                        {
                            entityPM.AccountTypeCode = "2";
                            break;
                        }
                    case "4":
                        {
                            entityPM.AccountTypeCode = "3";
                            break;
                        }
                    default:
                        {
                            entityPM.AccountTypeCode = "1";
                            break;
                        }
                }
            }
        }

        private void CheckIfGLAccountIsControlGLAccount(GLAccountPM entityPM) {
            // FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(entityPM.Tenant);
            FullAccountingSettingPM fullAccountingSetting = FullAccountingSettingQueryService.Get(entityPM.Tenant);
            if (!string.IsNullOrWhiteSpace(entityPM.ParentAccountId) && 
                (fullAccountingSetting.CustomerControlAccountId == entityPM.Id ||
                fullAccountingSetting.VendorControlAccountId == entityPM.Id ||
                fullAccountingSetting.FileControlAccountId == entityPM.Id ||
                fullAccountingSetting.OceanExportJobControlAccountId == entityPM.Id ||
                fullAccountingSetting.OceanImportJobControlAccountId == entityPM.Id ||
                fullAccountingSetting.AirExportJobControlAccountId == entityPM.Id ||
                fullAccountingSetting.AirImportJobControlAccountId == entityPM.Id
                )) {
                throw new ApplicationException("Can't set parent account for control accounts");
            }

        }
        protected override void OnUpdating(GLAccountPM entityPM, GLAccount entityPOCO)
        {

            if (!entityPM.IsControlAccount.GetValueOrDefault())
            {
                this.setAccountingTypeCodeByChartofAccountTypeCode(entityPM);
            }

            CheckIfGLAccountIsControlGLAccount(entityPM);
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            if (entityPM.GLAccountInterestPeriods.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete && s.PeriodStartDate !=null).GroupBy(x => x.PeriodStartDate).Any(g => g.Count() > 1))
            {

                throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.LineDateExist", entityPM.Tenant, showLocals));

            }
            if (entityPM.ActiveForInterest == true)
            {

                GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(entityPM.Tenant);
                GLAccountCurrency gLAccountCurrency = gLAccountCurrencyQueryService.GetGLAccountCurrencyByGLAccountId(entityPM.Id, entityPM.Tenant);
                if (gLAccountCurrency == null)
                {
                    if (entityPM.InterestCalculationStartDate == null)
                    {
                        throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.FieldInterestCalculationStartDateismandatory", entityPM.Tenant, showLocals));
                    }
                    else
                    {
                        bool IsNotDeletde = false;
                        foreach (GLAccountInterestPeriodPM periodPM in entityPM.GLAccountInterestPeriods)
                        {
                            if (periodPM.ChangeSetOp != ChangeSetOperation.Delete)
                            {
                                IsNotDeletde = true;
                                var item = entityPM.GLAccountInterestPeriods.Where(d => d.PeriodStartDate <= entityPM.InterestCalculationStartDate).FirstOrDefault();
                                if (item == null)
                                {

                                    throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.InterestCalculationStartDateValidation", entityPM.Tenant, showLocals) + " (" + String.Format("{0:dd.MM.yy}", entityPM.InterestCalculationStartDate.Value.Date) + ")");

                                }
                            }
                            else
                            {
                                GLAccountInterestPeriodPM item = entityPM.GLAccountInterestPeriods.Where(d => d.PeriodStartDate <= entityPM.InterestCalculationStartDate && d.LineNumber != periodPM.LineNumber ).FirstOrDefault();                          
                                if (item == null )
                                {
                                    throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.InterestCalculationStartDateValidation", entityPM.Tenant, showLocals) + " (" + String.Format("{0:dd.MM.yy}", entityPM.InterestCalculationStartDate.Value.Date) + ")");

                                }
                            }
                           

                        }
                        if (!IsNotDeletde)
                        {
                            throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.AtleastoneGLAccountInterestPeriodsrecordisrequired", entityPM.Tenant, showLocals));

                        }

                    }
                }
            }
            if (entityPM.ActiveForInterest == false)
            {
                for (int i = 0; i < entityPM.GLAccountInterestPeriods.Count; i++)
                {
                    if (entityPM.GLAccountInterestPeriods[i].ChangeSetOp != ChangeSetOperation.Delete)
                    {
                        throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccount.O.DeleteExistInterestperiods", entityPM.Tenant, showLocals));
                    }

                }
            }


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
            //if (entityPOCO.ChartOfAccountsId != entityPM.ChartOfAccountsId)
            //{
            //    // ChartOfAccount changed
            //    // get transactions in closed period
            //    LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPOCO.Tenant);
            //    AccountingPeriodQueryService periodQuery = new AccountingPeriodQueryService(entityPOCO.Tenant);
            //    List<AccountingPeriodPM> tenantPeriods = periodQuery.GetAccountingPeriodsByTenantAndType("1", entityPOCO.Tenant); // 1- Accounting, Regular
            //    if (tenantPeriods.Count() > 0)
            //    {
            //        //
            //        // note: the opened periods may be found in a different years, 
            //        //       so I fetch the opened periods over years and check its closed transactions
            //        //

            //        DateTime closedDate;
            //        DateTime openDate;

            //        //List<AccountingPeriodPM> periodsWithOpenedMonths = tenantPeriods.Where(d => d.ClosedMonth != d.OpenMonth).ToList();
            //        foreach (AccountingPeriodPM period in tenantPeriods)
            //        {
            //            // prepare closed month date
            //            if (period.ClosedMonth == null)
            //                closedDate = new DateTime(period.Year, 1, 1, 0, 0, 0);
            //            else
            //                closedDate = new DateTime(period.Year, period.ClosedMonth.Value, DateTime.DaysInMonth(period.Year, period.ClosedMonth.Value), 23, 59, 59);

            //            // prepare open month date
            //            openDate = new DateTime(period.Year, period.OpenMonth, 1, 0, 0, 0);

            //            // get transactions in closed period
            //            IQueryable<LedgerTransaction> transactions = transQuery.GetClosedPeriodTransactions(entityPOCO.Id, closedDate, openDate, entityPOCO.Tenant);
            //            if (transactions.Count() > 0)
            //            {
            //                throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccounts.O.ChartOfAccountCantChangedGLAhaveTrans", entityPOCO.Tenant, useLocal));
            //            }
            //        }

            //    }

            //}


            if (entityPOCO.ChartOfAccountsTypeCode != entityPM.ChartOfAccountsTypeCode)
            {
                List<LedgerTransactionList> transactions = GetAccountTransactions(entityPM);
                if (transactions.Count > 0)
                {
                    throw new ApplicationException("Can't change chart of account type while the account has a transactions");
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

            ValidateCurrencyChange(entityPM, entityPOCO);

            if (entityPM.ActiveForInterest != entityPOCO.ActiveForInterest && entityPM.ActiveForInterest == true)
            {
                var args = new GLAccountInterestActivationBalanceArgs()
                {
                    Tenant = entityPOCO.Tenant,
                    GLAccountId = entityPOCO.Id,
                    AccountTypeCode = null,
                    InterestActivationDate = entityPM.InterestCalculationStartDate.HasValue ? entityPM.InterestCalculationStartDate.Value : DateTime.MinValue,
                    LastMadeGLAccountId = null,
                    MaxGLAccountsPerQuery = 100,
                    ActionDate = DateTime.Today,
                    BatchIt = 1,
                };
                using (var memStream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(GLAccountInterestActivationBalanceArgs));
                    serializer.Serialize(/*stringwriter*/memStream, args);

                    var communicationLogId = Communications.AddCommunicationLog(new CommunicationsParams()
                    {
                        Tenant = entityPOCO.Tenant,
                        CommunicationLogTypeCode = "Q",
                        QueueName = "externaltasksqueue" + entityPOCO.Tenant + 1,
                        Priority = 1,
                        InOut = "O",
                        Status = "D",
                        FileExtension = "xml",
                        //LoggingUserId = loggedUserId,
                        //LoggingObjectTableId = table.Id,
                        //LoggingEntityId = extDocPM.Id,

                        FolderName = "BatchTaskExecutionsQueue",

                        To = "GLAccountInterestActivationBalanceBatch",

                        //EntityId = declarationId,
                        //ObjectTableId = objectTableId,
                        Subject = "GLAccountInterestActivationBalanceBatch holder ",
                        ByteData = memStream.ToArray()


                    });
                    args.CommunicationLogId = communicationLogId;

                }
                //GLAccountInterestActivationBalanceBatch.CreateBatchFunctionalTestTask( args,false);
                var myGLAccountInterestActivationBalanceBatch = new BatchGLAccountInterestActivationBalanceTask(null);
                string subj = $"GLAccount Interest Activation Balance {entityPOCO.Id}";
                myGLAccountInterestActivationBalanceBatch.CreateQBatchTaskExecution<GLAccountInterestActivationBalanceArgs>(args, args.Tenant, subj, true);


            }

        }

        protected override void OnUpdating(GLAccountPM entityPM)
        {
            AddAcitivityLog(entityPM, "U");

            FillForeignFields(entityPM);
            FillSearchFields(entityPM);
            AddEventForGlAccountFollowUpData(entityPM);        
            HandleGLAccountFollowUpData(entityPM);

        }
        private void HandleGLAccountFollowUpData(GLAccountPM entityPM)
        {
            ContactPM loggedUser = GetLoggedContact(entityPM.Tenant);
            GLAccountFollowUpDataPM gLAccountFollowUpData = GetGLAccountFollowUpDataPM(entityPM);
            GLAccountFollowUpDataUpdateService gLAccountFollowUpDataUpdateService = new GLAccountFollowUpDataUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            if (gLAccountFollowUpData != null)
            {
                gLAccountFollowUpData= UpdateGLAccountFollowUpData(entityPM, gLAccountFollowUpData, loggedUser);
            }
            else
            {
                if(entityPM.GLAccountFollowUpRemarks != null || entityPM.GLAccountFollowUpDate!= null
                    || entityPM.FollowupNotes != null || entityPM.FollowupDate != null)
                gLAccountFollowUpData= CreateGLAccountFollowUpData(entityPM, loggedUser);
            }
          if( gLAccountFollowUpData != null)
                gLAccountFollowUpDataUpdateService.Update(gLAccountFollowUpData,true);
        }
        private GLAccountFollowUpDataPM CreateGLAccountFollowUpData(GLAccountPM accountPM,ContactPM loggedUser)
        {         
            GLAccountFollowUpDataPM gLAccountFollowUpData = new GLAccountFollowUpDataPM();
             gLAccountFollowUpData= MapGLAccountFollowUpFields(gLAccountFollowUpData, accountPM, loggedUser);
            gLAccountFollowUpData.ChangeSetOp = ChangeSetOperation.Insert;
            return gLAccountFollowUpData;
        }
        private GLAccountFollowUpDataPM MapGLAccountFollowUpFields(GLAccountFollowUpDataPM gLAccountFollowUpData, GLAccountPM accountPM, ContactPM loggedUser)
        {
            if ((accountPM.AccountTypeCode == GLAccountTypes.Client || accountPM.AccountTypeCode == GLAccountTypes.Vendor) && accountPM.GLAccountFollowUpRemarks != null || accountPM.GLAccountFollowUpDate != null)
            {
                gLAccountFollowUpData.FollowUpDate = accountPM.GLAccountFollowUpDate;
                gLAccountFollowUpData.FollowUpRemarks = accountPM.GLAccountFollowUpRemarks;
            }
            else if ((accountPM.AccountTypeCode == GLAccountTypes.Card || accountPM.AccountTypeCode == GLAccountTypes.Vendor) && accountPM.FollowupNotes != null || accountPM.FollowupDate != null)
            {
                gLAccountFollowUpData.FollowUpDate = accountPM.FollowupDate;
                gLAccountFollowUpData.FollowUpRemarks = accountPM.FollowupNotes;
            }
            else {
                gLAccountFollowUpData.FollowUpDate = null;
                gLAccountFollowUpData.FollowUpRemarks = null;
            }
                          
            gLAccountFollowUpData.UpdatedByUserId = loggedUser?.Id;
            gLAccountFollowUpData.Tenant = accountPM.Tenant;
            gLAccountFollowUpData.GlAccountId = accountPM.Id;
            gLAccountFollowUpData.UpdateDate = TenantServerConfigration.GetCurrentDateTime(accountPM.Tenant);
            return gLAccountFollowUpData;
        }
        private GLAccountFollowUpDataPM UpdateGLAccountFollowUpData(GLAccountPM accountPM, GLAccountFollowUpDataPM gLAccountFollowUpData, ContactPM loggedUser)
        {
          gLAccountFollowUpData=  MapGLAccountFollowUpFields(gLAccountFollowUpData, accountPM, loggedUser);
            gLAccountFollowUpData.ChangeSetOp = ChangeSetOperation.Update;
            return gLAccountFollowUpData;
        }
        private GLAccountFollowUpDataPM GetGLAccountFollowUpDataPM(GLAccountPM entityPM)
        {
            GLAccountFollowUpDataQueryService accountFollowUpDataQueryService = new GLAccountFollowUpDataQueryService(entityPM.Tenant);
            return accountFollowUpDataQueryService.GetSinglePMByAccountId(entityPM.Id, entityPM.Tenant);
        }
        private TenantPM GetTenantPM(int tenantId)
        {
            TenantQuery tenantQuery = new TenantQuery(tenantId);
            return tenantQuery.GetTenantFromDB(tenantId);
        }
        public void SendHybridTask(GLAccountPM glaccounPM)
        {

            TenantPM tenantPM = GetTenantPM(glaccounPM.Tenant);
            if (tenantPM.IsHybrid && glaccounPM.AccountTypeCode != "4" && glaccounPM.AccountTypeCode != "5" && glaccounPM.IsControlAccount == false)

            {
                FillGLAccountCurrencyCode(glaccounPM);

                CommunicationsParams comParams = CreateCommunicationParamsForGLAccount(glaccounPM);

                List<QueueTask> queueTasks = CreateQueueTasks(glaccounPM);

                comParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);

                Communications.AddCommunicationLog(comParams);
            }

        }

        private static void FillGLAccountCurrencyCode(GLAccountPM glaccounPM)
        {
            if (glaccounPM.CurrencyId != null)
            {
                CurrencyQuery currencyQuery = new CurrencyQuery(glaccounPM.Tenant);
                CurrencyPM currency = currencyQuery.GetSinglePM(glaccounPM.CurrencyId, glaccounPM.Tenant);
                glaccounPM.CurrencyCode = currency.Code;
                glaccounPM.CurrencySign = currency.Sign;
            }
        }

        private List<QueueTask> CreateQueueTasks(GLAccountPM glaccounPM)
        {
            APIDataContract.ApiV1.GLAccount gLAccount = GetMappedGLAccountDataContract(glaccounPM);

            string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(gLAccount);

            List<QueueTask> queue1Tasks = new List<QueueTask>
                {
                    new QueueTask()
                    {
                        Action = "GLAccount.Upsert",
                        Parameters = new List<Parameter>()
                        {
                            new Parameter{ Name = "GLAccountData", Order = 1, Value = xmlstring }
                        }
                    }
                };
            return queue1Tasks;
        }
        private APIDataContract.ApiV1.GLAccount GetMappedGLAccountDataContract(GLAccountPM gLAccountPM)
        {
            APIDataContract.ApiV1.GLAccountQueryService gLAccountQueryService = new APIDataContract.ApiV1.GLAccountQueryService(gLAccountPM.Tenant);
            APIDataContract.ApiV1.GLAccount glAccount = gLAccountQueryService.GLAccountDataMapping(gLAccountPM, gLAccountPM.Tenant);

            glAccount = MapGLAccountCardFields(glAccount);
            glAccount.CardCode = null;
            glAccount.PartnerTypeId = null;

            return glAccount;
        }
        private APIDataContract.ApiV1.GLAccount MapGLAccountCardFields(APIDataContract.ApiV1.GLAccount gLAccount)
        {
            List<CardList> cardLists = GetCardsByGLAccountId(gLAccount.Id, gLAccount.Tenant);

            List<Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Card> cards = new List<Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Card>();



            if (gLAccount.PartnerTypeId != null)
            {
                cardLists = cardLists.Where(d => d.PartnerTypeId == gLAccount.PartnerTypeId).ToList();
            }
            foreach (CardList card in cardLists)

            {
                Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Card connectedCard = new Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Card();
                connectedCard.Code = card.Code;
                connectedCard.PartnerCode = card.PartnerTypeId;
                if (gLAccount.CardCode == connectedCard.Code)
                {
                    connectedCard.IsDisconnectedFromGLAccount = true;
                }
                else { connectedCard.IsDisconnectedFromGLAccount = false; }
                cards.Add(connectedCard);

            }
            gLAccount.Cards = cards;
            return gLAccount;

        }
        private List<CardList> GetCardsByGLAccountId(string id, int tenant)
        {

            CardQuery cardQuery = new CardQuery(tenant);
            return cardQuery.GetCardPMsByGLAccountId(id, tenant);

        }
        private CommunicationsParams CreateCommunicationParamsForGLAccount(GLAccountPM glaccounPM)
        {
            int tenant = glaccounPM.Tenant;
            ContactPM loggedUser = GetLoggedContact(tenant);
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode("GLAccount", 0);




            CommunicationsParams comParams = new CommunicationsParams()
            {
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = loggedUser?.Id,
                LoggingObjectTableId = table?.Id,
                LoggingEntityId = glaccounPM.Id,
                Subject = "GLAccount Updated",
                FolderName = "ExternalTasksQueue",
            };
            return comParams;
        }

        private void FillSearchFields(GLAccountPM entityPM)
        {
            entityPM.SearchFields = entityPM.DisplayNumber + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        private void FillForeignFields(GLAccountPM entityPM)
        {
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
                GLAccountPM acc = query.GetByInternalNumber(entityPM.CustomerGLAccountInternalNumber, entityPM.Tenant)/*.FirstOrDefault<GLAccountPM>()*/;
                if (acc != null)
                {
                    entityPM.CustomerGLAccountId = acc.Id;
                }
            }
        }

        protected override void AddContext(GLAccountPM myTEntityPM)
        {
            base.AddContext(myTEntityPM);
            SetPriv();
        }
        protected override void UpdateComposition(GLAccountPM entityPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                InsertGLAccountMoreData(entityPM);

                InsertGLAccountRecocileData(entityPM);

                InsertGLAccountAgingData(entityPM);

            }

            var gLAccountWithholdingTaxUpdateService = new GLAccountWithholdingTaxUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            gLAccountWithholdingTaxUpdateService.UpdateMulti(entityPM.GLAccountWithholdingTaxes, entityPM.DeletedGLAccountWithholdingTaxes, entityPM, true);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && entityPM.GLAccountInterestPeriods.Count > 0)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                ContactPM contactLocal = GetLoggedContact(entityPM.Tenant);
                bool showLocals = !contactLocal.DontShowLocal;
                foreach (var line in entityPM.GLAccountInterestPeriods)
                {

                    if (line.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        GLAccountInterestPeriodQueryService gLAccountInterestPeriodQueryService = new GLAccountInterestPeriodQueryService(line.Tenant);
                        GLAccountInterestPeriodPM gLAccountInterestPeriodPM = gLAccountInterestPeriodQueryService.GetSingle(line.LineNumber, line.GLAccountId, false, false);

                        if (line.PeriodStartDate != gLAccountInterestPeriodPM.PeriodStartDate || line.StandardInterestRateBaseId != gLAccountInterestPeriodPM.StandardInterestRateBaseId || line.StandardAddInterestPercent != gLAccountInterestPeriodPM.StandardAddInterestPercent || line.ExceptionalInterestRateBaseId != gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId || line.ExceptionalAddInterestPercent != gLAccountInterestPeriodPM.ExceptionalAddInterestPercent || line.CreditInterestRateBaseId != gLAccountInterestPeriodPM.CreditInterestRateBaseId || line.CreditAddInterestPercent != gLAccountInterestPeriodPM.CreditAddInterestPercent)
                        {

                            string notes = "";
                            if (line.PeriodStartDate != gLAccountInterestPeriodPM.PeriodStartDate)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.PeriodStartDate", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.PeriodStartDate.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.PeriodStartDate.ToString() + "\n";
                            }
                            if (line.StandardInterestRateBaseId != gLAccountInterestPeriodPM.StandardInterestRateBaseId)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.StandardInterestRateBaseId", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.StandardInterestRateBaseName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.StandardInterestRateBaseName  + "\n";
                            }
                            if (line.StandardAddInterestPercent != gLAccountInterestPeriodPM.StandardAddInterestPercent)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.StandardAddInterestPercent", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.StandardAddInterestPercent.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.StandardAddInterestPercent.ToString() + "\n";
                            }
                            if (line.ExceptionalInterestRateBaseId != gLAccountInterestPeriodPM.ExceptionalInterestRateBaseId)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.ExceptionalInterestRateBaseId", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.ExceptionalInterestRateName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.ExceptionalInterestRateName  + "\n";
                            }
                            if (line.ExceptionalAddInterestPercent != gLAccountInterestPeriodPM.ExceptionalAddInterestPercent)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.ExceptionalAddInterestPercent", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.ExceptionalAddInterestPercent.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.ExceptionalAddInterestPercent.ToString() + "\n";
                            }
                            if (line.CreditInterestRateBaseId != gLAccountInterestPeriodPM.CreditInterestRateBaseId)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.CreditInterestRateBaseId", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.CreditInterestRateBaseName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.CreditInterestRateBaseName  + "\n";
                            }
                            if (line.CreditAddInterestPercent != gLAccountInterestPeriodPM.CreditAddInterestPercent)
                            {
                                notes += TranslateTextsClass.Translate("GLAccountInterestPeriod.F.CreditAddInterestPercent", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.CreditAddInterestPercent.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + gLAccountInterestPeriodPM.CreditAddInterestPercent.ToString() + "\n";
                            }
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                EntityId = entityPM.Id,
                                Tenant = entityPM.Tenant,
                                UserId = contact.Id,
                                ObjectTableName = "GLAccount",
                                IsAddedManually = false,
                                EventTypeCode = "LUPD",
                                Notes = notes,
                            });
                        }
                    }

                }
            }

            var gLAccountInterestPeriodUpdateService = new GLAccountInterestPeriodUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            InterestPeriodUpdate(entityPM.GLAccountInterestPeriods.Where(s => s.ChangeSetOp != ChangeSetOperation.Delete).ToList(), entityPM.GLAccountInterestPeriods.Where(s => s.ChangeSetOp == ChangeSetOperation.Delete).ToList(), Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && entityPM.GLAccountWithholdingTaxes.Count > 0)
            {

                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                foreach (var line in entityPM.GLAccountWithholdingTaxes)
                {
                    if (line.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        var currentContextTag = line.CurrentContextTag ?? "";
                        if (currentContextTag.ToString() == GLAccountWithholdingTaxUpdateService.RaiseEventAWNCConst)
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
                    }
                }
            }
            base.UpdateComposition(entityPM);
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
                    string s = TranslateTextsClass.Translate("Accounting.General.O.Added", entityPM.Tenant, true);
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
                ContactPM contact = GetLoggedContact(entityPM.Tenant);// contactRep.get(resolveLoggingUserId, entityPM.Tenant);
                showLocals = !contact.DontShowLocal;
                CreateEventsForSomeFields();
                if (entityPM.GLAccountWithholdingTaxes.Count > 0)
                {
                    foreach (var line in entityPM.GLAccountWithholdingTaxes)
                    {
                        //if (line.ChangeSetOp == ChangeSetOperation.Insert)
                        //{
                        //    var currentContextTag = line.CurrentContextTag ?? "";
                        //    if (currentContextTag.ToString() == GLAccountWithholdingTaxUpdateService.RaiseEventAWNCConst)
                        //    {
                        //        String notes = TranslateTextsClass.Translate("Accounting.O.WithholdingLineCreated", entityPM.Tenant).Replace(":", line.LineNumber + ":");
                        //        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        //        {
                        //            EntityId = entityPM.Id,
                        //            Tenant = entityPM.Tenant,
                        //            UserId = contact.Id,
                        //            ObjectTableName = "GLAccount",
                        //            IsAddedManually = false,
                        //            EventTypeCode = "AWNC",
                        //            Notes = notes,
                        //        });
                        //    }
                        //}


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
                        Notes = notes,

                    });

                    //  EntityPOCO.PreviousNumber = entityPOCO.DisplayNumber;
                    //  EntityPOCO.PreviousNumberChangeDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }

                if (entityPM.ActiveForInterest != entityPOCO.ActiveForInterest && entityPM.ActiveForInterest == true)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "AFIT",
                    });
                }

                if (entityPM.Smallcashbook != entityPOCO.Smallcashbook)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "SCBC",
                    });
                }

                if (entityPM.ActiveForInterest != entityPOCO.ActiveForInterest && entityPM.ActiveForInterest == false)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "IFIT",
                    });
                }

                if (entityPM.InterestCalculationStartDate != entityPOCO.InterestCalculationStartDate)
                {
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + entityPOCO.InterestCalculationStartDate.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.InterestCalculationStartDate.ToString();
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "ISDT",
                        Notes = notes,

                    });
                }

                if (entityPM.InterestCreditLimit != entityPOCO.InterestCreditLimit)
                {
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + entityPOCO.InterestCreditLimit.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.InterestCreditLimit.ToString();
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "IRCH",
                        Notes = notes,

                    });
                }

                if (entityPM.ActiveForInterestCreditInvoice != entityPOCO.ActiveForInterestCreditInvoice)
                {
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + entityPOCO.ActiveForInterestCreditInvoice.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.ActiveForInterestCreditInvoice.ToString();
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "AFIC",
                        Notes = notes,

                    });
                }

                if (entityPM.MinimumInterestInvoiceBilling != entityPOCO.MinimumInterestInvoiceBilling)
                {
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals) + entityPOCO.MinimumInterestInvoiceBilling.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals) + entityPM.MinimumInterestInvoiceBilling.ToString();
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "MIIB",
                        Notes = notes,

                    });
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
                        if (oldParent != null)
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
                        EventTypeCode = "GLRC",

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
                if (entityPM.Type == "ADDED")
                {

                    string s = TranslateTextsClass.Translate("Accounting.General.O.Added", entityPM.Tenant, true);
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
                if (entityPM.Type == "CHILD")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.ChildAdded", entityPM.Tenant, true);
                    string[] text = s.Split('-');



                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.ParentAccountId,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CHID",
                        Notes = text[0] + "- " + entityPM.DisplayNumber + " " + text[1],

                    });
                }

                if (entityPM.Type == "INACTIVE")
                {
                    string s = TranslateTextsClass.Translate("Accounting.General.O.Deactivated", entityPM.Tenant, true);
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

                if (entityPM.IsMultiCurrency.GetValueOrDefault() && !EntityPOCO.IsMultiCurrency.GetValueOrDefault())
                {

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "GCC",
                        Notes = entityPM.Change2MultiCurrencyNotes //"Changed from XXX to Multi Currency -- updated X transaction , deleted X reconciliations"

                    });
                }

            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }
        GLAccountFollowUpDataPM gLAccountFollowUp;
        private void AddEventForGlAccountFollowUpData(GLAccountPM accountPM)
        {
            ContactPM contact = GetLoggedContact(accountPM.Tenant);
            showLocals = !contact.DontShowLocal;
            gLAccountFollowUp = GetGLAccountFollowUpDataPM(accountPM);
            if(gLAccountFollowUp != null)
            {
                if(gLAccountFollowUp.FollowUpDate != accountPM.GLAccountFollowUpDate)
                {
                    string oldValue = gLAccountFollowUp.FollowUpDate.ToString();
                    string newValue = accountPM.GLAccountFollowUpDate.ToString();                  
                    // CreateEvent("EVFD", oldValue , newValue);                  
                }
              
                if (gLAccountFollowUp.FollowUpRemarks != accountPM.GLAccountFollowUpRemarks)
                {
                    string oldValue = gLAccountFollowUp.FollowUpRemarks;
                    string newValue = accountPM.GLAccountFollowUpRemarks;                  
                    // CreateEvent( "EVFR",oldValue,newValue);
                    
                }
            }
        }
        
        private void CreateEvent(string eventCode, string oldValue, string newValue)
        {
            String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", EntityPM.Tenant, showLocals) +
                           oldValue+TranslateTextsClass.Translate("Accounting.General.O.NewValue", EntityPM.Tenant, showLocals) + newValue;
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = EntityPM.Id,
                Tenant = EntityPM.Tenant,
                UserId = GetLoggedContact(EntityPM.Tenant).Id,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = notes,

            });
        }
        protected override void AfterUpdating(GLAccountPM entityPM, EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);
            // Update Card GLAccountId [Maheera]  //
            UpdateCardGLAccountId(entityPM.Tenant, entityPM.NewGLAccountCardId, entityPM.Id);
            SendHybridTask(entityPM);
        }

        private string GetDisplayNumberFromGLAccount(string GLAccountId,int tenant)
        {
            GLAccountQueryService query = new  GLAccountQueryService(tenant);
            return query.GetDisplayNumberByGLAccountId(GLAccountId, tenant);
        }

        public bool FullAccountingProvider { get; set; }
        protected override void Validate(GLAccountPM entityPM)
        {
            ValidationResult result = GLAccountValidator.IsGLAccountValid(entityPM, FullAccountingProvider);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

      

        // PRIVATE METHODS
        bool showLocals;
        string notes = null;
        string eventCode = null;
        private void CreateEventsForSomeFields()
        {
            List<string> properties = new List<string>() { "IsVatExempt", "Category1Id", "Category2Id", "Category3Id", "Category4Id", "Category5Id", "RevaluationEnabled", "IsMultiCurrency", "ReconcileMethodCode", "AutomaticReconcileId", "ReportingAsAnotherDocument" };
            //PropertyInfo[] pmProperties = EntityPM.GetType().GetProperties();
            //PropertyInfo[] pocoProperties = EntityPOCO.GetType().GetProperties();
            foreach (string property in properties)
            {
                SetNotesAndEventCodeForTraceEvent(property);
              if(notes != null)
                 CreateUpdateTraceEvent(notes, eventCode);
               
               
            }




        }

        private void SetNotesAndEventCodeForTraceEvent(string FieldName)
        {

            switch (FieldName)
            {
                case "Category1Id":
                    {
                        eventCode = "CAT1";
                        notes = GetTraceEventNotesForCategory1Field();
                        break;
                    }
                case "Category2Id":
                    {
                        eventCode = "CAT2";
                        notes = GetTraceEventNotesForCategory2Field();
                        break;
                    }
                case "Category3Id":
                    {
                        eventCode = "CAT3";
                        notes = GetTraceEventNotesForCategory3Field();
                        break;
                    }
                case "Category4Id":
                    {
                        eventCode = "CAT4";
                        notes = GetTraceEventNotesForCategory4Field();
                        break;
                    }
                case "Category5Id":
                    {
                        eventCode = "CAT5";
                        notes = GetTraceEventNotesForCategory5Field();
                        break;
                    }
                case "RevaluationEnabled":
                    {
                        if (EntityPOCO.RevaluationEnabled != EntityPM.RevaluationEnabled)
                        {
                            eventCode = "RVUP";
                            notes = GetTraceEventNotesForBooleanField();
                        }
                        else notes = null;
                        break;
                    }
                case "IsMultiCurrency":
                    {
                        if (EntityPOCO.IsMultiCurrency != EntityPM.IsMultiCurrency)
                        {
                            eventCode = "MLUP";
                            notes = GetTraceEventNotesForBooleanField();
                        }
                        else notes = null;
                        break;
                    }
                case "ReconcileMethodCode":
                    {
                        if (EntityPOCO.ReconcileMethodCode != EntityPM.ReconcileMethodCode)
                        {
                            eventCode = "RMUP";
                            notes = GetTraceEventNotesFoReconcileMethodCodeField();
                        }
                        else notes = null;
                        break;
                    }
                case "AutomaticReconcileId":
                    {
                        if (EntityPOCO.AutomaticReconcileId != EntityPM.AutomaticReconcileId)
                        {
                            eventCode = "ARCP";
                            notes = GetTraceEventNotesForAutomaticReconcileIdField();
                        }
                        else notes = null;
                        break;
                    }
                case "IsVatExempt":
                    {
                        if (EntityPOCO.IsVATExempt != EntityPM.IsVATExempt)
                        {
                            eventCode = "VAEX";
                            notes = GetTraceEventNotesFordVatExcempt();
                        }
                        else notes = null;
                        break;
                    }
                case "ReportingAsAnotherDocument":
                    {
                        if(EntityPOCO.ReportingAsAnotherDocument != EntityPM.ReportingAsAnotherDocument)
                        {
                            eventCode = "GLRC";
                            notes = "";
                        }
                        break;
                    }
            }

        }
        public string GetTraceEventNotesForCategory1Field()
        {
            if (EntityPOCO.Category1Id != EntityPM.Category1Id)
            {
                string oldValue = null;
                string newValue = null;
                Category1QueryService category1QueryService = new Category1QueryService(EntityPM.Tenant);
                Category1PM category1 = category1QueryService.GetSinglePM(EntityPOCO.Category1Id, EntityPOCO.Tenant);
                if (category1 != null)
                {
                    oldValue = showLocals ? category1.LocalName : category1.EnglishName;
                }
                category1 = category1QueryService.GetSinglePM(EntityPM.Category1Id, EntityPM.Tenant);
                if (category1 != null)
                {
                    newValue = showLocals ? category1.LocalName : category1.EnglishName;
                }
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;
            }
            else return null;
        }
        public string GetTraceEventNotesForCategory2Field()
        {
            if (EntityPOCO.Category2Id != EntityPM.Category2Id)
            {
                string oldValue = null;
                string newValue = null;
                Category2QueryService category2QueryService = new Category2QueryService(EntityPM.Tenant);
                Category2PM category2 = category2QueryService.GetSinglePM(EntityPOCO.Category2Id, EntityPOCO.Tenant);
                if (category2 != null)
                {
                    oldValue = showLocals ? category2.LocalName : category2.EnglishName;
                }
                category2 = category2QueryService.GetSinglePM(EntityPM.Category2Id, EntityPM.Tenant);
                if (category2 != null)
                {
                    newValue = showLocals ? category2.LocalName : category2.EnglishName;
                }
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;
            }
            else return null;
        }
        public string GetTraceEventNotesForCategory3Field()
        {
            if (EntityPOCO.Category3Id != EntityPM.Category3Id)
            {
                string oldValue = null;
                string newValue = null;
                Category3QueryService category3QueryService = new Category3QueryService(EntityPM.Tenant);
                Category3PM category3 = category3QueryService.GetSinglePM(EntityPOCO.Category3Id, EntityPOCO.Tenant);
                if (category3 != null)
                {
                    oldValue = showLocals ? category3.LocalName : category3.EnglishName;
                }
                category3 = category3QueryService.GetSinglePM(EntityPM.Category3Id, EntityPM.Tenant);
                if (category3 != null)
                {
                    newValue = showLocals ? category3.LocalName : category3.EnglishName;
                }
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;
            }
            else return null;
        }
        public string GetTraceEventNotesForCategory4Field()
        {
            if (EntityPOCO.Category4Id != EntityPM.Category4Id)
            {
                string oldValue = null;
                string newValue = null;
                Category4QueryService category4QueryService = new Category4QueryService(EntityPM.Tenant);
                Category4PM category4 = category4QueryService.GetSinglePM(EntityPOCO.Category4Id, EntityPOCO.Tenant);
                if (category4 != null)
                {
                    oldValue = showLocals ? category4.LocalName : category4.EnglishName;
                }
                category4 = category4QueryService.GetSinglePM(EntityPM.Category4Id, EntityPM.Tenant);
                if (category4 != null)
                {
                    newValue = showLocals ? category4.LocalName : category4.EnglishName;
                }
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;
            }
            else return null;
        }
        public string GetTraceEventNotesForCategory5Field()
        {
            if (EntityPOCO.Category5Id != EntityPM.Category5Id)
            {
                string oldValue = null;
                string newValue = null;
                Category5QueryService category5QueryService = new Category5QueryService(EntityPM.Tenant);
                Category5PM category5 = category5QueryService.GetSinglePM(EntityPOCO.Category5Id, EntityPOCO.Tenant);
                if (category5 != null)
                {
                    oldValue = showLocals ? category5.LocalName : category5.EnglishName;
                }
                category5 = category5QueryService.GetSinglePM(EntityPM.Category5Id, EntityPM.Tenant);
                if (category5 != null)
                {
                    newValue = showLocals ? category5.LocalName : category5.EnglishName;
                }
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;
            }
            else return null;
        }
        private string GetTraceEventNotesForBooleanField()
        {
            string oldValue = GetBooleanText(EntityPOCO.RevaluationEnabled);
            string newValue = GetBooleanText(EntityPM.RevaluationEnabled);
            return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;

        }
        private string GetTraceEventNotesFoReconcileMethodCodeField()
        {
            string oldValue = null;
            string newValue = null;
            ReconcileMethodQueryService reconcileMethodQueryService = new ReconcileMethodQueryService(EntityPM.Tenant);
            ReconcileMethodPM reconcileMethod = reconcileMethodQueryService.GetSinglePM(EntityPOCO.ReconcileMethodCode, EntityPOCO.Tenant);
            if (reconcileMethod != null)
            {
                oldValue = showLocals ? reconcileMethod.LocalName : reconcileMethod.EnglishName;

            }
            reconcileMethod = reconcileMethodQueryService.GetSinglePM(EntityPM.ReconcileMethodCode, EntityPM.Tenant);
            if (reconcileMethod != null)
            {
                newValue = showLocals ? reconcileMethod.LocalName : reconcileMethod.EnglishName;

            }
            return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;

        }
        private string GetTraceEventNotesForAutomaticReconcileIdField()
        {
            string oldValue = null;
            string newValue = null;
            IAccountingContext accountingContext = AccountingContext.GetContext(EntityPOCO.Tenant);

            AutomaticReconcileMethodListQueryService automaticReconcileMethodListQueryService = new AutomaticReconcileMethodListQueryService(accountingContext);
            AutomaticReconcileMethodList automaticReconcileMethod = automaticReconcileMethodListQueryService.GetById(EntityPOCO.AutomaticReconcileId, EntityPOCO.Tenant);
            if (automaticReconcileMethod != null)
            {
                oldValue = showLocals ? automaticReconcileMethod.LocalName : automaticReconcileMethod.Name;

            }
            automaticReconcileMethod = automaticReconcileMethodListQueryService.GetById(EntityPM.AutomaticReconcileId, EntityPM.Tenant);
            if (automaticReconcileMethod != null)
            {
                newValue = showLocals ? automaticReconcileMethod.LocalName : automaticReconcileMethod.Name;

            }
            return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;

        }
        private string GetTraceEventNotesFordVatExcempt()
        {
            if (EntityPOCO.IsVATExempt != EntityPM.IsVATExempt)
            {
                string oldValue = GetBooleanText(EntityPOCO.IsVATExempt);
                string newValue = GetBooleanText(EntityPM.IsVATExempt);
                return TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0, showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0, showLocals) + newValue;

            }
            else return null;
        }
        private string GetBooleanText(bool? value)
        {
            if (value == true)
            {
                return TranslateTextsClass.Translate("Accounting.General.O.True", 0, showLocals);
            }
            else if (value == false || value == null)
            {
                return TranslateTextsClass.Translate("Accounting.General.O.False", 0, showLocals);
            }
            else return TranslateTextsClass.Translate("Accounting.General.O.False", 0, showLocals);
        }
        private void CreateUpdateTraceEvent(string notes, string eventCode)
        {
            //  String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0,showLocals) + oldValue + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0,showLocals) + newValue;
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = EntityPM.Id,
                Tenant = EntityPM.Tenant,
                UserId = EntityPM.UpdatedByUserId,
                ObjectTableName = "GLAccount",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = notes,

            });



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

        private void InsertGLAccountMoreData(GLAccountPM entityPM)
        {
            var myGLAccountMoreDataUpdateService = new GLAccountMoreDataUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myGLAccountMoreDataUpdateService.Update(new GLAccountMoreDataPM()
            {
                AccountId = entityPM.Id,
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                BalanceInLocalCurrency = 0,
                LocalBalanceInDue = 0,

                TotalOpenChequesInLocalCur = 0,
                TotFutureOpenChequesInLocalCur = 0,

                BalanceInForeignCurrency = 0,
                ForeignBalanceInDue = 0,


            }, false);
        }

        private void InsertGLAccountAgingData(GLAccountPM entityPM)
        {
            var myGLAccountAgingDataPMUpdateService = new GLAccountAgingDataUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myGLAccountAgingDataPMUpdateService.Update(new GLAccountAgingDataPM()
            {
                AccountId = entityPM.Id,
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
            }, false);
        }
        
            
        private void InterestPeriodUpdate(List<GLAccountInterestPeriodPM> GLAccountInterestPeriods, List<GLAccountInterestPeriodPM> DeletedGLAccountInterestPeriods, int Tenant)
        {
            var myGLAccountInterestPeriodUpdateService = new GLAccountInterestPeriodUpdateService(this.MainContext, new Dictionary<string, IContext>(), Tenant);
            foreach (GLAccountInterestPeriodPM DeletedgLAccountInterestPeriodPM in DeletedGLAccountInterestPeriods)
            {
                myGLAccountInterestPeriodUpdateService.Update(DeletedgLAccountInterestPeriodPM, true);
            }

            foreach (GLAccountInterestPeriodPM gLAccountInterestPeriodPM in GLAccountInterestPeriods)
            {
                myGLAccountInterestPeriodUpdateService.Update(gLAccountInterestPeriodPM, false);
            }

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
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
            }
            delta = entityPM.LocalBalanceInDue.GetValueOrDefault() - entityPOCO.LocalBalanceInDue.GetValueOrDefault();
            if (delta != 0)
            {
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
            }
            if (entityPM.NextDueDate.GetValueOrDefault() != entityPOCO.NextDueDate.GetValueOrDefault())
            {
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
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
                    if (trans != null)
                        throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccounts.O.ReconcileMethodcantUpdated", 0, showLocals));
                }
            }

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


        private void UpdateCardGLAccountId(int tenant, string cardId, string glAccountId)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                CardQuery query = new CardQuery(tenant);
                CardService service = new CardService(CommonDataContext.GetContext(tenant), tenant);
                CardPM card = query.GetSinglePM(cardId, tenant);
                card.GLAccountId = glAccountId;
                card.GLAccountDisplayNumber = GetDisplayNumberFromGLAccount(glAccountId, tenant);
                service.Update(card);
            }
        }


        private void ValidateCurrencyChange(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            CheckMultiToSingleCurrencyChanged(entityPM, entityPOCO);
            CheckSingleToSingleCurrencyChanged(entityPM, entityPOCO);
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert) {
                CheckIfGlaccountIsConnectedToBankGlAccount(entityPM, entityPOCO);
            }
            CheckReconcileMethodChange(entityPM, entityPOCO);
        }

        private void CheckIfGlaccountIsConnectedToBankGlAccount(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            BankAccountRepository repo = new BankAccountRepository(entityPM.Tenant);
            var isGlaccountExistsInBankAccount = repo.CheckIfGlAccountExistsInBankAccount(entityPM.Id, entityPM.Tenant);
            if (entityPM.ChartOfAccountsTypeCode == ChartOfAccountsTypeEnum.Banks.ToIntString() && isGlaccountExistsInBankAccount
                && ((entityPOCO.IsMultiCurrency != entityPM.IsMultiCurrency) || (entityPOCO.CurrencyId != entityPM.CurrencyId))) {
                bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
                throw new ApplicationException(TranslateTextsClass.Translate("BankAccounts.O.PreventChangingCurrency", 0, useLocal));
            }
        }

        private static void CheckSplittedGLAccount(GLAccountPM entityPM)
        {
            IAccountingContext ccc = AccountingContext.GetContext(entityPM.Tenant);
            GLAccountCurrencyListQueryService GLAccountCurrencyQuery = new GLAccountCurrencyListQueryService(ccc);
            GLAccountCurrencyList glAccountCurrencyList = GLAccountCurrencyQuery.GetByAccountNumber(entityPM.Id, entityPM.Tenant);
            if (glAccountCurrencyList != null)
            {
                throw new ApplicationException("This GLAccount have splitted GLAccounts by currency, Deactivate these GLAccounts before doing these action");
            }
        }

        private void CheckSingleToSingleCurrencyChanged(GLAccountPM entityPM, GLAccount entityPOCO)
        {


            if ((entityPOCO.IsMultiCurrency == false && entityPM.IsMultiCurrency == false) && entityPOCO.CurrencyId != entityPM.CurrencyId)
            {
                List<LedgerTransactionList> openTransactions = GetOpenTransactionsForAccount(entityPM.Id, entityPM.Tenant);

                if (openTransactions.Count > 0)
                {

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

                        if (transactionsCurrencyId != entityPM.CurrencyId)
                        {
                            // throw error
                            // the transactions currency does not equal entered account currency 
                            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
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
                        ThrowOpenTransactionMessage(entityPM.Tenant);
                    }
                }
            }
        }

        private void ThrowOpenTransactionMessage(int tenant)
        {
            // not all transaction have same currency, the transactions have different currencies
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            throw new ApplicationException(TranslateTextsClass.Translate("Accounting.General.O.ThereOpenTransaction", 0, useLocal));
        }

        private List<LedgerTransactionList> GetOpenTransactionsForAccount(string accountId, int tenant)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(accountingContext);
            List<LedgerTransactionList> openTransactions = transactionListQuery.GetOpenByAccountId(accountId, tenant);
            return openTransactions;
        }

        private void CheckMultiToSingleCurrencyChanged(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            if (entityPOCO.IsMultiCurrency == true && entityPM.IsMultiCurrency == false)
            {
                CheckAccountTransactions(entityPM);
                CheckSplittedGLAccount(entityPM);
            }
        }
        private void CheckReconcileMethodChange(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            bool isTransactionFixed = !string.IsNullOrWhiteSpace(entityPM.Change2MultiCurrencyNotes);
            if (isTransactionFixed)
            {
                return;
            }
            if (
                (entityPOCO.IsMultiCurrency == false && entityPM.IsMultiCurrency == true)
                && (entityPOCO.ReconcileMethodCode != entityPM.ReconcileMethodCode)
                )
            {
                List<LedgerTransactionList> openTransactions = GetAccountTransactions(entityPM);
                if (openTransactions.Count > 0)
                {
                    var tenant = entityPM.Tenant;
                    var msg = TextCodesTranslator.TranslateText("GLAccount.O.CantChangeRecoMethod", tenant, LoggedContactResolver.GetLoggedContactShowLocal(tenant));
                    throw new ApplicationException(msg);
                }

            }
        }

        private static List<LedgerTransactionList> GetAccountTransactions(GLAccountPM entityPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(MyContext);
            List<LedgerTransactionList> openTransactions = transactionListQuery.GetByAccountId(entityPM.Id, entityPM.Tenant);
            return openTransactions;
        }

        private void CheckAccountTransactions(GLAccountPM entityPM)
        {

            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            LedgerTransactionListQueryService transactionListQuery = new LedgerTransactionListQueryService(MyContext);
            bool useLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
            List<LedgerTransactionList> openTransactions = transactionListQuery.GetOpenByAccountId(entityPM.Id, entityPM.Tenant);

            if (openTransactions.Count > 0)
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
                        ThrowOpenTransactionMessage(entityPM.Tenant);
                    }
                    else
                    {
                        //same currency, continue

                    }
                }
                else
                {
                    ThrowOpenTransactionMessage(entityPM.Tenant);
                }
            }
            else
            {
                // no transactions
                // so we can continue
            }
        }

        private void CheckCurrencyChangedFromSingleToMulti(GLAccountPM entityPM, GLAccount entityPOCO)
        {
            if (entityPOCO.IsMultiCurrency == false && entityPM.IsMultiCurrency == true)
            {
                // Allow to change 
            }
        }
        private void InsertGLAccountRecocileData(GLAccountPM entityPM)
        {
            var myGLAccountRecocileDataUpdateService = new GLAccountRecocileDataUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myGLAccountRecocileDataUpdateService.Update(new GLAccountRecocileDataPM()
            {
                AccountId = entityPM.Id,
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,



            }, false);
        }
    }


    public class GLAccountUpdateServiceBalancePriv : GLAccountUpdateService
    {
        private decimal? _deltaBalanceInLocalCurrency;
        private decimal? _deltaLocalBalanceInDue;
        private DateTime? _nextDueDate;
        public GLAccountUpdateServiceBalancePriv(IContext mainContext, Dictionary<string, IContext> additionalContexts, int tenant)
            : base(mainContext, additionalContexts, tenant)
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
            decimal? delta = null;
#if GLAccMoreData
            var delta = entityPM.BalanceInLocalCurrency.GetValueOrDefault() - entityPOCO.BalanceInLocalCurrency.GetValueOrDefault();
            if (delta != _deltaBalanceInLocalCurrency.GetValueOrDefault())
            {
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
            }
            delta = entityPM.LocalBalanceInDue.GetValueOrDefault() - entityPOCO.LocalBalanceInDue.GetValueOrDefault();

            if (delta != _deltaLocalBalanceInDue.GetValueOrDefault())
            {
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
            }
            if (entityPM.NextDueDate.GetValueOrDefault() != _nextDueDate.GetValueOrDefault())
            {
                throw new ApplicationException(messnoprivtochangeBalanceInLocalCurrency);
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
