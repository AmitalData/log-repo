using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.NotificationBL;
using System.Diagnostics;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.BL.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging;
using System.Data;
using System.Data.Entity.Core;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationCargoSplitUpdateService : EntityUpdateService<DeclarationCargoSplit, DeclarationCargoSplitPM,EntityPM>
    {
        protected override void OnCreating(DeclarationCargoSplitPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.DeclarationCargoSplit", entityPM.Tenant);
        }

        protected override void UpdateComposition(DeclarationCargoSplitPM entityPM)
        {
            DecCargoSplitConUpdateService decCargoSplitConUpdateServiceUpdateService = new DecCargoSplitConUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            decCargoSplitConUpdateServiceUpdateService.UpdateMulti(entityPM.DecCargoSplitCons, entityPM.DeletedDecCargoSplitCons, entityPM, false);

            DecCargoSplitCargoIdentifierUpdateService decCargoSplitCargoIdentifierUpdateServiceUpdateService = new DecCargoSplitCargoIdentifierUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            decCargoSplitCargoIdentifierUpdateServiceUpdateService.UpdateMulti(entityPM.DecCargoSplitCargoIdentifiers, entityPM.DeletedDecCargoSplitCargoIdentifiers, entityPM, false);
        }

        protected override void OnUpdating(DeclarationCargoSplitPM entityPM)
        {
                   
            UpdateUnifreight(entityPM);        
 
            var context = CustomContext.GetContext(entityPM.Tenant);
            DeclarationQueryService myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(entityPM.DeclarationId, true, false);
            if(declarationPM != null)
            {
                entityPM.Direction = declarationPM.Direction;
                entityPM.TransportModeId=declarationPM.TransportModeId;
            }
         }
    }
}
