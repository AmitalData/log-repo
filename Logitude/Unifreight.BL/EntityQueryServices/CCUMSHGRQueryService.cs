using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Unifreight.BL.EntityQueryServices
{
    public class CCUMSHGRQueryService : EntityQueryService<CCUMSHGR, CCUMSHGRKeys, CCUMSHGRPM, CCUFILEMPM , CCUFILEMKeys>
    {        
        public CCUMSHGRQueryService(AmitalContext context)
        {
            MainContext  = context;
            Repository = new CCUMSHGRRepository(context);
            mapping = new CCUMSHGRDataMapping();
        }

        public CCUMSHGRPM GetSingle(int FILENO, int LINENO,int tenant, bool getComposition)
        {
            var keys = new CCUMSHGRKeys() { FILENO = FILENO, LINENO = LINENO , Tenant=tenant };
            return  base.GetSingle(keys , getComposition ,false );        
        }

        protected override EntityKeyFields GetKeys(CCUMSHGR entityPOCO)
        {
            return new CCUMSHGRKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, Tenant = entityPOCO.TENANT };
        }

        public override void GetComposition(EntityKeyFields entityKeys, CCUMSHGRPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var myCCUMSHGRkeys = entityKeys as CCUMSHGRKeys;

            var myCCUSIGNUMQueryService = new CCUSIGNUMQueryService(amitalContext);
            entityPM.CCUSIGNUMPMs = myCCUSIGNUMQueryService.GetMulti(myCCUMSHGRkeys, true);
            entityPM.CCUSIGNUMPMsLastLine = (entityPM.CCUSIGNUMPMs.Count == 0) ? 0 : entityPM.CCUSIGNUMPMs.Max(rec => rec.LINENOSIGN);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}
