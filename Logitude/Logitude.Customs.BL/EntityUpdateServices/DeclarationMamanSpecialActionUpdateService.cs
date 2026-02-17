using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationMamanSpecialActionUpdateService : EntityUpdateService<DeclarationMamanSpecialAction, DeclarationMamanSpecialActionPM, EntityPM>
    {
        protected override void OnCreating(DeclarationMamanSpecialActionPM entityPM, EntityPM entityParentPM)
        {
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void AfterUpdating(DeclarationMamanSpecialActionPM entityPM, EntityPM entityParentPM)
        {
            var context = CustomContext.GetContext(entityPM.Tenant);

            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, false, false);
            CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, null, entityPM.Tenant);
            calculateDeclarationCourierStatus.CalcSpecialActionStatus(myDeclarationCourierStatusPM);
            
            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);        
            declarationCourierStatusUpdateService.Update(myDeclarationCourierStatusPM, true);

        }
    }
}
