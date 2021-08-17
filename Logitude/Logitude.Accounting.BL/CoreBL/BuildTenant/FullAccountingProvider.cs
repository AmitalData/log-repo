using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant
{


  public  class FullAccountingProvider 
    {
        string DefaultVATTypeId = null;
        private GLAccountUpdateService _GLAccountRepository;
        private IChartOfAccountProvider _ChartOfAccountProvider;
        int _Counter = 0;
        private DisplayNumberProvider _DisplayNumberProvider;
        private GLAccountMoreDataRepository _GLAccountMoreDataRepository;
        private GLAccountUpdateService _GLAccountUpdateService;

        public FullAccountingProvider(IChartOfAccountProvider chartOfAccountProvider , DisplayNumberProvider displayNumberProvider )
        {
            _DisplayNumberProvider = displayNumberProvider;
            _ChartOfAccountProvider = chartOfAccountProvider;
        }
        
        public FullAccountingSetting Insert(int tenant, IAccountingContext accountingContext, BuildAccountingTenantParam buildAccountingTenant)
        {
            if (buildAccountingTenant == null)
            {
                return null;
            }
            if 
                (
                //!buildAccountingTenant.OtherAccounts &&
                !buildAccountingTenant.VatAccounts &&
                !buildAccountingTenant.ControlAccounts &&
                !buildAccountingTenant.ExchangeRateDiff &&
                !buildAccountingTenant.RevenueExpense &&
                !buildAccountingTenant.TaxWithholding
                )
            {
                return null;
            }

            using (var trans = TransactionFactory.GetTransaction())
            {

                accountingContext = accountingContext ?? AccountingContext.GetContext(tenant);
                var fullAccountingSettingRepo
                    = new FullAccountingSettingRepository(accountingContext);
                var fullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
                FullAccountingSetting fullSetting = null;
                if (buildAccountingTenant.CheckAndInsertPoco)
                {
                    fullSetting = CheckAndCreatePoco(tenant, accountingContext, fullAccountingSettingQueryService);
                }
                else
                {
                    fullSetting = fullAccountingSettingRepo.GetSingleFullAccountingSetting(tenant);
                }
                //CreateVatGLAccount(tenant, accountingContext, fullSetting);
                if (buildAccountingTenant.ExchangeRateDiff)
                {
                    fullSetting.ExchangeRateDiffGLAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
                    {
                        Tenant = tenant,
                        AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),// "1",
                        InternalNumber = "",
                        DisplayNumber = "",
                        LocalName = "הפרשי שער",
                        EnglishName = "Exchange rate differences",
                        SearchFields = ",Exchange rate differences,הפרשי שער",
                        IsMultiCurrency = true,
                        CurrencyId = null,
                        RevenueExpenseType = RevenueExpenseTypeEnum.Expense.ToIntString(),// "3",//OTHER
                        IsControlAccount = false /*true*/,

                        Inactive = false,
                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Expenses.ToIntString(),//"1",//Revenues
                        ChartOfAccountsId = _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.Expenses),//"1-105"
                                                                                                                                   //CurrencyCode = "Multi",
                        ReconcileMethodCode = "0",
                        ControlAccountId = null,
                        AutomaticReconcileId = null,
                        PreviousEnglishName = null,
                        PreviousNumber = null,
                    });
                }
                if (buildAccountingTenant.RevenueExpense)
                {
                    fullSetting.RevenueExpenseGLAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
                    {
                        Tenant = tenant,
                        AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),// "1",
                        InternalNumber = "",
                        DisplayNumber = "",
                        LocalName = "רווח והפסד משנה קודמת",
                        EnglishName = "Profit and loss LAST YEAR",
                        SearchFields = ",Profit and loss LAST YEAR,רווח והפסד משנה קודמת",
                        IsMultiCurrency = true,
                        CurrencyId = null,
                        RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),// "3",//OTHER
                        IsControlAccount = false/*true*/,

                        Inactive = false,
                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Revenues.ToIntString(),//"1",//Revenues
                        ChartOfAccountsId = _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.Revenues),//"1-105"
                                                                                                                                   //CurrencyCode = "Multi",
                        ReconcileMethodCode = "0",
                        ControlAccountId = null,
                        AutomaticReconcileId = null,
                        PreviousEnglishName = null,
                        PreviousNumber = null,
                    });
                }
                if (String.IsNullOrWhiteSpace( buildAccountingTenant.ChartOfAccountsId))
                {
                    buildAccountingTenant.ChartOfAccountsId = null;
                }
                if (buildAccountingTenant.ControlAccounts)
                {
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||  
                        (buildAccountingTenant.ControlAccountId  == "CustomerControlAccountId"  && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId) )
                        )
                    {
                        CreateCustomerControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
    (buildAccountingTenant.ControlAccountId == "VendorControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId))
    )

                    {
                        CreateVendorControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
(buildAccountingTenant.ControlAccountId == "FileControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId)))

                    {
                        CreateFileControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
(buildAccountingTenant.ControlAccountId == "OceanExportJobControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId)))

                    {
                        CreateOceanExportJobControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
(buildAccountingTenant.ControlAccountId == "AirExportJobControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId)))

                    {
                        CreateAirExportJobControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
(buildAccountingTenant.ControlAccountId == "OceanImportJobControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId)))

                    {
                        CreateOceanImportJobControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    if (String.IsNullOrWhiteSpace(buildAccountingTenant.ControlAccountId) ||
(buildAccountingTenant.ControlAccountId == "AirImportJobControlAccountId" && !String.IsNullOrWhiteSpace(buildAccountingTenant.ChartOfAccountsId)))

                    {
                        CreateAirImportJobControlAccountId(tenant, accountingContext, fullSetting, buildAccountingTenant.ChartOfAccountsId);
                    }
                    ///TEnant  ---fullPm.TenantPaymentTermId = GetTenantPaymentTermId(tenant);
                }
                if (buildAccountingTenant.VatAccounts)
                {
                    CreateVatGLAccount(tenant, accountingContext, fullSetting);
                }
                if (buildAccountingTenant.TaxWithholding)
                {
                    fullSetting.TaxWithholdingGLAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
                    {
                        Tenant = tenant,
                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors.ToIntString(),// "7",
                        ChartOfAccountsId = _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.DebtorsAndCreditors),//"1-105"
                        InternalNumber = null,
                        DisplayNumber = "",
                        AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                        LocalName = "ניכוי מס במקור",
                        EnglishName = "Tax Deduction",
                        SearchFields = ",Tax Deduction,ניכוי מס במקור",
                        IsMultiCurrency = true,
                        CurrencyId = null,
                        RevenueExpenseType = "3",//OTHER


                        IsControlAccount = false,
                        Inactive = false,
                        //CurrencyCode = "Multi",
                        ReconcileMethodCode = "0",
                        ControlAccountId = null,
                        AutomaticReconcileId = null,
                        PreviousEnglishName = null,
                        PreviousNumber = null,
                    });
                }
                if (buildAccountingTenant.CheckAndInsertPoco)
                {
                    fullAccountingSettingRepo.Add(fullSetting);
                }
                else
                {
                    fullAccountingSettingRepo.Update(fullSetting);
                }
                accountingContext.SaveChanges();

                trans.Complete();
                return fullSetting;
            }
        }

        private FullAccountingSetting CheckAndCreatePoco(int tenant, IAccountingContext accountingContext, FullAccountingSettingQueryService fullAccountingSettingQueryService)
        {
            var fullSettingpm = fullAccountingSettingQueryService.GetSingle(tenant.ToString(), false, false);
            if (fullSettingpm != null)
            {
                throw new Exception("I create All Or Nothing !!");
            }
            var coaRepo = new ChartOfAccountRepository(tenant);
            var coa = coaRepo.GetAll(tenant).FirstOrDefault();
            if (coa != null)
            {
                throw new Exception("I create All Or Nothing !!");
            }

            _ChartOfAccountProvider.CreateCOA(accountingContext, tenant);
            FullAccountingSetting fullSetting = new FullAccountingSetting()
            {
                Id = tenant.ToString(),
                Tenant = tenant,
                //ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert
            };
            return fullSetting;
        }

        private void CreateAirImportJobControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.AirImportJobControlAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
            {
                Tenant = tenant,
                ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                ChartOfAccountsId = chartOfAccountsId??_ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject),//"1-105"
                InternalNumber = null,
                DisplayNumber = "",
                AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                LocalName = "מרכז ספדים יבוא אוירי",
                EnglishName = "AI Sped",
                SearchFields = ",AI Sped,מרכז ספדים יבוא אוירי",
                IsMultiCurrency = true,
                CurrencyId = null,
                RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),// "1",//הכנסות


                IsControlAccount = true,
                Inactive = false,
                //CurrencyCode = "Multi",
                ReconcileMethodCode = "0",
                ControlAccountId = null,
                AutomaticReconcileId = null,
                PreviousEnglishName = null,
                PreviousNumber = null,
            });
        }

        private void CreateOceanImportJobControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.OceanImportJobControlAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
            {
                Tenant = tenant,
                ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                ChartOfAccountsId = chartOfAccountsId??_ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject),//"1-105"
                InternalNumber = null,
                DisplayNumber = "",
                AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                LocalName = "מרכז ספדים יבוא ימי",
                EnglishName = "OI Sped",
                SearchFields = ",OI Sped,מרכז ספדים יבוא ימי",
                IsMultiCurrency = true,
                CurrencyId = null,
                RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),//> "1",//הכנסות


                IsControlAccount = true,
                Inactive = false,
                //CurrencyCode = "Multi",
                ReconcileMethodCode = "0",
                ControlAccountId = null,
                AutomaticReconcileId = null,
                PreviousEnglishName = null,
                PreviousNumber = null,
            });
        }

        private void CreateAirExportJobControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.AirExportJobControlAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
            {
                Tenant = tenant,
                ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                ChartOfAccountsId = chartOfAccountsId??_ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject),//"1-105"
                InternalNumber = "",
                DisplayNumber = "",
                AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                LocalName = "מרכז תיקי יצוא אוירי",
                EnglishName = "AE Control",
                SearchFields = ",AE Control,מרכז תיקי יצוא אוירי",
                IsMultiCurrency = true,
                CurrencyId = null,
                RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),//"1",//הכנסות


                IsControlAccount = true,
                Inactive = false,
                //CurrencyCode = "Multi",
                ReconcileMethodCode = "0",
                ControlAccountId = null,
                AutomaticReconcileId = null,
                PreviousEnglishName = null,
                PreviousNumber = null,
            });
        }

        private void CreateOceanExportJobControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.OceanExportJobControlAccountId =
                InsertGLAccountGetId(accountingContext, new GLAccount()
                {
                    Tenant = tenant,
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                            ChartOfAccountsId = chartOfAccountsId?? _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject),//"1-105"
                            InternalNumber = "",
                    DisplayNumber = "",
                    AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                            LocalName = "מרכז ספדים יצוא ימי",
                    EnglishName = "OE Control",
                    SearchFields = ",OE Control,מרכז ספדים יצוא ימי",
                    IsMultiCurrency = true,
                    CurrencyId = null,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),// "1",//הכנסות


                    IsControlAccount = true,
                    Inactive = false,
                            //CurrencyCode = "Multi",
                            ReconcileMethodCode = "0",
                    ControlAccountId = null,
                    AutomaticReconcileId = null,
                    PreviousEnglishName = null,
                    PreviousNumber = null,
                });
        }

        private void CreateFileControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.FileControlAccountId =
                                    InsertGLAccountGetId(accountingContext, new GLAccount()
                                    {
                                        Tenant = tenant,
                                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                            ChartOfAccountsId = chartOfAccountsId??_ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject),//"1-105"
                            InternalNumber = null,
                                        AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                            DisplayNumber = "",
                                        LocalName = "מרכז תיקים",
                                        EnglishName = "Files Control",
                                        SearchFields = ",Files Control,מרכז תיקים",
                                        IsMultiCurrency = true,
                                        CurrencyId = null,
                                        RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),//"1",//הכנסות

                                        IsControlAccount = true,
                                        Inactive = false,
                            //CurrencyCode = "Multi",
                            ReconcileMethodCode = "0",
                                        ControlAccountId = null,
                                        AutomaticReconcileId = null,
                                        PreviousEnglishName = null,
                                        PreviousNumber = null,
                                    });
        }

        private void CreateVendorControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.VendorControlAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
            {
                Tenant = tenant,
                InternalNumber = null,
                AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//1
                DisplayNumber = "Vendors Control",
                LocalName = "מרכז ספקים",
                EnglishName = "Vendors Control",
                SearchFields = "Vendors,Vendors Control,מרכז ספקים",
                IsMultiCurrency = true,
                CurrencyId = null,
                RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),//"2",//expenss
                IsControlAccount = true,

                Inactive = false,
                ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Vendors.ToIntString(),// "4",
                ChartOfAccountsId = chartOfAccountsId??_ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.Vendors),//"1-105"
                                                                                                                          //CurrencyCode = "Multi",
                ReconcileMethodCode = "0",
                ControlAccountId = null,
                AutomaticReconcileId = null,
                PreviousEnglishName = null,
                PreviousNumber = null,
            });
        }

        private void CreateCustomerControlAccountId(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting, string chartOfAccountsId)
        {
            fullSetting.CustomerControlAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
            {
                Tenant = tenant,
                InternalNumber = null,
                AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),//
                DisplayNumber = "Customers Control",
                LocalName = "מרכז לקוחות",
                EnglishName = "Customer Control",
                SearchFields = "Customers,Customers Control,מרכז לקוחות",
                IsMultiCurrency = true,
                CurrencyId = null,
                RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString() ,//   "1",//הכנסות
                IsControlAccount = true,

                Inactive = false,
                ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Customers.ToIntString(),//"3",
                ChartOfAccountsId = chartOfAccountsId?? _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.Customers),//"1-105"
                                                                                                                            //CurrencyCode = "Multi",
                ReconcileMethodCode = "0",
                ControlAccountId = null,
                AutomaticReconcileId = null,
                PreviousEnglishName = null,
                PreviousNumber = null,
            });
        }

        public void CreateVatGLAccount(int tenant, IAccountingContext accountingContext, FullAccountingSetting fullSetting)
        {
            if (!string.IsNullOrWhiteSpace(fullSetting.VATInputsGLAccountId))
            {
                throw new Exception("CreateVatGLAccount()-!string.IsNullOrWhiteSpace(fullSetting.VATInputsGLAccountId)");
            }
            if (!string.IsNullOrWhiteSpace(fullSetting.VATOutputGLAccountId))
            {
                throw new Exception("CreateVatGLAccount()-!string.IsNullOrWhiteSpace(fullSetting.VATOutputGLAccountId)");
            }
            using (var trans = TransactionFactory.GetTransaction())
            {
                fullSetting.DefaultVATTypeId = GetDefaultVATTypeId(tenant);


                fullSetting.AutomaticReconcileMethodId = null;
                fullSetting.VATInputsGLAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
                {
                    Tenant = tenant,
                    AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),// "1",
                    InternalNumber = "",
                    DisplayNumber = "",
                    LocalName = "מעמ תשומות",
                    EnglishName = "Input VAT",
                    SearchFields = ",Input VAT,מעמ תשומות",
                    IsMultiCurrency = true,
                    CurrencyId = null,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),// "3",//OTHER
                    IsControlAccount = false  /*true */,

                    Inactive = false,
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors.ToIntString(),//"1",//Revenues
                    ChartOfAccountsId = _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.DebtorsAndCreditors),//"1-105"
                                                                                                                                          ////CurrencyCode = "Multi",

                    ReconcileMethodCode = "0",
                    ControlAccountId = null,
                    AutomaticReconcileId = null,
                    PreviousEnglishName = null,
                    PreviousNumber = null,
                });
                fullSetting.VATOutputGLAccountId = InsertGLAccountGetId(accountingContext, new GLAccount()
                {
                    Tenant = tenant,
                    AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),// "1",
                    InternalNumber = "",
                    DisplayNumber = "",
                    LocalName = "מעמ עסקאות",
                    EnglishName = "Output VAT",
                    SearchFields = ",Output VAT,מעמ עסקאות",
                    IsMultiCurrency = true,
                    CurrencyId = null,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),
                    IsControlAccount = false,/*true*/

                    Inactive = false,
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors.ToIntString(),//"1",//Revenues
                    ChartOfAccountsId = _ChartOfAccountProvider.GetChartOfAccountsId(tenant, ChartOfAccountsTypeEnum.DebtorsAndCreditors),//"1-105"
                                                                                                                                          //CurrencyCode = "Multi",
                    ReconcileMethodCode = "0",
                    ControlAccountId = null,
                    AutomaticReconcileId = null,
                    PreviousEnglishName = null,
                    PreviousNumber = null,
                });

                trans.Complete();

            }
        }







        string InsertGLAccountGetId(IAccountingContext accountingContext,GLAccount poco)
        {
            //pm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            int display = CodeCounter.GetNumber(/*DummyTenantProvider*/ "DummyTP:" + poco.ChartOfAccountsTypeCode, poco.Tenant);
            int InternalNumber = CodeCounter.GetNumber("GLAccount", poco.Tenant);
            _GLAccountUpdateService = _GLAccountUpdateService ?? new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), poco.Tenant);
            var acc = new Def.EntityPMs.GLAccountPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = poco.Tenant,
                AccountTypeCode = poco.AccountTypeCode,
                ChartOfAccountsTypeCode = poco.ChartOfAccountsTypeCode,
                ChartOfAccountsId = poco.ChartOfAccountsId,
                ControlAccountId = //FullAccountingSetting.RevenueExpenseGLAccountId,
                    null,
                LocalName = poco.LocalName,
                EnglishName = poco.EnglishName,
                IsMultiCurrency = poco.IsMultiCurrency,
                IsControlAccount = poco.IsControlAccount,
                RevenueExpenseType = poco.RevenueExpenseType,
                DisplayNumber = _DisplayNumberProvider.GetDisplayNumber15CHAR(poco.ChartOfAccountsTypeCode, poco.IsControlAccount.GetValueOrDefault(), display, poco.Tenant),

            };
            _GLAccountUpdateService.FullAccountingProvider = true;
            _GLAccountUpdateService.Update(acc, true);
            return acc.Id;


            _GLAccountRepository = _GLAccountRepository ?? new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), poco.Tenant);

            poco.Id = IdCounter.GetNumber("GLAccount", poco.Tenant);
            poco.InternalNumber= CodeCounter.GetNumber("GLAccount", poco.Tenant).ToString();
            int curr = (++_Counter);
            poco.DisplayNumber = _DisplayNumberProvider.GetDisplayNumber15CHAR(poco.ChartOfAccountsTypeCode, true, curr, poco.Tenant);
            poco.SearchFields += poco.DisplayNumber;
            _GLAccountRepository.AddPocoFromBuildTenant(poco);

            _GLAccountMoreDataRepository = _GLAccountMoreDataRepository ?? new GLAccountMoreDataRepository(accountingContext);
            _GLAccountMoreDataRepository.Add(new GLAccountMoreData() { AccountId = poco.Id, Tenant = poco.Tenant });
            return poco.Id;
        }

     






        





        



        

        private string GetDefaultVATTypeId(int tenant)
        {
            //{"$id":"1","Id":"1-3047","Tenant":989,"IsSecured":true,"Code":"STD","EnglishName":"STD","LocalName":"STD","InActive":false,"Description":null,"LocalDescription":null,"ComputedLocalName":"STD","AddedManually":false,"SearchFields":"STD,STD,STD","NewEntityPercentage":null,"NewEntityPercentageDate":null,"ExternalVATCard":null,"ExternalTAXItemId":null,"ExternalVATCardExternalId":null,"ExternalTAXItemIdExternalId":null,"IsMultiPercentage":false,"VatTypePercentages":[{"$id":"2","Id":"1-2711","Tenant":989,"VatTypeId":"1-3047","FromDate":"2006-01-01T00:00:00","Percentage":17.0,"ChangeSetOp":0}],"VatTypeGroups":[]}

            if (DefaultVATTypeId != null)
            {
                return DefaultVATTypeId;
            }
            var vatTypeRep = new VatTypeRepository(tenant);
            var vatPoco = vatTypeRep.GetSingleVatTypeByCode("STD", tenant);
            DefaultVATTypeId = vatPoco.Id;
            return DefaultVATTypeId;
        }
    }

    public class DisplayNumberProvider: IDisplayNumberProvider
    {
        public int GetMaxDisplayNumberOfType(string chartOfAccountsTypeCode,  int tenant)
        {
            throw new Exception(@"use  int iMTKR = CodeCounter.GetNumber(/*DummyTenantProvider*/ ""DummyTP: "" + ""MTKR"", fullAccountingSetting.Tenant);");

            
            return 0;
            try
            {
                var qs = new GLAccountRepository(tenant);
                var max = qs.GetAll(tenant)
                    .Where(r => r.ChartOfAccountsTypeCode == chartOfAccountsTypeCode)
                    .Select(r => r.DisplayNumber.ToString().Substring(6))
                    .Max(r => Convert.ToInt32(r));
                return max;
            }
            catch (Exception)
            {

                return 0;
            }

            
            
        }

        public String GetDisplayNumber15CHAR(string chartOfAccountsTypeCode, bool isControl, int currentCounter, int tenant)
        {
            var tenant4Digit = new string('0', 4) + tenant;
            tenant4Digit = tenant4Digit.Substring(tenant4Digit.Length - 4);

            var miniMe = new string('0', 9) + currentCounter.ToString();
            miniMe = miniMe.Substring(miniMe.Length - 9);


            string res = null;
            var myType = "";
            switch (chartOfAccountsTypeCode)
            {
                case "SPDR":
                    myType ="JR";//2
                    break;
                case "SPDI":
                    myType = "JI";//2
                    break;
                case "MTKI":
                    myType = "FI";//2
                    break;

                case "MTKR":
                    myType = "FR";//2
                    break;

                default:
                    myType =
                        chartOfAccountsTypeCode + (isControl ? "1" : "0");//2
                    break;
            }

            res =
    tenant4Digit +//4
    myType +//2
    miniMe;//9

            if (res.Length != 15)
            {
                throw new Exception("(res.Length != 15)");
            }
            return res;
        }

    }

    public class ChartOfAccountProvider: IChartOfAccountProvider
    {
        private List<ChartOfAccount> _COALIst;
        public ChartOfAccountProvider()
        {

        }
        public void CreateCOA(IAccountingContext accountingContext, int tenant)
        {

            _COALIst = new List<ChartOfAccount>();
            _COALIst.Add(
            new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.Revenues.ToIntString(),
                LocalName = "הכנסות",
                EnglishName = "Revenues",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.Revenues.ToIntString(),
                Inactive = false,
                SearchFields = "1,Revenues,הכנסות",

            });
            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.Expenses.ToIntString(),
                LocalName = "הוצאות",
                EnglishName = "Expenses",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.Expenses.ToIntString(),
                Inactive = false,
                SearchFields = "2,Expenses,הוצאות",

            });

            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.Customers.ToIntString(),//"3",
                LocalName = "לקוחות",
                EnglishName = "Customers",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.Customers.ToIntString(),//"3",
                Inactive = false,
                SearchFields = "3,Customers,לקוחות",

            });
            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.Vendors.ToIntString(),//"4",
                LocalName = "ספקים",
                EnglishName = "Vendors",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.Vendors.ToIntString(),//"4",
                Inactive = false,
                SearchFields = "4,Vendors,ספקים",

            });

            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.Banks.ToIntString(),
                LocalName = "בנקים",
                EnglishName = "Banks",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.Banks.ToIntString(),
                Inactive = false,
                SearchFields = "5,Banks,בנקים",

            });
            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                LocalName = "תיקים וספדים",
                EnglishName = "Files&jobs",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.WorkersIsAlsoWorkAsProject.ToIntString(),// "6",
                Inactive = false,
                SearchFields = "6,Files&jobs,תיקים וספדים",

            });

            _COALIst.Add(new ChartOfAccount()
            {
                Id = IdCounter.GetNumber("ChartOfAccount", tenant),
                Tenant = tenant,
                Code = ChartOfAccountsTypeEnum.DebtorsAndCreditors.ToIntString(),
                LocalName = "חייבים ונושים",
                EnglishName = "Debtors&Creditors",
                ParentId = null,
                TypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors.ToIntString(),
                Inactive = false,
                SearchFields = "7,Debtors&Creditors,חייבים ונושים",

            });
            //var chartOfAccountUS = new ChartOfAccountUpdateService(tenant);
            // no one use UpdateService Using Repo
            var chartOfAccountRepo = new ChartOfAccountRepository(accountingContext);
            foreach (var item in _COALIst)
            {
                chartOfAccountRepo.Add(item);
            }
            chartOfAccountRepo.SubmitChanges();
        }


        public string GetChartOfAccountsId(int tenant, ChartOfAccountsTypeEnum chartOfAccountsTypeCode)
        {
            _COALIst = _COALIst ??GetParentFromDB(tenant);
            var pm = _COALIst.First(r => r.TypeCode == chartOfAccountsTypeCode.ToIntString());
            return pm.Id;
        }

        private List<ChartOfAccount> GetParentFromDB(int tenant)
        {
            var repo = new ChartOfAccountRepository(tenant);
            var list=repo.GetMainParent(tenant);
            return list;
        }
    }
}
namespace Logitude.Accounting.BL
{
    public enum ChartOfAccountsTypeEnum
    {
        Revenues = 1,
        Expenses = 2,
        Customers = 3,
        Vendors = 4,
        Banks = 5,
        Workers = 6,
        WorkersIsAlsoWorkAsProject = 6,
        DebtorsAndCreditors = 7//חייבים ונושים
    }

    public enum PaymentChequeStatuses
    {
        InCashbook = 1,
        InBank = 2,
        InBankAccount = 3,
        ReturnedFromBank = 4,
        ReturnedToCustomer = 5,
        Redeemed = 6,
        CashbookedReturnedFromTheBank = 7
    }

    public enum GLAccountTypeEnum
    {
        Card = 1,
        Client = 2,
        Vendor = 3,
        Job = 4,
        File = 5
    }
    public enum RevenueExpenseTypeEnum
    {
        Revenue = 1,
        Expense = 2,
        Other = 3
    }
    static class Ext
    {

        public static string ToIntString(this GLAccountTypeEnum @this)
        {
            return ((int)@this).ToString();
        }
        public static string ToIntString(this RevenueExpenseTypeEnum @this)
        {
            return ((int)@this).ToString();
        }


        public static string ToIntString(this ChartOfAccountsTypeEnum @this)
        {
            return ((int)@this).ToString();
        }

    }
    public class BuildAccountingTenantParam
    {
        //public bool OtherAccounts { get; set; }
        public bool VatAccounts { get; set; }
        public bool ControlAccounts { get; set; }
        public bool ExchangeRateDiff { get; set; }
        public bool RevenueExpense { get; set; }
        public bool TaxWithholding { get; set; }


        public string ControlAccountId { get; set; }
        public string ChartOfAccountsId { get; set; }

        //public bool CreateCustomerControlAccountId { get; set; }
        //public bool CreateVendorControlAccountId { get; set; }
        //public bool CreateFileControlAccountId { get; set; }
        //public bool CreateOceanExportJobControlAccountId { get; set; }
        //public bool CreateOceanImportJobControlAccountId { get; set; }
        //public bool CreateAirImportJobControlAccountId { get; set; }
        public bool CheckAndInsertPoco { get; set; }
    }
}