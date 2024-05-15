using Logitude.Customs.BL.BL;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.Data.AmitalModel
{
    public class SyncRecordCache
    {
        public static void ClearCacheLasySync(string fileNo, int tenant)
        {
            string cacheKey = $"SyncRecordQuery.GetLastSyncDate." + fileNo + ";" + tenant;
            CacheHelper.ClearCache(cacheKey);
        }

        public static void ClearCacheLasySyncByPrimaryNum(string primaryNum, int? tenant)
        {
            if (!tenant.HasValue || primaryNum == null || !long.TryParse(primaryNum, out long lCUSTOMFILENO))
                return;

            var fileNo = new CCUFILEMRepository(tenant.Value).GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
            if (fileNo == null)
                return;

            ClearCacheLasySync(fileNo.Value.ToString(), tenant.Value);
        }
    }
}
