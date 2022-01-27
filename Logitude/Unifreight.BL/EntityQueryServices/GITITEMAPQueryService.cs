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
    public class GITITEMAPQueryService : EntityQueryService<GITITEMAP, GITITEMAPKeys, GITITEMAPPM, object, GITITEMAPKeys>
    {
        public GITITEMAPQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GITITEMAPRepository(context);
            mapping = new GITITEMAPDataMapping();
        }

        public GITITEMAPPM GetSingle(decimal COUNTER,string APPROVTYPEID, bool getFromCache)
        {
            var keys = new GITITEMAPKeys() { COUNTER = COUNTER ,APPROVTYPEID=APPROVTYPEID};
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GITITEMAP entityPOCO)
        {
            return new GITITEMAPKeys() { COUNTER = entityPOCO.COUNTER, APPROVTYPEID=entityPOCO.APPROVTYPEID };
        }
    }
}

