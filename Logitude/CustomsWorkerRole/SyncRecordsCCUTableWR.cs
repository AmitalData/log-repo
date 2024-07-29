using CustomsWorkerRole.Utils;
using Logitude.BL.CommonDataModel.EntityQueries;
using Microsoft.Practices.ObjectBuilder2;
using NetCommonHelper.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace CustomsWorkerRole
{
    public class SyncRecordsCCUTableWR : CustomsWorkerEntryPoint
    {
        private bool isFirstTime = true;

        public override void WorkOnce()
        {
            if (isFirstTime)
            {
                DevLog.Instance.WriteDebug("SyncRecordsCCUTableWR start run (WorkOnce)");

                isFirstTime = false;
                Scheduler(SendSyncRecoredToUnifreightQueue, 30000, "SendSyncRecoredToUnifreightQueue");
            }
        }

        public void SendSyncRecoredToUnifreightQueue()
        {
            DevLog.Instance.WriteDebug("SendSyncRecoredToUnifreightQueue start run");

            List<SyncRecord> syncRecordsInQueueList = new List<SyncRecord>();
            SyncRecordQuery syncRecordQuery = new SyncRecordQuery();
            TenantQuery tenantQuery = new TenantQuery();
            List<SyncRecord> records = syncRecordQuery.GetAndMarkNewSyncRecord();

            if (records == null || records.Count == 0)
                return;

            DevLog.Instance.WriteDebug("SendSyncRecoredToUnifreightQueue, records count: " + records.Count);

            List<SyncRecord> newRecords = InsertRecordsForCloseTables(syncRecordQuery, tenantQuery, records);

            IEnumerable<IGrouping<int, SyncRecord>> RecordsGroupByTenants = records.Concat(newRecords).GroupBy(record => record.Tenant);

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

            syncRecordQuery.UpdateStatusInQueue(syncRecordsInQueueList);
        }

        private static List<SyncRecord> InsertRecordsForCloseTables(SyncRecordQuery syncRecordQuery, TenantQuery tenantQuery, List<SyncRecord> records)
        {
            IEnumerable<SyncRecord> tenant0CloseTableRecords = records.Where(record => record.KeyVal == "ALL" && record.Tenant == 0);
            List<SyncRecord> newRecords = new List<SyncRecord>();
            List<SyncRecord> RemoveRecords = new List<SyncRecord>();
            tenant0CloseTableRecords.ForEach(record =>
            {
                RemoveRecords.Add(record);
                records.Remove(record);
                tenantQuery.GetAll(true).ForEach(tenant =>
                {
                    newRecords.Add(new SyncRecord
                    {
                        FileNo = record.FileNo,
                        KeyVal = record.KeyVal,
                        Tenant = tenant.Id,
                        IsSync = record.IsSync,
                        CreateDate = DateTime.UtcNow,
                        Entname = record.Entname,
                        SyncDT = record.SyncDT,
                        TrigAction = record.TrigAction,
                    });
                });
            });

            syncRecordQuery.Add(newRecords);
            syncRecordQuery.Remove(RemoveRecords);
            return newRecords;
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
            string subjectCloseTables = "Sync close Table";
            string storageFolder = "ExternalTasksQueue";
            string actionUnifreightTables = "SyncUnifreightTable";
            string actionCloseTables = "SyncCloseTables";
            string tableName = "SyncRecord";
            string fileNos = string.Join(",", records
                .Where(record => record.KeyVal != "ALL").ToList()
                .ConvertAll(record => record.FileNo.Trim())
                .Distinct());
            string tablesName = string.Join(",", records
                .Where(record => record.KeyVal == "ALL").ToList()
                .ConvertAll(record => record.Entname.Trim())
                .Distinct());

            DevLog.Instance.WriteInfo("SendToQueue, fileNos: " + fileNos + ", closeTables: " + tablesName + ", tenant: " + tenant);

            if(fileNos.Length > 0)
                UnifreightQueueService.Insert(tenant, priority, queueName, subjectUnifreightTables, storageFolder, actionUnifreightTables, tableName, fileNos);

            if(tablesName.Length > 0)
                UnifreightQueueService.Insert(tenant, priority, queueName, subjectCloseTables, storageFolder, actionCloseTables, tableName, tablesName);
        }
    }
}
