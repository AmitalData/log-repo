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
using Logitude.Server.Tools.Utils;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUCARSCUpdateService : EntityUpdateService<CCUCARSC, CCUCARSCPM, SupplierInvoiceItem105PM>
    {  
        public CCUCARSCUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCARSCRepository(context);

            Mapping = new CCUCARSCDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCARSCPM entityPM)
        {
            return new CCUCARSCKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO, COUNTER = entityPM.COUNTER };
        }

        protected override void OnCreating(CCUCARSCPM entityPM, SupplierInvoiceItem105PM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityPM.LINENO = entityParentPM.LINENO;
        }

        protected override void OnUpdating(CCUCARSCPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCH = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }
        protected override void OnUpdating(CCUCARSCPM entityPM, CCUCARSC entityPOCO)
        {
            try
            {
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<CCUCARSCPM, CCUCARSC>("2018062018HD312280.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }
        protected override void AfterUpdating(CCUCARSCPM entityPM, SupplierInvoiceItem105PM entityParentPM)
        {
            if (entityParentPM.CCUCARSCs.Count == 0) return;

        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            (Repository as CCUCARSCRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}


