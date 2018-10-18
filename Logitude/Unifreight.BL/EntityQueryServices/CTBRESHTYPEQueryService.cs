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
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class CTBRESHTYPEQueryService : EntityQueryService<CTBRESHTYPE, CTBRESHTYPEKeys, CTBRESHTYPEPM, object, CTBRESHTYPEKeys>
    {
        public CTBRESHTYPEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBRESHTYPERepository(context);
            mapping = new CTBRESHTYPEDataMapping();
        }

        public CTBRESHTYPEPM GetSingle(string RESHIMONTYPE, bool getFromCache)
        {
            var keys = new CTBRESHTYPEKeys() { RESHIMONTYPE = RESHIMONTYPE};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBRESHTYPE entityPOCO)
        {
            return new CTBRESHTYPEKeys() { RESHIMONTYPE = entityPOCO.RESHIMONTYPE };
        }
    }
}

