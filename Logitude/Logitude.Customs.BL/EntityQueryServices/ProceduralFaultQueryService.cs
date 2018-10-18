using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class ProceduralFaultQueryService
    {
       public override void GetComposition(EntityKeyFields entityKeys, ProceduralFaultPM entityPM)
       {
           ICustomContext context = MainContext as CustomContext;
           ProceduralFaultKeys proceduralFaultKeys = entityKeys as ProceduralFaultKeys;
           ProceduralFaultsConnEntityQueryService proceduralFaultsConnectedEntityQueryService = new ProceduralFaultsConnEntityQueryService(context);

           entityPM.ProceduralFaultsConnEntities = proceduralFaultsConnectedEntityQueryService.GetMulti(proceduralFaultKeys, true);

       }

       public string GetFaultIdByFaultNumber(string proceduralFaultNumber, int tenant)
       {
           if (string.IsNullOrEmpty(proceduralFaultNumber)) return "";
           return repository.GetFaultIdByFaultNumber(proceduralFaultNumber, tenant);
       }
    }
}
