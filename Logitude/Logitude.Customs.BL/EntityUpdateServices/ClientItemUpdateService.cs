using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using System.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClientItemUpdateService : EntityUpdateService<ClientItem, ClientItemPM, EntityPM>
    {
        
        protected override void OnUpdating(ClientItemPM entityPM)
        {
            if (entityPM!=null && entityPM.ChangeSetOp != ChangeSetOperation.None) {
                ClientQueryService ClientQueryService = new ClientQueryService(entityPM.Tenant);
                var clientId = ClientQueryService.GetIdByCode(entityPM?.ClientCode, entityPM.Tenant);
                if(clientId!=null) 
                EventTracer.CreateTraceEvent(new EventTracerArgs() { EntityId = clientId, ObjectTableName = "Customs.Client", Tenant = entityPM.Tenant, UserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant), EventTypeCode = "CUCI", Notes = "item code:" + entityPM.ItemCode });
            }

        }



    }
    }
