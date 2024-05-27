using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.Helpers;
using System.Text.Json;
using Logitude.Server.Tools.Utils;
using System.Data.SqlClient;
using System.Data;
using Logitude.Customs.BL.BL;
using System.Runtime.Remoting.Contexts;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class SyncRecordQuery
    {
        SyncRecordRepository repository;

        public SyncRecordQuery()
        {
            repository = new SyncRecordRepository();
        }

        public SyncRecordQuery(int tenant)
        {
            repository = new SyncRecordRepository(tenant);
        }

        public SyncRecordQuery(SyncRecordRepository repository)
        {
            this.repository = repository;
        }

        public List<SyncRecord> get()
        {
            List<SyncRecord> a = repository.context.SyncRecord.ToList();
            return a;
        }

        public List<EntityRecord> GetUnsyncRecordsAndMarkAsInProcess(int tenant, string fileNo)
        {
            List<SyncRecord> groupRecord = repository.GetUnsyncAndMarkAsInProcess(tenant, fileNo);

            List<EntityRecord> entityRecords = groupRecord.Select(syncRecord =>
            {
                string recordAsJson = GetRecordOfRowNeedSync(syncRecord);
                if (recordAsJson == null)
                    return null;

                return new EntityRecord
                {
                    Key = syncRecord.KeyVal,
                    TrigAction = syncRecord.TrigAction,
                    Entname = syncRecord.Entname,
                    RecordAsJson = recordAsJson,
                    UpdateDate = syncRecord.SyncDT,
                };
            }).SkipWhile(x => x == null).ToList();

            return entityRecords;
        }

        public void UpdateSyncData(int tenant, string fileNo, DateTime syncDT)
        {
            repository.UpdateSyncDate(fileNo, syncDT, tenant);
        }

        private string GetRecordOfRowNeedSync(SyncRecord syncRecord)
        {
            
            if (syncRecord == null || string.IsNullOrEmpty(syncRecord.KeyVal))
            {
                Logger.LogMe($"GetRecordOfRowNeedSync, keyVal is empty or null, id: " + syncRecord?.Id, true);
                return null;
            }

            string query = $"SELECT * FROM {syncRecord.Entname} WHERE {syncRecord.KeyVal.Replace(",", " and ")}";

            SqlConnection conn = repository.context.GetActiveDbContext().Database.Connection as SqlConnection;
            conn.Open();
            var dataReader = new SqlCommand(query, conn).ExecuteReader();
            var dt = new DataTable();
            dt.Load(dataReader);
            conn.Close();

            if (dt.Rows.Count != 1)
            {
                Logger.LogMe($"record not found once for table: {syncRecord.Entname: name} and keyVal: {syncRecord.KeyVal.Replace(",", " and ")}", true);
                return null;
            }

            DataRow record = dt.Rows[0];
            Dictionary<string, object> dic = record.Table.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => record.IsNull(c) ? null : record[c]);
            string recordAsJson = JsonSerializer.Serialize(dic);

            return recordAsJson;
        }

        public DateTime? GetLastSyncDate(int tenant, string fileNo)
        {         
            string cacheKey = "SyncRecordQuery.GetLastSyncDate." + fileNo + ";" + tenant;
            DateTime? lastSync = CacheHelper.GetFromCache(cacheKey, () => 
                repository.GetLastSyncDate(tenant, fileNo));

            return lastSync;
        }

        public List<SyncRecord> GetAndMarkNewSyncRecord() => repository.GetAndMarkNewSyncRecord();

        public void UpdateStatusInQueue(List<SyncRecord> records) => repository.UpdateStatusInQueue(records);
    }
}