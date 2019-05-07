 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;
using Simplog.Server.Infrastructure;
using Logitude.TariffModule.BL.DataContracts;

namespace Logitude.TariffModule.BL.EntityQueryServices
{ 
   public partial class TariffQueryService
   {
        public override void GetComposition(EntityKeyFields entityKeys, TariffPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext; 
            TariffKeys tariffKeys = entityKeys as TariffKeys;

            TariffLineQueryService tariffLineQueryService = new TariffLineQueryService(context);
            entityPM.TariffLines = tariffLineQueryService.GetMulti(tariffKeys, true);

            TariffVersionQueryService tariffVersionQueryService = new TariffVersionQueryService(context);
            entityPM.TariffVersions = tariffVersionQueryService.GetMulti(tariffKeys, true);
        }

        public TariffsSummary GetCount(int tenant)
        {
            TariffsSummary tariffsSummary = new TariffsSummary() { Id = tenant };

            tariffsSummary.AirFreightCount=this.repository.GetAll(tenant).Where(p => p.TypeCode == "AFC").Count();


            return tariffsSummary;

        }


     
	 
   }
   
}
	 