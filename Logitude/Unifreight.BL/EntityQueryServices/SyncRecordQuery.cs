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
        DevLog logger = DevLog.Instance;

        public SyncRecordQuery()
        {
            int tenant = CacheHelper.GetFromCache("SyncRecordQuery_tenant", () =>
            {
                CustomsSetting settings = new CustomsSettingRepository(0).GetRealAll().FirstOrDefault(x => x.UnfConnectionString != null);
                if (settings == null)
                    throw new Exception("No MSync connection string found");

                logger.WriteDebug($"for up SyncRecordQuery Get first tenant from custsom.customssetting that have value in field UnfConnectionString, tenant: {settings.Tenant}, UnfConnectionString: {settings.UnfConnectionString}");
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

        public List<EntityRecord> GetUnsyncRecordsAndMarkAsInProcess(int tenant, string item, int? customsFileNo, bool allTask)
        {
            List<SyncRecord> notExistsRecord = new List<SyncRecord>();
            
            if(customsFileNo.HasValue)
            {
                item = repository.GetFileNo(tenant, customsFileNo.Value).ToString();
                if(string.IsNullOrEmpty(item))
                    throw new Exception($"fileNo for customsFileNo {customsFileNo} and tenant {tenant} not found");
            }

            List<SyncRecord> groupRecord = repository.GetUnsyncAndMarkAsInProcess(tenant, item, allTask);
            AddGGGQC(groupRecord);

            string offset = GetTimeZone(tenant);
            List<EntityRecord> entityRecords = groupRecord.Select(syncRecord =>
            {
                string recordAsJson = GetRecordOfRowNeedSync(syncRecord);
                if (recordAsJson == null)
                {
                    if(syncRecord.Entname == "GGGQC")
                        return null;
                    else
                        notExistsRecord.Add(syncRecord);
                }

                return new EntityRecord
                {
                    Key = syncRecord.KeyVal,
                    TrigAction = syncRecord.TrigAction,
                    Entname = syncRecord.Entname,
                    RecordAsJson = recordAsJson,
                    UpdateDate = syncRecord.SyncDT,
                    CraeteDate = syncRecord.CreateDate,
                    CreateDate = syncRecord.CreateDate,
                    DbOffset = offset

                };
            }).Where(x => x != null).ToList();

            repository.UpdateStatus(notExistsRecord, SyncRecordStatus.SyncedAndUpdated);

            return entityRecords;
        }

        private static void AddGGGQC(List<SyncRecord> syncRecords)
        {
            try
            {
                List<SyncRecord> gggqRecords = syncRecords.Where(record => record.Entname.ToLower() == "gggq").ToList();

                gggqRecords.ForEach(gggqRecord =>
                    syncRecords.Add(new SyncRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        FileNo = gggqRecord.FileNo,
                        KeyVal = gggqRecord.KeyVal,
                        Tenant = gggqRecord.Tenant,
                        IsSync = gggqRecord.IsSync,
                        CreateDate = gggqRecord.CreateDate,
                        Entname = "GGGQC",
                        SyncDT = gggqRecord.SyncDT,
                        TrigAction = gggqRecord.TrigAction,
                    }));
            }
            catch (Exception e)
            {
                DevLog.Instance.WriteFatal(e, "error on SendToUnifreightQueue when add GGGQC table");
            }
        }

        public void UpdateSyncData(int tenant, string itemUpdate, DateTime syncDT)
        {
            repository.UpdateSyncDate(itemUpdate, syncDT, tenant);
            SyncRecordCache.ClearCacheLastSync(itemUpdate, tenant);
        }

        public string GetRecordOfRowNeedSync(SyncRecord syncRecord)
        {
            if (syncRecord == null || string.IsNullOrEmpty(syncRecord.KeyVal) || syncRecord.TrigAction == "D")
            {
                DevLog.Instance.WriteInfo($"GetRecordOfRowNeedSync, keyVal is empty or null or is delete, id: {syncRecord?.Id}, trigAction: {syncRecord?.TrigAction}");
                return "[]";
            }

            string query = $"SELECT * FROM {syncRecord.Entname}";

            if (syncRecord.Entname.ToLower() == "ccumshgr")
                query += " WHERE FILE_NO = '" + syncRecord.FileNo + "'";
            else
                query += " WHERE " + syncRecord.KeyVal.Replace(",", " and ").Replace("=NULL", " is null ");

            SqlConnection conn = (repository.Context as IContext).GetActiveDbContext().Database.Connection as SqlConnection;
            conn.Open();
            SqlDataReader dataReader = new SqlCommand(query, conn).ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            conn.Close();

            if (dt.Rows.Count != 1 && syncRecord.Entname.ToLower() != "ccumshgr")
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

        public List<SyncRecord> GetAndMarkNewSyncRecord(List<int> tenants) => repository.GetAndMarkNewSyncRecord(tenants);

        public void UpdateStatus(List<SyncRecord> records, int status) => repository.UpdateStatus(records, status);

        public void Update(List<SyncRecord> records) => repository.Update(records);

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

        public List<SyncRecord> GetNeedToReturnToQueue() => repository.GetNeedToReturnToQueue();

        public string GetTimeZone(int tenant) => 
            CacheHelper.GetFromCache("SyncRecord_timeZone_" + tenant,() => repository.GetTimeZone());
    }
}