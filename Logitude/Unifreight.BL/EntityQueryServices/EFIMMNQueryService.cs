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
    public class EFIMMNQueryService : EntityQueryService<EFIMMN, EFIMMNKeys, EFIMMNPM, object, EFIMMNKeys>
    {
        public EFIMMNQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new EFIMMNRepository(context);
            mapping = new EFIMMNDataMapping();
        }

        public EFIMMNPM GetSingle(long FILENO,int STORGENO, bool getFromCache)
        {
            var keys = new EFIMMNKeys() { FILENO = FILENO,STORGENO=STORGENO };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(EFIMMN entityPOCO)
        {
            return new EFIMMNKeys() { FILENO = entityPOCO.FILENO };
        }

        public List<EFIMMN> GetByFileNo(int tenant,long fileno)
        {
            EFIMMNRepository repo = new EFIMMNRepository(tenant);
            return repo.GetByFileNo(fileno);
        }
    }
}

