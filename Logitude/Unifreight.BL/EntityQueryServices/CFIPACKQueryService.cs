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
using Simplog.Server.Infrastructure;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class CFIPACKQueryService : EntityQueryService<CFIPACK, CFIPACKKeys, CFIPACKPM, object, CFIPACKKeys>
    {
        public CFIPACKQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new CFIPACKRepository(context);
            this.mapping = new CFIPACKDataMapping();
        }

        public CFIPACKPM GetSingle(int FILENO, int LINENO, bool getComposition, bool getFromCache)
        {
            var EntityKeys = new CFIPACKKeys() { FILENO = FILENO, LINENO = LINENO };
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CFIPACK entityPOCO)
        {
            return new CFIPACKKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO };
        }

        public List<CFIPACKPM> GetMulti(int FILENO, AmitalContext context)
        {
            List<CFIPACKPM> entityPMs = new List<CFIPACKPM>();
            List<CFIPACK> entityPOCOs = (this.Repository as CFIPACKRepository).GetMulti(FILENO);
            if (entityPOCOs == null)
            {
                return null;
            }

            foreach (CFIPACK entityPOCO in entityPOCOs)
            {
                CFIPACKPM entityPM = new CFIPACKPM();
                EntityKeyFields entityKeys = GetKeys(entityPOCO);
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }

            return entityPMs;
        }
    }
}
