using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.Validators;
using System.ComponentModel.DataAnnotations;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconcileExternalPageLineUpdateService : EntityUpdateService<ReconcileExternalPageLine, ReconcileExternalPageLinePM, ReconcileExternalPagePM>
    {
        protected override void OnCreating(ReconcileExternalPageLinePM entityPM, ReconcileExternalPagePM entityParentPM)
        {
            if (entityPM.Id == null || entityPM.Id == "")
                entityPM.Id = IdCounter.GetNumber("ReconcileExternalPageLine", entityPM.Tenant);

            entityPM.ReconcileExternalPageId = entityParentPM.Id;

            base.OnCreating(entityPM, entityParentPM);
        }

    }
}
