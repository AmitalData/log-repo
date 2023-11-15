
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
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
    public partial class GovernmentProcedureTypeUpdateService : ICanUpdateClosedTable<GovernmentProcedureTypePM>
    {
        public string userId;
        protected override void OnUpdating(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                ICustomContext context = MainContext as CustomContext;
                GovernmentProcTypeTenantRepository definitionRep = new GovernmentProcTypeTenantRepository(context);
                GovernmentProcTypeTenant tenant_def = definitionRep.GetSingleByCode(entityPM.Code, Tenant);
                if (tenant_def != null)
                {
                    Map2TenantDef(entityPM, tenant_def);
                    definitionRep.Update(tenant_def);
                }
                else
                {
                    tenant_def = new GovernmentProcTypeTenant();
                    Map2TenantDef(entityPM, tenant_def);
                    tenant_def.Id = IdCounter.GetNumber("Customs.GovernmentProcTypeTenant", Tenant);
                    tenant_def.Code = entityPM.Code;
                    tenant_def.UpdateDate = DateTime.Now;
                    tenant_def.UpdatedByUserId = userId;
                    tenant_def.Tenant = Tenant;
                    definitionRep.Add(tenant_def);
                }
				entityPM.IsImport = entityPOCO.IsImport;
				entityPM.IsExport = entityPOCO.IsExport;
				entityPM.IndexOrder = entityPOCO.IndexOrder;
			}
        }

        private static void Map2TenantDef(GovernmentProcedureTypePM entityPM, GovernmentProcTypeTenant tenant_def)
        {
            tenant_def.IsImport = entityPM.IsImport;
            tenant_def.IsExport = entityPM.IsExport;
            tenant_def.IndexOrder = entityPM.IndexOrder;
        }
		protected override void AfterUpdating(GovernmentProcedureTypePM entityPM, EntityPM entityParentPM)
		{
			Mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
		}
	}
}
