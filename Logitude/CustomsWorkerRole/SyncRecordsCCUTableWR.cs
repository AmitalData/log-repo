using CustomsWorkerRole.Utils;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomsWorkerRole
{
    public class SyncRecordsCCUTableWR : CustomsWorkerEntryPoint
    {
        private bool isFirstTime = true;        

        public override void WorkOnce()
        {
            if (isFirstTime)
            {
                Logger.LogDebug("SyncRecordsCCUTableWR start run (WorkOnce)");

                isFirstTime = false;
                Scheduler(SendSyncRecoredToUnifreightQueue, 30000, "SendSyncRecoredToUnifreightQueue");
            }
        }

        private void SendSyncRecoredToUnifreightQueue()
        {
            Logger.LogDebug("SendSyncRecoredToUnifreightQueue start run");

            List<SyncRecord> syncRecordsInQueueList = new List<SyncRecord>();
            SyncRecordQuery syncRecordQuery = new SyncRecordQuery();
            List<SyncRecord> records = syncRecordQuery.GetAndMarkNewSyncRecord();

            if (records == null || records.Count == 0)
                return;

            Logger.LogDebug("SendSyncRecoredToUnifreightQueue, records count: " + records.Count);

            var RecordsGroupByTenants = records.GroupBy(record => record.Tenant).ToList();

            foreach (var group in RecordsGroupByTenants)
            {
                List<SyncRecord> recordsOfTenant = group.ToList();
                try
                {
                    SendToQueue(recordsOfTenant, group.Key);
                    syncRecordsInQueueList.AddRange(recordsOfTenant);
                }
                catch (Exception e)
                {                    
                    Logger.LogError(e, "error on SendToUnifreightQueue, tenant: {0}", group.Key);
                }
            }

            syncRecordQuery.UpdateStatusInQueue(syncRecordsInQueueList);            
        }

        private void Scheduler(Action actionAsync, double time, string actionName = null)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = time;
            aTimer.Elapsed += (o, eea) =>
            {
                try
                {
                    actionAsync();
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "error on Schdule action {0}", actionName);
                }
                finally
                {
                    aTimer.Start();
                }
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void SendToQueue(List<SyncRecord> records, int tenant)
        {
            int priority = 4;
            string queueName = "externaltasksqueue" + tenant + priority;
            string cacheName = "SyncRecordsCCUTableWR.DbQueueService." + queueName;
            string subject = "Sync Unifreight Table";
            string storageFolder = "ExternalTasksQueue";
            string action = "SyncUnifreightTable";
            string tableName = "SyncRecord";
            string fileNos = string.Join(",", records.ConvertAll(record => record.FileNo.Trim()).Distinct());
            
            Logger.LogDebug("SendToQueue, tenant: {0}, fileNos: {1}", tenant, fileNos);

            UnifreightQueueService.Insert(tenant, priority, queueName, subject, storageFolder, action, tableName, fileNos);
        }
    }
}
