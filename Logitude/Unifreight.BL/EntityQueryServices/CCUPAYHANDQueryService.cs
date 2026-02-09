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
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class CCUPAYHANDQueryService : EntityQueryService<CCUPAYHAND, CCUPAYHANDKeys, CCUPAYHANDPM, object, CCUPAYHANDKeys>
    {
        public CCUPAYHANDQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new CCUPAYHANDRepository(context);
            this.mapping = new CCUPAYHANDDataMapping();
        }

        public CCUPAYHANDPM GetSingle(int FILENO, bool getComposition, int tenant, bool getFromCache)
        {
            var EntityKeys = new CCUPAYHANDKeys() { FILENO = FILENO , Tenant=tenant };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUPAYHAND entityPOCO)
        {
            return new CCUPAYHANDKeys() { FILENO = entityPOCO.FILENO, Tenant = entityPOCO.TENANT };
        }

        public override void GetComposition(EntityKeyFields entityKeys, CCUPAYHANDPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var CCUPAYHANDKeys = entityKeys as CCUPAYHANDKeys;

            var CCUPAYLINEFQueryService = new CCUPAYLINEFQueryService(amitalContext);
            entityPM.CCUPAYLINEFPMs = CCUPAYLINEFQueryService.GetMulti(CCUPAYHANDKeys, true);
            entityPM.CCUPAYLINEFPMLastLine = (entityPM.CCUPAYLINEFPMs.Count == 0) ? 0 : entityPM.CCUPAYLINEFPMs.Max(rec => rec.LINENO);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}
