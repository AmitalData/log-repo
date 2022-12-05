using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SchedulerParamQueryService
    {
        public List<SchedulerParam> GetAllByProcedureCode(int tenant, string schedulerProcedureCode)
        {
            return repository.GetAllByProcedureCode( tenant,schedulerProcedureCode);
        }

    }
}