
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CurrencyTypeTenantUpdateService : EntityUpdateService<CurrencyTypeTenant, CurrencyTypeTenantPM, EntityPM>
    {

        protected override void OnCreating(CurrencyTypeTenantPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CurrencyTypeTenant", entityPM.Tenant);
            entityPM.Tenant = entityPM.Tenant;
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(CurrencyTypeTenantPM entityPM)
        {
            
            entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            entityPM.UpdateDate = DateTime.Now;
            

            base.OnUpdating(entityPM);
        }
        protected override void OnUpdating(CurrencyTypeTenantPM entityPM, CurrencyTypeTenant entityPOCO)
        {
            if(entityPOCO.Code!= entityPOCO.Code)
            {
                throw new Exception("if(entityPOCO.Code!= entityPOCO.Code)");
            }
            if (entityPOCO.Tenant != entityPOCO.Tenant)
            {
                throw new Exception("if(entityPOCO.Code!= entityPOCO.Code)");
            }

            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
