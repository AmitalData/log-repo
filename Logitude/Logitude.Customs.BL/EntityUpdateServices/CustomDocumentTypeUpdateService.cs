using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
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
    public partial class CustomDocumentTypeUpdateService : EntityUpdateService<CustomDocumentType, CustomDocumentTypePM, EntityPM>
      {
        public string userId;
		protected override void OnUpdating(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO)
		{
			if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
			{
				ICustomContext context = MainContext as CustomContext;
				CustomDocumentTypeTenantRepository definitionRep = new CustomDocumentTypeTenantRepository(context);
				CustomDocumentTypeTenant tenant_def = definitionRep.GetSingleByCode(entityPM.Code, Tenant);
				if (tenant_def != null)
				{
					Map2TenantDef(entityPM, tenant_def);
					definitionRep.Update(tenant_def);
				}
				else
				{
					tenant_def = new CustomDocumentTypeTenant();
					Map2TenantDef(entityPM, tenant_def);
					tenant_def.Id = IdCounter.GetNumber("Customs. CustomDocumentTypeTenant", Tenant);
					tenant_def.Code = entityPM.Code;
					tenant_def.UpdateDate = DateTime.Now;
					tenant_def.UpdatedByUserId = userId;
					tenant_def.Tenant = Tenant;
					definitionRep.Add(tenant_def);
				}
				
				entityPM.AutoSetOriginalDocumentTrue = entityPOCO.AutoSetOriginalDocumentTrue;
				entityPM.CustomsDocumentUpload = entityPOCO.CustomsDocumentUpload;
				entityPM.IsCourierManadatory = entityPOCO.IsCourierManadatory;
				entityPM.IsDiamondManadatory = entityPOCO.IsDiamondManadatory;
				entityPM.PointerLevel = entityPOCO.PointerLevel;
			}
		}
		private static void Map2TenantDef(CustomDocumentTypePM entityPM, CustomDocumentTypeTenant tenant_def)
        {
            tenant_def.AutoSetOriginalDocumentTrue = entityPM.AutoSetOriginalDocumentTrue;
            tenant_def.CustomsDocumentUpload = entityPM.CustomsDocumentUpload;
            tenant_def.IsCourierManadatory = entityPM.IsCourierManadatory;
            tenant_def.IsDiamondManadatory = entityPM.IsDiamondManadatory;
            tenant_def.PointerLevel = entityPM.PointerLevel;
        }
		protected override void AfterUpdating(CustomDocumentTypePM entityPM, EntityPM entityParentPM)
		{
			Mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
		}
	}
}
