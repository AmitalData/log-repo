using Logitude.Customs.BL.BL;
using Simplog.Server.Infrastructure;
using System;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.Data.AmitalModel
{
    public class SyncRecordCache
    {
        public static void ClearCacheLastSync(string fileNo, int tenant)
        {
            if (tenant == null || IsConnectedToUniFreight(tenant))
                return;

            TryCatch(() =>
            {
                string cacheKey = $"SyncRecordQuery.GetLastSyncDate." + fileNo + ";" + tenant;
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
            bool isNotConnected = tenant == null || customsSettings.Id == null || !customsSettings.IsConnectedToUniFreight;
            return !isNotConnected;
        }
    }
}
