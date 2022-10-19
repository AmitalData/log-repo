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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ServersNameUpdateService : EntityUpdateService<ServersName, ServersNamePM, EntityPM>
    {
        protected override void OnCreating(ServersNamePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id=IdCounter.GetNumber("Customs.ServersName", entityPM.Tenant);
            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
