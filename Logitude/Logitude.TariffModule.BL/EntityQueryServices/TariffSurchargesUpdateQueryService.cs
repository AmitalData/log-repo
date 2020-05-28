using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffSurchargesUpdateQueryService
    {
        public List<TariffSurchargesUpdatePM> GetTariffsLogsByTariffId(string tariffId,int version,  int tenant)
        {
            List<TariffSurchargesUpdate> logs = (from a in context.TariffSurchargesUpdates
                                            where a.TariffId == tariffId  && a.Tenant == tenant && a.Version == version
                                            select a).ToList();

            List<TariffSurchargesUpdatePM> logsPMs = new List<TariffSurchargesUpdatePM>();
            foreach (TariffSurchargesUpdate entityPOCO in logs)
            {
                TariffSurchargesUpdatePM entityPM = new TariffSurchargesUpdatePM();
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                logsPMs.Add(entityPM);
            }

            return logsPMs;
        }
    }
}
