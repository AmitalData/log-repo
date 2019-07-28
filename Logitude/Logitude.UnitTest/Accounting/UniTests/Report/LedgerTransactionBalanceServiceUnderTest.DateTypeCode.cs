using FakeItEasy;
using Logitude.Accounting.BL.CloseTables;
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

    public partial class LedgerTransactionBalanceServiceUnderTest
    {
        [TestMethod]
        public void Run_ByAccountingDate_YearTransferButVoided_Ignored()
        {
            bool voidedYearTransferTest = true;
            bool myYearTransferTest = true;
            var myIAccountingContext = GetIAccountingContextDateType(myYearTransferTest, voidedYearTransferTest);
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 01, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(100m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(350m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(2, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("2", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("3", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }

        [TestMethod]
        public void Run_ByAccountingDate_YearTransfer()
        {
            bool myYearTransferTest = true;
            var myIAccountingContext = GetIAccountingContextDateType(myYearTransferTest);
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 01, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(150m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(400m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(2, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("2", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("3", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }

        [TestMethod]
        public void Run_ByAccountingDate()
        {
            
            var myIAccountingContext = GetIAccountingContextDateType();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 01, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(100m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(400m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(3, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("1", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("3", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }

        [TestMethod]
        public void Run_ByAccountingDate_notInclude20180701()
        {
            var myIAccountingContext = GetIAccountingContextDateType();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 07, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(150m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(400m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(2, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("2", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("3", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }



        [TestMethod]
        public void Run_ByDueDate_notInclude20180701()
        {
            var myIAccountingContext = GetIAccountingContextDateType();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 07, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.DueDate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(200m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(400m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(2, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("1", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("3", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }


        [TestMethod]
        public void Run_ByDDocumentDate_notInclude20180701()
        {
            var myIAccountingContext = GetIAccountingContextDateType();
            var fltr = new LedgerTransactionBalanceFilter()
            {
                Tenant = _MyTenant,
                GLAccountId = _MainGLAccountIdTeva,
                IncludeRelatedCurrenciesAccount = false,
                IncludeChildAccounts = false,
                //CurrencyId = _CurrencyIdEUR,
                From = new DateTime(2018, 07, 1),
                To = new DateTime(2018, 11, 1),
                PageSize = 100,
                PageStartAtRecordIndex = 0,
                DateTypeCode = GLAccountTotalDateTypeValues.DocumentDate
            };
            var classUnderTest = new LedgerTransactionBalanceService(myIAccountingContext, fltr);


            //Act 
            classUnderTest.Run();
            Assert.IsNotNull(classUnderTest.Response);
            Assert.AreEqual(300m, classUnderTest.Response.StartBalanceLocal);
            Assert.AreEqual(400m, classUnderTest.Response.EndBalanceLocal);
            Assert.IsNotNull(classUnderTest.Response.MyLedgerTransactionList);
            Assert.AreEqual(1, classUnderTest.Response.MyLedgerTransactionList.Count());
            Assert.AreEqual("2", classUnderTest.Response.MyLedgerTransactionList.First().Id);
            Assert.AreEqual("2", classUnderTest.Response.MyLedgerTransactionList.Last().Id);
        }




        IAccountingContext GetIAccountingContextDateType(bool myYearTransferTest=false, bool voidedYearTransferTest = false)
        {
            var accountingCurrency = _CurrencyIdUSD;
            var myGLAccount = new GLAccount()
            {
                Tenant = _MyTenant,
                Id = _MainGLAccountIdTeva,
                IsMultiCurrency = true,
                //BalanceInLocalCurrency = 120205.02m
            };


            var mockGLAccount = new MockObjectSet<GLAccount>() { myGLAccount }; ;

            int yyyy = 2018;

            var mockGLAccountTotalByMonth = new MockObjectSet<GLAccountTotalByMonth>()
            {
                new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate,
                    Tenant=_MyTenant,
                    Year =yyyy-1,
                    Month =12,
                    AccountId = _MainGLAccountIdTeva,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = 0m,
                    ForeignAmountDebit = 100m,
                    LocalAmountCredit=0m,
                    LocalAmountDebit =100m,
                },
                new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                    Tenant=_MyTenant,
                    Year =yyyy-1,
                    Month =12,
                    AccountId = _MainGLAccountIdTeva,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = 0m,
                    ForeignAmountDebit = 100m,
                    LocalAmountCredit=0m,
                    LocalAmountDebit =100m,
                },
                new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.DocumentDate,
                    Tenant=_MyTenant,
                    Year =yyyy-1,
                    Month =12,
                    AccountId = _MainGLAccountIdTeva,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = 0m,
                    ForeignAmountDebit = 100m,
                    LocalAmountCredit=0m,
                    LocalAmountDebit =100m,
                },


            };






            var currDate = _StartDate.AddMonths(2);
            int myId = 1;
            var mockLedgerTransaction = new MockObjectSet<LedgerTransaction>();


            string YearTransferTestJornalstorno = "YearTransferTest storno";

            var listLedgerTransaction = new List<LedgerTransaction>()
            {
                 new LedgerTransaction()
                 {
                      Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(yyyy, 1, 1),
                    DueDate= new DateTime(yyyy, 7, 10),
                    DocumentDate= new DateTime(yyyy, 5, 15),
                    LocalAmountDebit = 50,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountDebit = 50,
                    JournalId="YearTransferTest"

                 },
                 new LedgerTransaction()
                 {
                      Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(yyyy, 8, 1),
                    DueDate= new DateTime(yyyy, 5, 10),
                    DocumentDate= new DateTime(yyyy, 9, 15),
                    LocalAmountDebit = 100,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountDebit = 100,

                 },
                 new LedgerTransaction()
                 {
                      Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(yyyy, 9, 1),
                    DueDate= new DateTime(yyyy, 8, 10),
                    DocumentDate= new DateTime(yyyy, 6, 15),
                    LocalAmountDebit = 150,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountDebit = 150,

                 },
                  new LedgerTransaction()
                 {
                      Id = GetId(ref myId),
                    CreateDate =currDate,
                    Tenant=_MyTenant,
                    AccountId = _MainGLAccountIdTeva,
                    AccountingDate = new DateTime(yyyy, 1, 1),
                    DueDate= new DateTime(yyyy, 7, 10),
                    DocumentDate= new DateTime(yyyy, 5, 15),
                    LocalAmountDebit = -50,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountDebit = -50,
                    JournalId=YearTransferTestJornalstorno

                 },

            };

            var mockJournal = new MockObjectSet<Journal>();

            if (myYearTransferTest)
            {
                var l=listLedgerTransaction.First();
                mockJournal.Add(

                    new Journal()
                    {
                        Id = l.JournalId,
                        AccountingDate = l.AccountingDate,
                        Tenant = l.Tenant,
                        AccountingEntityCode = "11",
               //         VoidedByJournalId= voidedYearTransferTest? "voidJ":""
                    }
                );
                if (!voidedYearTransferTest)
                {
                    removeYearTransferSrorno(YearTransferTestJornalstorno, listLedgerTransaction);
                }
                else
                {
                    var l2 = listLedgerTransaction.First(r => r.JournalId == YearTransferTestJornalstorno);
                    mockJournal.Add(

                        new Journal()
                        {
                            Id = l2.JournalId,
                            AccountingDate = l2.AccountingDate,
                            Tenant = l2.Tenant,
                            AccountingEntityCode = "11",
                        
                    }
                    );

                }

            }
            else
            {
                removeYearTransferSrorno(YearTransferTestJornalstorno, listLedgerTransaction);

            }
            listLedgerTransaction.ForEach(
                tran =>
                {
                    mockLedgerTransaction.Add(tran);
                }
            );
            AccTot(mockGLAccountTotalByMonth, listLedgerTransaction);
            DueTot(mockGLAccountTotalByMonth, listLedgerTransaction);
            DocumentDateTot(mockGLAccountTotalByMonth, listLedgerTransaction);

            var fakeIAccountingContext = A.Fake<IAccountingContext>();
            A.CallTo(() => fakeIAccountingContext.GLAccounts)
                .Returns(mockGLAccount);

            A.CallTo(() => fakeIAccountingContext.LedgerTransactions)
                .Returns(mockLedgerTransaction);

            A.CallTo(() => fakeIAccountingContext.GLAccountTotalByMonths)
                .Returns(mockGLAccountTotalByMonth);


            A.CallTo(() => fakeIAccountingContext.Journals)
                .Returns(mockJournal);
            A.CallTo(() => fakeIAccountingContext.JournalLines)
                .Returns(new MockObjectSet<JournalLine>());



            return fakeIAccountingContext;
        }

        private static void removeYearTransferSrorno(string YearTransferTestJornalstorno, List<LedgerTransaction> listLedgerTransaction)
        {
            var lYearTransferTestJornalstorno = listLedgerTransaction.First(r => r.JournalId == YearTransferTestJornalstorno);
            listLedgerTransaction.Remove(lYearTransferTestJornalstorno);
        }

        private void AccTot(MockObjectSet<GLAccountTotalByMonth> mockGLAccountTotalByMonth, List<LedgerTransaction> listLedgerTransaction)
        {
            var q = listLedgerTransaction
                        .GroupBy(r => new { r.AccountingDate.Year, r.AccountingDate.Month, r.AccountId, r.CurrencyId });

            var myTotList = q
                .Select(g => new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate,
                    Tenant = _MyTenant,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AccountId = g.Key.AccountId,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = g.Sum(r => r.ForeignAmountCredit),
                    ForeignAmountDebit = g.Sum(r => r.ForeignAmountDebit),
                    LocalAmountCredit = g.Sum(r => r.LocalAmountCredit),
                    LocalAmountDebit = g.Sum(r => r.LocalAmountDebit),
                }).ToList();
            myTotList.ForEach(r => { mockGLAccountTotalByMonth.Add(r); });
        }

        private void DueTot(MockObjectSet<GLAccountTotalByMonth> mockGLAccountTotalByMonth, List<LedgerTransaction> listLedgerTransaction)
        {
            var q = listLedgerTransaction
                        .GroupBy(r => new { r.DueDate.Year, r.DueDate.Month, r.AccountId, r.CurrencyId });

            var myTotList = q
                .Select(g => new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.DueDate,
                    Tenant = _MyTenant,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AccountId = g.Key.AccountId,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = g.Sum(r => r.ForeignAmountCredit),
                    ForeignAmountDebit = g.Sum(r => r.ForeignAmountDebit),
                    LocalAmountCredit = g.Sum(r => r.LocalAmountCredit),
                    LocalAmountDebit = g.Sum(r => r.LocalAmountDebit),
                }).ToList();
            myTotList.ForEach(r => { mockGLAccountTotalByMonth.Add(r); });
        }

        private void DocumentDateTot(MockObjectSet<GLAccountTotalByMonth> mockGLAccountTotalByMonth, List<LedgerTransaction> listLedgerTransaction)
        {
            var q = listLedgerTransaction
                        .GroupBy(r => new { r.DocumentDate.Year, r.DocumentDate.Month, r.AccountId, r.CurrencyId });

            var myTotList = q
                .Select(g => new GLAccountTotalByMonth()
                {
                    DateTypeCode = GLAccountTotalDateTypeValues.DocumentDate,
                    Tenant = _MyTenant,
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    AccountId = g.Key.AccountId,
                    CurrencyId = _CurrencyIdUSD,
                    ForeignAmountCredit = g.Sum(r => r.ForeignAmountCredit),
                    ForeignAmountDebit = g.Sum(r => r.ForeignAmountDebit),
                    LocalAmountCredit = g.Sum(r => r.LocalAmountCredit),
                    LocalAmountDebit = g.Sum(r => r.LocalAmountDebit),
                }).ToList();
            myTotList.ForEach(r => { mockGLAccountTotalByMonth.Add(r); });
        }

    }
}
