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

    public class CCUTAXUpdateService : EntityUpdateService<CCUTAX, CCUTAXPM, CCUFILEMPM>
    {  
        public CCUTAXUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUTAXRepository(context);

            Mapping = new CCUTAXDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUTAXPM entityPM)
        {
            return new CCUTAXKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

       protected override void OnCreating(CCUTAXPM entityPM, CCUFILEMPM entityParentPM)
       {
            entityPM.FILENO = entityParentPM.FILENO;
            entityParentPM.CCUTAXPMLastLine++;
            entityPM.LINENO = entityParentPM.CCUTAXPMLastLine;
        }

       protected override void OnUpdating(CCUTAXPM entityPM)
       {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCH = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }
        }

       protected override void AfterUpdating(CCUTAXPM entityPM, CCUFILEMPM entityParentPM)
       {
           if (entityParentPM.CCUTAXPM.Count == 0) return;
           entityParentPM.CCUTAXPMLastLine = entityParentPM.CCUTAXPM.Max(rec => rec.LINENO);
       }

        internal void FastDeleteComposition(CCUFILEMKeys cCUFILEMKeys, string taxType = null)
        {
            (Repository as CCUTAXRepository).FastDeleteMulti(cCUFILEMKeys, taxType);
        }
    }
}
