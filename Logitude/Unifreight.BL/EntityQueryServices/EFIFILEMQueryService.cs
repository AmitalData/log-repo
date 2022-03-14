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
    public class EFIFILEMQueryService : EntityQueryService<EFIFILEM, EFIFILEMKeys, EFIFILEMPM, object, EFIFILEMKeys>
    {
        public EFIFILEMQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new EFIFILEMRepository(context);
            mapping = new EFIFILEMDataMapping();
        }

        public EFIFILEMPM GetSingle(int FILENO, bool getFromCache)
        {
            var keys = new EFIFILEMKeys() { FILENO = FILENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(EFIFILEM entityPOCO)
        {
            return new EFIFILEMKeys() { FILENO = entityPOCO.FILENO };
        }
        
    }
}

