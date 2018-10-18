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
    public class GTBMANDTQueryService : EntityQueryService<GTBMANDT, GTBMANDTKeys, GTBMANDTPM, object, GTBMANDTKeys>
    {
        public GTBMANDTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GTBMANDTRepository(context);
            mapping = new GTBMANDTDataMapping();
        }

        public List<GTBMANDTPM> GetAllMandatory(string CLIENTCODE, string FORMNAME)
        {
            var listPoco = (this.Repository as GTBMANDTRepository).GetAllMandatory(CLIENTCODE, FORMNAME);
            var listPM = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return listPM;
        }
        public List<GTBMANDTPM> GetTabMandatory(string CLIENTCODE, string FORMNAME, string ENTITY)
        {
            var listPoco = (this.Repository as GTBMANDTRepository).GetTabMandatory(CLIENTCODE, FORMNAME, ENTITY);
            var listPM = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return listPM;
        }

        public GTBMANDTPM GetSingle(string CLIENTCODE, string ENTITY, string FORMNAME, string FIELDNAME, bool getComposition)
        {
            var keys = new GTBMANDTKeys() { CLIENTCODE = CLIENTCODE, ENTITY = ENTITY, FORMNAME = FORMNAME, FIELDNAME = FIELDNAME };
            return base.GetSingle(keys, getComposition, false);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GTBMANDT entityPOCO)
        {
            return new GTBMANDTKeys() { CLIENTCODE = entityPOCO.CLIENTCODE, ENTITY = entityPOCO.ENTITY, FORMNAME = entityPOCO.FORMNAME, FIELDNAME = entityPOCO.FIELDNAME };
        }
    }
}

