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
    public class CTBCOUNTRYQueryService : EntityQueryService<CTBCOUNTRY, CTBCOUNTRYKeys, CTBCOUNTRYPM, object, CTBCOUNTRYKeys>
    {
        public CTBCOUNTRYQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBCOUNTRYRepository(context);
            mapping = new CTBCOUNTRYDataMapping();
        }

        public CTBCOUNTRYPM GetSingle(string COUNTRYID, bool getFromCache)
        {
            var keys = new CTBCOUNTRYKeys() { COUNTRYID = COUNTRYID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBCOUNTRY entityPOCO)
        {
            return new CTBCOUNTRYKeys() { COUNTRYID = entityPOCO.COUNTRYID };
        }
    }
}

