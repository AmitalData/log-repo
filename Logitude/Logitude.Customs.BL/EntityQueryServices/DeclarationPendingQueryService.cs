using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationPendingQueryService : EntityQueryService<DeclarationPending, DeclarationPendingKeys, DeclarationPendingPM, DeclarationPM, DeclarationKeys>
    {


        public List<DeclarationPendingPM> GetDeclarationPendingsByDeclrationId(string declarationId, int tenant)
        {

            List<DeclarationPending> Pendings;

            Pendings = repository.GetDeclarationPendingsByDeclarationId(declarationId, tenant);

            List<DeclarationPendingPM> PendingsPMs = new List<DeclarationPendingPM>();
            DeclarationPendingDataMapping mapping = new DeclarationPendingDataMapping();
            foreach (DeclarationPending pending in Pendings)
            {

                DeclarationPendingPM pendingPM = new DeclarationPendingPM();
                mapping.CustomPOCOToPM(pendingPM, pending);
                mapping.POCOToPM(pendingPM, pending);

                PendingsPMs.Add(pendingPM);
            }
            return PendingsPMs;
        }


        public string GetDeclarationPendingsByPendingId(string pending, int tenant)
        {
            if (String.IsNullOrWhiteSpace(pending)) return null;
            return repository.GetDeclarationPendingsByPendingId(pending, tenant);
        }

        public bool GetDeclarationPending(string declarationId, int tenant)
        {
            return repository.DeclarationHasPending(declarationId, tenant);
        }
    }
}
