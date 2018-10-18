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
    public class GTBITEMQueryService : EntityQueryService<GTBITEM, GTBITEMKeys, GTBITEMPM, object, GTBITEMKeys>
    {
        public GTBITEMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBITEMRepository(context);
            mapping = new GTBITEMDataMapping();
        }

        public GTBITEMPM GetSingle(string PARTNERID, string ITEMID , bool getFromCache)
        {
            var keys = new GTBITEMKeys() { PARTNERID = PARTNERID, ITEMID = ITEMID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBITEM entityPOCO)
        {
            return new GTBITEMKeys() { PARTNERID = entityPOCO.PARTNERID, ITEMID = entityPOCO.ITEMID };
        }

        //public List<GTBITEMPM> GetListBy(string vendorId, string customerId, string search, int top)
        //{
           
        //    var pocoS= (Repository as GTBITEMRepository).GetListBy(vendorId, customerId, search, top);
        //    return pocoS.Select(poco => this.GetEntityPM(poco)).ToList();
        //}



    }
}


