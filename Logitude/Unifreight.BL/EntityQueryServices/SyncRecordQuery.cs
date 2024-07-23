using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Data.SqlClient;
using System.Data;
using Logitude.Customs.BL.BL;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.Models;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class SyncRecordQuery
    {
        SyncRecordRepository repository;

        public SyncRecordQuery()
        {
            int tenant = CacheHelper.GetFromCache("SyncRecordQuery_tenant", () =>
            {
                CustomsSetting settings = new CustomsSettingRepository(0).GetRealAll().FirstOrDefault(x => x.UnfConnectionString != null);
                if (settings == null)
                    throw new Exception("No MSync connection string found");

                return settings.Tenant;
            });
            repository = new SyncRecordRepository(tenant);
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
            List<SyncRecord> a = repository.Context.SyncRecord.ToList();
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
                    CraeteDate = syncRecord.CreateDate                
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
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"GetRecordOfRowNeedSync, keyVal is empty or null, id: " + syncRecord?.Id);
                return null;
            }

            string query = $"SELECT * FROM {syncRecord.Entname} WHERE {syncRecord.KeyVal.Replace(",", " and ")}";

            SqlConnection conn = repository.Context.GetActiveDbContext().Database.Connection as SqlConnection;
            conn.Open();
            var dataReader = new SqlCommand(query, conn).ExecuteReader();
            var dt = new DataTable();
            dt.Load(dataReader);
            conn.Close();

            if (dt.Rows.Count != 1)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"record not found once for table: {syncRecord.Entname: name} and keyVal: {syncRecord.KeyVal.Replace(",", " and ")}");
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