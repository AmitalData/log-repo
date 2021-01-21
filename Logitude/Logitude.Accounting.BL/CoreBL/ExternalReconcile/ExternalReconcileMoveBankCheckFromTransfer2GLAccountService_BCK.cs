#if adjustfeature
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class ExternalReconcileMoveBankCheckFromTransfer2GLAccountService
    {
        private IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        public const string M_LedgerNotInTransferBank = "התנועה איננה בחשבון בנק לשלם ";
        public const string M_InputPageLineNotInTransferBank = "הדף איננה בחשבון בנק לשלם ";
        public const string M_BankBelongtoDifferentBankThanLedger = "אין תאימות דף הבנק שייך לבנק אחר ";


        public const string M_AmountInPageAndLedgerMustBeEqual =
                    "סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות";
        public const string M_CheckAmountInPageMustBeInDebit = "סכום החובה בדף בנק חייב להיות גדול מאפס כנדרש בצק";
        public const string M_LedgerAlreadyHaveExternalReconcile = "התנועה מסומנת שהותאמה כבר חיצונית";
        public const string M_InProgressExternalReconcile_Ledger = "התנועה מסומנת בתהליך התאמה חצונית";
        public const string M_InProgressExternalReconcile_Page = "השורה בדף מסומנת בתהליך התאמה חצונית ";
        public JournalPM TheJournalPM { get; private set; }
        

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        /// <summary>
        // המחאה לשלם נתתי לספק המחאה דחויה  -paymentcheques
        //  ההמחאה תעבור בזכות לבנק לשלם BANK TRANSFER 
        // בחובה לאחר ימים מספר הספק הפקיד את ההמחאה ובדפי הבנק מצאתי שורה לפרעון !!!!
        ///  מתחילים .....
        /// בתהליך שבו מעבירים את ההמחאה מכרטיס "לשלם" לכרטיס "עוש" נוצרת פק' יומן 
        ///             journalLine.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
        ///             journalLine.CreditAccountId = myBankAccountPM.GLAccountId;

        /// 
        /// Validate:
        //        err.Add("התנועה איננה בחשבון בנק לשלם ");
        //        err.Add("הדף איננה בחשבון בנק לשלם ");
        //        err.Add("אין תאימות דף הבנק שייך לבנק אחר הנשלף מהתנועה");
        //       err.Add("סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות");
        //        err.Add("סכום החובה בדף בנק חייב להיות גדול מאפס כנדרש בצק");
        //        err.Add("התנועה מסומנת בתהליך התאמה חצונית");
        //        err.Add("התנועה מסומנת שהותאמה כבר חיצונית");
        //        err.Add("השורה בדף מסומנת שהותאמה כבר חיצונית");
        //        err.Add("השורה בדף מסומנת בתהליך התאמה חצונית");
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="ledgerTransactionId"></param>
        /// <param name="reconcileExternalPageLineId"></param>
        /// CreateJournalWithExtReconcile
        /// MoveBankCheckFromTransfer2GLAccount
        public void CreateJournalWithExtReconcile(int tenant, string ledgerTransactionBankTransferId, string reconcileExternalPageLineId)
        {
            LedgerTransactionPM myLedgerTransactionBankTransferPM;
            BankAccountPM bankAccountFromTransfer;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            string errString;
            PrepareAndValidate(tenant,true, ledgerTransactionBankTransferId, reconcileExternalPageLineId, out myLedgerTransactionBankTransferPM, out bankAccountFromTransfer, out myReconcileExternalPageLinePM, out errString);

            if (!string.IsNullOrWhiteSpace(errString))
            {
                throw new Exception(errString);
            }

            JournalPM journal = CreateJournal(myLedgerTransactionBankTransferPM, myReconcileExternalPageLinePM, bankAccountFromTransfer);
            if (journal.JournalExternalReconciles.Count > 1)
            {
                throw new Exception("Sorry meanwhile only one Adjust Allowed !!!");
            }
            TheJournalPM = journal;
        }

        public void PrepareAndValidate(int tenant, bool CheckINprogress , string ledgerTransactionBankTransferId, string reconcileExternalPageLineId, out LedgerTransactionPM myLedgerTransactionBankTransferPM, out BankAccountPM bankAccountFromTransfer, out ReconcileExternalPageLinePM myReconcileExternalPageLinePM, out string errString)
        {
            if (string.IsNullOrWhiteSpace(ledgerTransactionBankTransferId))
            {
                throw new Exception("ledgerTransactionBankTransferId is must");
            }

            if (string.IsNullOrWhiteSpace(reconcileExternalPageLineId))
            {
                throw new Exception("reconcileExternalPageLineId is must");
            }
            myLedgerTransactionBankTransferPM = _ExternalReconcileDataProvider.GetLedgerTransactionList(new List<string>() { ledgerTransactionBankTransferId }, tenant).FirstOrDefault();
            if (myLedgerTransactionBankTransferPM == null)
            {
                throw new Exception("Ledger not exist ");
            }
            bankAccountFromTransfer = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(myLedgerTransactionBankTransferPM.AccountId, tenant);
            BankAccountPM bankAccountFromReconcileExternalPageLine = _ExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(reconcileExternalPageLineId, tenant);

            myReconcileExternalPageLinePM = this._ExternalReconcileDataProvider.GetReconcileExternalPageLinePM(tenant, reconcileExternalPageLineId);


            errString = Validate(tenant, CheckINprogress, myLedgerTransactionBankTransferPM, myReconcileExternalPageLinePM, bankAccountFromTransfer, bankAccountFromReconcileExternalPageLine);
        }

        private JournalPM CreateJournal(LedgerTransactionPM myLedgerTransactionTransferPM, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, BankAccountPM myBankAccountPM)
        {

            //if (string.IsNullOrWhiteSpace(myReconcileExternalPageLinePM.Reference))
            //{
            //    throw new Exception("if (string.IsNullOrWhiteSpace(myReconcileExternalPageLinePM.Reference))");
            //}

            //newJournalMoveBankCheckFromTransfer2GLAccount/
            var journal = new JournalPM()
            {

            };
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalReconciles.Add()
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalLines.Add
            journal.Tenant = myLedgerTransactionTransferPM.Tenant;
            journal.Id = "new";
            journal.JournalNumber = "1";
            journal.CreateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.AccountingDate = //theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : 
                                     //_ExternalReconcileDataProvider.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
                myReconcileExternalPageLinePM.ReferenceDate;
            journal.TypeCode = "0";

            bool testedAndFoundAllOK = true;
            if (testedAndFoundAllOK)
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Approved;
            }
            else
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            }


            journal.CreatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(myLedgerTransactionTransferPM.Tenant);  // _ExternalReconcileDataProvider.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.AccountingEntityCode = "6"; journal.AccountingEntityCode = ""; // Cheque Deposit//
            journal.AccountingEntityCode = "12";//if  AccountingEntityCode = "6" crush while aRPaymentCheque.StatusCode = "6"; due aRPaymentCheque not found !!


            //journal.AccountingEntityId = theEntityPm.Id;
            journal.AccountingEntityReference = "";// myReconcileExternalPageLinePM.Reference;
            journal.UpdateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.UpdatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.ApproveDate = _ExternalReconcileDataProvider.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
            journal.ApprovedByUserId = _ExternalReconcileDataProvider.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;

            journal.ChangeSetOp = ChangeSetOperation.Insert;
            const string MyNotes = "פרעון שיק מהתאמה";

            JournalLinePM journalLineDebitTransfer = new JournalLinePM();
            journalLineDebitTransfer.Tenant = myLedgerTransactionTransferPM.Tenant;
            journalLineDebitTransfer.JournalId = journal.Id;
            journalLineDebitTransfer.Line = 1;
            journalLineDebitTransfer.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
            //journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
            journalLineDebitTransfer.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.DueDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.AccountingDate = journal.AccountingDate;
            journalLineDebitTransfer.LocalAmount = myLedgerTransactionTransferPM.LocalAmountCredit;
            journalLineDebitTransfer.CurrencyId = myLedgerTransactionTransferPM.CurrencyId;
            journalLineDebitTransfer.ForeignAmount = myLedgerTransactionTransferPM.ForeignAmountCredit;
            //journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;

            journalLineDebitTransfer.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineDebitTransfer.CreditAccountId = myBankAccountPM.GLAccountId;
            //journalLine.CreditControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId;
            journalLineDebitTransfer.Notes = MyNotes  + " " + myReconcileExternalPageLinePM.Notes;

            SetReference(myLedgerTransactionTransferPM, myReconcileExternalPageLinePM, journalLineDebitTransfer);

            journalLineDebitTransfer.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineDebitTransfer);



            JournalLinePM journalLineCreditBankGLId = new JournalLinePM();
            journalLineCreditBankGLId.Tenant = myLedgerTransactionTransferPM.Tenant;
            journalLineCreditBankGLId.JournalId = journal.Id;
            journalLineCreditBankGLId.Line = 1;
            journalLineCreditBankGLId.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;

            journalLineCreditBankGLId.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.DueDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.AccountingDate = journal.AccountingDate;
            journalLineCreditBankGLId.LocalAmount = myLedgerTransactionTransferPM.LocalAmountCredit;
            journalLineCreditBankGLId.CurrencyId = myLedgerTransactionTransferPM.CurrencyId;
            journalLineCreditBankGLId.ForeignAmount = myLedgerTransactionTransferPM.ForeignAmountCredit;
            journalLineCreditBankGLId.Reference1 = myReconcileExternalPageLinePM.Reference;
            journalLineCreditBankGLId.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineCreditBankGLId.CreditAccountId = myBankAccountPM.GLAccountId;
            journalLineCreditBankGLId.Notes = MyNotes;
            SetReference(myLedgerTransactionTransferPM, myReconcileExternalPageLinePM, journalLineCreditBankGLId);
            journalLineCreditBankGLId.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineCreditBankGLId);

            TryCreateInternalReconcileIfNotReconcile(myLedgerTransactionTransferPM, journal);

            journal.JournalExternalReconciles.Add(new JournalExternalReconcilePM()
            {
                Tenant = journal.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                JournalId = journal.Id,
                Line = 1,
                LedgerTransactionId = myLedgerTransactionTransferPM.Id,
                ReconcileExternalPageLineId = myReconcileExternalPageLinePM.Id
            });

            return journal;
        }

        private  void SetReference(LedgerTransactionPM myLedgerTransactionTransferPM, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, JournalLinePM journalLine)
        {
            var hash = new HashSet<string>();
            AddRef(hash,myReconcileExternalPageLinePM.Reference);

            AddRef(hash, myLedgerTransactionTransferPM.Reference2);
            AddRef(hash, myLedgerTransactionTransferPM.Reference1);
            AddRef(hash, myLedgerTransactionTransferPM.Reference3);

            var list = hash.ToList();
            var list2 = hash.ToList();
            int i = 1;
            while (list2.Count > 0 || i > 5)
            {
                string myref= list[i - 1];
                switch (i)
                {
                    case 1: { journalLine.Reference1 = myref; break; }
                    case 2: { journalLine.Reference2 = myref; break; }
                    case 3: { journalLine.Reference3 = myref; break; }
                    default:
                        {
                            journalLine.Reference1 = journalLine.Reference1 ?? "";
                            journalLine.Reference1 += " " + myref;
                        }
                        break;
                }
                list2.RemoveAt(0);
                i++;
            }


        }
        void AddRef(HashSet<string> list,string add)
        {
            list = list ?? new HashSet<string>();
            if (!string.IsNullOrWhiteSpace(add))
            {
                list.Add(add);
            }
        }
        private static void TryCreateInternalReconcileIfNotReconcile(LedgerTransactionPM myLedgerTransactionTransferPM, JournalPM journal)
        {
            //throw new Exception("TryCreateInternalReconcileIfNotReconcile");
            if (myLedgerTransactionTransferPM.IsReconciled)
            {
                LogMessagingUtil.Instance.AppendLine("SuppressCreateInternalReconcile:DUE myLedgerTransactionTransferPM.IsReconciled");
                return;
            }
            if (Math.Abs( myLedgerTransactionTransferPM.OpenAmount) != Math.Abs(myLedgerTransactionTransferPM.LocalAmountCredit))
            {
                LogMessagingUtil.Instance.AppendLine("SuppressCreateInternalReconcile:DUE myLedgerTransactionTransferPM.OpenAmount != myLedgerTransactionTransferPM.LocalAmountCredit");
                return;
            }
            journal.JournalReconciles.Add(new JournalReconcilePM()
            {
                Tenant = journal.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                JournalId = journal.Id,
                Line = 1,
                LedgerTransactionId = myLedgerTransactionTransferPM.Id,
                CurrencyId = myLedgerTransactionTransferPM.OpenAmountCurrencyId,
                //ReconciliationAmount = myLedgerTransactionTransferPM.LocalAmountCredit *-1,
                ReconciliationAmount = myLedgerTransactionTransferPM.OpenAmount,
                IsPartial = false
            });
        }

        private string Validate(int tenant, bool CheckINprogress,
            LedgerTransactionPM myLedgerTransactionBankTransferPM, ReconcileExternalPageLinePM myReconcileExternalPageLinePM,
            BankAccountPM bankAccountFromTransfer, BankAccountPM bankAccountFromReconcileExternalPageLine)
        {
            var err = new List<string>();
            if (myLedgerTransactionBankTransferPM.Tenant != tenant)
            {
                err.Add("myLedgerTransaction.Tenant!= tenant");
            }
            if (myReconcileExternalPageLinePM.Tenant != tenant)
            {
                err.Add("myReconcileExternalPageLinePM.Tenant!= tenant");
            }
            if (bankAccountFromTransfer == null)
            {
                err.Add(M_LedgerNotInTransferBank);//"התנועה איננה בחשבון בנק לשלם "
            }
            if (bankAccountFromReconcileExternalPageLine == null)
            {
                err.Add(M_InputPageLineNotInTransferBank);//"הדף איננה בחשבון בנק לשלם ");
            }
            
            if (bankAccountFromTransfer != null && bankAccountFromReconcileExternalPageLine != null &&
                bankAccountFromReconcileExternalPageLine.Id != bankAccountFromTransfer.Id)
            {
                err.Add(
                    M_BankBelongtoDifferentBankThanLedger//"אין תאימות דף הבנק שייך לבנק אחר הנשלף מהתנועה"
                    );
            }
            if (bankAccountFromTransfer != null && this._ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant)== bankAccountFromTransfer.CurrencyId)
            {
                if (myLedgerTransactionBankTransferPM.LocalAmountCredit != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                {
                    err.Add(
                        M_AmountInPageAndLedgerMustBeEqual//"סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות"
                        );
                }
            }
            else
            {
                if (myLedgerTransactionBankTransferPM.ForeignAmountCredit != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                {
                    err.Add(
                        M_AmountInPageAndLedgerMustBeEqual//"סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות"
                        );
                }
            }
         
            if (myReconcileExternalPageLinePM.DebitAmount <= 0)
            {
                err.Add(
                    M_CheckAmountInPageMustBeInDebit//"סכום החובה בדף בנק חייב להיות גדול מאפס כנדרש בצק"
                    );
            }



            //if (myReconcileExternalPageLinePM.IsReconciled)
            //{
            //    err.Add("השורה בדף מסומנת שהותאמה כבר חיצונית");
            //}
            if (myLedgerTransactionBankTransferPM.IsExternalReconcile)
            {
                err.Add(
                    M_LedgerAlreadyHaveExternalReconcile
                    //"התנועה מסומנת שהותאמה כבר חיצונית"

                    );
            }
            if (CheckINprogress)
            {
                CheckInProgressOnlyWhileJournalCreate(myLedgerTransactionBankTransferPM, myReconcileExternalPageLinePM, err);
            }
            return string.Join(Environment.NewLine, err.ToArray());

        }

        private static void CheckInProgressOnlyWhileJournalCreate(LedgerTransactionPM myLedgerTransactionBankTransferPM, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, List<string> err)
        {

            if (myLedgerTransactionBankTransferPM.InProgressExternalReconcile)
            {
                err.Add(
                    M_InProgressExternalReconcile_Ledger //="התנועה מסומנת בתהליך התאמה חצונית"
                    );
            }


            if (myReconcileExternalPageLinePM.InProgressExternalReconcile)
            {
                err.Add(
                    M_InProgressExternalReconcile_Page// "השורה בדף מסומנת בתהליך התאמה חצונית"
                    );
            }
        }



        //private LedgerTransactionPM GetLedgerTransactionPM(int tenant, string ledgerTransactionId)
        //{
        //    var qs = new LedgerTransactionQueryService(tenant);
        //    var LedgerTransactionPM=qs.GetSingle(ledgerTransactionId,false,false);
        //    return LedgerTransactionPM;
        //}

        

    }
}


#endif