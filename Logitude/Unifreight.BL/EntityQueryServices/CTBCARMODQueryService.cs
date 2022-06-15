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
    public class CTBCARMODQueryService : EntityQueryService<CTBCARMOD, CTBCARMODKeys, CTBCARMODPM, object, CTBCARMODKeys>
    {
        public CTBCARMODQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CTBCARMODRepository(context);
            mapping = new CTBCARMODDataMapping();
        }

        public CTBCARMODPM GetSingle(string CUSTOMERID, bool getFromCache)
        {
            var keys = new CTBCARMODKeys() { CUSTOMERID = CUSTOMERID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CTBCARMOD entityPOCO)
        {
            return new CTBCARMODKeys() { CUSTOMERID = entityPOCO.CUSTOMERID };
        }
    }
}

