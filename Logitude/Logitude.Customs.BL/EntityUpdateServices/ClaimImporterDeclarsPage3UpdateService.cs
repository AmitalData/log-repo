using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{

    public partial class ClaimImporterDeclarsPage3UpdateService  // : EntityUpdateService<ClaimImporterDeclarsPage3, ClaimImporterDeclarsPage3PM, ClaimPM>
    {

        protected override void OnCreating(ClaimImporterDeclarsPage3PM entityPM, ClaimPM entityParentPM)
        {
            entityPM.ClaimId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            if (entityPM.LineNo == 0)
            {
                int line = 0;
                if (entityParentPM.ClaimImporterDeclarsPage3.Count > 0)
                {
                    line = entityParentPM.ClaimImporterDeclarsPage3.Max(d => d.LineNo);
                }
                entityPM.LineNo = line + 1;
            }

            base.OnCreating(entityPM, entityParentPM);
        }
         
        protected override void UpdateComposition(ClaimImporterDeclarsPage3PM entityPM)
        {

            //if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            //{
            //    var qs = new ClaimImporterDeclarsP3LoiQueryService(entityPM.Tenant);
            //    var list2del = qs.GetMulti(new Data.EntityKeys.ClaimImporterDeclarsPage3Keys()
            //    {
            //        ClaimId = entityPM.ClaimId,
            //        LineNo = entityPM.LineNo
            //    }, false);
            //    entityPM.DeletedClaimImporterDeclarsP3Loi = new List<EntityPMs.ClaimImporterDeclarsP3LoiPM>();
            //    foreach (var item in list2del)
            //    {
            //        item.ChangeSetOp = ChangeSetOperation.Delete; 
            //        entityPM.DeletedClaimImporterDeclarsP3Loi.Add(item);
            //    }
                 
            //}

            var claimImporterDeclarsP3LoiUpdateService = new ClaimImporterDeclarsP3LoiUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimImporterDeclarsP3LoiUpdateService.UpdateMulti(entityPM.ClaimImporterDeclarsP3Loi, entityPM.DeletedClaimImporterDeclarsP3Loi, entityPM, false);

            base.UpdateComposition(entityPM);
        }
    }
}
