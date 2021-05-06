using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    /// <summary>
    /// *** התאמת עמלות בנק **
    //1---  //   ניתן לבחור שורה אחת או יותר מדפי בנק - חובה וגם זכות
    // ללא תנעות !!!!


    // 2 ****change***  Task 69959: התאמה חיצונית - תמיכה בסגירת הפרש בין כרטיס ודף בנק
    //  ניתן לבחור שורה אחת מדפי בנק - חובה וגם זכות
    //כנגד השורה הנל ניתן לבחור רשימה של תנעות  
    //התנעות כולם אמורות להיות מהעוש  
    //  לללללא מלשלם!!!!



    //  יפתח מסך נתוני התאמה
    //כרטיס לבחירה כל הסוגים למעט 6- עובדים
    //הערה - ילקח מהשורה הראשונה
    //ניתן כאמור בכל השדות ניתנות לשינוים
    //
    //בבנית הפקודה ילקח האסמכתא 1 + 2+ 3 אם יש + הערה מהמסך באם ריק ירשם ברירת מחדל :התאמת דף בנק(עמלה)
    //
    //
    //סוג פקודה חדש :12  בנק התאמה BANK Adjustment
    //
    //
    //לכל שורת בנק יפתח שורה הופכית בפקודה
    //דף בנק חובה 100 -  BANK_GLACCOUT 100 זכות -ולהפך
    //***תיקון יווצר קיבוץ  לשורה אחת בלבד  והנגדית שלה   ********
    //
    //מול כרטיס לבחירה כל הסוגים למעט 6- עובדים יצטבר לשורה 1 !
    //בהעברה ל הנה"ח יבוצע התאמה חיצונית 1 
    /// </summary>
    public class ExternalReconcileAdjustBankFeesService: 
        IExternalReconcileAdjustBankFeesService, IExternalReconcileAdjustBankFees_Validate
    {
        private IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        private List<string> _ErrorList;
        public const string M_TotalPLineNotZero="סך השורות הינו אפס -אין מה להתאים ";
        public const string M_IsReconciled="אחת השורות כבר הותאמה חיצונית";
        public const string M_NotAllPageLineIdsInTheSameBankAccount="לא כל שורות הבנק שייכים לאותו בנק";
        public const string M_NotAllPageLineIdsFoundInDB = "לא כל השורות הבנק נמצאו בשרת ה DB";

        public const string M_NotAllLedgerInTheSameBankAccount = "לא כל התנעות שייכות לאותו בנק";
        public const string M_NotAllLedgerIdsFoundInDB = "לא כל התנעות נמצאו בשרת ה DB";

        public const string M_WhileCreating_All_NotInProgressExternalReconcile = "אחת השורות בתהליך התאמה חצונית";
        public const string M_WhileStreaming_All_InProgressExternalReconcile = "אחת השורות לא מסומנת -בתהליך התאמה חצונית";
        public const string M_AccountShouldBeTheSameToBank = "הכרטיס בדף אמור להיות זהה ";
        public const string M_AdjustLadgerOnly1ExternalPageLineId = "בהתאמת תנועות יש לספק רק שורת דף בנק אחת";
        public const string M_AdjustAccoutMustBeDiffFromBank = "החשבון להפרשים חייב להיות שונה מהבנק";
        public JournalPM TheNewJournal { get; private set; }
        public ChangeSetOperation ChangeSetOp { get; private set; }

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        //const string MyNotes = "פרעון שיק מהתאמה";
        public void CreateJournalWithExtReconcile(int tenant , List<string> reconcileExternalPageLineIdList, string adjustGLAccountId,string screenNotes
            , DateTime accountingDate
            , List<string> ledgerTransactionIds =null
            )
        {
            ledgerTransactionIds = ledgerTransactionIds ?? new List<string>();
            //if (screenNotes.Equals("null", StringComparison.OrdinalIgnoreCase)){
            //    screenNotes = string.Empty;
            //}
            if (string.IsNullOrWhiteSpace(screenNotes) || screenNotes.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                screenNotes = "התאמת דף בנק (עמלה)";
            }
            ///List<string> reconcileExternalPageLineIdList = new List<string>() { reconcileExternalPageLineId };
            List<ReconcileExternalPageLineList> listOfpageLineList;
            List<ReconcileExternalPageList> listOfpageList;
            string accountingCurrencyId = null;// _ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);
            List<LedgerTransactionPM> ledgerTransactionList = null;
            bool CheckWhileStreaming = false;//we are in create mode !!
            PrapareAndValid(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, out listOfpageLineList, out listOfpageList, CheckWhileStreaming, 
                ledgerTransactionIds,
                out accountingCurrencyId,out ledgerTransactionList
                );

            

            var listOfAccId = listOfpageList.Select(r => r.GLAccountId).ToList();

            List<GLAccountList> ListOfGLAccountList = _ExternalReconcileDataProvider.GetListOfGLAccountList(tenant, listOfAccId);
            var bankGLAccountList = ListOfGLAccountList.First();//must have 

            CreateJournal(tenant, adjustGLAccountId, listOfpageLineList, listOfpageList, bankGLAccountList, accountingCurrencyId, screenNotes, accountingDate, ledgerTransactionList);

        }

        public void PrapareAndValid(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, out List<ReconcileExternalPageLineList> listOfpageLineList, out List<ReconcileExternalPageList> listOfpageList
            ,bool CheckWhileStreaming, 
            List<string> ledgerTransactionIds,
            out string accountingCurrencyId, out List<LedgerTransactionPM> ledgerTransactionList)
        {
            
            accountingCurrencyId = null;
            _ErrorList = new List<string>();
            listOfpageLineList = _ExternalReconcileDataProvider.GetReconcileExternalPageLineList(tenant, reconcileExternalPageLineIdList);
            var reconcileExternalPageIdList = listOfpageLineList.Select(r => r.ReconcileExternalPageId).Distinct().ToList();
            listOfpageList = _ExternalReconcileDataProvider.GetReconcileExternalPageList(tenant, reconcileExternalPageIdList);
            ledgerTransactionList =_ExternalReconcileDataProvider.GetLedgerTransactionList(ledgerTransactionIds, tenant);
            var reconcileExternalPageLineId = reconcileExternalPageLineIdList.First();
            var bankGLAccountList = _ExternalReconcileDataProvider.GetBankAccountFromReconcileExternalPageLineId(reconcileExternalPageLineId, tenant);
            accountingCurrencyId =this._ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);

            Validate(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, listOfpageLineList, listOfpageList, CheckWhileStreaming, ledgerTransactionList, ledgerTransactionIds, bankGLAccountList, accountingCurrencyId);
            if (_ErrorList.Count() > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, _ErrorList.ToArray()));
            }
        }

        private void CreateJournal(int tenant, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList, GLAccountList bankGLAccountList, string accountingCurrencyId, string screenNotes, DateTime accountingDate, List<LedgerTransactionPM> ledgerTransactionList)
        {
            CreateJournalHeader(tenant, accountingDate);


           
            //bool accumalateV2 = true;
            //if (accumalateV2)
            //{
                decimal PageLineForeignAmount = listOfpageLineList.Sum(r => r.DebitAmount - r.CreditAmount);
                //decimal PageLineLocalAmount = Convert2LocalAmountFast(tenant, PageLineForeignAmount, bankGLAccountList.CurrencyId, accountingCurrencyId, listOfpageLineList.First().ReferenceDate);
                var ledgerForeignAmount = ledgerTransactionList.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit);
                //var ledgerLocalAmountDebit = ledgerTransactionList.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit);

                decimal foreignTotalAmount = (ledgerForeignAmount + PageLineForeignAmount);
                bool creditTheBank =
                    //(ledgerForeignAmount + PageLineForeignAmount > 0);
                    (foreignTotalAmount > 0);

                
                decimal localTotalAmountConvertFromforeign = Convert2LocalAmountFast(tenant, foreignTotalAmount, bankGLAccountList.CurrencyId, accountingCurrencyId,
                    ///listOfpageLineList.First().ReferenceDate
                    accountingDate
                    );


                var firstJL =
                    //GetFirstJournalLine(tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountList, screenNotes, PageLineForeignAmount, PageLineLocalAmount, ledgerForeignAmount, ledgerLocalAmountDebit, creditTheBank);
                    GetFirstJournalLineSum(tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountList, screenNotes, foreignTotalAmount, localTotalAmountConvertFromforeign, creditTheBank);
                var LstJL =
                    //GetFirstJournalLine(tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountList, screenNotes, PageLineForeignAmount, PageLineLocalAmount, ledgerForeignAmount, ledgerLocalAmountDebit, creditTheBank);
                    GetFirstJournalLineSum(tenant, adjustGLAccountId, listOfpageLineList, bankGLAccountList, screenNotes, foreignTotalAmount, localTotalAmountConvertFromforeign, creditTheBank);
                LstJL.Line = 2;
                LstJL.ActionTypeCodeEnum = creditTheBank ? MyJournalActionTypeEnum.Debit : MyJournalActionTypeEnum.Credit;

                //int line = TheNewJournal.JournalLines.Max(r => r.Line);
                TheNewJournal.JournalLines.Add(firstJL);
                TheNewJournal.JournalLines.Add(LstJL); 

                
            //}
            //else
            //{
            //    CreateJournalLinesFromPageLines(listOfpageLineList, bankGLAccountList, adjustGLAccountId, accountingCurrencyId, screenNotes);
            //    CreateFromLedger(adjustGLAccountId, bankGLAccountList, accountingCurrencyId, screenNotes, ledgerTransactionList);

            //}






            //CreateJournalExternalReco();
            int line = 1;
            TheNewJournal.JournalExternalReconciles = listOfpageLineList.Select(r => GetJournalExternalReconcile(r, ref line)).ToList();

            TheNewJournal.JournalExternalReconciles.AddRange(GetJournalExternalReconcileFromLedger(ledgerTransactionList));
            
            //if (!accumalateV2)
            //{
            //    CreateJournalLineToadjustGLAccountId(adjustGLAccountId, bankGLAccountList);
            //    Accumalation2jounrnalLine();
            //}
            
        }
        private JournalLinePM GetFirstJournalLineSum(int tenant, string adjustGLAccountId, List<ReconcileExternalPageLineList> listOfpageLineList, GLAccountList bankGLAccountList, string screenNotes, decimal ForeignAmount, decimal LocalAmount, bool creditTheBank)
        {
            /*
             *צד הלדג'ר (GLACCOUNT בצד) כל תנועה  בקרדיט
מכפילים במינוס 1
מחברים את כל התנועות (גם הדביט וגם הקרדיט
+ 
צד דפי הבנק כל תנועה  בקרדיט
מכפיל ב מינוס 1
מחבר את כל התונעות_גם הדביט וגם הקרדיט) 

 


אם התוצאה קטנה מאפס יש להכפילה במינוס אחד ולחייב את צד הלדג'ר
אחרת
נזכה את צד הלדג'ר בתוצאה
             */
            return
new JournalLinePM()
{
    Tenant = tenant,
    JournalId = TheNewJournal.Id,
    Line = 1,


    DocumentDate = listOfpageLineList.First().ReferenceDate,
    DueDate = listOfpageLineList.First().ReferenceDate,
    AccountingDate = TheNewJournal.AccountingDate,



    CurrencyId = bankGLAccountList.CurrencyId,



    ///if r.DebitAmount != 0 then credit else debit 
    ActionTypeCodeEnum = creditTheBank ? MyJournalActionTypeEnum.Credit : MyJournalActionTypeEnum.Debit,
    CreditAccountId = creditTheBank ? bankGLAccountList.Id : adjustGLAccountId,
    DebitAccountId = creditTheBank ? adjustGLAccountId : bankGLAccountList.Id,
    LocalAmount = creditTheBank ? (LocalAmount ) : -1 * (LocalAmount),
    ForeignAmount = creditTheBank ? (ForeignAmount) : -1 * (ForeignAmount),
    Notes = screenNotes + Environment.NewLine + listOfpageLineList.First().Notes,
    Reference1 = listOfpageLineList.First().Reference,
    ChangeSetOp = ChangeSetOperation.Insert
};
        }



        private JournalLinePM GetFirstJournalLine(int tenant, string adjustGLAccountId, List<ReconcileExternalPageLineList> listOfpageLineList, GLAccountList bankGLAccountList, string screenNotes, decimal PageLineForeignAmount, decimal PageLineLocalAmount, decimal ledgerForeignAmount, decimal ledgerLocalAmountDebit, bool creditTheBank)
        {
            /*
             *צד הלדג'ר (GLACCOUNT בצד) כל תנועה  בקרדיט
מכפילים במינוס 1
מחברים את כל התנועות (גם הדביט וגם הקרדיט
+ 
צד דפי הבנק כל תנועה  בקרדיט
מכפיל ב מינוס 1
מחבר את כל התונעות_גם הדביט וגם הקרדיט) 

 


אם התוצאה קטנה מאפס יש להכפילה במינוס אחד ולחייב את צד הלדג'ר
אחרת
נזכה את צד הלדג'ר בתוצאה
             */
            return
new JournalLinePM()
{
    Tenant = tenant,
    JournalId = TheNewJournal.Id,
    Line = 1,


    DocumentDate = listOfpageLineList.First().ReferenceDate,
    DueDate = listOfpageLineList.First().ReferenceDate,
    AccountingDate = TheNewJournal.AccountingDate,



    CurrencyId = bankGLAccountList.CurrencyId,

    

    ///if r.DebitAmount != 0 then credit else debit 
    ActionTypeCodeEnum = creditTheBank ? MyJournalActionTypeEnum.Credit : MyJournalActionTypeEnum.Debit,
    CreditAccountId = creditTheBank ? bankGLAccountList.Id : adjustGLAccountId,
    DebitAccountId = creditTheBank ?  adjustGLAccountId: bankGLAccountList.Id,
    LocalAmount = creditTheBank? (PageLineLocalAmount + ledgerLocalAmountDebit) : -1* (PageLineLocalAmount + ledgerLocalAmountDebit),
    ForeignAmount = creditTheBank ? (PageLineForeignAmount + ledgerForeignAmount):-1* (PageLineForeignAmount + ledgerForeignAmount),
    Notes = screenNotes + Environment.NewLine + listOfpageLineList.First().Notes,
    Reference1 = listOfpageLineList.First().Reference,
    ChangeSetOp = ChangeSetOperation.Insert
};
        }

        private void Accumalation2jounrnalLine()
        {

            var jlPage = TheNewJournal.JournalLines.First();
            var jlAdjust = TheNewJournal.JournalLines.Last();

            jlPage.ActionTypeCodeEnum = jlAdjust.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit ? MyJournalActionTypeEnum.Debit : MyJournalActionTypeEnum.Credit;

            jlPage.CreditAccountId = jlAdjust.CreditAccountId;
            jlPage.DebitAccountId = jlAdjust.DebitAccountId;

            jlPage.LocalAmount = jlAdjust.LocalAmount;
            jlPage.ForeignAmount = jlAdjust.ForeignAmount;
            jlPage.ExchangeRate = jlAdjust.ExchangeRate;


            jlAdjust.DocumentDate = jlPage.DocumentDate = TheNewJournal.AccountingDate;//ohad request
            jlAdjust.DueDate = jlPage.DueDate = TheNewJournal.AccountingDate;//ohad request

            jlAdjust.Reference1 = jlPage.Reference1;//ohad request

            if (TheNewJournal.JournalLines.Count == 3)// if the
            {
                 jlPage.Reference2= jlAdjust.Reference2 = TheNewJournal.JournalLines[1].Reference1;//ohad request
            }
                
            if (TheNewJournal.JournalLines.Count > 2)
            {
                int count = (TheNewJournal.JournalLines.Count - 2);
                TheNewJournal.JournalLines.RemoveRange(1, count);
            }
            if (TheNewJournal.JournalLines.Count != 2 ||
                TheNewJournal.JournalLines.First() != jlPage ||
                TheNewJournal.JournalLines.Last() != jlAdjust
                )
            {
                throw new Exception("accumalation failed");
            }
            jlAdjust.Line = 2;


        }

        private List<JournalExternalReconcilePM> GetJournalExternalReconcileFromLedger(List<LedgerTransactionPM> ledgerTransactionList)
        {
            int line = TheNewJournal.JournalExternalReconciles.Max(r => r.Line);
            line++;
            return ledgerTransactionList.Select(
                            ledgerTransaction => new JournalExternalReconcilePM()
                            {
                                Tenant = TheNewJournal.Tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                                JournalId = TheNewJournal.Id,
                                Line = line++,


                                LedgerTransactionId = ledgerTransaction.Id,
                                ReconcileExternalPageLineId = null
                            }).ToList();
        }

        private int CreateFromLedger(string adjustGLAccountId, GLAccountList bankGLAccountList, string accountingCurrencyId, string screenNotes, List<LedgerTransactionPM> ledgerTransactionList)
        {


            int line = TheNewJournal.JournalLines.Max(r => r.Line);
            TheNewJournal.JournalLines.AddRange(
                
                ledgerTransactionList.Select(r =>
            new JournalLinePM()
            {
                Tenant = r.Tenant,
                JournalId = TheNewJournal.Id,
                Line = line++,


                DocumentDate = r.DocumentDate, //r.ReferenceDate,
                DueDate = r.DueDate, //r.ReferenceDate,
                AccountingDate = TheNewJournal.AccountingDate,



                CurrencyId = bankGLAccountList.CurrencyId,

                ForeignAmount = r.LocalAmountDebit != 0 ? r.ForeignAmountDebit : r.ForeignAmountCredit, //r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount,

                ///if r.DebitAmount != 0 then credit else debit 
                ActionTypeCodeEnum = r.LocalAmountDebit != 0 ? MyJournalActionTypeEnum.Credit : MyJournalActionTypeEnum.Debit,
                DebitAccountId = r.LocalAmountDebit != 0 ? adjustGLAccountId : bankGLAccountList.Id,
                CreditAccountId = r.LocalAmountDebit != 0 ? bankGLAccountList.Id : adjustGLAccountId,
                LocalAmount = r.LocalAmountDebit != 0 ? r.LocalAmountDebit : r.LocalAmountCredit, //Convert2LocalAmount(bankGLAccountList.CurrencyId, accountingCurrencyId, r),
                Notes = screenNotes + Environment.NewLine + r.Notes,
                Reference1 = r.Reference1,
                Reference2 = r.Reference2,
                Reference3 = r.Reference2,
                ChangeSetOp = ChangeSetOperation.Insert
            }).ToList()

            );
            return line;
        }


        private JournalExternalReconcilePM GetJournalExternalReconcile(ReconcileExternalPageLineList r, ref int line)
        {
            return new JournalExternalReconcilePM()
            {
                Tenant = TheNewJournal.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                JournalId = TheNewJournal.Id,
                Line = line++,


//                DROP INDEX[IX_ReconcileExternalPageLineId] ON[dbo].[JournalExternalReconciles]
//                ALTER TABLE[dbo].[JournalExternalReconciles] ALTER COLUMN[ReconcileExternalPageLineId][varchar](15) not null
//                CREATE INDEX[IX_ReconcileExternalPageLineId] ON[dbo].[JournalExternalReconciles]([ReconcileExternalPageLineId])


//IF EXISTS(SELECT name FROM sys.indexes WHERE name = N'IX_LedgerTransactionId' AND object_id = object_id(N'[dbo].[JournalExternalReconciles]', N'U'))
//    DROP INDEX[IX_LedgerTransactionId] ON[dbo].[JournalExternalReconciles]
//ALTER TABLE[dbo].[JournalExternalReconciles] ALTER COLUMN[LedgerTransactionId][varchar](15) NULL
//CREATE INDEX[IX_LedgerTransactionId] ON[dbo].[JournalExternalReconciles]([LedgerTransactionId])

                LedgerTransactionId = null,
                ReconcileExternalPageLineId = r.Id
            };
        }

        private void CreateJournalLineToadjustGLAccountId(string adjustGLAccountId, GLAccountList bankGLAccountList)
        {
            decimal totDebitlocal = TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit).Sum(r => r.LocalAmount);
            decimal totCreditlocal = TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit).Sum(r => r.LocalAmount);



            decimal totDebitForiegn = TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit).Sum(r => r.ForeignAmount);
            decimal totCreditForiegn = TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit).Sum(r => r.ForeignAmount);
            MyJournalActionTypeEnum myJournalActionTypeEnum = MyJournalActionTypeEnum.Credit;
            string debitAccountId = bankGLAccountList.Id;
            string creditAccountId = adjustGLAccountId;
            if (totDebitlocal - totCreditlocal < 0)
            {
                myJournalActionTypeEnum =MyJournalActionTypeEnum.Debit;
                debitAccountId = adjustGLAccountId;
                creditAccountId = bankGLAccountList.Id;

            }
            var last=TheNewJournal.JournalLines.Last();
            TheNewJournal.JournalLines.Add(new JournalLinePM
            {
                Tenant = last.Tenant,
                JournalId = TheNewJournal.Id,
                Line = last.Line + 1,


                DocumentDate = last.DocumentDate,
                DueDate = last.DueDate,
                AccountingDate = TheNewJournal.AccountingDate,



                CurrencyId = last.CurrencyId,

                ForeignAmount = Math.Abs(totDebitForiegn - totCreditForiegn),


                ActionTypeCodeEnum = myJournalActionTypeEnum,
                DebitAccountId = debitAccountId,// bankGLAccountList.Id,
                CreditAccountId = creditAccountId,// adjustGLAccountId,
                LocalAmount = Math.Abs(totDebitlocal - totCreditlocal),
                Notes = last.Notes ,
                Reference1 = last.Reference1,
                ChangeSetOp = ChangeSetOperation.Insert

            });
            

            

        }




        private void CreateJournalLinesFromPageLines(List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, GLAccountList bankGLAccountList, string adjustGLAccountId, string accountingCurrencyId, string screenNotes)
        {
            int line = 1;
            TheNewJournal.JournalLines = listOfpageLineList.Select(r =>
            new JournalLinePM()
            {
                Tenant = r.Tenant,
                JournalId = TheNewJournal.Id,
                Line = line++,


                DocumentDate = r.ReferenceDate,
                DueDate = r.ReferenceDate,
                AccountingDate = TheNewJournal.AccountingDate,



                CurrencyId = bankGLAccountList.CurrencyId,

                ForeignAmount = r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount,

                ///if r.DebitAmount != 0 then credit else debit 
                ActionTypeCodeEnum = r.DebitAmount != 0 ? MyJournalActionTypeEnum.Credit : MyJournalActionTypeEnum.Debit,
                DebitAccountId = r.DebitAmount != 0 ?  adjustGLAccountId: bankGLAccountList.Id,
                CreditAccountId = r.DebitAmount != 0 ? bankGLAccountList.Id : adjustGLAccountId,
                LocalAmount = Convert2LocalAmount(bankGLAccountList.CurrencyId, accountingCurrencyId, r),
                Notes = screenNotes + Environment.NewLine+ r.Notes,
                Reference1 =r.Reference,
                ChangeSetOp = ChangeSetOperation.Insert
            }).ToList();
        }


        private decimal Convert2LocalAmount(string currencyId, string accountingCurrencyId, ReconcileExternalPageLineList r)
        {
            decimal foreignAmount = r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount;
            if (currencyId == accountingCurrencyId)
            {
                return foreignAmount;
            }
            var rate =_ExternalReconcileDataProvider.GetLastRateByValueDate(r.Tenant, currencyId, accountingCurrencyId, r.ReferenceDate);
            rate = rate ?? new Logitude.BL.DataContracts.LastRate() { Rate = 1 };
            double? itemCurrencyRateRounded = MethodHelper.Round(rate.Rate, 5);
            itemCurrencyRateRounded = itemCurrencyRateRounded ?? 1;
            decimal local = foreignAmount * (decimal)itemCurrencyRateRounded.GetValueOrDefault();
            return local;
        }
        private decimal Convert2LocalAmountFast(int tenant,decimal foreignAmount ,string currencyId, string accountingCurrencyId, DateTime ReferenceDate)
        {
            //decimal foreignAmount = r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount;
            if (currencyId == accountingCurrencyId)
            {
                return foreignAmount;
            }
            var rate = _ExternalReconcileDataProvider.GetLastRateByValueDate(tenant, currencyId, accountingCurrencyId, ReferenceDate);
            rate = rate ?? new Logitude.BL.DataContracts.LastRate() { Rate = 1 };
            double? itemCurrencyRateRounded = MethodHelper.Round(rate.Rate, 5);
            itemCurrencyRateRounded = itemCurrencyRateRounded ?? 1;
            decimal local = foreignAmount * (decimal)itemCurrencyRateRounded.GetValueOrDefault();
            return local;
        }

        private void CreateJournalHeader(int tenant,DateTime accountingDate)
        {
            TheNewJournal/*_TheNewJournal */= new JournalPM();
            TheNewJournal.Tenant = tenant;
            TheNewJournal.Id = "new";
            TheNewJournal.JournalNumber = "1";
            TheNewJournal.CreateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            TheNewJournal.AccountingDate = //theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : 
                accountingDate;//_ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            TheNewJournal.TypeCode = "0";

            bool testedAndFoundAllOK = true;
            if (testedAndFoundAllOK)
            {
                TheNewJournal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Approved;
            }
            else
            {
                TheNewJournal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            }


            TheNewJournal.CreatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant);  // _ExternalReconcileDataProvider.ResolveUserId(tenant); ;
            
            TheNewJournal.AccountingEntityCode = "12";//public const string BankAdjustment = "12";   due aRPaymentCheque not found !!
            //journal.AccountingEntityId = theEntityPm.Id;
            //_TheNewJournal.AccountingEntityReference = myReconcileExternalPageLinePM.Reference;
            TheNewJournal.UpdateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            TheNewJournal.UpdatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant); ;
            TheNewJournal.ApproveDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            TheNewJournal.ApprovedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant); ;

            TheNewJournal.ChangeSetOp = ChangeSetOperation.Insert;
            


        }

        private void Validate(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList, bool CheckWhileStreaming, List<LedgerTransactionPM> ledgerTransactionList, List<string> ledgerTransactionIds, BankAccountPM bankAccountFromReconcileExternalPageLine, string accountingCurrencyId)
        {
            AllLineAreExistAndSameBankAccount(tenant, reconcileExternalPageLineIdList, listOfpageLineList, listOfpageList);
            AllPageLineCheckInProgressByWhileStreaming(listOfpageLineList, CheckWhileStreaming);
            if (adjustGLAccountId== bankAccountFromReconcileExternalPageLine.GLAccountId)
            {
                _ErrorList.Add(M_AdjustAccoutMustBeDiffFromBank);//"החשבון להפרשים חייב להיות שונה מהבנק";
            }
            if (ledgerTransactionIds.Count() > 0)
            {
                if (reconcileExternalPageLineIdList.Count()!=1)
                {
                    _ErrorList.Add(M_AdjustLadgerOnly1ExternalPageLineId);
                }
                AllLedgerAreExistAndSameBankAccount(tenant, ledgerTransactionIds, ledgerTransactionList, adjustGLAccountId);
                AllLedgerCheckInProgressByWhileStreaming(ledgerTransactionList, CheckWhileStreaming);
                if (ledgerTransactionList.First().AccountId != listOfpageList.First().GLAccountId)
                {
                    _ErrorList.Add(M_AccountShouldBeTheSameToBank);
                }
            }
            
            


            TotalPLineNotZero(listOfpageLineList, ledgerTransactionList, bankAccountFromReconcileExternalPageLine, accountingCurrencyId);

            //AdjustGLAccountInMyTenant(tenant, listOfpageList);
            //AdjustGLAccountIsNotWorker();
        }

        private void AllLedgerCheckInProgressByWhileStreaming(List<LedgerTransactionPM> ledgerTransactionList, bool checkWhileStreaming)
        {
            if (!checkWhileStreaming)
            {
                if (ledgerTransactionList.Any(r => r.InProgressExternalReconcile))
                {
                    _ErrorList.Add(M_WhileCreating_All_NotInProgressExternalReconcile);
                }

            }
            else
            {
                if (!ledgerTransactionList.TrueForAll(r => r.InProgressExternalReconcile))
                {
                    _ErrorList.Add(M_WhileStreaming_All_InProgressExternalReconcile);
                }
            }
            //if (ledgerTransactionList.Any(r => r.IsReconciled))
            //{
            //    _ErrorList.Add(M_IsReconciled);
            //}

        }

        private void AllLedgerAreExistAndSameBankAccount(int tenant, List<string> ledgerTransactionIds, List<LedgerTransactionPM> ledgerTransactionList, 
            string adjustGLAccountId)
        {
              if (ledgerTransactionList.Count() != ledgerTransactionIds.Count())
            {
                _ErrorList.Add(M_NotAllLedgerIdsFoundInDB);
            }
            if (ledgerTransactionList.Any(r => r.Tenant != tenant))
            {
                _ErrorList.Add(M_NotAllLedgerIdsFoundInDB);
            }
            if (ledgerTransactionList.Select(r => r.AccountId).Distinct().Count() != 1)
            {
                _ErrorList.Add(M_NotAllLedgerInTheSameBankAccount);
            }
            if( ledgerTransactionList.Select(r => r.AccountId).First() == adjustGLAccountId)
            {
                _ErrorList.Add(M_AdjustAccoutMustBeDiffFromBank);
                // = "החשבון להפרשים חייב להיות שונה מהבנק";
            }
        }

        private void AllPageLineCheckInProgressByWhileStreaming( List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList,bool CheckWhileStreaming)
        {
            if (!CheckWhileStreaming)
            {
                if (listOfpageLineList.Any(r => r.InProgressExternalReconcile))
                {
                    _ErrorList.Add(M_WhileCreating_All_NotInProgressExternalReconcile);
                }

            }
            else
            {
                if (!listOfpageLineList.TrueForAll(r => r.InProgressExternalReconcile))
                {
                    _ErrorList.Add(M_WhileStreaming_All_InProgressExternalReconcile);
                }
            }
            if (listOfpageLineList.Any(r => r.IsReconciled))
            {
                _ErrorList.Add(M_IsReconciled);
            }

        }

        private void TotalPLineNotZero(List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<LedgerTransactionPM> ledgerTransactionList, BankAccountPM bankAccountFromReconcileExternalPageLine, string accountingCurrencyId)
        {
            var tot = listOfpageLineList.Sum(r => r.DebitAmount - r.CreditAmount);

            decimal totLedger = 0;
            

            if (bankAccountFromReconcileExternalPageLine!=null && accountingCurrencyId == bankAccountFromReconcileExternalPageLine.CurrencyId)
            {
                totLedger = ledgerTransactionList.Sum(r => r.LocalAmountDebit- r.LocalAmountCredit);
            }
            else
            {
                totLedger = ledgerTransactionList.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit);
            }


            if (tot + totLedger == 0)
            {
                _ErrorList.Add(M_TotalPLineNotZero);
            }

        }

        private void AllLineAreExistAndSameBankAccount(int tenant, List<string> reconcileExternalPageLineIdList, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList)
        {
            

            
            if (listOfpageLineList.Count()!= reconcileExternalPageLineIdList.Count())
            {
                _ErrorList.Add(M_NotAllPageLineIdsFoundInDB);
            }
            if (listOfpageLineList.Any(r=>r.Tenant!= tenant))
            {
                _ErrorList.Add(M_NotAllPageLineIdsFoundInDB);
            }
            if (listOfpageList.Select(r=>r.GLAccountId).Distinct().Count()!=1)
            {
                _ErrorList.Add(M_NotAllPageLineIdsInTheSameBankAccount);
            }

        }
    }
    public interface IExternalReconcileAdjustBankFeesService
    {
        void CreateJournalWithExtReconcile(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, string screenNotes, DateTime accountingDate
            , List<string> ledgerTransactionIds = null);
        void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider);
        JournalPM TheNewJournal { get; }
    }
    public interface IExternalReconcileAdjustBankFees_Validate
    {
        void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider);
        void PrapareAndValid(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, out List<ReconcileExternalPageLineList> listOfpageLineList, out List<ReconcileExternalPageList> listOfpageList, bool CheckWhileStreaming,
            List<string> ledgerTransactionIds,
            out string accountingCurrencyId, out List<LedgerTransactionPM> ledgerTransactionList);
    }
}
