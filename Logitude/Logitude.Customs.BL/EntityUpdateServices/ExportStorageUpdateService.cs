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
    public partial class ExportStorageUpdateService
    {
        protected override void OnCreating(ExportStoragePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.ExportStorage", entityPM.Tenant).ToString();
            if (entityPM.OpenDate == null)
            {
                entityPM.OpenDate = DateTime.Now; //for now 
            }
            
            base.OnCreating(entityPM, entityParentPM);
        }

    }
}
