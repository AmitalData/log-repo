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
using Simplog.Server.Infrastructure.DataContracts;
using Unifreight.BL.Inteface;
using Simplog.Server.Infrastructure.Helpers;
using static Unifreight.BL.BL.QuoteOPPortsHelper;
using Unifreight.BL.BL;

namespace Unifreight.BL.EntityQueryServices
{
    public class ETBPORTQueryService : EntityQueryService<ETBPORT, ETBPORTKeys, ETBPORTPM, object, ETBPORTKeys>, IGetSinglePortFromCacheWithCountry
    {
        private AmitalContext MainContext;

        public ETBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBPORTRepository(context);
            mapping = new ETBPORTDataMapping();
        }

        public ETBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new ETBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        public Ports GetSingleFromCacheWithCountry(string portId)
        {
            Ports res = CacheHelper.GetFromCache("ETBPORT" + portId, ()=> GetSingleWithCountry(portId));
            return res;
        }

        public Ports GetSingleWithCountry(string portId)
        {
            IQueryable<QPorts> baseQ = (from port in MainContext.ETBPORTs
                                        select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });
            IQueryable<Ports> q = QuoteOPPortsHelper.GetQuery(portId, baseQ, MainContext);
            Ports res = q.ToList().FirstOrDefault();
            return res;
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBPORT entityPOCO)
        {
            return new ETBPORTKeys() { PORTID = entityPOCO.PORTID };
        }
    }
}

