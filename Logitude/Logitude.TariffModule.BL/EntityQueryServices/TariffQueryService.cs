using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Linq;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffQueryService
   {
        public override void GetComposition(EntityKeyFields entityKeys, TariffPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext; 
            TariffKeys tariffKeys = entityKeys as TariffKeys;
            
            TariffVersionQueryService tariffVersionQueryService = new TariffVersionQueryService(context);
            entityPM.TariffVersions = tariffVersionQueryService.GetMulti(tariffKeys, true);
        }

        public TariffsSummary GetCount(int tenant)
        {
            TariffsSummary tariffsSummary = new TariffsSummary() { Id = tenant };
            tariffsSummary.AirFreightCount=this.repository.GetAll(tenant).Where(p => p.TypeCode == "AFC").Count();
            tariffsSummary.AirSurchargeCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ASC").Count();
            return tariffsSummary;
        }
   }   
}
	 