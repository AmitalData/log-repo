
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class CardIndexReportService
    {
        private IAccountingContext _AccountingContext;
        private CardIndexReportParams _Param;
        private List<string> _allIdAccounts;
        public List<LedgerTransactionBalanceResponse> CardIndexs { get; }
        const int constPageSize = 20 * 1000;
        public CardIndexReportService(IAccountingContext accountingContext, CardIndexReportParams param)
        {
            _AccountingContext = accountingContext;
            _Param = param;
            CardIndexs = new List<LedgerTransactionBalanceResponse>();
        }
        public void Run()
        {
            var sw = Stopwatch.StartNew();
            CheckParams();
            GetGLAccountPopulation();
            foreach (var currGLAccountId in _allIdAccounts)
            {
                var myLedgerTransactionBalanceFilter = this._Param as LedgerTransactionBalanceFilter;
                myLedgerTransactionBalanceFilter.GLAccountId = currGLAccountId;
                myLedgerTransactionBalanceFilter.PageSize = constPageSize;
                myLedgerTransactionBalanceFilter.PageStartAtRecordIndex = 0;
                myLedgerTransactionBalanceFilter.CallBack = null;
                myLedgerTransactionBalanceFilter
                    .ClacOpenReconciledAmount_OnlyWithout_IncludeRelatedCurrenciesAccount_IncludeChildAccounts = 
                    !(myLedgerTransactionBalanceFilter.IncludeChildAccounts || myLedgerTransactionBalanceFilter.IncludeRelatedCurrenciesAccount);
                var myLedgerTransactionBalanceService = new LedgerTransactionBalanceService(_AccountingContext, myLedgerTransactionBalanceFilter, this._Param.IsReconciled);
                myLedgerTransactionBalanceService.Run();
                //if (this._Param.IsReconciled.HasValue /*&& _Param.IsReconciled==false*/)
                //{
                //    bool IsReconciled =this._Param.IsReconciled.GetValueOrDefault();
                //    myLedgerTransactionBalanceService.Response.MyLedgerTransactionList = myLedgerTransactionBalanceService.Response.MyLedgerTransactionList
                //        .Where(r => r.IsReconciled == IsReconciled).ToList();
                //}
                myLedgerTransactionBalanceService.Response.GLAccountId = currGLAccountId;
                CardIndexs.Add(myLedgerTransactionBalanceService.Response);

            }
            Debug.WriteLine($"CardIndexReportService count:{_allIdAccounts.Count} took {sw.Elapsed}");
        }

        private void GetGLAccountPopulation()
        {
            bool includeControlAccount = false;

            var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
            //                    var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccountsCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
            //                        _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.IncludeChildAccounts);
           


                var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccountsTypeCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
                    _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.AccountTypeCode, _Param.ChartOfAccountsId, _Param.IncludeChildAccounts,
                    _Param.ChartOfAccountsTypeCode,
                    _Param.SalesmanId,
                    includeControlAccount,_Param.UseSecurityLevel,_Param.CollectorId);
                var hash = new HashSet<string>(hashsetallIdAccounts);
                _allIdAccounts = new List<string>(hash);// hashsetallIdAccounts);
           
            
            if (_Param.IncludeRelatedCurrenciesAccount)
            {

            }

            if (_Param.IncludeChildAccounts)
            {
            }

        }

        private void CheckParams()
        {
            GLAccountFilterIsMust();

        }

        private void GLAccountFilterIsMust()
        {
            if (string.IsNullOrWhiteSpace(this._Param.Category1Id + this._Param.Category2Id + this._Param.Category3Id + this._Param.Category4Id + this._Param.Category5Id + this._Param.ChartOfAccountsId + this._Param.AccountTypeCode + this._Param.GLAccountId + _Param.ChartOfAccountsTypeCode + _Param.SalesmanId))
            {

                throw new Exception("GLAccountFilterIsMust");
            }
        }
    }

    public class CardIndexReportParams : LedgerTransactionBalanceFilter
    {
        public CardIndexReportParams()
        {

        }

        public string Category1Id { get; set; }
        public string Category2Id { get; set; }
        public string Category3Id { get; set; }
        public string Category4Id { get; set; }
        public string Category5Id { get; set; }
        public string AccountTypeCode { get; set; }
        public string ChartOfAccountsId { get; set; }

        public bool? IsReconciled { get; set; }
        public string ChartOfAccountsTypeCode { get;  set; }
        public string SalesmanId { get; set; }
        public string CollectorId { get; set; }
        public bool UseSecurityLevel { get; set; }
        public string CollectorId { get; set; }

        //public bool IncludeRelatedCurrenciesAccount { get; set; }

    }
}
