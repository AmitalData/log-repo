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


        internal void Update_InProgressExternalReconcile(List<string> listReconcileExternalPageLineId, int tenant, bool Value_ExternalReconcileInProgress)
        {
            var reconcileExternalPageLineQueryService = new ReconcileExternalPageLineQueryService(MainContext as IAccountingContext);
            var pmList = reconcileExternalPageLineQueryService.GetPageLinesPMsByIdList(listReconcileExternalPageLineId, tenant);

            foreach (var item in pmList)
            {
                if (Value_ExternalReconcileInProgress == true)//while prepare check while streaming do not check !!
                {
                    if (item.InProgressExternalReconcile)
                    {
                        throw new Exception("Already InReconcileProgress");
                    }
                }
                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                item.InProgressExternalReconcile = /*true*/ Value_ExternalReconcileInProgress;
            }
            this.UpdateMulti(pmList, new List<ReconcileExternalPageLinePM>(), new ReconcileExternalPagePM(), true);
        }
    }
}
