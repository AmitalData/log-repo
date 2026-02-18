using CustomsWorkerRole.Utils;
using Logitude.Server.Tools.Utils;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Microsoft.Practices.ObjectBuilder2;
using NetCommonHelper.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;
using System.Threading;
using System.Configuration;

namespace CustomsWorkerRole
{
    public class SyncRecordsCCUTableWR : CustomsWorkerEntryPoint
    {
        private bool isFirstTime = true;
        private static readonly string lockKey = "SyncRecordsCCUTableWR";
        private List<int> tennats;
        private static int mainTennat = 0;

        public override void WorkOnce()
        {
            if (isFirstTime)
            {
                DevLog.Instance.WriteDebug("SyncRecordsCCUTableWR start run (WorkOnce)");

                isFirstTime = false;

                InitScheduler();
                InitTenants();
            }
        }

        private void InitScheduler()
        {
            double sendSyncRecoredToUnifreightQueueInterval = 30000;
            if (double.TryParse(ConfigurationManager.AppSettings["sendSyncRecoredToUnifreightQueueInterval"], out double configuredInterval))
                sendSyncRecoredToUnifreightQueueInterval = configuredInterval;

            Scheduler(SendSyncRecoredToUnifreightQueue, sendSyncRecoredToUnifreightQueueInterval, "SendSyncRecoredToUnifreightQueue");
            
            double returnToQueueInterval = 600000;
            if (double.TryParse(ConfigurationManager.AppSettings["ReturnToQueueInterval"], out double configuredInterval2))
                returnToQueueInterval = configuredInterval2;

            Scheduler(ReturnToQueue, returnToQueueInterval, "ReturnToQueue");        
        }

        private void InitTenants()
        {
            bool haveTenantDB = int.TryParse(ConfigurationManager.AppSettings["TenantDB"], out int TenantDB);
            tennats = haveTenantDB ?
                new List<int>() { TenantDB } :
                CustomsSettingQueryService.GetNotSeperatedDB().Select(x => x.Tenant).ToList();

            if(haveTenantDB)
                mainTennat = TenantDB;
        }

        public void SendSyncRecoredToUnifreightQueue()
        {
            try
            {
                bool finish = false;
                while (!finish)
                {
                    DevLog.Instance.WriteDebug("SendSyncRecoredToUnifreightQueue start run");

                    List<SyncRecord> syncRecordsInQueueList = new List<SyncRecord>();
                    SyncRecordQuery syncRecordQuery = new SyncRecordQuery();

                    Lock();

                    List<SyncRecord> records = syncRecordQuery.GetAndMarkNewSyncRecord(tennats);

                    if (records == null || records.Count == 0)
                    {
                        Unlock();
                        finish = true;
                        return;
                    }

                    DevLog.Instance.WriteDebug("SendSyncRecoredToUnifreightQueue, records count: " + records.Count);                    

                    IEnumerable<IGrouping<int, SyncRecord>> RecordsGroupByTenants = records.GroupBy(record => record.Tenant);

                    foreach (IGrouping<int, SyncRecord> group in RecordsGroupByTenants)
                    {
                        List<SyncRecord> recordsOfTenant = group.ToList();
                        if (recordsOfTenant.Count == 0)
                            continue;

                        try
                        {
                            SendToQueue(recordsOfTenant, group.Key);
                            syncRecordsInQueueList.AddRange(recordsOfTenant);
                        }
                        catch (Exception e)
                        {
                            DevLog.Instance.WriteFatal(e, "error on SendToUnifreightQueue, tenant: " + group.Key);
                        }
                    }

                    syncRecordQuery.UpdateStatus(syncRecordsInQueueList, SyncRecordStatus.InQueue);

                    Unlock();

                    Thread.Sleep(100);
                }
            }
            catch (DbUpdateException e) when (e.Message.Contains("SyncRecordsCCUTableWR") && e.Message.Contains("GeneralLock"))
            {
                DevLog.Instance.WriteTrace("Another WR lock this job");
            }
            catch (Exception e)
            {
                DevLog.Instance.WriteFatal(e, "error on SendToUnifreightQueue");
                try
                {
                    Unlock();
                }
                catch (Exception ex)
                {
                    DevLog.Instance.WriteFatal(ex, "Unlock failed");
                }
            }
        }

        private static void Lock()
        {
            var concurrentKiller = new ConcurrentKiller();
            concurrentKiller.FreeLockIfCreated15MinOld(lockKey, mainTennat);
            concurrentKiller.LockOrCrashOnCommitDueUnique(lockKey, mainTennat);
        }

        private static void Unlock()
        {
            new ConcurrentKiller().FreeLock(lockKey, mainTennat);
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
                    DevLog.Instance.WriteFatal(e, "error on Schdule action " + actionName);
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
            string subjectUnifreightTables = "Sync Unifreight Table";
            string storageFolder = "ExternalTasksQueue";
            string actionUnifreightTables = "SyncUnifreightTable";
            string tableName = "SyncRecord";
            string fileNos = string.Join(",", records
                .Where(record => record.KeyVal != "ALL").ToList()
                .ConvertAll(record => record.FileNo.Trim())
                .Distinct());

            DevLog.Instance.WriteInfo("SendToQueue, fileNos: " + fileNos + ", tenant: " + tenant);

            UnifreightQueueService.Insert(tenant, priority, queueName, subjectUnifreightTables, storageFolder, actionUnifreightTables, tableName, fileNos);
        }

        public void ReturnToQueue()
        {
            DevLog.Instance.WriteDebug("ReturnToQueue start run");

            SyncRecordQuery syncRecordQuery = new SyncRecordQuery();
            List<SyncRecord> records = new SyncRecordQuery().GetNeedToReturnToQueue();

            if (records == null || records.Count == 0)
            {
                DevLog.Instance.WriteDebug("ReturnToQueue, records count: 0");
                return;
            }

            DevLog.Instance.WriteDebug("ReturnToQueue, records count: " + records.Count);
            DevLog.Instance.WriteTrace("ReturnToQueue, records: " + string.Join(", ", records));

            records.ForEach(record =>
            {
                record.IsSync = SyncRecordStatus.New;
                record.LastRequeueTime = DateTime.Now;
                record.IsRequeued = true;
            }) ;

            syncRecordQuery.Update(records);            

            DevLog.Instance.WriteDebug("ReturnToQueue finish");
        }
    }
}