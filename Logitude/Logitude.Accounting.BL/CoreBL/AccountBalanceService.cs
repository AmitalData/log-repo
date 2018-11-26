#if false
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
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
    public class AccountBalanceService
    {
        int _Tenant;
        string _GLAccountId;
        DateTime _AccoutingDate;
        //bool _IncludeChildAccounts;
        //bool _IncludeRelatedCurrenciesAccount;
        private IAccountingContext _AccountingContext;
        private bool _HaveAccountingQueued;

        //private List<string> _ListOfAccountId;
        private IQueryable<string> _ListOfAccountId;
        private Stopwatch _sw;
        private StringBuilder _StringBuilder;
        public AccountBalanceService(IAccountingContext accountingContext,int tenant, 
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
            AccountBalance = new AccountBalanceM() { Tenant = _Tenant};
            _StringBuilder = new StringBuilder();
        }
        public void CalculateBalance(DateTime accoutingDate, bool includeAccoutingDateLTransaction = false, bool verbose = false)
        {
            _AccoutingDate = accoutingDate;
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
                if (_AccountingContext==null)
                {
                    _AccountingContext = AccountingContext.GetContext(_Tenant);    
                }
                
                _sw.Restart();

                
                //IncludeRelatedCurrenciesAccount(_ListOfAccountId);
                //LogIt("IncludeRelatedCurrenciesAccount");
                //IncludeChildAccounts(_ListOfAccountId);
                //LogIt("IncludeChildAccounts");


                var firstDayOfMonth = new DateTime(_AccoutingDate.Year, _AccoutingDate.Month, 1); ;
                var dateLast = firstDayOfMonth.AddMilliseconds(-1);


                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {

                    var myGLAccountTotalByMonthQueryService = new GLAccountTotalByMonthQueryService(_AccountingContext);
                    var currencySumUntillMounth = myGLAccountTotalByMonthQueryService.GetCurrencySumUntillNotInclude(_ListOfAccountId, _AccoutingDate.Year, _AccoutingDate.Month, _Tenant);
                    LogIt("currencySumUntillMounth");
                    if (verbose)
                    {
                        AccountBalance.verbose = new AccountBalanceverboseM();
                        AccountBalance.verbose.CurrencySumUntillMounth = currencySumUntillMounth;
                    }

                    var accoutingDateUntillNotInclude = _AccoutingDate.Date;
                    if (includeAccoutingDateLTransaction)
                    {
                        accoutingDateUntillNotInclude = accoutingDateUntillNotInclude.AddDays(1);
                    }
                    var myLedgerTransactionQueryService = new LedgerTransactionQueryService(_AccountingContext);
                    var thisMounthGLAccountTotalByMonthByAccountingDate = myLedgerTransactionQueryService
                        .CalcGLAccountTotalByMonthByAccountingDate(firstDayOfMonth, accoutingDateUntillNotInclude, _Tenant, _ListOfAccountId);
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
                    _HaveAccountingQueued = AnyAccountingQueued();
                    LogIt("AnyAccountingQueued");
                    AccountBalance.HaveAccountingQueued = _HaveAccountingQueued;


                    var totals = (from rec in currencySumUntillMounth.Union(theMounthCurrencySum)
                                  group rec by rec.CurrencyId into gCurrencyId
                                  select new CurrencySum()
                                  {
                                      AccountId = _GLAccountId,
                                      CurrencyId = gCurrencyId.Key,
                                      LocalAmountCredit = gCurrencyId.Sum(rec => rec.LocalAmountCredit ),
                                      LocalAmountDebit = gCurrencyId.Sum(rec => rec.LocalAmountDebit ),
                                      ForeignAmountCredit = gCurrencyId.Sum(rec => rec.ForeignAmountCredit ),
                                      ForeignAmountDebit = gCurrencyId.Sum(rec => rec.ForeignAmountDebit )
                                  }
                     ).ToList();

                    AccountBalance.Totals = totals;
                    AccountBalance.TotalLocalAmountDebit =AccountBalance.Totals.Sum(r => r.LocalAmountDebit);
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

        private bool AnyAccountingQueued()
        {
            var myJournalQueryService = new JournalQueryService(_AccountingContext);
            var have = myJournalQueryService.GetAnyPendingApprovedDev(_ListOfAccountId, _Tenant);
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
                               BalanceForeign = tot.ForeignAmountDebit - tot.ForeignAmountCredit
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



#endif