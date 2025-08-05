using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using System;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using NetCommonHelper.Logger;
using System.Data.Entity;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class SyncRecordRepository : IRepository<SyncRecord>
    {
        AmitalContext context;
        DevLog logger = DevLog.Instance;

        public SyncRecordRepository(AmitalContext context)
        {
            this.context = context;
        }

        public SyncRecordRepository(int tenant)
        {
            context = AmitalContext.GetContext(tenant);
        }

        public void Add(SyncRecord entity)
        {
            context.SyncRecord.Add(entity);
        }

        public void Add(List<SyncRecord> records)
        {
            context.SyncRecord.AddRange(records);
        }

        public void Remove(SyncRecord entity)
        {
            context.SyncRecord.Attach(entity);
            context.SyncRecord.Remove(entity);
        }

        public void Remove(List<SyncRecord> entityList)
        {
            entityList.ForEach(entity => context.SyncRecord.Attach(entity));
            context.SyncRecord.RemoveRange(entityList);
        }

        public void Update(SyncRecord entity)
        {
            context.SyncRecord.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SyncRecord> All()
        {
            return context.SyncRecord.ToList();
        }

        public AmitalContext Context
        {
            get { return context; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SyncRecord> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SyncRecord GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<SyncRecord> GetUnsyncAndMarkAsInProcess(int tenant, string item, bool allTask)
        {
            DateTime dateTimeNow = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            dateTimeNow = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, dateTimeNow.Hour, dateTimeNow.Minute, dateTimeNow.Second, dateTimeNow.Millisecond);

            IQueryable<SyncRecord> recordsQurey = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (allTask ? syncRecord.IsSync < SyncRecordStatus.SyncedAndUpdated : (syncRecord.IsSync == SyncRecordStatus.InQueue || syncRecord.IsSync == SyncRecordStatus.Synced)) &&
                (syncRecord.FileNo == item));

            logger.WriteTrace($"SyncRecord, GetUnsyncAndMarkAsInProcess query {recordsQurey}");

            List<SyncRecord> records = recordsQurey.ToList();

            foreach (SyncRecord syncRecord in records)
            {
                syncRecord.IsSync = SyncRecordStatus.Synced;
                syncRecord.SyncDT = dateTimeNow;
            }

            context.SaveChanges();

            IEnumerable<SyncRecord> q = records
                .GroupBy(record => new { record.Entname, record.KeyVal, record.TrigAction })
                .Select(group => group.OrderByDescending(record => record.CreateDate).FirstOrDefault())
                .OrderBy(record => record.CreateDate);
            List<SyncRecord> groupRecord = q.ToList();

            logger.WriteTrace($"SyncRecord, GetUnsyncAndMarkAsInProcess groupRecord {System.Text.Json.JsonSerializer.Serialize(groupRecord)}");

            return groupRecord;
        }

        public int GetFileNo(int tenant, int customsFileNo) =>
            context.CCUFILEMs.Where(file => file.TENANT == tenant && file.CUSTOMFILENO == customsFileNo)
                .Select(file => file.FILENO)
                .FirstOrDefault();

        public void UpdateSyncDate(string itemUpdate, DateTime syncDT, int tenant)
        {
            syncDT = syncDT.AddSeconds(1);

            IEnumerable<SyncRecord> query = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (syncRecord.FileNo == itemUpdate || syncRecord.Entname == itemUpdate) &&
                syncRecord.IsSync == SyncRecordStatus.Synced &&
                syncRecord.SyncDT <= syncDT);

            List<SyncRecord> records = query.ToList();

            for (int i = 0; i < records.Count; i++)
                records.ElementAt(i).IsSync = SyncRecordStatus.SyncedAndUpdated;

            context.SaveChanges();
        }

        public DateTime? GetLastSyncDate(int tenant, string fileNo)
        {
            SyncRecord record = context.SyncRecord
                .Where(syncRecord => syncRecord.Tenant == tenant && syncRecord.FileNo == fileNo)
                .OrderByDescending(x => x.CreateDate)
                .FirstOrDefault();

            if (record == null)
                return null;

            return record.SyncDT ?? DateTime.Now;
        }

        public List<SyncRecord> GetAndMarkNewSyncRecord(List<int> tenants)
        {
            IEnumerable<SyncRecord> query = context.SyncRecord
                .Where(syncRecord => syncRecord.IsSync == SyncRecordStatus.New && tenants.Contains(syncRecord.Tenant))
                .Take(1000);

            logger.WriteDebug($"SyncRecord, GetAndMarkNewSyncRecord db: {context.GetConnection().Database}");

            List<SyncRecord> records = query.ToList();

            UpdateStatus(records, SyncRecordStatus.InProcess);

            return records;
        }

        public void UpdateStatus(List<SyncRecord> records, int status)
        {
            for (int i = 0; i < records.Count; i++)
            {
                records[i].IsSync = status;
                context.SyncRecord.Attach(records[i]);
                context.SetAsModified(records[i]);
            }

            context.SaveChanges();
        }

        public List<SyncRecord> GetNeedToReturnToQueue()
        {
            int inQueue = context.SyncRecord.Count(syncRecord => syncRecord.IsSync == SyncRecordStatus.New);

            DevLog.Instance.WriteDebug($"SyncRecord, GetNeedToReturnToQueue, in queue: {inQueue}");

            if (inQueue > 200)
                return new List<SyncRecord>();

            IQueryable<SyncRecord> query = context.SyncRecord.Where(syncRecord =>
                syncRecord.IsSync > SyncRecordStatus.New && syncRecord.IsSync < SyncRecordStatus.SyncedAndUpdated &&
                syncRecord.CreateDate < DbFunctions.AddMinutes(DateTime.Now, -30)).Take(100);

            System.Diagnostics.Debug.Print($"*************** SyncRecord, GetNeedToReturnToQueue query {query}");
            List<SyncRecord> records = query.ToList();

            return records;
        }

        public string GetTimeZone()
        {
            try
            {
                string sql = "SELECT RIGHT(FORMAT(SYSDATETIMEOFFSET(), 'yyyy-MM-dd HH:mm:ss.fffffff zzz'), 6) AS CurrentTimeZoneOffset";
                return context.GetActiveDbContext().Database.SqlQuery<string>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.WriteFatal(ex, $"GetTimeZone failed: {ex.Message}");
                return null;
            }
        }
    }

    public class SyncRecordStatus
    {
        public const int New = 0;
        public const int InProcess = 1;
        public const int InQueue = 2;
        public const int Synced = 3;
        public const int SyncedAndUpdated = 4;
    }
}