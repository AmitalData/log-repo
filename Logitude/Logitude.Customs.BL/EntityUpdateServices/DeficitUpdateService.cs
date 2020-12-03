using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeficitUpdateService
    {
        protected override void OnCreating(DeficitPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id=IdCounter.GetNumber("Customs.Deficit",entityPM.Tenant);
            ICustomContext context=MainContext as CustomContext;
            TapagQueryService tapagQueryService = new TapagQueryService(context);

            /*TapagPM tapag = tapagQueryService.GetSingleTapagByLeadingFileNumber(entityPM.LeadingFileNumber, entityPM.Tenant);
            if (tapag != null)
            {
                entityPM.TapagId = tapag.Id;
            }
            else
            {*/
            TapagPM tapag = new TapagPM()
            {
                Id = IdCounter.GetNumber("Customs.Tapag", entityPM.Tenant),
                Tenant = entityPM.Tenant,
                CustomerId = entityPM.CustomerId,
                CustomsBranchCode = entityPM.CustomsBranchCode,
                ImporterId = entityPM.ImporterId,
                FollowDate = entityPM.FollowDate,
                IsClosed = entityPM.IsClosed,
                LeadingFileNumber = entityPM.LeadingFileNumber,
                ProfessionUnitTypeCode = entityPM.ProfessionUnitTypeCode,
                SpecializationTypeCode = entityPM.SpecializationTypeCode,
                TapagNumber = entityPM.TapagNumber,
                TapagTypeCode = "1",
                ValidityDate = entityPM.ValidityDate,
                CreateDate = entityPM.CreateDate,
                ChangeSetOp = ChangeSetOperation.Insert,
            };

                TapagUpdateService tapagUpdate = new TapagUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
                tapagUpdate.Update(tapag, false);
                entityPM.TapagId = tapag.Id;
            //}

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(DeficitPM entityPM)
        {
            OnUpdatingTapag(entityPM);
            base.OnUpdating(entityPM);
        }

        private void OnUpdatingTapag(DeficitPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            var tapagQueryService = new TapagQueryService(context);
            var tapagUpdateService = new TapagUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

            TapagPM tapagPM = tapagQueryService.GetSingle(entityPM.TapagId, true, false);
            if (tapagPM != null)
            {
                tapagPM.ChangeSetOp = ChangeSetOperation.Update;
                //tapagPM.CustomsBranchCode = entityPM.CustomsBranchCode;
                tapagPM.ValidityDate = entityPM.ValidityDate;
                tapagPM.FollowDate = entityPM.FollowDate;
                tapagUpdateService.Update(tapagPM, false);
            }
        }

        protected override void UpdateComposition(DeficitPM entityPM)
        {
            DeficitDecisionUpdateService deficitDecisionUpdateService = new DeficitDecisionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            deficitDecisionUpdateService.UpdateMulti(entityPM.DeficitDecisions, entityPM.DeletedDeficitDecisions, entityPM, false);
            base.UpdateComposition(entityPM);

        }
    }
}
