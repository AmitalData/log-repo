using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffLineQueryService
    {
        public List<TariffLinePM> GetTariffLinesByTariffAndVersion(string tariffId, int version, int tenant)
        {
            List<TariffLine> tariffLines = (from a in context.TariffLines
                                            where a.TariffId == tariffId && a.Version == version && a.Tenant == tenant
                                            select a).ToList();

            List<TariffLinePM> tariffLinePMs = new List<TariffLinePM>();
            foreach (TariffLine entityPOCO in tariffLines)
            {
                TariffLinePM entityPM = new TariffLinePM();
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                tariffLinePMs.Add(entityPM);
            }
            
            return tariffLinePMs;
        }

        public override void GetComposition(EntityKeyFields entityKeys, TariffLinePM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext;
            TariffLineKeys tariffLineKeys = entityKeys as TariffLineKeys;
            
            TariffLinesContainersPriceQueryService tariffLinesContainersPriceQueryService = new TariffLinesContainersPriceQueryService(context);            
            entityPM.ContainersPrices = tariffLinesContainersPriceQueryService.GetMulti(tariffLineKeys, true);
        }
    }
}
