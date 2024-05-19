using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SyncRecordRepository : IRepository<SyncRecord>
    {
        ICommonDataContext commonDataContext;

        public SyncRecordRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public SyncRecordRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public SyncRecordRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public void Add(SyncRecord entity)
        {
            context.SyncRecord.Add(entity);
        }

        public void Remove(SyncRecord entity)
        {
            context.SyncRecord.Attach(entity);
            context.SyncRecord.Remove(entity);
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

        public ICommonDataContext context
        {
            get { return commonDataContext; }
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

        public List<SyncRecord> GetUnsyncAndMarkAsInProcess(int tenant, string fileNo)
        {
            IEnumerable<SyncRecord> records = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                (syncRecord.IsSync == 2 || syncRecord.IsSync == 3) &&
                syncRecord.FileNo == fileNo
            );

            foreach (SyncRecord syncRecord in records)
            {
                syncRecord.IsSync = 3;
                syncRecord.SyncDT = DateTime.Now;
            }

            context.SaveChanges();

            List<SyncRecord> groupRecord = records.GroupBy(record => new { record.Entname, record.KeyVal, record.TrigAction }).Select(group =>
            {
                var record = group.First();
                return record;
            }).ToList();

            return groupRecord;
        }

        public void UpdateSyncDate(string fileNo, DateTime syncDT, int tenant)
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);

            IEnumerable<SyncRecord> records = context.SyncRecord.Where(syncRecord =>
                syncRecord.Tenant == tenant &&
                syncRecord.FileNo == fileNo &&
                syncRecord.IsSync == 3 &&
                syncRecord.SyncDT <= syncDT &&
                syncRecord.CreateDate > yesterday
            );

            for (int i = 0; i < records.Count(); i++)
                records.ElementAt(i).IsSync = 4;

            //IEnumerable<SyncRecord> records2 = context.SyncRecord.Where(syncRecord =>
            //    syncRecord.Tenant == tenant &&
            //    syncRecord.FileNo == fileNo &&                
            //    syncRecord.SyncDT == syncDT &&
            //    syncRecord.CreateDate > yesterday
            //);

            //for (int i = 0; i < records2.Count(); i++)
            //    records2.ElementAt(i).IsSync = 2;

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
                syncRecord.IsSync == 0 && syncRecord.CreateDate > yesterday);

            for (int i = 0; i < records.Count(); i++)
                records.ElementAt(i).IsSync = 1;

            return records.ToList();
        }

        public void UpdateStatusInQueue(List<SyncRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                records[i].IsSync = 2;                
                context.SyncRecord.Attach(records[i]);
                context.SetAsModified(records[i]);
            }

            context.SaveChanges();
        }
    }
}