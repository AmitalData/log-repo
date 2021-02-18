using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CourierPendingReasonQueryService : EntityQueryService<CourierPendingReason, CourierPendingReasonKeys, CourierPendingReasonPM, object, CourierPendingReasonKeys>
    {
        public List<CourierPendingReasonPM> GetCourierPendingReasonByUnifreightStatus(string unifreightStatusCode, int tenant)
        {
            List<CourierPendingReason> courierPendingReason = repository.GetCourierPendingReasonByUnifreightStatus(unifreightStatusCode);
            List<CourierPendingReasonPM> courierPendingReasonPMList = new List<CourierPendingReasonPM>();

            foreach (CourierPendingReason item in courierPendingReason)
            {
                CourierPendingReasonPM courierPendingReasonPM = GetEntityPM(item);
                courierPendingReasonPMList.Add(courierPendingReasonPM);
            }

            return courierPendingReasonPMList;
        }

        public CourierPendingReasonPM GetSingleCourierPendingReasonByCode(string code, int tenant)
        {
            CourierPendingReason  courierPendingReason = repository.GetByCode(code, tenant);
            return GetEntityPM(courierPendingReason);
        }
        
    }
}
