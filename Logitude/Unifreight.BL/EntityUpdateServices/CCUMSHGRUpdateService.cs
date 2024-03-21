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
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUMSHGRUpdateService : EntityUpdateService<CCUMSHGR, CCUMSHGRPM, CCUFILEMPM>
    {
        public CCUMSHGRUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUMSHGRRepository(context);

            Mapping = new CCUMSHGRDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUMSHGRPM entityPM)
        {
            return new CCUMSHGRKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(CCUMSHGRPM entityPM, CCUFILEMPM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityParentPM.CCUMSHGRLastLine++;
            entityPM.LINENO = entityParentPM.CCUMSHGRLastLine;
        }

        protected override void OnUpdating(CCUMSHGRPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCHRONIZED = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }

        protected override void AfterUpdating(CCUMSHGRPM entityPM, CCUFILEMPM entityParentPM)
        {
            if (entityParentPM.CCUMSHGRs.Count == 0) return;
            entityParentPM.CCUMSHGRLastLine = entityParentPM.CCUMSHGRs.Max(rec => rec.LINENO);      
        }

        internal void FastDeleteComposition(EntityKeyFields parentEntityKeys)
        {
            var myCCUSIGNUMUpdateService = new CCUSIGNUMUpdateService(this.MainContext as AmitalContext);
            myCCUSIGNUMUpdateService.FastDeleteComposition(parentEntityKeys);

            (Repository as CCUMSHGRRepository).FastDeleteMulti(parentEntityKeys);
        }

        protected override void UpdateComposition(CCUMSHGRPM entityPM)
        {
            var myCCUSIGNUMUpdateService = new CCUSIGNUMUpdateService(this.MainContext as AmitalContext);
            myCCUSIGNUMUpdateService.UpdateMulti(entityPM.CCUSIGNUMPMs, entityPM.DeletedCCUSIGNUMPMs, entityPM, false);

            base.UpdateComposition(entityPM);
        }
    }
}
