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
    public class ExternalReconcileAdjustBankFeesService
    {
        private IExternalReconcileDataProvider _ExternalReconcileDataProvider;
        private List<string> _ErrorList;
        public const string M_TotalPLineNotZero="סך השורות הינו אפס -אין מה להתאים ";
        public const string M_IsReconciled="אחת השורות כבר הותאמה חיצונית";
        public const string M_NotAllPageLineIdsInTheSameBankAccount="לא כל שורות הבנק שייכים לאותו בנק";
        public const string M_NotAllPageLineIdsFoundInDB = "לא כל השורות נמצאו בשרת ה DB";

        public const string M_InProgressExternalReconcile = "אחת השורות בתהליך התאמה חצונית";

        public JournalPM _TheNewJournal { get; private set; }
        public ChangeSetOperation ChangeSetOp { get; private set; }

        public void MustInit(IExternalReconcileDataProvider externalReconcileDataProvider)
        {
            _ExternalReconcileDataProvider = externalReconcileDataProvider;
        }
        void CreateJournalWithExtReconcile(int tenant ,List<string> reconcileExternalPageLineIdList,string adjustGLAccountId,string screenNotes)
        {
            if (string.IsNullOrWhiteSpace(screenNotes))
            {
                screenNotes = "התאמת דף בנק (עמלה)";
            }
            _ErrorList = new List<string>();
            var listOfpageLineList = _ExternalReconcileDataProvider.GetReconcileExternalPageLineList(tenant, reconcileExternalPageLineIdList);

            var reconcileExternalPageIdList =listOfpageLineList.Select(r => r.ReconcileExternalPageId).Distinct().ToList();
            var listOfpageList = _ExternalReconcileDataProvider.GetReconcileExternalPageList(tenant, reconcileExternalPageIdList);
            Validate(tenant, reconcileExternalPageLineIdList, adjustGLAccountId, listOfpageLineList, listOfpageList);
            if (_ErrorList.Count()>0)
            {
                throw new Exception(string.Join(Environment.NewLine, _ErrorList.ToArray()));
            }


            string accountingCurrencyId = _ExternalReconcileDataProvider.GetaccountingCurrencyId(tenant);

            var listOfAccId =listOfpageList.Select(r => r.GLAccountId).ToList();

            List<GLAccountList> ListOfGLAccountList = _ExternalReconcileDataProvider.GetListOfGLAccountList(tenant, listOfAccId);
            var bankGLAccountList = ListOfGLAccountList.First();//must have 
            CreateJournal(tenant, adjustGLAccountId, listOfpageLineList, listOfpageList,  bankGLAccountList, accountingCurrencyId, screenNotes);

        }

        private void CreateJournal(int tenant, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList, GLAccountList bankGLAccountList, string accountingCurrencyId, string screenNotes)
        {
            CreateJournalHeader(tenant);
            CreateJournalLinesFromPageLines(listOfpageLineList, bankGLAccountList, adjustGLAccountId, accountingCurrencyId, screenNotes);



            //CreateJournalExternalReco();
            int line = 1;
            _TheNewJournal.JournalExternalReconciles = listOfpageLineList.Select(r => GetJournalExternalReconcile(r,ref line)).ToList();

            CreateJournalLineToadjustGLAccountId(adjustGLAccountId, bankGLAccountList);
        }

        

        private JournalExternalReconcilePM GetJournalExternalReconcile(ReconcileExternalPageLineList r, ref int line)
        {
            return new JournalExternalReconcilePM()
            {
                Tenant = _TheNewJournal.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,

                JournalId = _TheNewJournal.Id,
                Line = line++,
                //IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ReconcileExternalPageLineId' AND object_id = object_id(N'[dbo].[JournalExternalReconciles]', N'U'))
                //DROP INDEX[IX_ReconcileExternalPageLineId] ON[dbo].[JournalExternalReconciles]
                //ALTER TABLE[dbo].[JournalExternalReconciles] ALTER COLUMN[ReconcileExternalPageLineId][varchar](15) NULL
                //CREATE INDEX[IX_ReconcileExternalPageLineId] ON[dbo].[JournalExternalReconciles]([ReconcileExternalPageLineId])
                LedgerTransactionId = null,
                ReconcileExternalPageLineId = r.Id
            };
        }

        private void CreateJournalLineToadjustGLAccountId(string adjustGLAccountId, GLAccountList bankGLAccountList)
        {
            decimal totDebitlocal = _TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit).Sum(r => r.LocalAmount);
            decimal totCreditlocal = _TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit).Sum(r => r.LocalAmount);



            decimal totDebitForiegn = _TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Debit).Sum(r => r.ForeignAmount);
            decimal totCreditForiegn = _TheNewJournal.JournalLines.Where(r => r.ActionTypeCodeEnum == MyJournalActionTypeEnum.Credit).Sum(r => r.ForeignAmount);
            MyJournalActionTypeEnum myJournalActionTypeEnum = MyJournalActionTypeEnum.Credit;
            if (totDebitlocal - totCreditlocal < 0)
            {
                myJournalActionTypeEnum =MyJournalActionTypeEnum.Debit;
            }
            var last=_TheNewJournal.JournalLines.Last();
            _TheNewJournal.JournalLines.Add(new JournalLinePM
            {
                Tenant = last.Tenant,
                JournalId = _TheNewJournal.Id,
                Line = last.Line+1,


                DocumentDate = last.DocumentDate,
                DueDate = last.DueDate,
                AccountingDate = _TheNewJournal.AccountingDate,



                CurrencyId = last.CurrencyId,

                ForeignAmount = (totDebitForiegn-totCreditForiegn),

                
                ActionTypeCodeEnum = myJournalActionTypeEnum,
                DebitAccountId = adjustGLAccountId,
                CreditAccountId = bankGLAccountList.Id,
                LocalAmount = (totDebitlocal - totCreditlocal),
                Notes = last.Notes ,
                Reference1 = last.Reference1,
                ChangeSetOp = ChangeSetOperation.Insert

            });
            

            

        }




        private void CreateJournalLinesFromPageLines(List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, GLAccountList bankGLAccountList, string adjustGLAccountId, string accountingCurrencyId, string screenNotes)
        {
            int line = 1;
            _TheNewJournal.JournalLines = listOfpageLineList.Select(r =>
            new JournalLinePM()
            {
                Tenant = r.Tenant,
                JournalId = _TheNewJournal.Id,
                Line = line++,


                DocumentDate = r.ReferenceDate,
                DueDate = r.ReferenceDate,
                AccountingDate = _TheNewJournal.AccountingDate,



                CurrencyId = bankGLAccountList.CurrencyId,

                ForeignAmount = r.DebitAmount != 0 ? r.DebitAmount : r.CreditAmount,

                ///if r.DebitAmount != 0 then credit else debit 
                ActionTypeCodeEnum = r.DebitAmount != 0 ? MyJournalActionTypeEnum.Credit : MyJournalActionTypeEnum.Debit,
                DebitAccountId = r.DebitAmount != 0 ? bankGLAccountList.Id : adjustGLAccountId,
                CreditAccountId = r.CreditAmount != 0 ? bankGLAccountList.Id : adjustGLAccountId,
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

        private void CreateJournalHeader(int tenant)
        {
            _TheNewJournal/*_TheNewJournal */= new JournalPM();
            _TheNewJournal.Tenant = tenant;
            _TheNewJournal.Id = "new";
            _TheNewJournal.JournalNumber = "1";
            _TheNewJournal.CreateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            _TheNewJournal.AccountingDate = //theEntityPm.AccountingDate != null ? theEntityPm.AccountingDate.Value : 
                _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            _TheNewJournal.TypeCode = "0";

            bool testedAndFoundAllOK = true;
            if (testedAndFoundAllOK)
            {
                _TheNewJournal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Approved;
            }
            else
            {
                _TheNewJournal.StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Draft;
            }


            _TheNewJournal.CreatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant);  // _ExternalReconcileDataProvider.ResolveUserId(tenant); ;
            
            _TheNewJournal.AccountingEntityCode = "12";//public const string BankAdjustment = "12";   due aRPaymentCheque not found !!
            //journal.AccountingEntityId = theEntityPm.Id;
            //_TheNewJournal.AccountingEntityReference = myReconcileExternalPageLinePM.Reference;
            _TheNewJournal.UpdateDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            _TheNewJournal.UpdatedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant); ;
            _TheNewJournal.ApproveDate = _ExternalReconcileDataProvider.GetCurrentDateTime(tenant);
            _TheNewJournal.ApprovedByUserId = _ExternalReconcileDataProvider.ResolveUserId(tenant); ;

            _TheNewJournal.ChangeSetOp = ChangeSetOperation.Insert;
            const string MyNotes = "פרעון שיק מהתאמה";


        }

        private void Validate(int tenant, List<string> reconcileExternalPageLineIdList, string adjustGLAccountId, List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList, List<Data.EntityLists.ReconcileExternalPageList> listOfpageList)
        {
            AllLineAreExistAndSameBankAccount(tenant, reconcileExternalPageLineIdList, listOfpageLineList, listOfpageList);
            AllPageLineNotAlreadyReconcile(listOfpageLineList);
            TotalPLineNotZero(listOfpageLineList);

            //AdjustGLAccountInMyTenant(tenant, listOfpageList);
            //AdjustGLAccountIsNotWorker();
        }


        

        private void AllPageLineNotAlreadyReconcile( List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList)
        {
            if (listOfpageLineList.Any(r=>r.InProgressExternalReconcile))
            {
                _ErrorList.Add(M_InProgressExternalReconcile);
            }
            if (listOfpageLineList.Any(r => r.IsReconciled))
            {
                _ErrorList.Add(M_IsReconciled);
            }

        }

        private void TotalPLineNotZero(List<Data.EntityLists.ReconcileExternalPageLineList> listOfpageLineList)
        {
            var tot = listOfpageLineList.Sum(r => r.DebitAmount - r.DebitAmount);
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
            if (listOfpageList.Select(r=>r.GLAccountId).Count()!=1)
            {
                _ErrorList.Add(M_NotAllPageLineIdsInTheSameBankAccount);
            }

        }
    }
}
