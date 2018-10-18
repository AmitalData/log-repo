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
    public class LedgerTransactionBalanceServiceUnderTestcs
    {
        private int _MyTenant;
        private int _OtherTenant;
        private string _myGLAccountId;
        private string _CurrencyIdUSD;
        private string _CurrencyIdNIS;
        private string _CurrencyIdEUR;
        private DateTime _StartDate;
        private string _GLAccountCurrency;
        private string _CurrencyIdJPY;
        public LedgerTransactionBalanceServiceUnderTestcs()
        {
            _MyTenant = 1; _OtherTenant = 99;
            _myGLAccountId = "myGLAccountId";
            _GLAccountCurrency = "GLAccountCurrency";

            _CurrencyIdUSD = "USD";
            _CurrencyIdNIS = "NIS";
            _CurrencyIdEUR = "EUR";
            _CurrencyIdJPY = "JPY";
            _StartDate = new DateTime(2015, 12,01);

        }
        [TestMethod]
        public void RunCheckParam_ThrowException()
        {
            //prepare ...
            var myIAccountingContext = GetIAccountingContext();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _myGLAccountId,
                CurrencyId = _CurrencyIdEUR,
                From = _StartDate.AddMonths(2),
                To = _StartDate.AddMonths(10),
                PageSize = 10,
                CurrZeroPage = 0,

            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();

            Assert.IsNotNull(classUnderTest.Response);
            var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_myGLAccountId));
            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);

            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            //"Have1CurrencyIdInPeriod": null,
            Assert.AreEqual(new DateTime(2016,09,01), classUnderTest.Response.MaxCreateAt);
            Assert.AreEqual(-339.62m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(-339.62m, classUnderTest.Response.EndBalanceLocal);
            Assert.AreEqual(10m, classUnderTest.Response.StartBalanceForeign);
            Assert.AreEqual(10m, classUnderTest.Response.EndBalanceForeign);
            Assert.AreEqual(14, classUnderTest.Response.TotalRowCount);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);


            Assert.AreEqual(10, classUnderTest.Response.MyLedgerTransactionList.Count());
            var _1stTrans =classUnderTest.Response.MyLedgerTransactionList.FirstOrDefault();
            Assert.IsNotNull(_1stTrans);
            Assert.AreEqual("3", _1stTrans.Id);
            Assert.AreEqual(500m, _1stTrans.LocalAmountDebit);
            Assert.AreEqual(0m, _1stTrans.LocalAmountCredit);
            Assert.AreEqual(500m -0m- 339.62m, _1stTrans.CumulativeLocalAmount);


            Assert.AreEqual(110m, _1stTrans.CumulativeForeignAmount);

            var _lastTrans = classUnderTest.Response.MyLedgerTransactionList.LastOrDefault();
            Assert.IsNotNull(_lastTrans);
            Assert.AreEqual("28", _lastTrans.Id);
            Assert.AreEqual(0m, _lastTrans.LocalAmountDebit);
            Assert.AreEqual(500m, _lastTrans.LocalAmountCredit);
            var lastCumulativeLocalAmount = 160.38m;
            Assert.AreEqual(lastCumulativeLocalAmount + 0m - 500m , _lastTrans.CumulativeLocalAmount);
            Assert.AreEqual(classUnderTest.Response.EndBalanceLocal, _lastTrans.CumulativeLocalAmount);




            fltr.CurrZeroPage = 1;
            classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();


            Assert.IsNotNull(classUnderTest.Response);
            //var str = JsonConvert.SerializeObject(classUnderTest.Response);
            Assert.IsNotNull(classUnderTest.Response.AllIdAccounts);
            Assert.IsTrue(classUnderTest.Response.AllIdAccounts.Contains(_myGLAccountId));
            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);

            Assert.AreEqual(false, classUnderTest.Response.SuppressCumulativeDueMultiCurrencyInPeriod);
            

        }
        IAccountingContext GetIAccountingContext()
        {

            var myGLAccount = new GLAccount()
            {
                Tenant = _MyTenant,
                Id = _myGLAccountId,
                BalanceInLocalCurrency = 120205.02m
            };
            var mockGLAccount = new MockObjectSet<GLAccount>() { myGLAccount }; ;

            var mockGLAccountCurrency = new MockObjectSet<GLAccountCurrency>() { 
                new GLAccountCurrency{
                    Tenant =_MyTenant,
                    MainGLAccountId=_myGLAccountId,
                    GLAccountId= _GLAccountCurrency,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
                    CurrencyId = _CurrencyIdEUR,
                    ForeignAmountCredit = 10m,
                    ForeignAmountDebit = 20m,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
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
                    AccountId = _myGLAccountId,
                    AccountingDate = new DateTime(currDate.Year, currDate.Month, 28),
                    LocalAmountCredit = 100,
                    CurrencyId = _CurrencyIdNIS,
                    ForeignAmountCredit = 100
                });
            }
            return listLedgerTransaction;
        }

        private static string GetId(ref int myId)
        {
            var d= myId++;
            return d.ToString();
        }





        
    }
}
