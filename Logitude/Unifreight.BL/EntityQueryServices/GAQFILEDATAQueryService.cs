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
    public class GAQFILEDATAQueryService : EntityQueryService<GAQFILEDATA, GAQFILEDATAKeys, GAQFILEDATAPM, object, GAQFILEDATAKeys>
    {
        public GAQFILEDATAQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GAQFILEDATARepository(context);
            mapping = new GAQFILEDATADataMapping();
        }
        public List<GAQFILEDATAPM> GetEntity(string ENTNAME, string PRIMARYNUM, string APPQID)
        {
            var listPoco = (this.Repository as GAQFILEDATARepository).GetEntity(ENTNAME, PRIMARYNUM, APPQID);
            var listPM = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return listPM;
        }
        public List<GAQFILEDATAPM> GetFieldMetadata(string ENTNAME, string PRIMARYNUM, string APPQID, string fieldPath)
        {
            var listPoco = (this.Repository as GAQFILEDATARepository).GetAll()
                .Where(a => a.ENTNAME == ENTNAME && a.PRIMARYNUM == PRIMARYNUM && a.APPQID == APPQID && a.PATH == fieldPath).ToList();
            var listPM = listPoco.Select(poco => this.GetEntityPM(poco)).ToList();
            return listPM;
        }

        public GAQFILEDATAPM GetSingle(string ENTNAME, string PRIMARYNUM, string APPQID, string PATH, string FIELDID, bool getComposition)
        {
            var keys = new GAQFILEDATAKeys() { ENTNAME = ENTNAME, PRIMARYNUM = PRIMARYNUM, APPQID = APPQID, PATH = PATH, FIELDID = FIELDID };
            return base.GetSingle(keys, getComposition, false);
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GAQFILEDATA entityPOCO)
        {
            return new GAQFILEDATAKeys()
            {
                ENTNAME = entityPOCO.ENTNAME,
                PRIMARYNUM = entityPOCO.PRIMARYNUM,
                APPQID=entityPOCO.APPQID ,
                PATH = entityPOCO.PATH,
                FIELDID = entityPOCO.FIELDID
            };
        }
        
    }
}
