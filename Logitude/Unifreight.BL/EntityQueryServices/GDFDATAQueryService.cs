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
    public class GDFDATAQueryService : EntityQueryService<GDFDATA, GDFDATAKeys, GDFDATAPM, object, GDFDATAKeys>
    {
        public GDFDATAQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GDFDATARepository(context);
            mapping = new GDFDATADataMapping();
        }

        public GDFDATAPM GetSingle(string DISTRID, string DEFID, string BRANCHID, string CARDID, bool getComposition, bool getFromCache)
        {
            var keys = new GDFDATAKeys() { DISTRID = DISTRID, DEFID = DEFID, BRANCHID = BRANCHID, CARDID = CARDID };
            return base.GetSingle(keys, getComposition, getFromCache) ?? new GDFDATAPM();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GDFDATA entityPOCO)
        {
            return new GDFDATAKeys() { DISTRID = entityPOCO.DISTRID, DEFID = entityPOCO.DEFID, BRANCHID = entityPOCO.BRANCHID, CARDID = entityPOCO.CARDID };
        }

        public string GetCardIdByDefaultValue(string DISTRID, string DEFID, string BRANCHID, string SHORTDEFDATA)
        {
            string accountNo = "";
            var listPoco = (this.Repository as GDFDATARepository).GetAll()
                .Where(a => a.DISTRID == DISTRID && a.DEFID == DEFID && a.BRANCHID == BRANCHID && a.SHORTDEFDATA == SHORTDEFDATA).ToList();

            if (listPoco != null && listPoco.Count > 0)
            {
                accountNo = listPoco.FirstOrDefault().CARDID;
            }

            return accountNo;
            
            /*string entityKeyString = "GDFDATA," + DISTRID + "," + DEFID + "," + BRANCHID + "," + CARDID;
            var pm = CacheManager.GetOrInsertNewObject<GDFDATAPM>(entityKeyString, () =>
            {
                var pm1 = base.GetSingle(keys, getComposition, false);
            });
            return pm;*/

        }
    }
}
