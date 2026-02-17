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
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{

    public class GGGQQueryService : EntityQueryService<GGGQ, GGGQKeys, GGGQPM, object, GGGQKeys>
    {
        //private AmitalContext _AmitalContext;
        //private GGGQRepository _GGGQRepository;
        public GGGQQueryService(AmitalContext context)
        {
            //_AmitalContext = context;
            //_GGGQRepository = new GGGQRepository(context);
            this.MainContext = context;
            this.Repository = new GGGQRepository(context);
            this.mapping = new GGGQDataMapping();
        }

        //public GGGQ GetSingle(string QUEID, bool getComposition, bool getFromCache)
        //{
        //    var curGGGQ = _GGGQRepository.GetSingle(QUEID);
            
        //    return curGGGQ;
        //}
        public GGGQPM GetSingle(string QUEID, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new GGGQKeys() { QUEID = QUEID };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GGGQ entityPOCO)
        {
            return new GGGQKeys() { QUEID = entityPOCO.QUEID };
        }
    }
}
