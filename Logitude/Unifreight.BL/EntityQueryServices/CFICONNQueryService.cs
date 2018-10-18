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
    public class CFICONNQueryService : EntityQueryService<CFICONN, CFICONNKeys, CFICONNPM, object, CFICONNKeys>
    {
        public CFICONNQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CFICONNRepository(context);
            mapping = new CFICONNDataMapping();
        }

        public CFICONNPM GetSingle(string FILENO, long CUSTOMFILE, bool getFromCache)
        {
            var keys = new CFICONNKeys() { FILENO = FILENO, CUSTOMFILE = CUSTOMFILE };
            return base.GetSingle(keys, false, getFromCache);
        }

        public List<string> GetImportFilesByCustomFile(long CUSTOMFILE)
        {
            var entityPOCOs = (this.Repository as CFICONNRepository).GetImportFilesByCustomFile(CUSTOMFILE);
            if (entityPOCOs == null)
            {
                return null;
            }
            return entityPOCOs.Select(poco => poco.FILENO).ToList();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFICONN entityPOCO)
        {
            return new CFICONNKeys() { FILENO = entityPOCO.FILENO, CUSTOMFILE = entityPOCO.CUSTOMFILE };
        }
    }
}


