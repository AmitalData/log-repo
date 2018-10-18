using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
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
    public partial class ClaimQueryService : EntityQueryService<Claim, ClaimKeys, ClaimPM, object, ClaimKeys>
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, ClaimPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClaimKeys claimKeys = entityKeys as ClaimKeys;
            ClaimsRelatedEntityQueryService claimsRelatedEntityQueryService = new ClaimsRelatedEntityQueryService(context);
            ClaimImporterDeclarsPage3QueryService claimImporterDeclarsPage3QueryService = new ClaimImporterDeclarsPage3QueryService(context);
            ClaimImporterDeclarsPage3AQueryService claimImporterDeclarsPage3AQueryService = new ClaimImporterDeclarsPage3AQueryService(context);
            ClaimImporterDeclarsPage3BQueryService claimImporterDeclarsPage3BQueryService = new ClaimImporterDeclarsPage3BQueryService(context);

            entityPM.ClaimsRelatedEntities = claimsRelatedEntityQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimImporterDeclarsPage3 = claimImporterDeclarsPage3QueryService.GetMulti(claimKeys, true);
            entityPM.ClaimImporterDeclarsPage3A = claimImporterDeclarsPage3AQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimImporterDeclarsPage3B = claimImporterDeclarsPage3BQueryService.GetMulti(claimKeys, true);
        }
    }
}
