using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffLineUpdateService
    {
        protected override void OnCreating(TariffLinePM entityPM, TariffVersionPM entityParentPM)
        {
            entityPM.TariffId = entityParentPM.TariffId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                int index = 0;
                entityPM.Id = IdCounter.GetNumber("TariffLine", entityPM.Tenant);
                TariffLineRepository tariffLineRepository = new TariffLineRepository(entityPM.Tenant);
                //List<TariffLine> lines= tariffLineRepository.GetTariffLinesByTariff(entityPM.TariffId, entityPM.Tenant);
                //if (lines.Count > 0)
                //{
                //    index = lines.Max(p => p.Index)+1;
                //}
                //EntityPM.Index = index;
            }

        }
    }
}
