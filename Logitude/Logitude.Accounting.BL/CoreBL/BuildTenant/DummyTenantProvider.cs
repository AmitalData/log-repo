using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant
{
    public class DummyTenantProvider
    {
        private int _BuildJournalEachMonth = 200;//20000
        private List<TotalPerM> _TotalPerChartOfAccountsId;

        public void GenrateGLAccount(
            //int BuildGLAccountEachType,
            DummyTenantProviderArg dummyTenantProviderArg,
            IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider,
            FullAccountingSetting fullSetting, int tenant)
        {




            fullSetting = fullSetting ?? GetFullSetting(accountingContext, tenant);

            CreateGLAccount(accountingContext, chartOfAccountProvider, displayNumberProvider, fullSetting, dummyTenantProviderArg);


        }

        private FullAccountingSetting GetFullSetting(IAccountingContext accountingContext, int tenant)
        {
            var repo = new FullAccountingSettingRepository(accountingContext);
            var poco = repo.GetSingleFullAccountingSetting(tenant);
            return poco;
        }

        public void GenrateJournals(
            int BuildJournalEachMonth,
            IAccountingContext accountingContext, int forYear, int tenant)
        {
            _BuildJournalEachMonth = BuildJournalEachMonth;

            //int forYear = 2017;

            Stopwatch sw = null;
            var typeregular = "1"; //1	Regular	רגיל	1,Regular,רגיל	0
            var accountingPeriodQueryService = new AccountingPeriodQueryService(tenant);
            var accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodByType(typeregular, tenant); ;

            var journalTesterClass = new JournalTesterClass();
            journalTesterClass.ForceRecreate();
            int count = 0;
            for (int MM = 1; MM <= 12; MM++)
            {
                if (!JournalValidatorNotStatic.IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), new DateTime(forYear, MM, 1)))
                {
                    continue;
                }
                for (int i = 0; i < BuildJournalEachMonth; i++)
                {
                    try
                    {
                        


                        sw = Stopwatch.StartNew();
                        var id = journalTesterClass.InsertRandomJournal(accountingContext, tenant, forYear, MM);
                        Debug.WriteLine("create journal " + id.ToString() + " TOOK:" + sw.Elapsed.ToString());
                        count++;

                    }
                    catch (Exception eee)
                    {
                        LogitudeSettings.HandleLogMe($"BuildJournalEachMonth({i})"  +eee.ToString(), true, "AccLoadTest", new DateTime(2019, 5, 1));
                        //throw;
                    }
                    Thread.Sleep(10);
                    //if (count % 1000 == 0)
                    //{
                    //    accountingContext.SaveChanges();
                    //}
                }
            }


        }


        private void CreateJournalPerMonthOld(IAccountingContext accountingContext, DisplayNumberProvider displayNumberProvider, FullAccountingSetting FullAccountingSetting, int forYear, int totGornalPerMonth)
        {



            var glRepo = new GLAccountRepository(accountingContext);
            var allGlAcc = glRepo.GetQChildAccountsByChartOfAccountIdList(
                new List<string>() {
                    ChartOfAccountsTypeEnum.Revenues.ToIntString() , ChartOfAccountsTypeEnum.Expenses.ToIntString(),
                    ChartOfAccountsTypeEnum.Customers.ToIntString(),
                    ChartOfAccountsTypeEnum.Vendors.ToIntString()
                }, FullAccountingSetting.Tenant)
                .Where(r => r.Inactive == false)
                .Select(r =>
                new
                {

                    r.Id,
                    r.ChartOfAccountsId,
                    r.IsControlAccount,


                }).ToList();

            var glAccList = allGlAcc.Select(r => new GLAccountPM() { Id = r.Id, ChartOfAccountsId = r.ChartOfAccountsId, IsControlAccount = r.IsControlAccount }).ToList();
            var RevenuesCount = glAccList.Where(r => r.IsControlAccount == false && r.ChartOfAccountsId == ChartOfAccountsTypeEnum.Revenues.ToIntString()).Count();

            var ExpensesCount = glAccList.Where(r => r.IsControlAccount == false && r.ChartOfAccountsId == ChartOfAccountsTypeEnum.Expenses.ToIntString()).Count();

            var CustomersCount = glAccList.Where(r => r.IsControlAccount == false && r.ChartOfAccountsId == ChartOfAccountsTypeEnum.Customers.ToIntString()).Count();


            var VendorsCount = glAccList.Where(r => r.IsControlAccount == false && r.ChartOfAccountsId == ChartOfAccountsTypeEnum.Vendors.ToIntString()).Count();


            CreateJournalLineDueDateMustBeMoreThenAccountingDate();



        }




        private void CreateJournalLineDueDateMustBeMoreThenAccountingDate()
        {
            throw new NotImplementedException();
        }

        private void CreateGLAccount(IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting,
            DummyTenantProviderArg dummyTenantProviderArg//, int BuildGLAccountEachType
            )
        {


            _TotalPerChartOfAccountsId = accountingContext.GLAccounts
                .Where(r => r.Tenant == fullAccountingSetting.Tenant)
                //.Where(r => r.AccountTypeCode == GLAccountTypeEnum.Job.ToIntString())
                .GroupBy(r => r.ChartOfAccountsId)
                .Select(g => new TotalPerM() { Key = g.Key, Value = g.Count() })
                .ToList();


            //bool fixSize = false;
            //TimeSpan? ts = TimeSpan.FromMinutes(30);
            //if (BuildGLAccountEachType > 1000)
            //{
            //    ts = TimeSpan.FromHours(1);
            //}
            //if (fixSize) BuildGLAccountEachType = 100000;
            if (dummyTenantProviderArg.CreateJobs>0)
            {
                CreateJobs(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateJobs);
                accountingContext.SaveChanges();
            }
            




            //using (var trans = TransactionFactory.GetTransaction(ts))
            {
                //if (fixSize) BuildGLAccountEachType = 20000;
                if (dummyTenantProviderArg.CreateCustomers > 0)
                {
                    CreateCustomers(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateCustomers);
                    accountingContext.SaveChanges();
                }

                //  trans.Complete();
            }

            if (dummyTenantProviderArg.CreateFiles > 0)
            {


                //if (fixSize) BuildGLAccountEachType = 300000;
                CreateFiles(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateFiles);
                accountingContext.SaveChanges();
            }
            if (dummyTenantProviderArg.CreateVendors > 0)
            //using (var trans = TransactionFactory.GetTransaction(ts))
            {
                //if (fixSize) BuildGLAccountEachType = 20000;
                CreateVendors(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateVendors);
                accountingContext.SaveChanges();
                //  trans.Complete();
            }
            if (dummyTenantProviderArg.CreateExpanse > 0)
            ///using (var trans = TransactionFactory.GetTransaction(ts))
            {
                //if (fixSize) BuildGLAccountEachType = 2000;
                CreateExpanse(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateExpanse);
                accountingContext.SaveChanges();
                // trans.Complete();
            }
            //using (var trans = TransactionFactory.GetTransaction(ts))
            if (dummyTenantProviderArg.CreateRevenue > 0)
            {
                //if (fixSize) BuildGLAccountEachType = 2000;
                CreateRevenue(accountingContext, chartOfAccountProvider, displayNumberProvider, fullAccountingSetting, dummyTenantProviderArg.CreateRevenue);

                accountingContext.SaveChanges();
                //  trans.Complete();
            }








        }

        private void CreateFiles(IAccountingContext accountingContext, ChartOfAccountProvider chartOfAccountProvider, DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int buildGLAccountEachType)
        {


            var myJournalTesterClass = new JournalTesterClass();
            var us = new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), fullAccountingSetting.Tenant);
            //for (int i =
            //    //displayNumberProvider
            //    //.GetMaxDisplayNumberOfType("MTKR", fullAccountingSetting.Tenant)
            //    iMTKR
            //    ; i < buildGLAccountEachType; i++)
            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Workers.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;

            while (tot++ < buildGLAccountEachType)
            {


                int iMTKR = (new  CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + "MTKR", fullAccountingSetting.Tenant);


                us.Update(new Def.EntityPMs.GLAccountPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = fullAccountingSetting.Tenant,
                    AccountTypeCode = GLAccountTypeEnum.File.ToIntString(),
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Workers.ToIntString(),
                    ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Workers),
                    ControlAccountId = fullAccountingSetting.FileControlAccountId,
                    LocalName = "תיק " + " " + iMTKR.ToString(),
                    EnglishName = "FILE" + " " + iMTKR.ToString(),
                    IsMultiCurrency = true,
                    IsControlAccount = false,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),
                    DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR("MTKR", false, iMTKR, fullAccountingSetting.Tenant),
                    CustomerGLAccountId = myJournalTesterClass.GetClientCard(accountingContext, fullAccountingSetting.Tenant)

                    // = FullAccountingSetting.CustomerControlAccountName + " " + i.ToString(),
                    //AccountTypeCode 

                }, false);
                if (iMTKR % 500 == 0)
                {
                    accountingContext.SaveChanges();

                }
            }
            accountingContext.SaveChanges();
        }

        private void CreateJobs(IAccountingContext accountingContext, ChartOfAccountProvider chartOfAccountProvider, DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int buildGLAccountEachType)
        {


            var us = new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), fullAccountingSetting.Tenant);
            //for (int i =
            //    //displayNumberProvider
            //    //.GetMaxDisplayNumberOfType("SPDR", fullAccountingSetting.Tenant)
            //    ; i < buildGLAccountEachType; i++)
            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Workers.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;




            while (tot++ < buildGLAccountEachType)
            {
                int iSPDR = (new  CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + "SPDR", fullAccountingSetting.Tenant);


                us.Update(new Def.EntityPMs.GLAccountPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = fullAccountingSetting.Tenant,
                    AccountTypeCode = GLAccountTypeEnum.Job.ToIntString(),
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Workers.ToIntString(),
                    ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Workers),
                    ControlAccountId = fullAccountingSetting.OceanImportJobControlAccountId,
                    LocalName = "ספד" + " " + iSPDR.ToString(),
                    EnglishName = "Job" + " " + iSPDR.ToString(),
                    IsMultiCurrency = true,
                    IsControlAccount = false,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),
                    DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR("SPDR", false, iSPDR, fullAccountingSetting.Tenant),


                    // = FullAccountingSetting.CustomerControlAccountName + " " + i.ToString(),
                    //AccountTypeCode 

                }, false);
                if (iSPDR % 500 == 0)
                {
                    accountingContext.SaveChanges();

                }
            }
            accountingContext.SaveChanges();
        }

        private void CreateRevenue(IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int times)
        {
            var timeLimit = times;
            if (timeLimit > 2000)
            {
                timeLimit = 2000;
            }

            var us = new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), fullAccountingSetting.Tenant);
            //for (int i =
            //    //displayNumberProvider
            //    //.GetMaxDisplayNumberOfType(ChartOfAccountsTypeEnum.Revenues.ToIntString(), fullAccountingSetting.Tenant) 
            //    iRevenues
            //    ; i < timeLimit; i++)


            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Revenues.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;


            while (tot++ < timeLimit)
            {

                int iRevenues = (new  CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + ChartOfAccountsTypeEnum.Revenues.ToIntString(), fullAccountingSetting.Tenant);
                us.Update(new Def.EntityPMs.GLAccountPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = fullAccountingSetting.Tenant,
                    AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Revenues.ToIntString(),
                    ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Revenues),
                    ControlAccountId = //FullAccountingSetting.RevenueExpenseGLAccountId,
                    null,
                    LocalName = "הכנסות" + " " + iRevenues.ToString(),
                    EnglishName = "Revenue" + " " + iRevenues.ToString(),
                    IsMultiCurrency = true,
                    IsControlAccount = false,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Revenue.ToIntString(),
                    DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR(ChartOfAccountsTypeEnum.Revenues.ToIntString(), false, iRevenues, fullAccountingSetting.Tenant),

                }, false);
                if (iRevenues % 100 == 0)
                {
                    accountingContext.SaveChanges();
                }
            }
            accountingContext.SaveChanges();
        }

        private void CreateExpanse(IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int times)
        {
            var timeLimit = times;
            if (timeLimit > 2000)
            {
                timeLimit = 2000;
            }

            var us = new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), fullAccountingSetting.Tenant);
            //for (int i =
            //    //displayNumberProvider
            //    //.GetMaxDisplayNumberOfType(ChartOfAccountsTypeEnum.Expenses.ToIntString(), fullAccountingSetting.Tenant)
            //    iExpenses
            //    ; i < timeLimit; i++)

            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Expenses.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;

            while (tot++ < timeLimit)
            {
                int iExpenses =
            (new  CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + ChartOfAccountsTypeEnum.Expenses.ToIntString(), fullAccountingSetting.Tenant);


                us.Update(new Def.EntityPMs.GLAccountPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Tenant = fullAccountingSetting.Tenant,
                    AccountTypeCode = GLAccountTypeEnum.Card.ToIntString(),
                    ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Expenses.ToIntString(),
                    ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Expenses),
                    ControlAccountId = //FullAccountingSetting.RevenueExpenseGLAccountId,
                    null,
                    LocalName = "הוצאות" + " " + iExpenses.ToString(),
                    EnglishName = "Expense" + " " + iExpenses.ToString(),
                    IsMultiCurrency = true,
                    IsControlAccount = false,
                    RevenueExpenseType = RevenueExpenseTypeEnum.Expense.ToIntString(),
                    DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR(ChartOfAccountsTypeEnum.Expenses.ToIntString(), false, iExpenses, fullAccountingSetting.Tenant),

                }, false);
                if (iExpenses % 100 == 0)
                {
                    accountingContext.SaveChanges();
                }
            }
            accountingContext.SaveChanges();
        }

        private void CreateCustomers(IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int times)
        {
            GLAccountUpdateService us = GetGLAccountUpdateService(accountingContext, fullAccountingSetting);
            //for (int i =
            //    displayNumberProvider
            //    .GetMaxDisplayNumberOfType(ChartOfAccountsTypeEnum.Customers.ToIntString(), fullAccountingSetting.Tenant)
            //    ; i < times; i++)
            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Customers.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;
            if (Amount2addMore)
            {
                tot = 0;
            }
            while (tot++ < times)
            {
                using (var scope = TransactionFactory.GetNewTransaction())
                {

                    int iClient = (new CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + GLAccountTypeEnum.Client.ToIntString(), fullAccountingSetting.Tenant);
                    us.Update(new Def.EntityPMs.GLAccountPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = fullAccountingSetting.Tenant,
                        AccountTypeCode = GLAccountTypeEnum.Client.ToIntString(),
                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Customers.ToIntString(),
                        ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Customers),
                        ControlAccountId = fullAccountingSetting.CustomerControlAccountId,
                        LocalName = "לקוחות" + " " + iClient.ToString(),
                        EnglishName = "Customer" + " " + iClient.ToString(),
                        IsMultiCurrency = true,
                        IsControlAccount = false,
                        RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),
                        DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR(ChartOfAccountsTypeEnum.Customers.ToIntString(), false, iClient, fullAccountingSetting.Tenant),


                        // = FullAccountingSetting.CustomerControlAccountName + " " + i.ToString(),
                        //AccountTypeCode 

                    }, true);
                    scope.Complete();
                }

            }

        }
        int _CountGLAccountUpdateService = 0;
        private GLAccountUpdateService GetGLAccountUpdateService(IAccountingContext accountingContext, FullAccountingSetting fullAccountingSetting)
        {
            if (_CountGLAccountUpdateService > 100)
            {
                accountingContext = AccountingContext.GetContext(fullAccountingSetting.Tenant);
                _CountGLAccountUpdateService = 0;
            }
            _CountGLAccountUpdateService++;
            return new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), fullAccountingSetting.Tenant);
        }

        bool amount2addMore = false;
        public bool Amount2addMore
        {
            get
            {
                return amount2addMore;
            }
            set
            {
                amount2addMore = value;
            }
        }
        //public bool Amount2addMore { get => amount2addMore; set => amount2addMore = value; } // Wrong Format For JEnkins

        private void CreateVendors(IAccountingContext accountingContext,
            ChartOfAccountProvider chartOfAccountProvider,
            DisplayNumberProvider displayNumberProvider, FullAccountingSetting fullAccountingSetting, int amount)
        {
            var us = //new GLAccountUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), 
                GetGLAccountUpdateService(accountingContext, fullAccountingSetting);
            //for (int i = displayNumberProvider
            //    .GetMaxDisplayNumberOfType(ChartOfAccountsTypeEnum.Customers.ToIntString(), fullAccountingSetting.Tenant)
            //    ; i < times; i++)
            int tot = _TotalPerChartOfAccountsId.Where(r => r.Key == ChartOfAccountsTypeEnum.Vendors.ToIntString()).DefaultIfEmpty(new TotalPerM()).First().Value;
            if (Amount2addMore)
            {
                tot = 0;
            }
            while (tot++ < amount)
            {

                int iVendors =  (new  CodeCounterWrapper(true)).GetNumber(/*DummyTenantProvider*/ "DummyTP:" + ChartOfAccountsTypeEnum.Vendors.ToIntString(), fullAccountingSetting.Tenant);
                using (var scope = TransactionFactory.GetNewTransaction())
                {

                    us.Update(new Def.EntityPMs.GLAccountPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = fullAccountingSetting.Tenant,
                        AccountTypeCode = GLAccountTypeEnum.Vendor.ToIntString(),
                        ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.Vendors.ToIntString(),
                        ChartOfAccountsId = chartOfAccountProvider.GetChartOfAccountsId(fullAccountingSetting.Tenant, ChartOfAccountsTypeEnum.Vendors),
                        ControlAccountId = fullAccountingSetting.CustomerControlAccountId,
                        LocalName = "ספקים" + " " + iVendors.ToString(),
                        EnglishName = "Vendors" + " " + iVendors.ToString(),
                        IsMultiCurrency = true,
                        IsControlAccount = false,
                        RevenueExpenseType = RevenueExpenseTypeEnum.Other.ToIntString(),
                        DisplayNumber = displayNumberProvider.GetDisplayNumber15CHAR(ChartOfAccountsTypeEnum.Vendors.ToIntString(), false, iVendors, fullAccountingSetting.Tenant),


                        // = FullAccountingSetting.CustomerControlAccountName + " " + i.ToString(),
                        //AccountTypeCode 

                    }, true);
                    //if (iVendors % 100 == 0)
                    //{
                    //    accountingContext.SaveChanges();
                    //}
                    scope.Complete();
                }
            }
            accountingContext.SaveChanges();
        }
    }



    class JournalTesterClass
    {
        static Random _Rand = new Random(DateTime.Now.TimeOfDay.TotalMinutes.GetHashCode());


        //static Hashtable _
        List<string> _Client = null;
        List<string> _Vendor;
        List<string> _Revenue;
        List<string> _Expense { get; set; }
        List<string> _Jobs;
        List<string> _Files;

        public string GetClientCard(IAccountingContext accountingContext, int tenant)
        {
            InitStatic(accountingContext, tenant);
            if (_Client == null)
            {
                if (true)
                {
                    _Client = _GLAccList
                        .Where(r => r.ChartOfAccountsTypeCode == ChartOfAccountsTypeEnum.Customers.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }
                else
                {
                    //var qs = new GLAccountQueryService(tenant);
                    //2	לקוח	Client	0
                    var repo = new GLAccountRepository(accountingContext);

                    _Client = repo.GetGLAccountId(tenant, ((int)GLAccountTypePM.GLAccountTypeEnum.Client).ToString(), "", 100);
                }


            }
            if (_Client.Count < 1)
            {
                throw new Exception("if (_Client.Count < 1)");
            }
            var pos = _Rand.Next(_Client.Count);
            return _Client.ElementAt(pos);
        }
        public string GetVendorCard(IAccountingContext accountingContext, int tenant)
        {
            if (_Vendor == null)
            {

                if (true)
                {
                    _Vendor = _GLAccList
                        .Where(r => r.ChartOfAccountsTypeCode == ChartOfAccountsTypeEnum.Vendors.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }
                else
                {
                    //var qs = new GLAccountQueryService(tenant);
                    //2	לקוח	Client	0
                    var repo = new GLAccountRepository(accountingContext);

                    _Vendor = repo.GetGLAccountId(tenant, ((int)GLAccountTypePM.GLAccountTypeEnum.Vendor).ToString(), "", 100);
                }
            }
            if (_Vendor.Count < 1)
            {
                throw new Exception("if (_Vendor.Count < 1)");
            }
            var pos = _Rand.Next(_Vendor.Count);
            return _Vendor.ElementAt(pos);
        }
        public string GetExpenseCard(IAccountingContext accountingContext, int tenant)
        {
            if (_Expense == null)
            {
                if (true)
                {
                    _Expense = _GLAccList
                        .Where(r => r.ChartOfAccountsTypeCode == ChartOfAccountsTypeEnum.Expenses.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }
                else
                {
                    //var qs = new GLAccountQueryService(tenant);
                    //2	לקוח	Client	0
                    var repo = new GLAccountRepository(accountingContext);

                    _Expense = repo.GetGLAccountId(tenant, "", ((int)RevenueExpenseTypePM.RevenueExpenseTypeEnum.Expense).ToString(), 100);
                }
            }
            if (_Expense.Count < 1)
            {
                throw new Exception("if (_Expense.Count < 1)");
            }
            var pos = _Rand.Next(_Expense.Count);
            return _Expense.ElementAt(pos);
        }

        public string GetRevenueCard(IAccountingContext accountingContext, int tenant)
        {
            if (_Revenue == null)
            {
                if (true)
                {
                    _Revenue = _GLAccList
                        .Where(r => r.ChartOfAccountsTypeCode == ChartOfAccountsTypeEnum.Revenues.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }
                else
                {
                    //var qs = new GLAccountQueryService(tenant);
                    //2	לקוח	Client	0
                    var repo = new GLAccountRepository(accountingContext);

                    _Revenue = repo.GetGLAccountId(tenant, "", ((int)RevenueExpenseTypePM.RevenueExpenseTypeEnum.Revenue).ToString(), 100);
                }
            }
            if (_Revenue.Count < 1)
            {
                throw new Exception("if (_Revenue.Count < 1)");
            }
            var pos = _Rand.Next(_Revenue.Count);
            return _Revenue.ElementAt(pos);
        }
        public enum JLCreditDebitVatProfile : int
        {
            CreditDebitInTwoLine = 1,
            CreditDebitOneLine = 2,
            DebitCreditAndVatdeduction = 3

        }
        enum JournalLineDebitCredit : int
        {
            VendorClient = 1,
            VendorJob,
            JobClient,
            FileJob,
            JobFile,
            RevExpense,

        }

        public string InsertRandomJournal(IAccountingContext accountingContext, int tenant, int YYYY, int MM)
        {


            InitStatic(accountingContext, tenant);
            //int tenant = FullAccountingSetting.Tenant;
            var email = AuthenticationUtil.ResolveLoggingUserId(tenant);
            var contactRepository = new ContactRepository(tenant);
            var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(tenant), tenant);
            var UpdatedByUserId = loggedContact.Id;
            var row = _Rand.Next(1, 3);
            var forMonth = new DateTime(YYYY, MM, 1);
            var accDate1 = forMonth.AddDays(_Rand.Next(1, 28));
            var dueDate = forMonth.AddDays(_Rand.Next(1, 28));

            bool f = true;

            Logitude.Accounting.Def.EntityPMs.JournalPM j = null;

            j = new Logitude.Accounting.Def.EntityPMs.JournalPM()
            {
                AccountingDate = accDate1,
                Tenant = tenant,
                //JournalNumber = "1003",
                //StatusCode = "2",
                StatusCodeEnum = Logitude.Accounting.Def.EntityPMs.JournalStatusTypePM.StatusCodeEnum.Approved,
                UpdatedByUserId = UpdatedByUserId,
                UpdateDate = DateTime.Now,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                CreateDate = DateTime.Now,
                CreatedByUserId = UpdatedByUserId,
            };
            bool typeVendorClientOrRevExpense = true;
            j.JournalLines = new List<JournalLinePM>();
            string creditCard = "";
            string debitCard = "";
            int lineId = 0;
            for (int i = 0; i < row; i++)
            {
                JournalLineDebitCredit rendomJournalLineDebitCredit =
                    //(JournalLineDebitCredit)_Rand.Next(1, 6);
                    (JournalLineDebitCredit)Enum.ToObject(typeof(JournalLineDebitCredit), _Rand.Next(1, 7));


                switch (rendomJournalLineDebitCredit)
                {
                    case JournalLineDebitCredit.VendorClient:
                        creditCard = GetClientCard(accountingContext, tenant);
                        debitCard = GetVendorCard(accountingContext, tenant);
                        break;
                    case JournalLineDebitCredit.VendorJob:
                        debitCard = GetVendorCard(accountingContext, tenant);
                        creditCard = GetJob(accountingContext, tenant);
                        break;
                    case JournalLineDebitCredit.JobClient:
                        creditCard = GetClientCard(accountingContext, tenant);
                        debitCard = GetJob(accountingContext, tenant);
                        break;
                    case JournalLineDebitCredit.FileJob:
                        creditCard = GetFile(accountingContext, tenant);
                        debitCard = GetJob(accountingContext, tenant);

                        break;
                    case JournalLineDebitCredit.JobFile:
                        creditCard = GetJob(accountingContext, tenant);
                        debitCard = GetFile(accountingContext, tenant);

                        break;
                    case JournalLineDebitCredit.RevExpense:
                        creditCard = GetRevenueCard(accountingContext, tenant);
                        debitCard = GetExpenseCard(accountingContext, tenant);

                        break;

                }

                DateTime DocumentDate = DateTime.Now.AddDays(_Rand.Next(-600, 180));
                DateTime DueDate = dueDate;//DateTime.Now.AddDays(_Rand.Next(-600, 180));
                decimal localAm = _Rand.Next(201, 5000000) / 100m;
                decimal foreignAm = localAm / 3 + ((int)accDate1.Subtract(forMonth).TotalDays / 100);
                
                //Task 48370: Transactions service -Load Test & Performance - change logic - Create only journal lines 1 - credit or - 2 - debit
                //JLCreditDebitVatProfile journalLinesProfile = (JLCreditDebitVatProfile)Enum.ToObject(typeof(JLCreditDebitVatProfile), _Rand.Next(4));
                JLCreditDebitVatProfile journalLinesProfile = JLCreditDebitVatProfile.CreditDebitInTwoLine;
                switch (journalLinesProfile)
                {
                    case JLCreditDebitVatProfile.CreditDebitOneLine:
                        j.JournalLines.Add(new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                        {
                            AccountingDate = j.AccountingDate,
                            //ActionName = "4", 
                            ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitAndCredit,
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            CreditAccountId = creditCard,
                            DebitAccountId = debitCard,
                            DocumentDate = DocumentDate,
                            DueDate = DueDate,

                            ForeignAmount = foreignAm,
                            LocalAmount = localAm,
                            CurrencyId = GetCurrencyId(tenant, "USD"),

                            JournalId = j.Id,
                            Line = lineId++,
                            Tenant = tenant
                        });
                        break;
                    case JLCreditDebitVatProfile.DebitCreditAndVatdeduction:
                        j.JournalLines.Add(new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                        {
                            AccountingDate = j.AccountingDate,
                            //ActionName = "4", 
                            ActionTypeCodeEnum = MyJournalActionTypeEnum.DebitCreditAndVatdeduction,
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            CreditAccountId = creditCard,
                            DebitAccountId = debitCard,
                            DocumentDate = DocumentDate,
                            DueDate = DueDate,

                            ForeignAmount = foreignAm,
                            LocalAmount = localAm,
                            CurrencyId = GetCurrencyId(tenant, "USD"),

                            JournalId = j.Id,
                            Line = lineId++,
                            Tenant = tenant
                        });
                        break;
                    case JLCreditDebitVatProfile.CreditDebitInTwoLine:
                    default:

                        j.JournalLines.Add(new JournalLinePM()
                        {
                            AccountingDate = j.AccountingDate,
                            //ActionName = "1", 
                            ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            CreditAccountId = creditCard,
                            DebitAccountId = null,

                            DocumentDate = DocumentDate,
                            DueDate = DueDate,

                            ForeignAmount = foreignAm,
                            LocalAmount = localAm,
                            CurrencyId = GetCurrencyId(tenant, "USD"),

                            JournalId = j.Id,
                            Line = lineId++,
                            Tenant = tenant

                        });
                        j.JournalLines.Add(
                           new Logitude.Accounting.Def.EntityPMs.JournalLinePM
                           {
                               AccountingDate = j.AccountingDate,
                               //ActionName = "2", 
                               ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                               ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                               //CreditAccountId = "35", 

                               CreditAccountId = null,
                               DebitAccountId = debitCard,
                               DocumentDate = DocumentDate,
                               DueDate = DueDate,
                               ForeignAmount = foreignAm,
                               LocalAmount = localAm,
                               CurrencyId = GetCurrencyId(tenant, "USD"),
                               JournalId = j.Id,
                               Line = 2,
                               Tenant = 989
                           });
                        break;
                }
                typeVendorClientOrRevExpense = !typeVendorClientOrRevExpense;


            }

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var accountingContext1 = AccountingContext.GetContext(tenant);
                var logger = (accountingContext1 as DbContextBase).CreateLogger();
                var us = new Logitude.Accounting.BL.EntityUpdateServices.JournalUpdateService(accountingContext1, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                try
                {
                    us.Update(j, true);
                }
                catch (Exception eee)
                {
                    //logger.ToString();
                    ///LogitudeSettings.HandleLogMe(eee.ToString(), true, "AccLoadTest", new DateTime(2019, 5, 1));
                    bool @throw = true;
                    if (@throw)
                    {
                        throw;
                    }

                }
                finally
                {
                    logger.Dispose();
                }

                scope.Complete();
                return j.Id;
            }

        }

        private string GetJob(IAccountingContext accountingContext, int tenant)
        {
            if (_Jobs == null)
            {

                if (true)
                {
                    _Jobs = _GLAccList
                        .Where(r => r.AccountTypeCode == GLAccountTypeEnum.Job.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }

            }
            if (_Jobs.Count < 1)
            {
                throw new Exception("if (_Jobs.Count < 1)");
            }
            var pos = _Rand.Next(_Jobs.Count);
            return _Jobs.ElementAt(pos);
        }


        private string GetFile(IAccountingContext accountingContext, int tenant)
        {
            if (_Files == null)
            {

                if (true)
                {
                    _Files = _GLAccList
                        .Where(r => r.AccountTypeCode == GLAccountTypeEnum.File.ToIntString())
                        .Where(r => r.IsControlAccount == false)
                        .Select(r => r.Id).ToList();
                }

            }
            if (_Files.Count < 1)
            {
                throw new Exception("if (_Files.Count < 1)");
            }
            var pos = _Rand.Next(_Files.Count);
            return _Files.ElementAt(pos);
        }
        public void ForceRecreate()
        {
            _GLAccList = null;
        }
        private void InitStatic(IAccountingContext accountingContext, int tenant)
        {

            if (_GLAccList != null)
            {
                return;
            }
            var glRepo = new GLAccountRepository(accountingContext);
            var allGlAcc = glRepo.GetQAccountsByChartOfAccountsTypeCodeList(
                new List<string>() {
                    ChartOfAccountsTypeEnum.Revenues.ToIntString() , ChartOfAccountsTypeEnum.Expenses.ToIntString(),
                    ChartOfAccountsTypeEnum.Customers.ToIntString(),
                    ChartOfAccountsTypeEnum.Vendors.ToIntString(),
                    ChartOfAccountsTypeEnum.Workers.ToIntString()//

                }, tenant)
                .Where(r => r.Inactive == false)
                .Select(r =>
                new
                {

                    r.Id,
                    r.ChartOfAccountsTypeCode,
                    r.IsControlAccount,
                    r.AccountTypeCode

                }).ToList();
            _GLAccList = allGlAcc.Select(r => new GLAccountPM()
            {
                Id = r.Id,
                ChartOfAccountsTypeCode = r.ChartOfAccountsTypeCode,
                IsControlAccount = r.IsControlAccount,
                AccountTypeCode = r.AccountTypeCode
            }).ToList();
            return;

        }

        static List<CurrencyPM> _CurrencyList = new List<CurrencyPM>();

        private List<GLAccountPM> _GLAccList;

        private string GetCurrencyId(int tenant, string code)
        {
            var pmCache = _CurrencyList.Where(r => r.Code == code && r.Tenant == tenant).FirstOrDefault();
            if (pmCache != null)
            {
                return pmCache.Id;
            }
            var a = new CurrencyQuery(tenant);
            var pm = a.GetCurrencyByCodeOrName(code, null, tenant).First();
            _CurrencyList.Add(pm);
            return pm.Id;
        }


    }
    public class TotalPerM { public string Key { get; set; } public int Value { get; set; } }
    public class DummyTenantProviderArg
    {
        
        public int CreateJobs { get; set; }
        public int CreateCustomers { get; set; }
        public int CreateFiles { get; set; }
        public int CreateVendors { get; set; }
        public int CreateExpanse { get; set; }
        public int CreateRevenue { get; set; }
    }

}
