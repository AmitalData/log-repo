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
    public partial class ExceptionReasonQueryService : EntityQueryService<ExceptionReason, ExceptionReasonKeys, ExceptionReasonPM, object, ExceptionReasonKeys>
    {
        public List<ExceptionReasonPM> GetExceptionReasonByUnifreightStatus(string unifreightStatusCode, int tenant)
        {
            List<ExceptionReason> exceptionReason = repository.GetExceptionReasonByUnifreightStatus(unifreightStatusCode);
            List<ExceptionReasonPM> exceptionReasonPMList = new List<ExceptionReasonPM>();

            foreach (ExceptionReason item in exceptionReason)
            {
                ExceptionReasonPM exceptionReasonPM = GetEntityPM(item);
                exceptionReasonPMList.Add(exceptionReasonPM);
            }

            return exceptionReasonPMList;
        }
    }
}
