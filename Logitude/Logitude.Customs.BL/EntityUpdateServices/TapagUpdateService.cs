using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class TapagUpdateService
    {
        protected override void OnCreating(TapagPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            //entityPM.TapagNumber = CodeCounter.GetNumber("Customs.Tapag", entityPM.Tenant).ToString();
            entityPM.Id = IdCounter.GetNumber("Customs.Tapag", entityPM.Tenant).ToString();
            entityPM.CreateDate = DateTime.Now;//for now 
            if (string.IsNullOrEmpty(entityPM.TapagNumber))
            {
                entityPM.TapagNumber = CodeCounter.GetNumber("Customs.Tapag", entityPM.Tenant).ToString();
            }
            base.OnCreating(entityPM, entityParentPM);
        }

        public void OnUpdatingTapag <TEntityPM>(TEntityPM entityPM, bool commit)
            where TEntityPM : ITapagPM, new()
        {
            
            ICustomContext context = MainContext as CustomContext;
            var tapagQueryService = new TapagQueryService(context);
            var tapagUpdateService = new TapagUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

            TapagPM tapagPM = tapagQueryService.GetSingle(entityPM.TapagID, true, false);
            if (tapagPM != null)
            {
                tapagPM.ChangeSetOp = ChangeSetOperation.Update;
                tapagPM.TapagTypeCode = entityPM.TapagTypeCode;
                tapagPM.CustomsBranchCode = entityPM.CustomsBranchCode;
                tapagPM.ValidityDate = entityPM.ValidityDate;
                tapagPM.FollowDate = entityPM.FollowDate;
                tapagUpdateService.Update(tapagPM, false);
            }

        }
    }
}
