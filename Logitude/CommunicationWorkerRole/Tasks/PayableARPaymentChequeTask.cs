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
                LogInfo(new StringBuilder().Append(DateTime.Now.ToString()).AppendLine("PayableARPaymentChequeTask:Start").ToString());
                var tenantsAccountingActivated = new List<int>();
                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
                {
                    var tenantRepo = new TenantRepository(0);
                    tenantsAccountingActivated = tenantRepo.All().Where(r => r.AccountingActivated).Select(r => r.Id).ToList();
                }
                _SB.Append(DateTime.Now.ToString()).Append("tenantsAccountingActivated:").Append(String.Join(",", tenantsAccountingActivated)).AppendLine();
                LogInfo(new StringBuilder().Append(DateTime.Now.ToString()).Append("tenantsAccountingActivated:").Append(String.Join(",", tenantsAccountingActivated)).AppendLine().ToString());
                foreach (var tenant in tenantsAccountingActivated)
                {
                    _SB.Append(DateTime.Now.ToString()).Append("tenant:").Append(tenant).AppendLine();
                    LogInfo(new StringBuilder().Append(DateTime.Now.ToString()).Append("tenant:").Append(tenant).AppendLine().ToString());
                    try
                    { 
                        var myPostDatedChequesRedemptionBatch = new PostDatedChequesRedemptionBatch();
                        myPostDatedChequesRedemptionBatch.RunAllPayablePostDatedARPaymentCheques(tenant);
                        string responseText = myPostDatedChequesRedemptionBatch.ResponseText();
                        _SB.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                        LogInfo(new StringBuilder().Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine().ToString());
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                        _SB.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                        LogException(new StringBuilder().Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine().ToString());
                        ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", $"PayableARPaymentChequeTask({tenant})", null);
                        NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                    }
                    
                }
                
            }
            finally
            {

                if (failed)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError(_SB.ToString());
                    LogException(_SB.ToString());
                    throw new Exception(_SB.ToString());
                }
                else
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo(_SB.ToString());
                }
            }
            //base.StartTask();
        }
    }
}
