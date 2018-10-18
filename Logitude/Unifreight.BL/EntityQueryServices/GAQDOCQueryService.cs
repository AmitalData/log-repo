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

    public class GAQDOCQueryService : EntityQueryService<GAQDOC, GAQDOCKeys, GAQDOCPM, object, GAQDOCKeys>
    {
        public GAQDOCQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GAQDOCRepository(context);
            mapping = new GAQDOCDataMapping();
        }

        public GAQDOCPM GetSingle(string APPQID, string FOLDERCODE, string DOCID, bool getComposition)
        {
            var keys = new GAQDOCKeys() { APPQID = APPQID, FOLDERCODE = FOLDERCODE, DOCID = DOCID };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GAQDOC entityPOCO)
        {
            return new GAQDOCKeys() { APPQID = entityPOCO.APPQID, FOLDERCODE = entityPOCO.FOLDERCODE, DOCID = entityPOCO.DOCID };
        }
    }
}
