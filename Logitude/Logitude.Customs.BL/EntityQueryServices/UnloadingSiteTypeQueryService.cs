using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class UnloadingSiteTypeQueryService : ICanGetAllClosedTable<UnloadingSiteTypePM>
    {
        public IQueryable<UnloadingSiteTypePM> GetUnloadingSiteTypePMs()
        {
            IQueryable<UnloadingSiteType> pocos = repository.GetAll();
            IQueryable<UnloadingSiteTypePM> query = from a in pocos
                                                 select new UnloadingSiteTypePM
                                                 {
                                                     Code = a.Code,
                                                     EnglishName = a.EnglishName,
                                                     LocalName = a.LocalName,
                                                     Inactive = a.Inactive,
                                                 };
            return query;
        }
        public List<UnloadingSiteTypePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
    }
}
