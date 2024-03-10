using System;
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
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUQUELOCKUpdateService : EntityUpdateService<CCUQUELOCK, CCUQUELOCKPM, EntityPM>
    {
        public CCUQUELOCKUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUQUELOCKRepository(context);

            Mapping = new CCUQUELOCKDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUQUELOCKPM entityPM)
        {
            return new CCUQUELOCKKeys() { ENTNAME = entityPM.ENTNAME, FILENO = entityPM.FILENO };
        }

        protected override void OnCreating(CCUQUELOCKPM entityPM, EntityPM entityParentPM)
        {
        }
        protected override void OnUpdating(CCUQUELOCKPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCH = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }
    }
}

