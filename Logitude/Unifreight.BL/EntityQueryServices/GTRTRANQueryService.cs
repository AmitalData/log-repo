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
    public class GTRTRANQueryService : EntityQueryService<GTRTRAN, GTRTRANKeys, GTRTRANPM, object, GTRTRANParentKeys>
    {
        public GTRTRANQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTRTRANRepository(context);
            mapping = new GTRTRANDataMapping();
        }

        public List<GTRTRANPM> GetMulti(string PARTNERID, string TABLEID)
        {
            var keys = new GTRTRANParentKeys() { PARTNERID = PARTNERID, TABLEID = TABLEID};
            return base.GetMulti(keys, false);
        }

        public GTRTRANPM GetSingle(string PARTNERID, string TABLEID, string PARTNERCODE, string LOCALCODE, bool getComposition)
        {
            var keys = new GTRTRANKeys() { PARTNERID = PARTNERID, TABLEID = TABLEID, PARTNERCODE = PARTNERCODE, LOCALCODE = LOCALCODE };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTRTRAN entityPOCO)
        {
            return new GTRTRANKeys() { PARTNERID = entityPOCO.PARTNERID, TABLEID = entityPOCO.TABLEID, PARTNERCODE = entityPOCO.PARTNERCODE, LOCALCODE = entityPOCO.LOCALCODE };
        }

        public GTRTRAN GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            return (this.Repository as GTRTRANRepository).GetTranslationP2L(partnerID, tableID, partnerCode);
        }

        public GTRTRAN GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            return (this.Repository as GTRTRANRepository).GetTranslationL2P(partnerID, tableID, localCode);
        }
    }
}
