using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsBookQueryService
    {
        public CustomsBookPM GetCustomsBookByTenant(int tenant)
        {
            var pocos = repository.GetCustomsBookByTenant(tenant);
            if (pocos == null) return null;
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).FirstOrDefault();
            return pms;
        }

        public CustomsBookPM GetCustomsBookData()
        {
            var pocos = repository.GetCustomsBookData();
            if (pocos == null) return null;
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).FirstOrDefault();
            return pms;
        }
    }
}
