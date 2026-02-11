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
    public class YCULTASKQueryService : EntityQueryService<YCULTASK, YCULTASKKeys, YCULTASKPM, object, YCULTASKKeys>
    {

        //private AmitalContext _AmitalContext;
        //private YCULTASKRepository _YCULTASKRepository;
        public YCULTASKQueryService(AmitalContext context)
        {
            //_AmitalContext = context;
            //_YCULTASKRepository = new YCULTASKRepository(context);
            this.MainContext = context;
            this.Repository = new YCULTASKRepository(context);
            this.mapping = new YCULTASKDataMapping();
        }

        //public YCULTASKPM GetSingle(string TASKID, bool getComposition)
        //{
        //    YCULTASKPM curYCULTASK = new YCULTASKPM();
        //    return curYCULTASK;
        //}

        public YCULTASKPM GetSingle(string TASKID,int tenant, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new YCULTASKKeys() { TASKID = TASKID, Tenant=tenant };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(YCULTASK entityPOCO)
        {
            return  new YCULTASKKeys() { TASKID = entityPOCO.TASKID, Tenant = entityPOCO.TENANT };
        }
    }
}
