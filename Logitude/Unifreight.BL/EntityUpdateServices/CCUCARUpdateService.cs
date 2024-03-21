using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUCARUpdateService : EntityUpdateService<CCUCAR, CCUCARPM, SupplierInvoiceItem105PM>
    {
        public CCUCARUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCARRepository(context);

            Mapping = new CCUCARDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCARPM entityPM)
        {
            return new CCUCARKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(CCUCARPM entityPM, SupplierInvoiceItem105PM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityPM.LINENO = entityParentPM.LINENO;
            
        }

        protected override void OnUpdating(CCUCARPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCHRONIZED = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }

        protected override void AfterUpdating(CCUCARPM entityPM, SupplierInvoiceItem105PM entityParentPM)
        {
            if (entityParentPM.CCUCARs.Count == 0) return;
            
        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            (Repository as CCUCARRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}


