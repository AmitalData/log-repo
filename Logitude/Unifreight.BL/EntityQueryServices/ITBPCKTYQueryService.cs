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
    public class ITBPCKTYQueryService : EntityQueryService<ITBPCKTY, ITBPCKTYKeys, ITBPCKTYPM, object, ITBPCKTYKeys>
    {
        public ITBPCKTYQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ITBPCKTYRepository(context);
            mapping = new ITBPCKTYDataMapping();
        }

        public ITBPCKTYPM GetSingle(string PACKTYPEID, bool getFromCache)
        {
            var keys = new ITBPCKTYKeys() { PACKTYPEID = PACKTYPEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        public List<ITBPCKTYPM> All()
        {
            List<ITBPCKTY> entityPOCOs = (this.Repository as ITBPCKTYRepository).All();
            List<ITBPCKTYPM> entityPMs = new List<ITBPCKTYPM>();

            foreach (ITBPCKTY entityPOCO in entityPOCOs)
            {
                ITBPCKTYPM entityPM = new ITBPCKTYPM();
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }

            return entityPMs;
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ITBPCKTY entityPOCO)
        {
            return new ITBPCKTYKeys() { PACKTYPEID = entityPOCO.PACKTYPEID };
        }
    }
}

