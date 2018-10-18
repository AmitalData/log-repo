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
    public partial class ClaimImporterDeclarsPage3QueryService : EntityQueryService<ClaimImporterDeclarsPage3, ClaimImporterDeclarsPage3Keys, ClaimImporterDeclarsPage3PM, ClaimPM, ClaimKeys>
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, ClaimImporterDeclarsPage3PM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClaimImporterDeclarsPage3Keys claimImporterDeclarsPage3Keys = entityKeys as ClaimImporterDeclarsPage3Keys;
            ClaimImporterDeclarsP3LoiQueryService claimImporterDeclarsP3LoiQueryService = new ClaimImporterDeclarsP3LoiQueryService(context);

            entityPM.ClaimImporterDeclarsP3Loi = claimImporterDeclarsP3LoiQueryService.GetMulti(claimImporterDeclarsPage3Keys, true);

            base.GetComposition(entityKeys, entityPM);
        }

    }
}
