using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using System.Data;
using System.Data.Entity.Core;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPendingUpdateService //: EntityUpdateService<DeclarationPending, DeclarationPendingPM, DeclarationPM>
    {
        protected override void OnCreating(DeclarationPendingPM entityPM, EntityPM entityParentPM)
        {
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void AfterUpdating(DeclarationPendingPM entityPM, EntityPM entityParentPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationID, false, false);
            if (_MyDeclarationCourierStatusPM != null)
            {
                var prevCourierPendingReasonList = _MyDeclarationCourierStatusPM.CourierPendingReasonList;
                DeclarationPendingQueryService myCourierPendingReasonQueryService = new DeclarationPendingQueryService(context);
                List<DeclarationPendingPM> declarationPendingPMList = myCourierPendingReasonQueryService.GetDeclarationPendingsByDeclarationId(_MyDeclarationCourierStatusPM.DeclarationId, _MyDeclarationCourierStatusPM.Tenant);
                _MyDeclarationCourierStatusPM.CourierPendingReasonList = null;
                foreach (var declarationPending in declarationPendingPMList)
                {
                    if (declarationPending.Status == "A")
                    {
                        if (_MyDeclarationCourierStatusPM.CourierPendingReasonList == null)
                        {
                            _MyDeclarationCourierStatusPM.CourierPendingReasonList = declarationPending.CourierPendingReasonCode;
                        }
                        else
                        {
                            _MyDeclarationCourierStatusPM.CourierPendingReasonList = string.Concat(_MyDeclarationCourierStatusPM.CourierPendingReasonList, ",", declarationPending.CourierPendingReasonCode);
                        }
                    }
                }
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationCourierStatusPM.Tenant);
                if (_MyDeclarationCourierStatusPM.CourierPendingReasonList != prevCourierPendingReasonList)
                {
                    _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                }
            }
        }


    }
}
