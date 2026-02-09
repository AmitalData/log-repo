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
    public class CCUSIGNUMUpdateService : EntityUpdateService<CCUSIGNUM, CCUSIGNUMPM, CCUMSHGRPM>
    {
        public CCUSIGNUMUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUSIGNUMRepository(context);

            Mapping = new CCUSIGNUMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUSIGNUMPM entityPM)
        {
            return new CCUSIGNUMKeys() { FILENO = entityPM.FILENO, LINENOMSHGR = entityPM.LINENOMSHGR, LINENOSIGN = entityPM.LINENOSIGN, Tenant = entityPM.Tenant };
        }

        protected override void OnCreating(CCUSIGNUMPM entityPM, CCUMSHGRPM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityPM.LINENOMSHGR = entityParentPM.LINENO;
            entityParentPM.CCUSIGNUMPMsLastLine++;
            entityPM.LINENOSIGN = entityParentPM.CCUSIGNUMPMsLastLine;
        }

        protected override void OnUpdating(CCUSIGNUMPM entityPM)
        {
            if (entityPM.Tenant != 0)
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

        protected override void AfterUpdating(CCUSIGNUMPM entityPM, CCUMSHGRPM entityParentPM)
        {
            if (entityParentPM.CCUSIGNUMPMs.Count == 0) return;
            entityParentPM.CCUSIGNUMPMsLastLine = entityParentPM.CCUSIGNUMPMs.Max(rec => rec.LINENOSIGN);
        }

        internal void FastDeleteComposition(EntityKeyFields parentEntityKeys)
        {
            (Repository as CCUSIGNUMRepository).FastDeleteMulti(parentEntityKeys);
        }
    }
}
