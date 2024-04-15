using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Accounting.UniTests
{
    public partial class JournalValidatorUnitTest
    {
        

        [TestMethod]
        public void journalApproveParser_CreateLedger_MapByJournalActionType_ok()
        {
            


            var myJournalPM = new JournalPM()
            {
                AccountingDate = new DateTime(2016, 3, 1), //ClosedMonth = 1, OpenMonth = 9 
                StatusCode = "6",
                JournalLines = new List<JournalLinePM>() {
                new JournalLinePM(){
                    Line=1,
                    ActionTypeCode="1",
                    LocalAmount=3 ,
                     ExchangeRate=3,
                     ForeignAmount=1,

                     //DebitAccountId="1-1"//, 
                     

                     AccountingDate = new DateTime(2016, 3, 1),
                     DueDate = new DateTime(2016, 3, 1),
                     DocumentDate = new DateTime(2016, 3, 1),
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"

                },
                new JournalLinePM(){
                    Line=2,
                    ActionTypeCode="2",
                    LocalAmount=3 ,
                     ExchangeRate=1,
                     ForeignAmount=3,



                     AccountingDate = new DateTime(2016, 3, 1),
                     DueDate = new DateTime(2016, 3, 1),
                     DocumentDate = new DateTime(2016, 3, 1),
                     DebitAccountId="Card1-1", CreditAccountId="Card1-1"

                }
            }
            };

            var myNewJournalValidatorContext = GetValidationContext(myJournalPM);


            bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
            var journalApproveParser = new JournalApproveParser(myJournalPM, false,
                myNewJournalValidatorContext//GetValidationContext(myJournalPM)
                );
            journalApproveParser.CreateLedger_MapByJournalActionType();
            Assert.AreEqual(2, journalApproveParser.LedgerTransactions.Count);
            Assert.AreEqual("Card1-1", journalApproveParser.LedgerTransactions[0].AccountId);
            Assert.AreEqual(3M, journalApproveParser.LedgerTransactions[0].LocalAmountCredit);

        }
    }
}
