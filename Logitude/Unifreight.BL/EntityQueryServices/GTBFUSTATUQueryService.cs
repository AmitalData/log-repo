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
    public class GTBFUSTATUQueryService : EntityQueryService<GTBFUSTATU, GTBFUSTATUKeys, GTBFUSTATUPM, object, GTBFUSTATUKeys>
    {
        public GTBFUSTATUQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBFUSTATURepository(context);
            mapping = new GTBFUSTATUDataMapping();
        }

        public GTBFUSTATUPM GetSingle(string ENTNAME, string STATUSCODE, bool getFromCache)
        {
            var keys = new GTBFUSTATUKeys() { ENTNAME = ENTNAME ,STATUSCODE=STATUSCODE};
            return base.GetSingle(keys, false, getFromCache);
        }
        public List<GTBFUSTATU> GetAll()
        {
            return this.Repository.All().ToList();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBFUSTATU entityPOCO)
        {
            return new GTBFUSTATUKeys() { ENTNAME = entityPOCO.ENTNAME,STATUSCODE=entityPOCO.STATUSCODE };
        }
    }
}

