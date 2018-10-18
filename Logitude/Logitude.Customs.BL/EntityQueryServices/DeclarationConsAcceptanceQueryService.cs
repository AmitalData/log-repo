using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationConsAcceptanceQueryService : EntityQueryService<DeclarationConsAcceptance, DeclarationConsAcceptanceKeys, DeclarationConsAcceptancePM, object, DeclarationConsAcceptanceKeys>
    {
        public int? GetMaxCounterKey(string declarationId, int tenant)
        {
            return repository.GetMaxCounterKey(declarationId, tenant);
        }

        public List<DeclarationConsAcceptancePM> GetMulti(DeclarationKeys entityParentKeys,bool getComposition)
        {
            List<DeclarationConsAcceptance> entityPOCOs = Repository.GetMulti(entityParentKeys);
            List<DeclarationConsAcceptancePM> entityPMs = new List<DeclarationConsAcceptancePM>();
            foreach (DeclarationConsAcceptance entityPOCO in entityPOCOs)
            {
                DeclarationConsAcceptancePM entityPM = new DeclarationConsAcceptancePM();
                DeclarationKeys entityKeys = new DeclarationKeys() { Id = entityPOCO.DeclarationId,};//GetKeys(entityPOCO);
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
    }
}
