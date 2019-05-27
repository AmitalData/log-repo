using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
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
    public class YearTransferService : IYearTransferService
    {
        StringBuilder _sb= new StringBuilder();
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
        public JournalPM ProccessJournal(IAccountingContext accountingContext,int YYyear,int tenant)
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
                    string transText = "";
                    bool useLocal = true;
                    transText = TranslateTextsClassTranslate("General.MC.ACC.YearTransfer", 0, useLocal);
                    if (String.IsNullOrWhiteSpace(transText))
                    {
                        transText = "Closed Month";
                    }

                    if (!IsMonthOpenForAccountingDate(accountingPeriodsByTypeRegular.AsQueryable(), accountingDate))
                    {
                        throw new Exception(transText);
                    }
                }
            }




            _sb.AppendLine($"GetRevenueExpenseGLAccountFromAccSetting({tenant})");
            _FullAccountingSettingPM = GetRevenueExpenseGLAccountFromAccSetting(accountingContext,tenant);
            _AllRevenueExpenseCards = GetQAllRevenueExpenseCards(accountingContext,tenant);



            var listOfAccountId = _AllRevenueExpenseCards.Select(r => r.Id).AsQueryable<string>();//.ToList();
            
            var item =listOfAccountId.FirstOrDefault(id => id == _FullAccountingSettingPM.RevenueExpenseGLAccountId);
            if (item != null)
            {
                //listOfAccountId.Remove(item);
                listOfAccountId = listOfAccountId.Where(r => r != item);
            }
            _TotalBalance = GetBalance(accountingContext,_EndOfYearUserInput,tenant, listOfAccountId);

            
            if (_TotalBalance.Count == 0)
            {
                return null;
            }
            var usrid = AuthenticationUtil.ResolveUserId(tenant);
            DateTime @now= TenantServerConfigration.GetCurrentDateTime(tenant);
            var journalPM =CreateJournal(
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
                AccountingEntityCode = "1",
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
            CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal, RevenueType, MyJournalActionTypeEnum.Credit);

            CreateJLinesAganistMainREGLAcc(RevenueExpenseGLAccountId, listallRevenueExpenseCards, totalBalance, journal, ExpenseType, MyJournalActionTypeEnum.Debit);



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

        private MyJournalActionTypeEnum GetMyEnum(string revenueExpenseType)
        {
            switch (revenueExpenseType)
            {

                case RevenueType:
                   // return MyJournalActionTypeEnum.Credit;
                    return MyJournalActionTypeEnum.Debit;

                    break;

                case ExpenseType:
                default:
                   // return MyJournalActionTypeEnum.Debit;
                    return MyJournalActionTypeEnum.Credit;
                    break;

            }
        }

        private void CreateJLinesAganistMainREGLAcc(string RevenueExpenseGLAccountId, List<GLAccountAndMoreDTO> allRevenueExpenseCards, List<CurrencySum> totalBalance, JournalPM journal, string revenueExpenseType,MyJournalActionTypeEnum journalActionTypeEnum)
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


        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private JournalLinePM GetJournalLine(MyJournalActionTypeEnum journalActionTypeEnum, CurrencySum myCurrencySum, JournalPM journal, string RevenueExpenseGLAccountId, string revenueExpenseType)
        {
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
            switch (journalActionTypeEnum)
            {
                
                case MyJournalActionTypeEnum.Credit:
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit;
                    journalLine.CreditAccountId = myCurrencySum.AccountId; //
                    journalLine.DebitAccountId= RevenueExpenseGLAccountId; //
                    break;
                case MyJournalActionTypeEnum.Debit:
                    journalLine.ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit;
                    journalLine.DebitAccountId = myCurrencySum.AccountId; //
                    journalLine.CreditAccountId = RevenueExpenseGLAccountId; //
                    break;
                
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
            endAccountBalanceService.CalculateBalance(GLAccountTotalDateTypeValues.Accountingdate,CalculateBalanceIsNotIncludeSo_endOfYearUserInputPlus1, false, true);

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
            if (YYyear.ToString().Length != 2)
            {
                throw new Exception("You must enter two characters only, חובה להזין רק שני תווים בשדה");
            }
            DateTime endOfYearUserInput = DateTime.MaxValue;
            string OldDateStr = "YY-12-31";
            OldDateStr = OldDateStr.Replace("YY", YYyear.ToString());
            DateTime.TryParseExact(OldDateStr, "yy-MM-dd", null, DateTimeStyles.AllowWhiteSpaces, out endOfYearUserInput);
            if (endOfYearUserInput.Year >= DateTime.Now.Year)
            {
                throw new Exception("“ You must choose past years only” “אתה חייב לבחור שנים קודמות בלבד");
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
