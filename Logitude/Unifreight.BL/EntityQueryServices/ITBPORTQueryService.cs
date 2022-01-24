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
using Unifreight.BL.Inteface;
using static Unifreight.BL.BL.QuoteOPPortsHelper;
using Unifreight.BL.BL;

namespace Unifreight.BL.EntityQueryServices
{
    public class ITBPORTQueryService : EntityQueryService<ITBPORT, ITBPORTKeys, ITBPORTPM, object, ITBPORTKeys>, IGetSinglePortFromCacheWithCountry
    {
        private AmitalContext MainContext;

        public ITBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ITBPORTRepository(context);
            mapping = new ITBPORTDataMapping();
        }

        public ITBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new ITBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ITBPORT entityPOCO)
        {
            return new ITBPORTKeys() { PORTID = entityPOCO.PORTID };
        }

        public Ports GetSingleFromCacheWithCountry(string portId)
        {
            Ports res = CacheHelper.GetFromCache("ITBPORT" + portId, () => GetSingleWithCountry(portId));
            return res;
        }

        public Ports GetSingleWithCountry(string portId)
        {
            IQueryable<QPorts> baseQ = (from port in MainContext.ITBPORTs
                                        select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });
            IQueryable<Ports> q = QuoteOPPortsHelper.GetQuery(portId, baseQ, MainContext);
            Ports res = q.ToList().FirstOrDefault();
            return res;
        }
    }
}

