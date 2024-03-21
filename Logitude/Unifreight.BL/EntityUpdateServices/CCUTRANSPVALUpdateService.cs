//Yuval Chalup 14.06.2015 TASK-13951
using System;
using System.Collections.Generic;
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
    public class CCUTRANSPVALUpdateService : EntityUpdateService<CCUTRANSPVAL, CCUTRANSPVALPM, CCUFILEMPM>
    {
        public CCUTRANSPVALUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUTRANSPVALRepository(context);

            Mapping = new CCUTRANSPVALDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUTRANSPVALPM entityPM)
        {
            return new CCUTRANSPVALKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(CCUTRANSPVALPM entityPM, CCUFILEMPM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityParentPM.CCUTRANSPVALLastLine++;
            entityPM.LINENO = entityParentPM.CCUTRANSPVALLastLine;
        }

        protected override void OnUpdating(CCUTRANSPVALPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCHRONIZED = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }
        }

        protected override void AfterUpdating(CCUTRANSPVALPM entityPM, CCUFILEMPM entityParentPM)
        {
            if (entityParentPM.CCUTRANSPVALs.Count == 0) return;
            entityParentPM.CCUTRANSPVALLastLine = entityParentPM.CCUTRANSPVALs.Max(rec => rec.LINENO);
        }

        internal void FastDeleteComposition(EntityKeyFields parentEntityKeys)
        {
            (Repository as CCUTRANSPVALRepository).FastDeleteMulti(parentEntityKeys);
        }
    }
}
