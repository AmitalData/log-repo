using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.Resolvers;
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
        private bool _AdjustAsBankFee;
        private string _OnAdjust_adjustGLAccountId;
        private string _ScreenNotes;
        public const string M_LedgerNotInTransferBank = "התנועה איננה בחשבון בנק לשלם ";
        public const string M_InputPageLineNotInTransferBank = "הדף איננה בחשבון בנק לשלם ";
        public const string M_BankBelongtoDifferentBankThanLedger = "אין תאימות דף הבנק שייך לבנק אחר ";

#if adjustfeature
        public const string M_AmountInPageAndLedgerMustBeEqual =
                    "סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות";
#endif
        public const string M_OnAdjustMustInit = " סכום החובה בדף בנק שונה מסכום התנועה בכרטסת בנק לשלם בזכות אולם המשתנה כרטיס להפרשים לא אותחל";

        public const string M_CheckAmountInPageMustBeInDebit = "סכום החובה בדף בנק חייב להיות גדול מאפס כנדרש בצק";
        public const string M_LedgerAlreadyHaveExternalReconcile = "התנועה מסומנת שהותאמה כבר חיצונית";
        public const string M_InProgressExternalReconcile_Ledger = "התנועה מסומנת בתהליך התאמה חצונית";
        public const string M_InProgressExternalReconcile_Page = "השורה בדף מסומנת בתהליך התאמה חצונית ";
        public const string M_AdjustAccoutMustBeDiffFromBank = "החשבון להפרשים חייב להיות שונה מהבנק";
        public JournalPM TheJournalPM { get; private set; }
        

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        public void OnAdjustMustInit(string adjustGLAccountId, string screenNotes)
        {
            //screenNotes = "התאמת דף בנק (עמלה)";
            _OnAdjust_adjustGLAccountId = adjustGLAccountId;
            _ScreenNotes = screenNotes;
        }
        /// <summary>
        // המחאה לשלם נתתי לספק המחאה דחויה  -paymentcheques
        //  ההמחאה תעבור בזכות לבנק לשלם BANK TRANSFER 
        // בחובה לאחר ימים מספר הספק הפקיד את ההמחאה ובדפי הבנק מצאתי שורה לפרעון !!!!
        ///  מתחילים .....
        /// בתהליך שבו מעבירים את ההמחאה מכרטיס "לשלם" לכרטיס "עוש" נוצרת פק' יומן 
        ///             journalLine.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
        ///             journalLine.CreditAccountId = myBankAccountPM.GLAccountId;


        /////20210413 אפשרות התאמת שורת הפקדה אחת בדפי בנק מול שורה אחת ויותר בבנק הדחויים 

        /// 
        /// Validate:
        //        err.Add("התנועה איננה בחשבון בנק לשלם ");
        //        err.Add("הדף איננה בחשבון בנק לשלם ");
        //        err.Add("אין תאימות דף הבנק שייך לבנק אחר הנשלף מהתנועה");
        // ****revert-AdjustAllowed****      err.Add("סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות");
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
        /// 
        /*
 * בנק לשלם כרטיס 100 בזכות -תמיד בזכות
 *  - דף בנק 101 בחובה 
 * 
 * פקודת יומן 
 * חייב את בנק לשלם ב 100 -JLINE1
 * זכה את העוש ב 100-JLINE2
 * 
 * באם
 * יש הפרש של שקל עמלה 
 * זכה את הבנק עוש ב  שקל-JLINE3
 * חייב את כ ההפרשים ב שקל -JLINE4
 * 
 * 
 * בהעברה להנהח 
 * 
 * יוצר התאמה ראשונה מול הבנק לשלם 
 *   JLINE1 יוצר תנועה חדשה לחובת בנק לשלם100 
 *   מול התנועה הישנה שבזכות 100
 *
 * 
 * באם אין הפרשים A
 *יוצר התאמה שניה מול הבנק בעוש 
 * תנועה חדשה מזכה את העוש ב 100 JLINE2
 * 100  מול השורה של ההמחאה שנפרע  בדף הבנק המקורית
 * 
 *
 *באם יש ההפרש להתאמה דאז B
 * התאמה מול בנק העוש
 * 
 *  תנועה חדשה מזכה את העוש ב 100 JLINE2
 *  תנועה חדשה  משורה JLINE3  בזכות
 *  על שקל אחד 
 *  
 * 101  מול השורה של ההמחאה שנפרע  בדף הבנק המקורית בחובה 
 * /////20210413 אפשרות התאמת שורת הפקדה אחת בדפי בנק מול שורה אחת ויותר בבנק הדחויים 
 * 
 * 
 */

        public void CreateJournalWithExtReconcile(int tenant, List<string> ledgerTransactionBankTransferIdS, string reconcileExternalPageLineId)
        {
            List<LedgerTransactionPM> myLedgerTransactionBankTransferPMs;
            BankAccountPM bankAccountFromTransfer;
            ReconcileExternalPageLinePM myReconcileExternalPageLinePM;
            string errString;
            PrepareAndValidate(tenant,true, ledgerTransactionBankTransferIdS, reconcileExternalPageLineId, out myLedgerTransactionBankTransferPMs, out bankAccountFromTransfer, out myReconcileExternalPageLinePM, out errString);

            if (!string.IsNullOrWhiteSpace(errString))
            {
                throw new ApplicationException(errString);
            }

            JournalPM journal = CreateJournal(myLedgerTransactionBankTransferPMs, myReconcileExternalPageLinePM, bankAccountFromTransfer, ledgerTransactionBankTransferIdS);
            if (journal.JournalExternalReconciles.Count > 1)
            {
                //throw new ApplicationException("Sorry meanwhile only one Adjust Allowed !!!");
            }
            TheJournalPM = journal;
        }

        public void PrepareAndValidate(int tenant, bool CheckINprogress , List<string> ledgerTransactionBankTransferIdS, string reconcileExternalPageLineId, out List<LedgerTransactionPM> myLedgerTransactionBankTransferPMS, out BankAccountPM bankAccountFromTransfer, out ReconcileExternalPageLinePM myReconcileExternalPageLinePM, out string errString)
        {
            //if (string.IsNullOrWhiteSpace(ledgerTransactionBankTransferIdS))
            if (ledgerTransactionBankTransferIdS.Count<1)
            {
                throw new ApplicationException("ledgerTransactionBankTransferId is must");
            }

            if (string.IsNullOrWhiteSpace(reconcileExternalPageLineId))
            {
                throw new ApplicationException("reconcileExternalPageLineId is must");
            }
            ///myLedgerTransactionBankTransferPMS = _ExternalReconcileDataProvider.GetLedgerTransactionList(new List<string>() { ledgerTransactionBankTransferIdS }, tenant).FirstOrDefault();
            myLedgerTransactionBankTransferPMS = _ExternalReconcileDataProvider.GetLedgerTransactionList( ledgerTransactionBankTransferIdS , tenant);
            //if (myLedgerTransactionBankTransferPMS == null)
            if (myLedgerTransactionBankTransferPMS.Count != ledgerTransactionBankTransferIdS.Count)
            {
                throw new ApplicationException("Ledger not exist ");
            }
            var allTransAccouts=myLedgerTransactionBankTransferPMS.Select(r => r.AccountId).Distinct().ToList();
            if (allTransAccouts.Count>1)
            {
                throw new ApplicationException("All Ledgers must be in the same transfer bank accout ");
            }

            var allTransCurrencyIds = myLedgerTransactionBankTransferPMS.Select(r => r.CurrencyId).Distinct().ToList();
            if (allTransCurrencyIds.Count > 1)
            {
                throw new ApplicationException("All Ledgers must be in the same CurrencyId ");
            }

            string bankTransferAccountId= allTransAccouts.FirstOrDefault();
            bankAccountFromTransfer = _ExternalReconcileDataProvider.GetBankAccountFromTransferAccount(/*myLedgerTransactionBankTransferPMS.AccountId*/bankTransferAccountId, tenant);
            BankAccountPM bankAccountFromReconcileExternalPageLine = _ExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(reconcileExternalPageLineId, tenant);

            myReconcileExternalPageLinePM = this._ExternalReconcileDataProvider.GetReconcileExternalPageLinePM(tenant, reconcileExternalPageLineId);


            errString = Validate(tenant, CheckINprogress, myLedgerTransactionBankTransferPMS, myReconcileExternalPageLinePM, bankAccountFromTransfer, bankAccountFromReconcileExternalPageLine);
        }

        private JournalPM CreateJournal(List<LedgerTransactionPM> myLedgerTransactionTransferPMs, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, BankAccountPM myBankAccountPM, List<string> ledgerTransactionBankTransferIdS)
        {

            //if (string.IsNullOrWhiteSpace(myReconcileExternalPageLinePM.Reference))
            //{
            //    throw new ApplicationException("if (string.IsNullOrWhiteSpace(myReconcileExternalPageLinePM.Reference))");
            //}

            //newJournalMoveBankCheckFromTransfer2GLAccount/
            var journal = new JournalPM()
            {

            };
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalReconciles.Add()
            //newJournalMoveBankCheckFromTransfer2GLAccount.JournalLines.Add
            journal.Tenant = myLedgerTransactionTransferPMs.First().Tenant;
            journal.Id = "new";
            journal.JournalNumber = "1";
            journal.CreateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(journal.Tenant);
            journal.AccountingDate = //theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : 
                                     //_ExternalReconcileDataProvider.GetCurrentDateTime(myLedgerTransactionTransferPM.Tenant);
                myReconcileExternalPageLinePM.ReferenceDate;
            journal.TypeCode = "0";

            bool testedAndFoundAllOK = true;
            if (testedAndFoundAllOK)
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.InProcessing;
            }
            else
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            }


            journal.CreatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(journal.Tenant);  // _ExternalReconcileDataProvider.ResolveUserId(myLedgerTransactionTransferPM.Tenant); ;
            journal.AccountingEntityCode = "6"; journal.AccountingEntityCode = ""; // Cheque Deposit//
            journal.AccountingEntityCode = "12";//if  AccountingEntityCode = "6" crush while aRPaymentCheque.StatusCode = "6"; due aRPaymentCheque not found !!


            //journal.AccountingEntityId = theEntityPm.Id;
            journal.AccountingEntityReference = "";// myReconcileExternalPageLinePM.Reference;
            journal.UpdateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(journal.Tenant);
            journal.UpdatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(journal.Tenant); ;
            journal.ApproveDate = _ExternalReconcileDataProvider.GetCurrentDateTime(journal.Tenant);
            journal.ApprovedByUserId = _ExternalReconcileDataProvider.ResolveUserId(journal.Tenant); ;

            journal.ChangeSetOp = ChangeSetOperation.Insert;
            const string MyNotes = "פרעון שיק מהתאמה";

            JournalLinePM journalLineDebitTransfer = new JournalLinePM();
            journalLineDebitTransfer.Tenant = journal.Tenant;
            journalLineDebitTransfer.JournalId = journal.Id;
            journalLineDebitTransfer.Line = 1;
            journalLineDebitTransfer.ActionTypeCodeEnum = JournalActionTypeEnum.Debit;
            //journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
            journalLineDebitTransfer.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.DueDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineDebitTransfer.AccountingDate = journal.AccountingDate;
            //journalLineDebitTransfer.LocalAmount = myLedgerTransactionTransferPMs.LocalAmountCredit;
            journalLineDebitTransfer.LocalAmount = myLedgerTransactionTransferPMs.Sum(r => r.LocalAmountCredit);
            journalLineDebitTransfer.CurrencyId = myLedgerTransactionTransferPMs.First().CurrencyId;
            //journalLineDebitTransfer.ForeignAmount = myLedgerTransactionTransferPMs.ForeignAmountCredit;
            journalLineDebitTransfer.ForeignAmount = myLedgerTransactionTransferPMs.Sum(r => r.ForeignAmountCredit);
            //journalLine.ExchangeRate = (decimal)theEntityPm.InvoiceCurrencyExchangeRate;

            journalLineDebitTransfer.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineDebitTransfer.CreditAccountId = myBankAccountPM.GLAccountId;
            //journalLine.CreditControlAccountId = glAccount == null ? "" : glAccount.ControlAccountId;
            journalLineDebitTransfer.Notes = MyNotes + " " + myReconcileExternalPageLinePM.Notes;

            SetReference(myLedgerTransactionTransferPMs.First(), myReconcileExternalPageLinePM, journalLineDebitTransfer);

            journalLineDebitTransfer.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineDebitTransfer);



            JournalLinePM journalLineCreditBankGLId = new JournalLinePM();
            journalLineCreditBankGLId.Tenant = journal.Tenant;
            journalLineCreditBankGLId.JournalId = journal.Id;
            journalLineCreditBankGLId.Line = 1;
            journalLineCreditBankGLId.ActionTypeCodeEnum = JournalActionTypeEnum.Credit;

            journalLineCreditBankGLId.DocumentDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.DueDate = myReconcileExternalPageLinePM.ReferenceDate;
            journalLineCreditBankGLId.AccountingDate = journal.AccountingDate;
            journalLineCreditBankGLId.LocalAmount = myLedgerTransactionTransferPMs.Sum(r => r.LocalAmountCredit);
            journalLineCreditBankGLId.CurrencyId = myLedgerTransactionTransferPMs.First().CurrencyId;
            journalLineCreditBankGLId.ForeignAmount = myLedgerTransactionTransferPMs.Sum(r => r.ForeignAmountCredit);
            journalLineCreditBankGLId.Reference1 = myReconcileExternalPageLinePM.Reference;
            journalLineCreditBankGLId.DebitAccountId = myBankAccountPM.TransferGLAcccountId;
            journalLineCreditBankGLId.CreditAccountId = myBankAccountPM.GLAccountId;
            journalLineCreditBankGLId.Notes = MyNotes;
            SetReference(myLedgerTransactionTransferPMs.First(), myReconcileExternalPageLinePM, journalLineCreditBankGLId);
            journalLineCreditBankGLId.ChangeSetOp = ChangeSetOperation.Insert;
            journal.JournalLines.Add(journalLineCreditBankGLId);

            AdjustAsBankFee(myReconcileExternalPageLinePM, ledgerTransactionBankTransferIdS, journal);

            int JournalExternalReconcileLine = 1;
            foreach (var myLedgerTransactionTransferPM in myLedgerTransactionTransferPMs)
            {

                bool first = JournalExternalReconcileLine == 1;
                TryCreateInternalReconcileIfNotReconcile(myLedgerTransactionTransferPM, journal);

                journal.JournalExternalReconciles.Add(new JournalExternalReconcilePM()
                {
                    Tenant = journal.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,

                    JournalId = journal.Id,
                    Line = JournalExternalReconcileLine++,
                    LedgerTransactionId = myLedgerTransactionTransferPM.Id,
                    ReconcileExternalPageLineId = first ? myReconcileExternalPageLinePM.Id : null
                });
            }
            return journal;
        }

        private void AdjustAsBankFee(ReconcileExternalPageLinePM myReconcileExternalPageLinePM, List<string> ledgerTransactionBankTransferIds, JournalPM journal)
        {
            if (!_AdjustAsBankFee)
            {
                return;
            }

            var adjustDueBankFeesCreateJournal = new AdjustDueBankFees.CreateJournal();
            adjustDueBankFeesCreateJournal.MustInit(_ExternalReconcileDataProvider, journal);

            adjustDueBankFeesCreateJournal.CreateOnlyJournalLines(
                new List<string>() { myReconcileExternalPageLinePM.Id }, /*new List<string>() {*/ ledgerTransactionBankTransferIds /*}*/,

                _OnAdjust_adjustGLAccountId,
                _ScreenNotes, journal.AccountingDate

                );
            bool testedAndFoundAllOK = true;
            if (testedAndFoundAllOK)
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.InProcessing;
            }
            else
            {
                journal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            }
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
            //throw new ApplicationException("TryCreateInternalReconcileIfNotReconcile");
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
            List<LedgerTransactionPM> myLedgerTransactionBankTransferPMs, ReconcileExternalPageLinePM myReconcileExternalPageLinePM,
            BankAccountPM bankAccountFromTransfer, BankAccountPM bankAccountFromReconcileExternalPageLine)
        {
            var err = new List<string>();
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(tenant);
            //if (myLedgerTransactionBankTransferPMs.Tenant != tenant)
            if (myLedgerTransactionBankTransferPMs.Any(r => r.Tenant != tenant))
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
                //if (myLedgerTransactionBankTransferPMs.LocalAmountCredit != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                if (myLedgerTransactionBankTransferPMs.Sum(r=>r.LocalAmountCredit) != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                {
                    _AdjustAsBankFee = true;
                    //err.Add(
                    //    M_AmountInPageAndLedgerMustBeEqual//"סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות"
                    //    );
                }
            }
            else
            {
                //if (myLedgerTransactionBankTransferPMs.ForeignAmountCredit != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                if (myLedgerTransactionBankTransferPMs.Sum(r=>r.ForeignAmountCredit) != myReconcileExternalPageLinePM.DebitAmount) // WI 65377
                {
                    _AdjustAsBankFee = true;
                    //err.Add(
                    //    M_AmountInPageAndLedgerMustBeEqual//"סכום החובה בדף בנק חייב להיות זהה לסכום התנועה בכרטסת בנק לשלם בזכות"
                    //    );
                }
            }
            if (_AdjustAsBankFee && String.IsNullOrWhiteSpace(_OnAdjust_adjustGLAccountId))
            {
                string msg = TextCodesTranslator.TranslateText("ExternalReconciliation.O.CantReconcileTransferTransactionsWithMultipleBankPages", tenant, showLocal);
                err.Add(msg);
                // err.Add(
                //    M_OnAdjustMustInit// " סכום החובה בדף בנק שונה מסכום התנועה בכרטסת בנק לשלם בזכות אולם המשתנה כרטיס להפרשים לא אותחל";
                //     );
            }
            if (!String.IsNullOrWhiteSpace(_OnAdjust_adjustGLAccountId) && bankAccountFromTransfer != null &&  _OnAdjust_adjustGLAccountId == bankAccountFromTransfer.Id)
            {
                
                err.Add(
                    M_AdjustAccoutMustBeDiffFromBank// = "החשבון להפרשים חייב להיות שונה מהבנק";
                    );

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
            //if (myLedgerTransactionBankTransferPMs.IsExternalReconcile)
            if (myLedgerTransactionBankTransferPMs.Any(r=>r.IsExternalReconcile))
            {
                err.Add(
                    M_LedgerAlreadyHaveExternalReconcile
                    //"התנועה מסומנת שהותאמה כבר חיצונית"

                    );
            }
            if (CheckINprogress)
            {
                CheckInProgressOnlyWhileJournalCreate(myLedgerTransactionBankTransferPMs, myReconcileExternalPageLinePM, err);
            }
            return string.Join(Environment.NewLine, err.ToArray());

        }

        private static void CheckInProgressOnlyWhileJournalCreate(List<LedgerTransactionPM> myLedgerTransactionBankTransferPMs, ReconcileExternalPageLinePM myReconcileExternalPageLinePM, List<string> err)
        {

            //if (myLedgerTransactionBankTransferPMs.InProgressExternalReconcile)
            if (myLedgerTransactionBankTransferPMs.Any(r=>r.InProgressExternalReconcile))
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
