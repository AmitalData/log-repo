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
    public partial class ReferantExceptionQueryService : EntityQueryService<ReferantException, ReferantExceptionKeys, ReferantExceptionPM, object, ReferantExceptionKeys>
    {
        public List<ReferantExceptionPM> GetByDecId(string declarationId)
        {
            List<ReferantException> ReferantException = repository.GetByDecId(declarationId);
            List<ReferantExceptionPM> ReferantExceptionPMList = new List<ReferantExceptionPM>();

            foreach (ReferantException item in ReferantException)
            {
                ReferantExceptionPM ReferantExceptionPM = GetEntityPM(item);
                ReferantExceptionPMList.Add(ReferantExceptionPM);
            }
            return ReferantExceptionPMList;
        }
    }
}
