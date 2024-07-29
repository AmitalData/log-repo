using Logitude.Customs.BL.BL;
using Simplog.Server.Infrastructure;
using System;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.Data.AmitalModel
{
    public static class SyncRecordCache
    {
        private static string GetLastSyncKey(string fileNo, int tenant) =>
            "SyncRecordQuery.." + fileNo + ";" + tenant;

        public static DateTime? GetLastSyncDate(string fileNo, int tenant) 
        {
            DateTime? lastSync = null;
            TryCatch(() =>
            {
                string cacheKey = GetLastSyncKey(fileNo, tenant);
                lastSync = CacheHelper.GetFromCache(cacheKey, () =>
                    new SyncRecordRepository(tenant).GetLastSyncDate(tenant, fileNo));                
            });

            return lastSync;
        }

        public static void ClearCacheLastSync(string fileNo, int tenant)
        {
            TryCatch(() =>
            {
                if (tenant == null || IsConnectedToUniFreight(tenant))
                    return;

                string cacheKey = GetLastSyncKey(fileNo, tenant);
                CacheHelper.ClearCache(cacheKey);
            });
        }

        public static void ClearCacheLastSyncByPrimaryNum(string primaryNum, int? tenant)
        {            
            TryCatch(() =>
            {
                if (tenant == null || !tenant.HasValue || primaryNum == null || !long.TryParse(primaryNum, out long lCUSTOMFILENO) || IsConnectedToUniFreight(tenant.Value))
                    return;

                var fileNo = new CCUFILEMRepository(tenant.Value).GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                if (fileNo == null)
                    return;

                ClearCacheLastSync(fileNo.Value.ToString(), tenant.Value);
            });
        }

        private static void TryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e,"Error in ClearCacheLastSync error" );
            }
        }

        private static bool IsConnectedToUniFreight(int tenant)
        {
            LogitudeCustomsSettingsM customsSettings = LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenant);
            return customsSettings.Id == null || customsSettings.IsConnectedToUniFreight;            
        }
    }
}
