using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
//using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;

using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs;

namespace Unifreight.BL.EntityQueryServices
{
    public class GTBITMCNQueryService : EntityQueryService<GTBITMCN, GTBITMCNKeys, GTBITMCNPM, object, GTBITMCNKeys>
    {
        public GTBITMCNQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBITMCNRepository(context);
            mapping = new GTBITMCNDataMapping();
        }

        public GTBITMCNPM GetSingle(string PARTNERID, string ITEMID, string PARTNER2ID, string ITEM2ID, bool getFromCache)
        {
            var keys = new GTBITMCNKeys() { PARTNERID = PARTNERID, ITEMID = ITEMID, PARTNER2ID = PARTNER2ID, ITEM2ID = ITEM2ID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBITMCN entityPOCO)
        {
            return new GTBITMCNKeys() { PARTNERID = entityPOCO.PARTNERID, ITEMID = entityPOCO.ITEMID, PARTNER2ID = entityPOCO.PARTNER2ID, ITEM2ID = entityPOCO.ITEM2ID };
        }

        //public List<GTBITMCNPM> GetListBy(string vendorId, string customerId, string search, int top)
        //{

        //    var pocoS= (Repository as GTBITMCNRepository).GetListBy(vendorId, customerId, search, top);
        //    return pocoS.Select(poco => this.GetEntityPM(poco)).ToList();
        //}



    }
}


