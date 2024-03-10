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
    public class CCUCRREQUpdateService : EntityUpdateService<CCUCRREQ, CCUCRREQPM, SupplierInvoiceItem103PM>
    {  
        public CCUCRREQUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCRREQRepository(context);

            Mapping = new CCUCRREQDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUCRREQPM entityPM)
        {
            return new CCUCRREQKeys() { ENTNAME = entityPM.ENTNAME, FILENO = entityPM.FILENO, ACCLINENO = entityPM.ACCLINENO, ITEMLINE = entityPM.ITEMLINE, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(CCUCRREQPM entityPM, SupplierInvoiceItem103PM entityParentPM)
        {
            entityPM.ENTNAME = "CCUFILEM";
            entityPM.FILENO = entityParentPM.FILENO;
            entityPM.ACCLINENO = entityParentPM.ACCLINENO;
            entityPM.ITEMLINE = entityParentPM.LINENO;
            entityPM.PRATMEHES = entityParentPM.PRATMEHES;
            entityPM.ITEMNO = entityParentPM.ITEMNO;
            entityParentPM.CCUCRREQPMLastLine++;
            entityPM.LINENO = entityParentPM.CCUCRREQPMLastLine;           
        }

        protected override void OnUpdating(CCUCRREQPM entityPM)
        {
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
            bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
            if (!isConnectedToUnifreight)
            {
                entityPM.IS_SYNCH = false;
                entityPM.LAST_UPDATE_DT = DateTime.Now;
            }

        }

        protected override void AfterUpdating(CCUCRREQPM entityPM, SupplierInvoiceItem103PM entityParentPM)
        {
            if (entityParentPM.CCUCRREQPM.Count == 0) return;
            entityParentPM.CCUCRREQPMLastLine = entityParentPM.CCUCRREQPM.Max(rec => rec.LINENO);
        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            (Repository as CCUCRREQRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}

