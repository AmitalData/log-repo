using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CourierPendingReasonUpdateService
    {
        protected override void OnCreating(CourierPendingReasonPM entityPM, EntityPM entityParentPM)
        {
            ValidateEntity(entityPM);

            entityPM.Id = entityPM.Code;// IdCounter.GetNumber("Customs.CourierPendingReason", entityPM.Tenant);
        }

        protected override void OnUpdating(CourierPendingReasonPM entityPM, CourierPendingReason entityPOCO)
        {
            base.OnUpdating(entityPM, entityPOCO);
        }

        internal void ValidateEntity(CourierPendingReasonPM entityPM)
        {
            if (IsPendingCodeExisit(entityPM.Code, entityPM.Tenant))
            {
                throw new Exception("קיים Pending עם אותו הקוד"); // There is a pending with the same code
            }
        }

        bool IsPendingCodeExisit(string code, int tenant)
        {
            CourierPendingReasonRepository repo = new CourierPendingReasonRepository(tenant);
            var exist = repo.GetByCode(code, tenant);

            return ((exist != null) ? true : false);
        }
    }
}
