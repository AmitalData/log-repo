using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class PropertiesDetailsHistoryQueryService
    {
        public List<PropertiesDetailsHistoryPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public PropertiesDetailsHistoryPM GetPropertiesDetailsHistoryByCustomsItemId(string customsItemId)
        {
            PropertiesDetailsHistoryPM pm = null;
            var poco = repository.GetPropertiesDetailsHistoryByCustomsItemIdMostAccurate(customsItemId);


            if (poco != null)
            {


                pm = this.GetEntityPM(poco);
            }
            return pm;
        }
    }
}
