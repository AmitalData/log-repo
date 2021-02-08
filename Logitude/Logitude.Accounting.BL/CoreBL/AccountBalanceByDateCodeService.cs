
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL
{
    public class AccountBalanceByDateCodeService
    {
        int _Tenant;
        string _GLAccountId;
        DateTime _TheDate;
        private string _DateTypeCode;

        //bool _IncludeChildAccounts;
        //bool _IncludeRelatedCurrenciesAccount;
        private IAccountingContext _AccountingContext;
        private bool _HaveAccountingQueued;

        //private List<string> _ListOfAccountId;
        private IQueryable<string> _ListOfAccountId;
        private Stopwatch _sw;
        private StringBuilder _StringBuilder;
        public AccountBalanceByDateCodeService(IAccountingContext accountingContext, int tenant,
            string GLAccountId,
            //bool IncludeChildAccounts, bool IncludeRelatedCurrenciesAccount
            //List<string> listOfAccountId
            IQueryable<string> listOfAccountId
)
        {
            _sw = Stopwatch.StartNew();
            _AccountingContext = accountingContext;
            _Tenant = tenant;
            _GLAccountId = GLAccountId;
            //_ListOfAccountId = new List<string>(new HashSet<string>(listOfAccountId));
            _ListOfAccountId = listOfAccountId;
            if (_AccountingContext == null)
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);
            }
            //_IncludeChildAccounts = IncludeChildAccounts;
            //_IncludeRelatedCurrenciesAccount = IncludeRelatedCurrenciesAccount;
            AccountBalance = new AccountBalanceM() { Tenant = _Tenant };
            _StringBuilder = new StringBuilder();
        }
        public void CalculateBalance(
            bool openBalancePlease_ReCalcYearTransfer,
            string  DateTypeCode,DateTime theDate,
            bool checkHaveAccountingQueued ,
            bool inclusiveTheDateLTransaction /*= false*/, 
            bool verbose /*= false*/,
            bool ClacOpenReconciledAmount)
        {
            _TheDate = theDate;
            _DateTypeCode = DateTypeCode;
            if (string.IsNullOrWhiteSpace(_DateTypeCode))
            {
                _DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate;
            }
            var swFull = Stopwatch.StartNew();
            try
            {


                if (String.IsNullOrWhiteSpace(_GLAccountId))
                {
                    throw new Exception("GLAccountId is must");
                }
                if (!_ListOfAccountId.Contains(_GLAccountId))
                {
                    throw new Exception("_ListOfAccountId.Contains(_GLAccountId)");
                }
                if (_AccountingContext == null)
                {
                    _AccountingContext = AccountingContext.GetContext(_Tenant);
                }

                _sw.Restart();


                //IncludeRelatedCurrenciesAccount(_ListOfAccountId);
                //LogIt("IncludeRelatedCurrenciesAccount");
                //IncludeChildAccounts(_ListOfAccountId);
                //LogIt("IncludeChildAccounts");


                var firstDayOfMonth = new DateTime(_TheDate.Year, _TheDate.Month, 1); ;
                var dateLast = firstDayOfMonth.AddMilliseconds(-1);


                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {

                    var myGLAccountTotalByMonthQueryService = new GLAccountTotalByMonthQueryService(_AccountingContext);
                    var currencySumUntillMounth = myGLAccountTotalByMonthQueryService.GetCurrencySumUntillNotIncludeDateType(_ListOfAccountId,_DateTypeCode, _TheDate.Year, _TheDate.Month, _Tenant);
                    LogIt("currencySumUntillMounth");
                    if (verbose)
                    {
                        AccountBalance.verbose = new AccountBalanceverboseM();
                        AccountBalance.verbose.CurrencySumUntillMounth = currencySumUntillMounth;
                    }

                    var DateUntillNotInclude = _TheDate.Date;
                    if (inclusiveTheDateLTransaction)
                    {
                        DateUntillNotInclude = DateUntillNotInclude.AddDays(1);
                    }
                    var myLedgerTransactionQueryService = new LedgerTransactionQueryService(_AccountingContext);
                    var thisMounthGLAccountTotalByMonthByAccountingDate = myLedgerTransactionQueryService
                        .CalcGLAccountTotalByMonthByDateType(_DateTypeCode, firstDayOfMonth, DateUntillNotInclude, _Tenant, _ListOfAccountId);
                    var theMounthCurrencySum = thisMounthGLAccountTotalByMonthByAccountingDate.Select(byMounth => new CurrencySum()
                    {
                        AccountId = byMounth.AccountId,
                        CurrencyId = byMounth.CurrencyId,
                        LocalAmountCredit = byMounth.LocalAmountCredit,
                        LocalAmountDebit = byMounth.LocalAmountDebit,
                        ForeignAmountCredit = byMounth.ForeignAmountCredit,
                        ForeignAmountDebit = byMounth.ForeignAmountDebit,
                    }).ToList();
                    LogIt("theMounthCurrencySum");
                    if (verbose)
                    {
                        AccountBalance.verbose.TheMounthCurrencySum = theMounthCurrencySum;
                    }
                    _HaveAccountingQueued = AnyAccountingQueued(checkHaveAccountingQueued);
                    LogIt("AnyAccountingQueued");
                    AccountBalance.HaveAccountingQueued = _HaveAccountingQueued;
                    ;


                    List<CurrencySum> yearTransferLedgerTransactionCurrencySum = new List<CurrencySum>();
                    if (openBalancePlease_ReCalcYearTransfer)
                    {
                        var openBalanceDate = DateUntillNotInclude;
                        if (DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate
                            && openBalanceDate.Month == 1 && openBalanceDate.Day == 1)
                        {

                            var myLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                            var qYearTransferLedgerTransaction = myLedgerTransactionRepository
                                .GetYearTransferLedgerTransaction(_GLAccountId, openBalanceDate.Year, _Tenant);
                            var yearTransferLedgerTransaction = qYearTransferLedgerTransaction.ToList();
                            AccountBalance.YearTransferLedgerTransactionIds = yearTransferLedgerTransaction.Select(r => r.Id).ToList();

                            yearTransferLedgerTransactionCurrencySum =
    (from lt in yearTransferLedgerTransaction
     group lt by new
     {
         lt.AccountId,
         lt.CurrencyId
     }
         into groupBy_currency
     select new CurrencySum()
     {
         AccountId = groupBy_currency.Key.AccountId,
         CurrencyId = groupBy_currency.Key.CurrencyId,
         //LocalAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountCredit) * -1,
         //LocalAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountDebit) * -1,
         //ForeignAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountCredit) * -1,
         //ForeignAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountDebit) * -1,

         LocalAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountCredit) ,
         LocalAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountDebit) ,
         ForeignAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountCredit) ,
         ForeignAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountDebit) ,


     }).ToList();




                        }
                    }

                    var totals = (from rec in currencySumUntillMounth.Concat(theMounthCurrencySum)
                                  .Concat(yearTransferLedgerTransactionCurrencySum)
                                  group rec by rec.CurrencyId into gCurrencyId
                                  select new CurrencySum()
                                  {
                                      AccountId = _GLAccountId,
                                      CurrencyId = gCurrencyId.Key,
                                      LocalAmountCredit = gCurrencyId.Sum(rec => rec.LocalAmountCredit),
                                      LocalAmountDebit = gCurrencyId.Sum(rec => rec.LocalAmountDebit),
                                      ForeignAmountCredit = gCurrencyId.Sum(rec => rec.ForeignAmountCredit),
                                      ForeignAmountDebit = gCurrencyId.Sum(rec => rec.ForeignAmountDebit)
                                  }
                     ).ToList();







                    if (ClacOpenReconciledAmount )
                    {
                        if (_ListOfAccountId.Count() > 1)
                        {
                            throw new Exception("if (ClacOpenReconciledAmount && _ListOfAccountId.Count()>1)");
                        }

                        var res=
                            myLedgerTransactionQueryService
                            .CalcCurrencySumOpenAmountByMonthByDateType(_DateTypeCode, DateUntillNotInclude, _Tenant, _ListOfAccountId).FirstOrDefault();
                        if (res!=null)
                        {
                            AccountBalance.StartTotalOpenAmount = res.OpenAmount;
                            AccountBalance.OpenAmountCurrencyId = res.OpenAmountCurrencyId;
                        }

                    }


                    AccountBalance.Totals = totals;
                    AccountBalance.TotalLocalAmountDebit = AccountBalance.Totals.Sum(r => r.LocalAmountDebit);
                    AccountBalance.TotalLocalAmountCredit = AccountBalance.Totals.Sum(r => r.LocalAmountCredit);


                    var mustDue = true;//https://startbigthinksmall.wordpress.com/2009/05/04/the-transaction-has-aborted-tricky-net-transactionscope-behavior/
                    if (mustDue)
                    {
                        scope.Complete();
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _StringBuilder.AppendLine("GetBalance():TotalTime:" + swFull.Elapsed.ToString());
                AccountBalance.LogMessage = _StringBuilder.ToString();
            }
        }

        private void LogIt(string mess)
        {

            mess = mess + ":Took:" + _sw.Elapsed.ToString();
            Debug.WriteLine(mess);

            _sw.Restart();
            _StringBuilder.AppendLine(mess);
        }

        private bool AnyAccountingQueued(bool checkHaveAccountingQueued)
        {
            if (!checkHaveAccountingQueued)
            {
                return false;
            }
            var myJournalQueryService = new JournalQueryService(_AccountingContext);
            var have = myJournalQueryService
                //.GetAnyPendingApprovedDev(_ListOfAccountId, _Tenant);
                .GetAnyPendingApproved(_ListOfAccountId, _Tenant);
            if (_sw.Elapsed > TimeSpan.FromSeconds(1))
            {
                Debug.WriteLine("AnyAccountingQueued():Please make index  ");
            }
            return have;
        }

        //private void IncludeChildAccounts(List<string> listOfAccId)
        //{
        //    if (_IncludeChildAccounts)
        //    {
        //        var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
        //        var pms = myGLAccountQueryService.GetChildAccounts(_GLAccountId, _Tenant);
        //        var AccList = pms.Select(rec => rec.Id).ToList();
        //        listOfAccId.AddRange(AccList);
        //    }
        //}

        //private void IncludeRelatedCurrenciesAccount(List<string> listOfAccId)
        //{
        //    if (_IncludeRelatedCurrenciesAccount)
        //    {


        //        var myGLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(_AccountingContext);
        //        var RelatedCurrenciesAccountList = myGLAccountCurrencyQueryService.GetRelatedCurrenciesAccount(_Tenant, _GLAccountId).Select(rec => rec.GLAccountId).ToList();
        //        listOfAccId.AddRange(RelatedCurrenciesAccountList);


        //    }
        //}


        public AccountBalanceM AccountBalance { get; set; }

        public void ReSetAccountList(bool IncludeChildAccounts, bool IncludeRelatedCurrenciesAccount)
        {
            var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
            var hashsetallIdAccounts = myGLAccountQueryService.GetQAllIdAccounts(_Tenant, _GLAccountId, IncludeRelatedCurrenciesAccount, IncludeChildAccounts);
            _ListOfAccountId = hashsetallIdAccounts;// new List<string>(new HashSet<string>(hashsetallIdAccounts));
        }
    }
    public class AccountBalanceParam
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public bool IncludeChildAccounts { get; set; }
        public bool IncludeRelatedCurrenciesAccount { get; set; }

        public bool OpenBalancePlease_ReCalcYearTransfer { get; set; }
        public DateTime accoutingDate { get; set; }
        public bool includeAccoutingDateLTransaction { get; set; }
        public bool verbose { get; set; }

    }
    public class AccountBalanceM
    {

        public int Tenant { get; set; }
        [XmlIgnore]
        public string LogMessage { get; set; }

        [XmlIgnore]
        public AccountBalanceverboseM verbose { get; set; }

        public bool HaveAccountingQueued { get; set; }
        public List<CurrencySum> Totals { get; set; }

        public decimal? TotalLocalAmountDebit { get; set; }

        public decimal? TotalLocalAmountCredit { get; set; }
        public List<string> YearTransferLedgerTransactionIds { get; internal set; }
        public string OpenAmountCurrencyId { get; set; }
        public decimal StartTotalOpenAmount { get; set; }

        internal List<CallBackBalance> GetCallBackBalanceOfCurrency(string currencyId)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
            {
                throw new Exception("string.IsNullOrWhiteSpace(currencyId)");
            }
            return GetCallBackBalance().Where(r => r.CurrencyId == currencyId).ToList();

        }
        internal List<CallBackBalance> GetCallBackBalance()
        {
            var qTotals = (from tot in Totals
                           select new CallBackBalance
                           {
                               CurrencyId = tot.CurrencyId,
                               BalanceForeign = tot.ForeignAmountDebit - tot.ForeignAmountCredit,
                               BalanceLocal = tot.LocalAmountDebit - tot.LocalAmountCredit
                           }).ToList();
            return qTotals;
        }

        internal decimal? GetBalanceOfLocalAmount(string currencyId)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
            {
                throw new Exception("string.IsNullOrWhiteSpace(currencyId)");
            }
            var tot = Totals.Where(r => r.CurrencyId == currencyId).FirstOrDefault();
            if (tot == null)
            {
                return null;
            }
            return (tot.LocalAmountDebit - tot.LocalAmountCredit);
        }
        public decimal? GetBalanceOfCurrency(string currencyId)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
            {
                throw new Exception("string.IsNullOrWhiteSpace(currencyId)");
            }
            //var accountingCurrencyId=  AccountingSettingResolver.ResolveAccountingCurrencyId(Tenant);
            //if (accountingCurrencyId == currencyId)
            //{
            //    return (TotalLocalAmountDebit.GetValueOrDefault() - TotalLocalAmountCredit.GetValueOrDefault());
            //}
            //else
            //{
            var tot = Totals.Where(r => r.CurrencyId == currencyId).FirstOrDefault();
            if (tot == null)
            {
                return null;
            }
            return (tot.ForeignAmountDebit - tot.ForeignAmountCredit);
            //}

        }

        public decimal? GetBalanceOfLocalAmount()
        {
            return (TotalLocalAmountDebit.GetValueOrDefault() - TotalLocalAmountCredit.GetValueOrDefault());
        }






    }

    public class AccountBalanceverboseM
    {
        public List<CurrencySum> CurrencySumUntillMounth { get; set; }
        public List<CurrencySum> TheMounthCurrencySum;
    }

}
/*
Service for providing Account Balance By parameters - WI 22290
Paramters
The service will get the following parameters
GLAccountId
AccoutingDate
IncludeChildAccounts
IncludeRelatedCurrenciesAccount
Response
Check if IncludeChildAccounts= True , in this case
Retrieve GLAccounts table where ParentAccountId = GLAccountId
if found any child records , create a list of them + the original GLAccountId
Check if IncludeRelatedCurrenciesAccount= True , in this case
Retrieve GLAccountCurrencies table where CustomerGLAccountId= GLAccountId
if found any child records , create a list of them + the original GLAccountId
The service will retrieve the GLAccountTotalByMonths table 
AccountId = GLAccountId (or list of Id’s)
Year <= Year From AccoutingDate
Month < Month From AccoutingDate
Group the results by CurrencyId and sum the following fields
LocalAmountDebit
LocalAmountCredit
ForeignAmountDebit
ForeignAmountDebit

In case the requested AccoutingDate is not a full month we’ll need to calculate the balance for this specific month  by retrieving LedgerTransactions table
AccountingDate = between first day of the month and the day of the AccountingDate
AccountId = GLAccountId (or list of Id’s)
Group the results by CurrencyId and sum the following fields
LocalAmountDebit
LocalAmountCredit
ForeignAmountDebit
ForeignAmountDebit
Sum the matching  records from GLAccountTotalByMonths +LedgerTransactions and return the totals

*/
