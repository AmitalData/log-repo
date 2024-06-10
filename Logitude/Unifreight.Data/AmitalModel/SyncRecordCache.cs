using Logitude.Customs.BL.BL;
using Logitude.Server.Tools.Utils;
using System;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.Data.AmitalModel
{
    public class SyncRecordCache
    {
        public static void ClearCacheLastSync(string fileNo, int tenant)
        {
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
                if (tenant == null || !tenant.HasValue || primaryNum == null || !long.TryParse(primaryNum, out long lCUSTOMFILENO))
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
                Logger.LogError(e, "Error in ClearCacheLastSync ");
            }
        }
    }
}
