using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using System;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class SyncRecordRepository : IRepository<SyncRecord>
    {
        AmitalContext context;

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

        public List<SyncRecord> GetUnsyncAndMarkAsInProcess(int tenant, string item)
        {
            DateTime date = DateTime.Now;

            IQueryable<SyncRecord> records = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (syncRecord.IsSync == SyncRecordStatus.InQueue || syncRecord.IsSync == SyncRecordStatus.Synced) &&
                (syncRecord.FileNo == item || syncRecord.Entname == item)
            );

            foreach (SyncRecord syncRecord in records)
            {
                syncRecord.IsSync = SyncRecordStatus.Synced;
                syncRecord.SyncDT = date;
            }

            context.SaveChanges();

            List<SyncRecord> groupRecord =
                records.GroupBy(record => new { record.Entname, record.KeyVal, record.TrigAction })
                .Select(group => group.FirstOrDefault())
                .ToList();
            
            return groupRecord;
        }

        public void UpdateSyncDate(string itemUpdate, DateTime syncDT, int tenant)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            syncDT = syncDT.AddSeconds(1);

            IEnumerable<SyncRecord> records = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (syncRecord.FileNo == itemUpdate || syncRecord.Entname == itemUpdate) &&
                syncRecord.IsSync == SyncRecordStatus.Synced &&
                syncRecord.SyncDT <= syncDT &&
                syncRecord.CreateDate > yesterday
            );

            for (int i = 0; i < records.Count(); i++)
                records.ElementAt(i).IsSync = SyncRecordStatus.SyncedAndUpdated;

            context.SaveChanges();
        }

        public DateTime? GetLastSyncDate(int tenant, string fileNo) =>
            context.SyncRecord
                .Where(syncRecord => syncRecord.Tenant == tenant && syncRecord.FileNo == fileNo)
                .Max(syncRecord => syncRecord.SyncDT);

        public List<SyncRecord> GetAndMarkNewSyncRecord()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);

            IEnumerable<SyncRecord> records = context.SyncRecord.Where(syncRecord =>                                
                syncRecord.IsSync == SyncRecordStatus.New && syncRecord.CreateDate > yesterday && syncRecord.FileNo == "0");

            int recordsCounts = records.Count();
            for (int i = 0; i < recordsCounts; i++)
                records.ElementAt(i).IsSync = SyncRecordStatus.InProcess;

            return records.ToList();
        }

        public void UpdateStatusInQueue(List<SyncRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                records[i].IsSync = SyncRecordStatus.InQueue;                
                context.SyncRecord.Attach(records[i]);
                context.SetAsModified(records[i]);
            }

            context.SaveChanges();
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