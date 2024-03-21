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
using Logitude.Server.Tools.Models;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUPAYHANDUpdateService : EntityUpdateService<CCUPAYHAND, CCUPAYHANDPM, EntityPM>
    {       
        public CCUPAYHANDUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUPAYHANDRepository(context);

            Mapping = new CCUPAYHANDDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
   
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUPAYHANDPM entityPM)
        {
            return new CCUPAYHANDKeys() { FILENO = entityPM.FILENO };
        }

        protected override void OnCreating(CCUPAYHANDPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.FILENO == 0)
            {
                //new BusinessErrorException("please init entityPM.DeclarationId");
                CCUFILEMPM myCCUFILEMPM = entityParentPM as CCUFILEMPM;
                if(myCCUFILEMPM != null && myCCUFILEMPM.FILENO > 0)
                {
                    entityPM.FILENO = myCCUFILEMPM.FILENO;
                }
            }
        }

        protected override void OnUpdating(CCUPAYHANDPM entityPM, CCUPAYHAND entityPOCO)
        {
            try
            {
                CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
                bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
                if (!isConnectedToUnifreight)
                {
                    entityPM.IS_SYNCHRONIZED = false;
                    entityPM.LAST_UPDATE_DT = DateTime.Now;
                }
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<CCUPAYHANDPM, CCUPAYHAND>("20180708HD310784.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }

        private int GetCounter(string dirtyDeclarationPMId)
        {
            int i = Convert.ToInt32(dirtyDeclarationPMId.Replace("-", ""));
            return 50000000 + i;
        }

        protected override void UpdateComposition(CCUPAYHANDPM entityPM)
        {
            var myCCUPAYLINEFUpdateService = new CCUPAYLINEFUpdateService(this.MainContext as AmitalContext);
            myCCUPAYLINEFUpdateService.UpdateMulti(entityPM.CCUPAYLINEFPMs, entityPM.DeletedCCUPAYLINEFs, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        internal void FastDeleteComposition(CCUFILEMKeys cCUFILEMKeys) // moran 5.1.16 - AMI-55274
        {
            (Repository as CCUPAYHANDRepository).FastDeleteMulti(cCUFILEMKeys);
        }
    }
}
