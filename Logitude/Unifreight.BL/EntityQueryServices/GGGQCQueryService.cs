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

    public class GGGQCQueryService : EntityQueryService<GGGQC, GGGQCKeys, GGGQCPM, object, GGGQCKeys>
    {
        //private AmitalContext _AmitalContext;
        //private GGGQCRepository _GGGQCRepository;
        public GGGQCQueryService(AmitalContext context)
        {
            //_AmitalContext = context;
            //_GGGQCRepository = new GGGQCRepository(context);
            this.MainContext = context;
            this.Repository = new GGGQCRepository(context);
            this.mapping = new GGGQCDataMapping();
        }

        //public GGGQC GetSingle(string QUEID, bool getComposition, bool getFromCache)
        //{
        //    var curGGGQC = _GGGQCRepository.GetSingle(QUEID);

        //    return curGGGQC;
        //}
        public GGGQCPM GetSingle(string QUEID, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new GGGQCKeys() { QUEID = QUEID };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GGGQC entityPOCO)
        {
            return new GGGQCKeys() { QUEID = entityPOCO.QUEID };
        }
    }
}
