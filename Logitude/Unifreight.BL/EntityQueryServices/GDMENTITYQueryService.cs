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

    public class GDMENTITYQueryService : EntityQueryService<GDMENTITY, GDMENTITYKeys, GDMENTITYPM, object, GDMENTITYKeys>
    {
        public GDMENTITYQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDMENTITYRepository(context);
            mapping = new GDMENTITYDataMapping();
        }

        public GDMENTITYPM GetSingle(string COMID, string PRIMARYID, string PRIMARYNUM, bool getComposition)
        {
            var keys = new GDMENTITYKeys() { COMID = COMID, PRIMARYID = PRIMARYID, PRIMARYNUM = PRIMARYNUM };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDMENTITY entityPOCO)
        {
            return new GDMENTITYKeys() { COMID = entityPOCO.COMID, PRIMARYID = entityPOCO.PRIMARYID, PRIMARYNUM = entityPOCO.PRIMARYNUM };
        }

        public List<GDMENTITY> GetGDMENTITYListByPrimarys(string PRIMARYID, string PRIMARYNUM)
        {
            return (this.Repository as GDMENTITYRepository).GetGDMENTITYListByPrimarys(PRIMARYID, PRIMARYNUM);
        }

    }
}
