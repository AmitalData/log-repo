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
using NetCommonHelper.Logger;
using Simplog.Data.CommonDataModel;
using System.Text.Encodings.Web;
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel;

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

        public List<EntityRecord> GetUnsyncRecordsAndMarkAsInProcess(int tenant, string item)
        {
            List<SyncRecord> groupRecord = repository.GetUnsyncAndMarkAsInProcess(tenant, item);

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
            }).Where(x => x != null).ToList();

            return entityRecords;
        }

        public void UpdateSyncData(int tenant, string itemUpdate, DateTime syncDT)
        {
            repository.UpdateSyncDate(itemUpdate, syncDT, tenant);
            SyncRecordCache.ClearCacheLastSync(itemUpdate, tenant);
        }

        private string GetRecordOfRowNeedSync(SyncRecord syncRecord)
        {
            if (syncRecord == null || string.IsNullOrEmpty(syncRecord.KeyVal))
            {
                DevLog.Instance.WriteInfo($"GetRecordOfRowNeedSync, keyVal is empty or null, id: " + syncRecord?.Id);
                return null;
            }

            bool isCloseTeable = syncRecord.KeyVal == "ALL";

            string query = $"SELECT * FROM {syncRecord.Entname}"; 

            if(syncRecord.Entname.ToLower() == "ccumshgr")
                query += " WHERE FILE_NO = '" + syncRecord.FileNo + "'";
            else if (!isCloseTeable)
                query += " WHERE " + syncRecord.KeyVal.Replace(",", " and ");

            SqlConnection conn = 
                (isCloseTeable ? CommonDataContext.GetContext(syncRecord.Tenant) as IContext : repository.Context as IContext)
                .GetActiveDbContext().Database.Connection as SqlConnection;
            conn.Open();
            SqlDataReader dataReader = new SqlCommand(query, conn).ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            conn.Close();

            if (dt.Rows.Count != 1 && !isCloseTeable && syncRecord.Entname.ToLower() != "ccumshgr")
            {
                DevLog.Instance.WriteError($"record not found once for table: {syncRecord.Entname: name} and keyVal: {syncRecord.KeyVal.Replace(",", " and ")}");
                return null;
            }

            string[] columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            IEnumerable<Dictionary<string, string>> data = dt.Rows.Cast<DataRow>()
                    .Select(dr => columns.ToDictionary(c => c, c => dr[c].ToString()));

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string recordAsJson = JsonSerializer.Serialize(data, jsonOptions);

            return recordAsJson;
        }

        public DateTime? GetLastSyncDate(int tenant, string fileNo) => SyncRecordCache.GetLastSyncDate(fileNo, tenant);         

        public List<SyncRecord> GetAndMarkNewSyncRecord() => repository.GetAndMarkNewSyncRecord();

        public void UpdateStatusInQueue(List<SyncRecord> records) => repository.UpdateStatusInQueue(records);

        public void Add(List<SyncRecord> records)
        {
            repository.Add(records);
            repository.SubmitChanges();
        }

        public void Remove(List<SyncRecord> records)
        {
            repository.Remove(records);
            repository.SubmitChanges();
        }
    }
}