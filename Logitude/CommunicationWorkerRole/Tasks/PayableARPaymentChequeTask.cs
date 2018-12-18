using Logitude.Accounting.BL.Utils;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CommunicationWorkerRole.Tasks
{
    public class PayableARPaymentChequeTask : TaskManagerBase
    {
        private StringBuilder _SB;

        public PayableARPaymentChequeTask(string Id, int tenant) : base(Id, tenant)
        {
            _SB = new StringBuilder();
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                _SB.Append(DateTime.Now.ToString()).AppendLine("PayableARPaymentChequeTask:Start");
                var tenantsAccountingActivated = new List<int>();
                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                {
                    var tenantRepo = new TenantRepository(0);
                    tenantsAccountingActivated = tenantRepo.All().Where(r => r.AccountingActivated).Select(r => r.Id).ToList();
                }
                _SB.Append(DateTime.Now.ToString()).Append("tenantsAccountingActivated:").Append(String.Join(",", tenantsAccountingActivated)).AppendLine();

                foreach (var tenant in tenantsAccountingActivated)
                {
                    _SB.Append(DateTime.Now.ToString()).Append("tenant:").Append(tenant).AppendLine();
                    try
                    {
                        var myPostDatedChequesRedemptionBatch = new PostDatedChequesRedemptionBatch();
                        myPostDatedChequesRedemptionBatch.RunAllPayablePostDatedARPaymentCheques(tenant);
                        string responseText = myPostDatedChequesRedemptionBatch.ResponseText();
                        _SB.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                        _SB.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                        ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", $"PayableARPaymentChequeTask({tenant})", null);
                    }
                    
                }
            }
            finally
            {
                AccountingLogger.LogMe(_SB.ToString(),failed);
                if (failed)
                {
                    throw new Exception(_SB.ToString());
                }
            }
            //base.StartTask();
        }
    }
}
