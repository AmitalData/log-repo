using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
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
            if (!string.IsNullOrEmpty(entityPM.MamanSpecialActionStatusCode))
            {
                var context = CustomContext.GetContext(entityPM.Tenant);

                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, true, false);
                string specialActionStatus = myDeclarationCourierStatusPM.SpecialActionStatus;
                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, null, entityPM.Tenant);
                calculateDeclarationCourierStatus.CalcSpecialActionStatus(myDeclarationCourierStatusPM);

                if (specialActionStatus != myDeclarationCourierStatusPM.SpecialActionStatus)
                {
                    //LogitudeSettings.HandleLogMe("AfterUpdating myDeclarationCourierStatusPM " + myDeclarationCourierStatusPM.DeclarationId + " SpecialActionStatus " + myDeclarationCourierStatusPM.SpecialActionStatus, false, "maman", new DateTime(2019, 2, 1));


                    var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(entityPM.Tenant);
                    declarationCourierStatusRepository.Lock_forUpdateNOWAIT(entityPM.DeclarationId);


                    myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    declarationCourierStatusUpdateService.Update(myDeclarationCourierStatusPM, true);
                }
            }

        }
    }
}
