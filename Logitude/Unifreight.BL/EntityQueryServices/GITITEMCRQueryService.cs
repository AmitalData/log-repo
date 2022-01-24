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

namespace Unifreight.BL.EntityQueryServices
{

    public class GITITEMCRQueryService : EntityQueryService<GITITEMCR, GITITEMCRKeys, GITITEMCRPM, GITITEMPM, GITITEMKeys>
    {
        public GITITEMCRQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new GITITEMCRRepository(context);
            this.mapping = new GITITEMCRDataMapping();
        }

        public GITITEMCRPM GetSingle(string COUNTER, string REQCERT, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new GITITEMCRKeys() { COUNTER = COUNTER , REQCERT = REQCERT};
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GITITEMCR entityPOCO)
        {
            return new GITITEMCRKeys() { COUNTER = entityPOCO.COUNTER.ToString(), REQCERT = entityPOCO.REQCERT };
        }

        public List<GITITEMCRPM> GetMulti(string COUNTER, bool getFromCache)
        {
            var keys = new GITITEMKeys() { COUNTER = COUNTER };
            return base.GetMulti(keys, false, getFromCache);
        }
    }
}
