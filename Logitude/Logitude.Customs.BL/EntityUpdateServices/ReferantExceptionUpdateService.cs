using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ReferantExceptionUpdateService
    {

        protected override void OnCreating(ReferantExceptionPM entityPM, EntityPM entityParentPM)
        {

            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void OnUpdating(ReferantExceptionPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            if (entityPM != null)
            {
                ExceptionReasonQueryService exceptionReasonQueryService = new ExceptionReasonQueryService(entityPM.Tenant);
                ExceptionReasonPM exceptionReasonPm = exceptionReasonQueryService.GetSingle(entityPM.ExceptionReasonsCode, false, false);
                if (exceptionReasonPm.UnifreightStatusCode != null)
                {
                    if (entityPM.Status == "A")
                    {
                        UpdateUnifreight(entityPM, exceptionReasonPm);
                    }
                }
            }
            base.OnUpdating(entityPM);
        }
    }
}
