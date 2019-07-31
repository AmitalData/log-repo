//using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
#if true



    public class BalanceGroupByCollectorService
    {
        private BalanceGroupByCollectorReportParam _Param;
        private GLAccountRepository _myGLAccountRepository;
        private IQueryable<GLAccount> _qAllAccAging4AccountTypeCode_CustomerOrVendor;
        private IAccountingContext _AccountingContext;
        private GLAccountQueryService _myGLAccountQueryService;
        IQueryable<string> _AccountIdList = null;
        public BalanceGroupByCollectorService(BalanceGroupByCollectorReportParam param)
        {
            _Param = param;
        }
        public List<BalanceGroupByCollectorM> RunReport()
        {
            CheckeParams();
            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Param.Tenant);
                //var qsGLAccountTotalByMonth = new GLAccountTotalByMonthQueryService(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
                var repoLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                _myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
                _myGLAccountRepository = new GLAccountRepository(_AccountingContext);
                var contactRepository = new ContactRepository(_Param.Tenant);
                var myGLAccountMoreDataRepository = new GLAccountMoreDataRepository(_AccountingContext);


                FilterGLAccountByParams();

                bool byDueDate = (_Param.GroupByDate == AgingReportParam.DateEnum.DueDate);
                var qAccBalance = (from acc in _AccountIdList
                                         join md in myGLAccountMoreDataRepository.GetAll(_Param.Tenant) on acc equals md.AccountId

                                         select new
                                         {
                                             MyBalance = byDueDate ? md.LocalBalanceInDue : md.BalanceInLocalCurrency,
                                             MyAccId = acc
                                         }
                           );
                if (_2Test)
                {
                    var rqCollectorBalance = qAccBalance.ToList();
                }
                var qCollBalance = (from accBalance in qAccBalance
                                    join card in (_AccountingContext as AccountingContext).Cards.Where(r => r.Tenant == _Param.Tenant) on accBalance.MyAccId equals card.GLAccountId into CardLeftOuterJoin
                                    from cardCanBeEmpty in CardLeftOuterJoin.DefaultIfEmpty()
                                    join cust in (_AccountingContext as AccountingContext).Customers.Where(r => r.Tenant == _Param.Tenant) on cardCanBeEmpty.Id equals cust.Id into custLeftOuterJoin


                                    //join cust in (_AccountingContext as AccountingContext).Customers.Where(r => r.Tenant == _Param.Tenant) on accBalance.MyAccId equals cust.Id into custLeftOuterJoin
                                    from custCanBeEmpty in custLeftOuterJoin.DefaultIfEmpty()

                                    select new
                                    {
                                        MyBalance = accBalance.MyBalance,
                                        MyCollectorId = custCanBeEmpty != null ? custCanBeEmpty.CollectorId ?? "" : ""
                                    }
);

                if (_2Test)
                {
                    var rqCollBalance = qCollBalance.ToList();
                }

                var gCollectorTotalBalance = (
                from r in qCollBalance
                group r by r.MyCollectorId into groupbyCollector
                select new
                {
                    MyCollectorId = groupbyCollector.Key,
                    MyCollectorBalance = groupbyCollector.Sum(r => r.MyBalance)
                }
                );
                if (_2Test)
                {
                    var rgCollectorTotalBalance = gCollectorTotalBalance.ToList();
                }
                var qCollectorNameTotalBalance =
                    (
                    from r in gCollectorTotalBalance
                    join u in
                    //contactRepository.GetAllContacts(_Param.Tenant)
                    (_AccountingContext as AccountingContext).Contacts.Where(r => r.Tenant == _Param.Tenant)
                    on r.MyCollectorId equals u.Id into userLeftOuter
                    from uLeftOuter in userLeftOuter.DefaultIfEmpty()
                    select new BalanceGroupByCollectorM
                    {
                        TotalPerCollector = r.MyCollectorBalance,
                        CollectorId = uLeftOuter.Id,
                        CollectorLocalName = uLeftOuter.LocalName,
                        CollectorEnglishName = uLeftOuter.EnglishName
                    }
                    );

                var res = qCollectorNameTotalBalance.ToList();
                return res;
            }
        }

        private void CheckeParams()
        {
            switch (_Param.AccountTypeCode)
            {
                case AgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1:
                    throw new Exception("AccountTypeCode  must Aging4AccountTypeCodeEnum.Customer2/Aging4AccountTypeCodeEnum.Vendor3");
                    break;
                case AgingReportParam.Aging4AccountTypeCodeEnum.Customer2:
                    break;
                case AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3:
                    break;
                default:
                    throw new Exception("AccountTypeCode  must Aging4AccountTypeCodeEnum.Customer2/Aging4AccountTypeCodeEnum.Vendor3");
                    break;
            }

        }
        bool _2Test = false;
        private void FilterGLAccountByParams()
        {

            var accType = GetAccountType(_Param.AccountTypeCode);
            _qAllAccAging4AccountTypeCode_CustomerOrVendor = _myGLAccountRepository
                //.GetQAllByAccountTypeCode(_Param.Tenant, GetAccountType(_Param.Aging4AccountTypeCode));
                .GetAll(_Param.Tenant)
                .Where(a => a.AccountTypeCode == accType);

            if (!String.IsNullOrWhiteSpace(_Param.VendorCustomerId))
            {
                var qVendorCustomerId = (from acc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                                         where acc.Id == _Param.VendorCustomerId
                                         select acc.Id
                );
                Union_AccountIdList(qVendorCustomerId);
            }







            if (!String.IsNullOrWhiteSpace(_Param.CollectorId))
            {

                var qGLAccIdByCollectorId = _myGLAccountQueryService.GetQGLAccIdByCollectorId(_Param.Tenant, _Param.CollectorId, GetAccountType(_Param.AccountTypeCode));
                Join_AccountIdList(qGLAccIdByCollectorId);

            }


            if (_AccountIdList == null)//Bug 42372: Reco - Problem with Code (MaxAmount)
            {
                //without any filter so get all by AccountTypeCode
                _AccountIdList = _qAllAccAging4AccountTypeCode_CustomerOrVendor.Select(r => r.Id).AsQueryable<string>();

            }
            else
            {

                _AccountIdList =
                    (from filterAccId in _AccountIdList
                     join glAcc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                     on filterAccId equals glAcc.Id
                     select filterAccId);
            }

            if (_2Test)
            {
                var r = _AccountIdList.ToList();
            }

        }
        private void Join_AccountIdList(IQueryable<string> addMyQueryable)
        {
            if (_AccountIdList == null)
            {
                _AccountIdList = addMyQueryable;
            }
            else
            {
                _AccountIdList =
                    (from a in _AccountIdList
                     join a2 in addMyQueryable
                     on a equals a2
                     select a);

            }

        }
        private void Union_AccountIdList(IQueryable<string> addMyQueryable)
        {
            if (_AccountIdList == null)
            {
                _AccountIdList = addMyQueryable;
            }
            else
            {
                _AccountIdList = _AccountIdList
                //.AddRange
                .Union
                (addMyQueryable);
            }

        }

        private string GetAccountType(Logitude.Accounting.BL.CoreBL.Reports.AgingReportParam.Aging4AccountTypeCodeEnum Aging4AccountTypeCode)
        {
            return ((int)Aging4AccountTypeCode).ToString();
        }

    }

    public class BalanceGroupByCollectorReportParam
    {
        public AgingReportParam.Aging4AccountTypeCodeEnum AccountTypeCode { get; set; }
        public int Tenant { get; set; }
        public string VendorCustomerId { get; set; }
        public string CollectorId { get; set; }
        public AgingReportParam.DateEnum GroupByDate { get; set; }
        public string GroupByDate_Options { get; set; }
        public string AccountTypeCode_Options { get; set; }

    }

    public class BalanceGroupByCollectorM
    {
        public decimal? TotalPerCollector { get; set; }
        public string CollectorId { get; set; }
        public string CollectorLocalName { get; set; }
        public string CollectorEnglishName { get; set; }
    }
#endif
}
