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
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{

    public class CCUSUPITEMSIUpdateService : EntityUpdateService<CCUSUPITEMSI, CCUSUPITEMSIPM, SupplierInvoiceItem103PM>
    {
        public CCUSUPITEMSIUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUSUPITEMSIRepository(context);

            Mapping = new CCUSUPITEMSIDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUSUPITEMSIPM entityPM)
        {
            return new CCUSUPITEMSIKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO, ACCLINENO = entityPM.ACCLINENO };
        }

        protected override void OnCreating(CCUSUPITEMSIPM entityPM, SupplierInvoiceItem103PM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.FILENO = entityParentPM.FILENO;
                entityPM.LINENO = entityParentPM.LINENO;
                entityPM.ACCLINENO = entityParentPM.ACCLINENO;
            }
        }

        protected override void OnUpdating(CCUSUPITEMSIPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCHRONIZED = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }

        //protected override void UpdateComposition(CCUSUPITEMSIPM entityPM)
        //{

        //    var myCCUCARUpdateService = new CCUCARUpdateService(this.MainContext as AmitalContext);
        //    myCCUCARUpdateService.UpdateMulti(entityPM.CCUCARs, entityPM.DeletedCCUCARPM, entityPM, false);

        //    var myCCUCARLUpdateService = new CCUCARLUpdateService(this.MainContext as AmitalContext);
        //    myCCUCARLUpdateService.UpdateMulti(entityPM.CCUCARLs, entityPM.DeletedCCUCARLPM, entityPM, false);

        //    var myCCUCARSCUpdateService = new CCUCARSCUpdateService(this.MainContext as AmitalContext); // moran 11.7.16 - Task 20132
        //    myCCUCARSCUpdateService.UpdateMulti(entityPM.CCUCARSCs, entityPM.DeletedCCUCARSCPM, entityPM, false);

        //}

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            (Repository as CCUSUPITEMSIRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
