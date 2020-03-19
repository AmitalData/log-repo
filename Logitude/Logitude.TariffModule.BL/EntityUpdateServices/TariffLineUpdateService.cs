using Logitude.Server.Tools.Counters;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Server.Infrastructure;
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
                entityPM.Id = IdCounter.GetNumber("TariffLine", entityPM.Tenant);                
            }
        }

        protected override void OnUpdating(TariffLinePM entityPM, TariffLine entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {


            }
        }

        protected override void UpdateComposition(TariffLinePM entityPM)
        {
            TariffLinesContainersPriceUpdateService tariffLinesContainersPriceUpdateService = new TariffLinesContainersPriceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLinesContainersPriceUpdateService.UpdateMulti(entityPM.ContainersPrices, entityPM.DeletedContainersPrices, entityPM, false);
        }
    }
}
