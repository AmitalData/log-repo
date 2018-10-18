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
    public class CTBCURRENCYQueryService : EntityQueryService<CTBCURRENCY, CTBCURRENCYKeys, CTBCURRENCYPM, object, CTBCURRENCYKeys>
    {
        public CTBCURRENCYQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBCURRENCYRepository(context);
            mapping = new CTBCURRENCYDataMapping();
        }

        public CTBCURRENCYPM GetSingle(string CURRENCYID, bool getFromCache)
        {
            var keys = new CTBCURRENCYKeys() { CURRENCYID = CURRENCYID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBCURRENCY entityPOCO)
        {
            return new CTBCURRENCYKeys() { CURRENCYID = entityPOCO.CURRENCYID };
        }
    }
}

