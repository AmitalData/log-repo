using Logitude.Accounting.BL.CoreBL;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommunicationWorkerRole.Tasks
{
    public class RecalculateCashbooksTotalsTask: TaskManagerBase
    {

        private StringBuilder _SB;
        int tenant;

        public RecalculateCashbooksTotalsTask(string Id, int tenant) : base(Id, tenant)
        {
            _SB = new StringBuilder();
            this.tenant = tenant;
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                _SB.Append(DateTime.Now.ToString()).AppendLine("RecalculateCashbooksTotalsTask:Start");
                var tenantsAccountingActivated = new List<int>();
                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                {
                    var tenantRepo = new TenantRepository(0);
                    tenantsAccountingActivated = tenantRepo.All().Where(r => r.AccountingActivated).Select(r => r.Id).ToList();
                    scope.Complete();
                }
                _SB.Append(DateTime.Now.ToString()).Append("tenantsAccountingActivated:").Append(String.Join(",", tenantsAccountingActivated)).AppendLine();
                    try
                    {
                        var cashbookService = new CashbookService();
                        var updatedCount = cashbookService.RecalculateCashbooksTotals(tenant);
                        string responseText = updatedCount;
                        _SB.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                        _SB.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"RecalculateCashbooksTotalsTask()", null);
                    }
            }
            finally
            {
                if (failed)
                {
                    throw new Exception(_SB.ToString());
                }
            }
        }

    }
}
