using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffVersionQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TariffVersionPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext;
            TariffVersionKeys tariffVersionKeys = entityKeys as TariffVersionKeys;

            TariffLineQueryService tariffLineQueryService = new TariffLineQueryService(context);
            entityPM.TariffLines = tariffLineQueryService.GetMulti(tariffVersionKeys, true);
        }

        public List<TariffVersionPM> GetDraftVersion(TariffKeys tariffKeys, bool getComposition = true)
        {
            List<TariffVersion> entityPOCOs = Repository.GetMulti(tariffKeys);
            List<TariffVersionPM> entityPMs = new List<TariffVersionPM>();
            foreach (TariffVersion entityPOCO in entityPOCOs.Where(d => d.IsDraft))
            {
                TariffVersionPM entityPM = new TariffVersionPM();
                EntityKeyFields entityKeys = GetKeys(entityPOCO);
                if (entityKeys != null && getComposition)
                {
                    GetComposition(entityKeys, entityPM);
                }

                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }
            return entityPMs;
        }

        public List<TariffVersionPM> GetActiveVersions(string tariffId, int tenant)
        {
            TariffVersionRepository repository = new TariffVersionRepository(tenant);
            List<TariffVersionPM> entityPMs = new List<TariffVersionPM>();
            List<TariffVersion> entityPOCOs = repository.GetActiveVersions(tariffId, tenant);

            foreach (TariffVersion entityPOCO in entityPOCOs)
            {
                TariffVersionPM entityPM = new TariffVersionPM();                
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }

            return entityPMs;
        }

        public List<TariffVersionPM> GetAllVersionsForTariff(string tariffId, int tenant)
        {
            TariffVersionRepository repository = new TariffVersionRepository(tenant);
            List<TariffVersionPM> entityPMs = new List<TariffVersionPM>();
            List<TariffVersion> entityPOCOs = repository.GetAllVersions(tariffId, tenant);

            foreach (TariffVersion entityPOCO in entityPOCOs)
            {
                TariffVersionPM entityPM = new TariffVersionPM();
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }

            return entityPMs;
        }
    }
}
