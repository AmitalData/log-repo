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
using System.Text.Encodings.Web;
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace Unifreight.BL.EntityQueryServices
{
    public class SyncRecordQuery
    {
        SyncRecordRepository repository;
        static DevLog logger = DevLog.Instance;

        public SyncRecordQuery(int tenant)
        {
            repository = new SyncRecordRepository(tenant);
        }

        public SyncRecordQuery(SyncRecordRepository repository)
        {
            this.repository = repository;
        }

        public static int GetTenantOfSyncRecord()
        {
            int tenant = CacheHelper.GetFromCache("SyncRecordQuery_tenant", () =>
            {
                CustomsSetting settings = new CustomsSettingRepository(0).GetRealAll().FirstOrDefault(x => x.UnfConnectionString != null);
                if (settings == null)
                    throw new Exception("No MSync connection string found");

                logger.WriteDebug($"for up SyncRecordQuery Get first tenant from custsom.customssetting that have value in field UnfConnectionString, tenant: {settings.Tenant}, UnfConnectionString: {settings.UnfConnectionString}");
                return settings.Tenant;
            });

            return tenant;
        }

        public List<SyncRecord> get()
        {
            List<SyncRecord> a = repository.Context.SyncRecord.ToList();
            return a;
        }

        public List<EntityRecord> GetUnsyncRecordsAndMarkAsInProcess(int tenant, string item, int? customsFileNo, bool allTask)
        {
            List<SyncRecord> notExistsRecord = new List<SyncRecord>();

            if (customsFileNo.HasValue)
            {
                item = repository.GetFileNo(tenant, customsFileNo.Value).ToString();
                if (string.IsNullOrEmpty(item))
                    throw new Exception($"fileNo for customsFileNo {customsFileNo} and tenant {tenant} not found");
            }

            List<SyncRecord> groupRecord = repository.GetUnsyncAndMarkAsInProcess(tenant, item, allTask);

            AddGGGQC(groupRecord);

            string offset = GetTimeZone(tenant);
            List<IGrouping<string, SyncRecord>> groupedByEntname = groupRecord.GroupBy(r => r.Entname).ToList();

            List<EntityRecord> entityRecords = new List<EntityRecord>();
            List<Task> entityRecordTasks = new List<Task>();
            int tasksNumber;
            if (!int.TryParse(Environment.GetEnvironmentVariable("tasksNumberForSync") ?? "", out tasksNumber))
                tasksNumber = 20;
            
            SemaphoreSlim throttler = new SemaphoreSlim(tasksNumber);

            groupedByEntname.ForEach(group =>
            {
                throttler.Wait();
                Task task = Task.Run(() =>
                {
                    try
                    {
                        List<SyncRecord> recordsInGroup = group.ToList();
                        string tableName = group.Key;
                        List<SyncRecordWithJson> syncRecordsWithJson = new SyncRecordQuery(tenant).GetRecordOfRowNeedSync(recordsInGroup, tableName);

                        List<EntityRecord> entityRecordsInGroup = syncRecordsWithJson.Select(syncRecordWithJson =>
                        {
                            string recordAsJson = syncRecordWithJson.RecordAsJson;
                            SyncRecord syncRecord = syncRecordWithJson.SyncRecord;

                            if ((recordAsJson == null || recordAsJson == "[]") && tableName == "GGGQC")
                                return null;
                            else if (recordAsJson == null)
                                notExistsRecord.Add(syncRecord);

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
                        entityRecords.AddRange(entityRecordsInGroup);
                    }
                    catch (Exception ex)
                    {
                        logger.WriteFatal(ex, $"Error processing sync records of entname: {group?.Key}");                        
                    }
                    finally
                    {
                        throttler.Release();
                    }
                });
                entityRecordTasks.Add(task);

            });
            Task.WaitAll(entityRecordTasks.ToArray());
            throttler.Dispose();

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

        public List<SyncRecordWithJson> GetRecordOfRowNeedSync(List<SyncRecord> syncRecords, string tableName)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append($"SELECT * FROM {tableName} ");
            bool firstCondition = true;
            List<SyncRecordWithJson> syncRecordsWithJson = syncRecords.Select(sr => new SyncRecordWithJson { SyncRecord = sr, RecordAsJson = "[]" }).ToList();

            syncRecordsWithJson.ForEach(syncRecordssWithJson =>
            {
                SyncRecord syncRecord = syncRecordssWithJson.SyncRecord;
                if (syncRecord == null || string.IsNullOrEmpty(syncRecord.KeyVal) || syncRecord.TrigAction == "D")
                    return;

                queryBuilder.Append(firstCondition ? " WHERE " : " OR ");
                if (firstCondition)
                    firstCondition = false;

                queryBuilder.Append(syncRecord.Entname.ToLower() == "ccumshgr" ?
                    " ( FILE_NO = '" + syncRecord.FileNo + "') " :
                    " (" + syncRecord.KeyVal.Replace(",", " and ").Replace("=NULL", " is null ") + ") ");
            });

            if (firstCondition)
            {
                DevLog.Instance.WriteInfo($"GetRecordOfRowNeedSync, keyVal is empty or null or is delete, id: {syncRecordsWithJson.FirstOrDefault().SyncRecord?.Id}, trigAction: {syncRecordsWithJson.FirstOrDefault().SyncRecord?.TrigAction}");
                return syncRecordsWithJson;
            }

            string query = queryBuilder.ToString();

            SqlConnection conn = (repository.Context as IContext).GetActiveDbContext().Database.Connection as SqlConnection;
            conn.Open();
            SqlDataReader dataReader = new SqlCommand(query, conn).ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            conn.Close();

            string[] columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            IEnumerable<Dictionary<string, string>> data = dt.Rows.Cast<DataRow>()
                    .Select(dr => columns.ToDictionary(c => c, c =>
                     dr[c] is DateTime datetime ?
                            datetime.ToString("d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) :
                            dr[c]?.ToString()
                    ));

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

            int theardNumber;
            if (!int.TryParse(Environment.GetEnvironmentVariable("theardNumberForSync") ?? "", out theardNumber))
                theardNumber = 20;
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = theardNumber };

            Parallel.ForEach(
                syncRecordsWithJson
                    .Where(sr => !(sr.SyncRecord == null || string.IsNullOrEmpty(sr.SyncRecord.KeyVal) || sr.SyncRecord.TrigAction == "D"))
                    .ToList(),
                options,
                sr =>
                {
                    Dictionary<string, string> conditions = new Dictionary<string, string>();
                    sr.SyncRecord.KeyVal.Split(',').ToList().ForEach(str =>
                    {
                        string[] split = str.Split('=');
                        conditions.Add(split[0], split[1].Replace("'", ""));
                    });
                    sr.RecordAsJson = JsonSerializer.Serialize(
                        data.Where(row =>
                            conditions.All(condition => row[condition.Key] == condition.Value)),
                        jsonOptions
                    );
                }
            );

            return syncRecordsWithJson;
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
            CacheHelper.GetFromCache("SyncRecord_timeZone_" + tenant, () => repository.GetTimeZone());

        public class SyncRecordWithJson
        {
            public SyncRecord SyncRecord { get; set; }
            public string RecordAsJson { get; set; }
        }
    }
}