
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class GovernmentProcTypeTenantQueryService
    {
        public List<GovernmentProcTypeTenantPM> GetByTenat(int tenat, string code)
        {
            var pocos = repository.GetAll(tenat,code).ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            //var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return pms;
        }
    }
}
