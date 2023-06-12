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
    public partial class DeclarationStatusQueryService : EntityQueryService<DeclarationStatus, DeclarationStatusKeys, DeclarationStatusPM, object, DeclarationStatusKeys>
    {


        public List<DeclarationStatusPM> GetByDeclarationIdAndTenant(int tenant, string declarationId)
        {

            List<DeclarationStatus> DeclarationStatuses;
            DeclarationStatuses = repository.GetByDeclarationIdAndTenant(tenant,declarationId);
            List<DeclarationStatusPM> DeclarationStatusesPMs = new List<DeclarationStatusPM>();
            DeclarationStatusDataMapping mapping = new DeclarationStatusDataMapping();
            foreach (DeclarationStatus declarationStatus in DeclarationStatuses)
            {

                DeclarationStatusPM declarationStatusPM = new DeclarationStatusPM();
                mapping.CustomPOCOToPM(declarationStatusPM, declarationStatus);
                mapping.POCOToPM(declarationStatusPM, declarationStatus);

                DeclarationStatusesPMs.Add(declarationStatusPM);
            }
            return DeclarationStatusesPMs;
        }
    }
}
