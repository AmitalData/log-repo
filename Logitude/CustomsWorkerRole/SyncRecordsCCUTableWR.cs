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
        private readonly SyncRecordQuery syncRecordQuery = new SyncRecordQuery();        

        public override void WorkOnce()
        {
            if (isFirstTime)
            {
                isFirstTime = false;
                Scheduler(SendSyncRecoredToUnifreightQueue, 30000, "SendSyncRecoredToUnifreightQueue");
            }
        }

        private void SendSyncRecoredToUnifreightQueue()
        {
            List<SyncRecord> syncRecordsInQueueList = new List<SyncRecord>();
            List<SyncRecord> records = syncRecordQuery.GetAndMarkNewSyncRecord();

            if (records == null || records.Count == 0)
                return;

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
                    Logger.LogMe("error on SendToUnifreightQueue, Exception: " + e.ToString(), true);
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
                    Logger.LogMe("error on Schdule action " + actionName + ", Exception: " + e.ToString(), true);
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
            
            Logger.LogMe("SendToQueue, fileNos: " + fileNos, false);

            UnifreightQueueService.Insert(tenant, priority, queueName, subject, storageFolder, action, tableName, fileNos);
        }
    }
}
