using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logitude.Accounting.BL.CoreBL;
using System.Linq;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;
using FakeItEasy;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class YearTransferUnitTest
    {

        [TestInitialize]
        public void TestInitialize1()
        {
            var textCodeTranslatorFake = A.Fake<ITextCodeTranslator>();
            A.CallTo(() => textCodeTranslatorFake.Translate(A<string>.Ignored, A<int>.Ignored))
                .ReturnsLazily(
                (string textCodeCode, int tenant) =>
                {
                    return textCodeCode;
                }
            );
            YearTransferService.OverrideITextCodeTranslator = textCodeTranslatorFake;
        }


        [TestMethod]
        public void YearTransfer_ok()
        {
            const string RevenueAcc = "1-10";
            const string ExpenseAcc = "1-20";
            const string curUSD = "USD";
            const string curEUR = "EUR";
            var service = new YearTransferService();
            var journal =
            service.CreateJournal(
                new DateTime(2017, 12, 31),
                
                (new System.Collections.Generic.List<GLAccountAndMoreDTO>() {
                    new GLAccountAndMoreDTO()
                    {
                        Id=RevenueAcc,
                        RevenueExpenseType=YearTransferService.RevenueType

                    },
                    new GLAccountAndMoreDTO()
                    {
                        Id=ExpenseAcc,
                        RevenueExpenseType=YearTransferService.ExpenseType

                    }
                }).AsQueryable<GLAccountAndMoreDTO>()
                ,
                new System.Collections.Generic.List<Logitude.Accounting.Data.Repositories.CurrencySum>()
                {
                    new Logitude.Accounting.Data.Repositories.CurrencySum()
                    {
                         AccountId =ExpenseAcc,
                          CurrencyId=curUSD ,
                           ForeignAmountCredit =151.1m,
                            ForeignAmountDebit = 1.1m,
                             LocalAmountCredit = 400m
                    },
                    //new Logitude.Accounting.Data.Repositories.CurrencySum()
                    //{
                    //     AccountId =RevenueAcc,
                    //      CurrencyId=curEUR ,
                    //       ForeignAmountCredit =22.1m,
                    //        ForeignAmountDebit = 2.1m,
                    //         LocalAmountCredit = 20m
                    //}
                },
                getRevenueExpenseId(), "userId", new DateTime(2018, 02, 28),
                989

                );
            Assert.IsNotNull(journal);

            //Assert.AreEqual(journal.AccountingEntityReference, "YearTransfer");
            Assert.IsNotNull(journal.JournalLines);
            Assert.AreEqual(journal.JournalLines.Count, 2);

            Assert.AreEqual(journal.JournalLines[0].ActionTypeCodeEnum, Logitude.Accounting.Def.EntityPMs.MyJournalActionTypeEnum.Debit);
            Assert.AreEqual(journal.JournalLines[0].ForeignAmount, -150M);
            Assert.AreEqual(journal.JournalLines[0].LocalAmount, -400M);
            Assert.AreEqual(journal.JournalLines[0].DebitAccountId, getRevenueExpenseId());

            Assert.AreEqual(journal.JournalLines[1].ActionTypeCodeEnum, Logitude.Accounting.Def.EntityPMs.MyJournalActionTypeEnum.Credit);
            Assert.AreEqual(journal.JournalLines[1].ForeignAmount, -150M);
            Assert.AreEqual(journal.JournalLines[1].LocalAmount, -400M);
            Assert.AreEqual(journal.JournalLines[1].CreditAccountId, ExpenseAcc);


            var CreditAmmount =
           journal.JournalLines.Where(r => r.ActionTypeCodeEnum == Logitude.Accounting.Def.EntityPMs.MyJournalActionTypeEnum.Credit).Sum(r => r.LocalAmount);

            var DebitAmmount =
           journal.JournalLines.Where(r => r.ActionTypeCodeEnum == Logitude.Accounting.Def.EntityPMs.MyJournalActionTypeEnum.Debit).Sum(r => r.LocalAmount);
            Assert.IsTrue(DebitAmmount == CreditAmmount, "Expected DebitAmmount == CreditAmmount");
        }

        private static string getRevenueExpenseId()
        {
            return "1-1";
        }
    }
}
