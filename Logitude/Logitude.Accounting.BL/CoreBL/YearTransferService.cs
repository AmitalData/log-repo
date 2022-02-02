using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.Batch;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class YearTransferService : IYearTransferService, ICancelYearTransferService, ICheckAndQYearTransferService
    {
        StringBuilder _sb = new StringBuilder();
        public const string RevenueType = "1";
        public const string ExpenseType = "2";
        private IQueryable<GLAccountAndMoreDTO> _AllRevenueExpenseCards;
        public const string M_ClosedMonth = "ARInvoice.M.ClosedMonth";



        private DateTime _EndOfYearUserInput;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private List<CurrencySum> _TotalBalance;

        public YearTransferService()
        {

        }
        
        public string Check_CreateQBatchTaskYearTransfer(int YYyear, int tenant, string userId)
        {
            
            var accountingContext = AccountingContext.GetContext(tenant);
            CheckThrowExceptionIfNeeded(accountingContext, YYyear, tenant);

            
            var myBatchYearTransferService = new BatchYearTransferService(null);
            return myBatchYearTransferService.CreateQBatchTaskExecution<BatchYearTransferParams>(new BatchYearTransferParams() { Tenant = tenant, YYyear = YYyear, UserId= userId }, tenant, $"YearTransfer({YYyear})", false);
        }


        public JournalPM ProccessJournal(IAccountingContext accountingContext, int YYyear, int tenant,String usrid)
        {
            CheckThrowExceptionIfNeeded(accountingContext, YYyear, tenant);

            _sb.AppendLine($"GetRevenueExpenseGLAccountFromAccSetting({tenant})");
            _FullAccountingSettingPM = GetRevenueExpenseGLAccountFromAccSetting(accountingContext, tenant);
            _AllRevenueExpenseCards = GetQAllRevenueExpenseCards(accountingContext, tenant);



            var listOfAccountId = _AllRevenueExpenseCards.Select(r => r.Id).AsQueryable<string>();//.ToList();

            var item = listOfAccountId.FirstOrDefault(id => id == _FullAccountingSettingPM.RevenueExpenseGLAccountId);
            if (item != null)
            {
                //listOfAccountId.Remove(item);
                listOfAccountId = listOfAccountId.Where(r => r != item);
            }
            _TotalBalance = GetBalance(accountingContext, _EndOfYearUserInput, tenant, listOfAccountId);


            if (_TotalBalance.Count == 0)
            {
                return null;
            }
            //var usrid = AuthenticationUtil.ResolveUserId(tenant);
            DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
            var journalPM = CreateJournal(
                _EndOfYearUserInput,
                _AllRevenueExpenseCards,
                _TotalBalance,
                _FullAccountingSettingPM.RevenueExpenseGLAccountId,
                usrid, @now,
                tenant);
            var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            JournalUP.Update(journalPM, true);
            return journalPM;
        }

        public void CheckThrowExceptionIfNeeded(IAccountingContext accountingContext, int YYyear, int tenant)
        {
            _sb.AppendLine($"CheckYear({YYyear})");
            _EndOfYearUserInput = CheckYear(YYyear);
            DateTime accountingDate = _EndOfYearUserInput.AddDays(1);//1.1.(yyyy+1)

            if (accountingDate != null)
            {
                AccountingPeriodQueryService accountingPeriodQueryService = new AccountingPeriodQueryService(accountingContext);
                List<AccountingPeriodPM> accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodsByTenantAndType("1", tenant);
                if (accountingPeriodsByTypeRegular != null)
                {
                    if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), accountingDate))
                    {
                        string transText = "";
                        bool useLocal = true;
                        transText = TranslateTextsClassTranslate("Accounting.O.AccountingPeriodClosed", 0, useLocal);
                        if (String.IsNullOrWhiteSpace(transText))
                        {
                            transText = "Closed Month";
                        }
                        throw new Exception(transText);
                    }
                }
                DateTime accountingDateFrom = accountingDate.AddYears(-1);//1.1.yyyy
                DateTime accountingDateTo = _EndOfYearUserInput;//31.12.yyyy

                JournalQueryService journalQueryService = new JournalQueryService(accountingContext);
                List<JournalPM> journalsNotLT = journalQueryService.GetJournalsNotLTByAccDate(accountingDateFrom, accountingDateTo, tenant).ToList();
                if (journalsNotLT != null)
                {
                    JournalPM journalNotLT = journalsNotLT.FirstOrDefault();
                    if (journalNotLT != null)
                    {
                        throw new Exception(NotLTMessage(journalNotLT.JournalNumber));
                    }
                }


                List<JournalPM> yearTransferJournalsNotLT = journalQueryService.GetJournalsNotLTByAccDateAccEntity(_EndOfYearUserInput, DateTime.Today, "11", tenant).ToList();
                if (yearTransferJournalsNotLT != null)
                {
                    JournalPM yearTransferjournalNotLT = yearTransferJournalsNotLT.FirstOrDefault();
                    if (yearTransferjournalNotLT != null)
                    {
                        throw new Exception(NotLTMessage(yearTransferjournalNotLT.JournalNumber));
                    }
                }


                List<JournalPM> notVoidedJournalPMs = journalQueryService.GetJournalsByAccountingEntityCodeAndDate("11", accountingDate, tenant).Where(j => !j.IsVoided.HasValue || !j.IsVoided.Value).ToList();
                if (notVoidedJournalPMs != null)
                {
                    bool problem = false;
                    string journalNo = "";
                    JournalPM jPM = notVoidedJournalPMs.Where(j => String.IsNullOrEmpty(j.OriginalJournalId)).FirstOrDefault();
                    if (jPM != null) // at least one journal without OriginalJournalId
                    {
                        problem = true;
                        journalNo = jPM.JournalNumber;
                    }
                    else
                    {
                        List<String> originalJournalIds = notVoidedJournalPMs.Select(j => j.OriginalJournalId).ToList();
                        List<JournalPM> notVoidedOriginalJournals = journalQueryService.GetJournalPMsByIds(originalJournalIds, tenant).Where(originalJournal => !originalJournal.IsVoided.HasValue || !originalJournal.IsVoided.Value).ToList();
                        List<String> notVoidedOriginalIds = notVoidedOriginalJournals.Select(k => k.Id).ToList();
                        List<JournalPM> realJournals = notVoidedJournalPMs.Where(j => notVoidedOriginalIds.Contains(j.OriginalJournalId)).ToList();
                        if (realJournals != null && realJournals.Count > 0)
                        {
                            jPM = realJournals.FirstOrDefault();
                            if (jPM != null) // at least one journal where OriginalJournalId is not voided
                            {
                                problem = true;
                                journalNo = jPM.JournalNumber;
                            }
                        }
                    }
                    if (problem)
                    {
                        string transText = "";
                        bool useLocal = true;
                        transText = TranslateTextsClassTranslate("Accounting.O.YearTransferredAlready", 0, useLocal) + journalNo;
                        if (String.IsNullOrWhiteSpace(transText))
                        {
                            transText = "The chosen year is transferred already, In order to transfer it again, you must void Journal " + journalNo;
                        }

                        throw new Exception(transText);
                    }
                }

            }
        }

        private string NotLTMessage(string jNo)
        {
            string transText = "";
            bool useLocal = true;
            string transText1 = TranslateTextsClassTranslate("Accounting.General.O.JournalNumber", 0, useLocal) + jNo;
            if (String.IsNullOrWhiteSpace(transText1))
            {
                transText1 = $"Journal No. {jNo}";
            }
            string transText2 = TranslateTextsClassTranslate("Accounting.General.O.YearTransferJournalNotLT", 0, useLocal);
            if (String.IsNullOrWhiteSpace(transText2))
            {
                transText2 = $" is not registered. Can not complete the year transfer process, please contact Support Center.";
            }
            transText = transText1 + transText2;
            return (transText);
        }


        public JournalPM CancelYear(IAccountingContext accountingContext, int YYyear, int tenant)
        {
            JournalPM originalPM = CheckCancelYear(accountingContext, YYyear, tenant);
            JournalPM stornoJournalPM = DoCancelYear(accountingContext, originalPM, tenant);
            return stornoJournalPM;
        }

        public JournalPM DoCancelYear(IAccountingContext accountingContext, JournalPM origPM, int tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(accountingContext);
            JournalPM stornoPM = null;
            if (origPM != null)

            {
                try
                {
                    var service = new JournalVoidUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                    var stornoOverrideM = new StornoOverrideM()
                    {
                        AccountingEntityCode = "11",
                        AccountingEntityId = origPM.Id,
                        AccountingEntityReference = origPM.JournalNumber,
                    };
                    JournalPM jPM = service.VoidJournal(origPM.Id, tenant, stornoOverrideM);
             //       JournalPM savedPM = journalQueryService.GetSinglePM(origPM.Id, tenant);

                    stornoPM = journalQueryService.GetSinglePMByOriginal(origPM.Id, tenant);
                }
                catch (Exception e)
                {
                    string text = $"Journal {origPM.JournalNumber} Storno issue failed";
                    throw new Exception($"{text} ", e);
                }
            }
            else
            {
                throw new Exception("Original Journal not found");
            }

            return stornoPM;
        }
        public JournalPM CheckCancelYear(IAccountingContext accountingContext, int YYyear, int tenant)
        {
            _sb.AppendLine($"CheckYear({YYyear})");
            _EndOfYearUserInput = CheckYear(YYyear);
            DateTime accountingDate = _EndOfYearUserInput.AddDays(1);//1.1.(yyyy+1)
            JournalPM origPM = null;
            JournalQueryService journalQueryService = new JournalQueryService(accountingContext);

            if (accountingDate != null)
            {
                AccountingPeriodQueryService accountingPeriodQueryService = new AccountingPeriodQueryService(accountingContext);
                List<AccountingPeriodPM> accountingPeriodsByTypeRegular = accountingPeriodQueryService.GetAccountingPeriodsByTenantAndType("1", tenant);
                if (accountingPeriodsByTypeRegular != null)
                {
                    if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), accountingDate))
                    {
                        string transText = "";
                        bool useLocal = true;
                        transText = TranslateTextsClassTranslate("Accounting.O.AccountingPeriodClosed", 0, useLocal);
                        if (String.IsNullOrWhiteSpace(transText))
                        {
                            transText = "Closed Month";
                        }
                        throw new Exception(transText);
                    }
                }


                List<JournalPM> yearTransferJournalsNotLT = journalQueryService.GetJournalsNotLTByAccDateAccEntity(_EndOfYearUserInput, DateTime.Today, "11", tenant).ToList();
                if (yearTransferJournalsNotLT != null)
                {
                    JournalPM yearTransferjournalNotLT = yearTransferJournalsNotLT.FirstOrDefault();
                    if (yearTransferjournalNotLT != null)
                    {
                        throw new Exception(NotLTMessage(yearTransferjournalNotLT.JournalNumber));
                    }
                }


                List<JournalPM> notVoidedJournalPMs = journalQueryService.GetJournalsByAccountingEntityCodeAndDate("11", accountingDate, tenant).Where(j => !j.IsVoided.HasValue || !j.IsVoided.Value).ToList();
                if (notVoidedJournalPMs == null)
                {
                    string transText = "";
                    bool useLocal = true;
                    transText = TranslateTextsClassTranslate("Accounting.O.AccountingPeriodYet", 0, useLocal);
                    if (String.IsNullOrWhiteSpace(transText))
                    {
                        transText = "The chosen year is not yet transferred";
                    }

                    throw new Exception(transText);

                }

                else //if (notVoidedJournalPMs != null)
                {
                    JournalPM jPM = notVoidedJournalPMs.Where(j => String.IsNullOrEmpty(j.OriginalJournalId)).FirstOrDefault();
                    if (jPM != null) // at least one journal without OriginalJournalId
                    {
                        origPM = jPM; // that's the one to void it
                    }
                    else
                    {
                        string journalNo = "";
                        List<String> originalJournalIds = notVoidedJournalPMs.Select(j => j.OriginalJournalId).ToList();
                        List<JournalPM> formerTransfers = journalQueryService.GetJournalPMsByIds(originalJournalIds, tenant).ToList();
                        if (formerTransfers != null && formerTransfers.Count > 0)
                        {
                            jPM = formerTransfers.OrderByDescending(j => j.JournalNumber).FirstOrDefault();
                            if (jPM != null) // at least one journal where OriginalJournalId is not voided
                            {
                                journalNo = jPM.JournalNumber;
                            }
                        }
                        else //OUR problem: OriginalJournalId is not a voided journal; 
                             // so the question is: 
                             // can the notVoidedJournalPMs.FirstOrDefault()  serve as the journal to void id? 
                        {
                            JournalPM nvjPM = notVoidedJournalPMs.OrderByDescending(j => j.JournalNumber).Where(j => String.IsNullOrEmpty(j.OriginalJournalId)).FirstOrDefault();

                            if (nvjPM != null)
                            {
                                journalNo = nvjPM.Id; // cannot be there because OriginalJournalId IS NOT NULL  <==> the journal is s storno of the OriginalJournalId  
                            }
                            else
                            {
                                journalNo = "0";
                            }
                        }


                        string transText = "";
                        string transText_1 = "";
                        string transText_22 = "";
                        bool useLocal = true;
                        transText_1 = TranslateTextsClassTranslate("Accounting.O.TheYearTransferJournal", 0, useLocal);
                        if (String.IsNullOrWhiteSpace(transText_1))
                        {
                            transText_1 = "The Year Transfer Journal ";
                        }
                        transText_22 = TranslateTextsClassTranslate("Accounting.O.YearTransferCancelledAlready", 0, useLocal);
                        if (String.IsNullOrWhiteSpace(transText_22))
                        {
                            transText_22 = " for chosen yead is cancelled already";
                        }
                        transText = $"{transText_1}{journalNo}{transText_22}";

                        throw new Exception(transText);
                    }
                }

            }
            return origPM;
 
        }



        public virtual JournalPM CreateJournal(DateTime endOfYearUserInput,  IQueryable<GLAccountAndMoreDTO> allRevenueExpenseCards, List<CurrencySum> totalBalance,string RevenueExpenseGLAccountId, string usrid,DateTime @now, int tenant)
        {

           
            
            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = tenant,
                ///journal.JournalNumber = "1";
                CreateDate = @now,
                AccountingDate = endOfYearUserInput.AddDays(1),//1.1.(yyyy+1)
                TypeCode = "0",
                StatusCode = "2",
                //journal.CreatedByUserId = theEntityPm.CreatedByUserId;

                //journal.AccountingEntityId = theEntityPm.Id;
                //journal.AccountingEntityReference = theEntityPm.InvoiceNumber;
                UpdateDate = @now,
                //journal.UpdatedByUserId = theEntityPm.UpdatedByUserId;
                ApproveDate = @now,
                //journal.ApprovedByUserId = theEntityPm.ApprovedByUserId;
                AccountingEntityCode = "11", // "Year Transfer"
            //    AccountingEntityId = null,
                CreatedByUserId= usrid,
                ApprovedByUserId = usrid,
                 
                ExternalNo = null,
                ExternalSystem = null,
                OriginalJournalId = null,
             //   AccountingEntityReference= "Year Transfer"

            };

            journal.LineCounter = 0;

            ;
            var listallRevenueExpenseCards = allRevenueExpenseCards.ToList();
            //         CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal,RevenueType, MyJournalActionTypeEnum.Debit);

            //         CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal, ExpenseType, MyJournalActionTypeEnum.Credit);
            CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal, RevenueType, JournalActionTypeEnum.Credit);

            CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal, ExpenseType, JournalActionTypeEnum.Debit);



            var journalLinesOfChildAcc = (from myCurrencySum in totalBalance
                                          //.Where( r=>(r.ForeignAmountDebit - r.ForeignAmountCredit)!=0 )
                                          .OrderBy(r => r.CurrencyId)
                                          join glAcc in allRevenueExpenseCards.OrderBy(r => r.RevenueExpenseType)
                                          on myCurrencySum.AccountId equals glAcc.Id
                                          select GetJournalLine(GetMyEnum( glAcc.RevenueExpenseType), myCurrencySum, journal, RevenueExpenseGLAccountId, glAcc.RevenueExpenseType));
                                          //select GetJournalLine(MyJournalActionTypeEnum.Credit, myCurrencySum, journal, RevenueExpenseGLAccountId));

            journal.JournalLines.AddRange(journalLinesOfChildAcc);
            
            return journal;
        }

        private JournalActionTypeEnum GetMyEnum(string revenueExpenseType)
        {
            switch (revenueExpenseType)
            {

                case RevenueType:
                   // return MyJournalActionTypeEnum.Credit;
                    return JournalActionTypeEnum.Debit;

                    break;

                case ExpenseType:
                default:
                   // return MyJournalActionTypeEnum.Debit;
                    return JournalActionTypeEnum.Credit;
                    break;

            }
        }

        private void CreateJLinesAganistMainREGLAcc(string RevenueExpenseGLAccountId, List<GLAccountAndMoreDTO> allRevenueExpenseCards, List<CurrencySum> totalBalance, JournalPM journal, string revenueExpenseType,JournalActionTypeEnum journalActionTypeEnum)
        {
            var Type1Ids = allRevenueExpenseCards.Where(r => r.RevenueExpenseType == revenueExpenseType).Select(r => r.Id).ToList();

            var TypeJL
                =
                (from myCurrencySum1 in totalBalance.Where(r => Type1Ids.Contains(r.AccountId))
                 group myCurrencySum1 by myCurrencySum1.CurrencyId into g
                 let myCurrencySumGroup1 = new CurrencySum
                 {
                     AccountId = RevenueExpenseGLAccountId,
                     CurrencyId = g.Key,
                     ForeignAmountCredit = g.Sum(r => r.ForeignAmountCredit),
                     ForeignAmountDebit = g.Sum(r => r.ForeignAmountDebit),
                     LocalAmountCredit = g.Sum(r => r.LocalAmountCredit),
                     LocalAmountDebit = g.Sum(r => r.LocalAmountDebit),

                 }
                 select GetJournalLine(journalActionTypeEnum, myCurrencySumGroup1, journal, "", revenueExpenseType)
                 );
            //
            journal.JournalLines.AddRange(TypeJL);
        }
        public static ITextCodeTranslator OverrideITextCodeTranslator { get; set; }

        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            if (OverrideITextCodeTranslator != null)
            {
                return OverrideITextCodeTranslator.Translate(textCodeCode, tenant);
            }
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private JournalLinePM GetJournalLine(JournalActionTypeEnum journalActionTypeEnum, CurrencySum myCurrencySum, JournalPM journal, string RevenueExpenseGLAccountId, string revenueExpenseType)
        {
            if (string.IsNullOrWhiteSpace(RevenueExpenseGLAccountId))
            {
                RevenueExpenseGLAccountId = null;
            }
            int tenant = journal.Tenant;
            bool useLocal = true;
            var journalLine = new JournalLinePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                AccountingDate = journal.AccountingDate,
                Line = ++journal.LineCounter,
                CurrencyId = myCurrencySum.CurrencyId,
                //DocumentDate = journal.do
                LocalAmount = (myCurrencySum.LocalAmountDebit - myCurrencySum.LocalAmountCredit),
                ForeignAmount = (myCurrencySum.ForeignAmountDebit - myCurrencySum.ForeignAmountCredit),
//                DocumentDate = journal.CreateDate,
                DocumentDate = _EndOfYearUserInput.AddDays(1),
//                DueDate = journal.CreateDate,
                DueDate = _EndOfYearUserInput.AddDays(1),
                Notes = TranslateTextsClassTranslate("General.MC.ACC.YearTransfer", 0, useLocal),

            };
            if (revenueExpenseType == RevenueType)
            {
                journalLine.LocalAmount = -journalLine.LocalAmount;
                journalLine.ForeignAmount = -journalLine.ForeignAmount;
            }
            int actionCode = 0;
            switch (journalActionTypeEnum)
            {
                case JournalActionTypeEnum.Credit:
                    journalLine.ActionTypeCodeEnum = JournalActionTypeEnum.Credit;
                    actionCode = (int)JournalActionTypeEnum.Credit;
                    journalLine.ActionCode = actionCode.ToString();
                    journalLine.CreditAccountId = myCurrencySum.AccountId; //
                    journalLine.DebitAccountId= RevenueExpenseGLAccountId; //
                    break;
                case JournalActionTypeEnum.Debit:
                    journalLine.ActionTypeCodeEnum = JournalActionTypeEnum.Debit;
                    actionCode = (int)JournalActionTypeEnum.Debit;
                    journalLine.ActionCode = actionCode.ToString();
                    journalLine.DebitAccountId = myCurrencySum.AccountId; //
                    journalLine.CreditAccountId = RevenueExpenseGLAccountId; //
                    break;
                
            }
            if (string.IsNullOrWhiteSpace(journalLine.CreditAccountId))
            {
                journalLine.CreditAccountId = null;
            }
            if (string.IsNullOrWhiteSpace(journalLine.DebitAccountId))
            {
                journalLine.DebitAccountId = null;
            }

            return journalLine;
        }
        /// <summary>
        /// חשב יתרת כל הכרטסים מסןג הוצאות והכנסות (ללא הכרטיס בFULL RevenueExpenseGLAccountId )
        /// הורד כאילו שהיתרה של ה יתרה בשקלים ובמטח הינה  אפס !!
        /// </summary>
        /// <param name="accountingContext"></param>
        /// <param name="endOfYearUserInput"></param>
        /// <param name="tenant"></param>
        /// <param name="listOfAccountId"></param>
        /// <returns></returns>
        private List<CurrencySum> GetBalance(IAccountingContext accountingContext, DateTime endOfYearUserInput, int tenant, IQueryable<string> listOfAccountId)
        {
            var endAccountBalanceService = new AccountBalanceByDateCodeService(accountingContext, tenant, listOfAccountId.First(),
                 listOfAccountId
                );

            var CalculateBalanceIsNotIncludeSo_endOfYearUserInputPlus1 = endOfYearUserInput.AddDays(1);
            bool openBalancePlease_ReCalcYearTransfer = true;//yaron :irrlavant end of year

            endAccountBalanceService.CalculateBalance(
openBalancePlease_ReCalcYearTransfer, 
GLAccountTotalDateTypeValues.Accountingdate, CalculateBalanceIsNotIncludeSo_endOfYearUserInputPlus1,false, false, true,false,false);

            var totals = (from rec in endAccountBalanceService.AccountBalance.verbose.CurrencySumUntillMounth.Union(endAccountBalanceService.AccountBalance.verbose.TheMounthCurrencySum)
                          group rec by new
                          { rec.AccountId, rec.CurrencyId }
                          into gCurrency    
                          select new CurrencySum()
                          {
                              AccountId = gCurrency.Key.AccountId,
                              CurrencyId = gCurrency.Key.CurrencyId,
                              LocalAmountCredit = gCurrency.Sum(rec => rec.LocalAmountCredit),
                              LocalAmountDebit = gCurrency.Sum(rec => rec.LocalAmountDebit),
                              ForeignAmountCredit = gCurrency.Sum(rec => rec.ForeignAmountCredit),
                              ForeignAmountDebit = gCurrency.Sum(rec => rec.ForeignAmountDebit)
                          }
                    ).ToList();


            totals = (from rec in totals
                      where
                          (
                          (rec.LocalAmountDebit - rec.LocalAmountCredit) != 0
                          ||
                          (rec.ForeignAmountDebit - rec.ForeignAmountCredit) != 0
                          )
                      select rec)
                      .ToList();
                          

            return totals;
        }

        

        private IQueryable<GLAccountAndMoreDTO> GetQAllRevenueExpenseCards(IAccountingContext accountingContext, int tenant)
        {
            var myGLAccountQueryService = new GLAccountQueryService(accountingContext);
            var myExpensesGLAccountList = myGLAccountQueryService.GetQAllRevenueExpenseCardsByIsControlAccount(tenant,false);
            return myExpensesGLAccountList;
        }

        private FullAccountingSettingPM GetRevenueExpenseGLAccountFromAccSetting(IAccountingContext accountingContext,int tenant)
        {
            bool useLocal = true;
            string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM =myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new Exception("No FullAccountingSettingPM  for tenant ");
            }
            if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.RevenueExpenseGLAccountId))
            {
                //   throw new Exception("No myFullAccountingSettingPM.RevenueExpenseGLAccountId  for tenant ");
                text = TranslateTextsClassTranslate("YearTransfer.O.RevenueExpenseType", 0, useLocal);
                // A year transfer account is undefined or not configured correctly
                throw new Exception(text);
            }
            else
            {
                GLAccountQueryService myGLAccountQueryService = new GLAccountQueryService(accountingContext);
                GLAccountPM revenueExpenseGLAccount = myGLAccountQueryService.GetSingle(myFullAccountingSettingPM.RevenueExpenseGLAccountId, false, true);
                if (revenueExpenseGLAccount == null || revenueExpenseGLAccount.RevenueExpenseType != "3")
                {
                    text = TranslateTextsClassTranslate("YearTransfer.O.RevenueExpenseType", 0, useLocal);
                    // A year transfer account is undefined or not configured correctly
                    throw new Exception(text);
                }
            }
            return myFullAccountingSettingPM;

        }

        private DateTime CheckYear(int YYyear)
        {
            bool useLocal = true;
            int yylen = YYyear.ToString().Length;
            if (yylen != 2 && yylen != 4)
            {
                string text = TranslateTextsClassTranslate("YearTransfer.O.TwoOrFourDigits", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "Enter year in either two or four digits only";
                throw new Exception(text); //("Enter year in either two or four digits only, יש להזין שנה בשתי ספרות או בארבע ספרות בלבד");
            }

            DateTime endOfYearUserInput = DateTime.MaxValue;
            if (yylen == 4)
            {
                string OldDateStr = "YYYY-12-31";
                OldDateStr = OldDateStr.Replace("YYYY", YYyear.ToString());
                DateTime.TryParseExact(OldDateStr, "yyyy-MM-dd", null, DateTimeStyles.AllowWhiteSpaces, out endOfYearUserInput);
            }
            else
            {
                string OldDateStr = "YY-12-31";
                OldDateStr = OldDateStr.Replace("YY", YYyear.ToString());
                DateTime.TryParseExact(OldDateStr, "yy-MM-dd", null, DateTimeStyles.AllowWhiteSpaces, out endOfYearUserInput);
            }
            if (endOfYearUserInput.Year >= DateTime.Now.Year)
            {
                string text = TranslateTextsClassTranslate("YearTransfer.O.PastYears", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "Enter past years only";
                throw new Exception(text); //Enter past years only, יש להזין שנים קודמות בלבד");
            }
            return endOfYearUserInput;

        }


        public static bool IsMonthOpenForAccountingDate(
            IQueryable<AccountingPeriodPM> accountingPeriodsByTypeRegular,
            //JournalPM myJournalPM
            DateTime AccountingDate
            )
        {
            bool valid = true;
            var currentAccountingPeriodPM = accountingPeriodsByTypeRegular.FirstOrDefault(periods =>
                periods.PeriodTypeCode == "1" && periods.Year == /*myJournalPM.*/AccountingDate.Date.Year);
            if (currentAccountingPeriodPM == null)
            {
                valid = false;
                //errorsList.Add(transText);
            }
            else
            {
                var accountingDateMonth = /*myJournalPM.*/AccountingDate.Date.Month;

                if (accountingDateMonth > currentAccountingPeriodPM.ClosedMonth.GetValueOrDefault())
                {
                    //Valid ... AccountingDateMonth must be greater than close Mounth
                }
                else
                {
                    //Not Valid ... AccountingDateMonth must be greater than close Mounth
                    //not valid  8>=8 
                    //not valid  0>=1 - Must Open mounth before work on year !!
                    valid = false;
                    //errorsList.Add(transText); //ClosedMonth Must B
                }
                if (accountingDateMonth == currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  equal to OpenMonth
                }
                else if (accountingDateMonth < currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  less than OpenMonth
                }
                else
                {

                    valid = false;
                    //errorsList.Add(transText);
                }
            }
            return valid;
        }



    }
}
