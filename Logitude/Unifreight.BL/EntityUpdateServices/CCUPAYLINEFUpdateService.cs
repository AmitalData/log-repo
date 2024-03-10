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
    public class CCUPAYLINEFUpdateService : EntityUpdateService<CCUPAYLINEF, CCUPAYLINEFPM, CCUPAYHANDPM>
    {
        public CCUPAYLINEFUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUPAYLINEFRepository(context);

            Mapping = new CCUPAYLINEFDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
       
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUPAYLINEFPM entityPM)
        {
            return new CCUPAYLINEFKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(CCUPAYLINEFPM entityPM, CCUPAYHANDPM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;

            entityParentPM.CCUPAYLINEFPMLastLine++;
            entityPM.LINENO = entityParentPM.CCUPAYLINEFPMLastLine;
        }

        protected override void OnUpdating(CCUPAYLINEFPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCH = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }
        }

        protected override void AfterUpdating(CCUPAYLINEFPM entityPM, CCUPAYHANDPM entityParentPM)
        {
            if (entityParentPM.CCUPAYLINEFPMs.Count == 0) return;
            entityParentPM.CCUPAYLINEFPMLastLine = entityParentPM.CCUPAYLINEFPMs.Max(rec => rec.LINENO);
        }

        internal void FastDeleteComposition(CCUFILEMKeys cCUFILEMKeys) // moran 5.1.16 - AMI-55274
        {
            (Repository as CCUPAYLINEFRepository).FastDeleteMulti(cCUFILEMKeys);
        }
    }
}
