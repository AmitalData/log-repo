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
using static Unifreight.BL.BL.QuoteOPPortsHelper;
using Unifreight.BL.BL;
using Unifreight.BL.Inteface;

namespace Unifreight.BL.EntityQueryServices
{
    public class RTBPORTQueryService : EntityQueryService<RTBPORT, RTBPORTKeys, RTBPORTPM, object, RTBPORTKeys>, IGetSinglePortFromCacheWithCountry
    {
        private AmitalContext MainContext;

        public RTBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new RTBPORTRepository(context);
            mapping = new RTBPORTDataMapping();
        }

        public RTBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new RTBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(RTBPORT entityPOCO)
        {
            return new RTBPORTKeys() { PORTID = entityPOCO.PORTID };
        }

        public Ports GetSingleFromCacheWithCountry(string portId)
        {
            Ports res = CacheHelper.GetFromCache("RTBPORT" + portId, () => GetSingleWithCountry(portId));
            return res;
        }

        public Ports GetSingleWithCountry(string portId)
        {
            IQueryable<QPorts> baseQ = (from port in MainContext.RTBPORTs
                                        select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });
            IQueryable<Ports> q = QuoteOPPortsHelper.GetQuery(portId, baseQ, MainContext);
            Ports res = q.ToList().FirstOrDefault();
            return res;
        }
    }
}

