using System;
using System.Collections.Generic;
using FakeItEasy;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class CreateAutoReconcileWhileStreamingServiceUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {


            IAccountingContext fakeIAccountingContext;
            fakeIAccountingContext = A.Fake<IAccountingContext>();

            var myCreateAutoReconcileWhileStreamingService = A.Fake < CreateAutoReconcileWhileStreamingService>(option => option.CallsBaseMethods());



            List<LedgerTransactionPM> myOrginalJournalTransaction, myStornoLedgerTransactionPM;
            JournalStornoReconcileServiceUnitTest.GetExampleReconcile(out myOrginalJournalTransaction, out myStornoLedgerTransactionPM);

            A.CallTo(() =>
                    myCreateAutoReconcileWhileStreamingService
                    .GetLedgerTransactionToReconcile(new List<string>() { "1-toclose", "2-toclose" }))
            .Returns(new List<LedgerTransactionPM>()
            {
                new LedgerTransactionPM()
                {
                    Id= "1-toclose",
                     AccountId ="AccToClose",
                     AmountToReconcile=100,
                      InReconcileProgress=true,
                       OpenAmount =100,
                        OpenAmountCurrencyCode="USD"

                },
                new LedgerTransactionPM()
                {
                    Id= "2-toclose",
                     AccountId ="AccToClose",
                     AmountToReconcile=200,
                      InReconcileProgress=true,
                       OpenAmount =200,
                        OpenAmountCurrencyCode="USD"

                }
            });

            myCreateAutoReconcileWhileStreamingService
                .MustInit(fakeIAccountingContext, 
                new JournalPM() { JournalReconciles= new List<JournalReconcilePM>() {
                     new JournalReconcilePM()
                     {
                          ReconciliationAmount=100,
                           LedgerTransactionId="1-toclose",
                            
                     }
                } }, 
                new List<LedgerTransactionPM>() { });
            myCreateAutoReconcileWhileStreamingService.CreateAutoReconcileWhileStreaming();




        }
    }
}
