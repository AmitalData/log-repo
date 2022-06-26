using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.BL;
using Logitude.Customs.Def.Contracts;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ItemGovernmentProcedureTypeQueryService : ICanGetAllClosedTable<ItemGovernmentProcedureTypePM>
    {
        public List<ItemGovernmentProcedureTypePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            //var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return pms;
        }

        public List<ItemGovernmentProcedureTypePM> GetAllFromCache() => CacheHelper.GetFromCache("ItemGovernmentProcedureTypePMGetAll", GetAll);
    }
}
