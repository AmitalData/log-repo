using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CourierDeclarationQueryService
    {
        public int? GetCourierMasterMaxSequenceNumeric(string courierMasterId, int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            return courierDeclarationRepository.GetCourierMasterMaxSequenceNumeric(courierMasterId, tenant);
        }

        //public CourierDeclarationPM GetCourierDeclarationByDeclarationId(string declarationId, int Tenant)
        //{
        //    string entityKeyString = $"GetCourierDeclarationByDeclarationId({declarationId},{Tenant})";
        //    var res = CacheManager.GetOrInsertNewObject<CourierDeclarationPM>(entityKeyString, () =>
        //    {
        //        return this.GetCourierDeclarationByDeclarationIdCore(declarationId, Tenant);
        //    });
        //    return res;
        //}
        public CourierDeclarationPM GetCourierDeclarationByDeclarationId_Cache(string declarationId, int tenant)
        {
            string key = $"GetCourierDeclarationByDeclarationId_Cache({declarationId}, {tenant})";
            var res = CacheManager.GetOrInsertNewObject<CourierDeclarationPM>(key,
                () =>
                {
                    return this.GetCourierDeclarationByDeclarationId(declarationId, tenant);
                });
            return res;
        }
        public CourierDeclarationPM GetCourierDeclarationByDeclarationId(string declarationId, int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            CourierDeclarationPM courierDeclarationPM = null;

            var poco = courierDeclarationRepository.GetCourierDeclarationByDeclarationId(declarationId, tenant);
            if (poco != null)
            {
                courierDeclarationPM = this.GetEntityPM(poco, false, null);
            }
            return courierDeclarationPM;
        }

        public string GetMAWBCourierMasterByDeclarationId(string declarationId, int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            return courierDeclarationRepository.GetMAWBCourierMasterByDeclarationId(declarationId, tenant);
        }
        public string GetCourierMasterIdByDeclarationId(string declarationId,int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            return courierDeclarationRepository.GetCourierMasterIdByDeclarationId(declarationId, tenant);
        }
        public List<string> GetDeclarationIdsByCourierMasterID(string courierMasterid, int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            return courierDeclarationRepository.GetDeclarationIdsByCourierMasterID(courierMasterid, tenant);
        }
        public int CountNoOfCourierHawbwWithoutHatara(string couriermasterid, int tenant)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(context);
            return courierDeclarationRepository.CounNoOfCourierHawbwWithoutHatara(couriermasterid, tenant);
        }
        public int CountNoOfCourierHawbWithoutDelivery(string couriermasterid, int tenant)
        {
            CourierDeclarationRepository courierMasterRepository = new CourierDeclarationRepository(context);
            return courierMasterRepository.CountNoOfCourierHawbWithoutDelivery(couriermasterid, tenant);
        }
        public string GetFirstDeclarationCustomFileByCourierMasterId(string courierMasterId, int tenant)
        {
            return  repository.GetFirstDeclarationCustomFileByCourierMasterId(tenant, courierMasterId);
        }
    }
}