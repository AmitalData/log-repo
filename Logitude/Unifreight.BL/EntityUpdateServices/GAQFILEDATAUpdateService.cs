using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GAQFILEDATAUpdateService : EntityUpdateService<GAQFILEDATA, GAQFILEDATAPM, EntityPM>
    {
        public GAQFILEDATAUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GAQFILEDATARepository(context);

            Mapping = new GAQFILEDATADataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        public int FastDeleteEntityLastValue(string ENTNAME, string PRIMARYNUM, string APPQID)
        {
            string FIELDID = "LastValue";
            var rowAffected = (this.Repository as GAQFILEDATARepository).FastDeleteMultiEntityField(ENTNAME, PRIMARYNUM, APPQID, FIELDID);
            return rowAffected;
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GAQFILEDATAPM entityPM)
        {
            return new GAQFILEDATAKeys() { ENTNAME = entityPM.ENTNAME, PRIMARYNUM = entityPM.PRIMARYNUM, APPQID=entityPM.APPQID, PATH = entityPM.PATH, FIELDID = entityPM.FIELDID };
        }

        protected override void OnCreating(GAQFILEDATAPM entityPM, EntityPM entityParentPM)
        {
        }
    }
}
