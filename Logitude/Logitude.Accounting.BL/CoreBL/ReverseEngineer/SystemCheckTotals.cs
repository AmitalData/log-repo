using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class SystemCheckTotals
    {

        public void TotalSumMustBeZero(int tenant)
        {

            using (var scope = TransactionFactory.GetTransaction())
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                var repoGLAccount = new GLAccountRepository(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);

                var maxYear = repoGLAccountTotalByMonth.GetAll(tenant).Max(r => r.Year);
                var minYear = repoGLAccountTotalByMonth.GetAll(tenant).Min(r => r.Year);
                var fullAccountingSetting = //Hope From Cache
                FullAccountingSettingQueryService
                .Get(tenant);
                for (int i = 0; i < 2; i++)
                {
                    bool controlAccountLevel = i == 1;
                    var qAllCardsAndDetialsAccType = //Get The Account List
                         repoGLAccount.
                         GetQAllCardsAndDetailsAccType(tenant,
                         controlAccountLevel ? fullAccountingSetting.CustomerControlAccountId : "",
                         controlAccountLevel ? fullAccountingSetting.VendorControlAccountId : "",
                         ""
                         ,
                         controlAccountLevel ? fullAccountingSetting.FileControlAccountId : "");


                    for (int year = minYear; year <= maxYear; year++)
                        for (int month = 1; month < 13; month++)
                        {

                            var q = (from tot in repoGLAccountTotalByMonth
                                     .GetQuaryableMonthTotals(year, month, tenant)
                                     join glAcc in qAllCardsAndDetialsAccType
                                     on tot.AccountId equals glAcc.Id
                                     group tot by 1 into g
                                     select g.Sum(tot => tot.LocalAmountDebit - tot.LocalAmountCredit)
                                );
                            var res = q.FirstOrDefault();
                            if (res != 0)
                            {
                                var accIdList = qAllCardsAndDetialsAccType.Select(r => r.Id);
                                throw new Exception($"not equal to zero for year:{year} month:{month } controlAccountLevel:{controlAccountLevel}");
                            }

                        }
                }
            }
        }

        public void LedgerTransactionSumMustBeZero(int tenant, int YYYY)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                var repoGLAccount = new GLAccountRepository(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
                var ledgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                DateTime from = new DateTime(YYYY, 1, 1);
                DateTime toDate = new DateTime(YYYY, 12, 31);
                var res = ledgerTransactionRepository.GetAll(tenant)
                    .Where(rec => EntityFunctions.TruncateTime(rec.AccountingDate) >= from.Date)
                    .Where(rec => EntityFunctions.TruncateTime(rec.AccountingDate) <= toDate.Date)
                    //.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit);
                    .Select(r => r.LocalAmountDebit - r.LocalAmountCredit)
                    .DefaultIfEmpty(0)
                    .Sum();
                if (res != 0)
                {

                    throw new Exception($"LedgerTransactionSumMustBeZero:not equal to zero for year:{YYYY} ");
                }

            }

        }
    }
}

