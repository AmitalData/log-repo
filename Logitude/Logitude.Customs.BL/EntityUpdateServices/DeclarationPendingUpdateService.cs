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
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Models;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPendingUpdateService : EntityUpdateService<DeclarationPending, DeclarationPendingPM, DeclarationPM>
    {
        protected override void OnCreating(DeclarationPendingPM entityPM, DeclarationPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.DeclarationID = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);
        }
        
    }
}
