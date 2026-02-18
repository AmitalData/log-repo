using FakeItEasy;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
   
    [TestClass]
    public partial class LedgerTransactionBalanceServiceUnderTest
    {
        private int _MyTenant;
        private int _OtherTenant;
        private string _MainGLAccountIdTeva;
        private string _CurrencyIdUSD;
        private string _CurrencyIdNIS;
        private string _CurrencyIdEUR;
        private DateTime _StartDate;
        private string _GLAccountCurrencyTevaJPY;
        private string _CurrencyIdJPY;
        private string _MyGLAccountIdChildUSA;
        public LedgerTransactionBalanceServiceUnderTest()
        {
            _MyTenant = 1; _OtherTenant = 99;
            _MainGLAccountIdTeva = "TevaLTd";
            _GLAccountCurrencyTevaJPY = "TevaJPY";
            _MyGLAccountIdChildUSA = "TevaUSA";
            _CurrencyIdUSD = "USD";
            _CurrencyIdNIS = "NIS";
            _CurrencyIdEUR = "EUR";
            _CurrencyIdJPY = "JPY";
            _StartDate = new DateTime(2015, 12,01);

        }

        [TestMethod]
        public void Run_SearchFields()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var myTRans = myIAccountingContext.LedgerTransactions
                .Where(r => r.AccountId == _MainGLAccountIdTeva);
                //.Where(r => r.CurrencyId == _CurrencyIdEUR);
            foreach (var tran in myTRans)
            {
                var id = int.Parse(tran.Id);
                if (id % 2 == 0)
                {
                    tran.SearchFields = "My1st";
                }
                else
                {
                    tran.SearchFields = "My2nd";
                }
            }
                
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                //CurrencyId = _CurrencyIdEUR,
                SearchFields="My2nd",
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 10,
                PageStartAtRecordIndex = 0,
                

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            

            
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016, 09, 01), classUnderTest.Response.MaxCreateAt);



            //LocalAmountDebit =100, LocalAmountCredit=50, 

            Assert.IsNull(classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            Assert.IsNull(classUnderTest.Response.StartBalanceLocal);
            Assert.IsNull(classUnderTest.Response.EndBalanceLocal);




            Assert.IsNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.IsNull(classUnderTest.Response.EndBalanceForeignList);
            

            Assert.AreEqual(21, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


            Assert.AreEqual(10, classUnderTest.Response.MyLedgerTransactionList.Count());
            var my1stTrans = classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(my1stTrans);
            Assert.AreEqual("3", my1stTrans.Id);
            Assert.AreEqual(500m, my1stTrans.LocalAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);
            
            
            Assert.AreEqual(100m, my1stTrans.ForeignAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);


            Assert.AreEqual(0, my1stTrans.CumulativeLocalAmount);
            Assert.AreEqual(0, my1stTrans.CumulativeForeignAmount);


            

            var lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(lastTrans);
            Assert.AreEqual("21", lastTrans.Id);

            var list =classUnderTest.Response.MyLedgerTransactionList
                .Where(r => r.SearchFields != "My2nd" || r.CumulativeForeignAmount != 0m || r.CumulativeLocalAmount != 0m)
                .ToList();
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);

            
            //fltr.CallBack = fltr;
            fltr.CallBack = (classUnderTest.Response as LedgerTransactionBalanceFilterCallBack);
            fltr.PageStartAtRecordIndex = 10;
            classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            list = classUnderTest.Response.MyLedgerTransactionList
                .Where(r => r.SearchFields != "My2nd" || r.CumulativeForeignAmount != 0m || r.CumulativeLocalAmount != 0m)
                .ToList();
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
            my1stTrans = classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(my1stTrans);
            Assert.AreEqual("19", my1stTrans.Id);
        }

        [TestMethod]
        public void Run_BasicCurrencyIdEUR()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 10,
                PageStartAtRecordIndex = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);

            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016,09,01), classUnderTest.Response.MaxCreateAt);
              
                    
                    
            //LocalAmountDebit =100, LocalAmountCredit=50, 
            Assert.AreEqual(50m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(50m, classUnderTest.Response.EndBalanceLocal);                  
                    
            
            
            
            Assert.IsNotNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.AreEqual(1,classUnderTest.Response.StartBalanceForeignList.Count);
            //ForeignAmountDebit = 20m, ForeignAmountCredit = 10m,
            Assert.AreEqual(10m, classUnderTest.Response.StartBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.StartBalanceForeignList.First().CurrencyId);


            Assert.IsNotNull(classUnderTest.Response.EndBalanceForeignList);
            Assert.AreEqual(1, classUnderTest.Response.EndBalanceForeignList.Count);
            Assert.AreEqual(10m, classUnderTest.Response.EndBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.EndBalanceForeignList.First().CurrencyId);
            

            Assert.AreEqual(14, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


            Assert.AreEqual(10, classUnderTest.Response.MyLedgerTransactionList.Count());
            var my1stTrans =classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(my1stTrans);
            Assert.AreEqual("3", my1stTrans.Id);
            Assert.AreEqual(500m, my1stTrans.LocalAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);
            //50 classUnderTest.Response.StartBalanceLocal
            Assert.AreEqual(500m -0m+50m, my1stTrans.CumulativeLocalAmount);


            Assert.AreEqual(100m, my1stTrans.ForeignAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);
            Assert.AreEqual(100m - 0m + 10m, my1stTrans.CumulativeForeignAmount);

            Assert.AreEqual(110m, my1stTrans.CumulativeForeignAmount);

            var lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(lastTrans);
            Assert.AreEqual("28", lastTrans.Id);
            Assert.AreEqual(0m, lastTrans.LocalAmountDebit);
            Assert.AreEqual(500m, lastTrans.LocalAmountCredit);
            var lastCumulativeLocalAmount = //160.38m;
                550m;
            Assert.AreEqual(lastCumulativeLocalAmount + 0m - 500m , lastTrans.CumulativeLocalAmount);




            var lastCumulativeforeignAmount = //160.38m;
                110m;
            Assert.AreEqual(0m, lastTrans.ForeignAmountDebit);
            Assert.AreEqual(100m, lastTrans.ForeignAmountCredit);
            Assert.AreEqual(lastCumulativeforeignAmount + 0m - 100m, lastTrans.CumulativeForeignAmount);


            Assert.AreEqual(classUnderTest.Response.EndBalanceLocal, lastTrans.CumulativeLocalAmount);
            Assert.IsNull(fltr.CallBack);

            fltr.CallBack = (classUnderTest.Response as LedgerTransactionBalanceFilterCallBack);
            fltr.PageStartAtRecordIndex = 10;
            classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();


            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);

            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);




            Assert.AreEqual(4, classUnderTest.Response.MyLedgerTransactionList.Count());
            
            lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(lastTrans);
            Assert.AreEqual("40", lastTrans.Id);
            Assert.AreEqual(0m, lastTrans.LocalAmountDebit);
            Assert.AreEqual(500m, lastTrans.LocalAmountCredit);
            
            Assert.AreEqual(classUnderTest.Response.EndBalanceLocal, lastTrans.CumulativeLocalAmount);

        }





        [TestMethod]
        public void Run_IncludeChildAccounts_CurrencyIdEUR()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = true,
                GLAccountId = _MainGLAccountIdTeva,
                CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 10,
                PageStartAtRecordIndex = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MyGLAccountIdChildUSA));
            Assert.AreEqual(2,classUnderTest.Response.AllIdAccounts.Count);
            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);


            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016, 09, 01), classUnderTest.Response.MaxCreateAt);
            Assert.AreEqual(100M-50m+100M-50M, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(100M - 50m + 100M - 50M, classUnderTest.Response.EndBalanceLocal);

            Assert.IsNotNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.AreEqual(1, classUnderTest.Response.StartBalanceForeignList.Count);
            Assert.AreEqual(10m + 10m, classUnderTest.Response.StartBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.StartBalanceForeignList.First().CurrencyId);


            Assert.IsNotNull(classUnderTest.Response.EndBalanceForeignList);
            Assert.AreEqual(1, classUnderTest.Response.EndBalanceForeignList.Count);
            Assert.AreEqual(10m + 10m, classUnderTest.Response.EndBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.EndBalanceForeignList.First().CurrencyId);


            var trans2AccountCurrency = classUnderTest.Response.MyLedgerTransactionList.Where(rec => rec.AccountId == _MyGLAccountIdChildUSA).ToList();
            Assert.IsNotNull(trans2AccountCurrency);


            Assert.AreEqual(14 + 2, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


        }

        [TestMethod]
        public void Run_AllCurrencies()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                //CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 100,
                PageStartAtRecordIndex = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.AreEqual(1,classUnderTest.Response.AllIdAccounts.Count);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.AreEqual(true, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016, 09, 01), classUnderTest.Response.MaxCreateAt);

            Assert.AreEqual(-339.62m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(-339.62m, classUnderTest.Response.EndBalanceLocal);
            
            
            Assert.IsNotNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.AreEqual(2, classUnderTest.Response.StartBalanceForeignList.Count);
            Assert.AreEqual(-89.92m, classUnderTest.Response.StartBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdUSD, classUnderTest.Response.StartBalanceForeignList.First().CurrencyId);
            Assert.AreEqual(10m, classUnderTest.Response.StartBalanceForeignList.Last().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.StartBalanceForeignList.Last().CurrencyId);

            Assert.IsNotNull(classUnderTest.Response.EndBalanceForeignList);
            Assert.AreEqual(2, classUnderTest.Response.EndBalanceForeignList.Count);
            Assert.AreEqual(-89.92m, classUnderTest.Response.EndBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdUSD, classUnderTest.Response.EndBalanceForeignList.First().CurrencyId);
            Assert.AreEqual(10m, classUnderTest.Response.EndBalanceForeignList.Last().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.EndBalanceForeignList.Last().CurrencyId);
            var rowCout =14*3;
            Assert.AreEqual(rowCout, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


            Assert.AreEqual(rowCout, classUnderTest.Response.MyLedgerTransactionList.Count());
            var my1stTrans = classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(my1stTrans);
            Assert.AreEqual("3", my1stTrans.Id);
            Assert.AreEqual(500m, my1stTrans.LocalAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);
            Assert.AreEqual(500m - 0m - 339.62m, my1stTrans.CumulativeLocalAmount);


            Assert.AreEqual(0m, my1stTrans.CumulativeForeignAmount);

            var lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(lastTrans);
            Assert.AreEqual("42", lastTrans.Id);
            Assert.AreEqual(0m, lastTrans.LocalAmountDebit);
            Assert.AreEqual(100m, lastTrans.LocalAmountCredit);
            var lastCumulativeLocalAmount = -239.62M;
            Assert.AreEqual(lastCumulativeLocalAmount + 0m - 100m, lastTrans.CumulativeLocalAmount);
            Assert.AreEqual(classUnderTest.Response.EndBalanceLocal, lastTrans.CumulativeLocalAmount);



            fltr.CallBack = (classUnderTest.Response as LedgerTransactionBalanceFilterCallBack);
            fltr.PageStartAtRecordIndex = 100;
            classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();


            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));



            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(0, classUnderTest.Response.MyLedgerTransactionList.Count());

        }


        [TestMethod]
        public void Run_FutureAccountDate_AllCurrencies()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                //CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(30),
                To = _StartDate.AddMonths(40),
                PageSize = 100,
                PageStartAtRecordIndex = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.AreEqual(1, classUnderTest.Response.AllIdAccounts.Count);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.AreEqual(true, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(DateTime.Now.Date, classUnderTest.Response.MaxCreateAt.GetValueOrDefault().Date);

            Assert.AreEqual(-339.62m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(-339.62m, classUnderTest.Response.EndBalanceLocal);


            Assert.IsNotNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.AreEqual(2, classUnderTest.Response.StartBalanceForeignList.Count);
            Assert.AreEqual(-89.92m, classUnderTest.Response.StartBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdUSD, classUnderTest.Response.StartBalanceForeignList.First().CurrencyId);
            Assert.AreEqual(10m, classUnderTest.Response.StartBalanceForeignList.Last().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.StartBalanceForeignList.Last().CurrencyId);

            Assert.IsNotNull(classUnderTest.Response.EndBalanceForeignList);
            Assert.AreEqual(2, classUnderTest.Response.EndBalanceForeignList.Count);
            Assert.AreEqual(-89.92m, classUnderTest.Response.EndBalanceForeignList.First().BalanceForeign);
            Assert.AreEqual(_CurrencyIdUSD, classUnderTest.Response.EndBalanceForeignList.First().CurrencyId);
            Assert.AreEqual(10m, classUnderTest.Response.EndBalanceForeignList.Last().BalanceForeign);
            Assert.AreEqual(_CurrencyIdEUR, classUnderTest.Response.EndBalanceForeignList.Last().CurrencyId);
            var rowCout = 0;//future
            Assert.AreEqual(rowCout, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


            Assert.AreEqual(rowCout, classUnderTest.Response.MyLedgerTransactionList.Count());

        }

        [TestMethod]
        public void Run_IncludeRelatedCurrenciesAccount_AllCurrencies()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount=true,
                IncludeChildAccounts= false,
                //CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 100,
                PageStartAtRecordIndex = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.AreEqual(2, classUnderTest.Response.AllIdAccounts.Count);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_MainGLAccountIdTeva));
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_GLAccountCurrencyTevaJPY));

            Assert.AreEqual(true, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016, 09, 01), classUnderTest.Response.MaxCreateAt);

            Assert.AreEqual(-339.62m +50m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(-339.62m +50m, classUnderTest.Response.EndBalanceLocal);


            Assert.IsNotNull(classUnderTest.Response.StartBalanceForeignList);
            Assert.AreEqual(3, classUnderTest.Response.StartBalanceForeignList.Count);
            Assert.AreEqual(-89.92m, classUnderTest.Response.StartBalanceForeignList.First( r=>r.CurrencyId==_CurrencyIdUSD).BalanceForeign);
            
            Assert.AreEqual(10m, classUnderTest.Response.StartBalanceForeignList.First( r=>r.CurrencyId==_CurrencyIdEUR).BalanceForeign);
            Assert.AreEqual(50.45m, classUnderTest.Response.StartBalanceForeignList.First( r=>r.CurrencyId==_CurrencyIdJPY).BalanceForeign);
            

            Assert.IsNotNull(classUnderTest.Response.EndBalanceForeignList);
            Assert.AreEqual(3, classUnderTest.Response.EndBalanceForeignList.Count);

            Assert.AreEqual(-89.92m, classUnderTest.Response.EndBalanceForeignList.First(r => r.CurrencyId == _CurrencyIdUSD).BalanceForeign);

            Assert.AreEqual(10m, classUnderTest.Response.EndBalanceForeignList.First(r => r.CurrencyId == _CurrencyIdEUR).BalanceForeign);
            Assert.AreEqual(50.45m, classUnderTest.Response.EndBalanceForeignList.First(r => r.CurrencyId == _CurrencyIdJPY).BalanceForeign);
            

            var rowCout = 14 * 3+2;
            Assert.AreEqual(rowCout, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);

            var jpyTrans = classUnderTest.Response.MyLedgerTransactionList.Where(r => r.AccountId == _GLAccountCurrencyTevaJPY).ToList();
            Assert.IsNotNull(jpyTrans);
            Assert.AreEqual(2, jpyTrans.Count);

            Assert.AreEqual(2, jpyTrans.Where(r => r.CurrencyId == _CurrencyIdJPY).Count());

            Assert.AreEqual(rowCout, classUnderTest.Response.MyLedgerTransactionList.Count());
            var my1stTrans = classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(my1stTrans);
            Assert.AreEqual("3", my1stTrans.Id);
            Assert.AreEqual(500m, my1stTrans.LocalAmountDebit);
            Assert.AreEqual(0m, my1stTrans.LocalAmountCredit);
            Assert.AreEqual(500m - 0m - 339.62m +50m, my1stTrans.CumulativeLocalAmount);


            Assert.AreEqual(0m, my1stTrans.CumulativeForeignAmount);

            var lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(lastTrans);
            Assert.AreEqual(0m, lastTrans.CumulativeForeignAmount);
            Assert.AreEqual("42", lastTrans.Id);
            Assert.AreEqual(0m, lastTrans.LocalAmountDebit);
            Assert.AreEqual(100m, lastTrans.LocalAmountCredit);
            var lastCumulativeLocalAmount = -239.62M+50M;
            Assert.AreEqual(lastCumulativeLocalAmount + 0m - 100m, lastTrans.CumulativeLocalAmount);
            Assert.AreEqual(classUnderTest.Response.EndBalanceLocal, lastTrans.CumulativeLocalAmount);





        }


        IAccountingContext GetIAccountingContext()
        {

            var myGLAccount = new GLAccount()
            {
                Tenant = _MyTenant,
                Id = _MainGLAccountIdTeva,
                IsMultiCurrency= true,
                //BalanceInLocalCurrency = 120205.02m
            };

            var myGLAccountChild = new GLAccount()
            {
                Tenant = _MyTenant,
                ParentAccountId = _MainGLAccountIdTeva,
                Id =_MyGLAccountIdChildUSA,
                //BalanceInLocalCurrency = 120205.02m
            };
            var mockGLAccount = new MockObjectSet<GLAccount>() { myGLAccount, myGLAccountChild }; ;

            var mockGLAccountCurrency = new MockObjectSet<GLAccountCurrency>() { 
                new GLAccountCurrency{
                    Tenant =_MyTenant,
                    MainGLAccountId=_MainGLAccountIdTeva,
                    GLAccountId= _GLAccountCurrencyTevaJPY,
                     CurrencyId =_CurrencyIdJPY,
                }
            };

            
            var mockGLAccountTotalByMonth = new MockObjectSet<GLAccountTotalByMonth>()
            {
                new GLAccountTotalByMonth()
                {
                    DateTypeCode = "1",
                    Tenant=_MyTenant,
                    Year =_StartDate.Date.Year,
                    Month =_StartDate.Date.Month,
                    AccountId = _MainGLAccountIdTeva,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = 100.23m,
                    ForeignAmountDebit = 10.31m,
                    LocalAmountCredit=400.92m, 
                    LocalAmountDebit =11.3m,
                },

                new GLAccountTotalByMonth()
                {
                    DateTypeCode = "1",
                    Tenant=_MyTenant,
                    Year =_StartDate.Date.Year,
                    Month =_StartDate.Date.Month,
                    AccountId = _MainGLAccountIdTeva,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountCredit = 10m,
                    ForeignAmountDebit = 20m,
                    LocalAmountCredit=50, 
                    LocalAmountDebit =100,
                },
  
                

                 new GLAccountTotalByMonth()
                {
                    DateTypeCode = "1",
                    Tenant=_MyTenant,
                    Year =_StartDate.Date.Year,
                    Month =_StartDate.Date.Month,
                    AccountId = _MyGLAccountIdChildUSA,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountCredit = 10m,
                    ForeignAmountDebit = 20m,
                    LocalAmountCredit=50, 
                    LocalAmountDebit =100,
                },

            new GLAccountTotalByMonth()
                {
                    DateTypeCode = "1",
                    Tenant=_MyTenant,
                    Year =_StartDate.Date.Year,
                    Month =_StartDate.Date.Month,
                    AccountId = _GLAccountCurrencyTevaJPY,
                    CurrencyId = _CurrencyIdJPY,
                    ForeignAmountCredit = 100m,
                    ForeignAmountDebit = 150.45m,
                    LocalAmountCredit=50, 
                    LocalAmountDebit =100,
                },
            };

            var mockLedgerTransaction = new MockObjectSet<LedgerTransaction>() ;
            var listLedgerTransaction = GetLedger();
            listLedgerTransaction.ForEach(
                tran =>
                {
                    mockLedgerTransaction.Add(tran);
                }
            );

            
            


            var fakeIAccountingContext = A.Fake<IAccountingContext>();
            A.CallTo(() => fakeIAccountingContext.GLAccounts)
                .Returns(mockGLAccount);

            A.CallTo(() => fakeIAccountingContext.LedgerTransactions)
                .Returns(mockLedgerTransaction);

            A.CallTo(() => fakeIAccountingContext.GLAccountTotalByMonths)
                .Returns(mockGLAccountTotalByMonth);

            A.CallTo(() => fakeIAccountingContext.GLAccountCurrencies)
                .Returns(mockGLAccountCurrency);

            A.CallTo(() => fakeIAccountingContext.Journals)
                .Returns(new MockObjectSet<Journal>());
            A.CallTo(() => fakeIAccountingContext.JournalLines)
                .Returns(new MockObjectSet<JournalLine>());



            return fakeIAccountingContext;
        }

        private List<LedgerTransaction> GetLedger()
        {
            var listLedgerTransaction = new List<LedgerTransaction>();

            var currDate = _StartDate.AddMonths(2);
            int myId = 1;
            
            for (int i = 0; i < 10; i++)
            {
                currDate = currDate.AddMonths(1);
                
                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 10),
                    LocalAmountDebit = 400,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountDebit = 100,

                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 20),
                    LocalAmountCredit = 400,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = 100
                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 5),
                    LocalAmountDebit = 500,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountDebit = 100
                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 15),
                    LocalAmountCredit = 500,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountCredit = 100
                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 18),
                    LocalAmountDebit = 100,
                    CurrencyId = _CurrencyIdNIS,
                    ForeignAmountDebit = 100
                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 28),
                    LocalAmountCredit = 100,
                    CurrencyId = _CurrencyIdNIS,
                    ForeignAmountCredit = 100
                });
            }

            currDate = _StartDate.AddMonths(4);
            listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _GLAccountCurrencyTevaJPY,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 10),
                    LocalAmountDebit = 400,
                    CurrencyId = _CurrencyIdJPY,
                    ForeignAmountDebit = 100,

                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _GLAccountCurrencyTevaJPY,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 20),
                    LocalAmountCredit = 400,
                    CurrencyId = _CurrencyIdJPY,
                    ForeignAmountCredit = 100
                });
            


            listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MyGLAccountIdChildUSA,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 10),
                    LocalAmountDebit = 400,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountDebit = 100,

                });

                listLedgerTransaction.Add(new LedgerTransaction()
                {
                    Id = GetId(ref myId),
                    CreateDate = currDate,
                    Tenant = _MyTenant,
                    AccountId = _MyGLAccountIdChildUSA,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 20),
                    LocalAmountCredit = 400,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountCredit = 100
                });

            
            return listLedgerTransaction;
        }

        private static string GetId(ref int myId)
        {
            var d= myId++;
            return d.ToString();
        }





        
    }
}
