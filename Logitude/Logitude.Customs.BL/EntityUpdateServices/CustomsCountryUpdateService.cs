using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsCountryUpdateService : ICanUpdateClosedTable<CustomsCountryPM>
    {
		public string userId;
        protected override void OnCreating(CustomsCountryPM entityPM, EntityPM entityParentPM)
        {
			
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(CustomsCountryPM entityPM, CustomsCountry entityPOCO)
		{
			if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
			{
				ICustomContext context = MainContext as CustomContext;
				CustomsCountryTenantRepository definitionRep = new CustomsCountryTenantRepository(context);
				CustomsCountryTenant tenant_def = definitionRep.GetSingleByCode(entityPM.Code, Tenant);
				if (tenant_def != null)
				{
					Map2TenantDef(entityPM, tenant_def);
					definitionRep.Update(tenant_def);
				}
				else
				{
					tenant_def = new CustomsCountryTenant();
					Map2TenantDef(entityPM, tenant_def);
					tenant_def.Id = IdCounter.GetNumber("Customs.CustomsCountryTenant", Tenant);
					tenant_def.Code = entityPM.Code;
					tenant_def.UpdateDate = DateTime.Now;
					if(userId != null)
                    {
                        tenant_def.UpdatedByUserId = userId;
                    }
                    else
					{
                        ContactRepository contactRepository = new ContactRepository(Tenant);
                        var systemUser = contactRepository.GetSingleContactByEmail("system@tenant" + Tenant + ".com", Tenant, true);
						if(systemUser != null)
						{
                            tenant_def.UpdatedByUserId = systemUser.Id;
                        } 
                    }
                    tenant_def.Tenant = Tenant;
					definitionRep.Add(tenant_def);
				}

				entityPM.TarriffCode = entityPOCO.TarriffCode;
				entityPM.MalamId = entityPOCO.MalamId;
				
			}
		}
		private static void Map2TenantDef(CustomsCountryPM entityPM, CustomsCountryTenant tenant_def)
		{
			tenant_def.TarriffCode = entityPM.TarriffCode;
			tenant_def.MalamId = entityPM.MalamId;
		}
		protected override void AfterUpdating(CustomsCountryPM entityPM, EntityPM entityParentPM)
		{
			Mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
		}
	}
}
