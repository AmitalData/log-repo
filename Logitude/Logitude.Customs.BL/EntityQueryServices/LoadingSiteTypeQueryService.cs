
using Logitude.Customs.BL.BL;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class LoadingSiteTypeQueryService : ICanGetAllClosedTable<LoadingSiteTypePM>
    {
        public List<LoadingSiteTypePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            //var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return pms;
        }

        public List<LoadingSiteTypePM> GetAllFromCache() => CacheHelper.GetFromCache("LoadingSiteTypePMGetAll", GetAll);
    }
}
