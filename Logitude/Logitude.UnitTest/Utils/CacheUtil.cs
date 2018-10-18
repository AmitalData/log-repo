#if false



using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.UnitTest.Utils
{
    public class CacheUtil
    {
        private static CacheUtil _Instance;

        private CacheUtil() { CacheManager.CacheWrapper = new MockCacheWrapper(); }
        public static CacheUtil Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CacheUtil();
                }
                return _Instance;
            }
        }
        public void Clear()
        {

            CacheManager.CacheWrapper = new MockCacheWrapper();
        }


        public void InitCacheAccountingVat(int _Tenant, DateTime _myDate,int Percentage)
        {

            string key = "ResolveAccountingVatList," + _Tenant.ToString();
            var percentageList = CacheManager.GetOrInsertNewObject<List<VatTypePercentagePM>>(key, () =>
            {

                var percentagesQuery = new List<VatTypePercentagePM>()
                {
                    new  VatTypePercentagePM(){ Tenant=_Tenant , FromDate=_myDate.AddDays(-1)  , Percentage=Percentage}
                     
                };
                var percentagePM = percentagesQuery
                    //.Where(rec => DbFunctions.TruncateTime(rec.FromDate) <= documentDate)
                    .OrderByDescending(d => d.FromDate).ToList();
                return percentagePM;
            }, true);

        }

        public void InitCacheFullAccountingSettingPM(FullAccountingSettingPM myFullAccountingSettingPM )
        {
            
            string key = "FullAccountingSettingPM," + myFullAccountingSettingPM.Tenant.ToString();

            var val = CacheManager.GetOrInsertNewObject<FullAccountingSettingPM>(key, () =>
            {

                var fullPm = myFullAccountingSettingPM;
                //new FullAccountingSettingPM() { Tenant = 1, VATOutputGLAccountId = "1-30" };
                return fullPm;
            });
        }

        public void InitCacheAccountingCurrencyId(int _Tenant, string CurrencyId )
        {

            string key = "ResolveAccountingCurrencyId," + _Tenant.ToString();

            string val = CacheManager.GetOrInsertNewObject<string>(key, () =>
            {

                return CurrencyId;
            },false);

        }
    }
}

#endif