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
    // ניתן לבחור רשימה של שורות מדפי בנק - חובה וגם זכות
    //
    //

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
    //
    //דף בנק חובה 100 -  BANK_GLACCOUT 100 זכות -ולהפך
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
        public const string M_NotAllPageLineIdsFoundInDB = "לא כל השורות נמצאו בשרת ה DB";

        public const string M_WhileCreating_All_NotInProgressExternalReconcile = "אחת השורות בתהליך התאמה חצונית";
        public const string M_WhileStreaming_All_InProgressExternalReconcile = "אחת השורות לא מסומנת -בתהליך התאמה חצונית";

        public JournalPM TheNewJournal { get; private set; }
        public ChangeSetOperation ChangeSetOp { get; private set; }

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        //const string MyNotes = "פרעון שיק מהתאמה";
        public void CreateJournalWithExtReconcile(int tenant ,List<string> reconcileExternalPageLineIdList,string adjustGLAccountId,string screenNotes
            , DateTime accountingDate
            )
        {
            if (string.IsNullOrWhiteSpace(screenNotes))
            {
                screenNotes = "התאמת דף בנק (עמלה)";
            }
            List<ReconcileExternalPageLineList> listOfpageLineList;
            List<ReconcileExternalPageList> listOfpageList;
            bool CheckWhileStreaming = false;//we are in create mode !!
            PrapareAndValid(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, out listOfpageLineList, out listOfpageList, CheckWhileStreaming);

            string accountingCurrencyId = _ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);

            var listOfAccId = listOfpageList.Select(r => r.GLAccountId).ToList();

            List<GLAccountList> ListOfGLAccountList = _ExternalReconcileDataProvider.GetListOfGLAccountList(tenant, listOfAccId);
            var bankGLAccountList = ListOfGLAccountList.First();//must have 
            CreateJournal(tenant, adjustGLAccountId, listOfpageLineList, listOfpageList, bankGLAccountList, accountingCurrencyId, screenNotes, accountingDate);

        }

        public void PrapareAndValid(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, out List<ReconcileExternalPageLineList> listOfpageLineList, out List<ReconcileExternalPageList> listOfpageList
            ,bool CheckWhileStreaming)
        {
            _ErrorList = new List<string>();
            listOfpageLineList = _ExternalReconcileDataProvider.GetReconcileExternalPageLineList(tenant, reconcileExternalPageLineIdList);
            var reconcileExternalPageIdList = listOfpageLineList.Select(r => r.ReconcileExternalPageId).Distinct().ToList();
            listOfpageList = _ExternalReconcileDataProvider.GetReconcileExternalPageList(tenant, reconcileExternalPageIdList);
            Validate(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, listOfpageLineList, listOfpageList, CheckWhileStreaming);
            if (_ErrorList.Count() > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, _ErrorList.ToArray()));
            }
        }

        private void CreateJournal(int tenant, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList, GLAccountList bankGLAccountList, string accountingCurrencyId, string screenNotes, DateTime accountingDate)
        {
            CreateJournalHeader(tenant, accountingDate);
            CreateJournalLinesFromPageLines(listOfpageLineList, bankGLAccountList, adjustGLAccountId, accountingCurrencyId, screenNotes);



            //CreateJournalExternalReco();
            int line = 1;
            TheNewJournal.JournalExternalReconciles = listOfpageLineList.Select(r => GetJournalExternalReconcile(r,ref line)).ToList();

            CreateJournalLineToadjustGLAccountId(adjustGLAccountId, bankGLAccountList);
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

        private void Validate(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList, bool CheckWhileStreaming)
        {
            AllLineAreExistAndSameBankAccount(tenant, reconcileExternalPageLineIdList, listOfpageLineList, listOfpageList);
            AllPageLineCheckInProgressByWhileStreaming(listOfpageLineList, CheckWhileStreaming);
            TotalPLineNotZero(listOfpageLineList);

            //AdjustGLAccountInMyTenant(tenant, listOfpageList);
            //AdjustGLAccountIsNotWorker();
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

        private void TotalPLineNotZero(List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList)
        {
            var tot = listOfpageLineList.Sum(r => r.DebitAmount - r.CreditAmount);
            if (tot==0)
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
        void CreateJournalWithExtReconcile(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, string screenNotes, DateTime accountingDate);
        void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider);
        JournalPM TheNewJournal { get; }
    }
    public interface IExternalReconcileAdjustBankFees_Validate
    {
        void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider);
        void PrapareAndValid(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, out List<ReconcileExternalPageLineList> listOfpageLineList, out List<ReconcileExternalPageList> listOfpageList, bool CheckWhileStreaming);
    }
}
